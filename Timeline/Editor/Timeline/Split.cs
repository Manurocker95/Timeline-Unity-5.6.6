using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009D RID: 157
	[Category("Editing/")]
	[Shortcut(115)]
	internal class Split : ClipAction
	{
		// Token: 0x060005DB RID: 1499 RVA: 0x0002A8A0 File Offset: 0x00028CA0
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			bool result = ClipModifier.Split(state.currentDirector, state.timeline, state.time, clips);
			state.Refresh();
			return result;
		}
	}
}
