using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000036 RID: 54
	public static class TimelinePlayableGraphExtensions
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x000089D4 File Offset: 0x00006BD4
		public static PlayableHandle CreateParticleControlPlayable(this PlayableGraph graph, ParticleSystem component, uint randomSeed)
		{
			PlayableHandle result;
			if (component == null)
			{
				result = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle = ScriptPlayableGraphExtensions.CreateScriptPlayable<ParticleControlPlayable>(graph);
				playableHandle.GetObject<ParticleControlPlayable>().Initialize(component, randomSeed);
				result = playableHandle;
			}
			return result;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008A18 File Offset: 0x00006C18
		public static PlayableHandle CreateActivationControlPlayable(this PlayableGraph graph, GameObject gameObject, ActivationControlPlayable.PostPlaybackState postPlaybackState)
		{
			PlayableHandle result;
			if (gameObject == null)
			{
				result = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle = ScriptPlayableGraphExtensions.CreateScriptPlayable<ActivationControlPlayable>(graph);
				ActivationControlPlayable @object = playableHandle.GetObject<ActivationControlPlayable>();
				@object.gameObject = gameObject;
				@object.postPlayback = postPlaybackState;
				result = playableHandle;
			}
			return result;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008A64 File Offset: 0x00006C64
		public static PlayableHandle CreateDirectorControlPlayable(this PlayableGraph graph, PlayableDirector director)
		{
			PlayableHandle result;
			if (director == null)
			{
				result = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle = ScriptPlayableGraphExtensions.CreateScriptPlayable<DirectorControlPlayable>(graph);
				playableHandle.GetObject<DirectorControlPlayable>().director = director;
				result = playableHandle;
			}
			return result;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public static PlayableHandle CreatePrefabControlPlayable(this PlayableGraph graph, GameObject prefabGameObject, Transform parentTransform)
		{
			PlayableHandle result;
			if (prefabGameObject == null)
			{
				result = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle = ScriptPlayableGraphExtensions.CreateScriptPlayable<PrefabControlPlayable>(graph);
				playableHandle.GetObject<PrefabControlPlayable>().Initialize(prefabGameObject, parentTransform);
				result = playableHandle;
			}
			return result;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008AEC File Offset: 0x00006CEC
		public static PlayableHandle CreateTimeControlPlayable(this PlayableGraph graph, ITimeControl timeControl)
		{
			PlayableHandle result;
			if (timeControl == null)
			{
				result = PlayableHandle.Null;
			}
			else
			{
				PlayableHandle playableHandle = ScriptPlayableGraphExtensions.CreateScriptPlayable<TimeControlPlayable>(graph);
				playableHandle.GetObject<TimeControlPlayable>().Initialize(timeControl);
				result = playableHandle;
			}
			return result;
		}
	}
}
