using System;
using System.ComponentModel;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A6 RID: 166
	[DisplayName("Open Compound")]
	internal class OpenCompound : ClipAction
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x0002AC18 File Offset: 0x00029018
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			MenuActionDisplayState result;
			if (clips.Length == 0)
			{
				result = MenuActionDisplayState.Hidden;
			}
			else
			{
				result = ((!clips[0].isNestedAsset) ? MenuActionDisplayState.Hidden : MenuActionDisplayState.Visible);
			}
			return result;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0002AC50 File Offset: 0x00029050
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			state.BreadcrumbDrillInto(clips[0].asset as PlayableAsset, clips[0]);
			return true;
		}
	}
}
