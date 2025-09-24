using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200004A RID: 74
	internal class AnimationTrackRecorder
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000267 RID: 615 RVA: 0x000158B4 File Offset: 0x00013CB4
		// (set) Token: 0x06000268 RID: 616 RVA: 0x000158CE File Offset: 0x00013CCE
		public TimelineClip recordClip { get; private set; }

		// Token: 0x06000269 RID: 617 RVA: 0x000158D7 File Offset: 0x00013CD7
		public void PrepareForRecord(TimelineWindow.TimelineState state)
		{
			this.m_ProcessedClips.Clear();
			this.m_RebindList.Clear();
			this.m_RefreshState = false;
			this.m_TracksToProcess.Clear();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00015904 File Offset: 0x00013D04
		public AnimationClip PrepareTrack(TrackAsset track, TimelineWindow.TimelineState state, GameObject gameObject, out double startTime)
		{
			AnimationTrack animationTrack = (AnimationTrack)track;
			AnimationClip result;
			if (!animationTrack.inClipMode)
			{
				startTime = 0.0;
				AnimationClip orCreateClip = animationTrack.GetOrCreateClip();
				if (!this.m_TracksToProcess.Contains(animationTrack))
				{
					this.m_TracksToProcess.Add(animationTrack);
				}
				this.m_RebindList.Add(gameObject);
				if (orCreateClip.empty)
				{
					startTime = 0.0;
					animationTrack.openClipTimeOffset = 0.0;
					animationTrack.openClipPreExtrapolation = 1;
					animationTrack.openClipPostExtrapolation = 1;
				}
				result = orCreateClip;
			}
			else
			{
				TimelineClip timelineClip = AnimationTrackRecorder.GetRecordingClipForTrack(track, state);
				if (timelineClip == null)
				{
					timelineClip = track.FindRecordingClipAtTime(state.time);
				}
				if (timelineClip == null)
				{
					timelineClip = AnimationTrackRecorder.AddRecordableClip(track, state);
					timelineClip.start = state.time;
					this.m_RebindList.Add(gameObject);
				}
				AnimationClip animationClip = timelineClip.animationClip;
				double num = state.time - timelineClip.start;
				if (num < 0.0)
				{
					Undo.RegisterCompleteObjectUndo(animationClip, "record.key");
					TimelineHelpers.PushUndo(track, "prepend.key");
					AnimationTrackRecorder.ShiftAnimationClip(animationClip, (float)(-(float)num));
					timelineClip.start = state.time;
					timelineClip.duration += -num;
					num = 0.0;
					this.m_RefreshState = true;
				}
				this.m_ClipTime = num;
				this.recordClip = timelineClip;
				startTime = this.recordClip.start;
				result = animationClip;
			}
			return result;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00015A84 File Offset: 0x00013E84
		public void FinializeTrack(TrackAsset track, TimelineWindow.TimelineState state)
		{
			AnimationTrack animationTrack = track as AnimationTrack;
			if (!animationTrack.inClipMode)
			{
				EditorUtility.SetDirty(animationTrack.GetOrCreateClip());
			}
			if (this.recordClip != null)
			{
				if (!this.m_ProcessedClips.Contains(this.recordClip.animationClip))
				{
					this.m_ProcessedClips.Add(this.recordClip.animationClip);
				}
				if (this.m_ClipTime > this.recordClip.duration)
				{
					TimelineHelpers.PushUndo(track, "add.key");
					this.recordClip.duration = this.m_ClipTime;
					this.m_RefreshState = true;
				}
				Extrapolation.CalculateExtrapolationTimes(track);
			}
			this.recordClip = null;
			this.m_ClipTime = 0.0;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00015B48 File Offset: 0x00013F48
		public void FinalizeRecording(TimelineWindow.TimelineState state)
		{
			for (int num = 0; num != this.m_ProcessedClips.Count; num++)
			{
				AnimationTrackRecorder.ProcessTemporaryKeys(this.m_ProcessedClips[num]);
			}
			this.m_RefreshState |= this.m_TracksToProcess.Any<TrackAsset>();
			this.m_TracksToProcess.Clear();
			if (this.m_ProcessedClips.Count > 0 || this.m_RefreshState)
			{
				state.GetWindow().RebuildGraphIfNecessary(false);
			}
			state.RebindAnimators(this.m_RebindList);
			if (this.m_ProcessedClips.Count > 0 || this.m_RefreshState)
			{
				state.EvaluateImmediate();
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00015C04 File Offset: 0x00014004
		public static string GetUniqueRecordedClipName(Object owner, string name)
		{
			string assetPath = AssetDatabase.GetAssetPath(owner);
			string result;
			if (!string.IsNullOrEmpty(assetPath))
			{
				IEnumerable<string> source = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
				where x != null
				select x.name;
				result = ObjectNames.GetUniqueName(source.ToArray<string>(), name);
			}
			else
			{
				TrackAsset trackAsset = owner as TrackAsset;
				if (trackAsset == null || trackAsset.clips.Length == 0)
				{
					result = name;
				}
				else
				{
					result = ObjectNames.GetUniqueName((from x in trackAsset.clips
					select x.displayName).ToArray<string>(), name);
				}
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00015CE4 File Offset: 0x000140E4
		public static TimelineClip AddRecordableClip(TrackAsset parentTrack, TimelineWindow.TimelineState state)
		{
			TimelineAsset timeline = state.timeline;
			TimelineClip result;
			if (timeline == null)
			{
				Debug.LogError("Parent Track needs to be bound to an asset to add a recordable");
				result = null;
			}
			else
			{
				AnimationClip animationClip = new AnimationClip();
				animationClip.name = AnimationTrackRecorder.GetUniqueRecordedClipName(parentTrack, AnimationTrackRecorder.kRecordClipDefaultName);
				animationClip.frameRate = state.frameRate;
				AnimationUtility.SetGenerateMotionCurves(animationClip, true);
				Undo.RegisterCreatedObjectUndo(animationClip, "create clip");
				TimelineHelpers.SaveAnimClipIntoObject(animationClip, parentTrack);
				TimelineClip timelineClip = parentTrack.CreateClipFromAsset(animationClip);
				if (timelineClip != null)
				{
					timelineClip.recordable = true;
					timelineClip.m_ID = timeline.GenerateNewId();
					timelineClip.displayName = animationClip.name;
					timelineClip.timeScale = 1.0;
					timelineClip.start = 0.0;
					timelineClip.duration = 0.0;
					timelineClip.m_ID = timeline.GenerateNewId();
					timelineClip.mixInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					timelineClip.mixOutCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
					timelineClip.preExtrapolationMode = 1;
					timelineClip.postExtrapolationMode = 1;
					TimelineHelpers.SaveAssetIntoObject(timelineClip.asset, parentTrack);
					state.Refresh();
				}
				result = timelineClip;
			}
			return result;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00015E24 File Offset: 0x00014224
		private static TimelineClip GetRecordingClipForTrack(TrackAsset track, TimelineWindow.TimelineState state)
		{
			return track.FindRecordingClipAtTime(state.time);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00015E48 File Offset: 0x00014248
		private static void ShiftAnimationClip(AnimationClip clip, float amount)
		{
			if (!(clip == null))
			{
				EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
				EditorCurveBinding[] objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
				foreach (EditorCurveBinding editorCurveBinding in curveBindings)
				{
					AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, editorCurveBinding);
					editorCurve.keys = AnimationTrackRecorder.ShiftKeys(editorCurve.keys, amount);
					AnimationUtility.SetEditorCurve(clip, editorCurveBinding, editorCurve);
				}
				foreach (EditorCurveBinding editorCurveBinding2 in objectReferenceCurveBindings)
				{
					ObjectReferenceKeyframe[] array3 = AnimationUtility.GetObjectReferenceCurve(clip, editorCurveBinding2);
					array3 = AnimationTrackRecorder.ShiftObjectKeys(array3, amount);
					AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding2, array3);
				}
				EditorUtility.SetDirty(clip);
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00015F1C File Offset: 0x0001431C
		private static Keyframe[] ShiftKeys(Keyframe[] keys, float time)
		{
			Keyframe[] result;
			if (keys == null || keys.Length == 0 || time == 0f)
			{
				result = keys;
			}
			else
			{
				Keyframe[] array = new Keyframe[keys.Length + 1];
				array[0] = keys[0];
				array[0].inTangent = 0f;
				array[0].outTangent = 0f;
				for (int i = 0; i < keys.Length; i++)
				{
					array[i + 1] = keys[i];
					Keyframe[] array2 = array;
					int num = i + 1;
					array2[num].time = array2[num].time + time;
				}
				array[1].inTangent = 0f;
				result = array;
			}
			return result;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00015FF0 File Offset: 0x000143F0
		private static ObjectReferenceKeyframe[] ShiftObjectKeys(ObjectReferenceKeyframe[] keys, float time)
		{
			ObjectReferenceKeyframe[] result;
			if (keys == null || keys.Length == 0 || time == 0f)
			{
				result = keys;
			}
			else
			{
				ObjectReferenceKeyframe[] array = new ObjectReferenceKeyframe[keys.Length + 1];
				array[0] = keys[0];
				for (int i = 0; i < keys.Length; i++)
				{
					array[i + 1] = keys[i];
					ObjectReferenceKeyframe[] array2 = array;
					int num = i + 1;
					array2[num].time = array2[num].time + time;
				}
				result = array;
			}
			return result;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00016090 File Offset: 0x00014490
		private static bool ProcessCurveBinding(AnimationClip clip, EditorCurveBinding binding)
		{
			float num = 1.5f;
			if ((double)clip.frameRate > 0.0)
			{
				num = 1.5f / clip.frameRate;
			}
			bool result = false;
			AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, binding);
			Keyframe[] keys = editorCurve.keys;
			if (keys.Length == 3 && AnimationUtility.GetKeyBroken(keys[1]))
			{
				float num2 = keys[1].time - keys[0].time;
				float num3 = keys[2].time - keys[1].time;
				float num4 = Mathf.Min(num2, num3);
				if (num4 <= num)
				{
					if (num2 < num3)
					{
						editorCurve.RemoveKey(1);
					}
					else
					{
						editorCurve.RemoveKey(2);
					}
					keys = editorCurve.keys;
					AnimationUtility.SetKeyBroken(editorCurve, 0, false);
					AnimationUtility.SetKeyBroken(editorCurve, 1, false);
					AnimationUtility.SetKeyLeftTangentMode(editorCurve, 0, 1);
					AnimationUtility.SetKeyLeftTangentMode(editorCurve, 1, 1);
					AnimationUtility.SetKeyRightTangentMode(editorCurve, 0, 1);
					AnimationUtility.SetKeyRightTangentMode(editorCurve, 1, 1);
					editorCurve.keys = keys;
					AnimationUtility.UpdateTangentsFromMode(editorCurve);
					AnimationUtility.SetEditorCurve(clip, binding, editorCurve);
					result = true;
				}
			}
			else if (keys.Length == 1 && keys[0].time == 0f)
			{
				editorCurve.AddKey(1f / clip.frameRate, keys[0].value);
				keys = editorCurve.keys;
				AnimationUtility.SetKeyLeftTangentMode(editorCurve, 0, 1);
				AnimationUtility.SetKeyRightTangentMode(editorCurve, 0, 1);
				AnimationUtility.SetKeyBroken(editorCurve, 0, true);
				AnimationUtility.SetKeyBroken(editorCurve, 1, true);
				Keyframe[] array = keys;
				int num5 = 0;
				float num6 = 0f;
				keys[0].outTangent = num6;
				array[num5].inTangent = num6;
				Keyframe[] array2 = keys;
				int num7 = 1;
				num6 = 0f;
				keys[1].outTangent = num6;
				array2[num7].inTangent = num6;
				editorCurve.keys = keys;
				AnimationUtility.SetEditorCurve(clip, binding, editorCurve);
				EditorUtility.SetDirty(clip);
				result = true;
			}
			else if (keys.Length == 2)
			{
				float num8 = keys[1].time - keys[0].time;
				if (AnimationUtility.GetKeyBroken(keys[0]) && AnimationUtility.GetKeyBroken(keys[1]) && num8 > 0f && num8 < num)
				{
					keys[1].value = keys[0].value;
					editorCurve.keys = keys;
					AnimationUtility.SetEditorCurve(clip, binding, editorCurve);
					EditorUtility.SetDirty(clip);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001631C File Offset: 0x0001471C
		private static void ProcessTemporaryKeys(AnimationClip clip)
		{
			if (!(clip == null))
			{
				bool flag = false;
				EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
				foreach (EditorCurveBinding editorCurveBinding in curveBindings)
				{
					if (!editorCurveBinding.propertyName.Contains("LocalRotation.w"))
					{
						EditorCurveBinding binding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(editorCurveBinding, clip);
						flag |= AnimationTrackRecorder.ProcessCurveBinding(clip, binding);
					}
				}
				if (flag)
				{
					EditorUtility.SetDirty(clip);
				}
			}
		}

		// Token: 0x040001F4 RID: 500
		public static readonly string kRecordClipDefaultName = "Recorded";

		// Token: 0x040001F5 RID: 501
		private readonly List<AnimationClip> m_ProcessedClips = new List<AnimationClip>();

		// Token: 0x040001F6 RID: 502
		private readonly List<GameObject> m_RebindList = new List<GameObject>();

		// Token: 0x040001F7 RID: 503
		private bool m_RefreshState = false;

		// Token: 0x040001F8 RID: 504
		private double m_ClipTime = 0.0;

		// Token: 0x040001FA RID: 506
		private readonly List<TrackAsset> m_TracksToProcess = new List<TrackAsset>();
	}
}
