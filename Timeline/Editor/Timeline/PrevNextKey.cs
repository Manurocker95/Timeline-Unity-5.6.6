using System;
using UnityEditor.Timeline.Utilities;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006C RID: 108
	internal class PrevNextKey : Manipulator
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x0001DD18 File Offset: 0x0001C118
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.control || evt.shift || evt.alt || evt.command)
				{
					result = base.IgnoreEvent();
				}
				else if (evt.keyCode != 44 && evt.keyCode != 46)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					float num = (float)state.time;
					bool flag = evt.keyCode == 44;
					TimelineAsset timeline = (target as TimelineWindow).timeline;
					if (timeline == null)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						if (this.m_Traverser == null)
						{
							this.m_Traverser = new KeyTraverser(timeline, 0.01f / state.frameRate);
						}
						float num2 = (!flag) ? this.m_Traverser.GetNextKey(num, state.dirtyStamp) : this.m_Traverser.GetPrevKey(num, state.dirtyStamp);
						if (num2 != num)
						{
							state.time = (double)num2;
							TimelineWindow timelineWindow = target as TimelineWindow;
							if (timelineWindow != null)
							{
								timelineWindow.Repaint();
							}
						}
						result = base.ConsumeEvent();
					}
				}
				return result;
			};
		}

		// Token: 0x0400027D RID: 637
		private KeyTraverser m_Traverser;
	}
}
