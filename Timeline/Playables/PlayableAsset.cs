using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>An base class for assets that can be used to instatiate a Playable at runtime.</para>
	/// </summary>
	// Token: 0x020000EA RID: 234
	
	[Serializable]
	public abstract class PlayableAsset : ScriptableObject, IPlayableAsset
	{
		/// <summary>
		///   <para>Implement this method to have your asset inject playables into the given graph.</para>
		/// </summary>
		/// <param name="graph">The graph to inject playables into.</param>
		/// <param name="owner">The game object which initiated the build.</param>
		/// <returns>
		///   <para>The playable injected into the graph, or the root playable if multiple playables are injected.</para>
		/// </returns>
		// Token: 0x060010E2 RID: 4322
		public abstract PlayableHandle CreatePlayable(PlayableGraph graph, GameObject owner);

		/// <summary>
		///   <para>A description of the outputs of the instantiated Playable.</para>
		/// </summary>
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00016978 File Offset: 0x00014B78
		public virtual PlayableBinding[] outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}

		/// <summary>
		///   <para>The playback duration in seconds of the instantiated Playable.</para>
		/// </summary>
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00016994 File Offset: 0x00014B94
		public virtual double duration
		{
			get
			{
				return PlayableBinding.DefaultDuration;
			}
		}

        // Token: 0x060010E5 RID: 4325 RVA: 0x000169B0 File Offset: 0x00014BB0
        public virtual double GetDuration()
        {
            return duration;
        }
    }
}
