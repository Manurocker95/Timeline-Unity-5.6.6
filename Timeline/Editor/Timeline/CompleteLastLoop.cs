using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200009F RID: 159
	[Category("Editing/")]
	[DisplayName("Complete Last Loop")]
	[SeparatorMenuItem(SeparatorMenuItemPosition.Before)]
	internal class CompleteLastLoop : ClipAction
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0002A904 File Offset: 0x00028D04
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			if (CompleteLastLoop.<>f__mg$cache0 == null)
			{
				CompleteLastLoop.<>f__mg$cache0 = new Func<TimelineClip, bool>(TimelineHelpers.HasUsableAssetDuration);
			}
			bool flag = clips.Any(CompleteLastLoop.<>f__mg$cache0);
			return (!flag) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002A94C File Offset: 0x00028D4C
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.CompleteLastLoop(clips);
		}

		// Token: 0x04000382 RID: 898
		[CompilerGenerated]
		private static Func<TimelineClip, bool> <>f__mg$cache0;
	}
}
