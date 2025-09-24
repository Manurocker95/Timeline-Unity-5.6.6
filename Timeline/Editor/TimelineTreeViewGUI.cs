using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor
{
	// Token: 0x0200008E RID: 142
	internal class TimelineTreeViewGUI
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x00028790 File Offset: 0x00026B90
		public TimelineTreeViewGUI(TimelineWindow sequencerWindow, TimelineAsset timeline, Rect rect)
		{
			this.m_Timeline = timeline;
			this.m_Window = sequencerWindow;
			TreeViewState treeViewState = new TreeViewState();
			this.m_TreeView = new TreeViewController(sequencerWindow, treeViewState);
			this.m_TreeView.horizontalScrollbarStyle = GUIStyle.none;
			this.m_SequenceTreeView = new SequenceTreeView(sequencerWindow, this.m_TreeView);
			TimelineDragging timelineDragging = new TimelineDragging(this.m_TreeView, this.m_Window, this.m_Timeline);
			this.m_DataSource = new TimelineDataSource(this, this.m_TreeView, sequencerWindow);
			TimelineDataSource dataSource = this.m_DataSource;
			dataSource.onVisibleRowsChanged = (Action)Delegate.Combine(dataSource.onVisibleRowsChanged, new Action(this.m_SequenceTreeView.CalculateRowRects));
			this.m_TreeView.Init(rect, this.m_DataSource, this.m_SequenceTreeView, timelineDragging);
			TreeViewController treeView = this.m_TreeView;
			treeView.dragEndedCallback = (Action<int[], bool>)Delegate.Combine(treeView.dragEndedCallback, new Action<int[], bool>(delegate(int[] ids, bool value)
			{
				this.state.selection.Clear();
			}));
			this.m_DataSource.ExpandItems(this.m_DataSource.root);
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00028894 File Offset: 0x00026C94
		public TreeViewItem root
		{
			get
			{
				return this.m_DataSource.root;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000288B4 File Offset: 0x00026CB4
		public TrackAsset[] visibleTracks
		{
			get
			{
				int num = -1;
				int num2 = -1;
				List<TrackAsset> list = new List<TrackAsset>();
				this.m_TreeView.gui.GetFirstAndLastRowVisible(ref num, ref num2);
				for (int i = num; i <= num2; i++)
				{
					TimelineTrackGUI timelineTrackGUI = this.m_TreeView.data.GetItem(i) as TimelineTrackGUI;
					if (timelineTrackGUI != null)
					{
						this.AddVisibleTrackRecursive(ref list, timelineTrackGUI.track);
					}
				}
				return list.ToArray();
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00028934 File Offset: 0x00026D34
		public List<TimelineClipGUI> allClipGuis
		{
			get
			{
				TimelineDataSource timelineDataSource = this.m_TreeView.data as TimelineDataSource;
				List<TimelineClipGUI> result;
				if (timelineDataSource != null && timelineDataSource.allTrackGuis != null)
				{
					result = (from x in timelineDataSource.allTrackGuis.OfType<TimelineTrackGUI>()
					where x != null
					select x).SelectMany((TimelineTrackGUI x) => x.clips).ToList<TimelineClipGUI>();
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x000289C8 File Offset: 0x00026DC8
		public List<TimelineTrackBaseGUI> allTrackGuis
		{
			get
			{
				TimelineDataSource timelineDataSource = this.m_TreeView.data as TimelineDataSource;
				List<TimelineTrackBaseGUI> result;
				if (timelineDataSource != null)
				{
					result = timelineDataSource.allTrackGuis;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00028A04 File Offset: 0x00026E04
		public Vector2 contentSize
		{
			get
			{
				return this.m_TreeView.GetContentSize();
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00028A24 File Offset: 0x00026E24
		// (set) Token: 0x06000558 RID: 1368 RVA: 0x00028A4C File Offset: 0x00026E4C
		public Vector2 scrollPosition
		{
			get
			{
				return this.m_TreeView.state.scrollPos;
			}
			set
			{
				Rect totalRect = this.m_TreeView.GetTotalRect();
				Vector2 contentSize = this.m_TreeView.GetContentSize();
				this.m_TreeView.state.scrollPos = new Vector2(value.x, Mathf.Min(new float[]
				{
					Mathf.Clamp(value.y, 0f, contentSize.y - totalRect.height)
				}));
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00028AC0 File Offset: 0x00026EC0
		private TimelineWindow.TimelineState state
		{
			get
			{
				return this.m_Window.state;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00028AE0 File Offset: 0x00026EE0
		public ITreeViewGUI gui
		{
			get
			{
				return this.m_SequenceTreeView;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00028AFC File Offset: 0x00026EFC
		public ITreeViewDataSource data
		{
			get
			{
				return (this.m_TreeView != null) ? this.m_TreeView.data : null;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00028B30 File Offset: 0x00026F30
		public TimelineWindow TimelineWindow
		{
			get
			{
				return this.m_Window;
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00028B4B File Offset: 0x00026F4B
		public void CalculateRowRects()
		{
			this.m_SequenceTreeView.CalculateRowRects();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00028B59 File Offset: 0x00026F59
		public void Reload()
		{
			this.m_TreeView.ReloadData();
			this.m_DataSource.ExpandItems(this.m_DataSource.root);
			this.m_SequenceTreeView.CalculateRowRects();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00028B88 File Offset: 0x00026F88
		public void OnGUI(Rect rect)
		{
			int controlID = GUIUtility.GetControlID(1, rect);
			this.m_Window.state.keyboardControl = controlID;
			this.m_TreeView.OnGUI(rect, controlID);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00028BBC File Offset: 0x00026FBC
		public float GetRowHeightWithPadding(TreeViewItem i)
		{
			return this.m_SequenceTreeView.GetSizeOfRow(i).y;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00028BE8 File Offset: 0x00026FE8
		public Rect GetRowRect(int row)
		{
			return this.m_SequenceTreeView.GetRowRect(row);
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00028C0C File Offset: 0x0002700C
		public List<TrackAsset> selection
		{
			get
			{
				List<TrackAsset> list = new List<TrackAsset>();
				int[] selection = this.m_TreeView.GetSelection();
				TrackAsset[] array = this.m_Timeline.flattenedTracks.ToArray();
				foreach (int num in selection)
				{
					foreach (TrackAsset trackAsset in array)
					{
						if (trackAsset.GetInstanceID() == num)
						{
							list.Add(trackAsset);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00028CA8 File Offset: 0x000270A8
		private void AddVisibleTrackRecursive(ref List<TrackAsset> list, TrackAsset track)
		{
			list.Add(track);
			if (!track.collapsed)
			{
				if (track.subTracks != null)
				{
					foreach (TrackAsset track2 in track.subTracks)
					{
						this.AddVisibleTrackRecursive(ref list, track2);
					}
				}
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00028D30 File Offset: 0x00027130
		public void SetSelection(int[] selectedIDs, bool revealSelectionAndFrameLastSelected)
		{
			if (this.m_TreeView != null)
			{
				this.m_TreeView.SetSelection(selectedIDs, revealSelectionAndFrameLastSelected);
			}
		}

		// Token: 0x0400034E RID: 846
		private readonly TimelineAsset m_Timeline;

		// Token: 0x0400034F RID: 847
		private readonly TreeViewController m_TreeView;

		// Token: 0x04000350 RID: 848
		private readonly SequenceTreeView m_SequenceTreeView;

		// Token: 0x04000351 RID: 849
		private readonly TimelineWindow m_Window;

		// Token: 0x04000352 RID: 850
		private readonly TimelineDataSource m_DataSource;
	}
}
