using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200005E RID: 94
	internal class Ripple
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0001B4C3 File Offset: 0x000198C3
		public Ripple(Ripple.RippleDirection direction)
		{
			this.m_Direction = direction;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001B4EC File Offset: 0x000198EC
		public void Init(TimelineClipGUI gui, TimelineWindow.TimelineState state)
		{
			TrackAsset track = gui.parentTrack.track;
			var source = state.selection.FilterByType<TimelineClipGUI>().GroupBy((TimelineClipGUI p) => p.parentTrack, (TimelineClipGUI p) => p.clip, (TimelineTrackGUI key, IEnumerable<TimelineClip> g) => new
			{
				track = key,
				clips = g.ToList<TimelineClip>()
			});
			if (!source.Any(r => r.track.track == track && r.clips[0] != gui.clip))
			{
				this.m_RippleTotal = 0f;
				List<TimelineClip> exclude = (from s in state.selection.FilterByType<TimelineClipGUI>()
				select s.clip).ToList<TimelineClip>();
				exclude.Add(gui.clip);
				if ((this.m_Direction & Ripple.RippleDirection.After) == Ripple.RippleDirection.After)
				{
					this.m_RippleAfter = (from c in track.clips
					where !exclude.Contains(c) && c.start >= gui.clip.start
					select c).ToList<TimelineClip>();
				}
				if ((this.m_Direction & Ripple.RippleDirection.Before) == Ripple.RippleDirection.Before)
				{
					this.m_RippleBefore = (from c in track.clips
					where !exclude.Contains(c) && c.start < gui.clip.start
					select c).ToList<TimelineClip>();
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0001B66C File Offset: 0x00019A6C
		public void Run(float offset, TimelineWindow.TimelineState state)
		{
			this.m_RippleTotal += offset;
			if (this.m_RippleTotal >= 0f)
			{
				this.m_AccumulatedOffsetRight += offset;
				foreach (TimelineClip timelineClip in this.m_RippleAfter)
				{
					if (timelineClip.start + (double)offset < 0.0)
					{
						break;
					}
					timelineClip.start += (double)offset;
				}
				if (this.m_AccumulatedOffsetLeft > 0f)
				{
					foreach (TimelineClip timelineClip2 in this.m_RippleBefore)
					{
						if (timelineClip2.start - (double)this.m_AccumulatedOffsetLeft < 0.0)
						{
							break;
						}
						timelineClip2.start -= (double)this.m_AccumulatedOffsetLeft;
					}
					this.m_AccumulatedOffsetLeft = 0f;
				}
			}
			else
			{
				this.m_AccumulatedOffsetLeft += offset;
				foreach (TimelineClip timelineClip3 in this.m_RippleBefore)
				{
					if (timelineClip3.start + (double)offset < 0.0)
					{
						break;
					}
					timelineClip3.start += (double)offset;
				}
				if (this.m_AccumulatedOffsetRight > 0f)
				{
					foreach (TimelineClip timelineClip4 in this.m_RippleAfter)
					{
						if (timelineClip4.start - (double)this.m_AccumulatedOffsetRight < 0.0)
						{
							break;
						}
						timelineClip4.start -= (double)this.m_AccumulatedOffsetRight;
					}
					this.m_AccumulatedOffsetRight = 0f;
				}
			}
		}

		// Token: 0x04000259 RID: 601
		private readonly Ripple.RippleDirection m_Direction;

		// Token: 0x0400025A RID: 602
		private float m_RippleTotal;

		// Token: 0x0400025B RID: 603
		private float m_AccumulatedOffsetLeft;

		// Token: 0x0400025C RID: 604
		private float m_AccumulatedOffsetRight;

		// Token: 0x0400025D RID: 605
		private List<TimelineClip> m_RippleAfter = new List<TimelineClip>();

		// Token: 0x0400025E RID: 606
		private List<TimelineClip> m_RippleBefore = new List<TimelineClip>();

		// Token: 0x0200005F RID: 95
		[Flags]
		public enum RippleDirection
		{
			// Token: 0x04000264 RID: 612
			After = 2,
			// Token: 0x04000265 RID: 613
			Before = 4,
			// Token: 0x04000266 RID: 614
			All = 6
		}
	}
}
