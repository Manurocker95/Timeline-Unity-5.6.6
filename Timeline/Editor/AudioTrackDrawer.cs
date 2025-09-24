using System;
using UnityEditor;
using UnityEditor.Timeline;

namespace UnityEngine.Timeline
{
	// Token: 0x02000036 RID: 54
	[CustomTrackDrawer(typeof(AudioTrack))]
	internal class AudioTrackDrawer : TrackDrawer
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000117BC File Offset: 0x0000FBBC
		public override Color trackColor
		{
			get
			{
				return DirectorStyles.Instance.customSkin.colorAudio;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000117E0 File Offset: 0x0000FBE0
		public override GUIContent GetIcon()
		{
			return EditorGUIUtility.IconContent("SceneviewAudio");
		}
	}
}
