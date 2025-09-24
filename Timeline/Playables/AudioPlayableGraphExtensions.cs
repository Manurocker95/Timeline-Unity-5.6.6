using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Internal;
using UnityEngine.Playables.Audio;
using UnityEngine.Scripting;
using UnityEngine.Experimental.Director;



namespace UnityEngine.Playables
{

    /// <summary>
    ///   <para>Extends PlayableGraph for Audio.</para>
    /// </summary>
    public static class AudioPlayableGraphExtensions
    {
        private static int s_NextId = 1;

        // Comment: Generate a unique, non-zero IntPtr to simulate a native handle.
        private static IntPtr NewPtr()
        {
            // Keep within 31-bit positive range for safety in old runtimes.
            int id = s_NextId++;
            return new IntPtr(id & 0x3FFFFFFF);
        }
        /// <summary>
        ///   <para>Creates an AudioPlayableOutput on the graph.</para>
        /// </summary>
        /// <param name="graph">The PlayableGraph object.</param>
        /// <param name="name">An indentifier for the output.</param>
        /// <param name="target">An optional mixer group to bind the output to.</param>
        /// <returns>
        ///   <para>Handle to the output created.</para>
        /// </returns>
        public static AudioPlayableOutput CreateAudioOutput(this PlayableGraph graph, string name, AudioMixerGroup target)
		{
			AudioPlayableOutput audioPlayableOutput = default(AudioPlayableOutput);
			AudioPlayableOutput result;
			if (!AudioPlayableGraphExtensions.InternalCreateAudioOutput(ref graph, name, out audioPlayableOutput.m_Output))
			{
				result = AudioPlayableOutput.Null;
			}
			else
			{
				audioPlayableOutput.target = target;
				result = audioPlayableOutput;
			}
			return result;
		}

        private static bool InternalCreateAudioOutput(ref PlayableGraph graph, string name, out PlayableOutput output)
        {
            output = default(PlayableOutput);
            output.m_Handle = NewPtr();
            output.m_Version = 1;
            return true;
        }

        /// <summary>
        ///   <para>Destroys the PlayableOutput.</para>
        /// </summary>
        /// <param name="graph">The output that will be destroyed.</param>
        /// <param name="output">The PlayableGraph object.</param>
        // Token: 0x06002332 RID: 9010 RVA: 0x00028888 File Offset: 0x00026A88
        public static void DestroyOutput(this PlayableGraph graph, AudioPlayableOutput output)
		{
			PlayableGraph.InternalDestroyOutput(ref graph, ref output.m_Output);
		}

		/// <summary>
		///   <para>Creates an AudioClipPlayable in the PlayableGraph.</para>
		/// </summary>
		/// <param name="graph">The PlayableGraph object.</param>
		/// <param name="clip">The AudioClip to play.</param>
		/// <param name="looping">Whether to allow the audio clip to loop.</param>
		/// <returns>
		///   <para>A PlayableHandle on the created Playable.</para>
		/// </returns>
		// Token: 0x06002333 RID: 9011 RVA: 0x0002889C File Offset: 0x00026A9C
		public static PlayableHandle CreateAudioClipPlayable(this PlayableGraph graph, AudioClip clip, bool looping)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AudioPlayableGraphExtensions.InternalCreateAudioClipPlayable(ref graph, clip, looping, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000288D4 File Offset: 0x00026AD4
		private static bool InternalCreateAudioClipPlayable(ref PlayableGraph graph, AudioClip clip, bool looping, ref PlayableHandle handle)
		{
			return AudioPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAudioClipPlayable(ref graph, clip, looping, ref handle);
		}

        private static bool INTERNAL_CALL_InternalCreateAudioClipPlayable(ref PlayableGraph graph, AudioClip clip, bool looping, ref PlayableHandle handle)
        {
            handle.m_Handle = NewPtr();
            handle.m_Version = 1;
            return true;
        }

        // Token: 0x06002336 RID: 9014 RVA: 0x000288F4 File Offset: 0x00026AF4
        [ExcludeFromDocs]
		public static PlayableHandle CreateAudioMixerPlayable(this PlayableGraph graph, int inputCount)
		{
			bool normalizeInputVolumes = false;
			return graph.CreateAudioMixerPlayable(inputCount, normalizeInputVolumes);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x00028914 File Offset: 0x00026B14
		[ExcludeFromDocs]
		public static PlayableHandle CreateAudioMixerPlayable(this PlayableGraph graph)
		{
			bool normalizeInputVolumes = false;
			int inputCount = 0;
			return graph.CreateAudioMixerPlayable(inputCount, normalizeInputVolumes);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x00028938 File Offset: 0x00026B38
		public static PlayableHandle CreateAudioMixerPlayable(this PlayableGraph graph, [DefaultValue("0")] int inputCount, [DefaultValue("false")] bool normalizeInputVolumes)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AudioPlayableGraphExtensions.InternalCreateAudioMixerPlayable(ref graph, inputCount, normalizeInputVolumes, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x00028970 File Offset: 0x00026B70
		private static bool InternalCreateAudioMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeInputVolumes, ref PlayableHandle handle)
		{
			return AudioPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAudioMixerPlayable(ref graph, inputCount, normalizeInputVolumes, ref handle);
		}

		// Token: 0x0600233A RID: 9018
		//[GeneratedByOldBindingsGenerator]
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAudioMixerPlayable(ref PlayableGraph graph, int inputCount, bool normalizeInputVolumes, ref PlayableHandle handle);

		// Token: 0x0600233B RID: 9019 RVA: 0x00028990 File Offset: 0x00026B90
		public static PlayableHandle CreateAudioDSPPlayable(this PlayableGraph graph, BuiltinDSPType dspType, ScriptableObject driver, params DSPFloatParameter[] dspParam)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AudioPlayableGraphExtensions.InternalCreateAudioDSPPlayableSO(ref graph, dspType, driver, dspParam, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000289C8 File Offset: 0x00026BC8
		private static bool InternalCreateAudioDSPPlayableSO(ref PlayableGraph graph, BuiltinDSPType dspType,  ScriptableObject driver, DSPFloatParameter[] dspParam, ref PlayableHandle handle)
		{
			return AudioPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAudioDSPPlayableSO(ref graph, dspType, driver, dspParam, ref handle);
		}

        // Token: 0x0600233D RID: 9021
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAudioDSPPlayableSO(ref PlayableGraph graph, BuiltinDSPType dspType,  ScriptableObject driver, DSPFloatParameter[] dspParam, ref PlayableHandle handle);

		// Token: 0x0600233E RID: 9022 RVA: 0x000289E8 File Offset: 0x00026BE8
		public static PlayableHandle CreateAudioDSPPlayable(this PlayableGraph graph, BuiltinDSPType dspType, MonoBehaviour driver, params DSPFloatParameter[] dspParam)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!AudioPlayableGraphExtensions.InternalCreateAudioDSPPlayableMB(ref graph, dspType, driver, dspParam, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x00028A20 File Offset: 0x00026C20
		private static bool InternalCreateAudioDSPPlayableMB(ref PlayableGraph graph, BuiltinDSPType dspType,  MonoBehaviour driver, DSPFloatParameter[] dspParam, ref PlayableHandle handle)
		{
			return AudioPlayableGraphExtensions.INTERNAL_CALL_InternalCreateAudioDSPPlayableMB(ref graph, dspType, driver, dspParam, ref handle);
		}

        // Token: 0x06002340 RID: 9024
        //[GeneratedByOldBindingsGenerator]
        [MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreateAudioDSPPlayableMB(ref PlayableGraph graph, BuiltinDSPType dspType,  MonoBehaviour driver, DSPFloatParameter[] dspParam, ref PlayableHandle handle);
	}
}
