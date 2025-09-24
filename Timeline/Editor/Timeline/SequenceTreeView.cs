using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200008D RID: 141
	internal class SequenceTreeView : ITreeViewGUI
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x00027C60 File Offset: 0x00026060
		public SequenceTreeView(TimelineWindow sequencerWindow, TreeViewController treeView)
		{
			this.m_TreeView = treeView;
			this.m_TreeView.useExpansionAnimation = true;
			SequenceTreeView.m_Styles = DirectorStyles.Instance;
			this.m_State = sequencerWindow.state;
			this.m_FoldoutWidth = DirectorStyles.Instance.foldout.fixedWidth;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00027CC0 File Offset: 0x000260C0
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00027CDA File Offset: 0x000260DA
		public bool showInsertionMarker { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00027CE4 File Offset: 0x000260E4
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00027CFE File Offset: 0x000260FE
		public virtual float topRowMargin { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00027D08 File Offset: 0x00026108
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00027D22 File Offset: 0x00026122
		public virtual float bottomRowMargin { get; private set; }

		// Token: 0x06000537 RID: 1335 RVA: 0x00027D2B File Offset: 0x0002612B
		public void OnInitialize()
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00027D30 File Offset: 0x00026130
		public Rect GetRectForFraming(int row)
		{
			return this.GetRowRect(row, 1f);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00027D54 File Offset: 0x00026154
		public virtual Vector2 GetSizeOfRow(TreeViewItem item)
		{
			Vector2 result;
			if (item.displayName == "root")
			{
				result = new Vector2(this.m_TreeView.GetTotalRect().width, 0f);
			}
			else
			{
				TimelineGroupGUI timelineGroupGUI = item as TimelineGroupGUI;
				if (timelineGroupGUI != null)
				{
					result = new Vector2(this.m_TreeView.GetTotalRect().width, timelineGroupGUI.GetHeight(this.m_State));
				}
				else if (item is TimelineSpacerGui)
				{
					result = new Vector2(this.m_TreeView.GetTotalRect().width, 0f);
				}
				else
				{
					float num = this.m_State.trackHeight;
					if (item.hasChildren && this.m_TreeView.data.IsExpanded(item))
					{
						num = Mathf.Min(this.m_State.trackHeight, SequenceTreeView.kMinTrackHeight);
					}
					if (timelineGroupGUI.track.mediaType == 5 || (!(timelineGroupGUI is TimelineTrackGUI) && timelineGroupGUI.track.mediaType != 5))
					{
						num = SequenceTreeView.kMinTrackHeight;
					}
					result = new Vector2(this.m_TreeView.GetTotalRect().width, num);
				}
			}
			return result;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00027EA4 File Offset: 0x000262A4
		public virtual void BeginRowGUI()
		{
			if (this.m_TreeView.GetTotalRect().width != this.GetRowRect(0).width)
			{
				this.CalculateRowRects();
			}
			this.m_DraggingInsertionMarkerRect.x = -1f;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00027EF4 File Offset: 0x000262F4
		public virtual void EndRowGUI()
		{
			if (this.m_DraggingInsertionMarkerRect.x >= 0f && Event.current.type == 7)
			{
				Rect draggingInsertionMarkerRect = this.m_DraggingInsertionMarkerRect;
				draggingInsertionMarkerRect.height = 2f;
				draggingInsertionMarkerRect.y -= draggingInsertionMarkerRect.height / 2f;
				if (!this.m_TreeView.dragging.drawRowMarkerAbove)
				{
					draggingInsertionMarkerRect.y += this.m_DraggingInsertionMarkerRect.height;
				}
				EditorGUI.DrawRect(draggingInsertionMarkerRect, Color.white);
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00027F90 File Offset: 0x00026390
		private void EncompassChildrenRowRect(TreeViewItem item, ref Rect rowRect)
		{
			if (item.children != null)
			{
				foreach (TreeViewItem item2 in item.children)
				{
					Vector2 sizeOfRow = this.GetSizeOfRow(item2);
					rowRect.height += sizeOfRow.y;
				}
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00028014 File Offset: 0x00026414
		public virtual void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
		{
			Rect headerRect = rowRect;
			Rect trackRect = rowRect;
			headerRect.width = this.m_State.sequencerHeaderWidth;
			trackRect.xMin += this.m_State.sequencerHeaderWidth;
			trackRect.width = rowRect.width - this.m_State.sequencerHeaderWidth - 1f;
			float foldoutIndent = this.GetFoldoutIndent(item);
			Rect rect = rowRect;
			int itemControlID = TreeViewController.GetItemControlID(item);
			TimelineTrackBaseGUI timelineTrackBaseGUI = item as TimelineTrackBaseGUI;
			TimelineSpacerGui timelineSpacerGui = item as TimelineSpacerGui;
			if (timelineSpacerGui == null)
			{
				bool flag = this.m_TreeView.data.IsExpandable(item) && timelineTrackBaseGUI != null && timelineTrackBaseGUI.track != null;
				if (flag)
				{
					if (item.children != null)
					{
						if (!item.children.Any((TreeViewItem i) => !(i is TimelineSpacerGui)))
						{
							flag = false;
						}
					}
				}
				timelineTrackBaseGUI.isExpanded = this.m_TreeView.data.IsExpanded(item);
				timelineTrackBaseGUI.Draw(headerRect, trackRect, this.m_State, foldoutIndent);
				if (trackRect.Contains(Event.current.mousePosition) || Event.current.isKey)
				{
					timelineTrackBaseGUI.OnEvent(Event.current, this.m_State, false);
				}
				if (Event.current.type == 7)
				{
					if (this.showInsertionMarker && this.m_TreeView.dragging != null && this.m_TreeView.dragging.GetRowMarkerControlID() == itemControlID)
					{
						this.m_DraggingInsertionMarkerRect = new Rect(rowRect.x + foldoutIndent, rowRect.y, rowRect.width - foldoutIndent, rowRect.height);
					}
				}
				if (flag)
				{
					rect.x = foldoutIndent - SequenceTreeView.kFoldOutOffset;
					rect.width = this.m_FoldoutWidth;
					EditorGUI.BeginChangeCheck();
					float num = (float)DirectorStyles.Instance.foldout.normal.background.height;
					rect.y += num / 2f;
					rect.height = num;
					bool flag2 = GUI.Toggle(rect, this.m_TreeView.data.IsExpanded(item), GUIContent.none, SequenceTreeView.m_Styles.foldout);
					if (EditorGUI.EndChangeCheck())
					{
						if (Event.current.alt)
						{
							this.m_TreeView.data.SetExpandedWithChildren(item, flag2);
						}
						else
						{
							this.m_TreeView.data.SetExpanded(item, flag2);
						}
					}
				}
				if (headerRect.Contains(Event.current.mousePosition) || Event.current.isKey)
				{
					timelineTrackBaseGUI.OnEvent(Event.current, this.m_State, false);
				}
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000282F4 File Offset: 0x000266F4
		public Rect GetRenameRect(Rect rowRect, int row, TreeViewItem item)
		{
			return rowRect;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0002830A File Offset: 0x0002670A
		public void BeginPingItem(TreeViewItem item, float topPixelOfRow, float availableWidth)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0002830D File Offset: 0x0002670D
		public void EndPingItem()
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00028310 File Offset: 0x00026710
		public Rect GetRowRect(int row, float rowWidth)
		{
			return this.GetRowRect(row);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0002832C File Offset: 0x0002672C
		public Rect GetRowRect(int row)
		{
			Rect result;
			if (this.m_RowRects.Count == 0)
			{
				result = default(Rect);
			}
			else if (row >= this.m_RowRects.Count)
			{
				result = default(Rect);
			}
			else
			{
				result = this.m_RowRects[row];
			}
			return result;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0002838C File Offset: 0x0002678C
		private float GetSpacing(TreeViewItem item)
		{
			TimelineTrackBaseGUI timelineTrackBaseGUI = item.parent as TimelineTrackBaseGUI;
			if (timelineTrackBaseGUI != null && !timelineTrackBaseGUI.isRoot)
			{
				if (timelineTrackBaseGUI.track is AnimationTrack)
				{
					return 1f;
				}
			}
			return 3f;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000283E0 File Offset: 0x000267E0
		public void CalculateRowRects()
		{
			if (!this.m_TreeView.isSearching)
			{
				IList<TreeViewItem> rows = this.m_TreeView.data.GetRows();
				this.m_RowRects = new List<Rect>(rows.Count);
				float num = 6f;
				this.m_MaxWidthOfRows = 1f;
				for (int i = 0; i < rows.Count; i++)
				{
					TreeViewItem item = rows[i];
					if (i != 0)
					{
						num += this.GetSpacing(item);
					}
					Vector2 sizeOfRow = this.GetSizeOfRow(item);
					this.m_RowRects.Add(new Rect(0f, num, sizeOfRow.x, sizeOfRow.y));
					num += sizeOfRow.y;
					if (sizeOfRow.x > this.m_MaxWidthOfRows)
					{
						this.m_MaxWidthOfRows = sizeOfRow.x;
					}
				}
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000284BD File Offset: 0x000268BD
		public virtual void BeginPingNode(TreeViewItem item, float topPixelOfRow, float availableWidth)
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000284C0 File Offset: 0x000268C0
		public virtual void EndPingNode()
		{
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000284C4 File Offset: 0x000268C4
		public virtual bool BeginRename(TreeViewItem item, float delay)
		{
			return false;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000284DA File Offset: 0x000268DA
		public virtual void EndRename()
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000284E0 File Offset: 0x000268E0
		public virtual float GetFoldoutIndent(TreeViewItem item)
		{
			float result;
			if (item.depth <= 1 || this.m_TreeView.isSearching)
			{
				result = TimelineWindowStyles.kBaseIndent;
			}
			else
			{
				result = (float)item.depth * TimelineWindowStyles.kBaseIndent;
			}
			return result;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0002852C File Offset: 0x0002692C
		public virtual float GetContentIndent(TreeViewItem item)
		{
			return this.GetFoldoutIndent(item);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00028548 File Offset: 0x00026948
		public int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
		{
			Debug.LogError("GetNumRowsOnPageUpDown: Not impemented");
			return (int)Mathf.Floor(heightOfTreeView / 30f);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00028574 File Offset: 0x00026974
		public void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
		{
			int rowCount = this.m_TreeView.data.rowCount;
			if (rowCount == 0)
			{
				firstRowVisible = (lastRowVisible = -1);
			}
			else
			{
				if (rowCount != this.m_RowRects.Count)
				{
					Debug.LogError("Mismatch in state: rows vs cached rects. Did you remember to hook up: dataSource.onVisibleRowsChanged += gui.CalculateRowRects ?");
					this.CalculateRowRects();
				}
				float y = this.m_TreeView.state.scrollPos.y;
				float height = this.m_TreeView.GetTotalRect().height;
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < this.m_RowRects.Count; i++)
				{
					bool flag = (this.m_RowRects[i].y > y && this.m_RowRects[i].y < y + height) || (this.m_RowRects[i].yMax > y && this.m_RowRects[i].yMax < y + height);
					if (flag)
					{
						if (num == -1)
						{
							num = i;
						}
						num2 = i;
					}
				}
				if (num != -1 && num2 != -1)
				{
					firstRowVisible = num;
					lastRowVisible = num2;
				}
				else
				{
					firstRowVisible = 0;
					lastRowVisible = rowCount - 1;
				}
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000286D8 File Offset: 0x00026AD8
		public Vector2 GetTotalSize()
		{
			Vector2 result;
			if (this.m_RowRects.Count == 0)
			{
				result = new Vector2(0f, 0f);
			}
			else
			{
				result = new Vector2(this.m_MaxWidthOfRows, this.m_RowRects[this.m_RowRects.Count - 1].yMax);
			}
			return result;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00028740 File Offset: 0x00026B40
		public virtual float halfDropBetweenHeight
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x04000341 RID: 833
		protected float m_FoldoutWidth;

		// Token: 0x04000342 RID: 834
		protected Rect m_DraggingInsertionMarkerRect;

		// Token: 0x04000343 RID: 835
		protected readonly TreeViewController m_TreeView;

		// Token: 0x04000344 RID: 836
		private List<Rect> m_RowRects = new List<Rect>();

		// Token: 0x04000345 RID: 837
		private float m_MaxWidthOfRows;

		// Token: 0x04000346 RID: 838
		private readonly TimelineWindow.TimelineState m_State;

		// Token: 0x04000347 RID: 839
		private static readonly float kMinTrackHeight = 25f;

		// Token: 0x04000348 RID: 840
		private static readonly float kFoldOutOffset = 14f;

		// Token: 0x04000349 RID: 841
		private static DirectorStyles m_Styles;
	}
}
