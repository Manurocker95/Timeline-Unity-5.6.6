using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000045 RID: 69
	internal static class TimelineInspectorUtility
	{
		// Token: 0x06000254 RID: 596 RVA: 0x00014F50 File Offset: 0x00013350
		public static void TimeField(SerializedProperty property, GUIContent label, bool readOnly, TimelineWindow.TimelineState state, double minValue, double maxValue)
		{
			Rect rect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			GUIContent guicontent = EditorGUI.BeginProperty(rect, label, property);
			rect = EditorGUI.PrefixLabel(rect, guicontent);
			int indentLevel = EditorGUI.indentLevel;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = 13f;
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
			Rect rect2;
			rect2..ctor(rect.xMin, rect.yMin, rect.width / 2f - 2f, rect.height);
			Rect rect3;
			rect3..ctor(rect.xMin + rect.width / 2f, rect.yMin, rect.width / 2f, rect.height);
			if (readOnly)
			{
				EditorGUI.FloatField(rect2, TimelineInspectorUtility.Styles.SecondsPrefix, (float)property.doubleValue, EditorStyles.label);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(rect2, property, TimelineInspectorUtility.Styles.SecondsPrefix);
				if (EditorGUI.EndChangeCheck())
				{
					property.doubleValue = Math.Min(maxValue, Math.Max(minValue, property.doubleValue));
				}
			}
			if (state != null)
			{
				double num = (double)state.frameRate;
				EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
				EditorGUI.BeginChangeCheck();
				double num2 = property.doubleValue;
				int num3 = TimeUtility.ToFrames(num2, num);
				double num4 = TimeUtility.ToExactFrames(num2, num);
				bool flag = TimeUtility.OnFrameBoundary(num2, num);
				if (readOnly)
				{
					if (flag)
					{
						EditorGUI.IntField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num3, EditorStyles.label);
					}
					else
					{
						EditorGUI.DoubleField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num4, EditorStyles.label);
					}
				}
				else if (flag)
				{
					int num5 = EditorGUI.IntField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num3);
					num2 = Math.Max(0.0, TimeUtility.FromFrames(num5, num));
				}
				else
				{
					double d = EditorGUI.DoubleField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num4);
					num2 = Math.Max(0.0, TimeUtility.FromFrames((int)Math.Floor(d), num));
				}
				if (EditorGUI.EndChangeCheck())
				{
					property.doubleValue = Math.Min(maxValue, Math.Max(-maxValue, num2));
				}
			}
			EditorGUI.indentLevel = indentLevel;
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.EndProperty();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00015194 File Offset: 0x00013594
		public static double TimeField(TimelineWindow.TimelineState state, GUIContent label, double time, bool readOnly, bool showMixed, double minValue, double maxValue)
		{
			Rect rect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			rect = EditorGUI.PrefixLabel(rect, label);
			int indentLevel = EditorGUI.indentLevel;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = 13f;
			EditorGUI.showMixedValue = showMixed;
			Rect rect2;
			rect2..ctor(rect.xMin, rect.yMin, rect.width / 2f, rect.height);
			Rect rect3;
			rect3..ctor(rect.xMin + rect.width / 2f, rect.yMin, rect.width / 2f, rect.height);
			if (readOnly)
			{
				EditorGUI.FloatField(rect2, TimelineInspectorUtility.Styles.SecondsPrefix, (float)time, EditorStyles.label);
			}
			else
			{
				time = EditorGUI.DoubleField(rect2, TimelineInspectorUtility.Styles.SecondsPrefix, time);
			}
			if (state != null)
			{
				double num = (double)state.frameRate;
				EditorGUI.showMixedValue = showMixed;
				int num2 = TimeUtility.ToFrames(time, (double)state.frameRate);
				double num3 = TimeUtility.ToExactFrames(time, (double)state.frameRate);
				bool flag = TimeUtility.OnFrameBoundary(time, (double)state.frameRate);
				if (readOnly)
				{
					if (flag)
					{
						EditorGUI.IntField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num2, EditorStyles.label);
					}
					else
					{
						EditorGUI.FloatField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, (float)num3, EditorStyles.label);
					}
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					double num5;
					if (flag)
					{
						int num4 = EditorGUI.IntField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num2);
						num5 = Math.Max(0.0, TimeUtility.FromFrames(num4, num));
					}
					else
					{
						double d = EditorGUI.DoubleField(rect3, TimelineInspectorUtility.Styles.FramesPrefix, num3);
						num5 = Math.Max(0.0, TimeUtility.FromFrames((int)Math.Floor(d), num));
					}
					if (EditorGUI.EndChangeCheck())
					{
						time = num5;
					}
				}
			}
			EditorGUI.indentLevel = indentLevel;
			EditorGUIUtility.labelWidth = labelWidth;
			return Math.Min(maxValue, Math.Max(minValue, time));
		}

		// Token: 0x02000046 RID: 70
		private static class Styles
		{
			// Token: 0x040001DE RID: 478
			public static readonly GUIContent SecondsPrefix = EditorGUIUtility.TextContent("s|Seconds");

			// Token: 0x040001DF RID: 479
			public static readonly GUIContent FramesPrefix = EditorGUIUtility.TextContent("f|Frames");
		}
	}
}
