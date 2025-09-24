using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BF RID: 191
	internal abstract class ToggleTrackAction : TrackAction
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x0002D208 File Offset: 0x0002B608
		protected void ToggleLock(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			if (tracks.Length != 0)
			{
				TimelineHelpers.PushUndo(tracks[0], "track.lock");
				foreach (TrackAsset trackAsset in tracks)
				{
					trackAsset.locked = !trackAsset.locked;
				}
				state.Refresh(true);
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002D264 File Offset: 0x0002B664
		protected void ToggleMute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			if (tracks.Length != 0)
			{
				TimelineHelpers.PushUndo(tracks[0], "track.mute");
				foreach (TrackAsset trackAsset in tracks)
				{
					trackAsset.muted = !trackAsset.muted;
				}
				state.Refresh(true);
			}
		}
	}
}
