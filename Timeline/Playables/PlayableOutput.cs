using System;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed output handle used by the standalone Legacy Timeline runtime.
    /// </summary>
    internal struct PlayableOutput
    {
        internal static bool IsValidInternal(ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.IsOutputValid(output);
        }

        internal static UnityEngine.Object GetInternalReferenceObject(
            ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetOutputReferenceObject(output);
        }

        internal static void SetInternalReferenceObject(
            ref PlayableOutput output,
            UnityEngine.Object target)
        {
            LegacyPlayableRuntime.SetOutputReferenceObject(output, target);
        }

        internal static UnityEngine.Object GetInternalUserData(
            ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetOutputUserData(output);
        }

        internal static void SetInternalUserData(
            ref PlayableOutput output,
            UnityEngine.Object target)
        {
            LegacyPlayableRuntime.SetOutputUserData(output, target);
        }

        internal static PlayableHandle InternalGetSourcePlayable(
            ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetOutputSourcePlayable(output);
        }

        internal static void InternalSetSourcePlayable(
            ref PlayableOutput output,
            ref PlayableHandle target)
        {
            LegacyPlayableRuntime.SetOutputSourcePlayable(output, target);
        }

        internal static int InternalGetSourceInputPort(
            ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetOutputSourceInputPort(output);
        }

        internal static void InternalSetSourceInputPort(
            ref PlayableOutput output,
            int port)
        {
            LegacyPlayableRuntime.SetOutputSourceInputPort(output, port);
        }

        internal static void InternalSetWeight(
            ref PlayableOutput output,
            float weight)
        {
            LegacyPlayableRuntime.SetOutputWeight(output, weight);
        }

        internal static float InternalGetWeight(
            ref PlayableOutput output)
        {
            return LegacyPlayableRuntime.GetOutputWeight(output);
        }

        internal IntPtr m_Handle;
        internal int m_Version;
    }
}
