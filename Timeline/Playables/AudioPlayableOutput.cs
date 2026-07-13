using System;
using UnityEngine.Audio;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed audio output for the standalone Legacy Timeline runtime.
    /// </summary>
    public struct AudioPlayableOutput
    {
        public static AudioPlayableOutput Null
        {
            get
            {
                AudioPlayableOutput output =
                    default(AudioPlayableOutput);

                output.m_Output = new PlayableOutput
                {
                    m_Handle = IntPtr.Zero,
                    m_Version = 69
                };

                return output;
            }
        }

        internal UnityEngine.Object referenceObject
        {
            get
            {
                return PlayableOutput.GetInternalReferenceObject(
                    ref m_Output);
            }
            set
            {
                PlayableOutput.SetInternalReferenceObject(
                    ref m_Output,
                    value);
            }
        }

        public UnityEngine.Object userData
        {
            get
            {
                return PlayableOutput.GetInternalUserData(
                    ref m_Output);
            }
            set
            {
                PlayableOutput.SetInternalUserData(
                    ref m_Output,
                    value);
            }
        }

        public bool IsValid()
        {
            return PlayableOutput.IsValidInternal(ref m_Output);
        }

        public AudioMixerGroup target
        {
            get
            {
                return LegacyPlayableRuntime.GetAudioOutputTarget(m_Output);
            }
            set
            {
                LegacyPlayableRuntime.SetAudioOutputTarget(
                    m_Output,
                    value);
            }
        }

        public PlayableHandle sourcePlayable
        {
            get
            {
                return PlayableOutput.InternalGetSourcePlayable(
                    ref m_Output);
            }
            set
            {
                PlayableOutput.InternalSetSourcePlayable(
                    ref m_Output,
                    ref value);
            }
        }

        public int sourceInputPort
        {
            get
            {
                return PlayableOutput.InternalGetSourceInputPort(
                    ref m_Output);
            }
            set
            {
                PlayableOutput.InternalSetSourceInputPort(
                    ref m_Output,
                    value);
            }
        }

        internal PlayableOutput m_Output;
    }
}
