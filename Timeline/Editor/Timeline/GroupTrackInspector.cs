using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000042 RID: 66
	[CustomEditor(typeof(GroupTrack))]
	internal class GroupTrackInspector : TrackAssetInspector
	{
		// Token: 0x0600024A RID: 586 RVA: 0x000149B0 File Offset: 0x00012DB0
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GroupTrack groupTrack = base.target as GroupTrack;
			List<TrackAsset> subTracks = groupTrack.subTracks;
			GUILayout.Label((subTracks.Count <= 0) ? GroupTrackInspector.EmptyGroupContent.text : string.Concat(new object[]
			{
				GroupTrackInspector.SubTracksContent.text,
				" (",
				subTracks.Count,
				")"
			}), EditorStyles.boldLabel, new GUILayoutOption[0]);
			GUILayout.Space(3f);
			EditorGUI.indentLevel++;
			this.m_SubTracks.list = groupTrack.subTracks;
			this.m_SubTracks.DoLayoutList();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00014A6C File Offset: 0x00012E6C
		public override void OnEnable()
		{
			string[] array = new string[]
			{
				"allo",
				"aaaaa"
			};
			this.m_SubTracks = new ReorderableList(array, typeof(string), true, true, false, false);
			this.m_SubTracks.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.OnDrawSubTrack);
			this.m_SubTracks.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.OnDrawHeader);
			this.m_SubTracks.showDefaultBackground = true;
			this.m_SubTracks.index = 0;
			this.m_SubTracks.elementHeight = 20f;
			this.m_SubTracks.draggable = false;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00014B10 File Offset: 0x00012F10
		private void OnDrawHeader(Rect rect)
		{
			int num = 4;
			float num2 = rect.width / (float)num;
			rect.width = num2;
			GUI.Label(rect, "Name", EditorStyles.label);
			rect.x += num2;
			GUI.Label(rect, "Type", EditorStyles.label);
			rect.x += num2;
			GUI.Label(rect, "Duration", EditorStyles.label);
			rect.x += num2;
			GUI.Label(rect, "Frames", EditorStyles.label);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00014BA0 File Offset: 0x00012FA0
		private void OnDrawSubTrack(Rect rect, int index, bool selected, bool focused)
		{
			TrackAsset trackAsset = base.target as TrackAsset;
			List<TrackAsset> subTracks = trackAsset.subTracks;
			int num = 4;
			float num2 = rect.width / (float)num;
			rect.width = num2;
			GUI.Label(rect, subTracks[index].name, EditorStyles.label);
			rect.x += num2;
			GUI.Label(rect, subTracks[index].GetType().Name, EditorStyles.label);
			rect.x += num2;
			GUI.Label(rect, subTracks[index].duration.ToString(), EditorStyles.label);
			rect.x += num2;
			double num3 = TimeUtility.ToExactFrames(subTracks[index].duration, (double)TimelineWindow.instance.state.frameRate);
			GUI.Label(rect, num3.ToString(), EditorStyles.label);
		}

		// Token: 0x040001CF RID: 463
		public static readonly GUIContent SubTracksContent = EditorGUIUtility.TextContent("Sub Tracks");

		// Token: 0x040001D0 RID: 464
		public static readonly GUIContent EmptyGroupContent = EditorGUIUtility.TextContent("Group Is Empty");

		// Token: 0x040001D1 RID: 465
		private ReorderableList m_SubTracks;
	}
}
