using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C6 RID: 198
	[DisplayName("Hide Clip Cascade")]
	[HideInMenu]
	internal class HideCascadeClips : TrackAction
	{
		// Token: 0x06000697 RID: 1687 RVA: 0x0002D77C File Offset: 0x0002BB7C
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			bool flag = tracks.All((TrackAsset x) => !x.displayCascadeClips);
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

		// Token: 0x06000698 RID: 1688 RVA: 0x0002D7C4 File Offset: 0x0002BBC4
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			foreach (TrackAsset trackAsset in tracks)
			{
				trackAsset.displayCascadeClips = false;
			}
			state.Refresh();
			return true;
		}
	}
}
