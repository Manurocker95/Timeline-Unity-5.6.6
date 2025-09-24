using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B9 RID: 185
	[DisplayName("Delete")]
	[Shortcut(127)]
	[Shortcut(8)]
	internal class DeleteAction : TimelineAction
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x0002C83C File Offset: 0x0002AC3C
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			TimelineClip[] array = (from x in state.selection.FilterByType<TimelineClipGUI>()
			select x.clip).ToArray<TimelineClip>();
			if (array.Length > 0)
			{
				ClipAction.InvokeByName("DeleteClips", state, array);
			}
			TrackAsset[] array2 = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToArray<TrackAsset>();
			if (array2.Length > 0)
			{
				TrackAction.InvokeByName("DeleteTracks", state, array2);
			}
			return true;
		}
	}
}
