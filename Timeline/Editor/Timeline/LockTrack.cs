using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C0 RID: 192
	[DisplayName("Lock")]
	[Shortcut(108)]
	internal class LockTrack : ToggleTrackAction
	{
		// Token: 0x0600067D RID: 1661 RVA: 0x0002D2C8 File Offset: 0x0002B6C8
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
				where !x.locked
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

		// Token: 0x0600067E RID: 1662 RVA: 0x0002D350 File Offset: 0x0002B750
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			base.ToggleLock(state, tracks);
			return true;
		}
	}
}
