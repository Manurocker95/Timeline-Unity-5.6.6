using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000073 RID: 115
	internal class SelectorTool : Manipulator
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x0001E7C4 File Offset: 0x0001CBC4
		private static bool CanClearSelection(Event evt, TimelineGroupGUI track)
		{
			return !track.selected && (evt.modifiers != 2 && evt.modifiers != 1) && evt.modifiers != 8;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001E80E File Offset: 0x0001CC0E
		public override void Init(IControl parent)
		{
			parent.DoubleClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
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
					else if (evt.button != 0)
					{
						result = base.IgnoreEvent();
					}
					else if (!timelineTrackGUI.indentedHeaderBounds.Contains(evt.mousePosition))
					{
						result = base.IgnoreEvent();
					}
					else
					{
						bool selected = timelineTrackGUI.selected;
						foreach (TimelineClipGUI item in timelineTrackGUI.clips)
						{
							if (selected)
							{
								state.selection.Add(item);
							}
							else
							{
								state.selection.Remove(item);
							}
						}
						result = base.ConsumeEvent();
					}
				}
				return result;
			};
			parent.MouseDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsCurrentEditingASequencerTextField())
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineGroupGUI timelineGroupGUI = target as TimelineGroupGUI;
					if (timelineGroupGUI == null)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						if (target is TimelineTrackGUI)
						{
							TimelineTrackGUI timelineTrackGUI = target as TimelineTrackGUI;
							if (timelineTrackGUI.locked)
							{
								if (SelectorTool.CanClearSelection(evt, timelineGroupGUI))
								{
									state.selection.Clear();
								}
								state.selection.Add(timelineTrackGUI);
							}
							bool flag = (from x in timelineTrackGUI.clips
							where x.bounds.Contains(evt.mousePosition)
							select x).Count<TimelineClipGUI>() > 0;
							if (flag && !TimelineWindow.instance.sequenceHeaderBounds.Contains(evt.mousePosition))
							{
								return base.IgnoreEvent();
							}
						}
						if (SelectorTool.CanClearSelection(evt, timelineGroupGUI))
						{
							state.selection.Clear();
						}
						List<TimelineGroupGUI> list = state.selection.FilterByType<TimelineGroupGUI>();
						if (evt.modifiers == 2 || evt.modifiers == 8)
						{
							if (timelineGroupGUI.selected)
							{
								state.selection.Remove(timelineGroupGUI);
							}
							else
							{
								state.selection.Add(timelineGroupGUI);
							}
						}
						else if (evt.modifiers == 1)
						{
							if (list.Count == 0 && !timelineGroupGUI.selected)
							{
								state.selection.Add(timelineGroupGUI);
							}
							else
							{
								bool flag2 = false;
								foreach (TimelineTrackBaseGUI timelineTrackBaseGUI in TimelineWindow.instance.allTracks)
								{
									if (!flag2)
									{
										if (timelineTrackBaseGUI == timelineGroupGUI || timelineTrackBaseGUI.selected)
										{
											state.selection.Add(timelineTrackBaseGUI);
											flag2 = true;
											continue;
										}
									}
									if (flag2)
									{
										if (timelineTrackBaseGUI == timelineGroupGUI || timelineTrackBaseGUI.selected)
										{
											state.selection.Add(timelineTrackBaseGUI);
											flag2 = false;
											continue;
										}
									}
									if (flag2)
									{
										state.selection.Add(timelineTrackBaseGUI);
									}
								}
							}
						}
						else
						{
							state.selection.Add(timelineGroupGUI);
						}
						result = base.IgnoreEvent();
					}
				}
				return result;
			};
		}
	}
}
