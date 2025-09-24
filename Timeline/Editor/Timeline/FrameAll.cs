using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000067 RID: 103
	internal class FrameAll : Manipulator
	{
		// Token: 0x0600038E RID: 910 RVA: 0x0001D46C File Offset: 0x0001B86C
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else if (evt.keyCode != 97 || evt.modifiers != null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					TimelineWindow timelineWindow = target as TimelineWindow;
					if (timelineWindow == null || timelineWindow.treeView == null)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						TrackAsset[] visibleTracks = timelineWindow.treeView.visibleTracks;
						if (visibleTracks.Length == 0)
						{
							result = base.IgnoreEvent();
						}
						else
						{
							float num = float.MaxValue;
							float num2 = float.MinValue;
							foreach (TrackAsset trackAsset in visibleTracks)
							{
								double num3;
								double num4;
								trackAsset.GetSequenceTime(ref num3, ref num4);
								num = Mathf.Min(num, (float)num3);
								num2 = Mathf.Max(num2, (float)(num3 + num4));
							}
							float num5 = num2 - Math.Max(0f, num);
							if (num5 > 0f)
							{
								state.SetTimeAreaShownRange(Mathf.Max(-10f, num - num5 * 0.1f), num2 + num5 * 0.1f);
							}
							else
							{
								state.SetTimeAreaShownRange(0f, 100f);
							}
							state.Evaluate();
							result = base.ConsumeEvent();
						}
					}
				}
				return result;
			};
		}
	}
}
