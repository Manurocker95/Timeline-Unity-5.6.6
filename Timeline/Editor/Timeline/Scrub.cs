using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000015 RID: 21
	internal class Scrub : Manipulator
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000A210 File Offset: 0x00008610
		public Scrub(Action<TimelineWindow.TimelineState, double, bool> onDrag)
		{
			this.m_OnDrag = onDrag;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000A220 File Offset: 0x00008620
		public override void Init(IControl parent)
		{
			bool isCaptured = false;
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.button != 0)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					state.captured.Remove(target as IControl);
					state.captured.Add(target as IControl);
					isCaptured = true;
					result = this.ConsumeEvent();
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.button != 0)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					state.captured.Clear();
					isCaptured = false;
					result = this.ConsumeEvent();
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.button != 0)
				{
					result = this.IgnoreEvent();
				}
				else if (!isCaptured)
				{
					result = this.IgnoreEvent();
				}
				else
				{
					if (this.m_OnDrag != null)
					{
						this.m_OnDrag(state, state.GetSnappedTimeAtMousePosition(evt.mousePosition), false);
					}
					result = this.ConsumeEvent();
				}
				return result;
			};
		}

		// Token: 0x04000129 RID: 297
		private Action<TimelineWindow.TimelineState, double, bool> m_OnDrag;
	}
}
