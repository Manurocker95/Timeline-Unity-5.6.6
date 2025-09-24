using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000074 RID: 116
	internal class TrackVerticalResize : Manipulator
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x0001EC38 File Offset: 0x0001D038
		public override void Init(IControl parent)
		{
			this.m_Captured = false;
			this.m_UndoAdded = false;
			this.m_CapturedHeight = 0f;
			this.m_CaptureMouseYPos = 0f;
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineTrackGUI timelineTrackGUI = target as TimelineTrackGUI;
				bool result;
				if (timelineTrackGUI == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					Rect rect = RectUtils.Encompass(timelineTrackGUI.headerBounds, timelineTrackGUI.boundingRect);
					rect.y = rect.yMax - 5f;
					if (rect.Contains(evt.mousePosition))
					{
						this.m_Captured = true;
						this.m_CapturedHeight = timelineTrackGUI.track.inlineAnimationCurveHeight;
						this.m_CaptureMouseYPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y;
						state.captured.Add((IControl)target);
						this.m_UndoAdded = false;
						result = base.ConsumeEvent();
					}
					else
					{
						result = base.IgnoreEvent();
					}
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!this.m_Captured)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineTrackGUI timelineTrackGUI = target as TimelineTrackGUI;
					if (timelineTrackGUI == null)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						if (!this.m_UndoAdded)
						{
							TimelineHelpers.PushUndo(timelineTrackGUI.track, "track.setheight");
							this.m_UndoAdded = true;
						}
						float num = this.m_CapturedHeight + (GUIUtility.GUIToScreenPoint(Event.current.mousePosition).y - this.m_CaptureMouseYPos);
						timelineTrackGUI.track.inlineAnimationCurveHeight = Mathf.Max(num, 60f);
						state.GetWindow().treeView.CalculateRowRects();
						result = base.ConsumeEvent();
					}
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!this.m_Captured)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					state.captured.Remove(target as IControl);
					this.m_Captured = false;
					result = base.ConsumeEvent();
				}
				return result;
			};
		}

		// Token: 0x04000280 RID: 640
		private bool m_Captured = false;

		// Token: 0x04000281 RID: 641
		private bool m_UndoAdded = false;

		// Token: 0x04000282 RID: 642
		private float m_CapturedHeight = 0f;

		// Token: 0x04000283 RID: 643
		private float m_CaptureMouseYPos = 0f;
	}
}
