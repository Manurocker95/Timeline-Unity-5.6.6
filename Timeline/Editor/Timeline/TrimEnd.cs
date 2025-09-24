using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009B RID: 155
	[Category("Editing/")]
	[DisplayName("Trim End")]
	[Shortcut(111)]
	internal class TrimEnd : ClipAction
	{
		// Token: 0x060005D6 RID: 1494 RVA: 0x0002A828 File Offset: 0x00028C28
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.TrimEnd(state.time, clips);
		}
	}
}
