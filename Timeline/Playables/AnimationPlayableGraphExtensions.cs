using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Extends PlayableGraph for Animation.</para>
	/// </summary>
	public static class AnimationPlayableGraphExtensions
	{
        // Track outputs per graph.
        private static readonly Dictionary<PlayableGraph, List<PlayableOutput>> s_GraphOutputs =
            new Dictionary<PlayableGraph, List<PlayableOutput>>();

        // Track created handles by type (optional, but useful if other code inspects them).
        private static readonly HashSet<PlayableHandle> s_AnimClipHandles = new HashSet<PlayableHandle>();
        private static readonly HashSet<PlayableHandle> s_AnimMixerHandles = new HashSet<PlayableHandle>();
        private static readonly HashSet<PlayableHandle> s_AnimControllerHandles = new HashSet<PlayableHandle>();
        private static readonly HashSet<PlayableHandle> s_AnimOffsetHandles = new HashSet<PlayableHandle>();
        private static readonly HashSet<PlayableHandle> s_AnimMotionXToDeltaHandles = new HashSet<PlayableHandle>();
        private static readonly HashSet<PlayableHandle> s_AnimLayerMixerHandles = new HashSet<PlayableHandle>();

        // Simple monotonically increasing fake native pointer generator for handles/outputs.
        private static long s_NextPtr = 1;

        private static IntPtr NewPtr()
        {
            // Comment: Provide a unique, non-zero IntPtr for identity; no actual native resource exists in 5.6.
            long id = s_NextPtr++;
            return new IntPtr(unchecked((int)(id & 0x7FFFFFFF))); // keep it within 32-bit for safety in 5.6
        }

        private static List<PlayableOutput> GetOutputList(ref PlayableGraph graph, bool createIfMissing)
        {
            List<PlayableOutput> list;
            if (!s_GraphOutputs.TryGetValue(graph, out list) && createIfMissing)
            {
                list = new List<PlayableOutput>();
                s_GraphOutputs[graph] = list;
            }
            return list;
        }

        /// <summary>
        ///   <para>Creates an AnimationPlayableOutput in the PlayableGraph. When the AnimationPlayableOutput.sourcePlayable is set, the Animator will be playing the Playable.</para>
        /// </summary>
        /// <param name="name">The name of output.</param>
        /// <param name="target">The target that will Play the AnimationPlayableOutput.sourcePlayable.</param>
        /// <param name="graph">The PlayableGraph object.</param>
        /// <returns>
        ///   <para>A PlayableHandle on the created Playable.</para>
        /// </returns>
        public static AnimationPlayableOutput CreateAnimationOutput(this PlayableGraph graph, string name, Animator target)
		{
			AnimationPlayableOutput animationPlayableOutput = default(AnimationPlayableOutput);
			AnimationPlayableOutput result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationOutput(ref graph, name, out animationPlayableOutput.m_Output))
			{
				result = AnimationPlayableOutput.Null;
			}
			else
			{
				animationPlayableOutput.target = target;
				result = animationPlayableOutput;
			}
			return result;
		}

        private static bool InternalCreateAnimationOutput(ref PlayableGraph graph, string name, out PlayableOutput output)
        {
            // Comment: Simulate output creation with a unique handle.
            output = default(PlayableOutput);
            output.m_Handle = NewPtr();
            output.m_Version = 1;

            List<PlayableOutput> list = GetOutputList(ref graph, true);
            list.Add(output);
            return true;
        }

        internal static void SyncUpdateAndTimeMode(this PlayableGraph graph, Animator animator)
		{
			AnimationPlayableGraphExtensions.InternalSyncUpdateAndTimeMode(ref graph, animator);
		}

        internal static void InternalSyncUpdateAndTimeMode(ref PlayableGraph graph, Animator animator)
        {
            // Comment: No-op in 5.6 shim. In modern Unity this syncs Animator update/time mode to the graph.
        }

        /// <summary>
        ///   <para>Creates an AnimationClipPlayable in the PlayableGraph.</para>
        /// </summary>
        /// <param name="graph">The PlayableGraph object.</param>
        /// <param name="clip">The AnimationClip that will be added in the graph.</param>
        /// <returns>
        ///   <para>A PlayableHandle on the created Playable.</para>
        /// </returns>
        public static PlayableHandle CreateAnimationClipPlayable(this PlayableGraph graph, AnimationClip clip)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationClipPlayable(ref graph, clip, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		private static bool InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref graph, clip, ref handle);
		}

        private static bool INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
        {
            // Comment: Create a new handle and record it as an AnimationClip playable.
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimClipHandles.Add(handle);
            return true;
        }

        [ExcludeFromDocs]
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph, int inputCount)
		{
			bool normalizeWeights = false;
			return graph.CreateAnimationMixerPlayable(inputCount, normalizeWeights);
		}

		[ExcludeFromDocs]
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph)
		{
			bool normalizeWeights = false;
			int inputCount = 0;
			return graph.CreateAnimationMixerPlayable(inputCount, normalizeWeights);
		}

		/// <summary>
		///   <para>Creates an AnimationMixerPlayable in the PlayableGraph.</para>
		/// </summary>
		/// <param name="inputCount">The number of inputs that the mixer will update.</param>
		/// <param name="normalizeWeights">Set to true if you want the system to force a weight normalization of the inputs. If true, the sum of all the inputs weights will always be 1.0.</param>
		/// <param name="graph">The PlayableGraph object.</param>
		/// <returns>
		///   <para>A PlayableHandle on the created Playable.</para>
		/// </returns>
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph, [DefaultValue("0")] int inputCount, [DefaultValue("false")] bool normalizeWeights)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationMixerPlayable(ref graph, inputCount, normalizeWeights, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.inputCount = inputCount;
				result = @null;
			}
			return result;
		}

		private static bool InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref graph, inputCount, normalizeWeights, ref handle);
		}

        private static bool INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimMixerHandles.Add(handle);
            // Comment: inputCount normalization behavior is simulated by caller setting handle.inputCount.
            return true;
        }

        /// <summary>
        ///   <para>Creates an AnimatorControllerPlayable in the PlayableGraph.</para>
        /// </summary>
        /// <param name="controller">The RuntimeAnimatorController that will be added in the graph.</param>
        /// <param name="graph">The PlayableGraph object.</param>
        /// <returns>
        ///   <para>A PlayableHandle on the created Playable.</para>
        /// </returns>
        public static PlayableHandle CreateAnimatorControllerPlayable(this PlayableGraph graph, RuntimeAnimatorController controller)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimatorControllerPlayable(ref graph, controller, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		private static bool InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref graph, controller, ref handle);
		}

        private static bool INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimControllerHandles.Add(handle);
            return true;
        }

        internal static PlayableHandle CreateAnimationOffsetPlayable(this PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationOffsetPlayable(ref graph, position, rotation, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.inputCount = inputCount;
				result = @null;
			}
			return result;
		}

		private static bool InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref graph, ref position, ref rotation, ref handle);
		}

        private static bool INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimOffsetHandles.Add(handle);
            // Comment: position/rotation values should be stored by AnimationOffsetPlayable shim itself.
            return true;
        }

        internal static PlayableHandle CreateAnimationMotionXToDeltaPlayable(this PlayableGraph graph)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationMotionXToDeltaPlayable(ref graph, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.inputCount = 1;
				result = @null;
			}
			return result;
		}

		private static bool InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref graph, ref handle);
		}


        private static bool INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimMotionXToDeltaHandles.Add(handle);
            return true;
        }

        [ExcludeFromDocs]
		internal static PlayableHandle CreateAnimationLayerMixerPlayable(this PlayableGraph graph)
		{
			int inputCount = 0;
			return graph.CreateAnimationLayerMixerPlayable(inputCount);
		}

	
		internal static PlayableHandle CreateAnimationLayerMixerPlayable(this PlayableGraph graph, [DefaultValue("0")] int inputCount)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AnimationPlayableGraphExtensions.InternalCreateAnimationLayerMixerPlayable(ref graph, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				@null.inputCount = inputCount;
				result = @null;
			}
			return result;
		}


		private static bool InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref graph, ref handle);
		}


        private static bool INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            s_AnimLayerMixerHandles.Add(handle);
            return true;
        }


        private static void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutput output)
        {
            // Comment: Remove the output from the graph's list, if present.
            List<PlayableOutput> list = GetOutputList(ref graph, false);
            if (list == null) return;

            for (int i = 0; i < list.Count; i++)
            {
                // Comment: Compare by handle/version identity.
                if (list[i].m_Handle == output.m_Handle && list[i].m_Version == output.m_Version)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        ///   <para>Destroys the PlayableOutput.</para>
        /// </summary>
        /// <param name="output">The output that will be destroyed.</param>
        /// <param name="graph">The PlayableGraph object.</param>
        // Token: 0x060026FA RID: 9978 RVA: 0x0002C278 File Offset: 0x0002A478
        public static void DestroyOutput(this PlayableGraph graph, AnimationPlayableOutput output)
		{
			AnimationPlayableGraphExtensions.InternalDestroyOutput(ref graph, ref output.m_Output);
		}

		/// <summary>
		///   <para>Gets the number of AnimationPlayableOutput in the PlayableGraph.</para>
		/// </summary>
		/// <param name="graph"></param>
		public static int GetAnimationOutputCount(this PlayableGraph graph)
		{
			return AnimationPlayableGraphExtensions.InternalAnimationOutputCount(ref graph);
		}


        private static int InternalAnimationOutputCount(ref PlayableGraph graph)
        {
            List<PlayableOutput> list = GetOutputList(ref graph, false);
            if (list == null) return 0;
            return list.Count;
        }

        /// <summary>
        ///   <para>Returns the AnimationPlayableOutput at the given index.</para>
        /// </summary>
        /// <param name="index">The index of the AnimationPlayableOutput.</param>
        /// <param name="graph"></param>
        public static AnimationPlayableOutput GetAnimationOutput(this PlayableGraph graph, int index)
		{
			AnimationPlayableOutput animationPlayableOutput = default(AnimationPlayableOutput);
			AnimationPlayableOutput result;
			if (!AnimationPlayableGraphExtensions.InternalGetAnimationOutput(ref graph, index, out animationPlayableOutput.m_Output))
			{
				result = AnimationPlayableOutput.Null;
			}
			else
			{
				result = animationPlayableOutput;
			}
			return result;
		}


        private static bool InternalGetAnimationOutput(ref PlayableGraph graph, int index, out PlayableOutput output)
        {
            // Comment: Return the indexed output from the graph's list if within range.
            output = default(PlayableOutput);
            List<PlayableOutput> list = GetOutputList(ref graph, false);
            if (list == null) return false;
            if (index < 0 || index >= list.Count) return false;

            output = list[index];
            return true;
        }
    }
}
