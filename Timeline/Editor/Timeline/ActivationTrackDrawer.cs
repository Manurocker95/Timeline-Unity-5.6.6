using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200001A RID: 26
	[CustomTrackDrawer(typeof(ActivationTrack))]
	internal class ActivationTrackDrawer : TrackDrawer
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000D920 File Offset: 0x0000BD20
		public override Color trackColor
		{
			get
			{
				return DirectorStyles.Instance.customSkin.colorActivation;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000D944 File Offset: 0x0000BD44
		public override GUIContent GetIcon()
		{
			if (ActivationTrackDrawer.s_IconContent == null)
			{
				ActivationTrackDrawer.s_IconContent = new GUIContent(DirectorStyles.Instance.activation.normal.background);
			}
			return ActivationTrackDrawer.s_IconContent;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000D988 File Offset: 0x0000BD88
		public override void OnBuildTrackContextMenu(GenericMenu menu, TrackAsset track, ITimelineState state)
		{
			if (track is ActivationTrack)
			{
				menu.AddItem(ActivationTrackDrawer.Styles.MenuText, false, delegate(object userData)
				{
					TimelineClip timelineClip = TimelineHelpers.CreateClipOnTrack(userData as Type, track, state);
					timelineClip.displayName = ActivationTrackDrawer.Styles.ClipText.text;
				}, typeof(ActivationPlayableAsset));
			}
		}

		// Token: 0x0400013F RID: 319
		private static GUIContent s_IconContent = null;

		// Token: 0x0200001B RID: 27
		internal static class Styles
		{
			// Token: 0x04000140 RID: 320
			public static readonly GUIContent MenuText = EditorGUIUtility.TextContent("Add Activation Clip");

			// Token: 0x04000141 RID: 321
			public static readonly GUIContent ClipText = EditorGUIUtility.TextContent("Active");
		}
	}
}
