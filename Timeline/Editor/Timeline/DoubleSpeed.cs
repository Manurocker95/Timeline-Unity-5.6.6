using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A1 RID: 161
	[Category("Speed/")]
	[DisplayName("Double Speed")]
	internal class DoubleSpeed : ClipAction
	{
		// Token: 0x060005E5 RID: 1509 RVA: 0x0002A9DC File Offset: 0x00028DDC
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			bool flag = clips.All((TimelineClip x) => TimelineClipCapsExtensions.SupportsSpeedMultiplier(x));
			return (!flag) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002AA24 File Offset: 0x00028E24
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.DoubleSpeed(clips);
		}
	}
}
