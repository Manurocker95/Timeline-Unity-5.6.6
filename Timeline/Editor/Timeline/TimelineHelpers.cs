using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200000E RID: 14
	internal static class TimelineHelpers
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00007108 File Offset: 0x00005508
		private static ScriptableObject CloneReferencedPlayableAsset(ScriptableObject original, PlayableDirector directorInstance)
		{
			ScriptableObject scriptableObject = Object.Instantiate<ScriptableObject>(original);
			if (scriptableObject == null || !(scriptableObject is IPlayableAsset))
			{
				throw new InvalidCastException("could not cast instantiated object into IPlayableAsset");
			}
			if (directorInstance != null)
			{
				SerializedObject serializedObject = new SerializedObject(original);
				SerializedObject serializedObject2 = new SerializedObject(scriptableObject);
				SerializedProperty iterator = serializedObject.GetIterator();
				if (iterator.Next(true))
				{
					do
					{
						serializedObject2.CopyFromSerializedProperty(iterator);
					}
					while (iterator.Next(false));
				}
				serializedObject2.ApplyModifiedProperties();
				EditorUtility.SetDirty(directorInstance);
			}
			if (directorInstance != null)
			{
				List<FieldInfo> list = (from f in scriptableObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(ExposedReference)
				select f).ToList<FieldInfo>();
				foreach (FieldInfo fieldInfo in list)
				{
					object value = fieldInfo.GetValue(scriptableObject);
					FieldInfo field = value.GetType().GetField("exposedName");
					if (field != null)
					{
						PropertyName propertyName = (PropertyName)field.GetValue(value);
						bool flag = false;
						Object referenceValue = directorInstance.GetReferenceValue(propertyName, ref flag);
						if (flag)
						{
							PropertyName propertyName2;
							propertyName2..ctor(GUID.Generate().ToString());
							directorInstance.SetReferenceValue(propertyName2, referenceValue);
							field.SetValue(value, propertyName2);
						}
					}
					fieldInfo.SetValue(scriptableObject, value);
				}
			}
			IEnumerable<FieldInfo> enumerable = from f in scriptableObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where !f.IsNotSerialized && f.FieldType == typeof(AnimationClip)
			select f;
			foreach (FieldInfo fieldInfo2 in enumerable)
			{
				fieldInfo2.SetValue(scriptableObject, TimelineHelpers.CloneAnimationClipIfRequired(fieldInfo2.GetValue(scriptableObject) as AnimationClip, original));
			}
			return scriptableObject;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000734C File Offset: 0x0000574C
		private static void SaveCloneToOriginalAsset(Object original, Object clone)
		{
			string assetPath = AssetDatabase.GetAssetPath(original);
			Object @object = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
			if (@object != null)
			{
				TimelineHelpers.SaveAssetIntoObject(clone, @object);
				EditorUtility.SetDirty(@object);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007384 File Offset: 0x00005784
		private static AnimationClip CloneAnimationClipIfRequired(AnimationClip clip, Object owner)
		{
			AnimationClip result;
			if (clip == null)
			{
				result = null;
			}
			else
			{
				string assetPath = AssetDatabase.GetAssetPath(clip);
				string assetPath2 = AssetDatabase.GetAssetPath(owner);
				bool flag = assetPath == assetPath2;
				if (flag)
				{
					AnimationClip animationClip = Object.Instantiate<AnimationClip>(clip);
					animationClip.name = AnimationTrackRecorder.GetUniqueRecordedClipName(owner, clip.name);
					animationClip.hideFlags = clip.hideFlags;
					if ((clip.hideFlags & 52) != 52 && assetPath2.Length > 0)
					{
						TimelineHelpers.SaveAnimClipIntoObject(animationClip, owner);
					}
					EditorUtility.SetDirty(owner);
					clip = animationClip;
				}
				result = clip;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007424 File Offset: 0x00005824
		public static TimelineClip Clone(TimelineClip clip, PlayableDirector directorInstance, TimelineAsset timeline)
		{
			TimelineClip clip2 = Object.Instantiate<EditorClip>(EditorClip.CreateEditorClip(timeline, clip)).clip;
			clip2.m_ID = Guid.NewGuid().GetHashCode();
			clip2.selected = false;
			clip2.parentTrack = null;
			clip2.ClearAnimatedParameterCurves();
			if (clip.curves != null)
			{
				AnimatedParameterExtensions.CreateCurvesIfRequired(clip2, clip.parentTrack);
				EditorUtility.CopySerialized(clip.curves, clip2.curves);
			}
			ScriptableObject scriptableObject = clip2.asset as ScriptableObject;
			if (scriptableObject != null && clip2.asset is IPlayableAsset)
			{
				ScriptableObject scriptableObject2 = TimelineHelpers.CloneReferencedPlayableAsset(scriptableObject, directorInstance);
				TimelineHelpers.SaveCloneToOriginalAsset(scriptableObject, scriptableObject2);
				clip2.asset = scriptableObject2;
				AnimationPlayableAsset animationPlayableAsset = scriptableObject2 as AnimationPlayableAsset;
				if (clip2.recordable && animationPlayableAsset != null && animationPlayableAsset.clip != null)
				{
					clip2.displayName = animationPlayableAsset.clip.name;
				}
			}
			return clip2;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007530 File Offset: 0x00005930
		public static TrackAsset Clone(TrackAsset asset, PlayableDirector directorInstance)
		{
			TrackAsset result;
			if (asset == null)
			{
				result = null;
			}
			else
			{
				TimelineAsset timelineAsset = asset.timelineAsset;
				if (timelineAsset == null)
				{
					result = null;
				}
				else
				{
					string[] array = (from x in timelineAsset.flattenedTracks
					select x.name).ToArray<string>();
					TrackAsset trackAsset = Object.Instantiate<TrackAsset>(asset);
					trackAsset.SetClips(new List<TimelineClip>());
					trackAsset.parent = null;
					trackAsset.name = ObjectNames.GetUniqueName(array, asset.name);
					trackAsset.subTracks = new List<TrackAsset>();
					if (asset.animClip != null)
					{
						trackAsset.animClip = TimelineHelpers.CloneAnimationClipIfRequired(asset.animClip, asset);
					}
					foreach (TimelineClip clip in asset.clips)
					{
						TimelineClip timelineClip = TimelineHelpers.Clone(clip, directorInstance, timelineAsset);
						timelineClip.parentTrack = trackAsset;
						trackAsset.AddClip(timelineClip);
					}
					result = trackAsset;
				}
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007640 File Offset: 0x00005A40
		public static void PushDestroyUndo(TimelineAsset timeline, Object thingToDirty, Object objectToDestroy, string operation)
		{
			if (!(objectToDestroy == null))
			{
				EditorUtility.SetDirty(thingToDirty);
				if (timeline != null)
				{
					EditorUtility.SetDirty(timeline);
				}
				Undo.DestroyObjectImmediate(objectToDestroy);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007674 File Offset: 0x00005A74
		public static bool IsCircularRef(TimelineAsset baseSeq, TimelineAsset other)
		{
			bool result;
			if (baseSeq == other)
			{
				result = true;
			}
			else
			{
				foreach (TrackAsset trackAsset in other.flattenedTracks)
				{
					foreach (TimelineClip timelineClip in trackAsset.clips)
					{
						if (timelineClip.isNestedAsset && timelineClip.asset is TimelineAsset)
						{
							bool flag = TimelineHelpers.IsCircularRef(baseSeq, timelineClip.asset as TimelineAsset);
							if (flag)
							{
								return true;
							}
						}
						else if (timelineClip.asset == baseSeq)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007770 File Offset: 0x00005B70
		public static void PushUndoNoRefreshOnUndo(Object thingToDirty, string operation)
		{
			TimelineHelpers.PushUndo(thingToDirty, "norefresh." + operation);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007784 File Offset: 0x00005B84
		public static void PushUndo(Object thingToDirty, string operation)
		{
			EditorUtility.SetDirty(thingToDirty);
			Undo.RegisterCompleteObjectUndo(thingToDirty, "sequence." + operation);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000077A0 File Offset: 0x00005BA0
		public static TrackType[] GetMixableTypes()
		{
			TrackType[] result;
			if (TimelineHelpers.cachedMixableTypes != null)
			{
				result = TimelineHelpers.cachedMixableTypes;
			}
			else
			{
				TimelineHelpers.cachedMixableTypes = (from x in EditorAssemblies.loadedTypes
				where !x.IsAbstract && typeof(TrackAsset).IsAssignableFrom(x)
				select x into t
				select new TrackType(t, TimelineHelpers.GetMediaTypeFromType(t))).ToArray<TrackType>();
				result = TimelineHelpers.cachedMixableTypes;
			}
			return result;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007824 File Offset: 0x00005C24
		public static bool IsTypeSupportedByTrack(TrackType trackType, Type objectType)
		{
			TrackType[] trackTypeHandle = TimelineHelpers.GetTrackTypeHandle(objectType);
			return trackTypeHandle.Contains(trackType);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007848 File Offset: 0x00005C48
		public static TimelineAsset.MediaType GetMediaTypeFromTrackType(TrackType trackType)
		{
			return TimelineHelpers.GetMediaTypeFromType(trackType.m_TrackType);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007868 File Offset: 0x00005C68
		public static TimelineAsset.MediaType GetMediaTypeFromType(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(TrackMediaType), true);
			return (!customAttributes.Any<object>()) ? 3 : ((TrackMediaType)customAttributes[0]).m_MediaType;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000078B0 File Offset: 0x00005CB0
		public static DirectorStreamType StreamTypeFromType(Type type)
		{
			TimelineAsset.MediaType mediaTypeFromType = TimelineHelpers.GetMediaTypeFromType(type);
			DirectorStreamType result = 3;
			switch (mediaTypeFromType)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 1;
				break;
			case 2:
				result = 2;
				break;
			case 3:
				result = 3;
				break;
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00007908 File Offset: 0x00005D08
		public static TrackType[] GetTrackTypeHandle(Type toBeHandled)
		{
			Type[] array = (from assemblyType in EditorAssemblies.loadedTypes
			where assemblyType.IsSubclassOf(typeof(TrackAsset))
			select assemblyType).ToArray<Type>();
			List<TrackType> list = new List<TrackType>();
			foreach (Type type in array)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
				foreach (object obj in customAttributes)
				{
					Type inspectedType = (obj as TrackClipTypeAttribute).inspectedType;
					if (inspectedType == toBeHandled || inspectedType.IsAssignableFrom(toBeHandled))
					{
						TrackType item = new TrackType(type, TimelineHelpers.GetMediaTypeFromType(type));
						list.Add(item);
					}
				}
			}
			if (toBeHandled == typeof(MonoScript))
			{
				list.Add(new TrackType(typeof(PlayableTrack), TimelineHelpers.GetMediaTypeFromType(typeof(PlayableTrack))));
			}
			return list.ToArray();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00007A28 File Offset: 0x00005E28
		public static IEnumerable<Type> GetTypesHandledByTrackType(TrackType trackType)
		{
			object[] customAttributes = trackType.m_TrackType.GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
			return from a in customAttributes
			select (a as TrackClipTypeAttribute).inspectedType;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007A78 File Offset: 0x00005E78
		public static Type[] GetAllStandalonePlayableAssets()
		{
			IEnumerable<TrackType> mixableTypes = TimelineHelpers.GetMixableTypes();
			if (TimelineHelpers.<>f__mg$cache0 == null)
			{
				TimelineHelpers.<>f__mg$cache0 = new Func<TrackType, IEnumerable<Type>>(TimelineHelpers.GetTypesHandledByTrackType);
			}
			IEnumerable<Type> second = mixableTypes.SelectMany(TimelineHelpers.<>f__mg$cache0);
			IEnumerable<Type> first = from assemblyType in EditorAssemblies.loadedTypes
			where typeof(IPlayableAsset).IsAssignableFrom(assemblyType) && typeof(ScriptableObject).IsAssignableFrom(assemblyType) && !assemblyType.IsSubclassOf(typeof(TrackAsset)) && assemblyType.Assembly.FullName.Contains("Assembly-CSharp")
			select assemblyType;
			TimelineHelpers.standaloneAssetTypes = first.Except(second).ToArray<Type>();
			return TimelineHelpers.standaloneAssetTypes;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007AF4 File Offset: 0x00005EF4
		public static TrackType TrackTypeFromType(Type t)
		{
			return new TrackType(t, TimelineHelpers.GetMediaTypeFromType(t));
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00007B18 File Offset: 0x00005F18
		public static Type GetCustomDrawer(Type trackType)
		{
			Type type = ScriptAttributeUtility.GetDrawerTypeForType(trackType);
			if (type == null || !typeof(TrackDrawer).IsAssignableFrom(type) || type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null)
			{
				type = typeof(TrackDrawer);
			}
			return type;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007B78 File Offset: 0x00005F78
		public static string GenerateUniqueActorName(TimelineAsset timeline, string prefix)
		{
			string result;
			if (!timeline.tracks.Exists((TrackAsset x) => x.name == prefix))
			{
				result = prefix;
			}
			else
			{
				int num = 1;
				string newName = prefix + num.ToString();
				while (timeline.tracks.Exists((TrackAsset x) => x.name == newName))
				{
					num++;
					newName = prefix + num.ToString();
				}
				result = newName;
			}
			return result;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00007C2D File Offset: 0x0000602D
		public static void SaveAssetIntoObject(Object childAsset, Object masterAsset)
		{
			if ((masterAsset.hideFlags & 52) != null)
			{
				childAsset.hideFlags |= 52;
			}
			else
			{
				childAsset.hideFlags |= 1;
				AssetDatabase.AddObjectToAsset(childAsset, masterAsset);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00007C6C File Offset: 0x0000606C
		public static bool HaveSameContainerAsset(Object assetA, Object assetB)
		{
			return !(assetA == null) && !(assetB == null) && (((assetA.hideFlags & 52) != null && (assetB.hideFlags & 52) != null) || AssetDatabase.GetAssetPath(assetA) == AssetDatabase.GetAssetPath(assetB));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007CD4 File Offset: 0x000060D4
		public static void SaveAnimClipIntoObject(AnimationClip clip, Object asset)
		{
			if ((asset.hideFlags & 52) != null)
			{
				clip.hideFlags |= 52;
			}
			else
			{
				AssetDatabase.AddObjectToAsset(clip, asset);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00007D04 File Offset: 0x00006104
		public static TrackAsset GetGroup(object o)
		{
			TrackAsset result;
			if (o == null)
			{
				result = null;
			}
			else
			{
				TrackAsset trackAsset = o as TrackAsset;
				TimelineGroupGUI timelineGroupGUI = o as TimelineGroupGUI;
				if (trackAsset == null)
				{
					if (timelineGroupGUI != null)
					{
						if (timelineGroupGUI.track.GetType() == TimelineHelpers.GroupTrackType.m_TrackType)
						{
							return timelineGroupGUI.track;
						}
						trackAsset = (timelineGroupGUI.track.parent as TrackAsset);
					}
				}
				while (trackAsset != null)
				{
					if (trackAsset.GetType() == TimelineHelpers.GroupTrackType.m_TrackType)
					{
						return trackAsset;
					}
					trackAsset = (trackAsset.parent as TrackAsset);
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00007DC0 File Offset: 0x000061C0
		public static Component AddRequiredComponent(GameObject go, TrackAsset asset)
		{
			Component result;
			if (go == null || asset == null)
			{
				result = null;
			}
			else
			{
				PlayableBinding[] outputs = asset.outputs;
				if (outputs.Length == 1)
				{
					if (outputs[0].streamType == null)
					{
						Animator animator = go.GetComponent<Animator>();
						if (animator == null)
						{
							animator = go.AddComponent<Animator>();
							animator.applyRootMotion = true;
						}
						return animator;
					}
					if (outputs[0].streamType == 3 && typeof(Component).IsAssignableFrom(outputs[0].sourceBindingType))
					{
						Component component = go.GetComponent(outputs[0].sourceBindingType);
						if (component == null)
						{
							component = go.AddComponent(outputs[0].sourceBindingType);
						}
						return component;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00007EB4 File Offset: 0x000062B4
		public static bool NeedsBinding(TrackAsset track)
		{
			bool result;
			if (track == null)
			{
				result = false;
			}
			else if (track is GroupTrack)
			{
				result = false;
			}
			else if (track is AudioTrack)
			{
				result = true;
			}
			else
			{
				TrackAsset trackAsset = track.parent as TrackAsset;
				result = (trackAsset != null && TimelineHelpers.NeedsBinding(trackAsset));
			}
			return result;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007F28 File Offset: 0x00006328
		public static double GetTrackEndTime(TrackAsset track)
		{
			double num = 0.0;
			foreach (TimelineClip timelineClip in track.clips)
			{
				if (timelineClip != null && !double.IsPositiveInfinity(timelineClip.duration))
				{
					num = Math.Max(num, timelineClip.start + timelineClip.duration);
				}
			}
			return num;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00007F9C File Offset: 0x0000639C
		public static double FindBestInsertionTime(ITimelineState state, TimelineClip clip, TrackAsset track)
		{
			float num = state.TimeToTimeAreaPixel(state.time);
			return TimelineHelpers.FindBestInsertionTime((TimelineWindow.TimelineState)state, clip, track, new Vector2(num, num));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00007FD4 File Offset: 0x000063D4
		public static double FindBestInsertionTime(ITimelineState state, TimelineClip clip, TrackAsset track, Vector2 mousePosition)
		{
			double droppedTime = state.SnapToFrameIfRequired((double)state.ScreenSpacePixelToTimeAreaTime(mousePosition.x));
			TimelineClip timelineClip = (from c in track.clips
			where c != clip && c.start - TimeUtility.kTimeEpsilon <= droppedTime
			orderby c.start
			select c).LastOrDefault<TimelineClip>();
			double result;
			if (timelineClip != null)
			{
				double num = timelineClip.start + timelineClip.duration;
				double num2 = (double)state.TimeAreaPixelToTime(0f);
				if (num < num2)
				{
					result = droppedTime;
				}
				else if (!float.IsPositiveInfinity(mousePosition.x) && droppedTime > num)
				{
					result = droppedTime;
				}
				else
				{
					result = num;
				}
			}
			else
			{
				timelineClip = (from c in track.clips
				where c != clip
				orderby c.start + c.duration
				select c).LastOrDefault<TimelineClip>();
				if (timelineClip != null)
				{
					result = timelineClip.start + timelineClip.duration;
				}
				else
				{
					result = 0.0;
				}
			}
			return result;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000811C File Offset: 0x0000651C
		public static string GetTrackCategoryName(TrackType trackType)
		{
			string result;
			if (trackType.m_TrackType == null || trackType.m_TrackType.Namespace == null)
			{
				result = "";
			}
			else if (trackType.m_TrackType.Namespace.Contains("UnityEngine"))
			{
				result = "";
			}
			else
			{
				result = trackType.m_TrackType.Namespace;
			}
			return result;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00008188 File Offset: 0x00006588
		public static string GetTrackMenuName(TrackType trackType)
		{
			return ObjectNames.NicifyVariableName(trackType.m_TrackType.Name);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000081B0 File Offset: 0x000065B0
		public static double GetLoopDuration(TimelineClip clip)
		{
			double clipAssetDuration = clip.clipAssetDuration;
			double result;
			if (clipAssetDuration == 1.7976931348623157E+308 || double.IsInfinity(clipAssetDuration))
			{
				result = clipAssetDuration;
			}
			else
			{
				result = clipAssetDuration / clip.timeScale;
			}
			return result;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000081F8 File Offset: 0x000065F8
		public static bool HasUsableAssetDuration(TimelineClip clip)
		{
			double clipAssetDuration = clip.clipAssetDuration;
			return clipAssetDuration < TimelineClip.kMaxTimeValue && !double.IsInfinity(clipAssetDuration);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000822C File Offset: 0x0000662C
		public static double[] GetLoopTimes(TimelineClip clip)
		{
			double[] result;
			if (!TimelineHelpers.HasUsableAssetDuration(clip))
			{
				result = new double[]
				{
					-clip.clipIn
				};
			}
			else
			{
				List<double> list = new List<double>();
				double loopDuration = TimelineHelpers.GetLoopDuration(clip);
				if (loopDuration <= TimeUtility.kTimeEpsilon)
				{
					result = new double[0];
				}
				else
				{
					double num = -clip.clipIn;
					double num2 = num + loopDuration;
					list.Add(num);
					while (num2 < clip.duration - TimelineWindow.TimelineState.kTimeEpsilon)
					{
						list.Add(num2);
						num2 += loopDuration;
					}
					result = list.ToArray();
				}
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000082C8 File Offset: 0x000066C8
		public static TimelineClip CreateClipOnTrack(Object asset, TrackAsset parentTrack, ITimelineState state, Vector2 mousePosition)
		{
			double end = parentTrack.end;
			TimelineClip timelineClip = parentTrack.CreateClipFromAsset(asset);
			if (timelineClip != null)
			{
				TimelineWindow.instance.state.selection.Clear();
				timelineClip.timeScale = 1.0;
				if (!float.IsPositiveInfinity(mousePosition.x) && !float.IsPositiveInfinity(mousePosition.y))
				{
					timelineClip.start = (double)state.ScreenSpacePixelToTimeAreaTime(mousePosition.x);
				}
				else
				{
					timelineClip.start = state.SnapToFrameIfRequired(end);
				}
				timelineClip.start = Math.Max(0.0, timelineClip.start);
				timelineClip.m_ID = state.timeline.GenerateNewId();
				timelineClip.mixInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
				timelineClip.mixOutCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
				timelineClip.selected = true;
				TimelineWindow.instance.state.selection.SelectInEditor(EditorClip.CreateEditorClip(state.timeline, timelineClip));
				Extrapolation.CalculateExtrapolationTimes(parentTrack);
				state.Refresh();
			}
			return timelineClip;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000083FC File Offset: 0x000067FC
		public static TimelineClip CreateClipOnTrack(Type playableAssetType, TrackAsset parentTrack, ITimelineState state)
		{
			return TimelineHelpers.CreateClipOnTrack(playableAssetType, parentTrack, state, TimelineHelpers.InvalidMousePosition);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00008420 File Offset: 0x00006820
		public static TimelineClip CreateClipOnTrack(Type playableAssetType, TrackAsset parentTrack, ITimelineState state, Vector2 mousePosition)
		{
			TimelineClip result;
			if (!typeof(IPlayableAsset).IsAssignableFrom(playableAssetType) || !typeof(ScriptableObject).IsAssignableFrom(playableAssetType))
			{
				result = null;
			}
			else
			{
				ScriptableObject scriptableObject = ScriptableObject.CreateInstance(playableAssetType);
				if (scriptableObject == null)
				{
					throw new InvalidOperationException("Could not create an instance of the ScriptableObject type " + playableAssetType.Name);
				}
				scriptableObject.name = playableAssetType.Name;
				TimelineHelpers.SaveAssetIntoObject(scriptableObject, parentTrack);
				result = TimelineHelpers.CreateClipOnTrack(scriptableObject, parentTrack, state, mousePosition);
			}
			return result;
		}

		// Token: 0x04000103 RID: 259
		public static readonly TrackType GroupTrackType = new TrackType(typeof(GroupTrack), 5);

		// Token: 0x04000104 RID: 260
		private static TrackType[] cachedMixableTypes;

		// Token: 0x04000105 RID: 261
		private static Type[] standaloneAssetTypes;

		// Token: 0x04000106 RID: 262
		public static Vector2 InvalidMousePosition = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x0400010E RID: 270
		[CompilerGenerated]
		private static Func<TrackType, IEnumerable<Type>> <>f__mg$cache0;
	}
}
