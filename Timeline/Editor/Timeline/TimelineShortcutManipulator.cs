using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000071 RID: 113
	internal class TimelineShortcutManipulator : Manipulator
	{
		// Token: 0x060003BA RID: 954 RVA: 0x0001E4EC File Offset: 0x0001C8EC
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
					result = TimelineAction.HandleShortcut(state, evt);
				}
				return result;
			};
			parent.ValidateCommand += ((object target, Event evt, TimelineWindow.TimelineState state) => evt.commandName == "Copy" || evt.commandName == "Paste" || evt.commandName == "Duplicate" || evt.commandName == "SelectAll");
			parent.ExecuteCommand += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.commandName == "Copy")
				{
					TimelineAction.InvokeByName("CopyAction", state);
					result = base.ConsumeEvent();
				}
				else if (evt.commandName == "Paste")
				{
					TimelineAction.InvokeByName("PasteAction", state);
					result = base.ConsumeEvent();
				}
				else if (evt.commandName == "Duplicate")
				{
					TimelineAction.InvokeByName("DuplicateAction", state);
					result = base.ConsumeEvent();
				}
				else if (evt.commandName == "SelectAll")
				{
					TimelineAction.InvokeByName("SelectAllAction", state);
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
		}
	}
}
