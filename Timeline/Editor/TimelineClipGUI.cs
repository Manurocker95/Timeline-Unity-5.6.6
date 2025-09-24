using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x0200007F RID: 127
	internal class TimelineClipGUI : Control, IBounds, ISelectable, IClipCurveEditorOwner
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x0001F8D8 File Offset: 0x0001DCD8
		public TimelineClipGUI(TimelineClip clip, TimelineTrackGUI parent)
		{
			this.m_ParentTrack = parent;
			this.m_Clip = clip;
			this.m_Styles = DirectorStyles.Instance;
			this.m_ID = clip.m_ID;
			this.m_Clip.dirtyHash = 0;
			this.supportResize = true;
			if (parent.drawer != null)
			{
				parent.drawer.ConfigureUIClip(this);
			}
			DragClipHandle clipHandleManipulator = (!TimelineClipCapsExtensions.SupportsClipIn(clip)) ? new SimpleDragClipHandle() : new DragClipHandle();
			this.m_LeftHandle = new TimelineClipHandle(this, TimelineClipHandle.DragDirection.Left, clipHandleManipulator);
			this.m_RightHandle = new TimelineClipHandle(this, TimelineClipHandle.DragDirection.Right, clipHandleManipulator);
			this.m_ClipStartTime = new TimelineClipTimeField(this, TimelineClipTimeField.Mode.Start);
			this.m_ClipEndTime = new TimelineClipTimeField(this, TimelineClipTimeField.Mode.End);
			this.m_BlendInHandle = new TimelineBlendHandle(this, TimelineBlendHandle.DragDirection.Left);
			this.m_BlendOutHandle = new TimelineBlendHandle(this, TimelineBlendHandle.DragDirection.Right);
			base.AddChild(this.m_LeftHandle);
			base.AddChild(this.m_RightHandle);
			base.AddChild(this.m_BlendInHandle);
			base.AddChild(this.m_BlendOutHandle);
			base.AddChild(this.m_ClipStartTime);
			base.AddChild(this.m_ClipEndTime);
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001FA20 File Offset: 0x0001DE20
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0001FA3A File Offset: 0x0001DE3A
		public bool supportResize { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001FA44 File Offset: 0x0001DE44
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0001FA5E File Offset: 0x0001DE5E
		public ClipCurveEditor clipCurveEditor { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001FA68 File Offset: 0x0001DE68
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0001FA82 File Offset: 0x0001DE82
		public EditorClip editorClip { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0001FA8C File Offset: 0x0001DE8C
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0001FAA6 File Offset: 0x0001DEA6
		public bool hideName { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0001FAB0 File Offset: 0x0001DEB0
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0001FACA File Offset: 0x0001DECA
		public TimelineClipGUI previousClip { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001FAD4 File Offset: 0x0001DED4
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0001FAEE File Offset: 0x0001DEEE
		public TimelineClipGUI nextClip { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001FAF8 File Offset: 0x0001DEF8
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0001FB12 File Offset: 0x0001DF12
		public bool visible { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001FB1C File Offset: 0x0001DF1C
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0001FB37 File Offset: 0x0001DF37
		public int zOrder
		{
			get
			{
				return this.m_ZOrder;
			}
			set
			{
				this.m_ZOrder = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001FB44 File Offset: 0x0001DF44
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0001FB5F File Offset: 0x0001DF5F
		public Rect rect
		{
			get
			{
				return this.m_Rect;
			}
			set
			{
				this.m_Rect = value;
				if (this.m_Rect.width < 0f)
				{
					this.m_Rect.width = 1f;
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001FB90 File Offset: 0x0001DF90
		public override Rect bounds
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001FBAC File Offset: 0x0001DFAC
		public Rect boundingRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001FBC8 File Offset: 0x0001DFC8
		public float blendingStopsAt
		{
			get
			{
				return (float)(this.clip.start + Math.Max(0.0, this.clip.blendInDuration));
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001FC04 File Offset: 0x0001E004
		public ReadOnlyCollection<Rect> loopRects
		{
			get
			{
				return this.m_LoopRects.AsReadOnly();
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0001FC24 File Offset: 0x0001E024
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0001FC3F File Offset: 0x0001E03F
		public Rect clippedRect
		{
			get
			{
				return this.m_ClippedRect;
			}
			set
			{
				this.m_ClippedRect = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001FC4C File Offset: 0x0001E04C
		public bool overlaps
		{
			get
			{
				return this.clip.hasBlendIn;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0001FC6C File Offset: 0x0001E06C
		public bool isOverlapped
		{
			get
			{
				return this.clip.hasBlendOut;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001FC8C File Offset: 0x0001E08C
		public TimelineTrackGUI parentTrack
		{
			get
			{
				return this.m_ParentTrack;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001FCA8 File Offset: 0x0001E0A8
		public string name
		{
			get
			{
				string result;
				if (this.hideName)
				{
					result = string.Empty;
				}
				else if (this.clip.displayName == null)
				{
					result = "(Empty)";
				}
				else
				{
					result = this.clip.displayName;
				}
				return result;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001FCFC File Offset: 0x0001E0FC
		public Rect UnClippedRect
		{
			get
			{
				return this.m_UnclippedRect;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001FD18 File Offset: 0x0001E118
		public bool selectable
		{
			get
			{
				return this.clip != null && !this.clip.locked;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001FD4C File Offset: 0x0001E14C
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x0001FD6C File Offset: 0x0001E16C
		public bool selected
		{
			get
			{
				return this.clip.selected;
			}
			set
			{
				this.clip.selected = value;
				this.OnSelectedChanged(value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001FD84 File Offset: 0x0001E184
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x0001FDA4 File Offset: 0x0001E1A4
		public bool inlineCurvesSelected
		{
			get
			{
				return this.clip.inlineCurvesSelected;
			}
			set
			{
				if (value)
				{
					Selection.UnselectAll();
					this.OnSelectedChanged(true);
					this.clip.selected = false;
				}
				this.clip.inlineCurvesSelected = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001FDD4 File Offset: 0x0001E1D4
		public object selectableObject
		{
			get
			{
				return this.clip;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001FDF0 File Offset: 0x0001E1F0
		public Rect mixOutRect
		{
			get
			{
				float mixOutPercentage = this.clip.mixOutPercentage;
				this.m_MixOutRect.Set(this.UnClippedRect.min.x + this.UnClippedRect.width * (1f - mixOutPercentage), this.UnClippedRect.min.y, this.UnClippedRect.width * mixOutPercentage, this.UnClippedRect.height);
				return this.m_MixOutRect;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001FE8C File Offset: 0x0001E28C
		public Rect mixInRect
		{
			get
			{
				this.m_MixInRect.Set(this.UnClippedRect.xMin, this.UnClippedRect.yMin, this.UnClippedRect.width * this.clip.mixInPercentage, this.UnClippedRect.height);
				return this.m_MixInRect;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001FEF8 File Offset: 0x0001E2F8
		internal TimelineClipGUI.BlendKind blendInKind
		{
			get
			{
				TimelineClipGUI.BlendKind result;
				if (this.mixInRect.width > TimelineClipGUI.k_MinMixWidth && this.overlaps)
				{
					result = TimelineClipGUI.BlendKind.Mix;
				}
				else if (this.mixInRect.width > TimelineClipGUI.k_MinMixWidth)
				{
					result = TimelineClipGUI.BlendKind.Ease;
				}
				else
				{
					result = TimelineClipGUI.BlendKind.None;
				}
				return result;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001FF58 File Offset: 0x0001E358
		internal TimelineClipGUI.BlendKind blendOutKind
		{
			get
			{
				TimelineClipGUI.BlendKind result;
				if (this.mixOutRect.width > TimelineClipGUI.k_MinMixWidth && this.isOverlapped)
				{
					result = TimelineClipGUI.BlendKind.Mix;
				}
				else if (this.mixOutRect.width > TimelineClipGUI.k_MinMixWidth)
				{
					result = TimelineClipGUI.BlendKind.Ease;
				}
				else
				{
					result = TimelineClipGUI.BlendKind.None;
				}
				return result;
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001FFB8 File Offset: 0x0001E3B8
		public int Hash()
		{
			return this.m_ID.GetHashCode() ^ this.m_Clip.Hash();
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0001FFF0 File Offset: 0x0001E3F0
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x00020010 File Offset: 0x0001E410
		public double start
		{
			get
			{
				return this.clip.start;
			}
			set
			{
				this.clip.start = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00020020 File Offset: 0x0001E420
		public double end
		{
			get
			{
				return this.clip.end;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00020040 File Offset: 0x0001E440
		public double duration
		{
			get
			{
				return this.clip.duration;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00020060 File Offset: 0x0001E460
		public bool supportsLooping
		{
			get
			{
				return TimelineClipCapsExtensions.SupportsLooping(this.clip);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00020080 File Offset: 0x0001E480
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0002009A File Offset: 0x0001E49A
		public int parityID { get; set; }

		// Token: 0x06000429 RID: 1065 RVA: 0x000200A4 File Offset: 0x0001E4A4
		private void CreateInlineCurveEditor(TimelineWindow.TimelineState state)
		{
			if (Event.current.type == 8)
			{
				if (this.clipCurveEditor == null)
				{
					AnimationClip animationClip = this.clip.animationClip;
					if (animationClip != null && animationClip.empty)
					{
						animationClip = null;
					}
					if (animationClip != null && !this.clip.recordable)
					{
						animationClip = null;
					}
					if (this.clip.curves != null || animationClip != null)
					{
						state.onEndFrame = (PendingUpdateDelegate)Delegate.Combine(state.onEndFrame, new PendingUpdateDelegate(delegate(ITimelineState istate)
						{
							this.clipCurveEditor = new ClipCurveEditor(new TimelineClipCurveDataSource(this), this.m_ParentTrack.TimelineWindow);
						}));
					}
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00020160 File Offset: 0x0001E560
		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0002017C File Offset: 0x0001E57C
		public virtual void Delete(ITimelineState state)
		{
			TrackAsset parentTrack = this.clip.parentTrack;
			ClipModifier.Delete(state.timeline, this.clip);
			parentTrack.OnEnable();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x000201B0 File Offset: 0x0001E5B0
		public void Reselect()
		{
			TimelineWindow.TimelineState state = TimelineWindow.instance.state;
			state.selection.SelectInEditor(this.editorClip);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x000201DC File Offset: 0x0001E5DC
		public override bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession)
		{
			return (this.parentTrack == null || !this.parentTrack.track.locked) && base.OnEvent(evt, state, isCaptureSession);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00020224 File Offset: 0x0001E624
		private void OnSelectedChanged(bool value)
		{
			AnimationCurvePreviewCache.ClearCache();
			Object obj = null;
			if (!(TimelineWindow.instance == null))
			{
				TimelineWindow.TimelineState state = TimelineWindow.instance.state;
				if (this.editorClip == null)
				{
					this.editorClip = EditorClip.CreateEditorClip(state.timeline, this.clip);
				}
				if (this.editorClip != null)
				{
					obj = this.editorClip;
					this.editorClip.director = state.currentDirector;
				}
				if (value)
				{
					Selection.UnselectInlineCurves();
					EditorClip editorClip = Selection.objects.OfType<EditorClip>().FirstOrDefault((EditorClip x) => x.clip == this.clip);
					if (editorClip == null || editorClip.clip != this.clip)
					{
						state.selection.SelectInEditor(obj);
					}
					else
					{
						this.editorClip = editorClip;
						this.editorClip.director = state.currentDirector;
					}
				}
				else
				{
					List<EditorClip> list = (from x in Selection.objects.OfType<EditorClip>()
					where x.clip == this.clip
					select x).ToList<EditorClip>();
					foreach (EditorClip editorClip2 in list)
					{
						Selection.Remove(editorClip2);
					}
				}
				this.m_ParentTrack.resortClips = true;
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000203A4 File Offset: 0x0001E7A4
		public void DrawDragPreview(Rect rect, Color color)
		{
			EditorGUI.DrawRect(rect, color);
			Graphics.ShadowLabel(rect, this.name, this.m_Styles.fontClip, Color.white, Color.black);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000203D0 File Offset: 0x0001E7D0
		private int ComputeDirtyHash()
		{
			return this.clip.selected.GetHashCode() ^ this.clip.clipAssetDuration.GetHashCode() ^ this.clip.duration.GetHashCode() ^ this.clip.timeScale.GetHashCode() ^ this.clip.start.GetHashCode();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0002046C File Offset: 0x0001E86C
		private void DrawClipByDrawer(Rect rect, string title, GUIStyle style, ITimelineState state, float timeOffset, bool callCustom)
		{
			this.m_ClipDrawData.uiClip = this;
			this.m_ClipDrawData.clip = this.clip;
			this.m_ClipDrawData.targetRect = rect;
			this.m_ClipDrawData.clipCenterSection = this.m_ClipCenterSection;
			this.m_ClipDrawData.unclippedRect = this.UnClippedRect;
			this.m_ClipDrawData.title = title;
			this.m_ClipDrawData.selected = this.selected;
			this.m_ClipDrawData.inlineCurvesSelected = this.inlineCurvesSelected;
			this.m_ClipDrawData.style = style;
			this.m_ClipDrawData.state = state;
			this.m_ClipDrawData.selectedStyle = this.m_Styles.selectedStyle;
			this.m_ClipDrawData.visibleTime = new Vector2((float)this.clip.ToLocalTimeUnbound((double)state.TimeAreaPixelToTime(rect.x + timeOffset)), (float)this.clip.ToLocalTimeUnbound((double)state.TimeAreaPixelToTime(rect.xMax + timeOffset)));
			this.parentTrack.drawer.DrawClip(this.m_ClipDrawData);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00020584 File Offset: 0x0001E984
		public void DrawInto(Rect rect, TimelineWindow.TimelineState state, TrackDrawer drawer)
		{
			this.CreateInlineCurveEditor(state);
			this.m_Rect = rect;
			float x = this.m_Rect.x;
			GUI.BeginClip(this.m_Rect);
			Rect rect2 = this.m_Rect;
			rect2.x = 0f;
			rect2.y = 0f;
			string str = "";
			if (this.clip.selected && !object.Equals(1.0, this.clip.timeScale))
			{
				str = " " + this.clip.timeScale.ToString("F2") + "x";
			}
			string title = this.m_Styles.Elipsify(this.name, rect2, this.m_Styles.fontClip) + str;
			this.DrawClipByDrawer(rect2, title, this.m_Styles.sequenceClip, state, x, true);
			GUI.EndClip();
			if (this.selected && this.supportResize)
			{
				Rect bounds = this.bounds;
				bounds.xMin += this.m_LeftHandle.bounds.width;
				bounds.xMax -= this.m_RightHandle.bounds.width;
				EditorGUIUtility.AddCursorRect(bounds, 8);
			}
			if (this.supportResize)
			{
				if (this.m_Rect.width > 3f * this.m_Styles.sequenceClipHandle.fixedWidth)
				{
					this.m_LeftHandle.Draw(this.m_Rect);
				}
				this.m_RightHandle.Draw(this.m_Rect);
				state.quadTree.Insert(this.m_LeftHandle);
				state.quadTree.Insert(this.m_RightHandle);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00020764 File Offset: 0x0001EB64
		private void CalculateClipRectangle(TrackAsset parentTrack, Rect trackRect, TimelineWindow.TimelineState state, int projectedClipHash)
		{
			if (this.m_ProjectedClipHash == projectedClipHash)
			{
				if (Event.current.type == 7 && !parentTrack.locked)
				{
					state.quadTree.Insert(this);
				}
			}
			else
			{
				this.m_ProjectedClipHash = projectedClipHash;
				this.visible = false;
				Rect rect = this.RectToTimeline(trackRect, state);
				if (rect.width < DirectorStyles.Instance.eventWhite.fixedWidth + 1f)
				{
					rect.width = DirectorStyles.Instance.eventWhite.fixedWidth + 1f;
					rect.x -= DirectorStyles.Instance.eventWhite.fixedWidth / 2f;
				}
				this.rect = rect;
				this.m_UnclippedRect = rect;
				if (Event.current.type == 7 && !parentTrack.locked)
				{
					state.quadTree.Insert(this);
				}
				if (rect.x < trackRect.xMin)
				{
					float num = trackRect.xMin - rect.x;
					rect.x = trackRect.xMin;
					rect.width -= num;
				}
				this.clippedRect = rect;
				if (rect.xMax >= trackRect.xMin)
				{
					if (rect.xMin <= trackRect.xMax)
					{
						if (this.clippedRect.width < 2f)
						{
							this.m_ClippedRect.width = 5f;
						}
						this.visible = true;
					}
				}
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00020908 File Offset: 0x0001ED08
		private void CalculateBlendRect()
		{
			this.m_ClipCenterSection = this.rect;
			this.m_ClipCenterSection.x = 0f;
			this.m_ClipCenterSection.y = 0f;
			this.m_ClipCenterSection.xMin = this.UnClippedRect.width * this.clip.mixInPercentage;
			this.m_ClipCenterSection.width = this.rect.width;
			this.m_ClipCenterSection.xMax = this.m_ClipCenterSection.xMax - this.mixOutRect.width;
			this.m_ClipCenterSection.xMax = this.m_ClipCenterSection.xMax - this.UnClippedRect.width * this.clip.mixInPercentage;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000209CC File Offset: 0x0001EDCC
		private static bool IsEmptyRecordingClip(TimelineClip clip)
		{
			bool result;
			if (clip == null || !clip.recordable)
			{
				result = false;
			}
			else
			{
				AnimationClip animationClip = clip.animationClip;
				result = (!(animationClip == null) && animationClip.empty);
			}
			return result;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00020A1C File Offset: 0x0001EE1C
		public virtual void Draw(Rect trackRect, TimelineWindow.TimelineState state, TrackDrawer drawer)
		{
			if (!TimelineClipGUI.IsEmptyRecordingClip(this.clip))
			{
				if (this.selected && !state.selection.Contains(this))
				{
					state.selection.Add(this);
				}
				if (this.clip.selected || this.clip.inlineCurvesSelected)
				{
					this.clip.dirtyHash = 0;
				}
				int num = this.ComputeDirtyHash();
				int num2 = state.timeAreaTranslation.GetHashCode() ^ state.timeAreaScale.GetHashCode() ^ trackRect.GetHashCode();
				this.CalculateClipRectangle(this.parentTrack.track, trackRect, state, num ^ num2);
				this.CalculateBlendRect();
				this.CalculateLoopRects(trackRect, state, num);
				this.clip.dirtyHash = num;
				if (drawer.canDrawExtrapolationIcon)
				{
					this.DrawExtrapolation(trackRect, this.UnClippedRect);
				}
				this.DrawInto(this.m_Rect, state, drawer);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00020B2C File Offset: 0x0001EF2C
		public void DrawBlendingCurves()
		{
			if (Event.current.type == 7)
			{
				Color color = (!this.selected) ? Color.white : TrackDrawer.GetHighlightColor(Color.white);
				Color colorTrackBackground = DirectorStyles.Instance.customSkin.colorTrackBackground;
				Color color2 = (!this.selected) ? DirectorStyles.Instance.customSkin.colorTrackBackground : Color.white;
				if (this.blendInKind == TimelineClipGUI.BlendKind.Ease)
				{
					ClipRenderer.RenderTexture(this.mixInRect, DirectorStyles.Instance.timelineClip.normal.background, DirectorStyles.Instance.blendingIn.normal.background, color, false);
					EditorGUI.DrawRect(new Rect(this.mixInRect.xMax - 2f, this.mixInRect.yMin, 2f, this.mixInRect.height), colorTrackBackground);
					Graphics.DrawAAPolyLine(4f, new Vector3[]
					{
						new Vector3(this.mixInRect.xMin + 1f, this.mixInRect.yMax - 1f, 0f),
						new Vector3(this.mixInRect.xMax, this.mixInRect.yMin - 1.5f, 0f)
					}, color2);
				}
				if (this.blendOutKind == TimelineClipGUI.BlendKind.Ease || this.blendOutKind == TimelineClipGUI.BlendKind.Mix)
				{
					ClipRenderer.RenderTexture(this.mixOutRect, DirectorStyles.Instance.timelineClip.normal.background, DirectorStyles.Instance.blendingOut.normal.background, color, false);
					EditorGUI.DrawRect(new Rect(this.mixOutRect.xMin, this.mixOutRect.yMin, 2f, this.mixOutRect.height), colorTrackBackground);
					Graphics.DrawLineAA(4f, new Vector3(this.mixOutRect.xMin + 1.5f, this.mixOutRect.yMin + 1.5f, 0f), new Vector3(this.mixOutRect.xMax, this.mixOutRect.yMax - 1f, 0f), color2);
				}
				if (this.blendInKind == TimelineClipGUI.BlendKind.Mix)
				{
					ClipRenderer.RenderTexture(this.mixInRect, DirectorStyles.Instance.timelineClip.normal.background, DirectorStyles.Instance.blendingOut.normal.background, color, false);
					EditorGUI.DrawRect(new Rect(this.mixInRect.xMax, this.mixInRect.yMin, 2f, this.mixOutRect.height), colorTrackBackground);
					Graphics.DrawAAPolyLine(4f, new Vector3[]
					{
						new Vector3(this.mixInRect.xMin, this.mixInRect.yMin, 0f),
						new Vector3(this.mixInRect.xMax, this.mixInRect.yMax - 1f, 0f)
					}, color2);
				}
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00020EAC File Offset: 0x0001F2AC
		private GUIStyle GetExtrapolationIcon(TimelineClip.ClipExtrapolation mode)
		{
			GUIStyle result = null;
			switch (mode)
			{
			case 0:
				return null;
			case 1:
				result = this.m_Styles.extrapolationHold;
				break;
			case 2:
				result = this.m_Styles.extrapolationLoop;
				break;
			case 3:
				result = this.m_Styles.extrapolationPingPong;
				break;
			case 4:
				result = this.m_Styles.extrapolationContinue;
				break;
			}
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00020F30 File Offset: 0x0001F330
		private Rect GetPreExtrapolationBounds(Rect trackRect, Rect clipRect, GUIStyle icon)
		{
			float num = clipRect.xMin - (icon.fixedWidth + 10f);
			float num2 = trackRect.yMin + (trackRect.height - icon.fixedHeight) / 2f;
			if (this.previousClip != null)
			{
				float num3 = Mathf.Abs(this.UnClippedRect.xMin - this.previousClip.UnClippedRect.xMax);
				if (num3 < icon.fixedWidth)
				{
					return new Rect(0f, 0f, 0f, 0f);
				}
				if (num3 < icon.fixedWidth + 20f)
				{
					float num4 = (num3 - icon.fixedWidth) / 2f;
					num = clipRect.xMin - (icon.fixedWidth + num4);
				}
			}
			return new Rect(num, num2, icon.fixedWidth, icon.fixedHeight);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00021024 File Offset: 0x0001F424
		private Rect GetPostExtrapolationBounds(Rect trackRect, Rect clipRect, GUIStyle icon)
		{
			float num = clipRect.xMax + 10f;
			float num2 = trackRect.yMin + (trackRect.height - icon.fixedHeight) / 2f;
			if (this.nextClip != null)
			{
				float num3 = Mathf.Abs(this.nextClip.UnClippedRect.xMin - this.UnClippedRect.xMax);
				if (num3 < icon.fixedWidth)
				{
					return new Rect(0f, 0f, 0f, 0f);
				}
				if (num3 < icon.fixedWidth + 20f)
				{
					float num4 = (num3 - icon.fixedWidth) / 2f;
					num = clipRect.xMax + num4;
				}
			}
			return new Rect(num, num2, icon.fixedWidth, icon.fixedHeight);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0002110A File Offset: 0x0001F50A
		private void DrawExtrapolationIcon(Rect rect, Color color, GUIStyle icon)
		{
			GUI.Label(rect, GUIContent.none, icon);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0002111C File Offset: 0x0001F51C
		private void DrawExtrapolation(Rect trackRect, Rect clipRect)
		{
			if (this.clip.hasPreExtrapolation)
			{
				GUIStyle extrapolationIcon = this.GetExtrapolationIcon(this.clip.preExtrapolationMode);
				if (extrapolationIcon != null)
				{
					Rect preExtrapolationBounds = this.GetPreExtrapolationBounds(trackRect, clipRect, extrapolationIcon);
					if (preExtrapolationBounds.width > 1f && preExtrapolationBounds.height > 1f)
					{
						this.DrawExtrapolationIcon(preExtrapolationBounds, Color.white, extrapolationIcon);
					}
				}
			}
			if (this.clip.hasPostExtrapolation)
			{
				GUIStyle extrapolationIcon2 = this.GetExtrapolationIcon(this.clip.postExtrapolationMode);
				if (extrapolationIcon2 != null)
				{
					Rect postExtrapolationBounds = this.GetPostExtrapolationBounds(trackRect, clipRect, extrapolationIcon2);
					if (postExtrapolationBounds.width > 1f && postExtrapolationBounds.height > 1f)
					{
						this.DrawExtrapolationIcon(postExtrapolationBounds, Color.white, extrapolationIcon2);
					}
				}
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000211F4 File Offset: 0x0001F5F4
		private static Rect ProjectRectOnTimeline(Rect rect, Rect trackRect, TimelineWindow.TimelineState state)
		{
			double localStart = state.GetWindow().context.localStart;
			rect.x += (float)localStart;
			Rect result = rect;
			result.x *= state.timeAreaScale.x;
			result.width *= state.timeAreaScale.x;
			result.x += state.timeAreaTranslation.x + trackRect.xMin;
			result.y = trackRect.y + 2f;
			result.height = trackRect.height - 4f;
			return result;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000212B4 File Offset: 0x0001F6B4
		public void CalculateLoopRects(Rect trackRect, TimelineWindow.TimelineState state, int currentClipHash)
		{
			if (this.clip.duration >= TimelineWindow.TimelineState.kTimeEpsilon)
			{
				if (this.clip.dirtyHash != currentClipHash)
				{
					float num = 0f;
					this.m_LoopRects.Clear();
					double[] loopTimes = TimelineHelpers.GetLoopTimes(this.clip);
					double loopDuration = TimelineHelpers.GetLoopDuration(this.clip);
					foreach (double num2 in loopTimes)
					{
						float num3 = Mathf.Min((float)(this.clip.duration - num2), (float)loopDuration);
						Rect item = TimelineClipGUI.ProjectRectOnTimeline(new Rect((float)(num2 + this.clip.start), 0f, num3, 0f), trackRect, state);
						this.m_LoopRects.Add(item);
						num += item.width;
					}
					if (num < 2f)
					{
						this.m_LoopRects.Clear();
					}
				}
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000213B0 File Offset: 0x0001F7B0
		public Rect RectToTimeline(Rect trackRect, TimelineWindow.TimelineState state)
		{
			Rect result;
			result..ctor((float)(this.clip.start + state.GetWindow().context.localStart) * state.timeAreaScale.x, 0f, (float)this.clip.duration * state.timeAreaScale.x, 0f);
			result.xMin += state.timeAreaTranslation.x + trackRect.xMin;
			result.xMax += state.timeAreaTranslation.x + trackRect.xMin;
			result.y = trackRect.y + 2f;
			result.height = trackRect.height - 4f;
			result.y = trackRect.y;
			result.height = trackRect.height;
			return result;
		}

		// Token: 0x0400029F RID: 671
		private readonly TimelineClip m_Clip;

		// Token: 0x040002A0 RID: 672
		private readonly DirectorStyles m_Styles;

		// Token: 0x040002A1 RID: 673
		private Rect m_Rect;

		// Token: 0x040002A2 RID: 674
		private Rect m_ClipCenterSection;

		// Token: 0x040002A3 RID: 675
		private readonly List<Rect> m_LoopRects = new List<Rect>();

		// Token: 0x040002A4 RID: 676
		private Rect m_ClippedRect;

		// Token: 0x040002A5 RID: 677
		private readonly int m_ID;

		// Token: 0x040002A6 RID: 678
		private Rect m_UnclippedRect;

		// Token: 0x040002A7 RID: 679
		private int m_ProjectedClipHash;

		// Token: 0x040002A8 RID: 680
		private readonly TimelineTrackGUI m_ParentTrack;

		// Token: 0x040002A9 RID: 681
		private readonly TimelineClipHandle m_LeftHandle;

		// Token: 0x040002AA RID: 682
		private readonly TimelineClipHandle m_RightHandle;

		// Token: 0x040002AB RID: 683
		private readonly TimelineBlendHandle m_BlendInHandle;

		// Token: 0x040002AC RID: 684
		private readonly TimelineBlendHandle m_BlendOutHandle;

		// Token: 0x040002AD RID: 685
		private readonly TimelineClipTimeField m_ClipStartTime;

		// Token: 0x040002AE RID: 686
		private readonly TimelineClipTimeField m_ClipEndTime;

		// Token: 0x040002AF RID: 687
		private TrackDrawer.ClipDrawData m_ClipDrawData;

		// Token: 0x040002B0 RID: 688
		private Rect m_MixOutRect = default(Rect);

		// Token: 0x040002B1 RID: 689
		private Rect m_MixInRect = default(Rect);

		// Token: 0x040002B2 RID: 690
		private int m_ZOrder = 0;

		// Token: 0x040002BA RID: 698
		private static readonly float k_MinMixWidth = 2f;

		// Token: 0x02000080 RID: 128
		internal enum BlendKind
		{
			// Token: 0x040002BD RID: 701
			None,
			// Token: 0x040002BE RID: 702
			Ease,
			// Token: 0x040002BF RID: 703
			Mix
		}
	}
}
