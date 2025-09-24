using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000064 RID: 100
	internal class RectangleSelect : Manipulator
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0001C7AC File Offset: 0x0001ABAC
		public override void Init(IControl parent)
		{
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsCurrentEditingASequencerTextField())
				{
					result = base.IgnoreEvent();
				}
				else if (evt.button != 0 && evt.button != 1)
				{
					result = base.IgnoreEvent();
				}
				else if (evt.modifiers == 4)
				{
					result = base.IgnoreEvent();
				}
				else if (TimelineWindow.instance.sequenceHeaderBounds.Contains(evt.mousePosition))
				{
					foreach (TimelineTrackBaseGUI timelineTrackBaseGUI in TimelineWindow.instance.allTracks)
					{
						Rect headerBounds = timelineTrackBaseGUI.headerBounds;
						headerBounds.y += TimelineWindow.instance.treeviewBounds.y;
						if (headerBounds.Contains(evt.mousePosition))
						{
							return base.IgnoreEvent();
						}
					}
					if (evt.modifiers == null)
					{
						state.selection.Clear();
					}
					result = base.IgnoreEvent();
				}
				else if (!TimelineWindow.instance.clipArea.Contains(evt.mousePosition))
				{
					result = base.IgnoreEvent();
				}
				else
				{
					Vector2 mousePosition = evt.mousePosition;
					Vector2 mousePosition2 = evt.mousePosition;
					mousePosition2.x += 1f;
					mousePosition2.y += 1f;
					List<IBounds> elementsInRectangle = Manipulator.GetElementsInRectangle(state.quadTree, mousePosition, mousePosition2);
					bool flag;
					if (!this.CanStartRectableSelect(evt, state, elementsInRectangle, out flag))
					{
						if (flag)
						{
							this.HandleReselection(state, elementsInRectangle);
						}
						else
						{
							this.HandleSingleSelection(evt, state, elementsInRectangle);
						}
						result = base.IgnoreEvent();
					}
					else
					{
						state.captured.Add(target as IControl);
						this.m_isCaptured = true;
						this.m_Start = evt.mousePosition;
						this.m_End = evt.mousePosition;
						if (RectangleSelect.CanClearSelection(evt))
						{
							state.selection.Clear();
						}
						result = base.IgnoreEvent();
					}
				}
				return result;
			};
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (this.m_isCaptured && evt.keyCode == 27)
				{
					state.captured.Remove(target as IControl);
					this.m_isCaptured = false;
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
			parent.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!this.m_isCaptured)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					state.captured.Remove(target as IControl);
					this.m_isCaptured = false;
					Rect rect = default(Rect);
					rect.min = new Vector2(Math.Min(this.m_Start.x, this.m_End.x), Math.Min(this.m_Start.y, this.m_End.y));
					rect.max = new Vector2(Math.Max(this.m_Start.x, this.m_End.x), Math.Max(this.m_Start.y, this.m_End.y));
					if (rect.width < 1f || rect.height < 1f)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						List<IBounds> list = state.quadTree.ContainedBy(rect);
						IEnumerable<IBounds> enumerable = from x in list
						where x is ISelectable
						select x;
						if (list.Count == 0)
						{
							result = base.IgnoreEvent();
						}
						else
						{
							if (RectangleSelect.CanClearSelection(evt))
							{
								state.selection.Clear();
							}
							foreach (IBounds bounds in enumerable)
							{
								if (!(bounds is TimelineGroupGUI))
								{
									state.selection.Add((ISelectable)bounds);
								}
							}
							result = base.ConsumeEvent();
						}
					}
				}
				return result;
			};
			parent.MouseDrag += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (!this.m_isCaptured)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					this.m_End = evt.mousePosition;
					result = base.ConsumeEvent();
				}
				return result;
			};
			parent.Overlay += this.DrawSelection;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001C814 File Offset: 0x0001AC14
		private static bool CanClearSelection(Event evt)
		{
			return evt.modifiers != 2 && evt.modifiers != 8 && evt.modifiers != 1;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001C850 File Offset: 0x0001AC50
		private bool CanStartRectableSelect(Event evt, TimelineWindow.TimelineState state, List<IBounds> elements, out bool hasOneSelected)
		{
			hasOneSelected = false;
			foreach (IBounds bounds in elements)
			{
				if (bounds != null)
				{
					if (bounds is InlineCurveEditor)
					{
						hasOneSelected = true;
						return false;
					}
					if (bounds is ISelectable && bounds is TimelineClipGUI)
					{
						ISelectable selectable = bounds as ISelectable;
						if (selectable.selected && evt.modifiers == null)
						{
							selectable.selected = true;
							hasOneSelected = true;
						}
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001C91C File Offset: 0x0001AD1C
		private void HandleSingleSelection(Event evt, TimelineWindow.TimelineState state, List<IBounds> elements)
		{
			if (RectangleSelect.CanClearSelection(evt))
			{
				state.selection.Clear();
			}
			TimelineClipGUI timelineClipGUI = (from x in elements.OfType<TimelineClipGUI>()
			orderby x.zOrder
			select x).Last<TimelineClipGUI>();
			if (evt.modifiers == 1)
			{
				if (!elements.OfType<ISelectable>().Any((ISelectable x) => x.selected))
				{
					timelineClipGUI.parentTrack.RangeSelectClips(timelineClipGUI, state);
				}
			}
			else if (evt.modifiers == 2 || evt.modifiers == 8)
			{
				bool selected = timelineClipGUI.selected;
				if (selected)
				{
					state.selection.Remove(timelineClipGUI);
				}
				else
				{
					state.selection.Add(timelineClipGUI);
				}
			}
			else
			{
				state.selection.Add(timelineClipGUI);
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001CA14 File Offset: 0x0001AE14
		private void HandleReselection(TimelineWindow.TimelineState state, List<IBounds> elements)
		{
			foreach (IBounds bounds in elements)
			{
				if (bounds is TimelineClipGUI)
				{
					TimelineClipGUI timelineClipGUI = bounds as TimelineClipGUI;
					timelineClipGUI.Reselect();
					break;
				}
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001CA88 File Offset: 0x0001AE88
		private bool DrawSelection(object target, Event e, TimelineWindow.TimelineState state)
		{
			bool result;
			if (!this.m_isCaptured)
			{
				result = false;
			}
			else
			{
				Rect rect = default(Rect);
				rect.min = new Vector2(Math.Min(this.m_Start.x, this.m_End.x), Math.Min(this.m_Start.y, this.m_End.y));
				rect.max = new Vector2(Math.Max(this.m_Start.x, this.m_End.x), Math.Max(this.m_Start.y, this.m_End.y));
				float segmentsLength = 2f;
				Vector3[] array = new Vector3[]
				{
					new Vector3(rect.xMin, rect.yMin, 0f),
					new Vector3(rect.xMax, rect.yMin, 0f),
					new Vector3(rect.xMax, rect.yMax, 0f),
					new Vector3(rect.xMin, rect.yMax, 0f)
				};
				Graphics.DrawDottedLine(array[0], array[1], segmentsLength, DirectorStyles.Instance.customSkin.colorRectangleSelect);
				Graphics.DrawDottedLine(array[1], array[2], segmentsLength, DirectorStyles.Instance.customSkin.colorRectangleSelect);
				Graphics.DrawDottedLine(array[2], array[3], segmentsLength, DirectorStyles.Instance.customSkin.colorRectangleSelect);
				Graphics.DrawDottedLine(array[3], array[0], segmentsLength, DirectorStyles.Instance.customSkin.colorRectangleSelect);
				result = true;
			}
			return result;
		}

		// Token: 0x04000271 RID: 625
		private Vector2 m_Start = Vector2.zero;

		// Token: 0x04000272 RID: 626
		private Vector2 m_End = Vector2.zero;

		// Token: 0x04000273 RID: 627
		private bool m_isCaptured = false;
	}
}
