using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000076 RID: 118
	internal class TrackDoubleClick : Manipulator
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x0001EF75 File Offset: 0x0001D375
		public override void Init(IControl parent)
		{
			parent.DoubleClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (evt.button != 0)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineTrackBaseGUI timelineTrackBaseGUI = target as TimelineTrackBaseGUI;
					result = EditTrackInAnimationWindow.Do(state, timelineTrackBaseGUI.track);
				}
				return result;
			};
		}
	}
}
