using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C3 RID: 195
	[DisplayName("UnMute")]
	[Shortcut(109)]
	internal class UnMuteTrack : ToggleTrackAction
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x0002D540 File Offset: 0x0002B940
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			bool flag = (from x in tracks
			where !x.muted
			select x).Count<TrackAsset>() > 0;
			MenuActionDisplayState result;
			if (flag)
			{
				result = MenuActionDisplayState.Hidden;
			}
			else
			{
				result = MenuActionDisplayState.Visible;
			}
			return result;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0002D590 File Offset: 0x0002B990
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			base.ToggleMute(state, tracks);
			return true;
		}
	}
}
