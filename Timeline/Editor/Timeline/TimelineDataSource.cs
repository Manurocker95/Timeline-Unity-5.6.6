using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000086 RID: 134
	internal class TimelineDataSource : TreeViewDataSource
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x00021C4F File Offset: 0x0002004F
		public TimelineDataSource(TimelineTreeViewGUI parentGUI, TreeViewController treeView, TimelineWindow sequencerWindow) : base(treeView)
		{
			this.m_TimelineWindow = sequencerWindow;
			this.m_ParentGUI = parentGUI;
			this.FetchData();
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00021C70 File Offset: 0x00020070
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x00021C8A File Offset: 0x0002008A
		public List<TimelineTrackBaseGUI> allTrackGuis { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00021C94 File Offset: 0x00020094
		public TreeViewItem treeroot
		{
			get
			{
				return this.m_RootItem;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00021CB0 File Offset: 0x000200B0
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00021CCA File Offset: 0x000200CA
		public int itemCounter { get; private set; }

		// Token: 0x0600045D RID: 1117 RVA: 0x00021CD4 File Offset: 0x000200D4
		public override void FetchData()
		{
			this.itemCounter = 1;
			this.m_RootItem = new TimelineGroupGUI(this.m_TreeView, this.m_ParentGUI, 1, 0, null, "root", null, true);
			Dictionary<TrackAsset, TimelineGroupGUI> dictionary = new Dictionary<TrackAsset, TimelineGroupGUI>();
			List<TrackAsset> list = this.m_TimelineWindow.timeline.tracks;
			this.allTrackGuis = new List<TimelineTrackBaseGUI>(this.m_TimelineWindow.timeline.tracks.Count);
			if (this.m_ParentGUI.TimelineWindow.state.searchFilter.Length > 0)
			{
				Regex searchPattern = new Regex(this.m_ParentGUI.TimelineWindow.state.searchFilter, RegexOptions.IgnoreCase);
				list = this.m_TimelineWindow.timeline.flattenedTracks;
				list = (from a in list
				where searchPattern.IsMatch(a.name)
				select a).ToList<TrackAsset>();
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateItem(list[i], ref dictionary, list, this.m_RootItem);
			}
			int num = this.m_TimelineWindow.timeline.GetInstanceID() + 1000;
			this.InsertGroupSpacers(this.m_RootItem, ref num);
			this.m_NeedRefreshRows = true;
			this.SetExpanded(this.m_RootItem, true);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00021E20 File Offset: 0x00020220
		private void InsertGroupSpacers(TreeViewItem item, ref int spacerId)
		{
			TimelineGroupGUI timelineGroupGUI = item as TimelineGroupGUI;
			if (timelineGroupGUI != null)
			{
				bool flag = false;
				if (timelineGroupGUI.track != null && timelineGroupGUI.track.mediaType == 5 && item != this.m_RootItem)
				{
					flag = true;
				}
				else if (timelineGroupGUI.track != null && timelineGroupGUI.track.mediaType != 5 && item != this.m_RootItem)
				{
					if (timelineGroupGUI.track.parent == null)
					{
						flag = true;
					}
				}
				if (flag)
				{
					spacerId++;
					TimelineSpacerGui item2 = new TimelineSpacerGui(spacerId, timelineGroupGUI.depth + 1, item);
					if (item.children == null)
					{
						item.children = new List<TreeViewItem>();
					}
					item.children.Add(item2);
				}
			}
			if (item.children != null && item.children.Count != 0)
			{
				foreach (TreeViewItem item3 in item.children)
				{
					this.InsertGroupSpacers(item3, ref spacerId);
				}
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00021F74 File Offset: 0x00020374
		private TimelineGroupGUI CreateItem(TrackAsset a, ref Dictionary<TrackAsset, TimelineGroupGUI> tree, List<TrackAsset> selectedRows, TreeViewItem parentTreeViewItem)
		{
			TimelineGroupGUI result;
			if (a == null)
			{
				result = null;
			}
			else
			{
				if (tree == null)
				{
					throw new ArgumentNullException("tree");
				}
				if (selectedRows == null)
				{
					throw new ArgumentNullException("selectedRows");
				}
				if (tree.ContainsKey(a))
				{
					result = tree[a];
				}
				else
				{
					TimelineTrackBaseGUI timelineTrackBaseGUI = parentTreeViewItem as TimelineTrackBaseGUI;
					if (selectedRows.Contains(a.parent as TrackAsset))
					{
						timelineTrackBaseGUI = this.CreateItem(a.parent as TrackAsset, ref tree, selectedRows, parentTreeViewItem);
					}
					int num = -1;
					if (timelineTrackBaseGUI != null)
					{
						num = timelineTrackBaseGUI.depth;
					}
					num++;
					TimelineGroupGUI timelineGroupGUI;
					if (a.GetType() != TimelineHelpers.GroupTrackType.m_TrackType)
					{
						timelineGroupGUI = new TimelineTrackGUI(this.m_TreeView, this.m_ParentGUI, a.GetInstanceID(), num, timelineTrackBaseGUI, a.name, a);
					}
					else
					{
						timelineGroupGUI = new TimelineGroupGUI(this.m_TreeView, this.m_ParentGUI, a.GetInstanceID(), num, timelineTrackBaseGUI, a.name, a, false);
					}
					this.allTrackGuis.Add(timelineGroupGUI);
					if (timelineTrackBaseGUI != null)
					{
						if (timelineTrackBaseGUI.children == null)
						{
							timelineTrackBaseGUI.children = new List<TreeViewItem>();
						}
						timelineTrackBaseGUI.children.Add(timelineGroupGUI);
					}
					else
					{
						this.m_RootItem = timelineGroupGUI;
						this.SetExpanded(this.m_RootItem, true);
					}
					tree[a] = timelineGroupGUI;
					AnimationTrack animationTrack = timelineGroupGUI.track as AnimationTrack;
					bool flag = animationTrack != null && animationTrack.ShouldShowInfiniteClipEditor();
					if (flag)
					{
						if (timelineGroupGUI.children == null)
						{
							timelineGroupGUI.children = new List<TreeViewItem>();
						}
					}
					else
					{
						bool flag2 = false;
						for (int num2 = 0; num2 != timelineGroupGUI.track.clips.Length; num2++)
						{
							AnimationClip animationClip = timelineGroupGUI.track.clips[num2].curves;
							AnimationClip animationClip2 = timelineGroupGUI.track.clips[num2].animationClip;
							if (animationClip != null && animationClip.empty)
							{
								animationClip = null;
							}
							if (animationClip2 != null && animationClip2.empty)
							{
								animationClip2 = null;
							}
							if (animationClip2 != null && (animationClip2.hideFlags & 8) != null)
							{
								animationClip2 = null;
							}
							if (!timelineGroupGUI.track.clips[num2].recordable)
							{
								animationClip2 = null;
							}
							flag2 = (animationClip != null || animationClip2 != null);
							if (flag2)
							{
								break;
							}
						}
						if (flag2)
						{
							if (timelineGroupGUI.children == null)
							{
								timelineGroupGUI.children = new List<TreeViewItem>();
							}
						}
					}
					if (a.subTracks != null)
					{
						for (int i = 0; i < a.subTracks.Count; i++)
						{
							this.CreateItem(a.subTracks[i], ref tree, selectedRows, timelineGroupGUI);
						}
					}
					result = timelineGroupGUI;
				}
			}
			return result;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0002227C File Offset: 0x0002067C
		public override bool CanBeParent(TreeViewItem item)
		{
			TimelineTrackGUI timelineTrackGUI = item as TimelineTrackGUI;
			return timelineTrackGUI == null;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000222A8 File Offset: 0x000206A8
		public void ExpandItems(TreeViewItem item)
		{
			if (this.treeroot == item)
			{
				this.SetExpanded(this.treeroot, true);
			}
			TimelineGroupGUI timelineGroupGUI = item as TimelineGroupGUI;
			if (timelineGroupGUI != null && timelineGroupGUI.track != null)
			{
				this.SetExpanded(item, !timelineGroupGUI.track.collapsed);
			}
			if (item.children != null)
			{
				for (int i = 0; i < item.children.Count; i++)
				{
					this.ExpandItems(item.children[i]);
				}
			}
		}

		// Token: 0x040002D6 RID: 726
		private readonly TimelineWindow m_TimelineWindow;

		// Token: 0x040002D7 RID: 727
		private readonly TimelineTreeViewGUI m_ParentGUI;
	}
}
