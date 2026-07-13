using System;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed playable that represents an AnimationClip.
    /// </summary>
    public sealed class AnimationClipPlayable : AnimationPlayable
    {
        public AnimationClip clip
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationClip(handle);
            }
        }

        public float speed
        {
            get
            {
                return (float)handle.speed;
            }
            set
            {
                handle.speed = value;
            }
        }

        public bool applyFootIK
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationApplyFootIK(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAnimationApplyFootIK(handle, value);
            }
        }

        internal bool removeStartOffset
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationRemoveStartOffset(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAnimationRemoveStartOffset(handle, value);
            }
        }
    }
}
