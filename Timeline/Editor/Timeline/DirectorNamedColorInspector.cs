using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000040 RID: 64
	[CustomEditor(typeof(DirectorNamedColor))]
	public class DirectorNamedColorInspector : Editor
	{
		// Token: 0x06000236 RID: 566 RVA: 0x00014754 File Offset: 0x00012B54
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("ToTextAsset", new GUILayoutOption[0]))
			{
				DirectorStyles.Instance.ExportSkinToFile();
			}
			if (GUILayout.Button("Reload From File", new GUILayoutOption[0]))
			{
				DirectorStyles.Instance.ReloadSkin();
				Selection.activeObject = DirectorStyles.Instance.customSkin;
			}
		}
	}
}
