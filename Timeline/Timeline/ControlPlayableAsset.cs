using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Asset that generates playables for controlling time-related elements on a GameObject.</para>
	/// </summary>
	// Token: 0x0200002B RID: 43
	[NotKeyable]
	[Serializable]
	public class ControlPlayableAsset : PlayableAsset, IPropertyPreview, ITimelineClipAsset
	{
		// Token: 0x06000170 RID: 368 RVA: 0x00007452 File Offset: 0x00005652
		public void OnEnable()
		{
			if (this.particleRandomSeed == 0U)
			{
				this.particleRandomSeed = (uint)Random.Range(1, 10000);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00007474 File Offset: 0x00005674
		public override double duration
		{
			get
			{
				return this.m_Duration;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007490 File Offset: 0x00005690
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | ((!this.m_SupportLoop) ? ClipCaps.None : ClipCaps.Looping);
			}
		}
  
        // Token: 0x06000173 RID: 371 RVA: 0x000074BC File Offset: 0x000056BC
        public override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
		{
			List<PlayableHandle> list = new List<PlayableHandle>();
            //GameObject gameObject = this.sourceGameObject.Resolve(graph.resolver);


            GameObject gameObject = this.sourceGameObject.defaultValue as GameObject;

            if (gameObject == null)
                gameObject = go;

            if (this.prefabGameObject != null)
			{
				Transform parentTransform = (!(gameObject != null)) ? null : gameObject.transform;
				PlayableHandle item = graph.CreatePrefabControlPlayable(this.prefabGameObject, parentTransform);
				gameObject = item.GetObject<PrefabControlPlayable>().prefabInstance;
				list.Add(item);
			}
			this.UpdateDurationAndLoopFlag(gameObject);
			PlayableHandle result;
			if (gameObject == null)
			{
				result = graph.CreatePlayable();
			}
			else
			{
				PlayableDirector component = go.GetComponent<PlayableDirector>();
				if (component != null)
				{
					this.m_ControlDirectorAsset = component.playableAsset;
				}
				if (this.active)
				{
					this.CreateActivationPlayable(gameObject, graph, list);
				}
				if (this.updateDirector)
				{
					this.SearchHierarchyAndConnectDirector(gameObject, graph, list);
				}
				if (this.updateParticle)
				{
					this.SearchHiearchyAndConnectParticleSystem(gameObject, graph, list);
				}
				if (this.updateITimeControl)
				{
					this.SearchHierarchyAndConnectControlableScripts(gameObject, graph, list);
				}
				PlayableHandle playableHandle = this.ConnectPlayablesToMixer(graph, list);
				result = playableHandle;
			}
			return result;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000075DC File Offset: 0x000057DC
		private PlayableHandle ConnectPlayablesToMixer(PlayableGraph graph, List<PlayableHandle> playables)
		{
			PlayableHandle playableHandle = graph.CreateGenericMixerPlayable(playables.Count);
			for (int num = 0; num != playables.Count; num++)
			{
				this.ConnectMixerAndPlayable(graph, playableHandle, playables[num], num);
			}
			playableHandle.propagateSetTime = true;
			return playableHandle;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007634 File Offset: 0x00005834
		private void CreateActivationPlayable(GameObject root, PlayableGraph graph, List<PlayableHandle> outplayables)
		{
			PlayableHandle item = graph.CreateActivationControlPlayable(root, this.postPlayback);
			if (item.IsValid())
			{
				outplayables.Add(item);
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007664 File Offset: 0x00005864
		private bool SearchHiearchyAndConnectParticleSystem(GameObject root, PlayableGraph graph, List<PlayableHandle> outplayables)
		{
			bool result;
			if (root == null)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				foreach (ParticleSystem particleSystem in this.GetParticleSystems(root))
				{
					if (particleSystem != null)
					{
						flag = true;
						outplayables.Add(graph.CreateParticleControlPlayable(particleSystem, this.particleRandomSeed));
					}
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000076FC File Offset: 0x000058FC
		private void SearchHierarchyAndConnectDirector(GameObject root, PlayableGraph graph, List<PlayableHandle> outplayables)
		{
			if (!(root == null))
			{
				foreach (PlayableDirector playableDirector in this.GetDirectors(root))
				{
					if (playableDirector != null)
					{
						if (playableDirector.playableAsset == this.m_ControlDirectorAsset)
						{
							Debug.LogWarningFormat("Control Playable ({0}) is referencing the same DirectorPlayable component than the one in which it is playing.", new object[]
							{
								base.name
							});
						}
						else
						{
							outplayables.Add(graph.CreateDirectorControlPlayable(playableDirector));
						}
					}
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000077B8 File Offset: 0x000059B8
		private void SearchHierarchyAndConnectControlableScripts(GameObject root, PlayableGraph graph, List<PlayableHandle> outplayables)
		{
			if (!(root == null))
			{
				foreach (MonoBehaviour monoBehaviour in this.GetControlableScripts(root))
				{
					outplayables.Add(graph.CreateTimeControlPlayable((ITimeControl)monoBehaviour));
				}
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007834 File Offset: 0x00005A34
		private void ConnectMixerAndPlayable(PlayableGraph graph, PlayableHandle mixer, PlayableHandle playable, int portIndex)
		{
			graph.Connect(playable, 0, mixer, portIndex);
			mixer.SetInputWeight(playable, 1f);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007854 File Offset: 0x00005A54
		private IEnumerable<ParticleSystem> GetParticleSystems(GameObject gameObject)
		{
			if (gameObject != null)
			{
				ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
				if (particleSystem != null)
				{
					yield return particleSystem;
				}
				else if (this.searchHierarchy)
				{
					int count = gameObject.transform.childCount;
					for (int i = 0; i < count; i++)
					{
						IEnumerable<ParticleSystem> childPS = this.GetParticleSystems(gameObject.transform.GetChild(i).gameObject);
						foreach (ParticleSystem ps in childPS)
						{
							yield return ps;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007888 File Offset: 0x00005A88
		private IEnumerable<PlayableDirector> GetDirectors(GameObject gameObject)
		{
			if (gameObject != null)
			{
				if (this.searchHierarchy)
				{
					PlayableDirector[] directors = gameObject.GetComponentsInChildren<PlayableDirector>(true);
					foreach (PlayableDirector d in directors)
					{
						yield return d;
					}
				}
				else
				{
					PlayableDirector director = gameObject.GetComponent<PlayableDirector>();
					if (director != null)
					{
						yield return director;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000078BC File Offset: 0x00005ABC
		private IEnumerable<MonoBehaviour> GetControlableScripts(GameObject root)
		{
			if (root == null)
			{
				yield break;
			}
			foreach (MonoBehaviour script in root.GetComponentsInChildren<MonoBehaviour>())
			{
				if (script is ITimeControl)
				{
					yield return script;
				}
			}
			yield break;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000078E8 File Offset: 0x00005AE8
		private void UpdateDurationAndLoopFlag(GameObject gameObject)
		{
			this.m_Duration = PlayableBinding.DefaultDuration;
			this.m_SupportLoop = false;
			if (!(gameObject == null))
			{
				List<PlayableDirector> list = this.GetDirectors(gameObject).ToList<PlayableDirector>();
				List<ParticleSystem> list2 = this.GetParticleSystems(gameObject).ToList<ParticleSystem>();
				if (list.Count == 1 && list2.Count == 0 && list[0].playableAsset != null)
				{
					this.m_Duration = list[0].playableAsset.duration;
					this.m_SupportLoop = (list[0].wrapMode == (DirectorWrapMode)1);
				}
				else if (list.Count == 0 && list2.Count == 1)
				{
					this.m_Duration = (double)list2[0].main.duration;
					this.m_SupportLoop = list2[0].main.loop;
				}
			}
		}

        // Token: 0x0600017E RID: 382 RVA: 0x000079E4 File Offset: 0x00005BE4
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            GameObject gameObject = null;

            if (this.sourceGameObject.defaultValue != null)
                gameObject = this.sourceGameObject.defaultValue as GameObject;
            //			GameObject gameObject = this.sourceGameObject.Resolve(director);
            if (gameObject != null)
			{
				if (this.updateParticle)
				{
					foreach (ParticleSystem particleSystem in this.GetParticleSystems(gameObject))
					{
						driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "randomSeed");
						driver.AddFromName<ParticleSystem>(particleSystem.gameObject, "autoRandomSeed");
					}
				}
				if (this.active)
				{
					driver.AddFromName(gameObject, "m_IsActive");
				}
				if (this.updateITimeControl)
				{
					foreach (MonoBehaviour monoBehaviour in this.GetControlableScripts(gameObject))
					{
						IPropertyPreview propertyPreview = monoBehaviour as IPropertyPreview;
						if (propertyPreview != null)
						{
							propertyPreview.GatherProperties(director, driver);
						}
						else
						{
							driver.AddFromComponent(monoBehaviour.gameObject, monoBehaviour);
						}
					}
				}
			}
		}

		/// <summary>
		///   <para>GameObject in the scene to control, or the parent of the instantiated prefab.</para>
		/// </summary>
		// Token: 0x040000C8 RID: 200
		[SerializeField]
		public ExposedReference<GameObject> sourceGameObject;

		/// <summary>
		///   <para>Prefab object that will be instantiated.</para>
		/// </summary>
		// Token: 0x040000C9 RID: 201
		[SerializeField]
		public GameObject prefabGameObject;

		/// <summary>
		///   <para>Indicates if user wants to control ParticleSystems.</para>
		/// </summary>
		// Token: 0x040000CA RID: 202
		[SerializeField]
		public bool updateParticle = true;

		/// <summary>
		///   <para>Let the particle systems behave the same way on each execution.</para>
		/// </summary>
		// Token: 0x040000CB RID: 203
		[SerializeField]
		public uint particleRandomSeed;

		/// <summary>
		///   <para>Indicate if user wants to control PlayableDirectors.</para>
		/// </summary>
		// Token: 0x040000CC RID: 204
		[SerializeField]
		public bool updateDirector = true;

		/// <summary>
		///   <para>Indicates that whether Monobehaviours implementing ITimeControl on the gameObject will be controlled.</para>
		/// </summary>
		// Token: 0x040000CD RID: 205
		[SerializeField]
		public bool updateITimeControl = true;

		/// <summary>
		///   <para>Indicate whether to search the entire hierachy for controlable components.</para>
		/// </summary>
		// Token: 0x040000CE RID: 206
		[SerializeField]
		public bool searchHierarchy = true;

		/// <summary>
		///   <para>Indicate if the playable will use Activation.</para>
		/// </summary>
		// Token: 0x040000CF RID: 207
		[SerializeField]
		public bool active = true;

		/// <summary>
		///   <para>How the sourceGameObject active state will be at the end of the graph.</para>
		/// </summary>
		// Token: 0x040000D0 RID: 208
		[SerializeField]
		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		// Token: 0x040000D1 RID: 209
		private PlayableAsset m_ControlDirectorAsset;

		// Token: 0x040000D2 RID: 210
		private double m_Duration = PlayableBinding.DefaultDuration;

		// Token: 0x040000D3 RID: 211
		private bool m_SupportLoop;
	}
}
