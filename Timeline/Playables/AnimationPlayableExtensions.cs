namespace UnityEngine.Playables
{
    /// <summary>
    /// Animation-specific extensions for managed PlayableHandles.
    /// </summary>
    public static class AnimationPlayableExtensions
    {
        /// <summary>
        /// Gets the AnimationClip used to animate properties on this playable.
        /// </summary>
        public static AnimationClip GetAnimatedProperties(
            this PlayableHandle handle)
        {
            if (!handle.IsValid())
                return null;

            return LegacyPlayableRuntime.GetAnimatedProperties(handle);
        }

        /// <summary>
        /// Sets the AnimationClip used to animate properties on this playable.
        /// </summary>
        public static void SetAnimatedProperties(
            this PlayableHandle handle,
            AnimationClip clip)
        {
            if (!handle.IsValid())
                return;

            LegacyPlayableRuntime.SetAnimatedProperties(
                handle,
                clip);
        }

        internal static AnimationClip GetAnimatedPropertiesInternal(
            ref PlayableHandle playable)
        {
            return playable.IsValid()
                ? LegacyPlayableRuntime.GetAnimatedProperties(playable)
                : null;
        }

        internal static void SetAnimatedPropertiesInternal(
            ref PlayableHandle playable,
            AnimationClip animatedProperties)
        {
            if (!playable.IsValid())
                return;

            LegacyPlayableRuntime.SetAnimatedProperties(
                playable,
                animatedProperties);
        }
    }
}
