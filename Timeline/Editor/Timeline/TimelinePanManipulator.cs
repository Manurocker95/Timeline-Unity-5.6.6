using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006A RID: 106
	internal class TimelinePanManipulator : Manipulator
	{
		// Token: 0x0600039A RID: 922 RVA: 0x0001D968 File Offset: 0x0001BD68
		private bool IsOverAnimEditor(Event evt, TimelineWindow.TimelineState state)
		{
			Vector2 mousePosition = evt.mousePosition;
			Vector2 endPoint = mousePosition;
			endPoint.x += 1f;
			endPoint.y += 1f;
			List<IBounds> elementsInRectangle = Manipulator.GetElementsInRectangle(state.quadTree, mousePosition, endPoint);
			return elementsInRectangle.Any((IBounds x) => x is InlineCurveEditor);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001D9DD File Offset: 0x0001BDDD
		public override void Init(IControl parent)
		{
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if ((evt.button == 2 && evt.modifiers == null) || (evt.button == 0 && evt.modifiers == 4))
				{
					if (this.IsOverAnimEditor(evt, state))
					{
						result = base.IgnoreEvent();
					}
					else
					{
						this.m_Cursor.cursor = 13;
						Control.AddCursor(this.m_Cursor);
						this.m_Active = true;
						result = base.ConsumeEvent();
					}
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				if (this.m_Active)
				{
					Control.RemoveCursor(this.m_Cursor);
					state.editorWindow.Repaint();
				}
				return base.IgnoreEvent();
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!this.m_Active)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					Rect treeviewBounds = TimelineWindow.instance.treeviewBounds;
					treeviewBounds.xMax = TimelineWindow.instance.position.xMax;
					treeviewBounds.yMax = TimelineWindow.instance.position.yMax;
					if ((evt.button == 2 && evt.modifiers == null) || (evt.button == 0 && evt.modifiers == 4))
					{
						if (state.GetWindow() != null && state.GetWindow().treeView != null)
						{
							Vector2 scrollPosition = state.GetWindow().treeView.scrollPosition;
							scrollPosition.y -= evt.delta.y;
							state.GetWindow().treeView.scrollPosition = scrollPosition;
							state.OffsetTimeArea((int)evt.delta.x);
							return base.ConsumeEvent();
						}
					}
					result = base.IgnoreEvent();
				}
				return result;
			};
		}

		// Token: 0x0400027A RID: 634
		private readonly CursorInfo m_Cursor = new CursorInfo();

		// Token: 0x0400027B RID: 635
		private bool m_Active = false;
	}
}
