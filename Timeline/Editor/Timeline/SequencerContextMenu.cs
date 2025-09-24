using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000BC RID: 188
	internal class SequencerContextMenu
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0002CA10 File Offset: 0x0002AE10
		public static void Show(TrackDrawer drawer, TrackAsset track, Vector2 mousePosition)
		{
			GenericMenu genericMenu = new GenericMenu();
			TimelineAction.AddToMenu(genericMenu, TimelineWindow.instance.state);
			genericMenu.AddSeparator("");
			TrackAction.AddToMenu(genericMenu, TimelineWindow.instance.state);
			if (drawer != null && !(track is GroupTrack))
			{
				genericMenu.AddSeparator("");
				drawer.OnBuildTrackContextMenu(genericMenu, track, TimelineWindow.instance.state);
			}
			if (track is GroupTrack)
			{
				genericMenu.AddSeparator("");
				TimelineGroupGUI.AddMenuItems(genericMenu, track as GroupTrack);
			}
			genericMenu.ShowAsContext();
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002CAAC File Offset: 0x0002AEAC
		public static void Show(TrackDrawer drawer, Vector2 mousePosition)
		{
			GenericMenu genericMenu = new GenericMenu();
			TimelineAction.AddToMenu(genericMenu, TimelineWindow.instance.state);
			genericMenu.AddSeparator("");
			ClipAction.AddToMenu(genericMenu, TimelineWindow.instance.state);
			if (drawer != null)
			{
				genericMenu.AddSeparator("");
				TimelineClip[] clips = (from x in TimelineWindow.instance.state.selection.FilterByType<TimelineClipGUI>()
				select x.clip).ToArray<TimelineClip>();
				drawer.OnBuildClipContextMenu(genericMenu, clips, TimelineWindow.instance.state);
			}
			genericMenu.ShowAsContext();
		}
	}
}
