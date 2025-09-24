using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Timeline;

namespace UnityEngine.Timeline
{
	// Token: 0x02000038 RID: 56
	internal static class AnimationTrackExtensions
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00012290 File Offset: 0x00010690
		public static void ConvertToClipMode(this AnimationTrack track)
		{
			if (track.CanConvertToClipMode())
			{
				TimelineHelpers.PushUndo(track, "convert.to.clip");
				if (!track.animClip.empty)
				{
					float num = AnimationClipCurveCache.Instance.GetCurveInfo(track.animClip).keyTimes.FirstOrDefault<float>();
					track.animClip.ShiftBySeconds(-num);
					TimelineClip timelineClip = track.CreateClipFromAsset(track.animClip);
					TimelineHelpers.SaveAssetIntoObject(timelineClip.asset, track);
					timelineClip.start = (double)num;
					timelineClip.preExtrapolationMode = track.openClipPreExtrapolation;
					timelineClip.postExtrapolationMode = track.openClipPostExtrapolation;
					timelineClip.recordable = true;
					AnimationPlayableAsset animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
					if (animationPlayableAsset)
					{
						animationPlayableAsset.position = track.openClipOffsetPosition;
						animationPlayableAsset.rotation = track.openClipOffsetRotation;
						track.openClipOffsetPosition = Vector3.zero;
						track.openClipOffsetRotation = Quaternion.identity;
					}
					Extrapolation.CalculateExtrapolationTimes(track);
				}
				track.animClip = null;
				EditorUtility.SetDirty(track);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00012390 File Offset: 0x00010790
		public static void ConvertFromClipMode(this AnimationTrack track, TimelineAsset timeline)
		{
			if (track.CanConvertFromClipMode())
			{
				TimelineHelpers.PushUndo(track, "convert.from.clip");
				TimelineClip timelineClip = track.clips[0];
				float time = (float)timelineClip.start;
				track.openClipTimeOffset = 0.0;
				track.openClipPreExtrapolation = timelineClip.preExtrapolationMode;
				track.openClipPostExtrapolation = timelineClip.postExtrapolationMode;
				AnimationPlayableAsset animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
				if (animationPlayableAsset)
				{
					track.openClipOffsetPosition = animationPlayableAsset.position;
					track.openClipOffsetRotation = animationPlayableAsset.rotation;
				}
				AnimationClip animationClip = timelineClip.animationClip;
				float num = (float)timelineClip.timeScale;
				if (!Mathf.Approximately(num, 1f))
				{
					if (!Mathf.Approximately(num, 0f))
					{
						num = 1f / num;
					}
					animationClip.ScaleTime(num);
				}
				animationClip.ShiftBySeconds(time);
				Object asset = timelineClip.asset;
				timelineClip.asset = null;
				ClipModifier.Delete(timeline, timelineClip);
				TimelineHelpers.PushDestroyUndo(null, track, asset, "convert.from.clip");
				track.animClip = animationClip;
				EditorUtility.SetDirty(track);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000124A4 File Offset: 0x000108A4
		public static bool CanConvertToClipMode(this AnimationTrack track)
		{
			return !(track == null) && !track.inClipMode && (track.animClip != null && !track.animClip.empty) && AnimationTrackExtensions.ActualLength(track.animClip) > 0f;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00012510 File Offset: 0x00010910
		public static bool CanConvertFromClipMode(this AnimationTrack track)
		{
			bool result;
			if (track == null || !track.inClipMode || track.clips.Length != 1 || track.clips[0].start < 0.0 || !track.clips[0].recordable)
			{
				result = false;
			}
			else
			{
				AnimationPlayableAsset animationPlayableAsset = track.clips[0].asset as AnimationPlayableAsset;
				result = (!(animationPlayableAsset == null) && TimelineHelpers.HaveSameContainerAsset(track, animationPlayableAsset.clip));
			}
			return result;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000125B0 File Offset: 0x000109B0
		public static bool ShouldShowInfiniteClipEditor(this AnimationTrack track)
		{
			return track != null && !track.inClipMode && track.animClip != null;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000125EC File Offset: 0x000109EC
		private static float ActualLength(AnimationClip clip)
		{
			AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
			float num = curveInfo.keyTimes.FirstOrDefault<float>();
			float num2 = curveInfo.keyTimes.LastOrDefault<float>();
			return num2 - num;
		}
	}
}
