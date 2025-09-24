using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
    /// <summary>
    ///   <para>Playable that plays an AnimationClip. Can be used as an input to an AnimationPlayable.</para>
    /// </summary>
    public sealed class AnimationClipPlayable : AnimationPlayable
	{
        private static readonly Dictionary<PlayableHandle, AnimationClip> s_Clips = new Dictionary<PlayableHandle, AnimationClip>();
        private static readonly Dictionary<PlayableHandle, float> s_Speeds = new Dictionary<PlayableHandle, float>();
        private static readonly Dictionary<PlayableHandle, bool> s_FootIK = new Dictionary<PlayableHandle, bool>();
        private static readonly Dictionary<PlayableHandle, bool> s_RemoveOffset = new Dictionary<PlayableHandle, bool>();

        /// <summary>
        ///   <para>AnimationClip played by this Playable.</para>
        /// </summary>
        public AnimationClip clip
		{
			get
			{
				return AnimationClipPlayable.GetAnimationClip(ref this.handle);
			}
		}

		/// <summary>
		///   <para>The speed at which the AnimationClip is played.</para>
		/// </summary>
		public float speed
		{
			get
			{
				return AnimationClipPlayable.GetSpeed(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetSpeed(ref this.handle, value);
			}
		}

		/// <summary>
		///   <para>Applies Humanoid FootIK solver.</para>
		/// </summary>
		public bool applyFootIK
		{
			get
			{
				return AnimationClipPlayable.GetApplyFootIK(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetApplyFootIK(ref this.handle, value);
			}
		}

		internal bool removeStartOffset
		{
			get
			{
				return AnimationClipPlayable.GetRemoveStartOffset(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetRemoveStartOffset(ref this.handle, value);
			}
		}

		private static AnimationClip GetAnimationClip(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetAnimationClip(ref handle);
		}

        private static AnimationClip INTERNAL_CALL_GetAnimationClip(ref PlayableHandle handle)
        {
            AnimationClip clip;
            if (s_Clips.TryGetValue(handle, out clip))
                return clip;
            return null;
        }

        private static float GetSpeed(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetSpeed(ref handle);
		}

        private static float INTERNAL_CALL_GetSpeed(ref PlayableHandle handle)
        {
            float val;
            if (s_Speeds.TryGetValue(handle, out val))
                return val;
            return 1f; // default playback speed
        }

        private static void SetSpeed(ref PlayableHandle handle, float value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetSpeed(ref handle, value);
		}

        private static void INTERNAL_CALL_SetSpeed(ref PlayableHandle handle, float value)
        {
            s_Speeds[handle] = value;
        }

        private static bool GetApplyFootIK(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetApplyFootIK(ref handle);
		}

        private static bool INTERNAL_CALL_GetApplyFootIK(ref PlayableHandle handle)
        {
            bool val;
            if (s_FootIK.TryGetValue(handle, out val))
                return val;
            return false;
        }

        private static void SetApplyFootIK(ref PlayableHandle handle, bool value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetApplyFootIK(ref handle, value);
		}

        private static void INTERNAL_CALL_SetApplyFootIK(ref PlayableHandle handle, bool value)
        {
            s_FootIK[handle] = value;
        }

        private static bool GetRemoveStartOffset(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetRemoveStartOffset(ref handle);
		}

        private static bool INTERNAL_CALL_GetRemoveStartOffset(ref PlayableHandle handle)
        {
            bool val;
            if (s_RemoveOffset.TryGetValue(handle, out val))
                return val;
            return false;
        }

        private static void SetRemoveStartOffset(ref PlayableHandle handle, bool value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetRemoveStartOffset(ref handle, value);
		}

        private static void INTERNAL_CALL_SetRemoveStartOffset(ref PlayableHandle handle, bool value)
        {
            s_RemoveOffset[handle] = value;
        }
    }
}
