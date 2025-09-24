using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006F RID: 111
	internal class TimelineZoomManipulator : Manipulator
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x0001E16C File Offset: 0x0001C56C
		private void DoZoom(float zoomFactor, Event evt, TimelineWindow.TimelineState state)
		{
			float num = state.PixelToTime(this.m_LastMouseDownPosition.x);
			Vector2 focalPoint;
			focalPoint..ctor(num, 0f);
			float num2 = Mathf.Max(0.01f, 1f + zoomFactor * 0.01f);
			bool flag = Mathf.Abs(state.timeAreaTranslation.x - 10f) < float.Epsilon;
			state.SetTimeAreaScaleFocused(focalPoint, num2 * state.timeAreaScale, false, true);
			if (flag)
			{
				Vector2 timeAreaTranslation = state.timeAreaTranslation;
				timeAreaTranslation.x = 10f;
				state.SetTimeAreaTransform(timeAreaTranslation, state.timeAreaScale);
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001E214 File Offset: 0x0001C614
		public override void Init(IControl parent)
		{
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				this.m_LastMouseDownPosition = evt.mousePosition;
				return base.IgnoreEvent();
			};
			parent.MouseWheel += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.delta.y == 0f)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.m_LastMouseDownPosition = evt.mousePosition;
					float num = Event.current.delta.x + Event.current.delta.y;
					num = -num;
					this.DoZoom(num, evt, state);
					result = base.ConsumeEvent();
				}
				return result;
			};
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.keyCode == 43 || evt.keyCode == 270 || evt.keyCode == 61)
				{
					this.DoZoom(2f, evt, state);
					result = base.ConsumeEvent();
				}
				else if (evt.keyCode == 45 || evt.keyCode == 269)
				{
					this.DoZoom(-2f, evt, state);
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.modifiers == 4 && evt.button == 1)
				{
					float num = Event.current.delta.x + Event.current.delta.y;
					num = -num;
					this.DoZoom(num, evt, state);
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
		}

		// Token: 0x0400027E RID: 638
		private Vector2 m_LastMouseDownPosition = Vector2.zero;
	}
}
