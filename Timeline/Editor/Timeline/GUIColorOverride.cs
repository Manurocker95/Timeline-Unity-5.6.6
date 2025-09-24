using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x020000AD RID: 173
	internal struct GUIColorOverride : IDisposable
	{
		// Token: 0x0600060F RID: 1551 RVA: 0x0002B22B File Offset: 0x0002962B
		public GUIColorOverride(Color newColor)
		{
			this.m_OldColor = GUI.color;
			GUI.color = newColor;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002B23F File Offset: 0x0002963F
		public void Dispose()
		{
			GUI.color = this.m_OldColor;
		}

		// Token: 0x04000389 RID: 905
		private readonly Color m_OldColor;
	}
}
