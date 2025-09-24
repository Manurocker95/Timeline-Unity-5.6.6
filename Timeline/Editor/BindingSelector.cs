using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x02000025 RID: 37
	internal class BindingSelector : ISelectable
	{
		// Token: 0x0600017E RID: 382 RVA: 0x0000EFE8 File Offset: 0x0000D3E8
		public BindingSelector(EditorWindow window, CurveEditor curveEditor)
		{
			this.m_Window = (window as TimelineWindow);
			this.m_CurveEditor = curveEditor;
			this.m_DopeLines = new ReorderableList(this.m_StringList, typeof(string), false, false, false, false);
			this.m_DopeLines.onSelectCallback = new ReorderableList.SelectCallbackDelegate(this.OnSelectDopeLine);
			this.m_DopeLines.drawElementBackgroundCallback = null;
			this.m_DopeLines.showDefaultBackground = false;
			this.m_DopeLines.index = 0;
			this.m_DopeLines.headerHeight = 0f;
			this.m_DopeLines.elementHeight = 20f;
			this.m_DopeLines.draggable = false;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000F0A8 File Offset: 0x0000D4A8
		public bool selectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000F0C0 File Offset: 0x0000D4C0
		public object selectableObject
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000F0D8 File Offset: 0x0000D4D8
		// (set) Token: 0x06000182 RID: 386 RVA: 0x0000F0F3 File Offset: 0x0000D4F3
		public bool selected
		{
			get
			{
				return this.m_PartOfSelection;
			}
			set
			{
				this.m_PartOfSelection = value;
				if (!this.m_PartOfSelection)
				{
					this.m_DopeLines.index = -1;
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000F118 File Offset: 0x0000D518
		public virtual void Delete(ITimelineState state)
		{
			if (this.m_DopeLines.index >= 1)
			{
				if (this.m_ClipDataSource != null)
				{
					AnimationClip animationClip = this.m_ClipDataSource.animationClip;
					if (!(animationClip == null))
					{
						int num = this.m_DopeLines.index - 1;
						EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animationClip);
						if (num < curveBindings.Length)
						{
							TimelineHelpers.PushUndo(animationClip, "delete.curve");
							AnimationUtility.SetEditorCurve(animationClip, curveBindings[this.m_DopeLines.index - 1], null);
							state.rebuildGraph = true;
						}
					}
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000F1C0 File Offset: 0x0000D5C0
		private void OnSelectDopeLine(ReorderableList list)
		{
			if (!this.m_Window.state.selection.Contains(this) || this.m_Window.state.selection.Count > 1)
			{
				this.m_Window.state.selection.Clear();
			}
			this.m_Window.state.selection.Add(this);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000F22F File Offset: 0x0000D62F
		public void OnGUI(Rect targetRect)
		{
			if (this.m_TreeView != null)
			{
				this.m_TreeView.OnEvent();
				this.m_TreeView.OnGUI(targetRect, GUIUtility.GetControlID(1));
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000F260 File Offset: 0x0000D660
		public void InitIfNeeded(Rect rect, CurveDataSource dataSource)
		{
			if (Event.current.type == 8)
			{
				this.m_ClipDataSource = dataSource;
				AnimationClip animationClip = dataSource.animationClip;
				int num = (this.m_DopeLines.list == null) ? 0 : this.m_DopeLines.list.Count;
				List<EditorCurveBinding> list = new List<EditorCurveBinding>();
				List<EditorCurveBinding> list2 = list;
				EditorCurveBinding item = default(EditorCurveBinding);
				item.propertyName = "Summary";
				list2.Add(item);
				if (animationClip != null)
				{
					list.AddRange(AnimationUtility.GetCurveBindings(animationClip));
				}
				this.m_DopeLines.list = list.ToArray();
				if (num != this.m_DopeLines.list.Count)
				{
					this.UpdateRowHeight();
				}
				if (this.m_TreeViewState == null)
				{
					this.m_TreeViewState = new TreeViewState();
					this.m_TreeView = new TreeViewController(this.m_Window, this.m_TreeViewState)
					{
						useExpansionAnimation = false,
						deselectOnUnhandledMouseDown = true
					};
					TreeViewController treeView = this.m_TreeView;
					treeView.selectionChangedCallback = (Action<int[]>)Delegate.Combine(treeView.selectionChangedCallback, new Action<int[]>(this.OnItemSelectionChanged));
					this.m_TreeViewDataSource = new BindingTreeViewDataSource(this.m_TreeView, animationClip);
					this.m_TreeView.Init(rect, this.m_TreeViewDataSource, new BindingTreeViewGUI(this.m_TreeView), null);
					this.m_TreeViewDataSource.UpdateData();
					this.OnItemSelectionChanged(null);
				}
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000F3CD File Offset: 0x0000D7CD
		private void UpdateRowHeight()
		{
			this.m_ClipDataSource.SetHeight((float)this.m_DopeLines.list.Count * this.m_DopeLines.elementHeight);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000F3F8 File Offset: 0x0000D7F8
		private void OnItemSelectionChanged(int[] selection)
		{
			if (selection == null || selection.Length == 0)
			{
				if (this.m_TreeViewDataSource.GetRows().Count > 0)
				{
					this.m_Selection = (from r in this.m_TreeViewDataSource.GetRows()
					select r.id).ToArray<int>();
				}
			}
			else
			{
				this.m_Selection = selection.ToArray<int>();
			}
			this.RefreshCurves();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000F480 File Offset: 0x0000D880
		public void RefreshCurves()
		{
			if (this.m_ClipDataSource != null && this.m_Selection != null)
			{
				List<EditorCurveBinding> list = new List<EditorCurveBinding>();
				foreach (int num in this.m_Selection)
				{
					CurveTreeViewNode curveTreeViewNode = (CurveTreeViewNode)this.m_TreeView.FindItem(num);
					if (curveTreeViewNode != null && curveTreeViewNode.bindings != null)
					{
						list.AddRange(curveTreeViewNode.bindings);
					}
				}
				AnimationClip animationClip = this.m_ClipDataSource.animationClip;
				List<CurveWrapper> list2 = new List<CurveWrapper>();
				int num2 = 0;
				foreach (EditorCurveBinding editorCurveBinding in list)
				{
					CurveWrapper curveWrapper = new CurveWrapper
					{
						id = num2++,
						binding = editorCurveBinding,
						groupId = -1,
						color = CurveUtility.GetPropertyColor(editorCurveBinding.propertyName),
						hidden = false,
						readOnly = false,
						renderer = new NormalCurveRenderer(AnimationUtility.GetEditorCurve(animationClip, editorCurveBinding)),
						getAxisUiScalarsCallback = new CurveWrapper.GetAxisScalarsCallback(this.GetAxisScalars)
					};
					curveWrapper.renderer.SetCustomRange(0f, animationClip.length);
					list2.Add(curveWrapper);
				}
				this.m_CurveEditor.animationCurves = list2.ToArray();
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000F60C File Offset: 0x0000DA0C
		public void RefreshTree()
		{
			if (this.m_TreeViewDataSource != null)
			{
				if (this.m_Selection == null)
				{
					this.m_Selection = new int[0];
				}
				string[] selected = (from x in this.m_Selection
				select this.m_TreeViewDataSource.FindItem(x) into t
				where t != null
				select t into c
				select c.displayName).ToArray<string>();
				this.m_TreeViewDataSource.UpdateData();
				int[] array = (from x in this.m_TreeViewDataSource.GetRows()
				where selected.Contains(x.displayName)
				select x.id).ToArray<int>();
				if (!array.Any<int>())
				{
					if (this.m_TreeViewDataSource.GetRows().Count > 0)
					{
						array = new int[]
						{
							this.m_TreeViewDataSource.GetItem(0).id
						};
					}
				}
				this.OnItemSelectionChanged(array);
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000F74C File Offset: 0x0000DB4C
		private Vector2 GetAxisScalars()
		{
			return new Vector2(1f, 1f);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000F770 File Offset: 0x0000DB70
		internal virtual bool IsRenamingNodeAllowed(TreeViewItem node)
		{
			return false;
		}

		// Token: 0x04000163 RID: 355
		private TreeViewController m_TreeView;

		// Token: 0x04000164 RID: 356
		private TreeViewState m_TreeViewState;

		// Token: 0x04000165 RID: 357
		private BindingTreeViewDataSource m_TreeViewDataSource;

		// Token: 0x04000166 RID: 358
		private CurveDataSource m_ClipDataSource;

		// Token: 0x04000167 RID: 359
		private TimelineWindow m_Window;

		// Token: 0x04000168 RID: 360
		private CurveEditor m_CurveEditor = null;

		// Token: 0x04000169 RID: 361
		private ReorderableList m_DopeLines;

		// Token: 0x0400016A RID: 362
		private string[] m_StringList = new string[0];

		// Token: 0x0400016B RID: 363
		public static float kBottomPadding = 5f;

		// Token: 0x0400016C RID: 364
		private int[] m_Selection;

		// Token: 0x0400016D RID: 365
		private bool m_PartOfSelection;
	}
}
