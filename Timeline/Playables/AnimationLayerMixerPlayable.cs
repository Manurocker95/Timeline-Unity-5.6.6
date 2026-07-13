namespace UnityEngine.Playables
{
    public class AnimationLayerMixerPlayable : AnimationPlayable
    {
        public static AnimationLayerMixerPlayable Create(
            PlayableGraph graph,
            int inputCount)
        {
            PlayableHandle handle =
                graph.CreateAnimationLayerMixerPlayable(inputCount);

            if (!handle.IsValid())
                return null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationLayerMixerPlayable));

            return handle.GetObject<AnimationLayerMixerPlayable>();
        }

        public void SetLayerMaskFromAvatarMask(
            uint layerIndex,
            AvatarMask mask)
        {
            LegacyPlayableRuntime.SetAnimationLayerMask(
                handle,
                layerIndex,
                mask);
        }

        public AvatarMask GetLayerMaskFromAvatarMask(
            uint layerIndex)
        {
            return LegacyPlayableRuntime.GetAnimationLayerMask(
                handle,
                layerIndex);
        }

        public void SetLayerAdditive(
            uint layerIndex,
            bool value)
        {
            LegacyPlayableRuntime.SetAnimationLayerAdditive(
                handle,
                layerIndex,
                value);
        }

        public bool IsLayerAdditive(uint layerIndex)
        {
            return LegacyPlayableRuntime.GetAnimationLayerAdditive(
                handle,
                layerIndex);
        }
    }
}
