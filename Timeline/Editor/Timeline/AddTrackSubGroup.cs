using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C4 RID: 196
	[DisplayName("Add Track Sub-Group")]
	internal class AddTrackSubGroup : TrackAction
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x0002D5D8 File Offset: 0x0002B9D8
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			MenuActionDisplayState result;
			if ((from x in tracks
			where x is GroupTrack
			select x).ToArray<TrackAsset>().Length != tracks.Length)
			{
				result = MenuActionDisplayState.Hidden;
			}
			else
			{
				result = MenuActionDisplayState.Visible;
			}
			return result;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002D62C File Offset: 0x0002BA2C
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			foreach (TrackAsset trackAsset in tracks)
			{
				TrackModifier.AddTrack(state.timeline, state.currentDirector, TimelineHelpers.GroupTrackType, trackAsset, "Track Sub-Group");
				TimelineTrackBaseGUI timelineTrackBaseGUI = TimelineTrackBaseGUI.FindGUITrack(trackAsset);
				if (timelineTrackBaseGUI != null)
				{
					TimelineWindow.instance.treeView.data.SetExpanded(timelineTrackBaseGUI, true);
				}
			}
			state.Refresh();
			return true;
		}
	}
}
