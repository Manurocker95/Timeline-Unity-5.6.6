using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200004D RID: 77
	internal static class TrackAssetRecordingExtensions
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00018A60 File Offset: 0x00016E60
		internal static void OnRecordingArmed(this TrackAsset track, PlayableDirector director)
		{
			if (!(track == null))
			{
				AnimationClip animationClip = track.FindRecordingAnimationClipAtTime(director.time);
				if (!(animationClip == null))
				{
					TrackAssetRecordingExtensions.s_ActiveClips[track] = animationClip;
					track.showInlineCurves = true;
				}
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00018AB0 File Offset: 0x00016EB0
		internal static void OnRecordingTimeChanged(this TrackAsset track, PlayableDirector director)
		{
			if (!(track == null))
			{
				AnimationClip animationClip = track.FindRecordingAnimationClipAtTime(director.time);
				AnimationClip activeRecordingAnimationClip = track.GetActiveRecordingAnimationClip();
				if (activeRecordingAnimationClip != animationClip)
				{
					TrackAssetRecordingExtensions.s_ActiveClips[track] = animationClip;
				}
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00018AFD File Offset: 0x00016EFD
		internal static void OnRecordingUnarmed(this TrackAsset track, PlayableDirector director)
		{
			TrackAssetRecordingExtensions.s_ActiveClips.Remove(track);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00018B0C File Offset: 0x00016F0C
		internal static AnimationClip GetActiveRecordingAnimationClip(this TrackAsset track)
		{
			AnimationClip result = null;
			TrackAssetRecordingExtensions.s_ActiveClips.TryGetValue(track, out result);
			return result;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00018B34 File Offset: 0x00016F34
		internal static bool IsRecordingToClip(this TrackAsset track, TimelineClip clip)
		{
			bool result;
			if (track == null || clip == null)
			{
				result = false;
			}
			else
			{
				AnimationClip activeRecordingAnimationClip = track.GetActiveRecordingAnimationClip();
				if (activeRecordingAnimationClip == null)
				{
					result = false;
				}
				else if (activeRecordingAnimationClip == clip.curves)
				{
					result = true;
				}
				else
				{
					AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
					result = (animationPlayableAsset != null && activeRecordingAnimationClip == animationPlayableAsset.clip);
				}
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00018BBC File Offset: 0x00016FBC
		internal static TimelineClip FindRecordingClipAtTime(this TrackAsset track, double time)
		{
			TimelineClip result;
			if (track == null)
			{
				result = null;
			}
			else
			{
				bool flag = track as AnimationTrack != null;
				TimelineClip timelineClip;
				if (flag)
				{
					timelineClip = (from x in track.clips
					where x.recordable && x.start < time + TimeUtility.kTimeEpsilon
					orderby x.start
					select x).LastOrDefault<TimelineClip>();
				}
				else
				{
					timelineClip = (from x in track.clips
					where x.start < time + TimeUtility.kTimeEpsilon && x.HasAnyAnimatableParameters()
					orderby x.start
					select x).LastOrDefault<TimelineClip>();
				}
				result = timelineClip;
			}
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00018C90 File Offset: 0x00017090
		internal static AnimationClip FindRecordingAnimationClipAtTime(this TrackAsset trackAsset, double time)
		{
			AnimationClip result;
			if (trackAsset == null)
			{
				result = null;
			}
			else
			{
				AnimationTrack animationTrack = trackAsset as AnimationTrack;
				if (animationTrack != null && !animationTrack.inClipMode)
				{
					result = animationTrack.animClip;
				}
				else
				{
					TimelineClip timelineClip = trackAsset.FindRecordingClipAtTime(time);
					if (timelineClip != null)
					{
						AnimationPlayableAsset animationPlayableAsset = timelineClip.asset as AnimationPlayableAsset;
						if (animationPlayableAsset != null)
						{
							result = animationPlayableAsset.clip;
						}
						else
						{
							AnimatedParameterExtensions.CreateCurvesIfRequired(timelineClip, null);
							result = timelineClip.curves;
						}
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00018D2A File Offset: 0x0001712A
		internal static void ClearRecordingState()
		{
			TrackAssetRecordingExtensions.s_ActiveClips.Clear();
		}

		// Token: 0x04000218 RID: 536
		private static readonly Dictionary<TrackAsset, AnimationClip> s_ActiveClips = new Dictionary<TrackAsset, AnimationClip>();
	}
}
