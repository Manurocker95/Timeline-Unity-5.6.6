using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Playables.Audio
{
    /// <summary>
    /// Managed audio mixer playable.
    /// </summary>
    public sealed class AudioMixerPlayable : AudioPlayable
    {
        public bool autoNormalizeVolumes
        {
            get
            {
                return LegacyPlayableRuntime.GetAudioMixerNormalizeVolumes(
                    handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAudioMixerNormalizeVolumes(
                    handle,
                    value);
            }
        }
    }
}
