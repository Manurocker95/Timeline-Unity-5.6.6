using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C9 RID: 201
	[HideInMenu]
	internal class DuplicateTracks : TrackAction
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x0002D904 File Offset: 0x0002BD04
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			foreach (TrackAsset track in tracks)
			{
				track.Duplicate(state.currentDirector);
			}
			state.Refresh();
			return true;
		}
	}
}
