namespace UnityEngine.Playables
{
    public sealed class AnimationClipPlayable : AnimationPlayable
    {
        public static AnimationClipPlayable Create(
            PlayableGraph graph,
            AnimationClip clip)
        {
            PlayableHandle handle =
                graph.CreateAnimationClipPlayable(clip);

            return handle.IsValid()
                ? handle.GetObject<AnimationClipPlayable>()
                : null;
        }

        public AnimationClip clip
        {
            get { return LegacyPlayableRuntime.GetAnimationClip(handle); }
        }

        public float speed
        {
            get { return (float)handle.speed; }
            set { handle.speed = value; }
        }

        public bool applyFootIK
        {
            get
            {
                return LegacyPlayableRuntime
                    .GetAnimationApplyFootIK(handle);
            }
            set
            {
                LegacyPlayableRuntime
                    .SetAnimationApplyFootIK(handle, value);
            }
        }

        public void SetApplyFootIK(bool value)
        {
            applyFootIK = value;
        }

        public bool GetApplyFootIK()
        {
            return applyFootIK;
        }

        internal bool removeStartOffset
        {
            get
            {
                return LegacyPlayableRuntime
                    .GetAnimationRemoveStartOffset(handle);
            }
            set
            {
                LegacyPlayableRuntime
                    .SetAnimationRemoveStartOffset(handle, value);
            }
        }
    }
}
