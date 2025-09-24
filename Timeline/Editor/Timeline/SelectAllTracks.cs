using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000070 RID: 112
	internal class SelectAllTracks : Manipulator
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x0001E426 File Offset: 0x0001C826
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (EditorGUI.actionKey && evt.keyCode == 97)
				{
					state.selection.Clear();
					foreach (TimelineTrackBaseGUI item in TimelineWindow.instance.allTracks)
					{
						state.selection.Add(item);
					}
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
