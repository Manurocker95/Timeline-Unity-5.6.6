using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000097 RID: 151
	internal class ClipModifier
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x000293BC File Offset: 0x000277BC
		private static void DeleteRecordedAnimation(TimelineAsset timeline, TimelineClip clip)
		{
			if (clip != null && clip.recordable)
			{
				AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
				if (!(animationPlayableAsset == null) && !(animationPlayableAsset.clip == null))
				{
					TimelineHelpers.PushDestroyUndo(timeline, animationPlayableAsset, animationPlayableAsset.clip, "delete.recording");
				}
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00029420 File Offset: 0x00027820
		public static bool Delete(TimelineAsset timeline, TimelineClip clip)
		{
			TimelineHelpers.PushUndo(clip.parentTrack, "delete.clip");
			if (clip.curves != null)
			{
				TimelineHelpers.PushDestroyUndo(timeline, clip.parentTrack, clip.curves, "delete.curves");
			}
			if (clip.asset != null)
			{
				string assetPath = AssetDatabase.GetAssetPath(clip.asset);
				if (assetPath == AssetDatabase.GetAssetPath(timeline))
				{
					ClipModifier.DeleteRecordedAnimation(timeline, clip);
					TimelineHelpers.PushDestroyUndo(timeline, clip.parentTrack, clip.asset, "delete.clip.asset");
				}
			}
			TrackAsset parentTrack = clip.parentTrack;
			parentTrack.RemoveClip(clip);
			Extrapolation.CalculateExtrapolationTimes(parentTrack);
			return true;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000294D4 File Offset: 0x000278D4
		public static bool Delete(TimelineAsset timeline, TimelineClip[] clips)
		{
			foreach (TimelineClip clip in clips)
			{
				ClipModifier.Delete(timeline, clip);
			}
			return true;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00029510 File Offset: 0x00027910
		private static void CreateTrackCompound(PlayableDirector directorComponent, TimelineAsset timeline, TimelineClip[] clips)
		{
			string text = "Create Compound";
			TrackAsset parentTrack = clips[0].parentTrack;
			TimelineHelpers.PushUndo(parentTrack, text);
			TimelineAsset timelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();
			timelineAsset.name = clips[0].displayName + text;
			TimelineHelpers.SaveAssetIntoObject(timelineAsset, timeline);
			TrackAsset trackAsset = timelineAsset.CreateTrack(timelineAsset, parentTrack.name, TimelineHelpers.TrackTypeFromType(parentTrack.GetType()));
			double num = clips.Min((TimelineClip t) => t.start);
			for (int i = 0; i < clips.Length; i++)
			{
				TimelineClip timelineClip = TimelineHelpers.Clone(clips[i], directorComponent, timelineAsset);
				timelineClip.parentTrack = trackAsset;
				timelineClip.start -= num;
				trackAsset.AddClip(timelineClip);
				clips[i].parentTrack.RemoveClip(clips[i]);
			}
			TimelineHelpers.SaveAssetIntoObject(trackAsset, timelineAsset);
			TimelineClip timelineClip2 = parentTrack.CreateClipFromAsset(timelineAsset);
			timelineClip2.start = num;
			parentTrack.SortClips();
			Undo.RegisterCreatedObjectUndo(timelineAsset, text);
			Undo.RegisterCreatedObjectUndo(trackAsset, text);
			Undo.SetCurrentGroupName(text);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00029620 File Offset: 0x00027A20
		public static bool CreateCompound(PlayableDirector directorComponent, TimelineAsset timeline, TimelineClip[] clips)
		{
			IEnumerable<TrackAsset> enumerable = (from t in clips
			select t.parentTrack).Distinct<TrackAsset>();
			using (IEnumerator<TrackAsset> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TrackAsset t = enumerator.Current;
					TimelineClip[] clips2 = (from c in clips
					where c.parentTrack == t
					select c).ToArray<TimelineClip>();
					ClipModifier.CreateTrackCompound(directorComponent, timeline, clips2);
				}
			}
			return true;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000296D0 File Offset: 0x00027AD0
		public static bool Tile(TimelineClip[] clips)
		{
			bool result;
			if (clips.Length < 2)
			{
				result = false;
			}
			else
			{
				IEnumerable<TrackAsset> enumerable = (from x in clips
				select x.parentTrack).Distinct<TrackAsset>();
				foreach (TrackAsset thingToDirty in enumerable)
				{
					TimelineHelpers.PushUndo(thingToDirty, "tile");
				}
				clips = (from x in clips
				orderby x.start + x.duration * TimeUtility.kTimeEpsilon
				select x).ToArray<TimelineClip>();
				double num = clips[0].start + clips[0].duration;
				for (int i = 1; i < clips.Length; i++)
				{
					clips[i].start = num;
					num += clips[i].duration;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000297DC File Offset: 0x00027BDC
		public static bool TrimStart(double trimTime, TimelineClip[] clips)
		{
			bool flag = false;
			foreach (TimelineClip clip in clips)
			{
				flag |= ClipModifier.TrimStart(trimTime, clip);
			}
			return flag;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0002981C File Offset: 0x00027C1C
		public static bool TrimStart(double trimTime, TimelineClip clip)
		{
			bool result;
			if (clip.asset == null)
			{
				result = false;
			}
			else if (clip.start > trimTime)
			{
				result = false;
			}
			else if (clip.start + clip.duration < trimTime)
			{
				result = false;
			}
			else
			{
				TimelineHelpers.PushUndo(clip.parentTrack, "clip.trimStart");
				clip.clipIn = trimTime - clip.start;
				clip.duration -= trimTime - clip.start;
				clip.start = trimTime;
				result = true;
			}
			return result;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000298B4 File Offset: 0x00027CB4
		public static bool TrimEnd(double trimTime, TimelineClip[] clips)
		{
			bool flag = false;
			foreach (TimelineClip clip in clips)
			{
				flag |= ClipModifier.TrimEnd(trimTime, clip);
			}
			return flag;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000298F4 File Offset: 0x00027CF4
		public static bool TrimEnd(double trimTime, TimelineClip clip)
		{
			bool result;
			if (clip.asset == null)
			{
				result = false;
			}
			else if (clip.start > trimTime)
			{
				result = false;
			}
			else if (clip.start + clip.duration < trimTime)
			{
				result = false;
			}
			else
			{
				TimelineHelpers.PushUndo(clip.parentTrack, "clip.trimEnd");
				clip.duration = trimTime - clip.start;
				result = true;
			}
			return result;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00029970 File Offset: 0x00027D70
		public static bool MatchDuration(TimelineClip[] clips)
		{
			double duration = clips[0].duration;
			for (int i = 1; i < clips.Length; i++)
			{
				TimelineHelpers.PushUndo(clips[i].parentTrack, "clip.matchDuration");
				clips[i].duration = duration;
			}
			return true;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000299C0 File Offset: 0x00027DC0
		public static bool Split(PlayableDirector directorComponent, TimelineAsset timeline, double splitTime, TimelineClip[] clips)
		{
			bool flag = false;
			foreach (TimelineClip timelineClip in clips)
			{
				if (timelineClip.start <= splitTime)
				{
					if (timelineClip.start + timelineClip.duration >= splitTime)
					{
						TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.split");
						double duration = timelineClip.duration;
						timelineClip.duration = splitTime - timelineClip.start;
						TimelineClip timelineClip2 = TimelineHelpers.Clone(timelineClip, directorComponent, timeline);
						timelineClip2.selected = false;
						timelineClip2.start = splitTime;
						timelineClip2.clipIn = timelineClip.duration + timelineClip.clipIn;
						timelineClip2.duration = duration - timelineClip.duration;
						timelineClip.parentTrack.AddClip(timelineClip2);
						flag |= true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00029A98 File Offset: 0x00027E98
		public static bool ResetEditing(TimelineClip[] clips)
		{
			bool flag = false;
			foreach (TimelineClip clip in clips)
			{
				flag |= ClipModifier.ResetEditing(clip);
			}
			return flag;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00029AD8 File Offset: 0x00027ED8
		public static bool ResetEditing(TimelineClip clip)
		{
			bool result;
			if (clip.asset == null)
			{
				result = false;
			}
			else
			{
				TimelineHelpers.PushUndo(clip.parentTrack, "clip.resetEditing");
				if (clip.clipAssetDuration < 1.7976931348623157E+308)
				{
					clip.duration = clip.clipAssetDuration / clip.timeScale;
				}
				clip.start -= clip.clipIn;
				clip.clipIn = 0.0;
				if (clip.start < 0.0)
				{
					clip.start = 0.0;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00029B84 File Offset: 0x00027F84
		public static bool CompleteLastLoop(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (TimelineHelpers.HasUsableAssetDuration(timelineClip))
				{
					double[] loopTimes = TimelineHelpers.GetLoopTimes(timelineClip);
					double loopDuration = TimelineHelpers.GetLoopDuration(timelineClip);
					TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.completeLastLoop");
					timelineClip.duration = timelineClip.start + loopTimes.LastOrDefault<double>() + loopDuration;
				}
			}
			return true;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00029C00 File Offset: 0x00028000
		public static bool CompleteLastLoop(TimelineClip clip)
		{
			TimelineClip[] clips = new TimelineClip[]
			{
				clip
			};
			return ClipModifier.CompleteLastLoop(clips);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00029C28 File Offset: 0x00028028
		public static bool TrimLastLoop(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (TimelineHelpers.HasUsableAssetDuration(timelineClip))
				{
					double[] loopTimes = TimelineHelpers.GetLoopTimes(timelineClip);
					double loopDuration = TimelineHelpers.GetLoopDuration(timelineClip);
					double num = timelineClip.duration - loopTimes.FirstOrDefault<double>();
					if (loopDuration > 0.0)
					{
						num = (timelineClip.duration - loopTimes.FirstOrDefault<double>()) / loopDuration;
					}
					int num2 = Mathf.FloorToInt((float)num);
					if (num2 > 0)
					{
						TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.trimLastLoop");
						timelineClip.duration = loopTimes.FirstOrDefault<double>() + (double)num2 * loopDuration;
					}
				}
			}
			return true;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00029CE4 File Offset: 0x000280E4
		public static bool TrimLastLoop(TimelineClip clip)
		{
			TimelineClip[] clips = new TimelineClip[]
			{
				clip
			};
			return ClipModifier.TrimLastLoop(clips);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00029D0C File Offset: 0x0002810C
		public static bool DoubleSpeed(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (TimelineClipCapsExtensions.SupportsSpeedMultiplier(timelineClip))
				{
					TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.doubleSpeed");
					timelineClip.timeScale *= 2.0;
					timelineClip.duration *= 0.5;
				}
			}
			return true;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00029D88 File Offset: 0x00028188
		public static bool HalfSpeed(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (TimelineClipCapsExtensions.SupportsSpeedMultiplier(timelineClip))
				{
					TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.halfSpeed");
					timelineClip.timeScale *= 0.5;
					timelineClip.duration *= 2.0;
				}
			}
			return true;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00029E04 File Offset: 0x00028204
		public static bool ResetSpeed(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				if (timelineClip.timeScale != 1.0)
				{
					TimelineHelpers.PushUndo(timelineClip.parentTrack, "clip.resetSpeed");
					timelineClip.duration *= timelineClip.timeScale;
					timelineClip.timeScale = 1.0;
				}
			}
			return true;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00029E80 File Offset: 0x00028280
		public static TimelineClip DuplicateClip(PlayableDirector directorComponent, TimelineClip clip)
		{
			return clip.Duplicate(directorComponent);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00029E9C File Offset: 0x0002829C
		public static bool DuplicateClips(PlayableDirector directorComponent, TimelineClip[] clips)
		{
			foreach (TimelineClip clip in clips)
			{
				ClipModifier.DuplicateClip(directorComponent, clip);
			}
			return true;
		}
	}
}
