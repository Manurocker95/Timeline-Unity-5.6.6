using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B7 RID: 183
	[DisplayName("Paste Into")]
	[SeparatorMenuItem(SeparatorMenuItemPosition.After)]
	internal class PasteIntoAction : TimelineAction
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x0002C46C File Offset: 0x0002A86C
		public static bool Do(TimelineWindow.TimelineState state, TrackAsset track)
		{
			List<TimelineClip> list = (from x in Clipboard.GetData<EditorClip>()
			select x.clip).ToList<TimelineClip>();
			double num;
			if (track.clips.Length == 0)
			{
				num = 0.0;
			}
			else
			{
				num = track.clips.Last<TimelineClip>().end;
			}
			for (int num2 = 0; num2 != list.Count; num2++)
			{
				TimelineClip timelineClip = list[num2];
				if (track.IsCompatibleWithClip(timelineClip))
				{
					if (num2 != 0)
					{
						num += list[num2].end - list[num2 - 1].end;
					}
					TimelineClip timelineClip2 = timelineClip.DuplicateAtTime(track, num, state.currentDirector, state.timeline);
					if (timelineClip2.isNestedAsset)
					{
						timelineClip2.isNestedAsset = false;
						timelineClip2.nestedOutputIndex = 0;
					}
				}
			}
			state.Refresh();
			return true;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002C578 File Offset: 0x0002A978
		public override MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state)
		{
			return (!PasteIntoAction.CanPasteInto(state)) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002C5A0 File Offset: 0x0002A9A0
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			TrackAsset track = state.selection.FilterByType<TimelineTrackBaseGUI>().ToList<TimelineTrackBaseGUI>()[0].track;
			return PasteIntoAction.Do(state, track);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002C5D8 File Offset: 0x0002A9D8
		private static bool CanPasteInto(TimelineWindow.TimelineState state)
		{
			List<TrackAsset> list = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToList<TrackAsset>();
			bool result;
			if (list.Count != 1)
			{
				result = false;
			}
			else
			{
				List<TimelineClip> list2 = (from x in Clipboard.GetData<EditorClip>()
				select x.clip).ToList<TimelineClip>();
				if (list2.Count == 0)
				{
					result = false;
				}
				else
				{
					TrackAsset trackAsset = list[0];
					TrackAsset parentTrack = list2[0].parentTrack;
					foreach (TimelineClip timelineClip in list2)
					{
						if (timelineClip.parentTrack != parentTrack || !trackAsset.IsCompatibleWithClip(timelineClip))
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}
	}
}
