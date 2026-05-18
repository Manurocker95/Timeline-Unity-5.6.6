using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Extends PlayableGraph for Animation.</para>
	/// </summary>
	// Token: 0x02000265 RID: 613
	public static class AnimationPlayableGraphExtensions
	{
		/// <summary>
		///   <para>Creates an AnimationPlayableOutput in the PlayableGraph. When the AnimationPlayableOutput.sourcePlayable is set, the Animator will be playing the Playable.</para>
		/// </summary>
		/// <param name="name">The name of output.</param>
		/// <param name="target">The target that will Play the AnimationPlayableOutput.sourcePlayable.</param>
		/// <param name="graph">The PlayableGraph object.</param>
		/// <returns>
		///   <para>A PlayableHandle on the created Playable.</para>
		/// </returns>
		// Token: 0x060026E0 RID: 9952 RVA: 0x0002BF98 File Offset: 0x0002A198
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

		// Token: 0x060026E1 RID: 9953
		
		[MethodImpl(4096)]
		private static extern bool InternalCreateAnimationOutput(ref PlayableGraph graph, string name, out PlayableOutput output);

		// Token: 0x060026E2 RID: 9954 RVA: 0x0002BFE0 File Offset: 0x0002A1E0
		internal static void SyncUpdateAndTimeMode(this PlayableGraph graph, Animator animator)
		{
			AnimationPlayableGraphExtensions.InternalSyncUpdateAndTimeMode(ref graph, animator);
		}

		// Token: 0x060026E3 RID: 9955
		
		[MethodImpl(4096)]
		internal static extern void InternalSyncUpdateAndTimeMode(ref PlayableGraph graph, Animator animator);

		/// <summary>
		///   <para>Creates an AnimationClipPlayable in the PlayableGraph.</para>
		/// </summary>
		/// <param name="graph">The PlayableGraph object.</param>
		/// <param name="clip">The AnimationClip that will be added in the graph.</param>
		/// <returns>
		///   <para>A PlayableHandle on the created Playable.</para>
		/// </returns>
		// Token: 0x060026E4 RID: 9956 RVA: 0x0002BFEC File Offset: 0x0002A1EC
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

		// Token: 0x060026E5 RID: 9957 RVA: 0x0002C024 File Offset: 0x0002A224
		private static bool InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref graph, clip, ref handle);
		}

		// Token: 0x060026E6 RID: 9958
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationClipPlayable(ref PlayableGraph graph, AnimationClip clip, ref PlayableHandle handle);

		// Token: 0x060026E7 RID: 9959 RVA: 0x0002C044 File Offset: 0x0002A244
		[ExcludeFromDocs]
		public static PlayableHandle CreateAnimationMixerPlayable(this PlayableGraph graph, int inputCount)
		{
			bool normalizeWeights = false;
			return graph.CreateAnimationMixerPlayable(inputCount, normalizeWeights);
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0002C064 File Offset: 0x0002A264
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
		// Token: 0x060026E9 RID: 9961 RVA: 0x0002C088 File Offset: 0x0002A288
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

		// Token: 0x060026EA RID: 9962 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
		private static bool InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref graph, inputCount, normalizeWeights, ref handle);
		}

		// Token: 0x060026EB RID: 9963
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeWeights, ref PlayableHandle handle);

		/// <summary>
		///   <para>Creates an AnimatorControllerPlayable in the PlayableGraph.</para>
		/// </summary>
		/// <param name="controller">The RuntimeAnimatorController that will be added in the graph.</param>
		/// <param name="graph">The PlayableGraph object.</param>
		/// <returns>
		///   <para>A PlayableHandle on the created Playable.</para>
		/// </returns>
		// Token: 0x060026EC RID: 9964 RVA: 0x0002C0E8 File Offset: 0x0002A2E8
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

		// Token: 0x060026ED RID: 9965 RVA: 0x0002C120 File Offset: 0x0002A320
		private static bool InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref graph, controller, ref handle);
		}

		// Token: 0x060026EE RID: 9966
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimatorControllerPlayable(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle);

		// Token: 0x060026EF RID: 9967 RVA: 0x0002C140 File Offset: 0x0002A340
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

		// Token: 0x060026F0 RID: 9968 RVA: 0x0002C180 File Offset: 0x0002A380
		private static bool InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref graph, ref position, ref rotation, ref handle);
		}

		// Token: 0x060026F1 RID: 9969
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationOffsetPlayable(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle);

		// Token: 0x060026F2 RID: 9970 RVA: 0x0002C1A0 File Offset: 0x0002A3A0
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

		// Token: 0x060026F3 RID: 9971 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		private static bool InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref graph, ref handle);
		}

		// Token: 0x060026F4 RID: 9972
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationMotionXToDeltaPlayable(ref PlayableGraph graph, ref PlayableHandle handle);

		// Token: 0x060026F5 RID: 9973 RVA: 0x0002C1FC File Offset: 0x0002A3FC
		[ExcludeFromDocs]
		internal static PlayableHandle CreateAnimationLayerMixerPlayable(this PlayableGraph graph)
		{
			int inputCount = 0;
			return graph.CreateAnimationLayerMixerPlayable(inputCount);
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x0002C21C File Offset: 0x0002A41C
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

		// Token: 0x060026F7 RID: 9975 RVA: 0x0002C25C File Offset: 0x0002A45C
		private static bool InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return AnimationPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref graph, ref handle);
		}

		// Token: 0x060026F8 RID: 9976
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAnimationLayerMixerPlayable(ref PlayableGraph graph, ref PlayableHandle handle);

		// Token: 0x060026F9 RID: 9977
		
		[MethodImpl(4096)]
		private static extern void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutput output);

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
		// Token: 0x060026FB RID: 9979 RVA: 0x0002C28C File Offset: 0x0002A48C
		public static int GetAnimationOutputCount(this PlayableGraph graph)
		{
			return AnimationPlayableGraphExtensions.InternalAnimationOutputCount(ref graph);
		}

		// Token: 0x060026FC RID: 9980
		
		[MethodImpl(4096)]
		private static extern int InternalAnimationOutputCount(ref PlayableGraph graph);

		/// <summary>
		///   <para>Returns the AnimationPlayableOutput at the given index.</para>
		/// </summary>
		/// <param name="index">The index of the AnimationPlayableOutput.</param>
		/// <param name="graph"></param>
		// Token: 0x060026FD RID: 9981 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
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

		// Token: 0x060026FE RID: 9982
		
		[MethodImpl(4096)]
		private static extern bool InternalGetAnimationOutput(ref PlayableGraph graph, int index, out PlayableOutput output);
	}
}
