using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B6 RID: 182
	[DisplayName("Paste")]
	[Shortcut(10, 118)]
	internal class PasteAction : TimelineAction
	{
		// Token: 0x0600064D RID: 1613 RVA: 0x0002C33C File Offset: 0x0002A73C
		public static bool Do(TimelineWindow.TimelineState state)
		{
			return TimelineAction.DoInternal(typeof(PasteAction), state);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002C364 File Offset: 0x0002A764
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			IEnumerable<EditorClip> data = Clipboard.GetData<EditorClip>();
			foreach (EditorClip editorClip in data)
			{
				double end = editorClip.clip.parentTrack.clips.Last<TimelineClip>().end;
				editorClip.clip.DuplicateAtTime(editorClip.clip.parentTrack, end, state.currentDirector, state.timeline);
			}
			IEnumerable<TrackAsset> data2 = Clipboard.GetData<TrackAsset>();
			foreach (TrackAsset track in data2)
			{
				track.Duplicate(state.currentDirector);
			}
			state.Refresh();
			return true;
		}
	}
}
