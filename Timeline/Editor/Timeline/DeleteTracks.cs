using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C7 RID: 199
	[HideInMenu]
	internal class DeleteTracks : TrackAction
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x0002D829 File Offset: 0x0002BC29
		public static void Do(TimelineAsset timeline, TrackAsset track)
		{
			TrackModifier.DeleteTrack(timeline, track);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002D834 File Offset: 0x0002BC34
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			foreach (TrackAsset track in tracks)
			{
				DeleteTracks.Do(state.timeline, track);
			}
			state.Refresh();
			return true;
		}
	}
}
