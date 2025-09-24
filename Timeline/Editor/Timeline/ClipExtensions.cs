using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000039 RID: 57
	internal static class ClipExtensions
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00012628 File Offset: 0x00010A28
		public static double FindClipInsertionTime(TimelineClip clip, TrackAsset track)
		{
			return ClipExtensions.FindClipInsertionTime(clip, track.clips);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0001264C File Offset: 0x00010A4C
		private static double FindClipInsertionTime(TimelineClip clip, IEnumerable<TimelineClip> clips)
		{
			List<TimelineClip> list = (from x in clips
			where x.parentTrack == clip.parentTrack
			where x.start >= clip.start
			orderby x.start
			select x).ToList<TimelineClip>();
			double end;
			if (list.Count == 0)
			{
				end = clip.end;
			}
			else
			{
				int num = list.Count - 1;
				TimelineClip timelineClip = list.Last<TimelineClip>();
				if (num == 0)
				{
					end = timelineClip.end;
				}
				else
				{
					for (int num2 = 0; num2 != list.Count; num2++)
					{
						if (num2 == num)
						{
							return timelineClip.end;
						}
						if (list[num2 + 1].start - list[num2].end >= clip.duration)
						{
							return list[num2].end;
						}
					}
					end = timelineClip.end;
				}
			}
			return end;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00012774 File Offset: 0x00010B74
		public static TimelineClip Duplicate(this TimelineClip clip, PlayableDirector director)
		{
			TrackAsset parentTrack = clip.parentTrack;
			TimelineAsset timelineAsset = parentTrack.timelineAsset;
			TimelineClip result;
			if (parentTrack == null || timelineAsset == null)
			{
				result = null;
			}
			else
			{
				double num = ClipExtensions.FindClipInsertionTime(clip, parentTrack.clips);
				if (double.IsInfinity(num))
				{
					result = null;
				}
				else
				{
					TimelineHelpers.PushUndo(parentTrack, "clone.clip");
					TimelineClip timelineClip = TimelineHelpers.Clone(clip, director, timelineAsset);
					timelineClip.start = num;
					clip.parentTrack.AddClip(timelineClip);
					clip.parentTrack.SortClips();
					TrackExtensions.ComputeBlendsFromOverlaps(clip.parentTrack.clips);
					result = timelineClip;
				}
			}
			return result;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0001281C File Offset: 0x00010C1C
		public static TimelineClip DuplicateAtTime(this TimelineClip clip, TrackAsset track, double time, PlayableDirector director, TimelineAsset timeline)
		{
			TimelineHelpers.PushUndo(track, "clone.clip");
			TimelineClip timelineClip = TimelineHelpers.Clone(clip, director, timeline);
			timelineClip.start = time;
			timelineClip.parentTrack = track;
			track.AddClip(timelineClip);
			track.SortClips();
			TrackExtensions.ComputeBlendsFromOverlaps(track.clips);
			return timelineClip;
		}
	}
}
