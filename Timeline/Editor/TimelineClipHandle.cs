using System;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x02000081 RID: 129
	internal class TimelineClipHandle : Control, IBounds
	{
		// Token: 0x06000444 RID: 1092 RVA: 0x0002151E File Offset: 0x0001F91E
		public TimelineClipHandle(TimelineClipGUI theClip, TimelineClipHandle.DragDirection direction, DragClipHandle clipHandleManipulator)
		{
			this.m_Direction = direction;
			this.m_Clip = theClip;
			this.m_Styles = DirectorStyles.Instance;
			base.AddManipulator(clipHandleManipulator);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00021548 File Offset: 0x0001F948
		public override Rect bounds
		{
			get
			{
				return this.m_Rect;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00021564 File Offset: 0x0001F964
		public Rect boundingRect
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00021580 File Offset: 0x0001F980
		public TimelineClipHandle.DragDirection direction
		{
			get
			{
				return this.m_Direction;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0002159C File Offset: 0x0001F99C
		public TimelineClipGUI clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000215B8 File Offset: 0x0001F9B8
		public void Draw(Rect clientRect)
		{
			Rect rect = clientRect;
			rect.width = this.m_Styles.sequenceClipHandle.fixedWidth;
			if (this.m_Direction == TimelineClipHandle.DragDirection.Left)
			{
				rect.width = this.m_Styles.sequenceClipHandle.fixedWidth;
				rect.x -= 1f;
			}
			if (this.m_Direction == TimelineClipHandle.DragDirection.Right)
			{
				rect.x = clientRect.xMax - this.m_Styles.sequenceClipHandle.fixedWidth;
				rect.x += 1f;
			}
			EditorGUIUtility.AddCursorRect(rect, 19);
			this.m_Rect = rect;
		}

		// Token: 0x040002C0 RID: 704
		private Rect m_Rect;

		// Token: 0x040002C1 RID: 705
		private TimelineClipGUI m_Clip;

		// Token: 0x040002C2 RID: 706
		private DirectorStyles m_Styles;

		// Token: 0x040002C3 RID: 707
		private TimelineClipHandle.DragDirection m_Direction;

		// Token: 0x02000082 RID: 130
		public enum DragDirection
		{
			// Token: 0x040002C5 RID: 709
			Left,
			// Token: 0x040002C6 RID: 710
			Right
		}
	}
}
