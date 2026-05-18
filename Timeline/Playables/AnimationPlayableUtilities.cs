using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Implements high-level utility methods to simplify use of the Playable API with Animations.</para>
	/// </summary>
	// Token: 0x0200022B RID: 555
	public class AnimationPlayableUtilities
	{
		/// <summary>
		///   <para>Plays the Playable on  the given Animator.</para>
		/// </summary>
		/// <param name="animator">Target Animator.</param>
		/// <param name="playable">The Playable that will be played.</param>
		/// <param name="graph">The Graph that owns the Playable.</param>
		// Token: 0x060024AB RID: 9387 RVA: 0x00029BF4 File Offset: 0x00027DF4
		public static void Play(Animator animator, PlayableHandle playable, PlayableGraph graph)
		{
            AnimationPlayableOutput output = graph.CreateAnimationOutput("AnimationClip", animator);
            output.sourcePlayable = playable;
            graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00029C28 File Offset: 0x00027E28
		public static PlayableHandle PlayClip(Animator animator, AnimationClip clip, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			AnimationPlayableOutput animationPlayableOutput = graph.CreateAnimationOutput("AnimationClip", animator);
			PlayableHandle playableHandle = graph.CreateAnimationClipPlayable(clip);
			animationPlayableOutput.sourcePlayable = playableHandle;
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return playableHandle;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00029C84 File Offset: 0x00027E84
		public static PlayableHandle PlayMixer(Animator animator, int inputCount, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			AnimationPlayableOutput animationPlayableOutput = graph.CreateAnimationOutput("Mixer", animator);
			PlayableHandle playableHandle = graph.CreateAnimationMixerPlayable(inputCount);
			animationPlayableOutput.sourcePlayable = playableHandle;
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return playableHandle;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00029CE0 File Offset: 0x00027EE0
		public static PlayableHandle PlayAnimatorController(Animator animator, RuntimeAnimatorController controller, out PlayableGraph graph)
		{
			graph = PlayableGraph.CreateGraph();
			AnimationPlayableOutput animationPlayableOutput = graph.CreateAnimationOutput("AnimatorControllerPlayable", animator);
			PlayableHandle playableHandle = graph.CreateAnimatorControllerPlayable(controller);
			animationPlayableOutput.sourcePlayable = playableHandle;
			graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
			return playableHandle;
		}
	}
}
