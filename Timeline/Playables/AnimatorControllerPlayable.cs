using System;
using System.Collections.Generic;
using UnityEngine.Internal;

namespace UnityEngine.Playables
{
    public class AnimatorControllerPlayable : AnimationPlayable
    {
        public static AnimatorControllerPlayable Create(
            PlayableGraph graph,
            RuntimeAnimatorController controller)
        {
            PlayableHandle handle =
                graph.CreateAnimatorControllerPlayable(controller);

            return handle.IsValid()
                ? handle.GetObject<AnimatorControllerPlayable>()
                : null;
        }

        public static implicit operator PlayableHandle(
            AnimatorControllerPlayable playable)
        {
            return playable.handle;
        }

        public RuntimeAnimatorController animatorController
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimatorController(handle);
            }
        }

        public float GetFloat(string name)
        {
            return LegacyPlayableRuntime.GetAnimatorFloat(
                handle,
                Animator.StringToHash(name));
        }

        public float GetFloat(int id)
        {
            return LegacyPlayableRuntime.GetAnimatorFloat(handle, id);
        }

        public void SetFloat(string name, float value)
        {
            SetFloat(Animator.StringToHash(name), value);
        }

        public void SetFloat(int id, float value)
        {
            LegacyPlayableRuntime.SetAnimatorFloat(handle, id, value);
        }

        public bool GetBool(string name)
        {
            return GetBool(Animator.StringToHash(name));
        }

        public bool GetBool(int id)
        {
            return LegacyPlayableRuntime.GetAnimatorBool(handle, id);
        }

        public void SetBool(string name, bool value)
        {
            SetBool(Animator.StringToHash(name), value);
        }

        public void SetBool(int id, bool value)
        {
            LegacyPlayableRuntime.SetAnimatorBool(handle, id, value);
        }

        public int GetInteger(string name)
        {
            return GetInteger(Animator.StringToHash(name));
        }

        public int GetInteger(int id)
        {
            return LegacyPlayableRuntime.GetAnimatorInteger(handle, id);
        }

        public void SetInteger(string name, int value)
        {
            SetInteger(Animator.StringToHash(name), value);
        }

        public void SetInteger(int id, int value)
        {
            LegacyPlayableRuntime.SetAnimatorInteger(handle, id, value);
        }

        public void SetTrigger(string name)
        {
            SetTrigger(Animator.StringToHash(name));
        }

        public void SetTrigger(int id)
        {
            LegacyPlayableRuntime.SetAnimatorTrigger(handle, id);
        }

        public void ResetTrigger(string name)
        {
            ResetTrigger(Animator.StringToHash(name));
        }

        public void ResetTrigger(int id)
        {
            LegacyPlayableRuntime.ResetAnimatorTrigger(handle, id);
        }

        public bool IsParameterControlledByCurve(string name)
        {
            return IsParameterControlledByCurve(
                Animator.StringToHash(name));
        }

        public bool IsParameterControlledByCurve(int id)
        {
            return LegacyPlayableRuntime.IsAnimatorParameterControlledByCurve(
                handle,
                id);
        }

        public int layerCount
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimatorLayerCount(handle);
            }
        }

        public string GetLayerName(int layerIndex)
        {
            return LegacyPlayableRuntime.GetAnimatorLayerName(
                handle,
                layerIndex);
        }

        public int GetLayerIndex(string layerName)
        {
            return LegacyPlayableRuntime.GetAnimatorLayerIndex(
                handle,
                layerName);
        }

        public float GetLayerWeight(int layerIndex)
        {
            return LegacyPlayableRuntime.GetAnimatorLayerWeight(
                handle,
                layerIndex);
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            LegacyPlayableRuntime.SetAnimatorLayerWeight(
                handle,
                layerIndex,
                weight);
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
        {
            return default(AnimatorStateInfo);
        }

        public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
        {
            return default(AnimatorStateInfo);
        }

        public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
        {
            return default(AnimatorTransitionInfo);
        }

        public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex)
        {
            return new AnimatorClipInfo[0];
        }

        public void GetCurrentAnimatorClipInfo(
            int layerIndex,
            List<AnimatorClipInfo> clips)
        {
            if (clips == null)
                throw new ArgumentNullException("clips");

            clips.Clear();
        }

        public void GetNextAnimatorClipInfo(
            int layerIndex,
            List<AnimatorClipInfo> clips)
        {
            if (clips == null)
                throw new ArgumentNullException("clips");

            clips.Clear();
        }

        public int GetCurrentAnimatorClipInfoCount(int layerIndex)
        {
            return 0;
        }

        public int GetNextAnimatorClipInfoCount(int layerIndex)
        {
            return 0;
        }

        public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex)
        {
            return new AnimatorClipInfo[0];
        }

        internal string ResolveHash(int hash)
        {
            return LegacyPlayableRuntime.ResolveAnimatorHash(
                handle,
                hash);
        }

        public bool IsInTransition(int layerIndex)
        {
            return LegacyPlayableRuntime.IsAnimatorInTransition(
                handle,
                layerIndex);
        }

        public int parameterCount
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimatorParameterCount(handle);
            }
        }

        public AnimatorControllerParameter GetParameter(int index)
        {
            AnimatorControllerParameter[] parameters =
                LegacyPlayableRuntime.GetAnimatorParameters(handle);

            if (index < 0 || index >= parameters.Length)
                throw new IndexOutOfRangeException("index");

            return parameters[index];
        }

        [ExcludeFromDocs]
        public void CrossFadeInFixedTime(
            string stateName,
            float transitionDuration,
            int layer)
        {
            CrossFadeInFixedTime(
                stateName,
                transitionDuration,
                layer,
                0f);
        }

        [ExcludeFromDocs]
        public void CrossFadeInFixedTime(
            string stateName,
            float transitionDuration)
        {
            CrossFadeInFixedTime(
                stateName,
                transitionDuration,
                -1,
                0f);
        }

        public void CrossFadeInFixedTime(
            string stateName,
            float transitionDuration,
            [DefaultValue("-1")] int layer,
            [DefaultValue("0.0f")] float fixedTime)
        {
            CrossFadeInFixedTime(
                Animator.StringToHash(stateName),
                transitionDuration,
                layer,
                fixedTime);
        }

        [ExcludeFromDocs]
        public void CrossFadeInFixedTime(
            int stateNameHash,
            float transitionDuration,
            int layer)
        {
            CrossFadeInFixedTime(
                stateNameHash,
                transitionDuration,
                layer,
                0f);
        }

        [ExcludeFromDocs]
        public void CrossFadeInFixedTime(
            int stateNameHash,
            float transitionDuration)
        {
            CrossFadeInFixedTime(
                stateNameHash,
                transitionDuration,
                -1,
                0f);
        }

        public void CrossFadeInFixedTime(
            int stateNameHash,
            float transitionDuration,
            [DefaultValue("-1")] int layer,
            [DefaultValue("0.0f")] float fixedTime)
        {
            LegacyPlayableRuntime.AnimatorCrossFadeInFixedTime(
                handle,
                stateNameHash,
                transitionDuration,
                layer,
                fixedTime);
        }

        [ExcludeFromDocs]
        public void CrossFade(
            string stateName,
            float transitionDuration,
            int layer)
        {
            CrossFade(
                stateName,
                transitionDuration,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void CrossFade(
            string stateName,
            float transitionDuration)
        {
            CrossFade(
                stateName,
                transitionDuration,
                -1,
                float.NegativeInfinity);
        }

        public void CrossFade(
            string stateName,
            float transitionDuration,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float normalizedTime)
        {
            CrossFade(
                Animator.StringToHash(stateName),
                transitionDuration,
                layer,
                normalizedTime);
        }

        [ExcludeFromDocs]
        public void CrossFade(
            int stateNameHash,
            float transitionDuration,
            int layer)
        {
            CrossFade(
                stateNameHash,
                transitionDuration,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void CrossFade(
            int stateNameHash,
            float transitionDuration)
        {
            CrossFade(
                stateNameHash,
                transitionDuration,
                -1,
                float.NegativeInfinity);
        }

        public void CrossFade(
            int stateNameHash,
            float transitionDuration,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float normalizedTime)
        {
            LegacyPlayableRuntime.AnimatorCrossFade(
                handle,
                stateNameHash,
                transitionDuration,
                layer,
                normalizedTime);
        }

        [ExcludeFromDocs]
        public void PlayInFixedTime(string stateName, int layer)
        {
            PlayInFixedTime(
                stateName,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void PlayInFixedTime(string stateName)
        {
            PlayInFixedTime(
                stateName,
                -1,
                float.NegativeInfinity);
        }

        public void PlayInFixedTime(
            string stateName,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float fixedTime)
        {
            PlayInFixedTime(
                Animator.StringToHash(stateName),
                layer,
                fixedTime);
        }

        [ExcludeFromDocs]
        public void PlayInFixedTime(int stateNameHash, int layer)
        {
            PlayInFixedTime(
                stateNameHash,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void PlayInFixedTime(int stateNameHash)
        {
            PlayInFixedTime(
                stateNameHash,
                -1,
                float.NegativeInfinity);
        }

        public void PlayInFixedTime(
            int stateNameHash,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float fixedTime)
        {
            LegacyPlayableRuntime.AnimatorPlayInFixedTime(
                handle,
                stateNameHash,
                layer,
                fixedTime);
        }

        [ExcludeFromDocs]
        public void Play(string stateName, int layer)
        {
            Play(
                stateName,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void Play(string stateName)
        {
            Play(
                stateName,
                -1,
                float.NegativeInfinity);
        }

        public void Play(
            string stateName,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float normalizedTime)
        {
            Play(
                Animator.StringToHash(stateName),
                layer,
                normalizedTime);
        }

        [ExcludeFromDocs]
        public void Play(int stateNameHash, int layer)
        {
            Play(
                stateNameHash,
                layer,
                float.NegativeInfinity);
        }

        [ExcludeFromDocs]
        public void Play(int stateNameHash)
        {
            Play(
                stateNameHash,
                -1,
                float.NegativeInfinity);
        }

        public void Play(
            int stateNameHash,
            [DefaultValue("-1")] int layer,
            [DefaultValue("float.NegativeInfinity")] float normalizedTime)
        {
            LegacyPlayableRuntime.AnimatorPlay(
                handle,
                stateNameHash,
                layer,
                normalizedTime);
        }

        public bool HasState(int layerIndex, int stateID)
        {
            return LegacyPlayableRuntime.AnimatorHasState(
                handle,
                layerIndex,
                stateID);
        }
    }
}
