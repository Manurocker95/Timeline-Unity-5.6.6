using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Playable that control particle systems.</para>
	/// </summary>
	// Token: 0x02000033 RID: 51
	public class ParticleControlPlayable : ScriptPlayable
	{
		/// <summary>
		///   <para>Which particle system to control.</para>
		/// </summary>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00008504 File Offset: 0x00006704
		public ParticleSystem particleSystem
		{
			get
			{
				return this.m_ParticleSystem;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000851F File Offset: 0x0000671F
		public void Initialize(ParticleSystem particleSystem, uint randomSeed)
		{
			this.m_RandomSeed = Math.Max(1U, randomSeed);
			this.m_ParticleSystem = particleSystem;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008538 File Offset: 0x00006738
		public override void PrepareFrame(FrameData data)
		{
			if (!(this.particleSystem == null) && this.particleSystem.gameObject.activeInHierarchy)
			{
				if (this.particleSystem.randomSeed != this.m_RandomSeed)
				{
					this.particleSystem.Stop();
					this.particleSystem.useAutoRandomSeed = false;
					this.particleSystem.randomSeed = this.m_RandomSeed;
				}
				float num = (float)this.handle.time;
				bool flag = Mathf.Approximately(this.m_LastTime, -1f) || !Mathf.Approximately(this.m_LastTime, num);
				if (flag)
				{
					float num2 = Time.fixedDeltaTime * 0.5f;
					float num3 = num;
					float num4 = num3 - this.m_LastTime;
					bool flag2 = num3 < this.m_LastTime || num3 < num2 || Mathf.Approximately(this.m_LastTime, -1f) || num4 > this.particleSystem.main.duration || !Mathf.Approximately(this.m_LastPsTime, this.particleSystem.time);
					if (flag2)
					{
						this.particleSystem.Simulate(0f, true, true);
						this.particleSystem.Simulate(num3, true, false);
					}
					else
					{
						float num5 = num3 % this.particleSystem.main.duration;
						float num6 = num5 - this.particleSystem.time;
						if (num6 < -num2)
						{
							num6 = num5 + this.particleSystem.main.duration - this.particleSystem.time;
						}
						this.particleSystem.Simulate(num6, true, false);
					}
					this.m_LastPsTime = this.particleSystem.time;
					this.m_LastTime = num;
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008714 File Offset: 0x00006914
		public override void OnPlayStateChanged(FrameData info, PlayState newState)
		{
			this.m_LastTime = -1f;
		}

		// Token: 0x040000E1 RID: 225
		private const float kUnsetTime = -1f;

		// Token: 0x040000E2 RID: 226
		private float m_LastTime = -1f;

		// Token: 0x040000E3 RID: 227
		private float m_LastPsTime = -1f;

		// Token: 0x040000E4 RID: 228
		private uint m_RandomSeed = 1U;

		// Token: 0x040000E5 RID: 229
		private ParticleSystem m_ParticleSystem;
	}
}
