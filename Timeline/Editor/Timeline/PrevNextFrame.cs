using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006B RID: 107
	internal class PrevNextFrame : Manipulator
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0001DC34 File Offset: 0x0001C034
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!evt.control)
				{
					result = base.IgnoreEvent();
				}
				else if (evt.shift || evt.alt || evt.command)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					if (evt.keyCode == 44)
					{
						state.frame--;
					}
					else
					{
						if (evt.keyCode != 46)
						{
							return base.IgnoreEvent();
						}
						state.frame++;
					}
					TimelineWindow timelineWindow = target as TimelineWindow;
					if (timelineWindow != null)
					{
						timelineWindow.Repaint();
					}
					result = base.ConsumeEvent();
				}
				return result;
			};
		}
	}
}
