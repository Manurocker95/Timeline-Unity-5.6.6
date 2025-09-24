using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	public sealed class AudioMixerPlayable : AudioPlayable
	{
        private static readonly Dictionary<PlayableHandle, bool> s_AutoNormalize = new Dictionary<PlayableHandle, bool>();

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

		private static bool GetAutoNormalize(ref PlayableHandle hdl)
		{
			return AudioMixerPlayable.INTERNAL_CALL_GetAutoNormalize(ref hdl);
		}

        private static bool INTERNAL_CALL_GetAutoNormalize(ref PlayableHandle hdl)
        {
            bool val;
            if (s_AutoNormalize.TryGetValue(hdl, out val))
                return val;
            return false; // default when not set
        }

        private static void SetAutoNormalize(ref PlayableHandle hdl, bool normalise)
		{
			AudioMixerPlayable.INTERNAL_CALL_SetAutoNormalize(ref hdl, normalise);
		}

        private static void INTERNAL_CALL_SetAutoNormalize(ref PlayableHandle hdl, bool normalise)
        {
            s_AutoNormalize[hdl] = normalise;
        }
    }
}
