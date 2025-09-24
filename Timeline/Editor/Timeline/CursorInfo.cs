using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000056 RID: 86
	internal class CursorInfo
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0001996C File Offset: 0x00017D6C
		public CursorInfo()
		{
			this.bounds = default(Rect);
			this.cursor = 0;
			ulong num = CursorInfo.ms_Counter;
			CursorInfo.ms_Counter = num + 1UL;
			this.m_Id = num;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000300 RID: 768 RVA: 0x000199AC File Offset: 0x00017DAC
		public ulong ID
		{
			get
			{
				return this.m_Id;
			}
		}

		// Token: 0x04000226 RID: 550
		public Rect bounds;

		// Token: 0x04000227 RID: 551
		public MouseCursor cursor;

		// Token: 0x04000228 RID: 552
		private ulong m_Id;

		// Token: 0x04000229 RID: 553
		private static ulong ms_Counter = 0UL;
	}
}
