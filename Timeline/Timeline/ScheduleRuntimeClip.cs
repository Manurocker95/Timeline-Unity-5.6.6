using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000009 RID: 9
	internal class ScheduleRuntimeClip : RuntimeClipBase
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00003AE9 File Offset: 0x00001CE9
		public ScheduleRuntimeClip(TimelineClip clip, PlayableHandle clipPlayable, PlayableHandle parentMixer, double startDelay = 2.0)
		{
			this.Create(clip, clipPlayable, parentMixer, startDelay);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003B04 File Offset: 0x00001D04
		public override double start
		{
			get
			{
				return Math.Max(0.0, this.m_Clip.start - this.m_StartDelay - RuntimeClipBase.kTimeEpsilon);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003B40 File Offset: 0x00001D40
		public override double duration
		{
			get
			{
				return this.m_Clip.duration + this.m_StartDelay;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00003B68 File Offset: 0x00001D68
		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003B84 File Offset: 0x00001D84
		public PlayableHandle mixer
		{
			get
			{
				return this.m_ParentMixer;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public PlayableHandle playable
		{
			get
			{
				return this.m_Playable;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003BBB File Offset: 0x00001DBB
		private void Create(TimelineClip clip, PlayableHandle clipPlayable, PlayableHandle parentMixer, double startDelay)
		{
			this.m_Clip = clip;
			this.m_Playable = clipPlayable;
			this.m_ParentMixer = parentMixer;
			this.m_StartDelay = startDelay;
			clipPlayable.playState = 0;
		}

		// Token: 0x17000013 RID: 19
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public override bool enable
		{
			set
			{
				PlayState playState = (!value) ? (PlayState)0 : (PlayState)1;
				PlayState playState2 = this.m_Playable.playState;
				if (playState != playState2)
				{
					this.m_NeedsInit = true;
					this.m_Playable.playState = playState;
					if (playState == (PlayState)0 && this.m_ParentMixer.IsValid())
					{
						this.m_ParentMixer.SetInputWeight(this.m_Playable, 0f);
					}
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003C56 File Offset: 0x00001E56
		public void SetTime(double time)
		{
			this.m_Playable.time = time;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003C65 File Offset: 0x00001E65
		public void SetDuration(double duration)
		{
			this.m_Playable.duration = duration;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003C74 File Offset: 0x00001E74
		public override void EvaluateAt(double localTime, double deltaTime, bool forceSeek)
		{
			double num = 0.0;
			bool flag = localTime >= this.clip.start && localTime <= this.clip.end;
			this.enable = flag;
			float num2 = 0f;
			if (flag)
			{
				num2 = this.clip.EvaluateMixIn(localTime) * this.clip.EvaluateMixOut(localTime);
			}
			if (this.mixer.IsValid())
			{
				this.mixer.SetInputWeight(this.playable, num2);
			}
			double num3 = this.clip.ToLocalTimeUnbound(localTime);
			if (forceSeek || this.m_NeedsInit)
			{
				this.m_NeedsInit = false;
				this.SetTime(Math.Max(num3 - num, 0.0));
			}
			this.SetDuration(this.clip.extrapolatedDuration);
		}

		// Token: 0x04000015 RID: 21
		private TimelineClip m_Clip;

		// Token: 0x04000016 RID: 22
		private PlayableHandle m_Playable;

		// Token: 0x04000017 RID: 23
		private PlayableHandle m_ParentMixer;

		// Token: 0x04000018 RID: 24
		private double m_StartDelay;

		// Token: 0x04000019 RID: 25
		private bool m_NeedsInit = true;
	}
}
