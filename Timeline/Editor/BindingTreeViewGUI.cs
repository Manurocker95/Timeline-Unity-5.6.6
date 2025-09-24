using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
	// Token: 0x02000028 RID: 40
	internal class BindingTreeViewGUI : TreeViewGUI
	{
		// Token: 0x0600019D RID: 413 RVA: 0x0000FBA3 File Offset: 0x0000DFA3
		public BindingTreeViewGUI(TreeViewController treeView) : base(treeView, true)
		{
			this.k_IconWidth = 13f;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000FBBC File Offset: 0x0000DFBC
		public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
		{
			Color color = GUI.color;
			GUI.color = ((node.parent.id != -1) ? BindingTreeViewGUI.s_ChildrenCurveLabelColor : Color.white);
			base.OnRowGUI(rowRect, node, row, selected, focused);
			GUI.color = color;
			this.DoCurveColorIndicator(rowRect, node as CurveTreeViewNode);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000FC18 File Offset: 0x0000E018
		protected override bool IsRenaming(int id)
		{
			return false;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000FC30 File Offset: 0x0000E030
		public override bool BeginRename(TreeViewItem item, float delay)
		{
			return false;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000FC48 File Offset: 0x0000E048
		private void DoCurveColorIndicator(Rect rect, CurveTreeViewNode node)
		{
			if (node != null)
			{
				if (Event.current.type == 7)
				{
					Color color = GUI.color;
					if (node.bindings.Length == 1 && !node.bindings[0].isPPtrCurve)
					{
						GUI.color = CurveUtility.GetPropertyColor(node.bindings[0].propertyName);
					}
					else
					{
						GUI.color = BindingTreeViewGUI.s_KeyColorForNonCurves;
					}
					Texture iconCurve = CurveUtility.GetIconCurve();
					rect..ctor(rect.xMax - BindingTreeViewGUI.s_RowRightOffset - (float)iconCurve.width * 0.5f - 5f, rect.yMin + BindingTreeViewGUI.s_ColorIndicatorTopMargin, (float)iconCurve.width, (float)iconCurve.height);
					GUI.DrawTexture(rect, iconCurve, 2, true, 1f);
					GUI.color = color;
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000FD28 File Offset: 0x0000E128
		protected override Texture GetIconForItem(TreeViewItem item)
		{
			CurveTreeViewNode curveTreeViewNode = item as CurveTreeViewNode;
			Texture result;
			if (curveTreeViewNode == null)
			{
				result = null;
			}
			else if (curveTreeViewNode.bindings == null || curveTreeViewNode.bindings.Length == 0)
			{
				result = null;
			}
			else
			{
				result = AssetPreview.GetMiniTypeThumbnail(curveTreeViewNode.bindings[0].type);
			}
			return result;
		}

		// Token: 0x04000177 RID: 375
		private static readonly float s_RowRightOffset = 10f;

		// Token: 0x04000178 RID: 376
		private static readonly float s_ColorIndicatorTopMargin = 3f;

		// Token: 0x04000179 RID: 377
		private static readonly Color s_KeyColorForNonCurves = new Color(0.7f, 0.7f, 0.7f, 0.5f);

		// Token: 0x0400017A RID: 378
		private static readonly Color s_ChildrenCurveLabelColor = new Color(1f, 1f, 1f, 0.7f);
	}
}
