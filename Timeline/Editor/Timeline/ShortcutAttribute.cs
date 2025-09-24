using System;
using System.Text;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class ShortcutAttribute : Attribute
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x00011618 File Offset: 0x0000FA18
		public ShortcutAttribute(EventModifiers modifiers, KeyCode key)
		{
			this.m_Modifiers = modifiers;
			this.m_KeyCode = key;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001163D File Offset: 0x0000FA3D
		public ShortcutAttribute(KeyCode key)
		{
			this.m_KeyCode = key;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0001165C File Offset: 0x0000FA5C
		public bool IsRecognized(Event evt)
		{
			bool result;
			if (evt.modifiers == 64)
			{
				result = (evt.keyCode == this.m_KeyCode);
			}
			else
			{
				result = (evt.modifiers == this.m_Modifiers && evt.keyCode == this.m_KeyCode);
			}
			return result;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000116B4 File Offset: 0x0000FAB4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.m_Modifiers != null)
			{
				if ((this.m_Modifiers & 1) == 1)
				{
					stringBuilder.Append("Shift");
				}
				else if (Application.platform == 7 && (this.m_Modifiers & 2) == 2)
				{
					stringBuilder.Append("Ctrl");
				}
				else if (Application.platform == null && (this.m_Modifiers & 8) == 8)
				{
					stringBuilder.Append("Cmd");
				}
				else if ((this.m_Modifiers & 4) == 4)
				{
					stringBuilder.Append("Alt");
				}
			}
			if (this.m_KeyCode != null)
			{
				if (this.m_Modifiers != null)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(Enum.GetName(typeof(KeyCode), this.m_KeyCode));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000195 RID: 405
		private EventModifiers m_Modifiers = 0;

		// Token: 0x04000196 RID: 406
		private KeyCode m_KeyCode = 0;
	}
}
