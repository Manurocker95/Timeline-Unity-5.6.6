using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009E RID: 158
	[Category("Editing/")]
	[DisplayName("Reset Editing")]
	internal class ResetClip : ClipAction
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x0002A8E0 File Offset: 0x00028CE0
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.ResetEditing(clips);
		}
	}
}
