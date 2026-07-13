using UnityEngine.Audio;
using UnityEngine.Internal;
using UnityEngine.Playables.Audio;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed audio extensions for the standalone Legacy Timeline runtime.
    /// </summary>
    public static class AudioPlayableGraphExtensions
    {
        public static AudioPlayableOutput CreateAudioOutput(
            this PlayableGraph graph,
            string name,
            AudioMixerGroup target)
        {
            AudioPlayableOutput output =
                default(AudioPlayableOutput);

            if (!LegacyPlayableRuntime.CreateAudioOutput(
                graph,
                name,
                out output.m_Output))
            {
                return AudioPlayableOutput.Null;
            }

            output.target = target;
            return output;
        }

        public static void DestroyOutput(
            this PlayableGraph graph,
            AudioPlayableOutput output)
        {
            PlayableGraph.InternalDestroyOutput(
                ref graph,
                ref output.m_Output);
        }

        public static PlayableHandle CreateAudioClipPlayable(
            this PlayableGraph graph,
            AudioClip clip,
            bool looping)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AudioClipPlayable));

            LegacyPlayableRuntime.SetAudioClip(handle, clip);
            LegacyPlayableRuntime.SetAudioLooped(handle, looping);

            if (clip != null)
                handle.duration = clip.length;

            return handle;
        }

        [ExcludeFromDocs]
        public static PlayableHandle CreateAudioMixerPlayable(
            this PlayableGraph graph,
            int inputCount)
        {
            return CreateAudioMixerPlayable(
                graph,
                inputCount,
                false);
        }

        [ExcludeFromDocs]
        public static PlayableHandle CreateAudioMixerPlayable(
            this PlayableGraph graph)
        {
            return CreateAudioMixerPlayable(graph, 0, false);
        }

        public static PlayableHandle CreateAudioMixerPlayable(
            this PlayableGraph graph,
            [DefaultValue("0")] int inputCount,
            [DefaultValue("false")] bool normalizeInputVolumes)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetPlayableType(
                handle,
                typeof(AudioMixerPlayable));

            LegacyPlayableRuntime.SetAudioMixerNormalizeVolumes(
                handle,
                normalizeInputVolumes);

            handle.inputCount = inputCount;
            return handle;
        }

        public static PlayableHandle CreateAudioDSPPlayable(
            this PlayableGraph graph,
            BuiltinDSPType dspType,
            ScriptableObject driver,
            params DSPFloatParameter[] dspParam)
        {
            return CreateManagedDSPPlayable(
                graph,
                dspType,
                driver,
                dspParam);
        }

        public static PlayableHandle CreateAudioDSPPlayable(
            this PlayableGraph graph,
            BuiltinDSPType dspType,
            MonoBehaviour driver,
            params DSPFloatParameter[] dspParam)
        {
            return CreateManagedDSPPlayable(
                graph,
                dspType,
                driver,
                dspParam);
        }

        private static PlayableHandle CreateManagedDSPPlayable(
            PlayableGraph graph,
            BuiltinDSPType dspType,
            UnityEngine.Object driver,
            DSPFloatParameter[] dspParam)
        {
            PlayableHandle handle = graph.CreatePlayable();

            if (!handle.IsValid())
                return PlayableHandle.Null;

            LegacyPlayableRuntime.SetAudioDSPData(
                handle,
                dspType,
                driver,
                dspParam);

            return handle;
        }
    }
}
