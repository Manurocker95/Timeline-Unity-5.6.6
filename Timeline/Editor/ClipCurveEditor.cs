using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x0200002C RID: 44
	internal class ClipCurveEditor
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x0000FF98 File Offset: 0x0000E398
		public ClipCurveEditor(CurveDataSource dataSource, TimelineWindow parentWindow)
		{
			this.m_DataSource = dataSource;
			this.m_CurveEditor = new CurveEditor(new Rect(0f, 0f, 1000f, 100f), new CurveWrapper[0], false);
			ClipCurveEditor.s_CurveEditorSettings.hSlider = false;
			ClipCurveEditor.s_CurveEditorSettings.vSlider = false;
			ClipCurveEditor.s_CurveEditorSettings.hRangeLocked = false;
			ClipCurveEditor.s_CurveEditorSettings.vRangeLocked = false;
			ClipCurveEditor.s_CurveEditorSettings.scaleWithWindow = true;
			ClipCurveEditor.s_CurveEditorSettings.hRangeMin = 0f;
			ClipCurveEditor.s_CurveEditorSettings.showAxisLabels = true;
			ClipCurveEditor.s_CurveEditorSettings.allowDeleteLastKeyInCurve = true;
			ClipCurveEditor.s_CurveEditorSettings.rectangleToolFlags = 0;
			ClipCurveEditor.s_CurveEditorSettings.vTickStyle = new TickStyle
			{
				tickColor = 
				{
					color = DirectorStyles.Instance.customSkin.colorInlineCurveVerticalLines
				},
				distLabel = 20,
				stubs = true
			};
			ClipCurveEditor.s_CurveEditorSettings.hTickStyle = new TickStyle
			{
				tickColor = 
				{
					color = new Color(0f, 0f, 0f, 0f)
				},
				distLabel = 0
			};
			this.m_CurveEditor.settings = ClipCurveEditor.s_CurveEditorSettings;
			this.m_CurveEditor.shownArea = new Rect(1f, 1f, 1f, 1f);
			this.m_CurveEditor.ignoreScrollWheelUntilClicked = true;
			this.m_CurveEditor.curvesUpdated = new CurveEditor.CallbackFunction(this.OnCurvesUpdated);
			this.m_BindingHierarchy = new BindingSelector(parentWindow, this.m_CurveEditor);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0001014F File Offset: 0x0000E54F
		public void SelectAllKeys()
		{
			this.m_CurveEditor.SelectAll();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0001015D File Offset: 0x0000E55D
		public void FrameClip()
		{
			this.m_CurveEditor.InvalidateBounds();
			this.m_CurveEditor.FrameClip(false, true);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00010178 File Offset: 0x0000E578
		public bool HasSelection()
		{
			return this.m_CurveEditor.hasSelection;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00010198 File Offset: 0x0000E598
		public Vector2 GetSelectionRange()
		{
			Bounds selectionBounds = this.m_CurveEditor.selectionBounds;
			return new Vector2(selectionBounds.min.x, selectionBounds.max.x);
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000101DC File Offset: 0x0000E5DC
		public CurveDataSource dataSource
		{
			get
			{
				return this.m_DataSource;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000101F8 File Offset: 0x0000E5F8
		private void OnCurvesUpdated()
		{
			if (this.m_DataSource != null)
			{
				if (this.m_CurveEditor != null)
				{
					if (this.m_CurveEditor.animationCurves.Length != 0)
					{
						List<CurveWrapper> list = (from c in this.m_CurveEditor.animationCurves
						where c.changed
						select c).ToList<CurveWrapper>();
						if (list.Count != 0)
						{
							AnimationClip animationClip = this.m_DataSource.animationClip;
							Undo.RegisterCompleteObjectUndo(animationClip, "Edit Clip Curve");
							foreach (CurveWrapper curveWrapper in list)
							{
								AnimationUtility.SetEditorCurve(animationClip, curveWrapper.binding, curveWrapper.curve);
								curveWrapper.changed = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000102F8 File Offset: 0x0000E6F8
		public void DrawHeader(Rect headerRect)
		{
			this.m_BindingHierarchy.InitIfNeeded(headerRect, this.m_DataSource);
			GUILayout.BeginArea(headerRect);
			this.m_ScrollPosition = GUILayout.BeginScrollView(this.m_ScrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar, new GUILayoutOption[0]);
			this.m_BindingHierarchy.OnGUI(new Rect(0f, 0f, headerRect.width, headerRect.height));
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00010378 File Offset: 0x0000E778
		private void UpdateCurveEditorIfNeeded(TimelineWindow.TimelineState state)
		{
			if (Event.current.type == 7 && this.m_DataSource != null && this.m_BindingHierarchy != null && !(this.m_DataSource.animationClip == null))
			{
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(this.m_DataSource.animationClip);
				int version = curveInfo.version;
				if (version != this.m_LastClipVersion)
				{
					if (this.m_LastCurveCount != curveInfo.curves.Length)
					{
						this.m_BindingHierarchy.RefreshTree();
						this.m_LastCurveCount = curveInfo.curves.Length;
					}
					else
					{
						this.m_BindingHierarchy.RefreshCurves();
					}
					if (this.m_LastClipVersion == -1)
					{
						this.FrameClip();
					}
					this.m_LastClipVersion = version;
				}
				if (state.timeInFrames)
				{
					this.m_CurveEditor.state = new ClipCurveEditor.FrameFormatCurveEditorState();
				}
				else
				{
					this.m_CurveEditor.state = new ClipCurveEditor.UnformattedCurveEditorState();
				}
				this.m_CurveEditor.invSnap = state.frameRate;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0001048C File Offset: 0x0000E88C
		public void DrawCurveEditor(Rect animEditorRect, TimelineWindow.TimelineState state, Vector2 activeRange, bool loop, bool selected)
		{
			this.UpdateCurveEditorIfNeeded(state);
			ZoomableArea curveEditor = this.m_CurveEditor;
			float num = this.CalculateTopMargin(animEditorRect.height);
			this.m_CurveEditor.bottommargin = num;
			curveEditor.topmargin = num;
			float num2 = state.TimeToPixel((double)this.m_DataSource.start) - animEditorRect.xMin;
			this.m_CurveEditor.rightmargin = 0f;
			this.m_CurveEditor.leftmargin = num2;
			this.m_CurveEditor.rect = new Rect(0f, 0f, animEditorRect.width, animEditorRect.height);
			this.m_CurveEditor.SetShownHRangeInsideMargins(0f, (state.PixelToTime(animEditorRect.xMax) - this.m_DataSource.start) * this.m_DataSource.timeScale);
			if (this.m_LastFrameRate != state.frameRate)
			{
				this.m_CurveEditor.hTicks.SetTickModulosForFrameRate(state.frameRate);
				this.m_LastFrameRate = state.frameRate;
			}
			foreach (CurveWrapper curveWrapper in this.m_CurveEditor.animationCurves)
			{
				curveWrapper.renderer.SetWrap(0, (!loop) ? 0 : 2);
			}
			this.m_CurveEditor.BeginViewGUI();
			Color color = GUI.color;
			GUI.color = Color.white;
			GUI.BeginGroup(animEditorRect);
			Graphics.DrawLine(new Vector2(num2, 0f), new Vector2(num2, animEditorRect.height), new Color(1f, 1f, 1f, 0.5f));
			float num3 = activeRange.x - animEditorRect.x;
			float num4 = activeRange.y - activeRange.x;
			if (selected)
			{
				Rect rect;
				rect..ctor(num3, 0f, num4, animEditorRect.height);
				ClipCurveEditor.DrawOutline(rect, 2f);
			}
			EditorGUI.BeginChangeCheck();
			Event current = Event.current;
			if (current.type == 8 || current.type == 7 || selected)
			{
				this.m_CurveEditor.CurveGUI();
			}
			this.m_CurveEditor.EndViewGUI();
			if (EditorGUI.EndChangeCheck())
			{
				this.OnCurvesUpdated();
			}
			Color colorInlineCurveOutOfRangeOverlay = DirectorStyles.Instance.customSkin.colorInlineCurveOutOfRangeOverlay;
			Rect rect2;
			rect2..ctor(num2, 0f, num3 - num2, animEditorRect.height);
			EditorGUI.DrawRect(rect2, colorInlineCurveOutOfRangeOverlay);
			Rect rect3;
			rect3..ctor(num3 + num4, 0f, animEditorRect.width - num3 - num4, animEditorRect.height);
			EditorGUI.DrawRect(rect3, colorInlineCurveOutOfRangeOverlay);
			GUI.color = color;
			GUI.EndGroup();
			Rect rect4 = animEditorRect;
			rect4.width = ClipCurveEditor.s_GridLabelWidth;
			float num5 = num2 - ClipCurveEditor.s_GridLabelWidth;
			if (num5 > 0f)
			{
				rect4.x = animEditorRect.x + num5;
			}
			GUI.BeginGroup(rect4);
			this.m_CurveEditor.GridGUI();
			GUI.EndGroup();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00010790 File Offset: 0x0000EB90
		private float CalculateTopMargin(float height)
		{
			return Mathf.Clamp(0.15f * height, 10f, 40f);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000107BC File Offset: 0x0000EBBC
		private static void DrawOutline(Rect rect, float tickness = 2f)
		{
			EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, tickness), Color.white);
			EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - tickness, rect.width, tickness), Color.white);
			EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, tickness, rect.height), Color.white);
			EditorGUI.DrawRect(new Rect(rect.xMax - tickness, rect.yMin, tickness, rect.height), Color.white);
		}

		// Token: 0x0400017F RID: 383
		private readonly CurveEditor m_CurveEditor = null;

		// Token: 0x04000180 RID: 384
		private static readonly CurveEditorSettings s_CurveEditorSettings = new CurveEditorSettings();

		// Token: 0x04000181 RID: 385
		private static readonly float s_GridLabelWidth = 40f;

		// Token: 0x04000182 RID: 386
		private readonly BindingSelector m_BindingHierarchy;

		// Token: 0x04000183 RID: 387
		private Vector2 m_ScrollPosition = Vector2.zero;

		// Token: 0x04000184 RID: 388
		private readonly CurveDataSource m_DataSource;

		// Token: 0x04000185 RID: 389
		private float m_LastFrameRate = 30f;

		// Token: 0x04000186 RID: 390
		private int m_LastClipVersion = -1;

		// Token: 0x04000187 RID: 391
		private int m_LastCurveCount = -1;

		// Token: 0x0200002D RID: 45
		private class FrameFormatCurveEditorState : ICurveEditorState
		{
			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001C1 RID: 449 RVA: 0x0001089C File Offset: 0x0000EC9C
			public TimeArea.TimeFormat timeFormat
			{
				get
				{
					return 2;
				}
			}
		}

		// Token: 0x0200002E RID: 46
		private class UnformattedCurveEditorState : ICurveEditorState
		{
			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001C3 RID: 451 RVA: 0x000108BC File Offset: 0x0000ECBC
			public TimeArea.TimeFormat timeFormat
			{
				get
				{
					return 0;
				}
			}
		}
	}
}
