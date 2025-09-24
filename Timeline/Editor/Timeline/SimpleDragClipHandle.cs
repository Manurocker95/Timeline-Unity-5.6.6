using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000061 RID: 97
	internal class SimpleDragClipHandle : DragClipHandle
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0001C444 File Offset: 0x0001A844
		protected override void OnMouseDrag(Event evt, TimelineWindow.TimelineState state, TimelineClipHandle handle)
		{
			float num = evt.delta.x / state.timeAreaScale.x;
			TimelineClipHandle.DragDirection direction = handle.direction;
			if (direction != TimelineClipHandle.DragDirection.Right)
			{
				if (direction == TimelineClipHandle.DragDirection.Left)
				{
					handle.clip.clip.start += (double)num;
					handle.clip.clip.duration -= (double)num;
				}
			}
			else
			{
				handle.clip.clip.duration += (double)num;
			}
			if (this.m_MagnetEngine != null && evt.modifiers != 1)
			{
				this.m_MagnetEngine.Snap(evt.delta.x);
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001C510 File Offset: 0x0001A910
		protected override void OnAttractedEdge(TimelineClip clip, AttractedEdge edge, double time, double duration)
		{
			if (edge != AttractedEdge.Right)
			{
				if (edge != AttractedEdge.Left)
				{
					clip.start = time;
					clip.duration = duration;
				}
				else
				{
					double num = time - clip.start;
					clip.duration -= num;
				}
			}
			else
			{
				clip.duration = time - clip.start;
				clip.duration = Math.Max(clip.duration, TimelineClip.kMinDuration);
			}
		}
	}
}
