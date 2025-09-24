using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000029 RID: 41
	internal abstract class CurveDataSource
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000FDE5 File Offset: 0x0000E1E5
		protected CurveDataSource(TimelineTrackGUI trackGUI)
		{
			this.m_TrackGUI = trackGUI;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000FDFC File Offset: 0x0000E1FC
		public void SetHeight(float height)
		{
			this.m_TrackGUI.SetHeight(height);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000FE0C File Offset: 0x0000E20C
		public Rect GetBackgroundRect(TimelineWindow.TimelineState state)
		{
			Rect boundingRect = this.m_TrackGUI.boundingRect;
			return new Rect(state.timeAreaTranslation.x + boundingRect.xMin, boundingRect.y, (float)state.timeline.duration * state.timeAreaScale.x, boundingRect.height);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001A7 RID: 423
		public abstract AnimationClip animationClip { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001A8 RID: 424
		public abstract float start { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001A9 RID: 425
		public abstract float timeScale { get; }

		// Token: 0x0400017B RID: 379
		protected readonly TimelineTrackGUI m_TrackGUI;

		// Token: 0x0400017C RID: 380
		protected bool infiniteEditor = true;
	}
}
