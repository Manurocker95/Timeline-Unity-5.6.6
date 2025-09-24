using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B5 RID: 181
	[DisplayName("Copy")]
	[Shortcut(10, 99)]
	internal class CopyAction : TimelineAction
	{
		// Token: 0x06000649 RID: 1609 RVA: 0x0002C258 File Offset: 0x0002A658
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			Clipboard.Clear();
			TimelineClip[] array = (from x in state.selection.FilterByType<TimelineClipGUI>()
			select x.clip).ToArray<TimelineClip>();
			if (array.Length > 0)
			{
				CopyClipsToClipboard.Do(state, array);
			}
			TrackAsset[] array2 = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToArray<TrackAsset>();
			if (array2.Length > 0)
			{
				CopyTracksToClipboard.Do(state, array2);
			}
			return true;
		}
	}
}
