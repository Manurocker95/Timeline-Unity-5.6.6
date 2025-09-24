using System;
using System.Linq;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x0200005A RID: 90
	internal class InlineCurveEditor : IBounds
	{
		// Token: 0x0600033B RID: 827 RVA: 0x00019B43 File Offset: 0x00017F43
		public InlineCurveEditor(TimelineTrackGUI trackGUI)
		{
			this.m_TrackGUI = trackGUI;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00019B54 File Offset: 0x00017F54
		Rect IBounds.boundingRect
		{
			get
			{
				return this.m_TrackRect;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00019B70 File Offset: 0x00017F70
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00019B8A File Offset: 0x00017F8A
		public Rect resizeRect { get; set; }

		// Token: 0x0600033F RID: 831 RVA: 0x00019B94 File Offset: 0x00017F94
		public bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			bool result;
			if (evt.type == null || evt.type == 16)
			{
				result = this.MouseOver(state);
			}
			else
			{
				IClipCurveEditorOwner clipCurveEditorOwner = this.GetClipCurveEditorOwner();
				if (clipCurveEditorOwner == null)
				{
					result = false;
				}
				else if (!clipCurveEditorOwner.inlineCurvesSelected)
				{
					result = false;
				}
				else
				{
					ClipCurveEditor clipCurveEditor = clipCurveEditorOwner.clipCurveEditor;
					if (clipCurveEditor == null)
					{
						result = false;
					}
					else if (evt.commandName == "FrameSelected")
					{
						this.FrameSelected(state, clipCurveEditor);
						result = true;
					}
					else
					{
						if (evt.type == 4 && evt.modifiers == null)
						{
							if (evt.character == 'a')
							{
								this.FrameAll(state, clipCurveEditor);
								return true;
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00019C68 File Offset: 0x00018068
		private void FrameAll(TimelineWindow.TimelineState state, ClipCurveEditor clipCurveEditor)
		{
			CurveDataSource dataSource = clipCurveEditor.dataSource;
			float start = dataSource.start;
			float duration = dataSource.animationClip.length / dataSource.timeScale;
			InlineCurveEditor.Frame(state, start, duration, InlineCurveEditor.s_FrameAllMarginFactor);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00019CA4 File Offset: 0x000180A4
		private void FrameSelected(TimelineWindow.TimelineState state, ClipCurveEditor clipCurveEditor)
		{
			if (!clipCurveEditor.HasSelection())
			{
				this.FrameAll(state, clipCurveEditor);
			}
			else
			{
				Vector2 selectionRange = clipCurveEditor.GetSelectionRange();
				if (selectionRange.x != selectionRange.y)
				{
					CurveDataSource dataSource = clipCurveEditor.dataSource;
					float start = dataSource.start + selectionRange.x / dataSource.timeScale;
					float duration = (selectionRange.y - selectionRange.x) / dataSource.timeScale;
					InlineCurveEditor.Frame(state, start, duration, InlineCurveEditor.s_FrameSelectedMarginFactor);
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00019D2C File Offset: 0x0001812C
		private static void Frame(TimelineWindow.TimelineState state, float start, float duration, float marginFactor)
		{
			float num = duration * marginFactor;
			state.SetTimeAreaShownRange(Mathf.Max(start - num, -10f), start + duration + num);
			state.Evaluate();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00019D5C File Offset: 0x0001815C
		private bool MouseOver(TimelineWindow.TimelineState state)
		{
			bool result;
			if (InlineCurveEditor.MouseOverHeaderArea(this.m_HeaderRect, this.m_TrackRect))
			{
				result = true;
			}
			else
			{
				ClipCurveEditor clipCurveEditor = this.GetClipCurveEditorOwner().clipCurveEditor;
				if (clipCurveEditor == null)
				{
					result = false;
				}
				else
				{
					Rect backgroundRect = clipCurveEditor.dataSource.GetBackgroundRect(state);
					result = InlineCurveEditor.MouseOverTrackArea(backgroundRect, this.m_TrackRect);
				}
			}
			return result;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00019DD0 File Offset: 0x000181D0
		private IClipCurveEditorOwner GetClipCurveEditorOwner()
		{
			return (this.m_LastSelectedClipGUI == null) ? this.m_TrackGUI : this.m_LastSelectedClipGUI;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00019E04 File Offset: 0x00018204
		private static bool MouseOverTrackArea(Rect curveRect, Rect trackRect)
		{
			curveRect.y = trackRect.y;
			curveRect.height = trackRect.height;
			curveRect.xMin = Mathf.Max(curveRect.xMin, trackRect.xMin);
			curveRect.xMax = trackRect.xMax;
			return curveRect.Contains(Event.current.mousePosition);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00019E70 File Offset: 0x00018270
		private static bool MouseOverHeaderArea(Rect headerRect, Rect trackRect)
		{
			headerRect.y = trackRect.y;
			headerRect.height = trackRect.height;
			return headerRect.Contains(Event.current.mousePosition);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00019EB4 File Offset: 0x000182B4
		private static void DrawCurveEditor(IClipCurveEditorOwner clipCurveEditorOwner, TimelineWindow.TimelineState state, Rect headerRect, Rect trackRect, Vector2 activeRange, bool locked)
		{
			ClipCurveEditor clipCurveEditor = clipCurveEditorOwner.clipCurveEditor;
			CurveDataSource dataSource = clipCurveEditor.dataSource;
			Rect backgroundRect = dataSource.GetBackgroundRect(state);
			bool flag = false;
			if (Event.current.type == null)
			{
				flag = (InlineCurveEditor.MouseOverTrackArea(backgroundRect, trackRect) || InlineCurveEditor.MouseOverHeaderArea(headerRect, trackRect));
			}
			clipCurveEditorOwner.clipCurveEditor.DrawHeader(headerRect);
			bool selected = !locked && (clipCurveEditorOwner.inlineCurvesSelected || flag);
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(locked);
			try
			{
				using (new GUIViewportScope(trackRect))
				{
					Rect animEditorRect = backgroundRect;
					animEditorRect.y = trackRect.y;
					animEditorRect.height = trackRect.height;
					animEditorRect.xMin = Mathf.Max(animEditorRect.xMin, trackRect.xMin);
					animEditorRect.xMax = trackRect.xMax;
					if (activeRange == Vector2.zero)
					{
						activeRange..ctor(animEditorRect.xMin, animEditorRect.xMax);
					}
					clipCurveEditor.DrawCurveEditor(animEditorRect, state, activeRange, clipCurveEditorOwner.supportsLooping, selected);
				}
			}
			finally
			{
				disabledScope.Dispose();
			}
			if (flag)
			{
				clipCurveEditorOwner.inlineCurvesSelected = true;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001A010 File Offset: 0x00018410
		public void Draw(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state, float identWidth)
		{
			this.m_TrackRect = trackRect;
			this.m_TrackRect.height = this.m_TrackRect.height - 5f;
			if (Event.current.type == 7)
			{
				state.quadTree.Insert(this);
			}
			headerRect.x += identWidth;
			headerRect.width -= identWidth;
			headerRect.x -= TimelineWindowStyles.kBaseIndent;
			headerRect.width += TimelineWindowStyles.kBaseIndent;
			headerRect.x += 4f;
			headerRect.width -= 4f;
			this.m_HeaderRect = headerRect;
			EditorGUI.DrawRect(this.m_HeaderRect, DirectorStyles.Instance.customSkin.colorAnimEditorBinding);
			AnimationTrack animationTrack = this.m_TrackGUI.track as AnimationTrack;
			if (animationTrack != null && !animationTrack.inClipMode)
			{
				this.DrawCurveEditorForInfiniteClip(this.m_HeaderRect, this.m_TrackRect, state);
			}
			else
			{
				this.DrawCurveEditorsForClipsOnTrack(this.m_HeaderRect, this.m_TrackRect, state);
			}
			if (Event.current.type == 7)
			{
				GUIStyle guistyle = new GUIStyle("RL DragHandle");
				guistyle.Draw(this.resizeRect, GUIContent.none, false, false, false, false);
			}
			this.m_TrackGUI.DrawLockState(trackRect, state);
			Rect rect;
			rect..ctor(headerRect.xMax + 4f, headerRect.yMax - 5f, trackRect.width - 4f, 5f);
			Color color = Handles.color;
			Handles.color = Color.black;
			Handles.DrawAAPolyLine(1f, new Vector3[]
			{
				new Vector3(rect.x, rect.yMax, 0f),
				new Vector3(rect.xMax, rect.yMax, 0f)
			});
			Handles.color = color;
			EditorGUIUtility.AddCursorRect(rect, 18);
			this.resizeRect = rect;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001A232 File Offset: 0x00018632
		private void DrawCurveEditorForInfiniteClip(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state)
		{
			if (this.m_TrackGUI.clipCurveEditor != null)
			{
				InlineCurveEditor.DrawCurveEditor(this.m_TrackGUI, state, headerRect, trackRect, Vector2.zero, this.m_TrackGUI.locked);
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001A268 File Offset: 0x00018668
		private void DrawCurveEditorsForClipsOnTrack(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state)
		{
			if (this.m_TrackGUI.clips.Count != 0)
			{
				if (Event.current.type == 8)
				{
					TimelineClipGUI timelineClipGUI = this.m_TrackGUI.clips.FirstOrDefault((TimelineClipGUI c) => c.clip.selected);
					if (timelineClipGUI != null && timelineClipGUI != this.m_LastSelectedClipGUI)
					{
						this.m_LastSelectedClipGUI = timelineClipGUI;
					}
					if (this.m_LastSelectedClipGUI == null)
					{
						this.m_LastSelectedClipGUI = this.m_TrackGUI.clips[0];
					}
				}
				if (this.m_LastSelectedClipGUI != null && this.m_LastSelectedClipGUI.clipCurveEditor != null)
				{
					Rect rect = this.m_LastSelectedClipGUI.rect;
					InlineCurveEditor.DrawCurveEditor(this.m_LastSelectedClipGUI, state, headerRect, trackRect, new Vector2(rect.xMin, rect.xMax), this.m_TrackGUI.locked);
				}
			}
		}

		// Token: 0x04000240 RID: 576
		private Rect m_TrackRect;

		// Token: 0x04000241 RID: 577
		private Rect m_HeaderRect;

		// Token: 0x04000242 RID: 578
		private readonly TimelineTrackGUI m_TrackGUI;

		// Token: 0x04000243 RID: 579
		private TimelineClipGUI m_LastSelectedClipGUI;

		// Token: 0x04000244 RID: 580
		private static readonly float s_FrameAllMarginFactor = 0.1f;

		// Token: 0x04000245 RID: 581
		private static readonly float s_FrameSelectedMarginFactor = 0.2f;
	}
}
