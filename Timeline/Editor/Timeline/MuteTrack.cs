using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C2 RID: 194
	[DisplayName("Mute")]
	[Shortcut(109)]
	internal class MuteTrack : ToggleTrackAction
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x0002D450 File Offset: 0x0002B850
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			bool flag = (from x in tracks
			where x is GroupTrack
			select x).Any<TrackAsset>();
			MenuActionDisplayState result;
			if (flag)
			{
				result = MenuActionDisplayState.Hidden;
			}
			else
			{
				bool flag2 = (from x in tracks
				where !x.muted
				select x).Count<TrackAsset>() > 0;
				if (flag2)
				{
					result = MenuActionDisplayState.Visible;
				}
				else
				{
					result = MenuActionDisplayState.Hidden;
				}
			}
			return result;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002D4D8 File Offset: 0x0002B8D8
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			base.ToggleMute(state, tracks);
			return true;
		}
	}
}
