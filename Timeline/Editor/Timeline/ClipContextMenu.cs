using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000063 RID: 99
	internal class ClipContextMenu : Manipulator
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0001C6A5 File Offset: 0x0001AAA5
		public override void Init(IControl parent)
		{
			parent.ContextClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				IEnumerable<TimelineClipGUI> source = state.selection.EditableClips();
				bool result;
				if (!source.Any<TimelineClipGUI>())
				{
					result = base.IgnoreEvent();
				}
				else if (!source.Any((TimelineClipGUI c) => c.bounds.Contains(evt.mousePosition)))
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineClipGUI timelineClipGUI = target as TimelineClipGUI;
					SequencerContextMenu.Show(timelineClipGUI.parentTrack.drawer, evt.mousePosition);
					result = base.ConsumeEvent();
				}
				return result;
			};
		}
	}
}
