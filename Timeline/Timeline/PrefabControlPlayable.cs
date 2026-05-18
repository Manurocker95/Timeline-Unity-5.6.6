using System;
using System.Collections;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Platyable that controls and instanciate Prefab.</para>
	/// </summary>
	// Token: 0x02000034 RID: 52
	public class PrefabControlPlayable : ScriptPlayable
	{
		/// <summary>
		///   <para>The instance of the prefab that has been generated for this playable.</para>
		/// </summary>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00008734 File Offset: 0x00006934
		public GameObject prefabInstance
		{
			get
			{
				return this.m_Instance;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008750 File Offset: 0x00006950
		public GameObject Initialize(GameObject prefabGameObject, Transform parentTransform)
		{
			if (prefabGameObject == null)
			{
				throw new ArgumentNullException("Prefab cannot be null");
			}
			if (this.m_Instance != null)
			{
				Debug.LogWarningFormat("Prefab Control Playable ({0}) has already been initialized with a Prefab ({1}).", new object[]
				{
					prefabGameObject.name,
					this.m_Instance.name
				});
			}
			else
			{
				this.m_Instance = Object.Instantiate<GameObject>(prefabGameObject, parentTransform, false);
				this.m_Instance.name = prefabGameObject.name + " [Timeline]";
				this.m_Instance.SetActive(false);
				PrefabControlPlayable.SetHideFlagsRecursive(this.m_Instance);
			}
			return this.m_Instance;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008803 File Offset: 0x00006A03
		public override void OnDestroy()
		{
			if (this.m_Instance)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.m_Instance);
				}
				else
				{
					Object.DestroyImmediate(this.m_Instance);
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008840 File Offset: 0x00006A40
		public override void OnPlayStateChanged(FrameData info, PlayState newState)
		{
			if (!(this.m_Instance == null))
			{
				if (newState != (PlayState)0)
				{
					if (newState == (PlayState)1)
					{
						this.m_Instance.SetActive(true);
					}
				}
				else
				{
					this.m_Instance.SetActive(false);
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008898 File Offset: 0x00006A98
		private static void SetHideFlagsRecursive(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            gameObject.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;

            if (!Application.isPlaying)
                gameObject.hideFlags |= HideFlags.HideInHierarchy;

            IEnumerator enumerator = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					PrefabControlPlayable.SetHideFlagsRecursive(transform.gameObject);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x040000E6 RID: 230
		private GameObject m_Instance = null;
	}
}
