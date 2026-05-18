using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	// Token: 0x02000215 RID: 533
	
	public sealed class AudioMixerPlayable : AudioPlayable
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x0002863C File Offset: 0x0002683C
		// (set) Token: 0x06002312 RID: 8978 RVA: 0x0002865C File Offset: 0x0002685C
		public bool autoNormalizeVolumes
		{
			get
			{
				return AudioMixerPlayable.GetAutoNormalize(ref this.handle);
			}
			set
			{
				AudioMixerPlayable.SetAutoNormalize(ref this.handle, value);
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0002866C File Offset: 0x0002686C
		private static bool GetAutoNormalize(ref PlayableHandle hdl)
		{
			return AudioMixerPlayable.INTERNAL_CALL_GetAutoNormalize(ref hdl);
		}

		// Token: 0x06002314 RID: 8980
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_GetAutoNormalize(ref PlayableHandle hdl);

		// Token: 0x06002315 RID: 8981 RVA: 0x00028688 File Offset: 0x00026888
		private static void SetAutoNormalize(ref PlayableHandle hdl, bool normalise)
		{
			AudioMixerPlayable.INTERNAL_CALL_SetAutoNormalize(ref hdl, normalise);
		}

		// Token: 0x06002316 RID: 8982
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetAutoNormalize(ref PlayableHandle hdl, bool normalise);
	}
}
