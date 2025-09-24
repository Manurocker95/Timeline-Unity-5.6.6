using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000072 RID: 114
	internal class NewTrackContextMenu : Manipulator
	{
		// Token: 0x060003BF RID: 959 RVA: 0x0001E6BB File Offset: 0x0001CABB
		public override void Init(IControl parent)
		{
			parent.ContextClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (TimelineWindow.instance.sequenceHeaderBounds.Contains(evt.mousePosition))
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
					TimelineWindow.instance.ShowNewTracksContextMenu(state, null, null);
					result = base.ConsumeEvent();
				}
				else
				{
					result = base.IgnoreEvent();
				}
				return result;
			};
		}
	}
}
