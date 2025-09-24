using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200003B RID: 59
	internal static class TrackExtensions
	{
		// Token: 0x0600020B RID: 523 RVA: 0x00012A54 File Offset: 0x00010E54
		public static AnimationClip GetOrCreateClip(this TrackAsset track)
		{
			bool flag = false;
			AnimationTrack animationTrack = track as AnimationTrack;
			if (animationTrack != null)
			{
				flag = animationTrack.inClipMode;
			}
			if (track.animClip == null && !flag)
			{
				track.animClip = new AnimationClip();
				track.animClip.name = AnimationTrackRecorder.GetUniqueRecordedClipName(track, AnimationTrackRecorder.kRecordClipDefaultName);
				Undo.RegisterCreatedObjectUndo(track.animClip, "Create Track");
				AnimationUtility.SetGenerateMotionCurves(track.animClip, true);
				TimelineHelpers.SaveAnimClipIntoObject(track.animClip, track);
			}
			return track.animClip;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00012AF0 File Offset: 0x00010EF0
		public static TimelineClip CreateClip(this TrackAsset track, double time)
		{
			object[] customAttributes = track.GetType().GetCustomAttributes(typeof(TrackClipTypeAttribute), true);
			TimelineClip result;
			if (customAttributes.Length == 0)
			{
				result = null;
			}
			else if (TimelineWindow.instance.state == null)
			{
				result = null;
			}
			else if (customAttributes.Length == 1)
			{
				TrackClipTypeAttribute trackClipTypeAttribute = customAttributes[0] as TrackClipTypeAttribute;
				TimelineClip timelineClip = TimelineHelpers.CreateClipOnTrack(trackClipTypeAttribute.inspectedType, track, TimelineWindow.instance.state, TimelineHelpers.InvalidMousePosition);
				timelineClip.start = time;
				result = timelineClip;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00012B80 File Offset: 0x00010F80
		private static bool Overlaps(TimelineClip blendOut, TimelineClip blendIn)
		{
			bool result;
			if (blendIn == blendOut)
			{
				result = false;
			}
			else if (Math.Abs(blendIn.start - blendOut.start) < TimeUtility.kTimeEpsilon)
			{
				result = (blendIn.duration > blendOut.duration);
			}
			else
			{
				result = (blendIn.start >= blendOut.start && blendIn.start < blendOut.end);
			}
			return result;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00012BF6 File Offset: 0x00010FF6
		public static void ComputeBlendsFromOverlaps(this TrackAsset asset)
		{
			TrackExtensions.ComputeBlendsFromOverlaps(asset.clips);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00012C04 File Offset: 0x00011004
		internal static void ComputeBlendsFromOverlaps(TimelineClip[] clips)
		{
			foreach (TimelineClip timelineClip in clips)
			{
				timelineClip.blendInDuration = -1.0;
				timelineClip.blendOutDuration = -1.0;
			}
			for (int j = 0; j < clips.Length; j++)
			{
				TimelineClip blendIn2 = clips[j];
				TimelineClip blendIn = blendIn2;
				TimelineClip timelineClip2 = (from c in clips
				where TrackExtensions.Overlaps(c, blendIn)
				orderby c.start
				select c).FirstOrDefault<TimelineClip>();
				if (timelineClip2 != null)
				{
					TrackExtensions.UpdateClipIntersection(timelineClip2, blendIn);
				}
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00012CD4 File Offset: 0x000110D4
		internal static void UpdateClipIntersection(TimelineClip blendOutClip, TimelineClip blendInClip)
		{
			if (TimelineClipCapsExtensions.SupportsBlending(blendOutClip) && TimelineClipCapsExtensions.SupportsBlending(blendInClip))
			{
				double num = Math.Max(0.0, blendOutClip.start + blendOutClip.duration - blendInClip.start);
				num = ((num > TrackExtensions.kMinOverlapTime) ? num : 0.0);
				blendOutClip.blendOutDuration = num;
				blendInClip.blendInDuration = num;
				TimelineClip.BlendCurveMode blendInCurveMode = blendInClip.blendInCurveMode;
				TimelineClip.BlendCurveMode blendOutCurveMode = blendOutClip.blendOutCurveMode;
				if (blendInCurveMode == 1 && blendOutCurveMode == null)
				{
					blendOutClip.mixOutCurve = CurveEditUtility.CreateMatchingCurve(blendInClip.mixInCurve);
				}
				else if (blendInCurveMode == null && blendOutCurveMode == 1)
				{
					blendInClip.mixInCurve = CurveEditUtility.CreateMatchingCurve(blendOutClip.mixOutCurve);
				}
				else if (blendInCurveMode == null && blendOutCurveMode == null)
				{
					blendInClip.mixInCurve = null;
					blendOutClip.mixOutCurve = null;
				}
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00012DBD File Offset: 0x000111BD
		internal static void UpdateClipIntersectionNoOverlap(TimelineClip blendOutClip, TimelineClip blendInClip)
		{
			blendInClip.start = blendOutClip.end;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00012DCC File Offset: 0x000111CC
		internal static bool MoveClipToTrack(TimelineClip clip, TrackAsset track)
		{
			bool result;
			if (clip == null || track == null || clip.parentTrack == track)
			{
				result = false;
			}
			else
			{
				TimelineHelpers.PushUndo(clip.parentTrack, "clip.move");
				TimelineHelpers.PushUndo(track, "clip.move");
				AnimationTrack animationTrack = track as AnimationTrack;
				if (animationTrack != null)
				{
					animationTrack.ConvertToClipMode();
				}
				TrackAsset parentTrack = clip.parentTrack;
				clip.parentTrack = track;
				Extrapolation.CalculateExtrapolationTimes(parentTrack);
				result = true;
			}
			return result;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00012E58 File Offset: 0x00011258
		internal static void RecursiveSubtrackClone(TrackAsset source, TrackAsset duplicate, PlayableDirector director)
		{
			List<TrackAsset> subTracks = source.subTracks;
			foreach (TrackAsset trackAsset in subTracks)
			{
				TrackAsset trackAsset2 = TimelineHelpers.Clone(trackAsset, director);
				duplicate.AddChild(trackAsset2);
				trackAsset2.parent = duplicate;
				TrackExtensions.RecursiveSubtrackClone(trackAsset, trackAsset2, director);
				Undo.RegisterCreatedObjectUndo(trackAsset2, "sequence.duplicate");
				TimelineHelpers.SaveAssetIntoObject(trackAsset2, source);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00012EE4 File Offset: 0x000112E4
		internal static bool Duplicate(this TrackAsset track, PlayableDirector director)
		{
			bool result;
			if (track == null)
			{
				result = false;
			}
			else
			{
				TimelineAsset timelineAsset = track.parent as TimelineAsset;
				TrackAsset trackAsset = track.parent as TrackAsset;
				if (timelineAsset == null && trackAsset == null)
				{
					Debug.LogWarning("Cannot duplicate track because it is not parented to known type");
					result = false;
				}
				else
				{
					TrackAsset trackAsset2 = TimelineHelpers.Clone(track, director);
					TrackExtensions.RecursiveSubtrackClone(track, trackAsset2, director);
					Undo.RegisterCreatedObjectUndo(trackAsset2, "sequence.duplicate");
					TimelineHelpers.SaveAssetIntoObject(trackAsset2, track.parent);
					TimelineHelpers.PushUndo(track.parent, "sequence.duplicate");
					if (timelineAsset != null)
					{
						timelineAsset.AddTrackAfter(trackAsset2, track);
					}
					else
					{
						trackAsset.AddChildAfter(trackAsset2, track);
					}
					trackAsset2.parent = track.parent;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00012FB4 File Offset: 0x000113B4
		internal static bool ReparentTracks(TrackAsset[] tracksToMove, PlayableAsset targetParent, TrackAsset insertMarker, bool insertBefore)
		{
			TrackAsset trackAsset = targetParent as TrackAsset;
			TimelineAsset timelineAsset = targetParent as TimelineAsset;
			bool result;
			if (tracksToMove == null || tracksToMove.Length == 0 || (trackAsset == null && timelineAsset == null))
			{
				result = false;
			}
			else
			{
				List<TrackAsset> list = (from x in tracksToMove
				where x.parent != targetParent
				select x).ToList<TrackAsset>();
				if (insertMarker == null && !list.Any<TrackAsset>())
				{
					result = false;
				}
				else
				{
					List<PlayableAsset> list2 = (from x in list
					select x.parent into x
					where x != null
					select x).Distinct<PlayableAsset>().ToList<PlayableAsset>();
					TimelineHelpers.PushUndo(targetParent, "reparent");
					foreach (PlayableAsset thingToDirty in list2)
					{
						TimelineHelpers.PushUndo(thingToDirty, "reparent");
					}
					foreach (TrackAsset thingToDirty2 in list)
					{
						TimelineHelpers.PushUndo(thingToDirty2, "reparent");
					}
					foreach (TrackAsset trackAsset2 in list)
					{
						if (trackAsset2.parent != targetParent)
						{
							TrackAsset trackAsset3 = trackAsset2.parent as TrackAsset;
							TimelineAsset timelineAsset2 = trackAsset2.parent as TimelineAsset;
							if (timelineAsset2 != null)
							{
								timelineAsset2.RemoveTrack(trackAsset2);
							}
							else if (trackAsset3 != null)
							{
								trackAsset3.RemoveSubTrack(trackAsset2);
							}
							if (trackAsset != null)
							{
								trackAsset.AddChild(trackAsset2);
								trackAsset.collapsed = false;
							}
							else
							{
								timelineAsset.AddTrack(trackAsset2);
							}
						}
					}
					if (insertMarker != null)
					{
						List<TrackAsset> allTracks = (!(trackAsset != null)) ? timelineAsset.tracks : trackAsset.subTracks;
						TimelineUtility.ReorderTracks(allTracks, tracksToMove.ToList<TrackAsset>(), insertMarker, insertBefore);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00013278 File Offset: 0x00011678
		internal static Type GetCustomPlayableType(this TrackAsset track)
		{
			Type result;
			if (track == null)
			{
				result = null;
			}
			else
			{
				TrackClipTypeAttribute trackClipTypeAttribute = Attribute.GetCustomAttribute(track.GetType(), typeof(TrackClipTypeAttribute)) as TrackClipTypeAttribute;
				if (trackClipTypeAttribute != null)
				{
					result = trackClipTypeAttribute.inspectedType;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0400019B RID: 411
		public static readonly double kMinOverlapTime = TimeUtility.kTimeEpsilon * 1000.0;
	}
}
