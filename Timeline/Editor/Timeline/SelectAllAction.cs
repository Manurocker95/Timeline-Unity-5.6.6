using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BB RID: 187
	[HideInMenu]
	internal class SelectAllAction : TimelineAction
	{
		// Token: 0x06000662 RID: 1634 RVA: 0x0002C964 File Offset: 0x0002AD64
		public override bool Execute(TimelineWindow.TimelineState state)
		{
			IClipCurveEditorOwner currentInlineCurveEditorOwner = state.selection.currentInlineCurveEditorOwner;
			bool result;
			if (currentInlineCurveEditorOwner != null && currentInlineCurveEditorOwner.clipCurveEditor != null)
			{
				currentInlineCurveEditorOwner.clipCurveEditor.SelectAllKeys();
				result = true;
			}
			else
			{
				state.selection.Clear();
				state.GetWindow().allTracks.ForEach(delegate(TimelineTrackBaseGUI x)
				{
					state.selection.Add(x);
				});
				result = true;
			}
			return result;
		}
	}
}
