using System;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000058 RID: 88
	internal class FrameSnap
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000334 RID: 820 RVA: 0x000199D8 File Offset: 0x00017DD8
		public double lastOffsetApplied
		{
			get
			{
				return this.m_LastOffsetApplied;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000199F3 File Offset: 0x00017DF3
		public void Reset()
		{
			this.m_CurrentOffset = 0.0;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00019A08 File Offset: 0x00017E08
		public double ApplyOffset(double currentValue, float delta, TimelineWindow.TimelineState state)
		{
			if (state.frameSnap)
			{
				double num = currentValue;
				this.m_LastOffsetApplied = 0.0;
				bool flag = true;
				this.m_CurrentOffset += (double)delta;
				if (!TimeUtility.OnFrameBoundary(currentValue, (double)state.frameRate))
				{
					double num2 = TimeUtility.ToExactFrames(currentValue, (double)state.frameRate) - (double)TimeUtility.ToFrames(currentValue, (double)state.frameRate);
					double num3 = TimeUtility.FromFrames(num2, (double)state.frameRate);
					if (Math.Abs(this.m_CurrentOffset) >= Math.Abs(num3))
					{
						currentValue += num3;
						this.m_CurrentOffset -= num3;
					}
					else
					{
						flag = false;
					}
				}
				if (flag)
				{
					double num4 = (double)TimeUtility.ToFrames(this.m_CurrentOffset, (double)state.frameRate);
					this.m_CurrentOffset = TimeUtility.FromFrames(TimeUtility.ToExactFrames(this.m_CurrentOffset, (double)state.frameRate) - num4, (double)state.frameRate);
					currentValue = TimeUtility.FromFrames(num4 + (double)TimeUtility.ToFrames(currentValue, (double)state.frameRate), (double)state.frameRate);
				}
				this.m_LastOffsetApplied = currentValue - num;
			}
			else
			{
				this.m_LastOffsetApplied = (double)delta;
				currentValue += (double)delta;
			}
			return currentValue;
		}

		// Token: 0x0400023E RID: 574
		private double m_CurrentOffset;

		// Token: 0x0400023F RID: 575
		private double m_LastOffsetApplied;
	}
}
