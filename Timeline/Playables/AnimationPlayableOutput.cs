using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Animation output for the PlayableGraph.  Defines how a Playable is connected to an Animator.</para>
	/// </summary>
	public struct AnimationPlayableOutput
	{
        private static readonly Dictionary<PlayableOutput, Animator> s_Targets = new Dictionary<PlayableOutput, Animator>();

        /// <summary>
        ///   <para>Used to compare against AnimationPlayableOutput instances to check their validity.</para>
        /// </summary>
        public static AnimationPlayableOutput Null
		{
			get
			{
				return new AnimationPlayableOutput
				{
					m_Output = new PlayableOutput
					{
						m_Version = 69
					}
				};
			}
		}

		internal Object referenceObject
		{
			get
			{
				return PlayableOutput.GetInternalReferenceObject(ref this.m_Output);
			}
			set
			{
				PlayableOutput.SetInternalReferenceObject(ref this.m_Output, value);
			}
		}

		/// <summary>
		///   <para>Used to pass custom data to ScriptPlayable.ProcessFrame.</para>
		/// </summary>
		public Object userData
		{
			get
			{
				return PlayableOutput.GetInternalUserData(ref this.m_Output);
			}
			set
			{
				PlayableOutput.SetInternalUserData(ref this.m_Output, value);
			}
		}

		/// <summary>
		///   <para>Returns true if the PlayableOutput has been properly constructed by the PlayableGraph and has not been destroyed.</para>
		/// </summary>
		public bool IsValid()
		{
			return PlayableOutput.IsValidInternal(ref this.m_Output);
		}

		/// <summary>
		///   <para>The Animator component that is bound to this output.</para>
		/// </summary>
		public Animator target
		{
			get
			{
				return AnimationPlayableOutput.InternalGetTarget(ref this.m_Output);
			}
			set
			{
				AnimationPlayableOutput.InternalSetTarget(ref this.m_Output, value);
			}
		}

		/// <summary>
		///   <para>The blend weight of the sourcePlayable to the animator.</para>
		/// </summary>
		public float weight
		{
			get
			{
				return PlayableOutput.InternalGetWeight(ref this.m_Output);
			}
			set
			{
				PlayableOutput.InternalSetWeight(ref this.m_Output, value);
			}
		}

        private static Animator InternalGetTarget(ref PlayableOutput output)
        {
            Animator a;
            if (s_Targets.TryGetValue(output, out a))
                return a;
            return null;
        }

        private static void InternalSetTarget(ref PlayableOutput output, Animator target)
        {
            s_Targets[output] = target;
        }

        /// <summary>
        ///   <para>The Playable that is bound to the output.</para>
        /// </summary>
        public PlayableHandle sourcePlayable
		{
			get
			{
				return PlayableOutput.InternalGetSourcePlayable(ref this.m_Output);
			}
			set
			{
				PlayableOutput.InternalSetSourcePlayable(ref this.m_Output, ref value);
			}
		}

		public int sourceInputPort
		{
			get
			{
				return PlayableOutput.InternalGetSourceInputPort(ref this.m_Output);
			}
			set
			{
				PlayableOutput.InternalSetSourceInputPort(ref this.m_Output, value);
			}
		}

		internal PlayableOutput m_Output;
	}
}
