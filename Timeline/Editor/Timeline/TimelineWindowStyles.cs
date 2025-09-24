using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x020000DA RID: 218
	internal class TimelineWindowStyles
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0003640C File Offset: 0x0003480C
		public static bool initialized
		{
			get
			{
				return TimelineWindowStyles.ms_Initialized;
			}
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00036426 File Offset: 0x00034826
		public static void Initialize()
		{
			TimelineWindowStyles.ms_Initialized = true;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00036430 File Offset: 0x00034830
		private static GUIContent CreateGuiContent(string name, string tooltip)
		{
			GUIContent guicontent = new GUIContent();
			guicontent.tooltip = tooltip;
			Texture2D texture2D = EditorGUIUtility.LoadIconRequired(name);
			if (texture2D)
			{
				guicontent.image = texture2D;
			}
			return guicontent;
		}

		// Token: 0x04000462 RID: 1122
		public static readonly GUIContent playContent = TimelineWindowStyles.CreateGuiContent("Animation.Play", "|Play the timeline (Space).");

		// Token: 0x04000463 RID: 1123
		public static readonly GUIContent gotoBeginingContent = TimelineWindowStyles.CreateGuiContent("Animation.FirstKey", "|Go to the beginning of the timeline. (Shift+<)");

		// Token: 0x04000464 RID: 1124
		public static readonly GUIContent gotoEndContent = TimelineWindowStyles.CreateGuiContent("Animation.LastKey", "|Go to the end of the timeline. (Shift+>)");

		// Token: 0x04000465 RID: 1125
		public static readonly GUIContent nextFrameContent = TimelineWindowStyles.CreateGuiContent("Animation.NextKey", "|Go to the next frame.");

		// Token: 0x04000466 RID: 1126
		public static readonly GUIContent previousFrameContent = TimelineWindowStyles.CreateGuiContent("Animation.PrevKey", "|Go to the previous frame.");

		// Token: 0x04000467 RID: 1127
		public static readonly GUIContent noSequenceAssetSelected = EditorGUIUtility.TextContent("To start creating a timeline, select a GameObject.");

		// Token: 0x04000468 RID: 1128
		public static readonly GUIContent createSequenceOnSelection = EditorGUIUtility.TextContent("To begin a new timeline with {0}, create {1}.");

		// Token: 0x04000469 RID: 1129
		public static readonly GUIContent emptySequenceMessage = EditorGUIUtility.TextContent("There are no tracks in this timeline.");

		// Token: 0x0400046A RID: 1130
		public static readonly GUIContent noSequencesInScene = EditorGUIUtility.TextContent("No timeline found in the scene.");

		// Token: 0x0400046B RID: 1131
		public static readonly GUIContent createNewSequenceText = EditorGUIUtility.TextContent("none");

		// Token: 0x0400046C RID: 1132
		public static readonly GUIContent newContent = EditorGUIUtility.TextContent("Add|Add new tracks.");

		// Token: 0x0400046D RID: 1133
		public static readonly GUIContent editTimelineAsAsset = EditorGUIUtility.TextContent("Edit Timeline Asset");

		// Token: 0x0400046E RID: 1134
		public static readonly GUIContent sequenceAsset = EditorGUIUtility.IconContent("TimelineAssetTab");

		// Token: 0x0400046F RID: 1135
		public static readonly GUIContent sequenceAssetEditModeTitle = EditorGUIUtility.TextContent("Sequence Asset");

		// Token: 0x04000470 RID: 1136
		public static readonly GUIContent previewContent = EditorGUIUtility.TextContent("Preview|Enable/disable scene preview mode.");

		// Token: 0x04000471 RID: 1137
		public static readonly float kBaseIndent = 15f;

		// Token: 0x04000472 RID: 1138
		public static readonly float kSequenceDefaultDuration = 10f;

		// Token: 0x04000473 RID: 1139
		public static readonly float kDurationGuiThickness = 5f;

		// Token: 0x04000474 RID: 1140
		public static readonly float kDefaultTrackHeight = 30f;

		// Token: 0x04000475 RID: 1141
		private static bool ms_Initialized = false;
	}
}
