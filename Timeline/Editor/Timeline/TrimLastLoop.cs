using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A0 RID: 160
	[Category("Editing/")]
	[DisplayName("Trim Last Loop")]
	internal class TrimLastLoop : ClipAction
	{
		// Token: 0x060005E2 RID: 1506 RVA: 0x0002A970 File Offset: 0x00028D70
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			if (TrimLastLoop.<>f__mg$cache0 == null)
			{
				TrimLastLoop.<>f__mg$cache0 = new Func<TimelineClip, bool>(TimelineHelpers.HasUsableAssetDuration);
			}
			bool flag = clips.Any(TrimLastLoop.<>f__mg$cache0);
			return (!flag) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002A9B8 File Offset: 0x00028DB8
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.TrimLastLoop(clips);
		}

		// Token: 0x04000383 RID: 899
		[CompilerGenerated]
		private static Func<TimelineClip, bool> <>f__mg$cache0;
	}
}
