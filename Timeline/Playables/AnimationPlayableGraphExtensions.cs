using UnityEngine.Internal;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Standalone managed animation extensions for Legacy Timeline.
    /// </summary>
    public static class AnimationPlayableGraphExtensions
    {
        public static AnimationPlayableOutput CreateAnimationOutput(
            this PlayableGraph graph,
            string name,
            Animator target)
        {
            AnimationPlayableOutput output =
                default(AnimationPlayableOutput);

            if (!LegacyPlayableRuntime.CreateAnimationOutput(
                graph,
                name,
                out output.m_Output))
            {
                return AnimationPlayableOutput.Null;
            }

            output.target = target;
            return output;
        }

        internal static void SyncUpdateAndTimeMode(
            this PlayableGraph graph,
            Animator animator)
        {
            // The standalone runtime is evaluated explicitly and does not
            // inherit Animator update settings from Unity's native graph.
        }

        public static PlayableHandle CreateAnimationClipPlayable(
            this PlayableGraph graph,
            AnimationClip clip)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationClipPlayable));

            LegacyPlayableRuntime.SetAnimationClip(handle, clip);

            if (clip != null)
                handle.duration = clip.length;

            return handle;
        }

        [ExcludeFromDocs]
        public static PlayableHandle CreateAnimationMixerPlayable(
            this PlayableGraph graph,
            int inputCount)
        {
            return CreateAnimationMixerPlayable(
                graph,
                inputCount,
                false);
        }

        [ExcludeFromDocs]
        public static PlayableHandle CreateAnimationMixerPlayable(
            this PlayableGraph graph)
        {
            return CreateAnimationMixerPlayable(graph, 0, false);
        }

        public static PlayableHandle CreateAnimationMixerPlayable(
            this PlayableGraph graph,
            [DefaultValue("0")] int inputCount,
            [DefaultValue("false")] bool normalizeWeights)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationMixerPlayable));

            LegacyPlayableRuntime.SetAnimationMixerNormalizeWeights(
                handle,
                normalizeWeights);

            handle.inputCount = inputCount;
            return handle;
        }

        public static PlayableHandle CreateAnimatorControllerPlayable(
            this PlayableGraph graph,
            RuntimeAnimatorController controller)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimatorControllerPlayable));

            LegacyPlayableRuntime.SetAnimatorController(
                handle,
                controller);

            return handle;
        }

        internal static PlayableHandle CreateAnimationOffsetPlayable(
            this PlayableGraph graph,
            Vector3 position,
            Quaternion rotation,
            int inputCount)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationOffsetPlayable));

            LegacyPlayableRuntime.SetAnimationOffset(
                handle,
                position,
                rotation);

            handle.inputCount = inputCount;
            return handle;
        }

        internal static PlayableHandle CreateAnimationMotionXToDeltaPlayable(
            this PlayableGraph graph)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationPlayable));

            LegacyPlayableRuntime.SetAnimationMotionXToDelta(
                handle,
                true);

            handle.inputCount = 1;
            return handle;
        }

        [ExcludeFromDocs]
        internal static PlayableHandle CreateAnimationLayerMixerPlayable(
            this PlayableGraph graph)
        {
            return CreateAnimationLayerMixerPlayable(graph, 0);
        }

        internal static PlayableHandle CreateAnimationLayerMixerPlayable(
            this PlayableGraph graph,
            [DefaultValue("0")] int inputCount)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AnimationMixerPlayable));

            LegacyPlayableRuntime.SetAnimationLayerMixer(
                handle,
                true);

            handle.inputCount = inputCount;
            return handle;
        }

        public static void DestroyOutput(
            this PlayableGraph graph,
            AnimationPlayableOutput output)
        {
            PlayableGraph.InternalDestroyOutput(
                ref graph,
                ref output.m_Output);
        }

        public static int GetAnimationOutputCount(
            this PlayableGraph graph)
        {
            return LegacyPlayableRuntime.GetAnimationOutputCount(graph);
        }

        public static AnimationPlayableOutput GetAnimationOutput(
            this PlayableGraph graph,
            int index)
        {
            AnimationPlayableOutput output =
                default(AnimationPlayableOutput);

            if (!LegacyPlayableRuntime.GetAnimationOutput(
                graph,
                index,
                out output.m_Output))
            {
                return AnimationPlayableOutput.Null;
            }

            return output;
        }
    }
}
