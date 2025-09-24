using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C5 RID: 197
	[DisplayName("Display Clip Cascade")]
	[HideInMenu]
	internal class DisplayCascadeClips : TrackAction
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x0002D6D0 File Offset: 0x0002BAD0
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			bool flag = tracks.All((TrackAsset x) => x.displayCascadeClips);
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

		// Token: 0x06000694 RID: 1684 RVA: 0x0002D718 File Offset: 0x0002BB18
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			foreach (TrackAsset trackAsset in tracks)
			{
				trackAsset.displayCascadeClips = true;
			}
			state.Refresh();
			return true;
		}
	}
}
