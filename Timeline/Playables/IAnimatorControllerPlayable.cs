using System;
using System.Collections.Generic;

namespace UnityEngine.Playables
{
	// Token: 0x0200025E RID: 606
	public interface IAnimatorControllerPlayable
	{
		/// <summary>
		///   <para>Gets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x060025FD RID: 9725
		float GetFloat(string name);

		/// <summary>
		///   <para>Gets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x060025FE RID: 9726
		float GetFloat(int id);

		/// <summary>
		///   <para>Sets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x060025FF RID: 9727
		void SetFloat(string name, float value);

		/// <summary>
		///   <para>Sets the value of a float parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002600 RID: 9728
		void SetFloat(int id, float value);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002601 RID: 9729
		bool GetBool(string name);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002602 RID: 9730
		bool GetBool(int id);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002603 RID: 9731
		void SetBool(string name, bool value);

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002604 RID: 9732
		void SetBool(int id, bool value);

		/// <summary>
		///   <para>Gets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002605 RID: 9733
		int GetInteger(string name);

		/// <summary>
		///   <para>Gets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002606 RID: 9734
		int GetInteger(int id);

		/// <summary>
		///   <para>Sets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002607 RID: 9735
		void SetInteger(string name, int value);

		/// <summary>
		///   <para>Sets the value of an integer parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002608 RID: 9736
		void SetInteger(int id, int value);

		/// <summary>
		///   <para>Sets a trigger parameter to active.
		/// A trigger parameter is a bool parameter that gets reset to false when it has been used in a transition. For state machines with multiple layers, the trigger will only get reset once all layers have been evaluated, so that the layers can synchronize their transitions on the same parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x06002609 RID: 9737
		void SetTrigger(string name);

		/// <summary>
		///   <para>Sets a trigger parameter to active.
		/// A trigger parameter is a bool parameter that gets reset to false when it has been used in a transition. For state machines with multiple layers, the trigger will only get reset once all layers have been evaluated, so that the layers can synchronize their transitions on the same parameter.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x0600260A RID: 9738
		void SetTrigger(int id);

		/// <summary>
		///   <para>Resets the trigger parameter to false.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x0600260B RID: 9739
		void ResetTrigger(string name);

		/// <summary>
		///   <para>Resets the trigger parameter to false.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x0600260C RID: 9740
		void ResetTrigger(int id);

		/// <summary>
		///   <para>Returns true if a parameter is controlled by an additional curve on an animation.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x0600260D RID: 9741
		bool IsParameterControlledByCurve(string name);

		/// <summary>
		///   <para>Returns true if a parameter is controlled by an additional curve on an animation.</para>
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="id">The id of the parameter. The id is generated using Animator::StringToHash.</param>
		// Token: 0x0600260E RID: 9742
		bool IsParameterControlledByCurve(int id);

		/// <summary>
		///   <para>The AnimatorController layer count.</para>
		/// </summary>
		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600260F RID: 9743
		int layerCount { get; }

		/// <summary>
		///   <para>Gets name of the layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002610 RID: 9744
		string GetLayerName(int layerIndex);

		/// <summary>
		///   <para>Gets the index of the layer with specified name.</para>
		/// </summary>
		/// <param name="layerName">The layer's name.</param>
		/// <returns>
		///   <para>The index of the layer.</para>
		/// </returns>
		// Token: 0x06002611 RID: 9745
		int GetLayerIndex(string layerName);

		/// <summary>
		///   <para>Gets the layer's current weight.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002612 RID: 9746
		float GetLayerWeight(int layerIndex);

		/// <summary>
		///   <para>Sets the layer's current weight.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		/// <param name="weight">The weight of the layer.</param>
		// Token: 0x06002613 RID: 9747
		void SetLayerWeight(int layerIndex, float weight);

		/// <summary>
		///   <para>Gets the current State information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002614 RID: 9748
		AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the next State information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002615 RID: 9749
		AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the Transition information on a specified AnimatorController layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002616 RID: 9750
		AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the list of AnimatorClipInfo currently played by the current state.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002617 RID: 9751
		AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>Gets the list of AnimatorClipInfo currently played by the next state.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x06002618 RID: 9752
		AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex);

		/// <summary>
		///   <para>This function should be used when you want to use the allocation-free method IAnimatorControllerPlayable.GetCurrentAnimatorClipInfo.</para>
		/// </summary>
		/// <param name="layerIndex">Layer's Index.</param>
		/// <returns>
		///   <para>Returns the count of AnimatorClipInfo for the current state.</para>
		/// </returns>
		// Token: 0x06002619 RID: 9753
		int GetCurrentAnimatorClipInfoCount(int layerIndex);

		// Token: 0x0600261A RID: 9754
		void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips);

		/// <summary>
		///   <para>This function should be used when you want to use the allocation-free method IAnimatorControllerPlayable.GetNextAnimatorClipInfo.</para>
		/// </summary>
		/// <param name="layerIndex">Layer's Index.</param>
		/// <returns>
		///   <para>Returns the count of AnimatorClipInfo for the next state.</para>
		/// </returns>
		// Token: 0x0600261B RID: 9755
		int GetNextAnimatorClipInfoCount(int layerIndex);

		// Token: 0x0600261C RID: 9756
		void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips);

		/// <summary>
		///   <para>Is the specified AnimatorController layer in a transition.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		// Token: 0x0600261D RID: 9757
		bool IsInTransition(int layerIndex);

		/// <summary>
		///   <para>The number of AnimatorControllerParameters used by the AnimatorController.</para>
		/// </summary>
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600261E RID: 9758
		int parameterCount { get; }

		/// <summary>
		///   <para>Read only access to the AnimatorControllerParameters used by the animator.</para>
		/// </summary>
		/// <param name="index">The index of the parameter.</param>
		// Token: 0x0600261F RID: 9759
		AnimatorControllerParameter GetParameter(int index);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.CrossFade, but the duration and offset in the target state are in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in seconds.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002620 RID: 9760
		void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer, float fixedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.CrossFade, but the duration and offset in the target state are in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in seconds.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002621 RID: 9761
		void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer, float fixedTime);

		/// <summary>
		///   <para>Creates a dynamic transition between the current state and the destination state.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in source state normalized time.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1.  If no explicit normalizedTime is specified or normalizedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002622 RID: 9762
		void CrossFade(string stateName, float transitionDuration, int layer, float normalizedTime);

		/// <summary>
		///   <para>Creates a dynamic transition between the current state and the destination state.</para>
		/// </summary>
		/// <param name="stateName">The name of the destination state.</param>
		/// <param name="transitionDuration">The duration of the transition. Value is in source state normalized time.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1.  If no explicit normalizedTime is specified or normalizedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002623 RID: 9763
		void CrossFade(int stateNameHash, float transitionDuration, int layer, float normalizedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.Play, but the offset in the target state is in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002624 RID: 9764
		void PlayInFixedTime(string stateName, int layer, float fixedTime);

		/// <summary>
		///   <para>Same as IAnimatorControllerPlayable.Play, but the offset in the target state is in fixed time.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="fixedTime">Start time of the current destination state. Value is in seconds. If no explicit fixedTime is specified or fixedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002625 RID: 9765
		void PlayInFixedTime(int stateNameHash, int layer, float fixedTime);

		/// <summary>
		///   <para>Plays a state.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in normalized time. If no explicit normalizedTime is specified or value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002626 RID: 9766
		void Play(string stateName, int layer, float normalizedTime);

		/// <summary>
		///   <para>Plays a state.</para>
		/// </summary>
		/// <param name="stateName">The name of the state to play.</param>
		/// <param name="layer">Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.</param>
		/// <param name="normalizedTime">Start time of the current destination state. Value is in normalized time. If no explicit normalizedTime is specified or value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.</param>
		/// <param name="stateNameHash">The AnimatorState fullPathHash, nameHash or shortNameHash to play. Passing 0 will transition to self.</param>
		// Token: 0x06002627 RID: 9767
		void Play(int stateNameHash, int layer, float normalizedTime);

		/// <summary>
		///   <para>Returns true if the AnimatorState is present in the Animator's controller. For a state named State in sub state machine SubStateMachine of state machine StateMachine, the shortNameHash can be generated using Animator.StringToHash("State"), and the fullPathHash can be generated using Animator.StringToHash("StateMachine.SubStateMachine.State"). Typically, the name of the top level state machine is the name of the Layer.</para>
		/// </summary>
		/// <param name="layerIndex">The layer's index.</param>
		/// <param name="stateID">The AnimatorState fullPathHash or shortNameHash.</param>
		// Token: 0x06002628 RID: 9768
		bool HasState(int layerIndex, int stateID);
	}
}
