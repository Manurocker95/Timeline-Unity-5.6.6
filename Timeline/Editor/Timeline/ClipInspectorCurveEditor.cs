using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000007 RID: 7
	internal class ClipInspectorCurveEditor
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00003DB0 File Offset: 0x000021B0
		public ClipInspectorCurveEditor()
		{
			this.m_CurveEditorSettings.allowDeleteLastKeyInCurve = false;
			this.m_CurveEditorSettings.allowDraggingCurvesAndRegions = true;
			this.m_CurveEditorSettings.hTickLabelOffset = 0.1f;
			this.m_CurveEditorSettings.showAxisLabels = true;
			this.m_CurveEditorSettings.useFocusColors = false;
			this.m_CurveEditorSettings.wrapColor = new EditorGUIUtility.SkinnedColor(Color.black);
			this.m_CurveEditorSettings.hSlider = false;
			this.m_CurveEditorSettings.hRangeMin = 0f;
			this.m_CurveEditorSettings.vRangeMin = 0f;
			this.m_CurveEditorSettings.vRangeMax = 1f;
			this.m_CurveEditorSettings.hRangeMax = 1f;
			this.m_CurveEditorSettings.vSlider = false;
			this.m_CurveEditorSettings.hRangeLocked = false;
			this.m_CurveEditorSettings.vRangeLocked = false;
			TickStyle tickStyle = new TickStyle();
			tickStyle.tickColor = new EditorGUIUtility.SkinnedColor(new Color(0f, 0f, 0f, 0.2f));
			tickStyle.distLabel = 30;
			tickStyle.stubs = false;
			tickStyle.centerLabel = true;
			this.m_CurveEditorSettings.hTickStyle = tickStyle;
			TickStyle tickStyle2 = new TickStyle();
			tickStyle2.tickColor = new EditorGUIUtility.SkinnedColor(new Color(1f, 0f, 0f, 0.2f));
			tickStyle2.distLabel = 20;
			tickStyle2.stubs = false;
			tickStyle2.centerLabel = true;
			this.m_CurveEditorSettings.vTickStyle = tickStyle2;
			this.m_CurveEditor = new CurveEditor(new Rect(0f, 0f, 1000f, 100f), new CurveWrapper[0], true);
			this.m_CurveEditor.settings = this.m_CurveEditorSettings;
			this.m_CurveEditor.ignoreScrollWheelUntilClicked = true;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003F8C File Offset: 0x0000238C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003FA7 File Offset: 0x000023A7
		public double trackTime
		{
			get
			{
				return this.m_trackTime;
			}
			set
			{
				this.m_trackTime = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003FB4 File Offset: 0x000023B4
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00003FCE File Offset: 0x000023CE
		public string headerString { get; set; }

		// Token: 0x06000034 RID: 52 RVA: 0x00003FD8 File Offset: 0x000023D8
		internal bool InitStyles()
		{
			bool result;
			if (EditorStyles.s_Current == null)
			{
				result = false;
			}
			else
			{
				if (this.m_LabelStyle == null)
				{
					this.m_LabelStyle = new GUIStyle(EditorStyles.whiteLargeLabel);
					this.m_LegendStyle = new GUIStyle(EditorStyles.miniBoldLabel);
					this.m_LabelStyle.alignment = 4;
					this.m_LegendStyle.alignment = 4;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004044 File Offset: 0x00002444
		internal void OnGUI(Rect clientRect, CurvePresetLibrary presets)
		{
			if (this.InitStyles())
			{
				if (this.m_Curves != null && this.m_Curves.Length != 0)
				{
					Rect rect;
					rect..ctor(clientRect.x, clientRect.y, clientRect.width, ClipInspectorCurveEditor.k_HeaderHeight);
					Rect rect2;
					rect2..ctor(clientRect.x, clientRect.y + rect.height, clientRect.width, clientRect.height - ClipInspectorCurveEditor.k_HeaderHeight - ClipInspectorCurveEditor.k_PresetHeight);
					Rect rect3;
					rect3..ctor(clientRect.x + 30f, clientRect.y + rect2.height + ClipInspectorCurveEditor.k_HeaderHeight, clientRect.width - 30f, ClipInspectorCurveEditor.k_PresetHeight);
					GUI.Box(rect, this.headerString, this.m_LabelStyle);
					this.m_CurveEditor.rect = rect2;
					this.m_CurveEditor.shownAreaInsideMargins = new Rect(0f, 0f, 1f, 1f);
					this.m_CurveEditor.animationCurves = this.m_CurveWrappers;
					this.UpdateSelectionColors();
					EditorGUI.BeginChangeCheck();
					this.m_CurveEditor.OnGUI();
					bool flag = EditorGUI.EndChangeCheck();
					this.DrawTrackHead(rect2);
					this.DrawPresets(rect3, presets);
					if (presets == null)
					{
						this.DrawLegend(rect3);
					}
					if (flag)
					{
						this.ProcessUpdates();
					}
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000041B0 File Offset: 0x000025B0
		private void DrawPresets(Rect position, CurvePresetLibrary curveLibrary)
		{
			if (!(curveLibrary == null) && curveLibrary.Count() != 0)
			{
				int num = (!(curveLibrary != null)) ? 0 : curveLibrary.Count();
				int num2 = Mathf.Min(num, 9);
				float num3 = (float)num2 * 30f + (float)(num2 - 1) * 10f;
				float num4 = (position.width - num3) * 0.5f;
				float num5 = (position.height - 15f) * 0.5f;
				float num6 = 3f;
				if (num4 > 0f)
				{
					num6 = num4;
				}
				GUI.BeginGroup(position);
				Color color;
				Color.white.a = color.a * 0.6f;
				for (int i = 0; i < num2; i++)
				{
					if (i > 0)
					{
						num6 += 10f;
					}
					Rect rect;
					rect..ctor(num6, num5, 30f, 15f);
					this.m_TextContent.tooltip = curveLibrary.GetName(i);
					if (GUI.Button(rect, this.m_TextContent, GUIStyle.none))
					{
						IEnumerable<CurveWrapper> enumerable = this.m_CurveWrappers;
						if (this.m_CurveWrappers.Length > 1)
						{
							enumerable = from x in this.m_CurveWrappers
							where x.selected == 1
							select x;
						}
						foreach (CurveWrapper curveWrapper in enumerable)
						{
							AnimationCurve animationCurve = curveLibrary.GetPreset(i) as AnimationCurve;
							curveWrapper.curve.keys = (Keyframe[])animationCurve.keys.Clone();
							curveWrapper.changed = true;
						}
					}
					if (Event.current.type == 7)
					{
						curveLibrary.Draw(rect, i);
					}
					num6 += 30f;
				}
				GUI.EndGroup();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000043BC File Offset: 0x000027BC
		private void DrawTrackHead(Rect clientRect)
		{
			if (TimelineWindow.styles != null)
			{
				if (!double.IsNaN(this.m_trackTime))
				{
					float num = this.m_CurveEditor.TimeToPixel((float)this.m_trackTime, clientRect);
					num = Mathf.Clamp(num, clientRect.xMin, clientRect.xMax);
					Vector2 vector;
					vector..ctor(num, clientRect.yMin);
					Vector2 vector2;
					vector2..ctor(num, clientRect.yMax);
					Graphics.DrawLine(vector, vector2, DirectorStyles.Instance.customSkin.colorPlayhead);
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004454 File Offset: 0x00002854
		private void DrawLegend(Rect r)
		{
			if (this.m_CurveWrappers != null && this.m_CurveWrappers.Length != 0)
			{
				Color color = GUI.color;
				float num = r.width / (float)this.m_CurveWrappers.Length;
				for (int i = 0; i < this.m_CurveWrappers.Length; i++)
				{
					CurveWrapper curveWrapper = this.m_CurveWrappers[i];
					if (curveWrapper != null)
					{
						Rect rect;
						rect..ctor(r.x + (float)i * num, r.y, num, r.height);
						Color color2 = curveWrapper.color;
						color2.a = 1f;
						GUI.color = color2;
						string text = this.LabelName(curveWrapper.binding.propertyName);
						EditorGUI.LabelField(rect, text, this.m_LegendStyle);
					}
				}
				GUI.color = color;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004528 File Offset: 0x00002928
		private string LabelName(string propertyName)
		{
			propertyName = AnimationWindowUtility.GetPropertyDisplayName(propertyName);
			int num = propertyName.LastIndexOfAny(ClipInspectorCurveEditor.s_kLabelMarkers);
			if (num >= 0)
			{
				propertyName = propertyName.Substring(num);
			}
			return propertyName;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004564 File Offset: 0x00002964
		public void SetCurves(AnimationCurve[] curves, EditorCurveBinding[] bindings)
		{
			this.m_Curves = curves;
			if (this.m_Curves != null && this.m_Curves.Length > 0)
			{
				this.m_CurveWrappers = new CurveWrapper[this.m_Curves.Length];
				for (int i = 0; i < this.m_Curves.Length; i++)
				{
					CurveWrapper curveWrapper = new CurveWrapper();
					curveWrapper.renderer = new NormalCurveRenderer(this.m_Curves[i]);
					curveWrapper.renderer.SetWrap(1, 1);
					curveWrapper.readOnly = false;
					curveWrapper.color = EditorGUI.kCurveColor;
					curveWrapper.renderer.SetCustomRange(0f, 1f);
					curveWrapper.id = curves[i].GetHashCode();
					if (bindings != null)
					{
						curveWrapper.binding = bindings[i];
						curveWrapper.color = CurveUtility.GetPropertyColor(bindings[i].propertyName);
						curveWrapper.id = bindings[i].GetHashCode();
					}
					curveWrapper.hidden = false;
					curveWrapper.regionId = -1;
					this.m_CurveWrappers[i] = curveWrapper;
				}
				this.UpdateSelectionColors();
				this.m_CurveEditor.animationCurves = this.m_CurveWrappers;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004694 File Offset: 0x00002A94
		internal void SetUpdateCurveCallback(Action<AnimationCurve, EditorCurveBinding> callback)
		{
			this.m_CurveUpdatedCallback = callback;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000046A0 File Offset: 0x00002AA0
		private void ProcessUpdates()
		{
			for (int i = 0; i < this.m_CurveWrappers.Length; i++)
			{
				CurveWrapper curveWrapper = this.m_CurveWrappers[i];
				if (curveWrapper.changed)
				{
					curveWrapper.changed = false;
					if (this.m_CurveUpdatedCallback != null)
					{
						this.m_CurveUpdatedCallback(curveWrapper.curve, curveWrapper.binding);
					}
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004708 File Offset: 0x00002B08
		public void SetSelected(AnimationCurve curve)
		{
			this.m_CurveEditor.SelectNone();
			for (int i = 0; i < this.m_Curves.Length; i++)
			{
				if (curve == this.m_Curves[i])
				{
					this.m_CurveWrappers[i].selected = 1;
					this.m_CurveEditor.AddSelection(new CurveSelection(this.m_CurveWrappers[i].id, 0));
				}
			}
			this.UpdateSelectionColors();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004780 File Offset: 0x00002B80
		private void UpdateSelectionColors()
		{
			if (this.m_CurveWrappers != null)
			{
				for (int i = 0; i < this.m_CurveWrappers.Length; i++)
				{
					CurveWrapper curveWrapper = this.m_CurveWrappers[i];
					Color color = curveWrapper.color;
					if (curveWrapper.readOnly)
					{
						color.a = 0.75f;
					}
					else if (curveWrapper.selected != null)
					{
						color.a = 1f;
					}
					else
					{
						color.a = 0.3f;
					}
					curveWrapper.color = color;
				}
			}
		}

		// Token: 0x0400003F RID: 63
		private CurveEditorSettings m_CurveEditorSettings = new CurveEditorSettings();

		// Token: 0x04000040 RID: 64
		private CurveEditor m_CurveEditor;

		// Token: 0x04000041 RID: 65
		private AnimationCurve[] m_Curves;

		// Token: 0x04000042 RID: 66
		private CurveWrapper[] m_CurveWrappers;

		// Token: 0x04000043 RID: 67
		private static readonly float k_HeaderHeight = 30f;

		// Token: 0x04000044 RID: 68
		private static readonly float k_PresetHeight = 30f;

		// Token: 0x04000045 RID: 69
		private Action<AnimationCurve, EditorCurveBinding> m_CurveUpdatedCallback;

		// Token: 0x04000046 RID: 70
		private GUIContent m_TextContent = new GUIContent();

		// Token: 0x04000047 RID: 71
		private GUIStyle m_LabelStyle;

		// Token: 0x04000048 RID: 72
		private GUIStyle m_LegendStyle;

		// Token: 0x04000049 RID: 73
		public static readonly double kDisableTrackTime = double.NaN;

		// Token: 0x0400004A RID: 74
		private double m_trackTime = ClipInspectorCurveEditor.kDisableTrackTime;

		// Token: 0x0400004C RID: 76
		private static char[] s_kLabelMarkers = new char[]
		{
			'_'
		};
	}
}
