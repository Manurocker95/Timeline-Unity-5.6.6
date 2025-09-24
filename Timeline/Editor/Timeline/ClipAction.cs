using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000098 RID: 152
	internal abstract class ClipAction : MenuItemActionBase
	{
		// Token: 0x060005C4 RID: 1476
		public abstract bool Execute(TimelineWindow.TimelineState state, TimelineClip[] clips);

		// Token: 0x060005C5 RID: 1477 RVA: 0x0002A1B8 File Offset: 0x000285B8
		public virtual MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return (clips.Length <= 0) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0002A1E0 File Offset: 0x000285E0
		public virtual bool CanExecute(TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			return this.GetDisplayState(state, clips) == MenuActionDisplayState.Visible;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0002A200 File Offset: 0x00028600
		public static bool InvokeByName(string actionName, TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			ClipAction clipAction = ClipAction.actions.First((ClipAction x) => x.GetType().Name == actionName);
			return clipAction != null && clipAction.Execute(state, clips);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002A250 File Offset: 0x00028650
		public static bool InvokeByName(string actionName, TimelineWindow.TimelineState state, TimelineClip clip)
		{
			TimelineClip[] clips = new TimelineClip[]
			{
				clip
			};
			ClipAction clipAction = ClipAction.actions.First((ClipAction x) => x.GetType().Name == actionName);
			return clipAction != null && clipAction.Execute(state, clips);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0002A2AC File Offset: 0x000286AC
		public static bool HandleShortcut(TimelineWindow.TimelineState state, Event evt, TimelineClip clip)
		{
			TimelineClip[] clips = new TimelineClip[]
			{
				clip
			};
			foreach (ClipAction clipAction in ClipAction.actions)
			{
				object[] customAttributes = clipAction.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);
				foreach (ShortcutAttribute shortcutAttribute in customAttributes)
				{
					if (shortcutAttribute.IsRecognized(evt))
					{
						if (MenuItemActionBase.s_ShowActionTriggeredByShortcut)
						{
							Debug.Log(clipAction.GetType().Name);
						}
						bool result = clipAction.Execute(state, clips);
						state.Refresh();
						state.Evaluate();
						return result;
					}
				}
			}
			return false;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0002A3A4 File Offset: 0x000287A4
		private static List<ClipAction> actions
		{
			get
			{
				if (ClipAction.s_ActionClasses == null)
				{
					ClipAction.s_ActionClasses = (from x in MenuItemActionBase.GetActionsOfType(typeof(ClipAction))
					select (ClipAction)x.GetConstructors()[0].Invoke(null)).ToList<ClipAction>();
				}
				return ClipAction.s_ActionClasses;
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0002A408 File Offset: 0x00028808
		public static void AddToMenu(GenericMenu menu, TimelineWindow.TimelineState state)
		{
			TimelineClip[] clips = (from x in state.selection.FilterByType<TimelineClipGUI>()
			select x.clip).ToArray<TimelineClip>();
			ClipAction.actions.ForEach(delegate(ClipAction action)
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
					MenuActionDisplayState displayState = action.GetDisplayState(state, clips);
					if (displayState == MenuActionDisplayState.Visible)
					{
						if (separator != null && separator.before)
						{
							menu.AddSeparator(text);
						}
						menu.AddItem(new GUIContent(text2), false, delegate(object f)
						{
							action.Execute(state, clips);
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

		// Token: 0x060005CC RID: 1484 RVA: 0x0002A480 File Offset: 0x00028880
		protected static bool DoInternal(Type t, TimelineWindow.TimelineState state, TimelineClip[] clips)
		{
			ClipAction clipAction = (ClipAction)t.GetConstructors()[0].Invoke(null);
			return clipAction.CanExecute(state, clips) && clipAction.Execute(state, clips);
		}

		// Token: 0x0400037F RID: 895
		private static List<ClipAction> s_ActionClasses = null;
	}
}
