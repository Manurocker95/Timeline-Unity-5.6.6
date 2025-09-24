using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
	// Token: 0x02000026 RID: 38
	internal class BindingTreeViewDataSource : TreeViewDataSource
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000F859 File Offset: 0x0000DC59
		public BindingTreeViewDataSource(TreeViewController treeView, AnimationClip clip) : base(treeView)
		{
			this.m_Clip = clip;
			base.showRootItem = false;
			base.rootIsCollapsable = false;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000F878 File Offset: 0x0000DC78
		private void SetupRootNodeSettings()
		{
			base.showRootItem = false;
			this.SetExpanded(base.root, true);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000F890 File Offset: 0x0000DC90
		private static string GroupName(EditorCurveBinding binding)
		{
			string text = AnimationWindowUtility.GetNicePropertyGroupDisplayName(binding.type, binding.propertyName);
			if (!string.IsNullOrEmpty(binding.path))
			{
				text = binding.path + " : " + text;
			}
			return text;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000F8E0 File Offset: 0x0000DCE0
		private static string PropertyName(EditorCurveBinding binding)
		{
			return AnimationWindowUtility.GetPropertyDisplayName(binding.propertyName);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000F904 File Offset: 0x0000DD04
		public override void FetchData()
		{
			if (!(this.m_Clip == null))
			{
				List<EditorCurveBinding> source = AnimationUtility.GetCurveBindings(this.m_Clip).ToList<EditorCurveBinding>();
				var enumerable = source.GroupBy((EditorCurveBinding p) => BindingTreeViewDataSource.GroupName(p), (EditorCurveBinding p) => p, (string key, IEnumerable<EditorCurveBinding> g) => new
				{
					parent = key,
					bindings = g.ToList<EditorCurveBinding>()
				});
				this.m_RootItem = new CurveTreeViewNode(-1, null, "root", null);
				this.m_RootItem.children = new List<TreeViewItem>();
				int hashCode = Guid.NewGuid().GetHashCode();
				foreach (var <>__AnonType in enumerable)
				{
					CurveTreeViewNode curveTreeViewNode = new CurveTreeViewNode(hashCode++, this.m_RootItem, <>__AnonType.parent, <>__AnonType.bindings.ToArray());
					this.m_RootItem.children.Add(curveTreeViewNode);
					if (<>__AnonType.bindings.Count > 1)
					{
						for (int i = 0; i < <>__AnonType.bindings.Count; i++)
						{
							if (curveTreeViewNode.children == null)
							{
								curveTreeViewNode.children = new List<TreeViewItem>();
							}
							CurveTreeViewNode item = new CurveTreeViewNode(hashCode++, curveTreeViewNode, BindingTreeViewDataSource.PropertyName(<>__AnonType.bindings[i]), new EditorCurveBinding[]
							{
								<>__AnonType.bindings[i]
							});
							curveTreeViewNode.children.Add(item);
						}
					}
				}
				this.SetupRootNodeSettings();
				this.m_NeedRefreshRows = true;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000FAFC File Offset: 0x0000DEFC
		public void UpdateData()
		{
			this.m_TreeView.ReloadData();
		}

		// Token: 0x04000172 RID: 370
		private AnimationClip m_Clip;
	}
}
