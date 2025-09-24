using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200002A RID: 42
	internal class TimelineClipCurveDataSource : CurveDataSource
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000FE71 File Offset: 0x0000E271
		public TimelineClipCurveDataSource(TimelineClipGUI clipGUI) : base(clipGUI.parentTrack)
		{
			this.m_ClipGUI = clipGUI;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000FE88 File Offset: 0x0000E288
		public override AnimationClip animationClip
		{
			get
			{
				return this.m_ClipGUI.clip.animationClip ?? this.m_ClipGUI.clip.curves;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000FEC4 File Offset: 0x0000E2C4
		public override float start
		{
			get
			{
				return (float)(this.m_ClipGUI.clip.start - this.m_ClipGUI.clip.clipIn);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000FEFC File Offset: 0x0000E2FC
		public override float timeScale
		{
			get
			{
				return (float)this.m_ClipGUI.clip.timeScale;
			}
		}

		// Token: 0x0400017D RID: 381
		private readonly TimelineClipGUI m_ClipGUI;
	}
}
