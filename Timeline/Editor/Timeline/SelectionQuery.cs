using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000011 RID: 17
	internal static class SelectionQuery
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00009098 File Offset: 0x00007498
		public static IEnumerable<TimelineClipGUI> EditableClips(this Selection selection)
		{
			return selection.FilterByType<TimelineClipGUI>();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000090B4 File Offset: 0x000074B4
		public static IEnumerable<TrackAsset> TracksWithSelectedClips(this Selection selection)
		{
			return (from x in selection.FilterByType<TimelineClipGUI>()
			select x.clip.parentTrack).Distinct<TrackAsset>();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000090F8 File Offset: 0x000074F8
		public static IEnumerable<TrackAsset> SelectedTracks(this Selection selection)
		{
			return (from x in selection
			select x.selectableObject).OfType<TrackAsset>();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009138 File Offset: 0x00007538
		public static IEnumerable<TrackAsset> SelectedTracksNoChildren(this Selection selection)
		{
			List<TrackAsset> list = selection.SelectedTracks().ToList<TrackAsset>();
			return from x in selection.SelectedTracks()
			where !SelectionQuery.ParentInList(x.parent as TrackAsset, list)
			select x;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000917C File Offset: 0x0000757C
		private static bool ParentInList(TrackAsset x, List<TrackAsset> list)
		{
			return x != null && (SelectionQuery.ParentInList(x.parent as TrackAsset, list) || list.Contains(x));
		}
	}
}
