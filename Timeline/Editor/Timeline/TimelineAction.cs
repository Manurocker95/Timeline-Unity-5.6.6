using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B4 RID: 180
	internal abstract class TimelineAction : MenuItemActionBase
	{
		// Token: 0x0600063E RID: 1598
		public abstract bool Execute(TimelineWindow.TimelineState state);

		// Token: 0x0600063F RID: 1599 RVA: 0x0002BE5C File Offset: 0x0002A25C
		public virtual MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state)
		{
			return MenuActionDisplayState.Visible;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002BE74 File Offset: 0x0002A274
		public virtual bool CanExecute(TimelineWindow.TimelineState state)
		{
			return this.GetDisplayState(state) == MenuActionDisplayState.Visible;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0002BE94 File Offset: 0x0002A294
		public static bool InvokeByName(string actionName, TimelineWindow.TimelineState state)
		{
			TimelineAction timelineAction = TimelineAction.actions.FirstOrDefault((TimelineAction x) => x.GetType().Name == actionName);
			return timelineAction != null && timelineAction.Execute(state);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0002BEE4 File Offset: 0x0002A2E4
		private static List<TimelineAction> actions
		{
			get
			{
				if (TimelineAction.s_ActionClasses == null)
				{
					TimelineAction.s_ActionClasses = (from x in MenuItemActionBase.GetActionsOfType(typeof(TimelineAction))
					select (TimelineAction)x.GetConstructors()[0].Invoke(null)).ToList<TimelineAction>();
				}
				return TimelineAction.s_ActionClasses;
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002BF44 File Offset: 0x0002A344
		public static void AddToMenu(GenericMenu menu, TimelineWindow.TimelineState state)
		{
			TimelineAction.actions.ForEach(delegate(TimelineAction action)
			{
				string text = string.Empty;
				CategoryAttribute categoryAttribute = MenuItemActionBase.GetCategoryAttribute(action);
				if (categoryAttribute == null)
				{
					text = string.Empty;
				}
				else
				{
					text = categoryAttribute.Category;
					if (!text.EndsWith("/"))
					{
						text += "/";
					}
				}
				string displayName = MenuItemActionBase.GetDisplayName(action);
				string text2 = text + displayName;
				SeparatorMenuItemAttribute separator = MenuItemActionBase.GetSeparator(action);
				bool flag = !MenuItemActionBase.IsHiddenInMenu(action);
				if (flag)
				{
					MenuActionDisplayState displayState = action.GetDisplayState(state);
					if (displayState == MenuActionDisplayState.Visible)
					{
						menu.AddItem(new GUIContent(text2), false, delegate(object f)
						{
							action.Execute(state);
						}, action);
					}
					if (displayState == MenuActionDisplayState.Disabled)
					{
						menu.AddDisabledItem(new GUIContent(text2));
					}
					if (displayState != MenuActionDisplayState.Hidden && separator != null && separator.after)
					{
						menu.AddSeparator(text);
					}
				}
			});
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0002BF7C File Offset: 0x0002A37C
		public static bool HandleShortcut(TimelineWindow.TimelineState state, Event evt)
		{
			foreach (TimelineAction timelineAction in TimelineAction.actions)
			{
				object[] customAttributes = timelineAction.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);
				foreach (ShortcutAttribute shortcutAttribute in customAttributes)
				{
					if (shortcutAttribute.IsRecognized(evt))
					{
						if (MenuItemActionBase.s_ShowActionTriggeredByShortcut)
						{
							Debug.Log(timelineAction.GetType().Name);
						}
						return timelineAction.Execute(state);
					}
				}
			}
			return false;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002C054 File Offset: 0x0002A454
		protected static bool DoInternal(Type t, TimelineWindow.TimelineState state)
		{
			TimelineAction timelineAction = (TimelineAction)t.GetConstructors()[0].Invoke(null);
			return timelineAction.CanExecute(state) && timelineAction.Execute(state);
		}

		// Token: 0x0400039E RID: 926
		private static List<TimelineAction> s_ActionClasses = null;
	}
}
