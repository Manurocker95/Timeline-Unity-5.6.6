using System;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x020000AE RID: 174
	internal struct GUIViewportScope : IDisposable
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x0002B250 File Offset: 0x00029650
		public GUIViewportScope(Rect position)
		{
			this.m_open = false;
			if (Event.current.type == 7 || Event.current.type == 8)
			{
				GUI.BeginClip(position, -position.min, Vector2.zero, false);
				this.m_open = true;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002B2A6 File Offset: 0x000296A6
		public void Dispose()
		{
			this.CloseScope();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0002B2AF File Offset: 0x000296AF
		private void CloseScope()
		{
			if (this.m_open)
			{
				GUI.EndClip();
				this.m_open = false;
			}
		}

		// Token: 0x0400038A RID: 906
		private bool m_open;
	}
}
