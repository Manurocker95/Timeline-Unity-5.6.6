using System;
using System.Collections.Generic;

namespace UnityEngine
{
	// Token: 0x02000007 RID: 7
	public class IntervalTree
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000366C File Offset: 0x0000186C
		public List<T> AllocateCache<T>(int maximumNumberOfIntersections)
		{
			return new List<T>(maximumNumberOfIntersections);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00003688 File Offset: 0x00001888
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000036A3 File Offset: 0x000018A3
		public bool dirty
		{
			get
			{
				return this.m_Dirty;
			}
			set
			{
				this.m_Dirty = true;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000036AD File Offset: 0x000018AD
		public void Add(IInterval item)
		{
			this.m_Nodes.Add(item);
			this.m_Dirty = true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000036C3 File Offset: 0x000018C3
		public void Add(IEnumerable<IInterval> items)
		{
			this.m_Nodes.AddRange(items);
			this.m_Dirty = true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000036D9 File Offset: 0x000018D9
		public void Clear()
		{
			this.m_Nodes.Clear();
			this.m_Dirty = true;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000036F0 File Offset: 0x000018F0
		public bool Contains(IInterval interval)
		{
			return this.m_Nodes.Contains(interval);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003711 File Offset: 0x00001911
		public void IntersectsWith<T>(double value, int bitFlag, ref List<T> results) where T : class, IInterval
		{
			if (this.m_Dirty)
			{
				this.Rebuild();
			}
			this.m_Root.Query<T>(value, bitFlag, ref results);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003735 File Offset: 0x00001935
		public void IntersectsWith<T>(double value, double duration, int bitFlag, ref List<T> results) where T : class, IInterval
		{
			if (this.m_Dirty)
			{
				this.Rebuild();
			}
			this.m_Root.Query<T>(value, duration, bitFlag, ref results);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000375B File Offset: 0x0000195B
		private void Rebuild()
		{
			this.m_Root = new IntervalNode(this.m_Nodes);
			this.m_Dirty = false;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003778 File Offset: 0x00001978
		public override int GetHashCode()
		{
			int num = this.m_Dirty.GetHashCode();
			for (int i = 0; i < this.m_Nodes.Count; i++)
			{
				num ^= (this.m_Nodes[i].start.GetHashCode() ^ this.m_Nodes[i].duration.GetHashCode());
			}
			return num;
		}

		// Token: 0x0400000E RID: 14
		private List<IInterval> m_Nodes = new List<IInterval>();

		// Token: 0x0400000F RID: 15
		private bool m_Dirty = true;

		// Token: 0x04000010 RID: 16
		private IntervalNode m_Root = null;
	}
}
