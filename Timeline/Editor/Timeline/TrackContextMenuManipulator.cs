using System;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000075 RID: 117
	internal class TrackContextMenuManipulator : Manipulator
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0001EE77 File Offset: 0x0001D277
		public override void Init(IControl parent)
		{
			parent.ContextClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineTrackGUI timelineTrackGUI = target as TimelineTrackGUI;
				bool result;
				if (timelineTrackGUI == null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					if (!timelineTrackGUI.headerBounds.Contains(evt.mousePosition))
					{
						bool flag = state.quadTree.GetItemsAtPosition(evt.mousePosition).Any((IBounds x) => x is TimelineClipGUI);
						if (flag)
						{
							return base.IgnoreEvent();
						}
					}
					timelineTrackGUI.drawer.trackMenuContext.clipTimeCreation = TrackDrawer.TrackMenuContext.ClipTimeCreation.Mouse;
					timelineTrackGUI.drawer.trackMenuContext.mousePosition = evt.mousePosition;
					timelineTrackGUI.DisplayTrackMenu(state);
					result = base.ConsumeEvent();
				}
				return result;
			};
		}
	}
}
