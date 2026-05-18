using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Handle representing a Playable created in a PlayableGraph. The PlayableHandle implements all general usage Playable methods.</para>
	/// </summary>
	// Token: 0x020000E7 RID: 231
	
	public struct PlayableHandle
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x000161A8 File Offset: 0x000143A8
		public T GetObject<T>() where T : IPlayable
		{
			T result;
			if (!this.IsValid())
			{
				result = default(T);
			}
			else
			{
				object scriptInstance = PlayableHandle.GetScriptInstance(ref this);
				if (scriptInstance != null)
				{
					result = (T)((object)scriptInstance);
				}
				else
				{
					Type playableTypeOf = PlayableHandle.GetPlayableTypeOf(ref this);
					T t = (T)((object)Activator.CreateInstance(playableTypeOf));
					t.playableHandle = this;
					PlayableHandle.SetScriptInstance(ref this, t);
					result = t;
				}
			}
			return result;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00016228 File Offset: 0x00014428
		private static object GetScriptInstance(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetScriptInstance(ref playable);
		}

		// Token: 0x0600107E RID: 4222
		
		[MethodImpl(4096)]
		private static extern object INTERNAL_CALL_GetScriptInstance(ref PlayableHandle playable);

		// Token: 0x0600107F RID: 4223 RVA: 0x00016244 File Offset: 0x00014444
		private static void SetScriptInstance(ref PlayableHandle playable, object scriptInstance)
		{
			PlayableHandle.INTERNAL_CALL_SetScriptInstance(ref playable, scriptInstance);
		}

		// Token: 0x06001080 RID: 4224
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetScriptInstance(ref PlayableHandle playable, object scriptInstance);

		/// <summary>
		///   <para>Returns true if the Playable is properly constructed by the PlayableGraph and has not been destroyed.</para>
		/// </summary>
		// Token: 0x06001081 RID: 4225 RVA: 0x00016250 File Offset: 0x00014450
		public bool IsValid()
		{
			return PlayableHandle.IsValidInternal(ref this);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0001626C File Offset: 0x0001446C
		private static bool IsValidInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_IsValidInternal(ref playable);
		}

		// Token: 0x06001083 RID: 4227
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_IsValidInternal(ref PlayableHandle playable);

		// Token: 0x06001084 RID: 4228 RVA: 0x00016288 File Offset: 0x00014488
		internal static Type GetPlayableTypeOf(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetPlayableTypeOf(ref playable);
		}

		// Token: 0x06001085 RID: 4229
		
		[MethodImpl(4096)]
		private static extern Type INTERNAL_CALL_GetPlayableTypeOf(ref PlayableHandle playable);

		/// <summary>
		///   <para>Used to compare PlayableHandles.</para>
		/// </summary>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x000162A4 File Offset: 0x000144A4
		public static PlayableHandle Null
		{
			get
			{
				return new PlayableHandle
				{
					m_Version = 10
				};
			}
		}

		/// <summary>
		///   <para>The PlayableGraph that created the playable.</para>
		/// </summary>
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x000162CC File Offset: 0x000144CC
		public PlayableGraph graph
		{
			get
			{
				PlayableGraph result = default(PlayableGraph);
				PlayableHandle.GetGraphInternal(ref this, ref result);
				return result;
			}
		}

		/// <summary>
		///   <para>Gets and Sets the number of inputs for the  Playable.</para>
		/// </summary>
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x000162F4 File Offset: 0x000144F4
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x00016310 File Offset: 0x00014510
		public int inputCount
		{
			get
			{
				return PlayableHandle.GetInputCountInternal(ref this);
			}
			set
			{
				PlayableHandle.SetInputCountInternal(ref this, value);
			}
		}

		/// <summary>
		///   <para>Gets and Sets the number of outputs for the  Playable.</para>
		/// </summary>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0001631C File Offset: 0x0001451C
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x00016338 File Offset: 0x00014538
		public int outputCount
		{
			get
			{
				return PlayableHandle.GetOutputCountInternal(ref this);
			}
			set
			{
				PlayableHandle.SetOutputCountInternal(ref this, value);
			}
		}

		/// <summary>
		///   <para>When playing, the time will advance in the Playable during evaluation of the graph.</para>
		/// </summary>
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00016344 File Offset: 0x00014544
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00016360 File Offset: 0x00014560
		public PlayState playState
		{
			get
			{
				return PlayableHandle.GetPlayStateInternal(ref this);
			}
			set
			{
				PlayableHandle.SetPlayStateInternal(ref this, value);
			}
		}

		/// <summary>
		///   <para>Modulates how time is incremented when the Playable is playing.</para>
		/// </summary>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0001636C File Offset: 0x0001456C
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x00016388 File Offset: 0x00014588
		public double speed
		{
			get
			{
				return PlayableHandle.GetSpeedInternal(ref this);
			}
			set
			{
				PlayableHandle.SetSpeedInternal(ref this, value);
			}
		}

		/// <summary>
		///   <para>The current  time of the Playable.</para>
		/// </summary>
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x00016394 File Offset: 0x00014594
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x000163B0 File Offset: 0x000145B0
		public double time
		{
			get
			{
				return PlayableHandle.GetTimeInternal(ref this);
			}
			set
			{
				PlayableHandle.SetTimeInternal(ref this, value);
			}
		}

		/// <summary>
		///   <para>A flag indicating that a playable has completed its operation.</para>
		/// </summary>
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x000163BC File Offset: 0x000145BC
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x000163D8 File Offset: 0x000145D8
		public bool isDone
		{
			get
			{
				return PlayableHandle.InternalGetDone(ref this);
			}
			set
			{
				PlayableHandle.InternalSetDone(ref this, value);
			}
		}

		/// <summary>
		///   <para>Indicates whether the playable will propagate set time calls to its inputs.</para>
		/// </summary>
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x000163E4 File Offset: 0x000145E4
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00016400 File Offset: 0x00014600
		public bool propagateSetTime
		{
			get
			{
				return PlayableHandle.InternalGetPropagateSetTime(ref this);
			}
			set
			{
				PlayableHandle.InternalSetPropagateSetTime(ref this, value);
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x0001640C File Offset: 0x0001460C
		internal bool canChangeInputs
		{
			get
			{
				return PlayableHandle.CanChangeInputsInternal(ref this);
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001097 RID: 4247 RVA: 0x00016428 File Offset: 0x00014628
		internal bool canSetWeights
		{
			get
			{
				return PlayableHandle.CanSetWeightsInternal(ref this);
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00016444 File Offset: 0x00014644
		internal bool canDestroy
		{
			get
			{
				return PlayableHandle.CanDestroyInternal(ref this);
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00016460 File Offset: 0x00014660
		private static bool CanChangeInputsInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_CanChangeInputsInternal(ref playable);
		}

		// Token: 0x0600109A RID: 4250
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_CanChangeInputsInternal(ref PlayableHandle playable);

		// Token: 0x0600109B RID: 4251 RVA: 0x0001647C File Offset: 0x0001467C
		private static bool CanSetWeightsInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_CanSetWeightsInternal(ref playable);
		}

		// Token: 0x0600109C RID: 4252
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_CanSetWeightsInternal(ref PlayableHandle playable);

		// Token: 0x0600109D RID: 4253 RVA: 0x00016498 File Offset: 0x00014698
		private static bool CanDestroyInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_CanDestroyInternal(ref playable);
		}

		// Token: 0x0600109E RID: 4254
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_CanDestroyInternal(ref PlayableHandle playable);

		// Token: 0x0600109F RID: 4255 RVA: 0x000164B4 File Offset: 0x000146B4
		private static PlayState GetPlayStateInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetPlayStateInternal(ref playable);
		}

		// Token: 0x060010A0 RID: 4256
		
		[MethodImpl(4096)]
		private static extern PlayState INTERNAL_CALL_GetPlayStateInternal(ref PlayableHandle playable);

		// Token: 0x060010A1 RID: 4257 RVA: 0x000164D0 File Offset: 0x000146D0
		private static void SetPlayStateInternal(ref PlayableHandle playable, PlayState playState)
		{
			PlayableHandle.INTERNAL_CALL_SetPlayStateInternal(ref playable, playState);
		}

		// Token: 0x060010A2 RID: 4258
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetPlayStateInternal(ref PlayableHandle playable, PlayState playState);

		// Token: 0x060010A3 RID: 4259 RVA: 0x000164DC File Offset: 0x000146DC
		private static double GetSpeedInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetSpeedInternal(ref playable);
		}

		// Token: 0x060010A4 RID: 4260
		
		[MethodImpl(4096)]
		private static extern double INTERNAL_CALL_GetSpeedInternal(ref PlayableHandle playable);

		// Token: 0x060010A5 RID: 4261 RVA: 0x000164F8 File Offset: 0x000146F8
		private static void SetSpeedInternal(ref PlayableHandle playable, double speed)
		{
			PlayableHandle.INTERNAL_CALL_SetSpeedInternal(ref playable, speed);
		}

		// Token: 0x060010A6 RID: 4262
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetSpeedInternal(ref PlayableHandle playable, double speed);

		// Token: 0x060010A7 RID: 4263 RVA: 0x00016504 File Offset: 0x00014704
		private static double GetTimeInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetTimeInternal(ref playable);
		}

		// Token: 0x060010A8 RID: 4264
		
		[MethodImpl(4096)]
		private static extern double INTERNAL_CALL_GetTimeInternal(ref PlayableHandle playable);

		// Token: 0x060010A9 RID: 4265 RVA: 0x00016520 File Offset: 0x00014720
		private static void SetTimeInternal(ref PlayableHandle playable, double time)
		{
			PlayableHandle.INTERNAL_CALL_SetTimeInternal(ref playable, time);
		}

		// Token: 0x060010AA RID: 4266
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetTimeInternal(ref PlayableHandle playable, double time);

		// Token: 0x060010AB RID: 4267 RVA: 0x0001652C File Offset: 0x0001472C
		private static bool InternalGetDone(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_InternalGetDone(ref playable);
		}

		// Token: 0x060010AC RID: 4268
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalGetDone(ref PlayableHandle playable);

		// Token: 0x060010AD RID: 4269 RVA: 0x00016548 File Offset: 0x00014748
		private static void InternalSetDone(ref PlayableHandle playable, bool isDone)
		{
			PlayableHandle.INTERNAL_CALL_InternalSetDone(ref playable, isDone);
		}

		// Token: 0x060010AE RID: 4270
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalSetDone(ref PlayableHandle playable, bool isDone);

		/// <summary>
		///   <para>The duration of the Playable in seconds.</para>
		/// </summary>
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00016554 File Offset: 0x00014754
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00016570 File Offset: 0x00014770
		public double duration
		{
			get
			{
				return PlayableHandle.GetDurationInternal(ref this);
			}
			set
			{
				PlayableHandle.SetDurationInternal(ref this, value);
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0001657C File Offset: 0x0001477C
		private static double GetDurationInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetDurationInternal(ref playable);
		}

		// Token: 0x060010B2 RID: 4274
		
		[MethodImpl(4096)]
		private static extern double INTERNAL_CALL_GetDurationInternal(ref PlayableHandle playable);

		// Token: 0x060010B3 RID: 4275 RVA: 0x00016598 File Offset: 0x00014798
		private static void SetDurationInternal(ref PlayableHandle playable, double duration)
		{
			PlayableHandle.INTERNAL_CALL_SetDurationInternal(ref playable, duration);
		}

		// Token: 0x060010B4 RID: 4276
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetDurationInternal(ref PlayableHandle playable, double duration);

		// Token: 0x060010B5 RID: 4277 RVA: 0x000165A4 File Offset: 0x000147A4
		private static bool InternalGetPropagateSetTime(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_InternalGetPropagateSetTime(ref playable);
		}

		// Token: 0x060010B6 RID: 4278
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_InternalGetPropagateSetTime(ref PlayableHandle playable);

		// Token: 0x060010B7 RID: 4279 RVA: 0x000165C0 File Offset: 0x000147C0
		private static void InternalSetPropagateSetTime(ref PlayableHandle playable, bool value)
		{
			PlayableHandle.INTERNAL_CALL_InternalSetPropagateSetTime(ref playable, value);
		}

		// Token: 0x060010B8 RID: 4280
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_InternalSetPropagateSetTime(ref PlayableHandle playable, bool value);

		// Token: 0x060010B9 RID: 4281 RVA: 0x000165CC File Offset: 0x000147CC
		private static void GetGraphInternal(ref PlayableHandle playable, ref PlayableGraph graph)
		{
			PlayableHandle.INTERNAL_CALL_GetGraphInternal(ref playable, ref graph);
		}

		// Token: 0x060010BA RID: 4282
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_GetGraphInternal(ref PlayableHandle playable, ref PlayableGraph graph);

		// Token: 0x060010BB RID: 4283 RVA: 0x000165D8 File Offset: 0x000147D8
		private static int GetInputCountInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetInputCountInternal(ref playable);
		}

		// Token: 0x060010BC RID: 4284
		
		[MethodImpl(4096)]
		private static extern int INTERNAL_CALL_GetInputCountInternal(ref PlayableHandle playable);

		// Token: 0x060010BD RID: 4285 RVA: 0x000165F4 File Offset: 0x000147F4
		private static void SetInputCountInternal(ref PlayableHandle playable, int count)
		{
			PlayableHandle.INTERNAL_CALL_SetInputCountInternal(ref playable, count);
		}

		// Token: 0x060010BE RID: 4286
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetInputCountInternal(ref PlayableHandle playable, int count);

		// Token: 0x060010BF RID: 4287 RVA: 0x00016600 File Offset: 0x00014800
		private static int GetOutputCountInternal(ref PlayableHandle playable)
		{
			return PlayableHandle.INTERNAL_CALL_GetOutputCountInternal(ref playable);
		}

		// Token: 0x060010C0 RID: 4288
		
		[MethodImpl(4096)]
		private static extern int INTERNAL_CALL_GetOutputCountInternal(ref PlayableHandle playable);

		// Token: 0x060010C1 RID: 4289 RVA: 0x0001661C File Offset: 0x0001481C
		private static void SetOutputCountInternal(ref PlayableHandle playable, int count)
		{
			PlayableHandle.INTERNAL_CALL_SetOutputCountInternal(ref playable, count);
		}

		// Token: 0x060010C2 RID: 4290
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetOutputCountInternal(ref PlayableHandle playable, int count);

		/// <summary>
		///   <para>Returns the PlayableHandle connected at the given input port index.</para>
		/// </summary>
		/// <param name="inputPort">The port index.</param>
		// Token: 0x060010C3 RID: 4291 RVA: 0x00016628 File Offset: 0x00014828
		public PlayableHandle GetInput(int inputPort)
		{
			return PlayableHandle.GetInputInternal(ref this, inputPort);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00016644 File Offset: 0x00014844
		private static PlayableHandle GetInputInternal(ref PlayableHandle playable, int index)
		{
			PlayableHandle result;
			PlayableHandle.INTERNAL_CALL_GetInputInternal(ref playable, index, out result);
			return result;
		}

		// Token: 0x060010C5 RID: 4293
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_GetInputInternal(ref PlayableHandle playable, int index, out PlayableHandle value);

		/// <summary>
		///   <para>Returns the PlayableHandle connected at the given ouput port index.</para>
		/// </summary>
		/// <param name="outputPort">The port index.</param>
		// Token: 0x060010C6 RID: 4294 RVA: 0x00016664 File Offset: 0x00014864
		public PlayableHandle GetOutput(int outputPort)
		{
			return PlayableHandle.GetOutputInternal(ref this, outputPort);
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x00016680 File Offset: 0x00014880
		private static PlayableHandle GetOutputInternal(ref PlayableHandle playable, int index)
		{
			PlayableHandle result;
			PlayableHandle.INTERNAL_CALL_GetOutputInternal(ref playable, index, out result);
			return result;
		}

		// Token: 0x060010C8 RID: 4296
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_GetOutputInternal(ref PlayableHandle playable, int index, out PlayableHandle value);

		// Token: 0x060010C9 RID: 4297 RVA: 0x000166A0 File Offset: 0x000148A0
		private static void SetInputWeightFromIndexInternal(ref PlayableHandle playable, int index, float weight)
		{
			PlayableHandle.INTERNAL_CALL_SetInputWeightFromIndexInternal(ref playable, index, weight);
		}

		// Token: 0x060010CA RID: 4298
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetInputWeightFromIndexInternal(ref PlayableHandle playable, int index, float weight);

		/// <summary>
		///   <para>Sets the weight of the Playable connected at the given input port index.</para>
		/// </summary>
		/// <param name="inputIndex">The port index.</param>
		/// <param name="weight">The weight. Should be between 0 and 1.</param>
		// Token: 0x060010CB RID: 4299 RVA: 0x000166AC File Offset: 0x000148AC
		public bool SetInputWeight(int inputIndex, float weight)
		{
			bool result;
			if (this.CheckInputBounds(inputIndex))
			{
				PlayableHandle.SetInputWeightFromIndexInternal(ref this, inputIndex, weight);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000166E0 File Offset: 0x000148E0
		private static float GetInputWeightFromIndexInternal(ref PlayableHandle playable, int index)
		{
			return PlayableHandle.INTERNAL_CALL_GetInputWeightFromIndexInternal(ref playable, index);
		}

		// Token: 0x060010CD RID: 4301
		
		[MethodImpl(4096)]
		private static extern float INTERNAL_CALL_GetInputWeightFromIndexInternal(ref PlayableHandle playable, int index);

		/// <summary>
		///   <para>Returns the weight of the Playable connected at the given input port index.</para>
		/// </summary>
		/// <param name="inputIndex">The port index.</param>
		// Token: 0x060010CE RID: 4302 RVA: 0x000166FC File Offset: 0x000148FC
		public float GetInputWeight(int inputIndex)
		{
			float result;
			if (this.CheckInputBounds(inputIndex))
			{
				result = PlayableHandle.GetInputWeightFromIndexInternal(ref this, inputIndex);
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00016730 File Offset: 0x00014930
		private static void SetInputWeightInternal(ref PlayableHandle playable, ref PlayableHandle input, float weight)
		{
			PlayableHandle.INTERNAL_CALL_SetInputWeightInternal(ref playable, ref input, weight);
		}

		// Token: 0x060010D0 RID: 4304
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetInputWeightInternal(ref PlayableHandle playable, ref PlayableHandle input, float weight);

		// Token: 0x060010D1 RID: 4305 RVA: 0x0001673C File Offset: 0x0001493C
		public void SetInputWeight(PlayableHandle input, float weight)
		{
			PlayableHandle.SetInputWeightInternal(ref this, ref input, weight);
		}

		/// <summary>
		///   <para>Destroys the Playable associated with this PlayableHandle.</para>
		/// </summary>
		// Token: 0x060010D2 RID: 4306 RVA: 0x00016748 File Offset: 0x00014948
		public void Destroy()
		{
			this.graph.DestroyPlayable(this);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0001676C File Offset: 0x0001496C
		public static bool operator ==(PlayableHandle x, PlayableHandle y)
		{
			return PlayableHandle.CompareVersion(x, y);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00016788 File Offset: 0x00014988
		public static bool operator !=(PlayableHandle x, PlayableHandle y)
		{
			return !PlayableHandle.CompareVersion(x, y);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000167A8 File Offset: 0x000149A8
		public override bool Equals(object p)
		{
			return p is PlayableHandle && PlayableHandle.CompareVersion(this, (PlayableHandle)p);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000167E0 File Offset: 0x000149E0
		public override int GetHashCode()
		{
			return this.m_Handle.GetHashCode() ^ this.m_Version.GetHashCode();
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00016818 File Offset: 0x00014A18
		internal static bool CompareVersion(PlayableHandle lhs, PlayableHandle rhs)
		{
			return lhs.m_Handle == rhs.m_Handle && lhs.m_Version == rhs.m_Version;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00016858 File Offset: 0x00014A58
		internal bool CheckInputBounds(int inputIndex)
		{
			return this.CheckInputBounds(inputIndex, false);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00016878 File Offset: 0x00014A78
		internal bool CheckInputBounds(int inputIndex, bool acceptAny)
		{
			bool result;
			if (inputIndex == -1 && acceptAny)
			{
				result = true;
			}
			else
			{
				if (inputIndex < 0)
				{
					throw new IndexOutOfRangeException("Index must be greater than 0");
				}
				if (this.inputCount <= inputIndex)
				{
					throw new IndexOutOfRangeException(string.Concat(new object[]
					{
						"inputIndex ",
						inputIndex,
						" is greater than the number of available inputs (",
						this.inputCount,
						")."
					}));
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400022D RID: 557
		internal IntPtr m_Handle;

		// Token: 0x0400022E RID: 558
		internal int m_Version;
	}
}
