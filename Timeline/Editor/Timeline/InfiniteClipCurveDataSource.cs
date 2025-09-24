using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200002B RID: 43
	internal class InfiniteClipCurveDataSource : CurveDataSource
	{
		// Token: 0x060001AE RID: 430 RVA: 0x0000FF22 File Offset: 0x0000E322
		public InfiniteClipCurveDataSource(TimelineTrackGUI trackGui) : base(trackGui)
		{
			this.m_AnimationTrack = (trackGui.track as AnimationTrack);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000FF40 File Offset: 0x0000E340
		public override AnimationClip animationClip
		{
			get
			{
				return this.m_AnimationTrack.animClip;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000FF60 File Offset: 0x0000E360
		public override float start
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000FF7C File Offset: 0x0000E37C
		public override float timeScale
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0400017E RID: 382
		private readonly AnimationTrack m_AnimationTrack;
	}
}
