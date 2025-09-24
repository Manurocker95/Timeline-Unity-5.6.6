using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000A9 RID: 169
	[HideInMenu]
	internal class CopyClipsToClipboard : ClipAction
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0002AD6C File Offset: 0x0002916C
		public static bool Do(TimelineWindow.TimelineState state, TimelineClip clip)
		{
			TimelineClip[] clips = new TimelineClip[]
			{
				clip
			};
			return ClipAction.DoInternal(typeof(CopyClipsToClipboard), state, clips);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0002ADA0 File Offset: 0x000291A0
		public static bool Do(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return ClipAction.DoInternal(typeof(CopyClipsToClipboard), state, clips);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0002ADC8 File Offset: 0x000291C8
		public override bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			Clipboard.AddDataCollection(from x in clips
			select EditorClip.CreateEditorClip(state.timeline, x));
			return true;
		}
	}
}
