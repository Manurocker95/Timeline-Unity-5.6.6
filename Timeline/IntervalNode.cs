using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
	// Token: 0x02000006 RID: 6
	internal class IntervalNode
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000031E0 File Offset: 0x000013E0
		public IntervalNode(IEnumerable<IInterval> items)
		{
			List<double> list = new List<double>();
			foreach (IInterval interval in items)
			{
				list.Add(interval.start);
				list.Add(interval.start + interval.duration);
			}
			if (list.Count != 0)
			{
				list.Sort();
				this.m_Center = list[list.Count / 2];
				this.m_Children = new List<IInterval>();
				List<IInterval> list2 = new List<IInterval>();
				List<IInterval> list3 = new List<IInterval>();
				foreach (IInterval interval2 in items)
				{
					double start = interval2.start;
					double num = interval2.start + interval2.duration;
					if (num < this.m_Center)
					{
						list2.Add(interval2);
					}
					else if (start > this.m_Center)
					{
						list3.Add(interval2);
					}
					else
					{
						this.m_Children.Add(interval2);
					}
				}
				if (this.m_Children.Count > 0)
				{
					this.m_Children = (from c in this.m_Children
					orderby c.start
					select c).ToList<IInterval>();
				}
				else
				{
					this.m_Children = null;
				}
				if (list2.Count > 0)
				{
					this.m_LeftNode = new IntervalNode(list2);
				}
				if (list3.Count > 0)
				{
					this.m_RightNode = new IntervalNode(list3);
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000033C4 File Offset: 0x000015C4
		public void Query<T>(double time, int bitflag, ref List<T> results) where T : class, IInterval
		{
			if (this.m_Children != null)
			{
				for (int i = 0; i < this.m_Children.Count; i++)
				{
					T t = this.m_Children[i] as T;
					if (t != null)
					{
						double start = t.start;
						double value = t.start + t.duration;
						if (start.CompareTo(time) > 0)
						{
							break;
						}
						if (time.CompareTo(start) >= 0 && time.CompareTo(value) < 0)
						{
							t.intervalBit = bitflag;
							results.Add(t);
						}
					}
				}
			}
			if (time.CompareTo(this.m_Center) < 0 && this.m_LeftNode != null)
			{
				this.m_LeftNode.Query<T>(time, bitflag, ref results);
			}
			else if (time.CompareTo(this.m_Center) > 0 && this.m_RightNode != null)
			{
				this.m_RightNode.Query<T>(time, bitflag, ref results);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000034F4 File Offset: 0x000016F4
		public void Query<T>(double start, double duration, int bitflag, ref List<T> results) where T : class, IInterval
		{
			double value = start;
			double value2 = start + duration;
			if (this.m_Children != null)
			{
				for (int i = 0; i < this.m_Children.Count; i++)
				{
					T t = this.m_Children[i] as T;
					if (t != null)
					{
						double start2 = t.start;
						double num = t.start + t.duration;
						if (start2.CompareTo(value2) > 0)
						{
							break;
						}
						if (value2.CompareTo(start2) >= 0 && num.CompareTo(value) >= 0)
						{
							t.intervalBit = bitflag;
							results.Add(t);
						}
					}
				}
			}
			if (value.CompareTo(this.m_Center) < 0 && this.m_LeftNode != null)
			{
				this.m_LeftNode.Query<T>(start, duration, bitflag, ref results);
			}
			if (value2.CompareTo(this.m_Center) > 0 && this.m_RightNode != null)
			{
				this.m_RightNode.Query<T>(start, duration, bitflag, ref results);
			}
		}

		// Token: 0x04000009 RID: 9
		private double m_Center;

		// Token: 0x0400000A RID: 10
		private List<IInterval> m_Children;

		// Token: 0x0400000B RID: 11
		private IntervalNode m_LeftNode;

		// Token: 0x0400000C RID: 12
		private IntervalNode m_RightNode;
	}
}
