using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A2 RID: 162
	[Category("Speed/")]
	[DisplayName("Half Speed")]
	internal class HalfSpeed : ClipAction
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x0002AA64 File Offset: 0x00028E64
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			bool flag = clips.All((TimelineClip x) => TimelineClipCapsExtensions.SupportsSpeedMultiplier(x));
			return (!flag) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002AAAC File Offset: 0x00028EAC
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.HalfSpeed(clips);
		}
	}
}
