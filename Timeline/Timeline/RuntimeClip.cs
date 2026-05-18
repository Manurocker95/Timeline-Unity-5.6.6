using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000008 RID: 8
	internal class RuntimeClip : RuntimeClipBase
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000383F File Offset: 0x00001A3F
		public RuntimeClip(TimelineClip clip, PlayableHandle clipPlayable, PlayableHandle parentMixer)
		{
			this.Create(clip, clipPlayable, parentMixer);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00003858 File Offset: 0x00001A58
		public override double start
		{
			get
			{
				return this.m_Clip.extrapolatedStart - RuntimeClipBase.kTimeEpsilon;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00003880 File Offset: 0x00001A80
		public override double duration
		{
			get
			{
				return (!this.inclusiveDuration) ? this.m_Clip.extrapolatedDuration : (this.m_Clip.extrapolatedDuration + RuntimeClipBase.kTimeEpsilon * 2.0);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000038CB File Offset: 0x00001ACB
		private void Create(TimelineClip clip, PlayableHandle clipPlayable, PlayableHandle parentMixer)
		{
			this.m_Clip = clip;
			this.m_Playable = clipPlayable;
			this.m_ParentMixer = parentMixer;
			clipPlayable.playState = 0;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000038EC File Offset: 0x00001AEC
		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00003908 File Offset: 0x00001B08
		public PlayableHandle mixer
		{
			get
			{
				return this.m_ParentMixer;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00003924 File Offset: 0x00001B24
		public PlayableHandle playable
		{
			get
			{
				return this.m_Playable;
			}
		}

		// Token: 0x1700000C RID: 12
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00003940 File Offset: 0x00001B40
		public override bool enable
		{
			set
			{
				PlayState playState = (!value) ? (PlayState)0 : (PlayState)1;
				PlayState playState2 = this.m_Playable.playState;
				if (playState != playState2)
				{
					this.m_Playable.playState = playState;
				}
				if (playState == (PlayState)0 && this.m_ParentMixer.IsValid())
				{
					this.m_ParentMixer.SetInputWeight(this.m_Playable, 0f);
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000039AB File Offset: 0x00001BAB
		public void SetTime(double time)
		{
			this.m_Playable.time = time;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000039BA File Offset: 0x00001BBA
		public void SetDuration(double duration)
		{
			this.m_Playable.duration = duration;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000039CC File Offset: 0x00001BCC
		public override void EvaluateAt(double localTime, double deltaTime, bool forceSeek)
		{
			this.enable = true;
			float num;
			if (this.clip.IsPreExtrapolatedTime(localTime))
			{
				num = this.clip.EvaluateMixIn((double)((float)this.clip.start));
			}
			else if (this.clip.IsPostExtrapolatedTime(localTime))
			{
				num = this.clip.EvaluateMixOut((double)((float)this.clip.end));
			}
			else
			{
				num = this.clip.EvaluateMixIn(localTime) * this.clip.EvaluateMixOut(localTime);
			}
			if (this.mixer.IsValid())
			{
				this.mixer.SetInputWeight(this.playable, num);
			}
			double time = this.clip.ToLocalTime(localTime);
			if (time.CompareTo(0.0) >= 0)
			{
				this.SetTime(time);
			}
			this.SetDuration(this.clip.extrapolatedDuration);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00003AC4 File Offset: 0x00001CC4
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00003ADF File Offset: 0x00001CDF
		internal bool inclusiveDuration
		{
			get
			{
				return this.m_InclusiveDuration;
			}
			set
			{
				this.m_InclusiveDuration = value;
			}
		}

		// Token: 0x04000011 RID: 17
		private TimelineClip m_Clip;

		// Token: 0x04000012 RID: 18
		private PlayableHandle m_Playable;

		// Token: 0x04000013 RID: 19
		private PlayableHandle m_ParentMixer;

		// Token: 0x04000014 RID: 20
		private bool m_InclusiveDuration = false;
	}
}
