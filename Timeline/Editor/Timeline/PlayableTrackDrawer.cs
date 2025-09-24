using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000051 RID: 81
	[CustomTrackDrawer(typeof(PlayableTrack))]
	internal class PlayableTrackDrawer : TrackDrawer
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00018F34 File Offset: 0x00017334
		public override Color trackColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00018F50 File Offset: 0x00017350
		public override GUIContent GetIcon()
		{
			return EditorGUIUtility.IconContent("cs Script Icon");
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00018F70 File Offset: 0x00017370
		public override void OnBuildTrackContextMenu(GenericMenu menu, TrackAsset track, ITimelineState state)
		{
			Type[] allStandalonePlayableAssets = TimelineHelpers.GetAllStandalonePlayableAssets();
			foreach (Type type in allStandalonePlayableAssets)
			{
				if (!type.IsDefined(typeof(HideInMenuAttribute), true))
				{
					string displayName = TrackDrawer.GetDisplayName(type);
					GUIContent guicontent = new GUIContent("Add Clip/" + displayName);
					menu.AddItem(guicontent, false, delegate(object userData)
					{
						TimelineHelpers.CreateClipOnTrack(userData as Type, track, state);
					}, type);
				}
			}
		}
	}
}
