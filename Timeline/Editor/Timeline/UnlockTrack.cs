using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C1 RID: 193
	[DisplayName("Unlock")]
	[Shortcut(108)]
	internal class UnlockTrack : ToggleTrackAction
	{
		// Token: 0x06000682 RID: 1666 RVA: 0x0002D3B8 File Offset: 0x0002B7B8
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			bool flag = (from x in tracks
			where !x.locked
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

		// Token: 0x06000683 RID: 1667 RVA: 0x0002D408 File Offset: 0x0002B808
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			base.ToggleLock(state, tracks);
			return true;
		}
	}
}
