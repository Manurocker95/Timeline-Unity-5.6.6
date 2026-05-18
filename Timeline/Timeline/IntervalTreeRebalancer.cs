using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200001A RID: 26
	internal class IntervalTreeRebalancer
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x000056E6 File Offset: 0x000038E6
		public IntervalTreeRebalancer(IntervalTree tree)
		{
			this.m_Tree = tree;
			this.m_Hash = this.m_Tree.GetHashCode();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005708 File Offset: 0x00003908
		public bool Rebalance()
		{
			int hashCode = this.m_Tree.GetHashCode();
			bool result;
			if (this.m_Hash != hashCode)
			{
				this.m_Tree.dirty = true;
				this.m_Hash = hashCode;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0400006F RID: 111
		private IntervalTree m_Tree;

		// Token: 0x04000070 RID: 112
		private int m_Hash;
	}
}
