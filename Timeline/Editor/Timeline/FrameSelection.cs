using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000068 RID: 104
	internal class FrameSelection : Manipulator
	{
		// Token: 0x06000391 RID: 913 RVA: 0x0001D5F9 File Offset: 0x0001B9F9
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else if (evt.keyCode != 102)
				{
					result = base.IgnoreEvent();
				}
				else if (state.selection.Count == 0)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					float num = float.MaxValue;
					float num2 = float.MinValue;
					List<TimelineClipGUI> list = state.selection.FilterByType<TimelineClipGUI>();
					if (!list.Any<TimelineClipGUI>())
					{
						result = base.IgnoreEvent();
					}
					else
					{
						foreach (TimelineClipGUI timelineClipGUI in list)
						{
							num = Mathf.Min(num, (float)timelineClipGUI.clip.start);
							num2 = Mathf.Max(num2, (float)timelineClipGUI.clip.start + (float)timelineClipGUI.clip.duration);
							if (timelineClipGUI.clipCurveEditor != null)
							{
								timelineClipGUI.clipCurveEditor.FrameClip();
							}
						}
						float num3 = num2 - num;
						if (Mathf.Abs(num3) < 1E-45f)
						{
							num3 = 1f;
						}
						state.SetTimeAreaShownRange(Mathf.Max(num - num3 * 0.2f, -10f), num2 + num3 * 0.2f);
						state.Evaluate();
						result = base.ConsumeEvent();
					}
				}
				return result;
			};
			parent.KeyUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.keyCode == 91)
				{
					result = this.DoEnterFastZoom(state);
				}
				else if (evt.keyCode == 93)
				{
					result = this.DoExitFastZoom(state);
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001D620 File Offset: 0x0001BA20
		private bool DoEnterFastZoom(TimelineWindow.TimelineState state)
		{
			bool result;
			if (this.m_isInFastZoom)
			{
				result = base.IgnoreEvent();
			}
			else
			{
				float num = (float)(state.time - 0.5);
				float max = (float)(state.time + 0.5);
				if (num < 0f)
				{
					num = 0f;
					max = 1f;
				}
				this.m_translation = state.timeAreaTranslation;
				this.m_scale = state.timeAreaScale;
				this.m_isInFastZoom = true;
				state.SetTimeAreaShownRange(num, max);
				state.Refresh();
				result = base.ConsumeEvent();
			}
			return result;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001D6BC File Offset: 0x0001BABC
		private bool DoExitFastZoom(TimelineWindow.TimelineState state)
		{
			bool result;
			if (!this.m_isInFastZoom)
			{
				result = base.IgnoreEvent();
			}
			else
			{
				this.m_isInFastZoom = false;
				state.SetTimeAreaTransform(this.m_translation, this.m_scale);
				state.Refresh();
				result = base.ConsumeEvent();
			}
			return result;
		}

		// Token: 0x04000277 RID: 631
		private bool m_isInFastZoom;

		// Token: 0x04000278 RID: 632
		private Vector2 m_translation = Vector2.zero;

		// Token: 0x04000279 RID: 633
		private Vector2 m_scale = Vector2.one;
	}
}
