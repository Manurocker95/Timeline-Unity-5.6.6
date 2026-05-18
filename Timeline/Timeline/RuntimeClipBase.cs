using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000018 RID: 24
	internal abstract class RuntimeClipBase : IInterval
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BA RID: 186
		public abstract double start { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BB RID: 187
		public abstract double duration { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000380C File Offset: 0x00001A0C
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003826 File Offset: 0x00001A26
		public int intervalBit { get; set; }

		// Token: 0x1700004E RID: 78
		// (set) Token: 0x060000BE RID: 190
		public abstract bool enable { set; }

		// Token: 0x060000BF RID: 191
		public abstract void EvaluateAt(double localTime, double deltaTime, bool forceSeek);

		// Token: 0x0400006D RID: 109
		internal static readonly double kTimeEpsilon = 2E-14;
	}
}
