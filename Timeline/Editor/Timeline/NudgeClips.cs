using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200006E RID: 110
	internal class NudgeClips : Manipulator
	{
		// Token: 0x060003AC RID: 940 RVA: 0x0001DF44 File Offset: 0x0001C344
		public override void Init(IControl parent)
		{
			parent.KeyDown += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				bool result;
				if (state.IsEditingASubItem())
				{
					result = base.IgnoreEvent();
				}
				else if ((evt.modifiers & 15) != null)
				{
					result = base.IgnoreEvent();
				}
				else
				{
					bool flag = evt.keyCode == 49 || evt.keyCode == 257;
					bool flag2 = evt.keyCode == 50 || evt.keyCode == 258;
					if (!evt.isKey || (!flag && !flag2))
					{
						result = base.IgnoreEvent();
					}
					else if (state.selection.isEmpty)
					{
						result = base.IgnoreEvent();
					}
					else
					{
						double offset = (!flag2) ? -1.0 : 1.0;
						bool flag3 = false;
						foreach (TimelineClipGUI timelineClipGUI in state.selection.EditableClips())
						{
							flag3 |= this.NudgeClip(timelineClipGUI.clip, state, offset);
						}
						if (flag3)
						{
							state.Evaluate();
							result = base.ConsumeEvent();
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

		// Token: 0x060003AD RID: 941 RVA: 0x0001DF5C File Offset: 0x0001C35C
		private bool NudgeClip(TimelineClip clip, TimelineWindow.TimelineState state, double offset)
		{
			bool result;
			if (clip == null)
			{
				result = false;
			}
			else
			{
				TimelineHelpers.PushUndo(clip.parentTrack, "nudge.clip");
				if (state.frameSnap)
				{
					clip.start = TimeUtility.FromFrames((double)TimeUtility.ToFrames(clip.start, (double)state.frameRate) + offset, (double)state.frameRate);
				}
				else
				{
					clip.start += offset / (double)state.frameRate;
				}
				EditorUtility.SetDirty(clip.parentTrack);
				result = true;
			}
			return result;
		}
	}
}
