using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>The PlayableGraph is used to manage PlayableHandle creation, destruction and connections.</para>
	/// </summary>
	// Token: 0x020000EC RID: 236
	
	public struct PlayableGraph
	{
		/// <summary>
		///   <para>Returns true if the PlayableGraph has been properly constructed using PlayableGraph.CreateGraph and is not deleted.</para>
		/// </summary>
		// Token: 0x060010E6 RID: 4326 RVA: 0x000169D4 File Offset: 0x00014BD4
		public bool IsValid()
		{
			return PlayableGraph.IsValidInternal(ref this);
		}

		// Token: 0x060010E7 RID: 4327
		
		[MethodImpl(4096)]
		private static extern bool IsValidInternal(ref PlayableGraph graph);

		/// <summary>
		///   <para>Creates a PlayableGraph.</para>
		/// </summary>
		/// <returns>
		///   <para>The created graph.</para>
		/// </returns>
		// Token: 0x060010E8 RID: 4328 RVA: 0x000169F0 File Offset: 0x00014BF0
		public static PlayableGraph CreateGraph()
		{
			PlayableGraph result = default(PlayableGraph);
			PlayableGraph.InternalCreate(ref result);
			return result;
		}

		// Token: 0x060010E9 RID: 4329
		
		[MethodImpl(4096)]
		internal static extern void InternalCreate(ref PlayableGraph graph);

		/// <summary>
		///   <para>Indicates that a graph has completed its operations.</para>
		/// </summary>
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x00016A18 File Offset: 0x00014C18
		public bool isDone
		{
			get
			{
				return PlayableGraph.InternalIsDone(ref this);
			}
		}

		// Token: 0x060010EB RID: 4331
		
		[MethodImpl(4096)]
		internal static extern bool InternalIsDone(ref PlayableGraph graph);

		/// <summary>
		///   <para>Property Table used to resolve ExposedReferences.</para>
		/// </summary>
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00016A34 File Offset: 0x00014C34
		// (set) Token: 0x060010ED RID: 4333 RVA: 0x00016A50 File Offset: 0x00014C50
		public IExposedPropertyTable resolver
		{
			get
			{
				return PlayableGraph.InternalGetResolver(ref this);
			}
			set
			{
				PlayableGraph.InternalSetResolver(ref this, value);
			}
		}

		// Token: 0x060010EE RID: 4334
		
		[MethodImpl(4096)]
		internal static extern IExposedPropertyTable InternalGetResolver(ref PlayableGraph graph);

		// Token: 0x060010EF RID: 4335
		
		[MethodImpl(4096)]
		internal static extern void InternalSetResolver(ref PlayableGraph graph, IExposedPropertyTable resolver);

		/// <summary>
		///   <para>Plays the graph.</para>
		/// </summary>
		// Token: 0x060010F0 RID: 4336 RVA: 0x00016A5C File Offset: 0x00014C5C
		public void Play()
		{
			PlayableGraph.InternalPlay(ref this);
		}

		// Token: 0x060010F1 RID: 4337
		
		[MethodImpl(4096)]
		internal static extern void InternalPlay(ref PlayableGraph graph);

		/// <summary>
		///   <para>Stops the graph, if it is playing.</para>
		/// </summary>
		// Token: 0x060010F2 RID: 4338 RVA: 0x00016A68 File Offset: 0x00014C68
		public void Stop()
		{
			PlayableGraph.InternalStop(ref this);
		}

		// Token: 0x060010F3 RID: 4339
		
		[MethodImpl(4096)]
		internal static extern void InternalStop(ref PlayableGraph graph);

		/// <summary>
		///   <para>Returns the number of PlayableHandle owned by the Graph.</para>
		/// </summary>
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00016A74 File Offset: 0x00014C74
		public int playableCount
		{
			get
			{
				return PlayableGraph.InternalPlayableCount(ref this);
			}
		}

		// Token: 0x060010F5 RID: 4341
		
		[MethodImpl(4096)]
		internal static extern int InternalPlayableCount(ref PlayableGraph graph);

		/// <summary>
		///   <para>Creates a ScriptPlayableOutput in the [PlayableGraph]].</para>
		/// </summary>
		/// <param name="name">The name of the output.</param>
		// Token: 0x060010F6 RID: 4342 RVA: 0x00016A90 File Offset: 0x00014C90
		public ScriptPlayableOutput CreateScriptOutput(string name)
		{
			ScriptPlayableOutput scriptPlayableOutput = default(ScriptPlayableOutput);
			ScriptPlayableOutput result;
			if (!PlayableGraph.InternalCreateScriptOutput(ref this, name, out scriptPlayableOutput.m_Output))
			{
				result = ScriptPlayableOutput.Null;
			}
			else
			{
				result = scriptPlayableOutput;
			}
			return result;
		}

		// Token: 0x060010F7 RID: 4343
		
		[MethodImpl(4096)]
		private static extern bool InternalCreateScriptOutput(ref PlayableGraph graph, string name, out PlayableOutput output);

		/// <summary>
		///   <para>This method allows you to create custom Playable instances.</para>
		/// </summary>
		/// <returns>
		///   <para>The created Playable.</para>
		/// </returns>
		// Token: 0x060010F8 RID: 4344 RVA: 0x00016ACC File Offset: 0x00014CCC
		public PlayableHandle CreatePlayable()
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!PlayableGraph.InternalCreatePlayable(ref this, ref @null))
			{
				result = PlayableHandle.Null;
			}
			else
			{
				result = @null;
			}
			return result;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00016B00 File Offset: 0x00014D00
		[ExcludeFromDocs]
		public PlayableHandle CreateGenericMixerPlayable()
		{
			int inputCount = 0;
			return this.CreateGenericMixerPlayable(inputCount);
		}

		/// <summary>
		///   <para>Creates a generic ScriptPlayable mixer.</para>
		/// </summary>
		/// <param name="inputCount">The number of input.</param>
		/// <returns>
		///   <para>The created Playable.</para>
		/// </returns>
		// Token: 0x060010FA RID: 4346 RVA: 0x00016B20 File Offset: 0x00014D20
		public PlayableHandle CreateGenericMixerPlayable([DefaultValue("0")] int inputCount)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableHandle result;
			if (!PlayableGraph.InternalCreatePlayable(ref this, ref @null))
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

		// Token: 0x060010FB RID: 4347 RVA: 0x00016B5C File Offset: 0x00014D5C
		private static bool InternalCreatePlayable(ref PlayableGraph graph, ref PlayableHandle handle)
		{
			return PlayableGraph.INTERNAL_CALL_InternalCreatePlayable(ref graph, ref handle);
		}

		// Token: 0x060010FC RID: 4348
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalCreatePlayable(ref PlayableGraph graph, ref PlayableHandle handle);

		/// <summary>
		///   <para>Destroys the graph.</para>
		/// </summary>
		// Token: 0x060010FD RID: 4349 RVA: 0x00016B78 File Offset: 0x00014D78
		public void Destroy()
		{
			PlayableGraph.DestroyInternal(ref this);
		}

		// Token: 0x060010FE RID: 4350
		
		[MethodImpl(4096)]
		private static extern void DestroyInternal(ref PlayableGraph graph);

		/// <summary>
		///   <para>Connects two Playable instances, either by referencing the Playable instances themselves or by their PlayableHandles.</para>
		/// </summary>
		/// <param name="source">The source playable or its handle.</param>
		/// <param name="sourceOutputPort">The port used in the source playable.</param>
		/// <param name="destination">The destination playable or its handle.</param>
		/// <param name="destinationInputPort">The port used in the destination playable.</param>
		/// <returns>
		///   <para>Returns true if connection is successful.</para>
		/// </returns>
		// Token: 0x060010FF RID: 4351 RVA: 0x00016B84 File Offset: 0x00014D84
		public bool Connect(PlayableHandle source, int sourceOutputPort, PlayableHandle destination, int destinationInputPort)
		{
			return PlayableGraph.ConnectInternal(ref this, source, sourceOutputPort, destination, destinationInputPort);
		}

		/// <summary>
		///   <para>Connects two Playable instances, either by referencing the Playable instances themselves or by their PlayableHandles.</para>
		/// </summary>
		/// <param name="source">The source playable or its handle.</param>
		/// <param name="sourceOutputPort">The port used in the source playable.</param>
		/// <param name="destination">The destination playable or its handle.</param>
		/// <param name="destinationInputPort">The port used in the destination playable.</param>
		/// <returns>
		///   <para>Returns true if connection is successful.</para>
		/// </returns>
		// Token: 0x06001100 RID: 4352 RVA: 0x00016BA4 File Offset: 0x00014DA4
		public bool Connect(Playable source, int sourceOutputPort, Playable destination, int destinationInputPort)
		{
			return PlayableGraph.ConnectInternal(ref this, source.handle, sourceOutputPort, destination.handle, destinationInputPort);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00016BD0 File Offset: 0x00014DD0
		private static bool ConnectInternal(ref PlayableGraph graph, PlayableHandle source, int sourceOutputPort, PlayableHandle destination, int destinationInputPort)
		{
			return PlayableGraph.INTERNAL_CALL_ConnectInternal(ref graph, ref source, sourceOutputPort, ref destination, destinationInputPort);
		}

		// Token: 0x06001102 RID: 4354
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_ConnectInternal(ref PlayableGraph graph, ref PlayableHandle source, int sourceOutputPort, ref PlayableHandle destination, int destinationInputPort);

		/// <summary>
		///   <para>Disconnects PlayableHandle.  The connections determine the topology of the PlayableGraph and how its is evaluated.</para>
		/// </summary>
		/// <param name="playable">The source playabe or its handle.</param>
		/// <param name="inputPort">The port used in the source playable.</param>
		// Token: 0x06001103 RID: 4355 RVA: 0x00016BF4 File Offset: 0x00014DF4
		public void Disconnect(Playable playable, int inputPort)
		{
			PlayableHandle handle = playable.handle;
			PlayableGraph.DisconnectInternal(ref this, ref handle, inputPort);
		}

		/// <summary>
		///   <para>Disconnects PlayableHandle.  The connections determine the topology of the PlayableGraph and how its is evaluated.</para>
		/// </summary>
		/// <param name="playable">The source playabe or its handle.</param>
		/// <param name="inputPort">The port used in the source playable.</param>
		// Token: 0x06001104 RID: 4356 RVA: 0x00016C14 File Offset: 0x00014E14
		public void Disconnect(PlayableHandle playable, int inputPort)
		{
			PlayableGraph.DisconnectInternal(ref this, ref playable, inputPort);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00016C20 File Offset: 0x00014E20
		private static void DisconnectInternal(ref PlayableGraph graph, ref PlayableHandle playable, int inputPort)
		{
			PlayableGraph.INTERNAL_CALL_DisconnectInternal(ref graph, ref playable, inputPort);
		}

		// Token: 0x06001106 RID: 4358
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_DisconnectInternal(ref PlayableGraph graph, ref PlayableHandle playable, int inputPort);

		/// <summary>
		///   <para>Destroys the Playable associated with this PlayableHandle.</para>
		/// </summary>
		/// <param name="playable">The playable to destroy.</param>
		// Token: 0x06001107 RID: 4359 RVA: 0x00016C2C File Offset: 0x00014E2C
		public void DestroyPlayable(PlayableHandle playable)
		{
			PlayableGraph.InternalDestroyPlayable(ref this, ref playable);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00016C38 File Offset: 0x00014E38
		private static void InternalDestroyPlayable(ref PlayableGraph graph, ref PlayableHandle playable)
		{
			PlayableGraph.INTERNAL_CALL_InternalDestroyPlayable(ref graph, ref playable);
		}

		// Token: 0x06001109 RID: 4361
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalDestroyPlayable(ref PlayableGraph graph, ref PlayableHandle playable);

		/// <summary>
		///   <para>Destroys the PlayableOutput.</para>
		/// </summary>
		/// <param name="output">The output to destroy.</param>
		// Token: 0x0600110A RID: 4362 RVA: 0x00016C44 File Offset: 0x00014E44
		public void DestroyOutput(ScriptPlayableOutput output)
		{
			PlayableGraph.InternalDestroyOutput(ref this, ref output.m_Output);
		}

		// Token: 0x0600110B RID: 4363
		
		[MethodImpl(4096)]
		internal static extern void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutput output);

		/// <summary>
		///   <para>Recursively destroys the given Playable and all children connected to its inputs.</para>
		/// </summary>
		/// <param name="playable">The playable to destroy.</param>
		// Token: 0x0600110C RID: 4364 RVA: 0x00016C54 File Offset: 0x00014E54
		public void DestroySubgraph(PlayableHandle playable)
		{
			PlayableGraph.InternalDestroySubgraph(ref this, playable);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00016C60 File Offset: 0x00014E60
		private static void InternalDestroySubgraph(ref PlayableGraph graph, PlayableHandle playable)
		{
			PlayableGraph.INTERNAL_CALL_InternalDestroySubgraph(ref graph, ref playable);
		}

		// Token: 0x0600110E RID: 4366
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalDestroySubgraph(ref PlayableGraph graph, ref PlayableHandle playable);

		// Token: 0x0600110F RID: 4367 RVA: 0x00016C6C File Offset: 0x00014E6C
		[ExcludeFromDocs]
		public void Evaluate()
		{
			float deltaTime = 0f;
			this.Evaluate(deltaTime);
		}

		/// <summary>
		///   <para>Evaluates all the PlayableOutputs in the graph, and updates all the connected Playables in the graph.</para>
		/// </summary>
		/// <param name="deltaTime">The time in seconds by which to advance each Playable in the graph.</param>
		// Token: 0x06001110 RID: 4368 RVA: 0x00016C88 File Offset: 0x00014E88
		public void Evaluate([DefaultValue("0")] float deltaTime)
		{
			PlayableGraph.InternalEvaluate(ref this, deltaTime);
		}

		// Token: 0x06001111 RID: 4369
		
		[MethodImpl(4096)]
		internal static extern void InternalEvaluate(ref PlayableGraph graph, float deltaTime);

		/// <summary>
		///   <para>Returns the number of PlayableHandle owned by the Graph that have no connected outputs.</para>
		/// </summary>
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00016C94 File Offset: 0x00014E94
		public int rootPlayableCount
		{
			get
			{
				return PlayableGraph.InternalRootPlayableCount(ref this);
			}
		}

		// Token: 0x06001113 RID: 4371
		
		[MethodImpl(4096)]
		internal static extern int InternalRootPlayableCount(ref PlayableGraph graph);

		/// <summary>
		///   <para>Returns the PlayableHandle with no output connections at the given index.</para>
		/// </summary>
		/// <param name="index">The index of the root PlayableHandle.</param>
		// Token: 0x06001114 RID: 4372 RVA: 0x00016CB0 File Offset: 0x00014EB0
		public PlayableHandle GetRootPlayable(int index)
		{
			PlayableHandle @null = PlayableHandle.Null;
			PlayableGraph.InternalGetRootPlayable(index, ref this, ref @null);
			return @null;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00016CD8 File Offset: 0x00014ED8
		internal static void InternalGetRootPlayable(int index, ref PlayableGraph graph, ref PlayableHandle handle)
		{
			PlayableGraph.INTERNAL_CALL_InternalGetRootPlayable(index, ref graph, ref handle);
		}

		// Token: 0x06001116 RID: 4374
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalGetRootPlayable(int index, ref PlayableGraph graph, ref PlayableHandle handle);

		// Token: 0x04000235 RID: 565
		internal IntPtr m_Handle;

		// Token: 0x04000236 RID: 566
		internal int m_Version;
	}
}
