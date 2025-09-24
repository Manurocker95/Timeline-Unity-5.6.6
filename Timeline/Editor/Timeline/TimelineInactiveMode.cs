using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000DD RID: 221
	internal class TimelineInactiveMode : TimelineMode
	{
		// Token: 0x0600083F RID: 2111 RVA: 0x000366F8 File Offset: 0x00034AF8
		public TimelineInactiveMode()
		{
			base.headerState = new TimelineMode.HeaderState
			{
				breadCrumb = TimelineModeGUIState.Disabled,
				options = TimelineModeGUIState.Enabled,
				searchFilter = TimelineModeGUIState.Enabled,
				sequenceSelector = TimelineModeGUIState.Disabled
			};
			base.trackOptionsState = new TimelineMode.TrackOptionsState
			{
				newButton = TimelineModeGUIState.Disabled,
				editAsAssetButton = TimelineModeGUIState.Enabled
			};
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0003675C File Offset: 0x00034B5C
		public override bool ShouldShowPlayRange(TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00036774 File Offset: 0x00034B74
		public override bool ShouldShowTimeCursor(TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0003678C File Offset: 0x00034B8C
		public override TimelineModeGUIState ToolbarState(TimelineWindow.TimelineState state)
		{
			return TimelineModeGUIState.Disabled;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000367A4 File Offset: 0x00034BA4
		public override TimelineModeGUIState TrackState(TimelineWindow.TimelineState state)
		{
			return TimelineModeGUIState.Disabled;
		}
	}
}
