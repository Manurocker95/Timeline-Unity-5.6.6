using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006D RID: 109
	internal class GotoStartEnd : Manipulator
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0001DE6B File Offset: 0x0001C26B
		public override void Init(IControl parent)
		{
			parent.KeyUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else if (evt.keyCode == 44 && evt.modifiers == 1)
				{
					this.GotoStart(state);
					result = base.ConsumeEvent();
				}
				else if (evt.keyCode == 46 && evt.modifiers == 1)
				{
					this.GotoEnd(state);
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001DE80 File Offset: 0x0001C280
		private void GotoStart(TimelineWindow.TimelineState state)
		{
			state.time = 0.0;
			state.EnsurePlayHeadIsVisible();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001DE98 File Offset: 0x0001C298
		private void GotoEnd(TimelineWindow.TimelineState state)
		{
			state.time = state.duration;
			state.EnsurePlayHeadIsVisible();
		}
	}
}
