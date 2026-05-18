using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000020 RID: 32
	[TrackClipType(typeof(ActivationPlayableAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Script)]
	[TrackBindingType(typeof(GameObject))]
	[Serializable]
	internal class ActivationTrack : TrackAsset
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006014 File Offset: 0x00004214
		internal override bool compilable
		{
			get
			{
				return base.isEmpty || base.compilable;
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006040 File Offset: 0x00004240
		public override PlayableHandle CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			PlayableHandle result = ScriptPlayableGraphExtensions.CreateScriptPlayable<ActivationMixerPlayable>(graph);
			result.inputCount = inputCount;
			this.m_ActivationMixer = result.GetObject<ActivationMixerPlayable>();
			PlayableDirector component = go.GetComponent<PlayableDirector>();
			this.UpdateBoundGameObject(component);
			this.UpdateTrackMode();
			return result;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006088 File Offset: 0x00004288
		private void UpdateBoundGameObject(PlayableDirector director)
		{
			if (director != null)
			{
				GameObject gameObject = director.GetGenericBinding(this) as GameObject;
				if (gameObject != null && this.m_ActivationMixer != null)
				{
					this.m_ActivationMixer.boundGameObject = gameObject;
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000060D6 File Offset: 0x000042D6
		public void UpdateTrackMode()
		{
			if (this.m_ActivationMixer != null)
			{
				this.m_ActivationMixer.postPlaybackState = this.m_PostPlaybackState;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000060F8 File Offset: 0x000042F8
		public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			GameObject gameObjectBinding = base.GetGameObjectBinding(director);
			if (gameObjectBinding != null)
			{
				driver.AddFromName(gameObjectBinding, "m_IsActive");
			}
		}

		// Token: 0x04000096 RID: 150
		[SerializeField]
		private ActivationTrack.PostPlaybackState m_PostPlaybackState;

		// Token: 0x04000097 RID: 151
		private ActivationMixerPlayable m_ActivationMixer = null;

		// Token: 0x02000021 RID: 33
		public enum PostPlaybackState
		{
			// Token: 0x04000099 RID: 153
			Active,
			// Token: 0x0400009A RID: 154
			Inactive,
			// Token: 0x0400009B RID: 155
			Revert,
			// Token: 0x0400009C RID: 156
			LeaveAsIs
		}
	}
}
