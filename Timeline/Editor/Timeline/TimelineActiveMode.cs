using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000DB RID: 219
	internal class TimelineActiveMode : TimelineMode
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x00036630 File Offset: 0x00034A30
		public TimelineActiveMode()
		{
			base.headerState = new TimelineMode.HeaderState
			{
				breadCrumb = TimelineModeGUIState.Enabled,
				options = TimelineModeGUIState.Enabled,
				searchFilter = TimelineModeGUIState.Enabled,
				sequenceSelector = TimelineModeGUIState.Enabled
			};
			base.trackOptionsState = new TimelineMode.TrackOptionsState
			{
				newButton = TimelineModeGUIState.Enabled,
				editAsAssetButton = TimelineModeGUIState.Hidden
			};
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00036694 File Offset: 0x00034A94
		public override bool ShouldShowTimeCursor(TimelineWindow.TimelineState state)
		{
			return true;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000366AC File Offset: 0x00034AAC
		public override bool ShouldShowPlayRange(TimelineWindow.TimelineState state)
		{
			return state.playRangeEnabled;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000366C8 File Offset: 0x00034AC8
		public override TimelineModeGUIState ToolbarState(TimelineWindow.TimelineState state)
		{
			return TimelineModeGUIState.Enabled;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000366E0 File Offset: 0x00034AE0
		public override TimelineModeGUIState TrackState(TimelineWindow.TimelineState state)
		{
			return TimelineModeGUIState.Enabled;
		}
	}
}
