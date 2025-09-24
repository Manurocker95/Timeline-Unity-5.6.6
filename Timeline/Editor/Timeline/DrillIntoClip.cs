using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000062 RID: 98
	internal class DrillIntoClip : Manipulator
	{
		// Token: 0x06000375 RID: 885 RVA: 0x0001C592 File Offset: 0x0001A992
		public override void Init(IControl parent)
		{
			parent.DoubleClick += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				TimelineClipGUI timelineClipGUI = target as TimelineClipGUI;
				bool result;
				if (timelineClipGUI == null || timelineClipGUI.clip == null)
				{
					result = base.IgnoreEvent();
				}
				else if (evt.button != 0)
				{
					result = base.IgnoreEvent();
				}
				else if (!timelineClipGUI.rect.Contains(evt.mousePosition))
				{
					result = base.IgnoreEvent();
				}
				else if (timelineClipGUI.clip.isNestedAsset)
				{
					ClipAction.InvokeByName("OpenCompound", state, timelineClipGUI.clip);
					result = base.ConsumeEvent();
				}
				else if (timelineClipGUI.clip.curves != null || timelineClipGUI.clip.animationClip != null)
				{
					ClipAction.InvokeByName("EditClipInAnimationWindow", state, timelineClipGUI.clip);
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
