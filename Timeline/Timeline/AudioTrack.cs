using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200002A RID: 42
	[TrackClipType(typeof(AudioClip))]
	[TrackMediaType(TimelineAsset.MediaType.Audio)]
	[Serializable]
	internal class AudioTrack : TrackAsset
	{
		// Token: 0x0600016C RID: 364 RVA: 0x000072B0 File Offset: 0x000054B0
		internal override PlayableHandle OnCreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree tree)
		{
			PlayableHandle playableHandle = AudioPlayableGraphExtensions.CreateAudioMixerPlayable(graph, base.clips.Length);
			for (int i = 0; i < base.clips.Length; i++)
			{
				TimelineClip timelineClip = base.clips[i];
				PlayableAsset playableAsset = timelineClip.asset as PlayableAsset;
				if (!(playableAsset == null))
				{
					PlayableHandle playableHandle2 = playableAsset.CreatePlayable(graph, go);
					tree.Add(new ScheduleRuntimeClip(timelineClip, playableHandle2, playableHandle, 2.0));
					graph.Connect(playableHandle2, 0, playableHandle, i);
					playableHandle2.speed = timelineClip.timeScale;
					playableHandle.SetInputWeight(playableHandle2, 1f);
				}
			}
			return playableHandle;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007364 File Offset: 0x00005564
		internal override void OnCreateClipFromAsset(Object asset, TimelineClip newClip)
		{
			if (!(asset is AudioClip))
			{
				throw new InvalidOperationException("Incompatible asset passed to AudioTrack.CreateClip");
			}
			AudioPlayableAsset audioPlayableAsset = ScriptableObject.CreateInstance<AudioPlayableAsset>();
			audioPlayableAsset.clip = (asset as AudioClip);
			newClip.asset = audioPlayableAsset;
			newClip.underlyingAsset = asset;
			newClip.duration = audioPlayableAsset.duration;
			newClip.displayName = (asset as AudioClip).name;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000073C8 File Offset: 0x000055C8
		public override PlayableBinding[] outputs
		{
			get
			{
				PlayableBinding[] array = new PlayableBinding[1];
				int num = 0;
				PlayableBinding playableBinding = default(PlayableBinding);
				playableBinding.sourceObject = this;
				playableBinding.streamName = base.name;
				playableBinding.streamType = (DirectorStreamType)1;
				array[num] = playableBinding;
				return array;
			}
		}
	}
}
