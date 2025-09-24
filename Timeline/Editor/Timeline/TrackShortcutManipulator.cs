using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000077 RID: 119
	internal class TrackShortcutManipulator : Manipulator
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0001EFD3 File Offset: 0x0001D3D3
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineTrackBaseGUI trackGUI = target as TimelineTrackBaseGUI;
					if (trackGUI == null || trackGUI.track == null)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						bool flag;
						if (!trackGUI.selected)
						{
							flag = (from x in state.selection.FilterByType<TimelineClipGUI>()
							select x).Any((TimelineClipGUI x) => x.parentTrack == trackGUI);
						}
						else
						{
							flag = true;
						}
						bool flag2 = flag;
						if (flag2)
						{
							result = TrackAction.HandleShortcut(state, evt, trackGUI.track);
						}
						else
						{
							result = base.IgnoreEvent();
						}
					}
				}
				return result;
			};
		}
	}
}
