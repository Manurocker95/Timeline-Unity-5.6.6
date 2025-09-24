using System;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000E0 RID: 224
	internal abstract class TimelineMode
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x000365AC File Offset: 0x000349AC
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x000365C6 File Offset: 0x000349C6
		public TimelineMode.HeaderState headerState { get; protected set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x000365D0 File Offset: 0x000349D0
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x000365EA File Offset: 0x000349EA
		public TimelineMode.TrackOptionsState trackOptionsState { get; protected set; }

		// Token: 0x06000849 RID: 2121
		public abstract bool ShouldShowPlayRange(TimelineWindow.TimelineState state);

		// Token: 0x0600084A RID: 2122
		public abstract bool ShouldShowTimeCursor(TimelineWindow.TimelineState state);

		// Token: 0x0600084B RID: 2123 RVA: 0x000365F4 File Offset: 0x000349F4
		public virtual bool ShouldShowTimeArea(TimelineWindow.TimelineState state)
		{
			return state.timeline != null && state.timeline.tracks.Any<TrackAsset>();
		}

		// Token: 0x0600084C RID: 2124
		public abstract TimelineModeGUIState TrackState(TimelineWindow.TimelineState state);

		// Token: 0x0600084D RID: 2125
		public abstract TimelineModeGUIState ToolbarState(TimelineWindow.TimelineState state);

		// Token: 0x020000E1 RID: 225
		public struct HeaderState
		{
			// Token: 0x04000480 RID: 1152
			public TimelineModeGUIState breadCrumb;

			// Token: 0x04000481 RID: 1153
			public TimelineModeGUIState sequenceSelector;

			// Token: 0x04000482 RID: 1154
			public TimelineModeGUIState searchFilter;

			// Token: 0x04000483 RID: 1155
			public TimelineModeGUIState options;
		}

		// Token: 0x020000E2 RID: 226
		public struct TrackOptionsState
		{
			// Token: 0x04000484 RID: 1156
			public TimelineModeGUIState newButton;

			// Token: 0x04000485 RID: 1157
			public TimelineModeGUIState editAsAssetButton;
		}
	}
}
