using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200001E RID: 30
	internal class ActivationMixerPlayable : ScriptPlayable
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005E34 File Offset: 0x00004034
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005E4E File Offset: 0x0000404E
		public GameObject boundGameObject { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005E58 File Offset: 0x00004058
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00005E73 File Offset: 0x00004073
		public ActivationTrack.PostPlaybackState postPlaybackState
		{
			get
			{
				return this.m_PostPlaybackState;
			}
			set
			{
				this.m_PostPlaybackState = value;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005E7D File Offset: 0x0000407D
		public override void OnGraphStart()
		{
			if (this.boundGameObject != null)
			{
				this.m_BoundGameObjectInitialStateIsActive = this.boundGameObject.activeInHierarchy;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005EA4 File Offset: 0x000040A4
		public override void OnGraphStop()
		{
			if (!(this.boundGameObject == null))
			{
				if (!Application.isPlaying)
				{
					this.boundGameObject.SetActive(this.m_BoundGameObjectInitialStateIsActive);
				}
				else
				{
					switch (this.m_PostPlaybackState)
					{
					case ActivationTrack.PostPlaybackState.Active:
						this.boundGameObject.SetActive(true);
						break;
					case ActivationTrack.PostPlaybackState.Inactive:
						this.boundGameObject.SetActive(false);
						break;
					case ActivationTrack.PostPlaybackState.Revert:
						this.boundGameObject.SetActive(this.m_BoundGameObjectInitialStateIsActive);
						break;
					}
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005F48 File Offset: 0x00004148
		public override void ProcessFrame(FrameData info, object playerData)
		{
			GameObject gameObject = playerData as GameObject;
			if (!(gameObject == null) && !(gameObject != this.boundGameObject))
			{
				int inputCount = this.handle.inputCount;
				bool active = false;
				for (int i = 0; i < inputCount; i++)
				{
					if (this.handle.GetInputWeight(i) > 0f)
					{
						active = true;
						break;
					}
				}
				gameObject.SetActive(active);
			}
		}

		// Token: 0x04000093 RID: 147
		private ActivationTrack.PostPlaybackState m_PostPlaybackState;

		// Token: 0x04000094 RID: 148
		private bool m_BoundGameObjectInitialStateIsActive = false;
	}
}
