using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000AA RID: 170
	[HideInMenu]
	internal class DuplicateClips : ClipAction
	{
		// Token: 0x06000603 RID: 1539 RVA: 0x0002AE3C File Offset: 0x0002923C
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			PlayableDirector director = (state == null) ? null : state.currentDirector;
			foreach (TimelineClip clip in clips)
			{
				TimelineClip timelineClip = clip.Duplicate(director);
				if (timelineClip != null && state != null)
				{
					state.selection.Clear();
					timelineClip.selected = true;
					state.selection.SelectInEditor(EditorClip.CreateEditorClip(timelineClip.parentTrack.timelineAsset, timelineClip));
				}
			}
			if (state != null)
			{
				state.Refresh();
			}
			return true;
		}
	}
}
