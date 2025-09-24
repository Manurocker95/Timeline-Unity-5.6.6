using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B8 RID: 184
	[DisplayName("Duplicate")]
	[Shortcut(10, 100)]
	internal class DuplicateAction : TimelineAction
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0002C754 File Offset: 0x0002AB54
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			TimelineClip[] array = (from x in state.selection.FilterByType<TimelineClipGUI>()
			select x.clip).ToArray<TimelineClip>();
			if (array.Length > 0)
			{
				ClipAction.InvokeByName("DuplicateClips", state, array);
			}
			TrackAsset[] array2 = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToArray<TrackAsset>();
			if (array2.Length > 0)
			{
				TrackAction.InvokeByName("DuplicateTracks", state, array2);
			}
			return true;
		}
	}
}
