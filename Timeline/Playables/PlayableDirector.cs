using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Instantiates a PlayableAsset and controls playback of Playable objects.</para>
	/// </summary>
	// Token: 0x02000306 RID: 774
	public class PlayableDirector : Behaviour, IExposedPropertyTable
	{
		/// <summary>
		///   <para>The current playing state of the component. (Read Only)</para>
		/// </summary>
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06003008 RID: 12296
		public extern PlayState state {   [MethodImpl(4096)] get; }

		/// <summary>
		///   <para>The PlayableAsset that is used to instantiate a playable for playback.</para>
		/// </summary>
		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x000444C4 File Offset: 0x000426C4
		// (set) Token: 0x0600300A RID: 12298 RVA: 0x000444E4 File Offset: 0x000426E4
		public PlayableAsset playableAsset
		{
			get
			{
				return this.GetPlayableAssetInternal() as PlayableAsset;
			}
			set
			{
				this.SetPlayableAssetInternal(value);
			}
		}

		/// <summary>
		///   <para>Controls how the time is incremented when it goes beyond the duration of the playable.</para>
		/// </summary>
		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600300B RID: 12299
		// (set) Token: 0x0600300C RID: 12300
		[Obsolete("Use wrapMode property instead")]
		public extern DirectorWrapMode extrapolationMode {   [MethodImpl(4096)] get;   [MethodImpl(4096)] set; }

		/// <summary>
		///   <para>Describes what to do when the graph is completed playing.</para>
		/// </summary>
		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600300D RID: 12301
		// (set) Token: 0x0600300E RID: 12302
		public extern DirectorWrapMode wrapMode {   [MethodImpl(4096)] get;   [MethodImpl(4096)] set; }

		/// <summary>
		///   <para>Controls how time is incremented when playing back.</para>
		/// </summary>
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600300F RID: 12303
		// (set) Token: 0x06003010 RID: 12304
		public extern DirectorUpdateMode timeUpdateMode {   [MethodImpl(4096)] get;   [MethodImpl(4096)] set; }

		/// <summary>
		///   <para>The component's current time. This value is incremented according to the PlayableDirector.timeUpdateMode when it is playing. You can also change this value manually.</para>
		/// </summary>
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06003011 RID: 12305
		// (set) Token: 0x06003012 RID: 12306
		public extern double time {   [MethodImpl(4096)] get;   [MethodImpl(4096)] set; }

		/// <summary>
		///   <para>The time at which the Playable should start when first played.</para>
		/// </summary>
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06003013 RID: 12307
		// (set) Token: 0x06003014 RID: 12308
		public extern double initialTime {   [MethodImpl(4096)] get;   [MethodImpl(4096)] set; }

		/// <summary>
		///   <para>The duration of the Playable in seconds.</para>
		/// </summary>
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06003015 RID: 12309
		public extern double duration {   [MethodImpl(4096)] get; }

		/// <summary>
		///   <para>Evaluates the currently playing Playable at  the current time.</para>
		/// </summary>
		// Token: 0x06003016 RID: 12310
		 
		[MethodImpl(4096)]
		public extern void Evaluate();

		/// <summary>
		///   <para>Tells the PlayableDirector to evaluate it's PlayableGraph on the next update.</para>
		/// </summary>
		// Token: 0x06003017 RID: 12311
		 
		[MethodImpl(4096)]
		public extern void DeferredEvaluate();

		/// <summary>
		///   <para>Instatiates a Playable using the provided PlayableAsset and starts playback.</para>
		/// </summary>
		/// <param name="asset">An asset to instantiate a playable from.</param>
		/// <param name="addMissingComponents">Should required components be added to targetGameObjects if they are missing.</param>
		/// <param name="playerArray">An array of PlayableDirector player components whose types match the outputs of the playable.</param>
		/// <param name="targetGameObjects">An array of game objects to extract the PlayableDirector player components from for each playable output.</param>
		/// <param name="mode">What to do when the time passes the duration of the playable.</param>
		// Token: 0x06003018 RID: 12312 RVA: 0x000444F0 File Offset: 0x000426F0
		public void Play(PlayableAsset asset)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("Argument 'asset' is null");
			}
			this.Play(asset, this.wrapMode);
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00044518 File Offset: 0x00042718
		public void Play(PlayableAsset asset, DirectorWrapMode mode)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("Argument 'asset' is null");
			}
			this.playableAsset = asset;
			this.wrapMode = mode;
			this.Play();
		}

		// Token: 0x0600301A RID: 12314
		 
		[MethodImpl(4096)]
		private extern void SetPlayableAssetInternal(ScriptableObject asset);

		// Token: 0x0600301B RID: 12315
		 
		[MethodImpl(4096)]
		private extern ScriptableObject GetPlayableAssetInternal();

		/// <summary>
		///   <para>Instatiates a Playable using the provided PlayableAsset and starts playback.</para>
		/// </summary>
		/// <param name="asset">An asset to instantiate a playable from.</param>
		/// <param name="addMissingComponents">Should required components be added to targetGameObjects if they are missing.</param>
		/// <param name="playerArray">An array of PlayableDirector player components whose types match the outputs of the playable.</param>
		/// <param name="targetGameObjects">An array of game objects to extract the PlayableDirector player components from for each playable output.</param>
		/// <param name="mode">What to do when the time passes the duration of the playable.</param>
		// Token: 0x0600301C RID: 12316
		 
		[MethodImpl(4096)]
		public extern void Play();

		/// <summary>
		///   <para>Stops playback of the current Playable and destroys the corresponding graph.</para>
		/// </summary>
		// Token: 0x0600301D RID: 12317
		 
		[MethodImpl(4096)]
		public extern void Stop();

		/// <summary>
		///   <para>Pauses playback of the currently running playable.</para>
		/// </summary>
		// Token: 0x0600301E RID: 12318
		 
		[MethodImpl(4096)]
		public extern void Pause();

		/// <summary>
		///   <para>Resume playing a paused playable.</para>
		/// </summary>
		// Token: 0x0600301F RID: 12319
		 
		[MethodImpl(4096)]
		public extern void Resume();

		/// <summary>
		///   <para>Sets an ExposedReference value.</para>
		/// </summary>
		/// <param name="id">Identifier of the ExposedReference.</param>
		/// <param name="value">The object to bind to set the reference value to.</param>
		// Token: 0x06003020 RID: 12320 RVA: 0x00044548 File Offset: 0x00042748
		public void SetReferenceValue(PropertyName id, Object value)
		{
			PlayableDirector.INTERNAL_CALL_SetReferenceValue(this, ref id, value);
		}

		// Token: 0x06003021 RID: 12321
		 
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetReferenceValue(PlayableDirector self, ref PropertyName id, Object value);

		// Token: 0x06003022 RID: 12322 RVA: 0x00044554 File Offset: 0x00042754
		public Object GetReferenceValue(PropertyName id, out bool idValid)
		{
			return PlayableDirector.INTERNAL_CALL_GetReferenceValue(this, ref id, out idValid);
		}

		// Token: 0x06003023 RID: 12323
		 
		[MethodImpl(4096)]
		private static extern Object INTERNAL_CALL_GetReferenceValue(PlayableDirector self, ref PropertyName id, out bool idValid);

		/// <summary>
		///   <para>Clears an exposed reference value.</para>
		/// </summary>
		/// <param name="id">Identifier of the ExposedReference.</param>
		// Token: 0x06003024 RID: 12324 RVA: 0x00044574 File Offset: 0x00042774
		public void ClearReferenceValue(PropertyName id)
		{
			PlayableDirector.INTERNAL_CALL_ClearReferenceValue(this, ref id);
		}

		// Token: 0x06003025 RID: 12325
		 
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_ClearReferenceValue(PlayableDirector self, ref PropertyName id);

		/// <summary>
		///   <para>The PlayableGraph created by the PlayableDirector.</para>
		/// </summary>
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x00044580 File Offset: 0x00042780
		public PlayableGraph playableGraph
		{
			get
			{
				PlayableGraph result = default(PlayableGraph);
				this.InternalGetCurrentGraph(ref result);
				return result;
			}
		}

		// Token: 0x06003027 RID: 12327
		 
		[MethodImpl(4096)]
		private extern void InternalGetCurrentGraph(ref PlayableGraph graph);

		/// <summary>
		///   <para>Sets the binding of a reference object from a PlayableBinding.</para>
		/// </summary>
		/// <param name="key">The source object in the PlayableBinding.</param>
		/// <param name="value">The object to bind to the key.</param>
		// Token: 0x06003028 RID: 12328
		 
		[MethodImpl(4096)]
		public extern void SetGenericBinding(Object key, Object value);

		/// <summary>
		///   <para>Returns a binding to a reference object.</para>
		/// </summary>
		/// <param name="key">The object that acts as a key.</param>
		// Token: 0x06003029 RID: 12329
		 
		[MethodImpl(4096)]
		public extern Object GetGenericBinding(Object key);
	}
}
