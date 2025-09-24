using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009A RID: 154
	[Category("Editing/")]
	[DisplayName("Trim Start")]
	[Shortcut(105)]
	internal class TrimStart : ClipAction
	{
		// Token: 0x060005D4 RID: 1492 RVA: 0x0002A7FC File Offset: 0x00028BFC
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.TrimStart(state.time, clips);
		}
	}
}
