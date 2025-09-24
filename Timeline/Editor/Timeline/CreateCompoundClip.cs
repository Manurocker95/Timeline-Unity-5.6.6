using System;
using System.ComponentModel;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A4 RID: 164
	[DisplayName("Create Compound Clip")]
	internal class CreateCompoundClip : ClipAction
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x0002AB74 File Offset: 0x00028F74
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return (clips.Length < 2) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002AB9C File Offset: 0x00028F9C
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipModifier.CreateCompound(state.currentDirector, state.timeline, clips);
		}
	}
}
