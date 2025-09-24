using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000043 RID: 67
	[CustomEditor(typeof(TimelineAsset))]
	internal class TimelineAssetInspector : Editor
	{
		// Token: 0x06000250 RID: 592 RVA: 0x00014CC0 File Offset: 0x000130C0
		private void InitializeProperties()
		{
			this.m_UpdateModeProperty = base.serializedObject.FindProperty("m_UpdateMode");
			this.m_ExtrapolationModeProperty = base.serializedObject.FindProperty("m_ExtrapolationMode");
			this.m_FrameRateProperty = base.serializedObject.FindProperty("m_EditorSettings").FindPropertyRelative("fps");
			this.m_DurationModeProperty = base.serializedObject.FindProperty("m_DurationMode");
			this.m_FixedDurationProperty = base.serializedObject.FindProperty("m_FixedDuration");
			this.m_ParameterNameProperty = base.serializedObject.FindProperty("m_ParameterName");
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00014D5C File Offset: 0x0001315C
		public void OnEnable()
		{
			this.InitializeProperties();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00014D68 File Offset: 0x00013168
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			EditorGUILayout.PropertyField(this.m_UpdateModeProperty, TimelineAssetInspector.Styles.UpdateMode, new GUILayoutOption[]
			{
				GUILayout.MinWidth(140f)
			});
			TimelineAsset.SequenceUpdateMode enumValueIndex = this.m_UpdateModeProperty.enumValueIndex;
			TimelineAsset.DurationMode enumValueIndex2 = this.m_DurationModeProperty.enumValueIndex;
			EditorGUI.indentLevel++;
			if (enumValueIndex != null && enumValueIndex != 2)
			{
				if (enumValueIndex == 1)
				{
					EditorGUILayout.PropertyField(this.m_ParameterNameProperty, TimelineAssetInspector.Styles.ParameterName, new GUILayoutOption[]
					{
						GUILayout.MinWidth(140f)
					});
				}
			}
			else
			{
				EditorGUILayout.PropertyField(this.m_ExtrapolationModeProperty, TimelineAssetInspector.Styles.ExtrapolationMode, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				EditorGUILayout.PropertyField(this.m_FrameRateProperty, TimelineAssetInspector.Styles.FrameRate, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				EditorGUILayout.PropertyField(this.m_DurationModeProperty, TimelineAssetInspector.Styles.DurationMode, new GUILayoutOption[]
				{
					GUILayout.MinWidth(140f)
				});
				if (enumValueIndex2 == 1)
				{
					TimelineInspectorUtility.TimeField(this.m_FixedDurationProperty, TimelineAssetInspector.Styles.FixedDuration, false, (!(TimelineWindow.instance != null)) ? null : TimelineWindow.instance.state, double.Epsilon, TimelineClip.kMaxTimeValue * 2.0);
				}
			}
			EditorGUI.indentLevel--;
			base.serializedObject.ApplyModifiedProperties();
		}

		// Token: 0x040001D2 RID: 466
		private SerializedProperty m_UpdateModeProperty;

		// Token: 0x040001D3 RID: 467
		private SerializedProperty m_ExtrapolationModeProperty;

		// Token: 0x040001D4 RID: 468
		private SerializedProperty m_FrameRateProperty;

		// Token: 0x040001D5 RID: 469
		private SerializedProperty m_DurationModeProperty;

		// Token: 0x040001D6 RID: 470
		private SerializedProperty m_FixedDurationProperty;

		// Token: 0x040001D7 RID: 471
		private SerializedProperty m_ParameterNameProperty;

		// Token: 0x02000044 RID: 68
		private static class Styles
		{
			// Token: 0x040001D8 RID: 472
			public static readonly GUIContent UpdateMode = EditorGUIUtility.TextContent("Update Mode|Specified how this Timeline will be updated by the Director component");

			// Token: 0x040001D9 RID: 473
			public static readonly GUIContent ExtrapolationMode = EditorGUIUtility.TextContent("Extrapolation|Specified what happens when evaluating this sequence past it's duration");

			// Token: 0x040001DA RID: 474
			public static readonly GUIContent FrameRate = EditorGUIUtility.TextContent("Frame Rate|The frame rate at which this sequence updates");

			// Token: 0x040001DB RID: 475
			public static readonly GUIContent DurationMode = EditorGUIUtility.TextContent("Duration Mode|Specified how the duration of the sequence is calculated");

			// Token: 0x040001DC RID: 476
			public static readonly GUIContent FixedDuration = EditorGUIUtility.TextContent("Duration|The length of the sequence");

			// Token: 0x040001DD RID: 477
			public static readonly GUIContent ParameterName = EditorGUIUtility.TextContent("Parameter Name|Name of the parameter driving this sequence");
		}
	}
}
