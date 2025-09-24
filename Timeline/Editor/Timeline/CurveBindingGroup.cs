using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200001F RID: 31
	internal class CurveBindingGroup
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000DD30 File Offset: 0x0000C130
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000DD4A File Offset: 0x0000C14A
		public CurveBindingPair[] curveBindingPairs { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000DD54 File Offset: 0x0000C154
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000DD6E File Offset: 0x0000C16E
		public Vector2 timeRange { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000DD78 File Offset: 0x0000C178
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000DD92 File Offset: 0x0000C192
		public Vector2 valueRange { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000DD9C File Offset: 0x0000C19C
		public bool isFloatCurve
		{
			get
			{
				return this.curveBindingPairs != null && this.curveBindingPairs.Length > 0 && this.curveBindingPairs[0].curve != null;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000DDE4 File Offset: 0x0000C1E4
		public bool isObjectCurve
		{
			get
			{
				return this.curveBindingPairs != null && this.curveBindingPairs.Length > 0 && this.curveBindingPairs[0].objectCurve != null;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000DE2C File Offset: 0x0000C22C
		public int count
		{
			get
			{
				int result;
				if (this.curveBindingPairs == null)
				{
					result = 0;
				}
				else
				{
					result = this.curveBindingPairs.Length;
				}
				return result;
			}
		}
	}
}
