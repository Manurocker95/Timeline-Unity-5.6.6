using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BD RID: 189
	internal abstract class TrackAction : MenuItemActionBase
	{
		// Token: 0x06000668 RID: 1640
		public abstract bool Execute(TimelineWindow.TimelineState state, TrackAsset[] tracks);

		// Token: 0x06000669 RID: 1641 RVA: 0x0002CB78 File Offset: 0x0002AF78
		public virtual MenuActionDisplayState GetDisplayState(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			return (tracks.Length <= 0) ? MenuActionDisplayState.Disabled : MenuActionDisplayState.Visible;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002CBA0 File Offset: 0x0002AFA0
		public virtual bool CanExecute(TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			return this.GetDisplayState(state, tracks) == MenuActionDisplayState.Visible;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002CBC0 File Offset: 0x0002AFC0
		public static bool InvokeByName(string actionName, TimelineWindow.TimelineState state, TrackAsset[] tracks)
		{
			return TrackAction.actions.First((TrackAction x) => x.GetType().Name == actionName).Execute(state, tracks);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002CC00 File Offset: 0x0002B000
		public static bool InvokeByName(string actionName, TimelineWindow.TimelineState state, TrackAsset track)
		{
			TrackAsset[] tracks = new TrackAsset[]
			{
				track
			};
			return TrackAction.actions.First((TrackAction x) => x.GetType().Name == actionName).Execute(state, tracks);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002CC4C File Offset: 0x0002B04C
		private static List<TrackAction> actions
		{
			get
			{
				if (TrackAction.s_ActionClasses == null)
				{
					TrackAction.s_ActionClasses = (from x in MenuItemActionBase.GetActionsOfType(typeof(TrackAction))
					select (TrackAction)x.GetConstructors()[0].Invoke(null)).ToList<TrackAction>();
				}
				return TrackAction.s_ActionClasses;
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002CCAC File Offset: 0x0002B0AC
		public static void AddToMenu(GenericMenu menu, TimelineWindow.TimelineState state)
		{
			TrackAsset[] tracks = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToArray<TrackAsset>();
			TrackAction.actions.ForEach(delegate(TrackAction action)
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
					MenuActionDisplayState displayState = action.GetDisplayState(state, tracks);
					if (displayState == MenuActionDisplayState.Visible)
					{
						menu.AddItem(new GUIContent(text2), false, delegate(object f)
						{
							action.Execute(state, tracks);
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

		// Token: 0x0600066F RID: 1647 RVA: 0x0002CD24 File Offset: 0x0002B124
		public static bool HandleShortcut(TimelineWindow.TimelineState state, Event evt, TrackAsset track)
		{
			List<TrackAsset> list = (from x in state.selection.FilterByType<TimelineTrackBaseGUI>()
			select x.track).ToList<TrackAsset>();
			if (!list.Any((TrackAsset x) => x == track))
			{
				list.Add(track);
			}
			foreach (TrackAction trackAction in TrackAction.actions)
			{
				object[] customAttributes = trackAction.GetType().GetCustomAttributes(typeof(ShortcutAttribute), true);
				foreach (ShortcutAttribute shortcutAttribute in customAttributes)
				{
					if (shortcutAttribute.IsRecognized(evt))
					{
						if (MenuItemActionBase.s_ShowActionTriggeredByShortcut)
						{
							Debug.Log(trackAction.GetType().Name);
						}
						return trackAction.Execute(state, list.ToArray());
					}
				}
			}
			return false;
		}

		// Token: 0x040003AA RID: 938
		private static List<TrackAction> s_ActionClasses = null;
	}
}
