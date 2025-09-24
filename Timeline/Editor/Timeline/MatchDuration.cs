using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009C RID: 156
	[Category("Editing/")]
	[DisplayName("Match Duration")]
	internal class MatchDuration : ClipAction
	{
		// Token: 0x060005D8 RID: 1496 RVA: 0x0002A854 File Offset: 0x00028C54
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return (clips.Length <= 1) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0002A87C File Offset: 0x00028C7C
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.MatchDuration(clips);
		}
	}
}
