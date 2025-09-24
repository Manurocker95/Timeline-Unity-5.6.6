using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200008C RID: 140
	internal class TimelineTrackGUI : TimelineGroupGUI, IClipCurveEditorOwner
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00025498 File Offset: 0x00023898
		public TimelineTrackGUI(TreeViewController tv, TimelineTreeViewGUI w, int id, int depth, TreeViewItem parent, string displayName, TrackAsset sequenceActor) : base(tv, w, id, depth, parent, displayName, sequenceActor, false)
		{
			AnimationTrack animationTrack = sequenceActor as AnimationTrack;
			if (animationTrack != null)
			{
				this.m_InfiniteTrackDrawer = new InfiniteTrackDrawer(new AnimationTrackKeyDataSource(animationTrack));
				this.UpdateInfiniteClipEditor(animationTrack, w.TimelineWindow);
				if (animationTrack.ShouldShowInfiniteClipEditor())
				{
					this.clipCurveEditor = new ClipCurveEditor(new InfiniteClipCurveDataSource(this), w.TimelineWindow);
				}
			}
			this.m_HeaderIcon = base.drawer.GetIcon();
			this.m_HadProblems = false;
			this.m_InitHadProblems = false;
			this.m_Bindings = base.track.outputs;
			base.AddManipulator(new TrackVerticalResize());
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00025590 File Offset: 0x00023990
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x000255AB File Offset: 0x000239AB
		public bool resortClips
		{
			get
			{
				return this.m_MustSortClips;
			}
			set
			{
				this.m_MustSortClips = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x000255B8 File Offset: 0x000239B8
		public override Rect boundingRect
		{
			get
			{
				Rect boundingRect = base.boundingRect;
				boundingRect.height += this.InlineAnimationCurveHeight();
				return boundingRect;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000255EC File Offset: 0x000239EC
		public List<TimelineClipGUI> clips
		{
			get
			{
				return this.m_ClipGUICache;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00025608 File Offset: 0x00023A08
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00025622 File Offset: 0x00023A22
		public InlineCurveEditor inlineCurveEditor { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0002562C File Offset: 0x00023A2C
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x00025646 File Offset: 0x00023A46
		public ClipCurveEditor clipCurveEditor { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0002564F File Offset: 0x00023A4F
		public override bool selected
		{
			set
			{
				if (value)
				{
					Selection.UnselectInlineCurves();
				}
				base.selected = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00025664 File Offset: 0x00023A64
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0002567F File Offset: 0x00023A7F
		public bool inlineCurvesSelected
		{
			get
			{
				return this.m_InlineCurvesSelected;
			}
			set
			{
				if (value)
				{
					Selection.UnselectAll();
					this.OnSelectedChanged(false);
				}
				this.m_InlineCurvesSelected = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x000256A0 File Offset: 0x00023AA0
		public bool supportsLooping
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000256B8 File Offset: 0x00023AB8
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x000256D2 File Offset: 0x00023AD2
		public bool curveEditorSelected { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000256DC File Offset: 0x00023ADC
		public override Rect indentedHeaderBounds
		{
			get
			{
				float num = (float)this.depth * DirectorStyles.Instance.indentWidth + 2f;
				Rect headerBounds = this.headerBounds;
				headerBounds.width = this.headerBounds.width - num;
				headerBounds.x += num;
				return headerBounds;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00025738 File Offset: 0x00023B38
		private bool trackAllowsRecording
		{
			get
			{
				if (this.m_TrackAllowsRecording == null)
				{
					this.m_TrackAllowsRecording = new bool?(this.DoesTrackAllowRecording());
				}
				return this.m_TrackAllowsRecording.Value;
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00025779 File Offset: 0x00023B79
		private void UpdateInfiniteClipEditor(AnimationTrack animationTrack, TimelineWindow window)
		{
			if (animationTrack != null && this.clipCurveEditor == null && animationTrack.ShouldShowInfiniteClipEditor())
			{
				this.clipCurveEditor = new ClipCurveEditor(new InfiniteClipCurveDataSource(this), window);
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000257B0 File Offset: 0x00023BB0
		protected override void OnSelectedChanged(bool value)
		{
			base.OnSelectedChanged(value);
			if (!(TimelineWindow.instance == null))
			{
				TimelineWindow.TimelineState state = TimelineWindow.instance.state;
				if (value && state != null && state.selection != null && base.track != null)
				{
					state.selection.SelectInEditor(base.track);
				}
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00025820 File Offset: 0x00023C20
		protected override void OnLockedChanged(bool value)
		{
			TimelineWindow.TimelineState state = TimelineWindow.instance.state;
			if (!base.locked)
			{
				foreach (TimelineClipGUI item in this.clips)
				{
					state.selection.Remove(item);
				}
			}
			if (base.locked)
			{
				state.UnarmForRecord(base.track);
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000258B8 File Offset: 0x00023CB8
		public override bool CanBeSelected(Vector2 mousePosition)
		{
			return this.headerBounds.Contains(mousePosition);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000258DC File Offset: 0x00023CDC
		public void RangeSelectClips(TimelineClipGUI lastClipToSelect, TimelineWindow.TimelineState state)
		{
			List<TimelineTrackBaseGUI> allTracks = TimelineWindow.instance.allTracks;
			bool flag = false;
			foreach (TimelineTrackBaseGUI timelineTrackBaseGUI in allTracks)
			{
				TimelineTrackGUI timelineTrackGUI = timelineTrackBaseGUI as TimelineTrackGUI;
				if (timelineTrackGUI != null)
				{
					timelineTrackGUI.SortClipsByStartTime();
					foreach (TimelineClipGUI timelineClipGUI in timelineTrackGUI.clips)
					{
						if (!flag && (timelineClipGUI == lastClipToSelect || timelineClipGUI.selected))
						{
							state.selection.Add(timelineClipGUI);
							flag = true;
						}
						else if (flag && (timelineClipGUI == lastClipToSelect || timelineClipGUI.selected))
						{
							state.selection.Add(timelineClipGUI);
							flag = false;
						}
						else if (flag)
						{
							state.selection.Add(timelineClipGUI);
						}
					}
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00025A14 File Offset: 0x00023E14
		private bool IsMuted(TimelineWindow.TimelineState state)
		{
			return !(base.track == null) && base.track.mediaType != 5 && !base.track.soloed && (state.soloTracks.Count > 0 || base.track.muted);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00025A94 File Offset: 0x00023E94
		public override void Draw(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state, float identWidth)
		{
			if (base.track.showInlineCurves && this.inlineCurveEditor == null)
			{
				this.inlineCurveEditor = new InlineCurveEditor(this);
			}
			this.UpdateInfiniteClipEditor(base.track as AnimationTrack, state.GetWindow());
			Rect trackRect2 = trackRect;
			float num = this.InlineAnimationCurveHeight();
			trackRect.height -= num;
			if (Event.current.type == 7)
			{
				this.m_TrackRect = trackRect;
				state.quadTree.Insert(this);
				int num2 = this.BlendHash();
				if (this.m_BlendHash != num2)
				{
					this.UpdateClipOverlaps(state, trackRect);
					this.m_BlendHash = num2;
				}
				base.isDropTarget = false;
			}
			if (TimelineTrackGUI.s_ArmForRecordContentOn == null)
			{
				TimelineTrackGUI.s_ArmForRecordContentOn = new GUIContent(state.styles.autoKey.active.background);
			}
			if (TimelineTrackGUI.s_ArmForRecordContentOff == null)
			{
				TimelineTrackGUI.s_ArmForRecordContentOff = new GUIContent(state.styles.autoKey.normal.background);
			}
			base.track.collapsed = !base.isExpanded;
			headerRect.width -= 2f;
			if (this.m_TrackHash != this.ComputeTrackHash())
			{
				this.RebuildGUICache(state);
			}
			bool flag = false;
			Vector2 timeAreaShownRange = state.timeAreaShownRange;
			if (base.drawer != null)
			{
				flag = base.drawer.DrawTrack(trackRect, base.track, timeAreaShownRange, state);
			}
			if (!flag)
			{
				using (new GUIViewportScope(trackRect))
				{
					this.DrawBackground(trackRect, state);
					if (this.m_MustSortClips)
					{
						int num3 = 0;
						this.SortClipsByStartTime();
						this.ResetParityID();
						foreach (TimelineClipGUI timelineClipGUI in this.m_ClipGUICache)
						{
							timelineClipGUI.parityID = this.GetNextParityID();
							timelineClipGUI.zOrder = num3++;
							if (timelineClipGUI.selected)
							{
								timelineClipGUI.zOrder += 1000;
							}
						}
						this.m_ClipGUICache = (from x in this.m_ClipGUICache
						orderby x.clip.start
						orderby x.selected
						select x).ToList<TimelineClipGUI>();
						this.m_MustSortClips = false;
					}
					this.DrawClips(trackRect, state);
					this.DrawClipConnectors(trackRect);
				}
				if (this.m_InfiniteTrackDrawer != null)
				{
					this.m_InfiniteTrackDrawer.DrawTrack(trackRect, base.track, timeAreaShownRange, state);
				}
			}
			this.DrawTrackHeader(headerRect, state, identWidth, num);
			this.DrawInlineCurves(headerRect, trackRect2, state, identWidth, num);
			this.DrawMuteState(trackRect, state);
			this.DrawLockState(trackRect, state);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00025DB0 File Offset: 0x000241B0
		public override bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			if (base.track.showInlineCurves && this.inlineCurveEditor != null)
			{
				if (this.inlineCurveEditor.OnEvent(evt, state, isCaptureSession))
				{
					return true;
				}
			}
			return base.OnEvent(evt, state, isCaptureSession);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00025E08 File Offset: 0x00024208
		private void DrawInlineCurves(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state, float identWidth, float inlineCurveHeight)
		{
			if (base.track.showInlineCurves && this.inlineCurveEditor != null && inlineCurveHeight != 0f)
			{
				float num = trackRect.height - inlineCurveHeight;
				trackRect.y += num;
				trackRect.height = inlineCurveHeight;
				headerRect.x += TimelineWindowStyles.kBaseIndent;
				headerRect.width -= TimelineWindowStyles.kBaseIndent;
				headerRect.y += num;
				headerRect.height = inlineCurveHeight;
				if (this.inlineCurveEditor != null)
				{
					this.inlineCurveEditor.Draw(headerRect, trackRect, state, identWidth);
				}
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00025EBE File Offset: 0x000242BE
		private void DrawLockMuteLabel(Rect rect)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00025EC4 File Offset: 0x000242C4
		private void DrawLockTrackBG(Rect trackRect)
		{
			int num = (int)Mathf.Ceil(trackRect.width / (float)this.m_Styles.lockedBG.normal.background.width);
			Rect rect = trackRect;
			rect.width = (float)this.m_Styles.lockedBG.normal.background.width;
			for (int num2 = 0; num2 != num; num2++)
			{
				GUI.Box(rect, GUIContent.none, this.m_Styles.lockedBG);
				rect.x += (float)this.m_Styles.lockedBG.normal.background.width - 1f;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00025F7C File Offset: 0x0002437C
		private void DrawTrackStateBox(Rect trackRect)
		{
			if (base.track.locked)
			{
				TimelineTrackGUI.s_LockMuteOverlay.text = "Locked";
				if (base.track.muted)
				{
					GUIContent guicontent = TimelineTrackGUI.s_LockMuteOverlay;
					guicontent.text += " / Muted";
				}
			}
			else if (base.track.muted)
			{
				TimelineTrackGUI.s_LockMuteOverlay.text = "Muted";
				if (base.track.locked)
				{
					TimelineTrackGUI.s_LockMuteOverlay.text = "Locked / Muted";
				}
			}
			Rect rect = trackRect;
			rect.width = this.m_Styles.fontClip.CalcSize(TimelineTrackGUI.s_LockMuteOverlay).x + 40f;
			rect.x += (trackRect.width - rect.width) / 2f;
			rect.height -= 4f;
			rect.y += 2f;
			using (new GUIColorOverride(this.m_Styles.customSkin.colorLockTextBG))
			{
				GUI.Box(rect, GUIContent.none, this.m_Styles.segmentCenter);
			}
			Graphics.ShadowLabel(rect, TimelineTrackGUI.s_LockMuteOverlay, this.m_Styles.fontClip, Color.white, Color.black);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00026104 File Offset: 0x00024504
		public void DrawLockState(Rect trackRect, TimelineWindow.TimelineState state)
		{
			if (base.track.locked)
			{
				this.DrawLockTrackBG(trackRect);
				this.DrawTrackStateBox(trackRect);
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00026128 File Offset: 0x00024528
		private void DrawErrorIcon(Rect position, TimelineWindow.TimelineState state)
		{
			Rect rect = position;
			rect.x = position.xMax + 3f;
			rect.width = state.bindingAreaWidth;
			EditorGUI.LabelField(position, this.m_ProblemIcon);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00026168 File Offset: 0x00024568
		private int ComputeTrackHash()
		{
			return base.track.clips.Length;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002618C File Offset: 0x0002458C
		private TrackBindingValidationResult GetTrackBindingValidationResult(TimelineWindow.TimelineState state)
		{
			TrackAsset track = (!base.isSubTrack()) ? base.track : base.parentTrack();
			return state.ValidateBindingForTrack(track);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000261C8 File Offset: 0x000245C8
		protected override bool DetectProblems(TimelineWindow.TimelineState state)
		{
			return !this.GetTrackBindingValidationResult(state).IsValid() && state != null && state.currentDirector != null;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00026208 File Offset: 0x00024608
		protected void DrawBackground(Rect trackRect, TimelineWindow.TimelineState state)
		{
			if (this.IsRecording(state) && this.m_InfiniteTrackDrawer != null && !this.m_InfiniteTrackDrawer.CanDraw(base.track, state))
			{
				this.DrawRecordingTrackBackground(trackRect);
			}
			else
			{
				Color color = base.drawer.GetTrackBackgroundColor(base.track);
				float a = color.a;
				if (this.selected)
				{
					float num;
					float num2;
					float num3;
					Color.RGBToHSV(color, ref num, ref num2, ref num3);
					num3 *= 1.3f;
					color = Color.HSVToRGB(num, num2, num3);
					color.a = a;
				}
				if (base.isDropTarget)
				{
					color = DirectorStyles.Instance.customSkin.colorTrackBackgroundSelected;
				}
				EditorGUI.DrawRect(trackRect, color);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000262C4 File Offset: 0x000246C4
		public float InlineAnimationCurveHeight()
		{
			float result;
			if (!base.track.showInlineCurves)
			{
				result = 0f;
			}
			else if (!TimelineUtility.TrackHasAnimationCurves(base.track))
			{
				result = 0f;
			}
			else
			{
				result = base.track.inlineAnimationCurveHeight;
			}
			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002631C File Offset: 0x0002471C
		public override float GetHeight(TimelineWindow.TimelineState state)
		{
			float num = base.drawer.GetHeight(base.track);
			if (num < 0f)
			{
				num = state.trackHeight;
			}
			num += this.InlineAnimationCurveHeight();
			return num * state.trackScale;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00026368 File Offset: 0x00024768
		private float GetExpandedHeight(TimelineWindow.TimelineState state)
		{
			return this.GetHeight(state);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00026388 File Offset: 0x00024788
		private static bool CanDrawIcon(GUIContent icon)
		{
			return icon != null && icon != GUIContent.none && icon.image != null;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000263C0 File Offset: 0x000247C0
		private bool showSceneReference
		{
			get
			{
				bool result;
				if (base.track == null || base.isSubTrack() || this.m_Bindings.Length == 0)
				{
					result = false;
				}
				else
				{
					PlayableBinding playableBinding = this.m_Bindings[0];
					result = (playableBinding.sourceObject != null && (playableBinding.streamType == null || (playableBinding.streamType == 3 && playableBinding.sourceBindingType != null && typeof(Object).IsAssignableFrom(playableBinding.sourceBindingType)) || playableBinding.streamType == 1));
				}
				return result;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0002647C File Offset: 0x0002487C
		private string GetTrackDisplayName(TrackAsset track, TimelineWindow.TimelineState state)
		{
			string result;
			if (track == null)
			{
				result = "";
			}
			else
			{
				string name = track.name;
				if (track.name.StartsWith(track.GetType().Name))
				{
					if (state.currentDirector != null)
					{
						GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(state.currentDirector, track);
						if (sceneGameObject != null)
						{
							name = sceneGameObject.name;
						}
					}
				}
				result = name;
			}
			return result;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00026504 File Offset: 0x00024904
		private void DrawTrackHeader(Rect headerRect, TimelineWindow.TimelineState state, float indentWidth, float inlineCurveHeight)
		{
			using (new GUIViewportScope(headerRect))
			{
				headerRect.x += indentWidth;
				headerRect.width -= indentWidth;
				if (Event.current.type == 7)
				{
					bool hasProblems = this.DetectProblems(state);
					this.RefreshStateIfBindingProblemIsFound(state, hasProblems);
					this.UpdateBindingProblemValues(hasProblems);
				}
				Rect rect = headerRect;
				rect.height -= inlineCurveHeight;
				this.DrawHeaderBackground(headerRect);
				rect.x += this.DrawTrackColorKind(rect, state);
				rect.x += this.DrawTrackIconKind(rect, state);
				this.DrawTrackBinding(rect, headerRect, state);
				if (base.track.mediaType != 5)
				{
					Rect rect2;
					rect2..ctor(headerRect.xMax - 16f - 3f, rect.y + (rect.height - 16f) / 2f, 16f, 16f);
					rect2.x -= this.DrawTrackDropDownMenu(rect2, state);
					rect2.x -= this.DrawInlineCurveButton(rect2, state);
					rect2.x -= this.DrawMuteButton(rect2, state);
					rect2.x -= this.DrawLockButton(rect2, state);
					rect2.x -= this.DrawRecordButton(rect2, state);
				}
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000266A4 File Offset: 0x00024AA4
		private void RefreshStateIfBindingProblemIsFound(TimelineWindow.TimelineState state, bool hasProblems)
		{
			if (this.m_InitHadProblems && this.m_HadProblems != hasProblems)
			{
				TrackBindingValidationResult trackBindingValidationResult = this.GetTrackBindingValidationResult(state);
				bool flag = !trackBindingValidationResult.IsValid() && trackBindingValidationResult.bindingState != TimelineTrackBindingState.BoundGameObjectIsDisabled && trackBindingValidationResult.bindingState != TimelineTrackBindingState.RequiredComponentOnBoundGameObjectIsDisabled;
				if (flag)
				{
					state.Refresh();
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00026709 File Offset: 0x00024B09
		private void UpdateBindingProblemValues(bool hasProblems)
		{
			this.m_HadProblems = hasProblems;
			this.m_InitHadProblems = true;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0002671C File Offset: 0x00024B1C
		private void DrawHeaderBackground(Rect headerRect)
		{
			Color color = (!this.selected) ? DirectorStyles.Instance.customSkin.colorTrackHeaderBackground : DirectorStyles.Instance.customSkin.colorSelection;
			Rect rect = headerRect;
			rect.x += this.m_Styles.trackSwatchStyle.fixedWidth;
			rect.width -= this.m_Styles.trackSwatchStyle.fixedWidth;
			EditorGUI.DrawRect(rect, color);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000267A0 File Offset: 0x00024BA0
		private float DrawTrackColorKind(Rect rect, TimelineWindow.TimelineState state)
		{
			float fixedWidth;
			using (new GUIColorOverride(base.drawer.trackColor))
			{
				rect.height = this.GetExpandedHeight(state);
				rect.width = this.m_Styles.trackSwatchStyle.fixedWidth;
				GUI.Box(rect, GUIContent.none, this.m_Styles.trackSwatchStyle);
				fixedWidth = this.m_Styles.trackSwatchStyle.fixedWidth;
			}
			return fixedWidth;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00026834 File Offset: 0x00024C34
		private float DrawTrackIconKind(Rect rect, TimelineWindow.TimelineState state)
		{
			rect.yMin += (rect.height - 16f) / 2f;
			rect.width = 16f;
			rect.height = 16f;
			if (this.m_HadProblems)
			{
				this.GenerateIconForBindingValidationResult(this.m_Styles, this.GetTrackBindingValidationResult(state));
				if (TimelineTrackGUI.CanDrawIcon(this.m_ProblemIcon))
				{
					this.DrawErrorIcon(rect, state);
				}
			}
			else if (TimelineTrackGUI.CanDrawIcon(this.m_HeaderIcon))
			{
				GUI.Box(rect, this.m_HeaderIcon, GUIStyle.none);
			}
			return rect.width;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000268EC File Offset: 0x00024CEC
		private void DrawMuteState(Rect trackRect, TimelineWindow.TimelineState state)
		{
			if (this.IsMuted(state))
			{
				Rect rect = trackRect;
				rect.x += this.m_Styles.trackSwatchStyle.fixedWidth;
				rect.width -= this.m_Styles.trackSwatchStyle.fixedWidth;
				EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorTrackDarken);
				this.DrawTrackStateBox(trackRect);
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00026964 File Offset: 0x00024D64
		private void DrawTrackBinding(Rect rect, Rect headerRect, TimelineWindow.TimelineState state)
		{
			if (this.showSceneReference)
			{
				if (state.currentDirector != null)
				{
					this.DoTrackBindingGUI(rect, headerRect, state);
					return;
				}
			}
			GUIStyle sequenceTrackHeaderFont = this.m_Styles.sequenceTrackHeaderFont;
			sequenceTrackHeaderFont.normal.textColor = ((!this.selected) ? this.m_Styles.customSkin.colorTrackFont : Color.white);
			bool flag = false;
			string text = base.drawer.GetCustomTitle(base.track);
			if (string.IsNullOrEmpty(text))
			{
				flag = true;
				text = this.GetTrackDisplayName(base.track, state);
			}
			rect.width = this.m_Styles.sequenceTrackHeaderFont.CalcSize(new GUIContent(text)).x;
			if (flag)
			{
				if (GUIUtility.keyboardControl == base.track.GetInstanceID())
				{
					Rect rect2 = rect;
					rect2.width = headerRect.xMax - rect.xMin - 80f;
					base.track.name = EditorGUI.DelayedTextField(rect2, GUIContent.none, base.track.GetInstanceID(), base.track.name, sequenceTrackHeaderFont);
				}
				else
				{
					EditorGUI.DelayedTextField(rect, GUIContent.none, base.track.GetInstanceID(), text, sequenceTrackHeaderFont);
				}
			}
			else
			{
				EditorGUI.LabelField(rect, text, sequenceTrackHeaderFont);
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00026ACC File Offset: 0x00024ECC
		protected float DrawTrackDropDownMenu(Rect rect, TimelineWindow.TimelineState state)
		{
			rect.y += 2f;
			if (GUI.Button(rect, GUIContent.none, this.m_Styles.trackOptions))
			{
				state.selection.Clear();
				state.selection.Add(this);
				base.DisplayTrackMenu(state);
			}
			return 16f;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00026B34 File Offset: 0x00024F34
		private float DrawMuteButton(Rect rect, TimelineWindow.TimelineState state)
		{
			float result;
			if (!base.isSubTrack() && base.track.muted)
			{
				if (GUI.Button(rect, GUIContent.none, state.styles.mute))
				{
					base.track.muted = false;
					state.Refresh();
				}
				result = 16f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00026BA4 File Offset: 0x00024FA4
		private float DrawLockButton(Rect rect, TimelineWindow.TimelineState state)
		{
			float result;
			if (base.track.locked)
			{
				if (GUI.Button(rect, GUIContent.none, state.styles.locked))
				{
					base.locked = false;
				}
				result = 16f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00026C00 File Offset: 0x00025000
		private float DrawInlineCurveButton(Rect rect, TimelineWindow.TimelineState state)
		{
			float result;
			if (!TimelineUtility.TrackHasAnimationCurves(base.track))
			{
				result = 0f;
			}
			else
			{
				bool flag = GUI.Toggle(rect, base.track.showInlineCurves, GUIContent.none, DirectorStyles.Instance.curves);
				if (flag != base.track.showInlineCurves)
				{
					TimelineHelpers.PushUndo(base.track, "showhide.inline.curves");
					base.track.showInlineCurves = flag;
					state.GetWindow().treeView.CalculateRowRects();
				}
				result = 16f;
			}
			return result;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00026C98 File Offset: 0x00025098
		private float DrawRecordButton(Rect rect, TimelineWindow.TimelineState state)
		{
			if (this.trackAllowsRecording)
			{
				TrackAsset track = (!base.isSubTrack()) ? base.track : base.parentTrack();
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(base.track.locked || !state.ValidateBindingForTrack(track).IsValid());
				try
				{
					if (this.IsRecording(state))
					{
						state.editorWindow.Repaint();
						float num = Time.realtimeSinceStartup % 1f;
						GUIContent none = TimelineTrackGUI.s_ArmForRecordContentOn;
						if (num < 0.22f)
						{
							none = GUIContent.none;
						}
						if (GUI.Button(rect, none, GUIStyle.none))
						{
							state.UnarmForRecord(base.track);
						}
					}
					else if (GUI.Button(rect, TimelineTrackGUI.s_ArmForRecordContentOff, GUIStyle.none))
					{
						state.ArmForRecord(base.track);
					}
					return 16f;
				}
				finally
				{
					disabledScope.Dispose();
				}
			}
			return 0f;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00026DC0 File Offset: 0x000251C0
		private void GenerateIconForBindingValidationResult(DirectorStyles styles, TrackBindingValidationResult validationResult)
		{
			if (!validationResult.IsValid())
			{
				if (this.m_ProblemIcon == null)
				{
					this.m_ProblemIcon = new GUIContent();
				}
				switch (validationResult.bindingState)
				{
				case TimelineTrackBindingState.NoGameObjectBound:
					this.m_ProblemIcon.image = styles.warningStyle.normal.background;
					this.m_ProblemIcon.tooltip = "This actor or track is not bound to any GameObject in the scene.";
					break;
				case TimelineTrackBindingState.BoundGameObjectIsDisabled:
					this.m_ProblemIcon.image = styles.warningStyle.normal.background;
					this.m_ProblemIcon.tooltip = "The bound GameObject (" + validationResult.bindingName + ") is disabled";
					break;
				case TimelineTrackBindingState.NoValidComponentOnBoundGameObject:
					this.m_ProblemIcon.image = styles.warningStyle.normal.background;
					this.m_ProblemIcon.tooltip = "Could not find an Animator on" + validationResult.bindingName;
					break;
				case TimelineTrackBindingState.RequiredComponentOnBoundGameObjectIsDisabled:
					this.m_ProblemIcon.image = styles.warningStyle.normal.background;
					this.m_ProblemIcon.tooltip = "Animator is disabled";
					break;
				}
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00026EF8 File Offset: 0x000252F8
		private void DoTrackBindingGUI(Rect rect, Rect headerRect, ITimelineState state)
		{
			rect.y += (rect.height - 16f) / 2f;
			rect.height = 16f;
			rect.width = Mathf.Min(115f, headerRect.xMax - rect.xMin - 80f);
			Object genericBinding = state.currentDirector.GetGenericBinding(base.track);
			if (rect.Contains(Event.current.mousePosition) && this.IsDraggingEvent() && DragAndDrop.objectReferences.Length == 1)
			{
				this.HandleDragAndDrop(state, TimelineTrackGUI.GetRequiredBindingType(this.m_Bindings[0]));
			}
			else
			{
				TrackAsset track = base.track;
				if (state.timeline != state.rootTimeline)
				{
					track = state.rootTrack;
					genericBinding = state.currentDirector.GetGenericBinding(state.rootTrack);
					EditorGUI.BeginDisabled(true);
				}
				switch (this.m_Bindings[0].streamType)
				{
				case 0:
				{
					EditorGUI.BeginChangeCheck();
					Animator animator = EditorGUI.ObjectField(rect, genericBinding, typeof(Animator), true) as Animator;
					if (EditorGUI.EndChangeCheck())
					{
						TimelineTrackGUI.SetTrackBinding(state, track, (!(animator == null)) ? animator.gameObject : null);
					}
					goto IL_226;
				}
				case 1:
				{
					EditorGUI.BeginChangeCheck();
					AudioMixerGroup objectToBind = EditorGUI.ObjectField(rect, genericBinding, typeof(AudioMixerGroup), false) as AudioMixerGroup;
					if (EditorGUI.EndChangeCheck())
					{
						TimelineTrackGUI.SetTrackBinding(state, track, objectToBind);
					}
					goto IL_226;
				}
				case 3:
					if (this.m_Bindings[0].sourceBindingType != null && typeof(Object).IsAssignableFrom(this.m_Bindings[0].sourceBindingType))
					{
						EditorGUI.BeginChangeCheck();
						Object objectToBind2 = EditorGUI.ObjectField(rect, genericBinding, this.m_Bindings[0].sourceBindingType, true);
						if (EditorGUI.EndChangeCheck())
						{
							TimelineTrackGUI.SetTrackBinding(state, track, objectToBind2);
						}
					}
					goto IL_226;
				}
				throw new NotImplementedException("");
				IL_226:
				if (state.timeline != state.rootTimeline)
				{
					EditorGUI.EndDisabled();
				}
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0002714C File Offset: 0x0002554C
		private static Type GetRequiredBindingType(PlayableBinding binding)
		{
			Type result = binding.sourceBindingType;
			if (binding.streamType == null)
			{
				result = typeof(Animator);
			}
			else if (binding.streamType == 1)
			{
				result = typeof(AudioMixerGroup);
			}
			return result;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000271A0 File Offset: 0x000255A0
		private void HandleDragAndDrop(ITimelineState state, Type requiredComponent)
		{
			DragAndDropVisualMode dragAndDropVisualMode = 32;
			if (requiredComponent != null && requiredComponent.IsInstanceOfType(DragAndDrop.objectReferences[0]))
			{
				dragAndDropVisualMode = 2;
				if (Event.current.type == 10)
				{
					TimelineTrackGUI.SetTrackBinding(state, base.track, DragAndDrop.objectReferences[0]);
				}
			}
			else if (typeof(Component).IsAssignableFrom(requiredComponent))
			{
				GameObject gameObjectBeingDragged = DragAndDrop.objectReferences[0] as GameObject;
				if (gameObjectBeingDragged != null)
				{
					dragAndDropVisualMode = 2;
					if (Event.current.type == 10)
					{
						Component component = gameObjectBeingDragged.GetComponent(requiredComponent);
						if (component == null)
						{
							string str = requiredComponent.ToString().Split(".".ToCharArray()).Last<string>();
							GenericMenu genericMenu = new GenericMenu();
							genericMenu.AddItem(EditorGUIUtility.TextContent("Create " + str + " on " + gameObjectBeingDragged.name), false, delegate(object nullParam)
							{
								gameObjectBeingDragged.AddComponent(requiredComponent);
								TimelineTrackGUI.SetTrackBinding(state, this.track, gameObjectBeingDragged);
							}, null);
							genericMenu.AddSeparator("");
							genericMenu.AddItem(EditorGUIUtility.TextContent("Cancel"), false, delegate(object userData)
							{
							}, null);
							genericMenu.ShowAsContext();
						}
						else
						{
							TimelineTrackGUI.SetTrackBinding(state, base.track, gameObjectBeingDragged);
						}
					}
				}
			}
			DragAndDrop.visualMode = dragAndDropVisualMode;
			if (dragAndDropVisualMode == 2)
			{
				DragAndDrop.AcceptDrag();
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00027376 File Offset: 0x00025776
		private static void SetTrackBinding(ITimelineState state, TrackAsset track, Object objectToBind)
		{
			if (state != null)
			{
				state.previewMode = false;
				TimelineUtility.SetBindingInDirector(state.currentDirector, track, objectToBind);
				state.rebuildGraph = true;
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0002739F File Offset: 0x0002579F
		private static void SetTrackBinding(ITimelineState state, TrackAsset track, GameObject gameObjectToBind)
		{
			if (state != null)
			{
				state.previewMode = false;
				TimelineUtility.SetSceneGameObject(state.currentDirector, track, gameObjectToBind);
				state.rebuildGraph = true;
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000273C8 File Offset: 0x000257C8
		private bool IsDraggingEvent()
		{
			return Event.current.type == 9 || Event.current.type == 15 || Event.current.type == 10;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00027410 File Offset: 0x00025810
		private bool IsRecording(TimelineWindow.TimelineState state)
		{
			return state.recording && state.IsArmedForRecord(base.track);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00027440 File Offset: 0x00025840
		private void DrawClips(Rect trackRect, TimelineWindow.TimelineState state)
		{
			for (int num = 0; num != this.m_ClipGUICache.Count; num++)
			{
				TimelineClipGUI timelineClipGUI = this.m_ClipGUICache[num];
				if (num + 1 < this.m_ClipGUICache.Count)
				{
					timelineClipGUI.nextClip = this.m_ClipGUICache[num + 1];
				}
				if (num - 1 >= 0)
				{
					timelineClipGUI.previousClip = this.m_ClipGUICache[num - 1];
				}
				timelineClipGUI.DrawBlendingCurves();
				Rect trackRect2 = trackRect;
				if (base.track.displayCascadeClips)
				{
					float num2 = Mathf.Ceil(trackRect.height / 2f);
					if (timelineClipGUI.parityID == 1)
					{
						trackRect2.y += num2;
					}
					trackRect2.height = num2;
				}
				timelineClipGUI.Draw(trackRect2, state, base.drawer);
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0002751C File Offset: 0x0002591C
		public void DrawRecordingTrackBackground(Rect trackRect)
		{
			EditorGUI.DrawRect(trackRect, DirectorStyles.Instance.customSkin.colorTrackBackgroundRecording);
			Graphics.ShadowLabel(trackRect, this.m_Styles.Elipsify(DirectorStyles.Instance.recordingLabel.text, trackRect, this.m_Styles.fontClip), this.m_Styles.fontClip, Color.white, Color.black);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00027580 File Offset: 0x00025980
		private void DrawClipConnectors(Rect trackRect)
		{
			double num = double.MinValue;
			List<TimelineClipGUI> list = (from c in this.m_ClipGUICache
			where c.visible
			orderby c.clip.start
			select c).ToList<TimelineClipGUI>();
			GUIStyle connector = this.m_Styles.connector;
			foreach (TimelineClipGUI timelineClipGUI in list)
			{
				double value = timelineClipGUI.clip.start - num;
				if (timelineClipGUI.clippedRect.width > 14f && Math.Abs(value) < 1E-09)
				{
					Rect clippedRect = timelineClipGUI.clippedRect;
					clippedRect.x -= connector.fixedWidth / 2f;
					clippedRect.width = connector.fixedWidth;
					clippedRect.height = connector.fixedHeight;
					GUI.Box(clippedRect, GUIContent.none, connector);
				}
				num = timelineClipGUI.clip.start + timelineClipGUI.clip.duration;
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000276DC File Offset: 0x00025ADC
		private void UpdateClipOverlaps(TimelineWindow.TimelineState state, Rect trackRect)
		{
			TrackExtensions.ComputeBlendsFromOverlaps((from c in this.m_ClipGUICache
			select c.clip).ToArray<TimelineClip>());
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00027714 File Offset: 0x00025B14
		public void RebuildGUICache(TimelineWindow.TimelineState state)
		{
			TimelineClipGUI[] array = (from x in state.selection.FilterByType<TimelineClipGUI>()
			where x.clip != null && x.clip.parentTrack == base.track
			select x).ToArray<TimelineClipGUI>();
			foreach (TimelineClipGUI item in array)
			{
				state.selection.Remove(item);
			}
			TimelineClipGUI[] array3 = (from x in state.captured.OfType<TimelineClipGUI>()
			where x.clip != null && x.clip.parentTrack == base.track
			select x).ToArray<TimelineClipGUI>();
			foreach (TimelineClipGUI item2 in array3)
			{
				state.captured.Remove(item2);
			}
			this.m_ChildrenControls = (from x in this.m_ChildrenControls
			where x.GetType() != typeof(TimelineClipGUI)
			select x).ToList<Control>();
			this.m_ClipGUICache = new List<TimelineClipGUI>();
			foreach (TimelineClip timelineClip in base.track.clips)
			{
				TimelineClipGUI item3 = new TimelineClipGUI(timelineClip, this);
				this.m_ClipGUICache.Add(item3);
				this.m_ChildrenControls.Add(item3);
				if (timelineClip.selected)
				{
					state.selection.Add(item3);
				}
			}
			this.SortClipsByStartTime();
			this.m_TrackHash = this.ComputeTrackHash();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00027883 File Offset: 0x00025C83
		public void SortClipsByStartTime()
		{
			this.m_ClipGUICache = (from x in this.m_ClipGUICache
			orderby x.clip.start
			select x).ToList<TimelineClipGUI>();
			this.m_MustSortClips = true;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000278C0 File Offset: 0x00025CC0
		public int BlendHash()
		{
			double num = 0.0;
			int num2 = 17;
			for (int i = 0; i < this.m_ClipGUICache.Count; i++)
			{
				TimelineClipGUI timelineClipGUI = this.m_ClipGUICache[i];
				num += -timelineClipGUI.clip.start + timelineClipGUI.clip.duration;
				num2 += timelineClipGUI.clip.blendInCurveMode;
				num2 += 127 * timelineClipGUI.clip.blendOutCurveMode;
			}
			return num.GetHashCode() ^ num2;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00027958 File Offset: 0x00025D58
		public override void OnGraphRebuilt()
		{
			this.RefreshCurveEditor();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00027964 File Offset: 0x00025D64
		public void RefreshCurveEditor()
		{
			AnimationTrack animationTrack = base.track as AnimationTrack;
			TimelineWindow instance = TimelineWindow.instance;
			if (animationTrack != null && instance != null && instance.state != null)
			{
				bool flag = this.clipCurveEditor != null;
				bool flag2 = animationTrack.ShouldShowInfiniteClipEditor();
				if (flag != flag2)
				{
					instance.state.AddEndFrameDelegate(delegate(ITimelineState x)
					{
						x.Refresh();
					});
				}
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x000279EC File Offset: 0x00025DEC
		private bool DoesTrackAllowRecording()
		{
			bool result;
			if (base.track is AnimationTrack)
			{
				result = true;
			}
			else
			{
				result = base.track.clips.Any((TimelineClip c) => c.HasAnyAnimatableParameters());
			}
			return result;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00027A45 File Offset: 0x00025E45
		public void ResetParityID()
		{
			this.m_ClipParityID = 0;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00027A50 File Offset: 0x00025E50
		public int GetNextParityID()
		{
			int clipParityID = this.m_ClipParityID;
			this.m_ClipParityID++;
			this.m_ClipParityID %= 2;
			return clipParityID;
		}

		// Token: 0x0400031F RID: 799
		private List<TimelineClipGUI> m_ClipGUICache = new List<TimelineClipGUI>();

		// Token: 0x04000320 RID: 800
		private bool m_MustSortClips = false;

		// Token: 0x04000321 RID: 801
		private static GUIContent s_ArmForRecordContentOn;

		// Token: 0x04000322 RID: 802
		private static GUIContent s_ArmForRecordContentOff;

		// Token: 0x04000323 RID: 803
		private bool m_HadProblems;

		// Token: 0x04000324 RID: 804
		private bool m_InitHadProblems;

		// Token: 0x04000325 RID: 805
		private int m_TrackHash = -1;

		// Token: 0x04000326 RID: 806
		private int m_BlendHash = -1;

		// Token: 0x04000327 RID: 807
		private readonly GUIContent m_HeaderIcon;

		// Token: 0x04000328 RID: 808
		private Rect m_IconRect;

		// Token: 0x04000329 RID: 809
		private Rect m_ButtonsRect;

		// Token: 0x0400032A RID: 810
		private readonly PlayableBinding[] m_Bindings;

		// Token: 0x0400032B RID: 811
		private bool? m_TrackAllowsRecording = null;

		// Token: 0x0400032C RID: 812
		private readonly InfiniteTrackDrawer m_InfiniteTrackDrawer = null;

		// Token: 0x0400032D RID: 813
		private int m_ClipParityID = 0;

		// Token: 0x0400032E RID: 814
		private static readonly GUIContent s_LockMuteOverlay = new GUIContent();

		// Token: 0x0400032F RID: 815
		private const int k_SelectedClipZOrder = 1000;

		// Token: 0x04000330 RID: 816
		private const float k_ButtonSize = 16f;

		// Token: 0x04000331 RID: 817
		private const float k_ButtonPadding = 3f;

		// Token: 0x04000332 RID: 818
		private const float k_LockTextPadding = 40f;

		// Token: 0x04000333 RID: 819
		private bool m_InlineCurvesSelected = false;
	}
}
