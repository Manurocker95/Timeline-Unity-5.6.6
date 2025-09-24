using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BA RID: 186
	[HideInMenu]
	[Shortcut(32)]
	internal class PlayTimelineAction : TimelineAction
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x0002C924 File Offset: 0x0002AD24
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			bool playing = state.playing;
			TimelineWindow.instance.Simulate(!playing);
			state.playing = !playing;
			return true;
		}
	}
}
