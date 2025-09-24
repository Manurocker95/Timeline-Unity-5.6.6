using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000069 RID: 105
	internal class MouseWheelHorizontalScroll : Manipulator
	{
		// Token: 0x06000397 RID: 919 RVA: 0x0001D8DF File Offset: 0x0001BCDF
		public override void Init(IControl parent)
		{
			parent.MouseWheel += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.delta.x == 0f)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					state.OffsetTimeArea((int)evt.delta.x * 10);
					result = base.ConsumeEvent();
				}
				return result;
			};
		}
	}
}
