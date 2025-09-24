using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Implements high-level utility methods to simplify use of the Playable API with Animations.</para>
	/// </summary>
	public class AnimationPlayableUtilities
	{
		/// <summary>
		///   <para>Plays the Playable on  the given Animator.</para>
		/// </summary>
		/// <param name="animator">Target Animator.</param>
		/// <param name="playable">The Playable that will be played.</param>
		/// <param name="graph">The Graph that owns the Playable.</param>
		public static void Play(Animator animator, PlayableHandle playable, PlayableGraph graph)
		{
            AnimationPlayableOutput animationPlayableOutput = graph.CreateAnimationOutput("AnimationClip", animator);
            animationPlayableOutput.sourcePlayable = playable;
            graph.SyncUpdateAndTimeMode(animator);
			graph.Play();
		}

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
