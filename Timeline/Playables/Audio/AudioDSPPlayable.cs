using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	public sealed class AudioDSPPlayable : AudioPlayable
	{
        private static readonly Dictionary<PlayableHandle, bool> s_Bypass = new Dictionary<PlayableHandle, bool>();

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

		private static void InternalSetAudioDSPBypass(ref PlayableHandle playable, bool bypass)
		{
			AudioDSPPlayable.INTERNAL_CALL_InternalSetAudioDSPBypass(ref playable, bypass);
		}

        private static void INTERNAL_CALL_InternalSetAudioDSPBypass(ref PlayableHandle playable, bool bypass)
        {
            s_Bypass[playable] = bypass;
        }

        // Token: 0x0600231D RID: 8989 RVA: 0x0002871C File Offset: 0x0002691C
        private static bool InternalGetAudioDSPBypass(ref PlayableHandle playable)
		{
			return AudioDSPPlayable.INTERNAL_CALL_InternalGetAudioDSPBypass(ref playable);
		}

        private static bool INTERNAL_CALL_InternalGetAudioDSPBypass(ref PlayableHandle playable)
        {
            bool val;
            if (s_Bypass.TryGetValue(playable, out val))
                return val;
            return false;
        }
    }
}
