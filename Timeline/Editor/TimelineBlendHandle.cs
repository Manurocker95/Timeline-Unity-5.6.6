using System;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x0200007D RID: 125
	internal class TimelineBlendHandle : Control, IBounds
	{
		// Token: 0x060003F1 RID: 1009 RVA: 0x0001F6FD File Offset: 0x0001DAFD
		public TimelineBlendHandle(TimelineClipGUI theClip, TimelineBlendHandle.DragDirection direction)
		{
			this.direction = direction;
			this.clip = theClip;
			this.m_Styles = DirectorStyles.Instance;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0001F720 File Offset: 0x0001DB20
		public override Rect bounds
		{
			get
			{
				return this.m_Rect;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0001F73C File Offset: 0x0001DB3C
		public Rect boundingRect
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001F758 File Offset: 0x0001DB58
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0001F772 File Offset: 0x0001DB72
		public TimelineBlendHandle.DragDirection direction { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001F77C File Offset: 0x0001DB7C
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0001F796 File Offset: 0x0001DB96
		public TimelineClipGUI clip { get; private set; }

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001F7A0 File Offset: 0x0001DBA0
		public void Draw(Rect clientRect, TimelineWindow.TimelineState state)
		{
			Rect rect = clientRect;
			if (this.direction == TimelineBlendHandle.DragDirection.Left)
			{
				rect.width = this.m_Styles.handLeft.fixedWidth;
				rect.x -= this.m_Styles.handLeft.fixedWidth;
				rect.y = rect.y + rect.height - this.m_Styles.handLeft.fixedHeight;
				rect.height = this.m_Styles.handLeft.fixedHeight;
			}
			if (this.direction == TimelineBlendHandle.DragDirection.Right)
			{
				rect.x = state.TimeToTimeAreaPixel((double)this.clip.blendingStopsAt);
				rect.y += 1f;
				rect.width = this.m_Styles.handLeft.fixedWidth;
				rect.height = this.m_Styles.handLeft.fixedHeight;
			}
			EditorGUIUtility.AddCursorRect(rect, 19);
			this.m_Rect = rect;
			GUI.Box(rect, GUIContent.none, (this.direction != TimelineBlendHandle.DragDirection.Left) ? this.m_Styles.handRight : this.m_Styles.handLeft);
		}

		// Token: 0x04000298 RID: 664
		private Rect m_Rect;

		// Token: 0x04000299 RID: 665
		private DirectorStyles m_Styles;

		// Token: 0x0200007E RID: 126
		public enum DragDirection
		{
			// Token: 0x0400029D RID: 669
			Left,
			// Token: 0x0400029E RID: 670
			Right
		}
	}
}
