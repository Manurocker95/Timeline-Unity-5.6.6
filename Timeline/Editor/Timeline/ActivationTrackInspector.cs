using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200001C RID: 28
	[CustomEditor(typeof(ActivationTrack))]
	internal class ActivationTrackInspector : TrackAssetInspector
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000DC7C File Offset: 0x0000C07C
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			EditorGUI.BeginChangeCheck();
			if (this.m_PostPlaybackProperty != null)
			{
				EditorGUILayout.PropertyField(this.m_PostPlaybackProperty, ActivationTrackInspector.Styles.PostPlaybackStateText, new GUILayoutOption[0]);
			}
			if (EditorGUI.EndChangeCheck())
			{
				base.serializedObject.ApplyModifiedProperties();
				ActivationTrack activationTrack = base.target as ActivationTrack;
				if (activationTrack != null)
				{
					activationTrack.UpdateTrackMode();
				}
			}
			base.OnInspectorGUI();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000DCF8 File Offset: 0x0000C0F8
		public override void OnEnable()
		{
			base.OnEnable();
			this.m_PostPlaybackProperty = base.serializedObject.FindProperty("m_PostPlaybackState");
		}

		// Token: 0x04000142 RID: 322
		private SerializedProperty m_PostPlaybackProperty;

		// Token: 0x0200001D RID: 29
		private static class Styles
		{
			// Token: 0x04000143 RID: 323
			public static readonly GUIContent PostPlaybackStateText = EditorGUIUtility.TextContent("Post-playback state");
		}
	}
}
