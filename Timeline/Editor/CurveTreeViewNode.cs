using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
	// Token: 0x02000027 RID: 39
	internal class CurveTreeViewNode : TreeViewItem
	{
		// Token: 0x0600019B RID: 411 RVA: 0x0000FB60 File Offset: 0x0000DF60
		public CurveTreeViewNode(int id, TreeViewItem parent, string displayName, EditorCurveBinding[] bindings) : base(id, (parent == null) ? -1 : (parent.depth + 1), parent, displayName)
		{
			this.m_Bindings = bindings;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000FB88 File Offset: 0x0000DF88
		public EditorCurveBinding[] bindings
		{
			get
			{
				return this.m_Bindings;
			}
		}

		// Token: 0x04000176 RID: 374
		private EditorCurveBinding[] m_Bindings;
	}
}
