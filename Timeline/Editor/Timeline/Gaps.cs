using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000014 RID: 20
	internal static class Gaps
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00009F24 File Offset: 0x00008324
		public static void Insert(TimelineAsset asset, double at, double amount, float tolerance)
		{
			IEnumerable<TimelineClip> enumerable = from x in asset.flattenedTracks.SelectMany((TrackAsset x) => x.clips)
			where x.start - at >= (double)(-(double)tolerance)
			select x;
			IEnumerable<TrackAsset> enumerable2 = (from x in enumerable
			select x.parentTrack).Distinct<TrackAsset>();
			foreach (TrackAsset thingToDirty in enumerable2)
			{
				TimelineHelpers.PushUndo(thingToDirty, "insert.time");
			}
			foreach (TimelineClip timelineClip in enumerable)
			{
				timelineClip.start += amount;
			}
		}
	}
}
