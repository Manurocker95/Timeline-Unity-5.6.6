using System;

namespace UnityEngine
{
	// Token: 0x02000005 RID: 5
	public interface IInterval
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11
		double start { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12
		double duration { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13
		// (set) Token: 0x0600000E RID: 14
		int intervalBit { get; set; }
	}
}
