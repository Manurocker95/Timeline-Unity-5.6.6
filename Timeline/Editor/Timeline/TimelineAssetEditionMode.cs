using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000DC RID: 220
	internal class TimelineAssetEditionMode : TimelineInactiveMode
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x000367BC File Offset: 0x00034BBC
		public TimelineAssetEditionMode()
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
				editAsAssetButton = TimelineModeGUIState.Enabled
			};
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00036820 File Offset: 0x00034C20
		public override TimelineModeGUIState TrackState(TimelineWindow.TimelineState state)
		{
			return TimelineModeGUIState.Enabled;
		}
	}
}
