using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	// Token: 0x02000217 RID: 535
	
	public sealed class AudioDSPPlayable : AudioPlayable
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0002869C File Offset: 0x0002689C
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x000286BC File Offset: 0x000268BC
		public bool bypass
		{
			get
			{
				return AudioDSPPlayable.InternalGetAudioDSPBypass(ref this.handle);
			}
			set
			{
				AudioDSPPlayable.InternalSetAudioDSPBypass(ref this.handle, value);
			}
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000286CC File Offset: 0x000268CC
		public static void SetBypass(PlayableHandle handle, bool bypass)
		{
			Type playableTypeOf = PlayableHandle.GetPlayableTypeOf(ref handle);
			if (playableTypeOf != null)
			{
				if (playableTypeOf != typeof(AudioDSPPlayable))
				{
					throw new InvalidOperationException("The handle is not an AudioDSPPlayable");
				}
				AudioDSPPlayable.InternalSetAudioDSPBypass(ref handle, bypass);
			}
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x00028710 File Offset: 0x00026910
		private static void InternalSetAudioDSPBypass(ref PlayableHandle playable, bool bypass)
		{
			AudioDSPPlayable.INTERNAL_CALL_InternalSetAudioDSPBypass(ref playable, bypass);
		}

		// Token: 0x0600231C RID: 8988
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalSetAudioDSPBypass(ref PlayableHandle playable, bool bypass);

		// Token: 0x0600231D RID: 8989 RVA: 0x0002871C File Offset: 0x0002691C
		private static bool InternalGetAudioDSPBypass(ref PlayableHandle playable)
		{
			return AudioDSPPlayable.INTERNAL_CALL_InternalGetAudioDSPBypass(ref playable);
		}

		// Token: 0x0600231E RID: 8990
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalGetAudioDSPBypass(ref PlayableHandle playable);
	}
}
