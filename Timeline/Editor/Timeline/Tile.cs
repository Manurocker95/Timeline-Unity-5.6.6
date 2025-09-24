using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A5 RID: 165
	internal class Tile : ClipAction
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0002ABCC File Offset: 0x00028FCC
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return (clips.Length <= 1) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002ABF4 File Offset: 0x00028FF4
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.Tile(clips);
		}
	}
}
