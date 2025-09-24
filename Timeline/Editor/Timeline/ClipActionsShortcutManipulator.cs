using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200005D RID: 93
	internal class ClipActionsShortcutManipulator : Manipulator
	{
		// Token: 0x06000359 RID: 857 RVA: 0x0001B454 File Offset: 0x00019854
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineClipGUI timelineClipGUI = target as TimelineClipGUI;
					if (!timelineClipGUI.selected)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						result = ClipAction.HandleShortcut(state, evt, timelineClipGUI.clip);
					}
				}
				return result;
			};
		}
	}
}
