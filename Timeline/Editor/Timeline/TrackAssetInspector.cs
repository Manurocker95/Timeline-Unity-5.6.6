using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000047 RID: 71
	[CustomEditor(typeof(TrackAsset), true)]
	internal class TrackAssetInspector : Editor
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000DA54 File Offset: 0x0000BE54
		protected TimelineWindow sequencerWindow
		{
			get
			{
				return TimelineWindow.instance;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000DA6E File Offset: 0x0000BE6E
		public override void OnInspectorGUI()
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000DA74 File Offset: 0x0000BE74
		internal override void OnHeaderTitleGUI(Rect titleRect, string header)
		{
			this.m_Name = base.serializedObject.FindProperty("m_Name");
			base.serializedObject.Update();
			Rect rect = titleRect;
			rect.height = 16f;
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = this.m_Name.hasMultipleDifferentValues;
			TimelineWindow instance = TimelineWindow.instance;
			bool flag = instance == null || instance.state == null || instance.state.currentDirector == null;
			EditorGUI.BeginDisabledGroup(flag);
			EditorGUI.BeginChangeCheck();
			string text = EditorGUI.DelayedTextField(rect, this.m_Name.stringValue, EditorStyles.textField);
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck() && !string.IsNullOrEmpty(text))
			{
				for (int i = 0; i < base.targets.Count<Object>(); i++)
				{
					ObjectNames.SetNameSmart(base.targets[i], text);
				}
				if (instance != null)
				{
					instance.Repaint();
				}
			}
			EditorGUI.EndDisabledGroup();
			base.serializedObject.ApplyModifiedProperties();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DB8C File Offset: 0x0000BF8C
		internal override void OnHeaderIconGUI(Rect iconRect)
		{
			if (!(TimelineWindow.instance == null))
			{
				TimelineTrackBaseGUI timelineTrackBaseGUI = TimelineWindow.instance.allTracks.Find((TimelineTrackBaseGUI uiTrack) => uiTrack.track == base.target as TrackAsset);
				if (timelineTrackBaseGUI != null)
				{
					GUI.Label(iconRect, timelineTrackBaseGUI.drawer.GetIcon());
				}
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000DBE4 File Offset: 0x0000BFE4
		internal override void DrawHeaderHelpAndSettingsGUI(Rect r)
		{
			Vector2 vector = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
			Object target = base.target;
			EditorGUI.HelpIconButton(new Rect(r.xMax - vector.x, r.y + 5f, vector.x, vector.y), target);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000DC3F File Offset: 0x0000C03F
		public virtual void OnEnable()
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000DC42 File Offset: 0x0000C042
		public virtual void OnDestroy()
		{
		}

		// Token: 0x040001E0 RID: 480
		private SerializedProperty m_Name;
	}
}
