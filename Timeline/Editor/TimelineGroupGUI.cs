using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x02000089 RID: 137
	internal class TimelineGroupGUI : TimelineTrackBaseGUI
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x000247B4 File Offset: 0x00022BB4
		public TimelineGroupGUI(TreeViewController treeview, TimelineTreeViewGUI treeviewGUI, int id, int depth, TreeViewItem parent, string displayName, TrackAsset trackAsset, bool isRoot) : base(id, depth, parent, displayName, trackAsset, treeview, treeviewGUI)
		{
			this.m_Styles = DirectorStyles.Instance;
			this.m_IsRoot = isRoot;
			string assetPath = AssetDatabase.GetAssetPath(trackAsset);
			string assetPath2 = AssetDatabase.GetAssetPath(treeviewGUI.TimelineWindow.timeline);
			if (assetPath != assetPath2)
			{
				this.m_IsReferencedTrack = true;
			}
			this.m_GroupDepth = 0;
			this.CalculateGroupDepth(parent, ref this.m_GroupDepth);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00024880 File Offset: 0x00022C80
		public override Rect boundingRect
		{
			get
			{
				return this.m_TrackRect;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0002489C File Offset: 0x00022C9C
		public override Rect headerBounds
		{
			get
			{
				this.m_HeaderBounds.x = Mathf.Max(0f, this.boundingRect.x - TimelineWindow.instance.state.sequencerHeaderWidth);
				this.m_HeaderBounds.y = this.boundingRect.y;
				this.m_HeaderBounds.width = TimelineWindow.instance.state.sequencerHeaderWidth;
				this.m_HeaderBounds.height = this.boundingRect.height;
				return this.m_HeaderBounds;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00024938 File Offset: 0x00022D38
		public virtual Rect indentedHeaderBounds
		{
			get
			{
				float num = (float)this.depth * DirectorStyles.Instance.indentWidth + 2f;
				float num2 = (float)DirectorStyles.Instance.foldout.normal.background.width + 4f;
				float num3 = num + num2;
				Rect headerRect = this.m_HeaderRect;
				headerRect.width = this.m_HeaderRect.width - num3;
				headerRect.x += num3;
				return headerRect;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000249B8 File Offset: 0x00022DB8
		public static void Create(TrackAsset parent, string title)
		{
			if (parent != null)
			{
				parent.collapsed = false;
			}
			TimelineWindow.instance.AddTrack(TimelineHelpers.GroupTrackType, parent, title);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000249E0 File Offset: 0x00022DE0
		public override bool IsMouseOver(Vector2 mousePosition)
		{
			return this.headerBounds.Contains(mousePosition);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00024A04 File Offset: 0x00022E04
		public override float GetHeight(TimelineWindow.TimelineState state)
		{
			return state.trackHeight;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00024A1F File Offset: 0x00022E1F
		public override void SetHeight(float height)
		{
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00024A24 File Offset: 0x00022E24
		protected override void OnSelectedChanged(bool value)
		{
			if (this.children != null)
			{
				if (value)
				{
					TimelineWindow.TimelineState state = TimelineWindow.instance.state;
					state.selection.SelectInEditor(base.track);
				}
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00024A66 File Offset: 0x00022E66
		protected override void OnLockedChanged(bool value)
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00024A69 File Offset: 0x00022E69
		public override void OnGraphRebuilt()
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00024A6C File Offset: 0x00022E6C
		private void CalculateGroupDepth(TreeViewItem parent, ref int depth)
		{
			if (parent != null)
			{
				TimelineGroupGUI timelineGroupGUI = parent as TimelineGroupGUI;
				if (timelineGroupGUI != null && !(timelineGroupGUI.track == null))
				{
					if (timelineGroupGUI.track.mediaType == 5)
					{
						depth++;
					}
					this.CalculateGroupDepth(parent.parent, ref depth);
				}
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00024ACC File Offset: 0x00022ECC
		protected override bool DetectProblems(TimelineWindow.TimelineState state)
		{
			return false;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00024AE4 File Offset: 0x00022EE4
		public override bool CanBeSelected(Vector2 mousePosition)
		{
			return this.m_HeaderBounds.Contains(mousePosition);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00024B08 File Offset: 0x00022F08
		public override void Draw(Rect headerRect, Rect trackRect, TimelineWindow.TimelineState state, float identWidth)
		{
			if (!(base.track == null))
			{
				if (!this.m_IsRoot)
				{
					if (this.depth == 1)
					{
						EditorGUI.DrawRect(headerRect, DirectorStyles.Instance.customSkin.colorSequenceBackground);
					}
					Rect rect = headerRect;
					rect.x = headerRect.xMax - 20f;
					rect.y = headerRect.y;
					rect.width = 20f;
					trackRect.width += state.bindingAreaWidth;
					float num = 0f;
					base.GetChildrenHeight(ref num, this);
					Rect rect2 = RectUtils.Encompass(headerRect, trackRect);
					if (base.isExpanded && this.children != null && this.children.Count > 1)
					{
						rect2.height += num;
					}
					rect2.x = identWidth;
					Color color = DirectorStyles.Instance.customSkin.colorGroup;
					if (base.track.mediaType != 5)
					{
						if (base.track.mediaType == null)
						{
							color = DirectorStyles.Instance.customSkin.colorAnimation;
						}
						if (base.track.mediaType == 1)
						{
							color = DirectorStyles.Instance.customSkin.colorAudio;
						}
						if (base.track.mediaType == 2)
						{
							color = DirectorStyles.Instance.customSkin.colorVideo;
						}
						if (base.track.mediaType == 3)
						{
							color = DirectorStyles.Instance.customSkin.colorScripting;
						}
					}
					this.m_TrackRect = trackRect;
					this.m_HeaderRect = headerRect;
					Color color2 = color;
					float a = color.a;
					float num2;
					float num3;
					float num4;
					Color.RGBToHSV(color2, ref num2, ref num3, ref num4);
					num4 -= 0.02f * (float)this.m_GroupDepth;
					color2 = Color.HSVToRGB(num2, num3, num4);
					if (this.selected)
					{
						num4 += 0.1f * (float)this.depth;
						color2 = Color.HSVToRGB(num2, num3, num4);
					}
					if (base.isDropTarget)
					{
						color2 = DirectorStyles.Instance.customSkin.colorDropTarget;
					}
					color2.a = a;
					using (new GUIColorOverride(color2))
					{
						GUI.Box(rect2, GUIContent.none, this.m_Styles.sequenceGroupBackground);
					}
					bool flag = base.track.collapsed != !base.isExpanded;
					base.track.collapsed = !base.isExpanded;
					if (this.m_MustRecomputeUnions || (flag && base.track.collapsed))
					{
						this.RecomputeRectUnions(state);
					}
					if (!base.isExpanded && this.children != null && this.children.Count > 0)
					{
						Rect parentRect = trackRect;
						foreach (TimelineClipUnion timelineClipUnion in this.m_Unions)
						{
							timelineClipUnion.Draw(parentRect, state);
						}
					}
					if (base.track.locked)
					{
						GUI.Button(trackRect, "Locked");
					}
					Rect rect3 = headerRect;
					rect3.xMin += identWidth + 20f;
					string text = (!(base.track != null)) ? "missing" : base.track.name;
					rect3.width = this.m_Styles.sequenceGroupFont.CalcSize(new GUIContent(text)).x;
					rect3.width = Math.Max(rect3.width, 50f);
					if (base.track != null && base.track is GroupTrack)
					{
						Color newColor = this.m_Styles.sequenceGroupFont.normal.textColor;
						if (this.selected)
						{
							newColor = Color.white;
						}
						string text2 = base.track.name;
						EditorGUI.BeginChangeCheck();
						using (new StyleNormalColorOverride(this.m_Styles.sequenceGroupFont, newColor))
						{
							text2 = EditorGUI.DelayedTextField(rect3, GUIContent.none, base.track.GetInstanceID(), base.track.name, this.m_Styles.sequenceGroupFont);
						}
						if (EditorGUI.EndChangeCheck())
						{
							base.track.name = text2;
							if (text2.Length == 0)
							{
								base.track.name = TimelineHelpers.GenerateUniqueActorName(state.timeline, "unnamed");
							}
							this.displayName = base.track.name;
						}
					}
					using (new StyleNormalColorOverride(this.m_Styles.sequenceTrackHeaderFont, Color.white))
					{
						if (GUI.Button(rect, "+", this.m_Styles.sequenceTrackHeaderFont))
						{
							this.OnAddTrackClicked(state);
						}
					}
					if (this.IsTrackRecording(state))
					{
						using (new GUIColorOverride(DirectorStyles.Instance.customSkin.colorTrackBackgroundRecording))
						{
							GUI.Label(rect2, GUIContent.none, this.m_Styles.sequenceClip);
						}
					}
					if (Event.current.type == 7)
					{
						base.isDropTarget = false;
					}
					if (this.m_IsReferencedTrack)
					{
						Rect rect4 = trackRect;
						rect4.x = state.timeAreaRect.xMax - 20f;
						rect4.y += 5f;
						rect4.width = 30f;
						GUI.Label(rect4, this.m_Styles.referenceTrackLabel, EditorStyles.label);
					}
				}
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00025128 File Offset: 0x00023528
		private void OnAddTrackClicked(TimelineWindow.TimelineState state)
		{
			state.GetWindow().ShowNewTracksContextMenu(state, base.track, this);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00025140 File Offset: 0x00023540
		protected bool isSubTrack()
		{
			return !(base.track == null) && !(base.track.parent == null) && base.track.parent is TrackAsset && ((TrackAsset)base.track.parent).mediaType != 5;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000251C0 File Offset: 0x000235C0
		protected TrackAsset parentTrack()
		{
			TrackAsset result;
			if (this.isSubTrack())
			{
				result = (base.track.parent as TrackAsset);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000251F8 File Offset: 0x000235F8
		private bool IsTrackRecording(TimelineWindow.TimelineState state)
		{
			return state.recording && base.track.mediaType == 5 && state.GetArmedTrack(base.track) != null;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0002524C File Offset: 0x0002364C
		private void RecomputeRectUnions(TimelineWindow.TimelineState state)
		{
			this.m_MustRecomputeUnions = false;
			this.m_Unions.Clear();
			if (this.children != null)
			{
				foreach (TreeViewItem treeViewItem in this.children)
				{
					TimelineTrackGUI timelineTrackGUI = treeViewItem as TimelineTrackGUI;
					if (timelineTrackGUI != null)
					{
						timelineTrackGUI.RebuildGUICache(state);
						this.m_Unions.AddRange(TimelineClipUnion.Build(timelineTrackGUI.clips));
					}
				}
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000252F4 File Offset: 0x000236F4
		public static void AddMenuItems(GenericMenu menu, GroupTrack track)
		{
			TimelineWindow.TimelineState state = TimelineWindow.instance.state;
			TrackType[] array = TimelineHelpers.GetMixableTypes();
			array = (from x in array
			orderby (!x.m_TrackType.Assembly.FullName.Contains("UnityEngine.Sequence")) ? 1 : 0
			select x).ToArray<TrackType>();
			foreach (TrackType trackType in array)
			{
				if (trackType.m_TrackType != typeof(GroupTrack))
				{
					GenericMenu.MenuFunction2 menuFunction = delegate(object e)
					{
						track.collapsed = false;
						state.GetWindow().AddTrack(e as TrackType, track);
						TimelineTrackBaseGUI timelineTrackBaseGUI = TimelineTrackBaseGUI.FindGUITrack(track);
						if (timelineTrackBaseGUI != null)
						{
							TimelineWindow.instance.treeView.data.SetExpanded(timelineTrackBaseGUI, true);
						}
					};
					object obj = trackType;
					string text = TimelineHelpers.GetTrackCategoryName(trackType);
					if (!string.IsNullOrEmpty(text))
					{
						text += "/";
					}
					menu.AddItem(new GUIContent("Add " + text + TimelineHelpers.GetTrackMenuName(trackType)), false, menuFunction, obj);
				}
			}
		}

		// Token: 0x040002E4 RID: 740
		protected DirectorStyles m_Styles;

		// Token: 0x040002E5 RID: 741
		protected bool m_MustRecomputeUnions = true;

		// Token: 0x040002E6 RID: 742
		protected int m_GroupDepth;

		// Token: 0x040002E7 RID: 743
		protected Rect m_TrackRect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x040002E8 RID: 744
		protected GUIContent m_ProblemIcon = null;

		// Token: 0x040002E9 RID: 745
		private Rect m_HeaderRect;

		// Token: 0x040002EA RID: 746
		private readonly bool m_IsReferencedTrack;

		// Token: 0x040002EB RID: 747
		private readonly List<TimelineClipUnion> m_Unions = new List<TimelineClipUnion>();

		// Token: 0x040002EC RID: 748
		private Rect m_HeaderBounds = new Rect(0f, 0f, 1f, 1f);
	}
}
