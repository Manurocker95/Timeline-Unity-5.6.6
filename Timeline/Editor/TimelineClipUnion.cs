using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x02000085 RID: 133
	internal class TimelineClipUnion
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x000218DC File Offset: 0x0001FCDC
		public void Add(TimelineClipGUI clip)
		{
			this.m_Members.Add(clip);
			if (this.m_Members.Count == 1)
			{
				this.m_BoundingRect = clip.clippedRect;
			}
			else
			{
				this.m_BoundingRect = RectUtils.Encompass(this.m_BoundingRect, clip.bounds);
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00021934 File Offset: 0x0001FD34
		public void Draw(Rect parentRect, TimelineWindow.TimelineState state)
		{
			if (this.m_InitUnionRect)
			{
				this.m_Start = (from c in this.m_Members
				orderby c.clip.start
				select c).First<TimelineClipGUI>().clip.start;
				this.m_Duration = this.m_Members.Sum((TimelineClipGUI c) => c.clip.duration);
				this.m_InitUnionRect = false;
			}
			this.m_Union = new Rect((float)(this.m_Start + state.GetWindow().context.localStart) * state.timeAreaScale.x, 0f, (float)this.m_Duration * state.timeAreaScale.x, 0f);
			this.m_Union.xMin = this.m_Union.xMin + (state.timeAreaTranslation.x + parentRect.x);
			this.m_Union.xMax = this.m_Union.xMax + (state.timeAreaTranslation.x + parentRect.x);
			this.m_Union.y = parentRect.y + 4f;
			this.m_Union.height = parentRect.height - 8f;
			if (this.m_Union.x < parentRect.xMin)
			{
				float num = parentRect.xMin - this.m_Union.x;
				this.m_Union.x = parentRect.xMin;
				this.m_Union.width = this.m_Union.width - num;
			}
			if (this.m_Union.xMax >= parentRect.xMin)
			{
				if (this.m_Union.xMin <= parentRect.xMax)
				{
					EditorGUI.DrawRect(this.m_Union, DirectorStyles.Instance.customSkin.colorClipUnion);
				}
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00021B3C File Offset: 0x0001FF3C
		public static List<TimelineClipUnion> Build(List<TimelineClipGUI> clips)
		{
			List<TimelineClipUnion> list = new List<TimelineClipUnion>();
			List<TimelineClipUnion> result;
			if (clips == null)
			{
				result = list;
			}
			else
			{
				TimelineClipUnion timelineClipUnion = null;
				foreach (TimelineClipGUI timelineClipGUI in clips)
				{
					Rect rect;
					if (timelineClipUnion == null)
					{
						timelineClipUnion = new TimelineClipUnion();
						timelineClipUnion.Add(timelineClipGUI);
						list.Add(timelineClipUnion);
					}
					else if (RectUtils.Intersection(timelineClipGUI.bounds, timelineClipUnion.m_BoundingRect, ref rect))
					{
						timelineClipUnion.Add(timelineClipGUI);
					}
					else
					{
						timelineClipUnion = new TimelineClipUnion();
						timelineClipUnion.Add(timelineClipGUI);
						list.Add(timelineClipUnion);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x040002CE RID: 718
		public List<TimelineClipGUI> m_Members = new List<TimelineClipGUI>();

		// Token: 0x040002CF RID: 719
		public Rect m_BoundingRect;

		// Token: 0x040002D0 RID: 720
		public Rect m_Union;

		// Token: 0x040002D1 RID: 721
		public double m_Start;

		// Token: 0x040002D2 RID: 722
		public double m_Duration;

		// Token: 0x040002D3 RID: 723
		public bool m_InitUnionRect = true;
	}
}
