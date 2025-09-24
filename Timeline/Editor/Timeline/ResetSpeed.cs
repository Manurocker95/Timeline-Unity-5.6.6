using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A3 RID: 163
	[Category("Speed/")]
	[DisplayName("Reset Speed")]
	internal class ResetSpeed : ClipAction
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0002AAEC File Offset: 0x00028EEC
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			bool flag = clips.All((TimelineClip x) => TimelineClipCapsExtensions.SupportsSpeedMultiplier(x));
			return (!flag) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002AB34 File Offset: 0x00028F34
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.ResetSpeed(clips);
		}
	}
}
