using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200004B RID: 75
	internal class TimelineRecording
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00016414 File Offset: 0x00014814
		internal static UndoPropertyModification[] ProcessUndoModification(UndoPropertyModification[] modifications, TimelineWindow.TimelineState state)
		{
			UndoPropertyModification[] result;
			if (TimelineRecording.HasAnyPlayableAssetModifications(modifications))
			{
				result = TimelineRecording.ProcessPlayableAssetModification(modifications, state);
			}
			else
			{
				result = TimelineRecording.ProcessMonoBehaviourModification(modifications, state);
			}
			return result;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00016448 File Offset: 0x00014848
		private static TrackAsset GetTrackForGameObject(GameObject gameObject, TimelineWindow.TimelineState state)
		{
			TrackAsset result;
			if (gameObject == null)
			{
				result = null;
			}
			else
			{
				PlayableDirector currentDirector = state.currentDirector;
				if (currentDirector == null)
				{
					result = null;
				}
				else
				{
					int num = int.MaxValue;
					TrackAsset trackAsset = null;
					TrackAsset[] outputTracks = state.timeline.outputTracks;
					for (int i = 0; i < outputTracks.Length; i++)
					{
						if (outputTracks[i].GetType() == typeof(AnimationTrack))
						{
							if (state.IsArmedForRecord(outputTracks[i]))
							{
								GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(currentDirector, outputTracks[i]);
								if (sceneGameObject != null)
								{
									int childLevel = TimelineRecording.GetChildLevel(sceneGameObject, gameObject);
									if (childLevel != -1 && childLevel < num)
									{
										trackAsset = outputTracks[i];
										num = childLevel;
									}
								}
							}
						}
					}
					if (trackAsset && !state.IsArmedForRecord(trackAsset))
					{
						trackAsset = null;
					}
					result = trackAsset;
				}
			}
			return result;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0001654C File Offset: 0x0001494C
		public static TrackAsset GetRecordingTrack(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			SerializedObject serializedObject = property.serializedObject;
			Component component = serializedObject.targetObject as Component;
			TrackAsset result;
			if (component == null)
			{
				result = null;
			}
			else
			{
				GameObject gameObject = component.gameObject;
				result = TimelineRecording.GetTrackForGameObject(gameObject, state);
			}
			return result;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00016598 File Offset: 0x00014998
		private static void GatherModifications(SerializedProperty property, List<PropertyModification> modifications)
		{
			if (property.hasChildren)
			{
				SerializedProperty serializedProperty = property.Copy();
				SerializedProperty endProperty = property.GetEndProperty(false);
				while (serializedProperty.Next(true) && !SerializedProperty.EqualContents(serializedProperty, endProperty))
				{
					TimelineRecording.GatherModifications(serializedProperty, modifications);
				}
			}
			bool flag = property.propertyType == 5;
			bool flag2 = property.propertyType == 2 || property.propertyType == 1 || property.propertyType == 0;
			if (flag || flag2)
			{
				SerializedObject serializedObject = property.serializedObject;
				PropertyModification propertyModification = new PropertyModification();
				propertyModification.target = serializedObject.targetObject;
				propertyModification.propertyPath = property.propertyPath;
				if (flag)
				{
					propertyModification.value = string.Empty;
					propertyModification.objectReference = property.objectReferenceValue;
				}
				else
				{
					propertyModification.value = TimelineUtility.PropertyToString(property);
				}
				if (serializedObject.targetObject is Component)
				{
					GameObject gameObject = ((Component)serializedObject.targetObject).gameObject;
					EditorCurveBinding editorCurveBinding;
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(propertyModification, gameObject, ref editorCurveBinding) != null)
					{
						modifications.Add(propertyModification);
					}
				}
				else
				{
					modifications.Add(propertyModification);
				}
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x000166D4 File Offset: 0x00014AD4
		public static bool CanRecord(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			bool result;
			if (TimelineRecording.IsPlayableAssetProperty(property))
			{
				result = AnimatedParameterExtensions.IsAnimatable(property.propertyType);
			}
			else if (TimelineRecording.GetRecordingTrack(property, state) == null)
			{
				result = false;
			}
			else
			{
				TimelineRecording.s_TempPropertyModifications.Clear();
				TimelineRecording.GatherModifications(property, TimelineRecording.s_TempPropertyModifications);
				result = TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>();
			}
			return result;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00016740 File Offset: 0x00014B40
		public static void AddKey(SerializedProperty prop, TimelineWindow.TimelineState state)
		{
			TimelineRecording.s_TempPropertyModifications.Clear();
			TimelineRecording.GatherModifications(prop, TimelineRecording.s_TempPropertyModifications);
			if (TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>())
			{
				IEnumerable<PropertyModification> source = TimelineRecording.s_TempPropertyModifications;
				if (TimelineRecording.<>f__mg$cache0 == null)
				{
					TimelineRecording.<>f__mg$cache0 = new Func<PropertyModification, UndoPropertyModification>(TimelineRecording.PropertyModificationToUndoPropertyModification);
				}
				UndoPropertyModification[] modifications = source.Select(TimelineRecording.<>f__mg$cache0).ToArray<UndoPropertyModification>();
				TimelineRecording.ProcessUndoModification(modifications, state);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000167AC File Offset: 0x00014BAC
		private static UndoPropertyModification PropertyModificationToUndoPropertyModification(PropertyModification prop)
		{
			UndoPropertyModification result = default(UndoPropertyModification);
			result.previousValue = prop;
			result.currentValue = new PropertyModification
			{
				objectReference = prop.objectReference,
				propertyPath = prop.propertyPath,
				target = prop.target,
				value = prop.value
			};
			result.keepPrefabOverride = true;
			return result;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00016818 File Offset: 0x00014C18
		private static AnimationClip GetRecordingClip(TrackAsset asset, TimelineWindow.TimelineState state, out double startTime, out double timeScale)
		{
			startTime = 0.0;
			timeScale = 1.0;
			TimelineClip timelineClip = asset.FindRecordingClipAtTime(state.time);
			AnimationClip result = asset.FindRecordingAnimationClipAtTime(state.time);
			if (timelineClip != null)
			{
				startTime = timelineClip.start;
				timeScale = timelineClip.timeScale;
			}
			return result;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00016878 File Offset: 0x00014C78
		private static bool GetClipAndRelativeTime(SerializedProperty property, TimelineWindow.TimelineState state, out AnimationClip outClip, out double keyTime, out bool keyInRange)
		{
			outClip = null;
			keyTime = 0.0;
			keyInRange = false;
			double num = 0.0;
			double num2 = 1.0;
			AnimationClip animationClip = null;
			if (TimelineRecording.IsPlayableAssetProperty(property))
			{
				IPlayableAsset target = (IPlayableAsset)property.serializedObject.targetObject;
				TimelineClip timelineClip = TimelineRecording.FindClipWithAsset(state.timeline, target, state.currentDirector);
				if (timelineClip != null && state.IsArmedForRecord(timelineClip.parentTrack))
				{
					AnimatedParameterExtensions.CreateCurvesIfRequired(timelineClip, null);
					animationClip = timelineClip.curves;
					num = timelineClip.start;
					num2 = timelineClip.timeScale;
				}
			}
			else
			{
				TrackAsset recordingTrack = TimelineRecording.GetRecordingTrack(property, state);
				if (recordingTrack != null)
				{
					animationClip = TimelineRecording.GetRecordingClip(recordingTrack, state, out num, out num2);
				}
			}
			bool result;
			if (animationClip == null)
			{
				result = false;
			}
			else
			{
				keyTime = (state.time - num) * num2;
				outClip = animationClip;
				keyInRange = (keyTime >= 0.0 && keyTime <= (double)animationClip.length * num2);
				result = true;
			}
			return result;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001699C File Offset: 0x00014D9C
		public static bool HasKey(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			AnimationClip animationClip = null;
			double num = 0.0;
			bool flag = false;
			bool result;
			if (!TimelineRecording.GetClipAndRelativeTime(property, state, out animationClip, out num, out flag) || !flag)
			{
				result = false;
			}
			else
			{
				TimelineRecording.s_TempPropertyModifications.Clear();
				TimelineRecording.GatherModifications(property, TimelineRecording.s_TempPropertyModifications);
				if (!TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>())
				{
					result = false;
				}
				else
				{
					AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(animationClip);
					foreach (PropertyModification modification in TimelineRecording.s_TempPropertyModifications)
					{
						EditorCurveBinding binding;
						if (TimelineRecording.HasBinding(property, modification, animationClip, out binding))
						{
							if (binding.isPPtrCurve)
							{
								ObjectReferenceKeyframe[] objectCurveForBinding = curveInfo.GetObjectCurveForBinding(binding);
								if (objectCurveForBinding != null)
								{
									int keyframeAtTime = CurveEditUtility.GetKeyframeAtTime(objectCurveForBinding, (float)num, animationClip.frameRate);
									if (keyframeAtTime != -1)
									{
										return true;
									}
								}
							}
							else
							{
								AnimationCurve curveForBinding = curveInfo.GetCurveForBinding(binding);
								if (curveForBinding != null)
								{
									int keyframeAtTime2 = CurveEditUtility.GetKeyframeAtTime(curveForBinding, (float)num, animationClip.frameRate);
									if (keyframeAtTime2 != -1)
									{
										return true;
									}
								}
							}
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00016AF4 File Offset: 0x00014EF4
		private static bool HasBinding(SerializedProperty property, PropertyModification modification, AnimationClip clip, out EditorCurveBinding binding)
		{
			Component component = property.serializedObject.targetObject as Component;
			bool result;
			if (component != null)
			{
				Type type = AnimationUtility.PropertyModificationToEditorCurveBinding(modification, component.gameObject, ref binding);
				binding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(binding, clip);
				result = (type != null);
			}
			else if (TimelineRecording.IsPlayableAssetProperty(property))
			{
				binding = EditorCurveBinding.FloatCurve(string.Empty, property.serializedObject.targetObject.GetType(), modification.propertyPath);
				result = true;
			}
			else
			{
				binding = default(EditorCurveBinding);
				result = false;
			}
			return result;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00016B9C File Offset: 0x00014F9C
		public static void RemoveKey(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			AnimationClip animationClip = null;
			double time = 0.0;
			bool flag = false;
			if (TimelineRecording.GetClipAndRelativeTime(property, state, out animationClip, out time, out flag) && flag)
			{
				TimelineRecording.s_TempPropertyModifications.Clear();
				TimelineRecording.GatherModifications(property, TimelineRecording.s_TempPropertyModifications);
				if (TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>())
				{
					TimelineHelpers.PushUndo(animationClip, "remove.key");
					foreach (PropertyModification modification in TimelineRecording.s_TempPropertyModifications)
					{
						EditorCurveBinding sourceBinding;
						if (TimelineRecording.HasBinding(property, modification, animationClip, out sourceBinding))
						{
							CurveEditUtility.RemoveKey(animationClip, sourceBinding, property, time);
						}
					}
				}
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00016C70 File Offset: 0x00015070
		public static bool HasCurve(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			AnimationClip clip = null;
			double num = 0.0;
			bool flag = false;
			bool result;
			if (!TimelineRecording.GetClipAndRelativeTime(property, state, out clip, out num, out flag))
			{
				result = false;
			}
			else
			{
				TimelineRecording.s_TempPropertyModifications.Clear();
				TimelineRecording.GatherModifications(property, TimelineRecording.s_TempPropertyModifications);
				if (!TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>())
				{
					result = false;
				}
				else
				{
					AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
					foreach (PropertyModification modification in TimelineRecording.s_TempPropertyModifications)
					{
						EditorCurveBinding binding;
						if (TimelineRecording.HasBinding(property, modification, clip, out binding))
						{
							if (binding.isPPtrCurve && curveInfo.GetObjectCurveForBinding(binding) != null)
							{
								return true;
							}
							if (!binding.isPPtrCurve && curveInfo.GetCurveForBinding(binding) != null)
							{
								return true;
							}
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00016D88 File Offset: 0x00015188
		public static void RemoveCurve(SerializedProperty property, TimelineWindow.TimelineState state)
		{
			AnimationClip animationClip = null;
			double num = 0.0;
			bool flag = false;
			if (TimelineRecording.GetClipAndRelativeTime(property, state, out animationClip, out num, out flag))
			{
				TimelineRecording.s_TempPropertyModifications.Clear();
				TimelineRecording.GatherModifications(property, TimelineRecording.s_TempPropertyModifications);
				if (TimelineRecording.s_TempPropertyModifications.Any<PropertyModification>())
				{
					TimelineHelpers.PushUndo(animationClip, "remove.curve");
					foreach (PropertyModification modification in TimelineRecording.s_TempPropertyModifications)
					{
						EditorCurveBinding editorCurveBinding;
						if (TimelineRecording.HasBinding(property, modification, animationClip, out editorCurveBinding))
						{
							if (editorCurveBinding.isPPtrCurve)
							{
								AnimationUtility.SetObjectReferenceCurve(animationClip, editorCurveBinding, null);
							}
							else
							{
								AnimationUtility.SetEditorCurve(animationClip, editorCurveBinding, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00016E70 File Offset: 0x00015270
		internal static UndoPropertyModification[] ProcessMonoBehaviourModification(UndoPropertyModification[] modifications, TimelineWindow.TimelineState state)
		{
			UndoPropertyModification[] result;
			if (state == null || state.currentDirector == null)
			{
				result = modifications;
			}
			else
			{
				TimelineRecording.s_UnprocessedMods.Clear();
				TimelineRecording.s_TrackRecorder.PrepareForRecord(state);
				TimelineRecording.s_ModsToProcess.Clear();
				TimelineRecording.s_ModsToProcess.AddRange(modifications.Reverse<UndoPropertyModification>());
				while (TimelineRecording.s_ModsToProcess.Count > 0)
				{
					UndoPropertyModification undoPropertyModification = TimelineRecording.s_ModsToProcess[TimelineRecording.s_ModsToProcess.Count - 1];
					TimelineRecording.s_ModsToProcess.RemoveAt(TimelineRecording.s_ModsToProcess.Count - 1);
					GameObject gameObjectFromModification = TimelineRecording.GetGameObjectFromModification(undoPropertyModification);
					TrackAsset trackForGameObject = TimelineRecording.GetTrackForGameObject(gameObjectFromModification, state);
					if (trackForGameObject != null)
					{
						double num = 0.0;
						AnimationClip animationClip = TimelineRecording.s_TrackRecorder.PrepareTrack(trackForGameObject, state, gameObjectFromModification, out num);
						TimelineRecording.s_RecordState.activeAnimationClip = animationClip;
						TimelineRecording.s_RecordState.activeRootGameObject = state.GetSceneReference(trackForGameObject);
						TimelineRecording.s_RecordState.activeGameObject = gameObjectFromModification;
						TimelineRecording.s_RecordState.currentFrame = Mathf.RoundToInt((float)((double)animationClip.frameRate * (state.time - num)));
						EditorUtility.SetDirty(animationClip);
						UndoPropertyModification[] array = TimelineRecording.GatherRelatedModifications(undoPropertyModification, TimelineRecording.s_ModsToProcess);
						Animator component = TimelineRecording.s_RecordState.activeRootGameObject.GetComponent<Animator>();
						AnimationTrack track = trackForGameObject as AnimationTrack;
						TimelineRecording.AddTrackOffset(track, array, animationClip, component);
						TimelineRecording.AddClipOffset(track, array, TimelineRecording.s_TrackRecorder.recordClip, component);
						bool flag = component != null && undoPropertyModification.currentValue.target == TimelineRecording.s_RecordState.activeRootGameObject.transform && TimelineRecording.HasOffsets(track, TimelineRecording.s_TrackRecorder.recordClip);
						if (flag)
						{
							array = TimelineRecording.HandleEulerModifications(track, TimelineRecording.s_TrackRecorder.recordClip, animationClip, (float)TimelineRecording.s_RecordState.currentFrame * animationClip.frameRate, array);
							TimelineRecording.RemoveOffsets(undoPropertyModification, track, TimelineRecording.s_TrackRecorder.recordClip, array);
						}
						UndoPropertyModification[] array2 = AnimationRecording.Process(TimelineRecording.s_RecordState, array);
						if (array2 != null && array2.Length != 0)
						{
							TimelineRecording.s_UnprocessedMods.AddRange(array2);
						}
						if (flag)
						{
							TimelineRecording.ReapplyOffsets(undoPropertyModification, track, TimelineRecording.s_TrackRecorder.recordClip, array);
						}
						TimelineRecording.s_TrackRecorder.FinializeTrack(trackForGameObject, state);
					}
					else
					{
						TimelineRecording.s_UnprocessedMods.Add(undoPropertyModification);
					}
				}
				TimelineRecording.s_TrackRecorder.FinalizeRecording(state);
				result = TimelineRecording.s_UnprocessedMods.ToArray();
			}
			return result;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000170F4 File Offset: 0x000154F4
		internal static bool IsPositionOrRotation(UndoPropertyModification modification)
		{
			return modification.currentValue.propertyPath.StartsWith("m_LocalPosition") || modification.currentValue.propertyPath.StartsWith("m_LocalRotation") || modification.currentValue.propertyPath.StartsWith("m_LocalEulerAnglesHint");
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00017158 File Offset: 0x00015558
		internal static bool IsRootModification(UndoPropertyModification modification)
		{
			return !modification.currentValue.propertyPath.Contains('/') && !modification.currentValue.propertyPath.Contains('\\');
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000171A0 File Offset: 0x000155A0
		internal static bool ClipHasPositionOrRotation(AnimationClip clip)
		{
			bool result;
			if (clip == null || clip.empty)
			{
				result = false;
			}
			else
			{
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				for (int i = 0; i < curveInfo.bindings.Length; i++)
				{
					if (curveInfo.bindings[i].type == typeof(Transform) && (curveInfo.bindings[i].propertyName.StartsWith("m_LocalPosition") || curveInfo.bindings[i].propertyName.StartsWith("m_LocalRotation") || curveInfo.bindings[i].propertyName.StartsWith("localEuler")))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00017284 File Offset: 0x00015684
		internal static TimelineAnimationUtilities.RigidTransform ComputeInitialClipOffsets(AnimationTrack track, UndoPropertyModification[] mods, Animator animator)
		{
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			if (mods[0].previousValue.target == animator.transform)
			{
				TimelineRecording.GetPreviousPositionAndRotation(mods, ref pos, ref rot);
			}
			else
			{
				pos = animator.transform.localPosition;
				rot = animator.transform.localRotation;
			}
			TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineAnimationUtilities.RigidTransform.Compose(pos, rot);
			TimelineAnimationUtilities.RigidTransform a = (!track.applyOffsets) ? TimelineAnimationUtilities.RigidTransform.identity : TimelineAnimationUtilities.RigidTransform.Compose(track.position, track.rotation);
			rigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(TimelineAnimationUtilities.RigidTransform.Inverse(a), rigidTransform);
			if (mods[0].previousValue.target == animator.transform)
			{
				TimelineRecording.SetPreviousPositionAndRotation(mods, a.position, a.rotation);
			}
			return rigidTransform;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00017368 File Offset: 0x00015768
		internal static void AddTrackOffset(AnimationTrack track, UndoPropertyModification[] mods, AnimationClip clip, Animator animator)
		{
			bool flag;
			if (!track.inClipMode && !TimelineRecording.ClipHasPositionOrRotation(clip))
			{
				if (mods.Any((UndoPropertyModification x) => TimelineRecording.IsPositionOrRotation(x) && TimelineRecording.IsRootModification(x)))
				{
					flag = (animator != null);
					goto IL_49;
				}
			}
			flag = false;
			IL_49:
			bool flag2 = flag;
			if (flag2)
			{
				TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineRecording.ComputeInitialClipOffsets(track, mods, animator);
				track.openClipOffsetPosition = rigidTransform.position;
				track.openClipOffsetRotation = rigidTransform.rotation;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000173EC File Offset: 0x000157EC
		internal static void AddClipOffset(AnimationTrack track, UndoPropertyModification[] mods, TimelineClip clip, Animator animator)
		{
			if (clip != null && !(clip.asset == null))
			{
				AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
				bool flag;
				if (track.inClipMode && animationPlayableAsset != null && !TimelineRecording.ClipHasPositionOrRotation(animationPlayableAsset.clip))
				{
					if (mods.Any((UndoPropertyModification x) => TimelineRecording.IsPositionOrRotation(x) && TimelineRecording.IsRootModification(x)))
					{
						flag = (animator != null);
						goto IL_82;
					}
				}
				flag = false;
				IL_82:
				bool flag2 = flag;
				if (flag2)
				{
					TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineRecording.ComputeInitialClipOffsets(track, mods, animator);
					animationPlayableAsset.position = rigidTransform.position;
					animationPlayableAsset.rotation = rigidTransform.rotation;
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000174A8 File Offset: 0x000158A8
		internal static TimelineAnimationUtilities.RigidTransform GetLocalToTrack(AnimationTrack track, TimelineClip clip)
		{
			TimelineAnimationUtilities.RigidTransform result;
			if (track == null)
			{
				result = TimelineAnimationUtilities.RigidTransform.Compose(Vector3.zero, Quaternion.identity);
			}
			else
			{
				AnimationPlayableAsset animationPlayableAsset = (clip != null) ? (clip.asset as AnimationPlayableAsset) : null;
				TimelineAnimationUtilities.RigidTransform a = (!track.applyOffsets) ? TimelineAnimationUtilities.RigidTransform.identity : TimelineAnimationUtilities.RigidTransform.Compose(track.position, track.rotation);
				TimelineAnimationUtilities.RigidTransform b = TimelineAnimationUtilities.RigidTransform.Compose(Vector3.zero, Quaternion.identity);
				if (animationPlayableAsset != null)
				{
					b = TimelineAnimationUtilities.RigidTransform.Compose(animationPlayableAsset.position, animationPlayableAsset.rotation);
				}
				else
				{
					b = TimelineAnimationUtilities.RigidTransform.Compose(track.openClipOffsetPosition, track.openClipOffsetRotation);
				}
				result = TimelineAnimationUtilities.RigidTransform.Mul(a, b);
			}
			return result;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00017570 File Offset: 0x00015970
		internal static bool HasOffsets(AnimationTrack track, TimelineClip clip)
		{
			bool result;
			if (track == null)
			{
				result = false;
			}
			else
			{
				bool flag = track.applyOffsets && (track.position != Vector3.zero || track.rotation != Quaternion.identity);
				AnimationPlayableAsset animationPlayableAsset = (clip != null) ? (clip.asset as AnimationPlayableAsset) : null;
				if (animationPlayableAsset)
				{
					flag |= (animationPlayableAsset.position != Vector3.zero || animationPlayableAsset.rotation != Quaternion.identity);
				}
				else
				{
					flag |= (track.openClipOffsetPosition != Vector3.zero || track.openClipOffsetRotation != Quaternion.identity);
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00017650 File Offset: 0x00015A50
		internal static void RemoveOffsets(UndoPropertyModification modification, AnimationTrack track, TimelineClip clip, UndoPropertyModification[] mods)
		{
			if (TimelineRecording.IsPositionOrRotation(modification))
			{
				GameObject gameObjectFromModification = TimelineRecording.GetGameObjectFromModification(modification);
				TimelineAnimationUtilities.RigidTransform b = TimelineAnimationUtilities.RigidTransform.Compose(gameObjectFromModification.transform.localPosition, gameObjectFromModification.transform.localRotation);
				TimelineAnimationUtilities.RigidTransform localToTrack = TimelineRecording.GetLocalToTrack(track, clip);
				TimelineAnimationUtilities.RigidTransform a = TimelineAnimationUtilities.RigidTransform.Inverse(localToTrack);
				TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(a, b);
				Vector3 localPosition = gameObjectFromModification.transform.localPosition;
				Quaternion localRotation = gameObjectFromModification.transform.localRotation;
				TimelineRecording.GetPreviousPositionAndRotation(mods, ref localPosition, ref localRotation);
				TimelineAnimationUtilities.RigidTransform rigidTransform2 = TimelineAnimationUtilities.RigidTransform.Mul(a, TimelineAnimationUtilities.RigidTransform.Compose(localPosition, localRotation));
				TimelineRecording.SetPreviousPositionAndRotation(mods, rigidTransform2.position, rigidTransform2.rotation);
				Vector3 localPosition2 = gameObjectFromModification.transform.localPosition;
				Quaternion localRotation2 = gameObjectFromModification.transform.localRotation;
				TimelineRecording.GetCurrentPositionAndRotation(mods, ref localPosition2, ref localRotation2);
				TimelineAnimationUtilities.RigidTransform rigidTransform3 = TimelineAnimationUtilities.RigidTransform.Mul(a, TimelineAnimationUtilities.RigidTransform.Compose(localPosition2, localRotation2));
				TimelineRecording.SetCurrentPositionAndRotation(mods, rigidTransform3.position, rigidTransform3.rotation);
				gameObjectFromModification.transform.localPosition = rigidTransform.position;
				gameObjectFromModification.transform.localRotation = rigidTransform.rotation;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0001775C File Offset: 0x00015B5C
		internal static void ReapplyOffsets(UndoPropertyModification modification, AnimationTrack track, TimelineClip clip, UndoPropertyModification[] mods)
		{
			if (TimelineRecording.IsPositionOrRotation(modification))
			{
				GameObject gameObjectFromModification = TimelineRecording.GetGameObjectFromModification(modification);
				TimelineAnimationUtilities.RigidTransform b = TimelineAnimationUtilities.RigidTransform.Compose(gameObjectFromModification.transform.localPosition, gameObjectFromModification.transform.localRotation);
				TimelineAnimationUtilities.RigidTransform localToTrack = TimelineRecording.GetLocalToTrack(track, clip);
				TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, b);
				Vector3 localPosition = gameObjectFromModification.transform.localPosition;
				Quaternion localRotation = gameObjectFromModification.transform.localRotation;
				TimelineRecording.GetPreviousPositionAndRotation(mods, ref localPosition, ref localRotation);
				TimelineAnimationUtilities.RigidTransform rigidTransform2 = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, TimelineAnimationUtilities.RigidTransform.Compose(localPosition, localRotation));
				TimelineRecording.SetPreviousPositionAndRotation(mods, rigidTransform2.position, rigidTransform2.rotation);
				Vector3 localPosition2 = gameObjectFromModification.transform.localPosition;
				Quaternion localRotation2 = gameObjectFromModification.transform.localRotation;
				TimelineRecording.GetCurrentPositionAndRotation(mods, ref localPosition2, ref localRotation2);
				TimelineAnimationUtilities.RigidTransform rigidTransform3 = TimelineAnimationUtilities.RigidTransform.Mul(localToTrack, TimelineAnimationUtilities.RigidTransform.Compose(localPosition2, localRotation2));
				TimelineRecording.SetCurrentPositionAndRotation(mods, rigidTransform3.position, rigidTransform3.rotation);
				gameObjectFromModification.transform.localPosition = rigidTransform.position;
				gameObjectFromModification.transform.localRotation = rigidTransform.rotation;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00017860 File Offset: 0x00015C60
		private static UndoPropertyModification[] GatherRelatedModifications(UndoPropertyModification toMatch, List<UndoPropertyModification> list)
		{
			List<UndoPropertyModification> list2 = new List<UndoPropertyModification>
			{
				toMatch
			};
			for (int i = list.Count - 1; i >= 0; i--)
			{
				UndoPropertyModification item = list[i];
				if (item.previousValue.target == toMatch.previousValue.target && TimelineRecording.DoesPropertyPathMatch(item.previousValue.propertyPath, toMatch.previousValue.propertyPath))
				{
					list2.Add(item);
					list.RemoveAt(i);
				}
			}
			return list2.ToArray();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00017904 File Offset: 0x00015D04
		private static GameObject GetGameObjectFromModification(UndoPropertyModification mod)
		{
			GameObject result = null;
			if (mod.previousValue.target is GameObject)
			{
				result = (mod.previousValue.target as GameObject);
			}
			else if (mod.previousValue.target is Component)
			{
				result = (mod.previousValue.target as Component).gameObject;
			}
			return result;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00017978 File Offset: 0x00015D78
		private static int GetChildLevel(GameObject parent, GameObject child)
		{
			int num = 0;
			while (child != null)
			{
				if (parent == child)
				{
					break;
				}
				if (child.transform.parent == null)
				{
					return -1;
				}
				child = child.transform.parent.gameObject;
				num++;
			}
			if (child != null)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000179FC File Offset: 0x00015DFC
		private static bool DoesPropertyPathMatch(string a, string b)
		{
			return AnimationWindowUtility.GetPropertyGroupName(a).Equals(AnimationWindowUtility.GetPropertyGroupName(a));
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00017A24 File Offset: 0x00015E24
		internal static void GetPreviousPositionAndRotation(UndoPropertyModification[] mods, ref Vector3 position, ref Quaternion rotation)
		{
			Transform transform = mods[0].previousValue.target as Transform;
			if (transform == null)
			{
				transform = (Transform)mods[0].currentValue.target;
			}
			position = transform.localPosition;
			rotation = transform.localRotation;
			foreach (UndoPropertyModification undoPropertyModification in mods)
			{
				string propertyPath = undoPropertyModification.previousValue.propertyPath;
				switch (propertyPath)
				{
				case "m_LocalPosition.x":
					position.x = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalPosition.y":
					position.y = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalPosition.z":
					position.z = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalRotation.x":
					rotation.x = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalRotation.y":
					rotation.y = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalRotation.z":
					rotation.z = float.Parse(undoPropertyModification.previousValue.value);
					break;
				case "m_LocalRotation.w":
					rotation.w = float.Parse(undoPropertyModification.previousValue.value);
					break;
				}
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00017C34 File Offset: 0x00016034
		internal static void GetCurrentPositionAndRotation(UndoPropertyModification[] mods, ref Vector3 position, ref Quaternion rotation)
		{
			Transform transform = (Transform)mods[0].currentValue.target;
			position = transform.localPosition;
			rotation = transform.localRotation;
			foreach (UndoPropertyModification undoPropertyModification in mods)
			{
				string propertyPath = undoPropertyModification.currentValue.propertyPath;
				switch (propertyPath)
				{
				case "m_LocalPosition.x":
					position.x = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalPosition.y":
					position.y = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalPosition.z":
					position.z = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalRotation.x":
					rotation.x = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalRotation.y":
					rotation.y = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalRotation.z":
					rotation.z = float.Parse(undoPropertyModification.currentValue.value);
					break;
				case "m_LocalRotation.w":
					rotation.w = float.Parse(undoPropertyModification.currentValue.value);
					break;
				}
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00017E20 File Offset: 0x00016220
		internal static void SetPreviousPositionAndRotation(UndoPropertyModification[] mods, Vector3 pos, Quaternion rot)
		{
			foreach (UndoPropertyModification undoPropertyModification in mods)
			{
				string propertyPath = undoPropertyModification.previousValue.propertyPath;
				switch (propertyPath)
				{
				case "m_LocalPosition.x":
					undoPropertyModification.previousValue.value = pos.x.ToString();
					break;
				case "m_LocalPosition.y":
					undoPropertyModification.previousValue.value = pos.y.ToString();
					break;
				case "m_LocalPosition.z":
					undoPropertyModification.previousValue.value = pos.z.ToString();
					break;
				case "m_LocalRotation.x":
					undoPropertyModification.previousValue.value = rot.x.ToString();
					break;
				case "m_LocalRotation.y":
					undoPropertyModification.previousValue.value = rot.y.ToString();
					break;
				case "m_LocalRotation.z":
					undoPropertyModification.previousValue.value = rot.z.ToString();
					break;
				case "m_LocalRotation.w":
					undoPropertyModification.previousValue.value = rot.w.ToString();
					break;
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0001800C File Offset: 0x0001640C
		internal static void SetCurrentPositionAndRotation(UndoPropertyModification[] mods, Vector3 pos, Quaternion rot)
		{
			foreach (UndoPropertyModification undoPropertyModification in mods)
			{
				string propertyPath = undoPropertyModification.previousValue.propertyPath;
				switch (propertyPath)
				{
				case "m_LocalPosition.x":
					undoPropertyModification.currentValue.value = pos.x.ToString();
					break;
				case "m_LocalPosition.y":
					undoPropertyModification.currentValue.value = pos.y.ToString();
					break;
				case "m_LocalPosition.z":
					undoPropertyModification.currentValue.value = pos.z.ToString();
					break;
				case "m_LocalRotation.x":
					undoPropertyModification.currentValue.value = rot.x.ToString();
					break;
				case "m_LocalRotation.y":
					undoPropertyModification.currentValue.value = rot.y.ToString();
					break;
				case "m_LocalRotation.z":
					undoPropertyModification.currentValue.value = rot.z.ToString();
					break;
				case "m_LocalRotation.w":
					undoPropertyModification.currentValue.value = rot.w.ToString();
					break;
				}
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000181F8 File Offset: 0x000165F8
		internal static UndoPropertyModification[] HandleEulerModifications(AnimationTrack track, TimelineClip clip, AnimationClip animClip, float time, UndoPropertyModification[] mods)
		{
			if (mods.Any((UndoPropertyModification x) => x.currentValue.propertyPath.StartsWith("m_LocalEulerAnglesHint") || x.currentValue.propertyPath.StartsWith("m_LocalRotation")))
			{
				TimelineAnimationUtilities.RigidTransform localToTrack = TimelineRecording.GetLocalToTrack(track, clip);
				if (localToTrack.rotation != Quaternion.identity)
				{
					if (TimelineRecording.s_LastTrackWarning != track)
					{
						TimelineRecording.s_LastTrackWarning = track;
						Debug.LogWarning("You are recording with an initial rotation offset. This may result in a misrepresentation of euler angles. When recording transform properties, it is recommended to reset rotation prior to recording");
					}
					Transform transform = mods[0].currentValue.target as Transform;
					if (transform != null)
					{
						TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineAnimationUtilities.RigidTransform.Inverse(localToTrack);
						IEnumerable<UndoPropertyModification> first = from x in mods
						where !x.currentValue.propertyPath.StartsWith("m_LocalEulerAnglesHint")
						select x;
						IEnumerable<UndoPropertyModification> second = TimelineRecording.FindBestEulerHint(rigidTransform.rotation * transform.localRotation, animClip, time, transform);
						return first.Union(second).ToArray<UndoPropertyModification>();
					}
					return (from x in mods
					where !x.currentValue.propertyPath.StartsWith("m_LocalEulerAnglesHint")
					select x).ToArray<UndoPropertyModification>();
				}
			}
			return mods;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00018330 File Offset: 0x00016730
		internal static IEnumerable<UndoPropertyModification> FindBestEulerHint(Quaternion rotation, AnimationClip clip, float time, Transform transform)
		{
			Vector3 vector = rotation.eulerAngles;
			AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.x"));
			AnimationCurve editorCurve2 = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.y"));
			AnimationCurve editorCurve3 = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "localEulerAnglesRaw.z"));
			if (editorCurve != null)
			{
				vector.x = editorCurve.Evaluate(time);
			}
			if (editorCurve2 != null)
			{
				vector.y = editorCurve2.Evaluate(time);
			}
			if (editorCurve3 != null)
			{
				vector.z = editorCurve3.Evaluate(time);
			}
			vector = QuaternionCurveTangentCalculation.GetEulerFromQuaternion(rotation, vector);
			return new UndoPropertyModification[]
			{
				TimelineRecording.PropertyModificationToUndoPropertyModification(new PropertyModification
				{
					target = transform,
					propertyPath = "m_LocalEulerAnglesHint.x",
					value = vector.x.ToString()
				}),
				TimelineRecording.PropertyModificationToUndoPropertyModification(new PropertyModification
				{
					target = transform,
					propertyPath = "m_LocalEulerAnglesHint.y",
					value = vector.y.ToString()
				}),
				TimelineRecording.PropertyModificationToUndoPropertyModification(new PropertyModification
				{
					target = transform,
					propertyPath = "m_LocalEulerAnglesHint.z",
					value = vector.z.ToString()
				})
			};
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000184D0 File Offset: 0x000168D0
		internal static bool HasAnyPlayableAssetModifications(UndoPropertyModification[] modifications)
		{
			return modifications.Any((UndoPropertyModification x) => x.currentValue.target is IPlayableAsset);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00018508 File Offset: 0x00016908
		internal static UndoPropertyModification[] ProcessPlayableAssetModification(UndoPropertyModification[] modifications, TimelineWindow.TimelineState state)
		{
			UndoPropertyModification[] result;
			if (state == null || state.currentDirector == null)
			{
				result = modifications;
			}
			else
			{
				List<UndoPropertyModification> list = new List<UndoPropertyModification>();
				foreach (UndoPropertyModification undoPropertyModification in modifications)
				{
					TimelineClip timelineClip = TimelineRecording.FindClipWithAsset(state.timeline, undoPropertyModification.currentValue.target as IPlayableAsset, state.currentDirector);
					if (timelineClip == null || !TimelineRecording.IsRecording(timelineClip, state) || !TimelineRecording.ProcessPlayableAssetRecording(undoPropertyModification, state, timelineClip))
					{
						list.Add(undoPropertyModification);
					}
				}
				if (list.Count<UndoPropertyModification>() != modifications.Length)
				{
					state.rebuildGraph = true;
					state.GetWindow().Repaint();
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000185E4 File Offset: 0x000169E4
		internal static TimelineClip FindClipWithAsset(TimelineAsset asset, IPlayableAsset target, PlayableDirector director)
		{
			TimelineClip result;
			if (target == null || asset == null || director == null)
			{
				result = null;
			}
			else
			{
				IEnumerable<TimelineClip> source = asset.flattenedTracks.SelectMany((TrackAsset x) => x.clips);
				result = source.FirstOrDefault((TimelineClip x) => x != null && x.asset != null && target == x.asset as IPlayableAsset);
			}
			return result;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0001866C File Offset: 0x00016A6C
		internal static bool IsRecording(TimelineClip clip, TimelineWindow.TimelineState state)
		{
			return clip != null && clip.parentTrack != null && state.IsArmedForRecord(clip.parentTrack);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000186A8 File Offset: 0x00016AA8
		internal static bool ProcessPlayableAssetRecording(UndoPropertyModification mod, TimelineWindow.TimelineState state, TimelineClip clip)
		{
			bool result;
			if (!clip.IsParameterAnimatable(mod.currentValue.propertyPath))
			{
				result = false;
			}
			else
			{
				double num = (state.time - clip.start + clip.clipIn) * clip.timeScale;
				if (num < 0.0)
				{
					result = false;
				}
				else
				{
					bool flag = clip.AddAnimatedParameterValueAt(mod.currentValue.propertyPath, float.Parse(mod.currentValue.value), (float)num);
					if (flag && AnimationMode.InAnimationMode())
					{
						EditorCurveBinding curveBinding = clip.GetCurveBinding(mod.previousValue.propertyPath);
						AnimationMode.AddPropertyModification(curveBinding, mod.previousValue, true);
						clip.parentTrack.showInlineCurves = true;
						if (state.GetWindow() != null && state.GetWindow().treeView != null)
						{
							state.GetWindow().treeView.CalculateRowRects();
						}
					}
					result = flag;
				}
			}
			return result;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000187A4 File Offset: 0x00016BA4
		private static bool IsPlayableAssetProperty(SerializedProperty property)
		{
			return property.serializedObject.targetObject is IPlayableAsset;
		}

		// Token: 0x040001FE RID: 510
		private static readonly List<PropertyModification> s_TempPropertyModifications = new List<PropertyModification>(6);

		// Token: 0x040001FF RID: 511
		private static readonly TimelineRecording.RecordingState s_RecordState = new TimelineRecording.RecordingState();

		// Token: 0x04000200 RID: 512
		private static readonly AnimationTrackRecorder s_TrackRecorder = new AnimationTrackRecorder();

		// Token: 0x04000201 RID: 513
		private static readonly List<UndoPropertyModification> s_UnprocessedMods = new List<UndoPropertyModification>();

		// Token: 0x04000202 RID: 514
		private static readonly List<UndoPropertyModification> s_ModsToProcess = new List<UndoPropertyModification>();

		// Token: 0x04000203 RID: 515
		private static AnimationTrack s_LastTrackWarning;

		// Token: 0x04000204 RID: 516
		private const string kLocalPosition = "m_LocalPosition";

		// Token: 0x04000205 RID: 517
		private const string kLocalRotation = "m_LocalRotation";

		// Token: 0x04000206 RID: 518
		private const string kLocalEulerHint = "m_LocalEulerAnglesHint";

		// Token: 0x04000207 RID: 519
		private const string kRotationWarning = "You are recording with an initial rotation offset. This may result in a misrepresentation of euler angles. When recording transform properties, it is recommended to reset rotation prior to recording";

		// Token: 0x04000208 RID: 520
		[CompilerGenerated]
		private static Func<PropertyModification, UndoPropertyModification> <>f__mg$cache0;

		// Token: 0x0200004C RID: 76
		internal class RecordingState : IAnimationRecordingState
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x060002AC RID: 684 RVA: 0x0001894C File Offset: 0x00016D4C
			// (set) Token: 0x060002AD RID: 685 RVA: 0x00018966 File Offset: 0x00016D66
			public GameObject activeGameObject { get; set; }

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x060002AE RID: 686 RVA: 0x00018970 File Offset: 0x00016D70
			// (set) Token: 0x060002AF RID: 687 RVA: 0x0001898A File Offset: 0x00016D8A
			public GameObject activeRootGameObject { get; set; }

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x060002B0 RID: 688 RVA: 0x00018994 File Offset: 0x00016D94
			// (set) Token: 0x060002B1 RID: 689 RVA: 0x000189AE File Offset: 0x00016DAE
			public AnimationClip activeAnimationClip { get; set; }

			// Token: 0x060002B2 RID: 690 RVA: 0x000189B7 File Offset: 0x00016DB7
			public void SaveCurve(AnimationWindowCurve curve)
			{
				Undo.RegisterCompleteObjectUndo(this.activeAnimationClip, "Edit Curve");
				AnimationRecording.SaveModifiedCurve(curve, this.activeAnimationClip);
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x060002B3 RID: 691 RVA: 0x000189D8 File Offset: 0x00016DD8
			public bool addZeroFrame
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x060002B4 RID: 692 RVA: 0x000189F0 File Offset: 0x00016DF0
			// (set) Token: 0x060002B5 RID: 693 RVA: 0x00018A0A File Offset: 0x00016E0A
			public int currentFrame { get; set; }
		}
	}
}
