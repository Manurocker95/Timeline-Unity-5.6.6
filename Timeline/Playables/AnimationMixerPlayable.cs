namespace UnityEngine.Playables
{
    public class AnimationMixerPlayable : AnimationPlayable
    {
        public static AnimationMixerPlayable Create(
            PlayableGraph graph,
            int inputCount)
        {
            return Create(graph, inputCount, false);
        }

        public static AnimationMixerPlayable Create(
            PlayableGraph graph,
            int inputCount,
            bool normalizeWeights)
        {
            PlayableHandle handle =
                graph.CreateAnimationMixerPlayable(
                    inputCount,
                    normalizeWeights);

            return handle.IsValid()
                ? handle.GetObject<AnimationMixerPlayable>()
                : null;
        }
    }
}
