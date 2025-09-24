using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000065 RID: 101
	internal class TrackZoom : Manipulator
	{
		// Token: 0x06000389 RID: 905 RVA: 0x0001D191 File Offset: 0x0001B591
		public override void Init(IControl parent)
		{
			parent.MouseWheel += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (EditorGUI.actionKey)
				{
					state.trackScale = Mathf.Min(Mathf.Max(state.trackScale + evt.delta.y * 0.1f, 1f), 100f);
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
