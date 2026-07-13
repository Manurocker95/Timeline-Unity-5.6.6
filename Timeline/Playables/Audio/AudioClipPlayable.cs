using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Playables.Audio
{
    /// <summary>
    /// Managed playable that represents an AudioClip.
    /// </summary>
    public sealed class AudioClipPlayable : AudioPlayable
    {
        public AudioClip clip
        {
            get
            {
                return LegacyPlayableRuntime.GetAudioClip(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAudioClip(handle, value);
            }
        }

        public bool looped
        {
            get
            {
                return LegacyPlayableRuntime.GetAudioLooped(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAudioLooped(handle, value);
            }
        }

        public bool isPlaying
        {
            get
            {
                return handle.IsValid() &&
                       handle.playState == PlayState.Playing &&
                       !handle.isDone;
            }
        }
    }
}
