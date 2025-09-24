using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B3 RID: 179
	internal class StyleNormalColorOverride : IDisposable
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x0002BE0C File Offset: 0x0002A20C
		public StyleNormalColorOverride(GUIStyle style, Color newColor)
		{
			this.m_Style = style;
			this.m_OldColor = style.normal.textColor;
			style.normal.textColor = newColor;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002BE39 File Offset: 0x0002A239
		public void Dispose()
		{
			this.m_Style.normal.textColor = this.m_OldColor;
		}

		// Token: 0x0400039C RID: 924
		private readonly GUIStyle m_Style;

		// Token: 0x0400039D RID: 925
		private readonly Color m_OldColor;
	}
}
