using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	internal class AudioPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000718C File Offset: 0x0000538C
		// (set) Token: 0x06000166 RID: 358 RVA: 0x000071A7 File Offset: 0x000053A7
		public AudioClip clip
		{
			get
			{
				return this.m_Clip;
			}
			set
			{
				this.m_Clip = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000071B4 File Offset: 0x000053B4
		public override double duration
		{
			get
			{
				double result;
				if (this.m_Clip == null)
				{
					result = base.duration;
				}
				else
				{
					result = (double)this.m_Clip.length;
				}
				return result;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000168 RID: 360 RVA: 0x000071F4 File Offset: 0x000053F4
		public override PlayableBinding[] outputs
		{
			get
			{
				if (this.m_Outputs == null)
				{
					PlayableBinding playableBinding = default(PlayableBinding);
					playableBinding.streamName = "audio";
					playableBinding.streamType = (DirectorStreamType)1;
					this.m_Outputs = new PlayableBinding[]
					{
						playableBinding
					};
				}
				return this.m_Outputs;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007254 File Offset: 0x00005454
		public override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return AudioPlayableGraphExtensions.CreateAudioClipPlayable(graph, this.m_Clip, this.m_Loop);
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000727C File Offset: 0x0000547C
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ClipCaps.Blending | ((!this.m_Loop) ? ClipCaps.None : ClipCaps.Looping);
			}
		}

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		[NotKeyable]
		private AudioClip m_Clip;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		[NotKeyable]
		private bool m_Loop = true;

		// Token: 0x040000C7 RID: 199
		private PlayableBinding[] m_Outputs = null;
	}
}
