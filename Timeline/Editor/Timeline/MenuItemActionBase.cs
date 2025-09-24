using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B1 RID: 177
	internal class MenuItemActionBase
	{
		// Token: 0x06000625 RID: 1573 RVA: 0x00029F8C File Offset: 0x0002838C
		protected static IEnumerable<Type> GetActionsOfType(Type actionType)
		{
			return from type in EditorAssemblies.loadedTypes
			where !type.IsGenericType && !type.IsNested && !type.IsAbstract && type.IsSubclassOf(actionType)
			select type;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00029FC8 File Offset: 0x000283C8
		protected static string GetDisplayName(MenuItemActionBase action)
		{
			object[] customAttributes = action.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);
			object[] customAttributes2 = action.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), true);
			StringBuilder stringBuilder = new StringBuilder();
			if (customAttributes2.Length > 0)
			{
				stringBuilder.Append((customAttributes2[0] as DisplayNameAttribute).DisplayName);
			}
			else
			{
				stringBuilder.Append(action.GetType().Name);
			}
			if (customAttributes.Length > 0)
			{
				stringBuilder.Append("\t\t");
			}
			for (int num = 0; num != customAttributes.Length; num++)
			{
				if (num > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(customAttributes[num].ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0002A09C File Offset: 0x0002849C
		protected static CategoryAttribute GetCategoryAttribute(MenuItemActionBase action)
		{
			object[] customAttributes = action.GetType().GetCustomAttributes(typeof(CategoryAttribute), true);
			CategoryAttribute result;
			if (customAttributes.Length > 0)
			{
				result = (customAttributes[0] as CategoryAttribute);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002A0E0 File Offset: 0x000284E0
		protected static SeparatorMenuItemAttribute GetSeparator(MenuItemActionBase action)
		{
			object[] customAttributes = action.GetType().GetCustomAttributes(typeof(SeparatorMenuItemAttribute), true);
			SeparatorMenuItemAttribute result;
			if (customAttributes.Length > 0)
			{
				result = (customAttributes[0] as SeparatorMenuItemAttribute);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002A124 File Offset: 0x00028524
		protected static bool IsHiddenInMenu(MenuItemActionBase action)
		{
			object[] customAttributes = action.GetType().GetCustomAttributes(typeof(HideInMenuAttribute), true);
			return customAttributes.Length > 0;
		}

		// Token: 0x0400039A RID: 922
		protected static bool s_ShowActionTriggeredByShortcut = false;
	}
}
