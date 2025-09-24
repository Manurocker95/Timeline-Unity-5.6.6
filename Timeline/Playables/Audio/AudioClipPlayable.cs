using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	/// <summary>
	///   <para>A playable which plays an AudioClip.</para>
	/// </summary>
	public sealed class AudioClipPlayable : AudioPlayable
	{
        private static readonly Dictionary<PlayableHandle, AudioClip> s_Clips = new Dictionary<PlayableHandle, AudioClip>();
        private static readonly Dictionary<PlayableHandle, bool> s_Loops = new Dictionary<PlayableHandle, bool>();
        private static readonly HashSet<PlayableHandle> s_Playing = new HashSet<PlayableHandle>();

        /// <summary>
        ///   <para>The AudioClip assigned to this AudioClipPlayable.</para>
        /// </summary>
        public AudioClip clip
		{
			get
			{
				return AudioClipPlayable.GetClip(ref this.handle);
			}
			set
			{
				AudioClipPlayable.SetClip(ref this.handle, value);
			}
		}


		private static AudioClip GetClip(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetClip(ref hdl);
		}

        private static AudioClip INTERNAL_CALL_GetClip(ref PlayableHandle hdl)
        {
            AudioClip clip;
            
			if (s_Clips.TryGetValue(hdl, out clip))
                return clip;

            return null;
        }

        private static void SetClip(ref PlayableHandle hdl, AudioClip clip)
		{
			AudioClipPlayable.INTERNAL_CALL_SetClip(ref hdl, clip);
		}

        private static void INTERNAL_CALL_SetClip(ref PlayableHandle hdl, AudioClip clip)
        {
            s_Clips[hdl] = clip;
            // Mark as "playing" if a clip is set, for compatibility.
            if (clip != null)
                s_Playing.Add(hdl);
        }

        /// <summary>
        ///   <para>Is the AudioClip set to loop.</para>
        /// </summary>
        public bool looped
		{
			get
			{
				return AudioClipPlayable.GetLooped(ref this.handle);
			}
			set
			{
				AudioClipPlayable.SetLooped(ref this.handle, value);
			}
		}


		private static bool GetLooped(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetLooped(ref hdl);
		}

        private static bool INTERNAL_CALL_GetLooped(ref PlayableHandle hdl)
        {
            bool loop;
            if (s_Loops.TryGetValue(hdl, out loop))
                return loop;
            return false;
        }

        private static void SetLooped(ref PlayableHandle hdl, bool looped)
		{
			AudioClipPlayable.INTERNAL_CALL_SetLooped(ref hdl, looped);
		}

        private static void INTERNAL_CALL_SetLooped(ref PlayableHandle hdl, bool looped)
        {
            s_Loops[hdl] = looped;
        }

        /// <summary>
        ///   <para>Is the AudioClip currently playing.</para>
        /// </summary>
        public bool isPlaying
		{
			get
			{
				return AudioClipPlayable.GetIsPlaying(ref this.handle);
			}
		}

		private static bool GetIsPlaying(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetIsPlaying(ref hdl);
		}

        private static bool INTERNAL_CALL_GetIsPlaying(ref PlayableHandle hdl)
        {
            return s_Playing.Contains(hdl);
        }
    }
}
