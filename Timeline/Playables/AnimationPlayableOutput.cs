using System;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed animation output that binds a playable to an Animator.
    /// </summary>
    public struct AnimationPlayableOutput
    {
        public static AnimationPlayableOutput Create(
            PlayableGraph graph,
            string name,
            Animator target)
        {
            return graph.CreateAnimationOutput(
                name,
                target);
        }

        public static AnimationPlayableOutput Null
        {
            get
            {
                AnimationPlayableOutput output =
                    default(AnimationPlayableOutput);

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

        public Animator target
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationOutputTarget(
                    m_Output);
            }
            set
            {
                LegacyPlayableRuntime.SetAnimationOutputTarget(
                    m_Output,
                    value);
            }
        }

        public float weight
        {
            get
            {
                return PlayableOutput.InternalGetWeight(ref m_Output);
            }
            set
            {
                PlayableOutput.InternalSetWeight(ref m_Output, value);
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

        public void SetSourcePlayable(Playable playable)
        {
            sourcePlayable = playable != null
                ? playable.handle
                : PlayableHandle.Null;
        }

        public void SetSourcePlayable(PlayableHandle playable)
        {
            sourcePlayable = playable;
        }

        public PlayableHandle GetSourcePlayable()
        {
            return sourcePlayable;
        }

        public void SetTarget(Animator animator)
        {
            target = animator;
        }

        public Animator GetTarget()
        {
            return target;
        }

        internal PlayableOutput m_Output;
    }
}
