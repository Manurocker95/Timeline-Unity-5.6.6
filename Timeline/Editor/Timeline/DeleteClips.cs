using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A8 RID: 168
	[HideInMenu]
	internal class DeleteClips : ClipAction
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x0002AD28 File Offset: 0x00029128
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			state.Stop();
			ClipModifier.Delete(state.timeline, clips);
			state.selection.Clear();
			state.Refresh(true);
			return true;
		}
	}
}
