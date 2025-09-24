using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000C8 RID: 200
	[HideInMenu]
	internal class CopyTracksToClipboard : TrackAction
	{
		// Token: 0x0600069E RID: 1694 RVA: 0x0002D880 File Offset: 0x0002BC80
		public static bool Do(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			CopyTracksToClipboard copyTracksToClipboard = new CopyTracksToClipboard();
			return copyTracksToClipboard.Execute(state, tracks);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002D8A4 File Offset: 0x0002BCA4
		public override bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			Clipboard.AddDataCollection(from x in tracks
			select x);
			return true;
		}
	}
}
