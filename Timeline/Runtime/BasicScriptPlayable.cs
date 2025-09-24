using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000030 RID: 48
	[Serializable]
	public abstract class BasicScriptPlayable : ScriptableObject, IPlayable, IPlayableAsset, IScriptPlayable
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008278 File Offset: 0x00006478
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00008293 File Offset: 0x00006493
		public PlayableHandle playableHandle
		{
			get
			{
				return this.handle;
			}
			set
			{
				this.handle = value;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000082A0 File Offset: 0x000064A0
		public static implicit operator PlayableHandle(BasicScriptPlayable b)
		{
			return b.handle;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000082BC File Offset: 0x000064BC
		public bool IsValid()
		{
			return this.handle.IsValid();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000082DC File Offset: 0x000064DC
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000082F8 File Offset: 0x000064F8
		public virtual PlayableBinding[] outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008312 File Offset: 0x00006512
		public virtual void OnGraphStart()
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008315 File Offset: 0x00006515
		public virtual void OnGraphStop()
		{
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008318 File Offset: 0x00006518
		public virtual void OnDestroy()
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000831B File Offset: 0x0000651B
		public virtual void PrepareFrame(FrameData info)
		{
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000831E File Offset: 0x0000651E
		public virtual void ProcessFrame(FrameData info, object playerData)
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008321 File Offset: 0x00006521
		public virtual void OnPlayStateChanged(FrameData info, PlayState newState)
		{
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008324 File Offset: 0x00006524
		public virtual PlayableHandle CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayableGraphExtensions.CloneScriptPlayable(graph, this);
		}

		// Token: 0x040000DF RID: 223
		public PlayableHandle handle;
	}
}
