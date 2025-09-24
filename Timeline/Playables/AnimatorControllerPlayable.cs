using HorrorEngine;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Playable that plays a RuntimeAnimatorController. Can be used as an input to an AnimationPlayable.</para>
	/// </summary>
	public class AnimatorControllerPlayable : AnimationPlayable
	{
        // Controller per handle (if you wire this elsewhere).
        private static readonly Dictionary<PlayableHandle, Animator> s_Controllers =
            new Dictionary<PlayableHandle, Animator>();

        public static implicit operator PlayableHandle(AnimatorControllerPlayable b)
		{
			return b.handle;
		}

		private static RuntimeAnimatorController GetAnimatorControllerInternal(ref PlayableHandle handle)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetAnimatorControllerInternal(ref handle);
		}

        private static RuntimeAnimatorController INTERNAL_CALL_GetAnimatorControllerInternal(ref PlayableHandle handle)
        {
            Animator ctrl;
            if (s_Controllers.TryGetValue(handle, out ctrl))
                return ctrl.runtimeAnimatorController;
            return null;
        }

        private static Animator GetAnimatorController(ref PlayableHandle handle)
        {
            Animator ctrl;
            if (s_Controllers.TryGetValue(handle, out ctrl))
                return ctrl;

            return null;
        }

        /// <summary>
        ///   <para>See IAnimatorControllerPlayable.GetFloat.</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public float GetFloat(string name)
		{
			return AnimatorControllerPlayable.GetFloatString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public float GetFloat(int id)
		{
			return AnimatorControllerPlayable.GetFloatID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetFloat(string name, float value)
		{
			AnimatorControllerPlayable.SetFloatString(ref this.handle, name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetFloat.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetFloat(int id, float value)
		{
			AnimatorControllerPlayable.SetFloatID(ref this.handle, id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool GetBool(string name)
		{
			return AnimatorControllerPlayable.GetBoolString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public bool GetBool(int id)
		{
			return AnimatorControllerPlayable.GetBoolID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetBool(string name, bool value)
		{
			AnimatorControllerPlayable.SetBoolString(ref this.handle, name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetBool.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>		
		public void SetBool(int id, bool value)
		{
			AnimatorControllerPlayable.SetBoolID(ref this.handle, id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public int GetInteger(string name)
		{
			return AnimatorControllerPlayable.GetIntegerString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		public int GetInteger(int id)
		{
			return AnimatorControllerPlayable.GetIntegerID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		public void SetInteger(string name, int value)
		{
			AnimatorControllerPlayable.SetIntegerString(ref this.handle, name, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetInteger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="id"></param>
		// Token: 0x06002638 RID: 9784 RVA: 0x0002B398 File Offset: 0x00029598
		public void SetInteger(int id, int value)
		{
			AnimatorControllerPlayable.SetIntegerID(ref this.handle, id, value);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x06002639 RID: 9785 RVA: 0x0002B3A8 File Offset: 0x000295A8
		public void SetTrigger(string name)
		{
			AnimatorControllerPlayable.SetTriggerString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x0600263A RID: 9786 RVA: 0x0002B3B8 File Offset: 0x000295B8
		public void SetTrigger(int id)
		{
			AnimatorControllerPlayable.SetTriggerID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.ResetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x0600263B RID: 9787 RVA: 0x0002B3C8 File Offset: 0x000295C8
		public void ResetTrigger(string name)
		{
			AnimatorControllerPlayable.ResetTriggerString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.ResetTrigger.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x0600263C RID: 9788 RVA: 0x0002B3D8 File Offset: 0x000295D8
		public void ResetTrigger(int id)
		{
			AnimatorControllerPlayable.ResetTriggerID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsParameterControlledByCurve.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x0600263D RID: 9789 RVA: 0x0002B3E8 File Offset: 0x000295E8
		public bool IsParameterControlledByCurve(string name)
		{
			return AnimatorControllerPlayable.IsParameterControlledByCurveString(ref this.handle, name);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsParameterControlledByCurve.</para>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		// Token: 0x0600263E RID: 9790 RVA: 0x0002B40C File Offset: 0x0002960C
		public bool IsParameterControlledByCurve(int id)
		{
			return AnimatorControllerPlayable.IsParameterControlledByCurveID(ref this.handle, id);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.layerCount.</para>
		/// </summary>
		public int layerCount
		{
			get
			{
				return AnimatorControllerPlayable.GetLayerCountInternal(ref this.handle);
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0002B450 File Offset: 0x00029650
		private static int GetLayerCountInternal(ref PlayableHandle handle)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetLayerCountInternal(ref handle);
		}

        private static int INTERNAL_CALL_GetLayerCountInternal(ref PlayableHandle handle)
		{
            Animator controller = GetAnimatorController(ref handle);
			if (controller != null)
			{
				return controller.layerCount;
            }

			return 0;
        }

        // Token: 0x06002642 RID: 9794 RVA: 0x0002B46C File Offset: 0x0002966C
        private static string GetLayerNameInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetLayerNameInternal(ref handle, layerIndex);
		}

		private static string INTERNAL_CALL_GetLayerNameInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetLayerName(layerIndex);
            }

            return "";
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerName.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public string GetLayerName(int layerIndex)
		{
			return AnimatorControllerPlayable.GetLayerNameInternal(ref this.handle, layerIndex);
		}

		private static int GetLayerIndexInternal(ref PlayableHandle handle, string layerName)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetLayerIndexInternal(ref handle, layerName);
		}

		private static int INTERNAL_CALL_GetLayerIndexInternal(ref PlayableHandle handle, string layerName)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetLayerIndex(layerName);
            }

			return 0;
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerIndex.</para>
		/// </summary>
		/// <param name="layerName"></param>
		// Token: 0x06002647 RID: 9799 RVA: 0x0002B4C8 File Offset: 0x000296C8
		public int GetLayerIndex(string layerName)
		{
			return AnimatorControllerPlayable.GetLayerIndexInternal(ref this.handle, layerName);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x0002B4EC File Offset: 0x000296EC
		private static float GetLayerWeightInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetLayerWeightInternal(ref handle, layerIndex);
		}

		private static float INTERNAL_CALL_GetLayerWeightInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetLayerWeight(layerIndex);
            }

            return 0f;
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetLayerWeight.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public float GetLayerWeight(int layerIndex)
		{
			return AnimatorControllerPlayable.GetLayerWeightInternal(ref this.handle, layerIndex);
		}

		private static void SetLayerWeightInternal(ref PlayableHandle handle, int layerIndex, float weight)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetLayerWeightInternal(ref handle, layerIndex, weight);
		}

		private static void INTERNAL_CALL_SetLayerWeightInternal(ref PlayableHandle handle, int layerIndex, float weight)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetLayerWeight(layerIndex, weight);
            }
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.SetLayerWeight.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <param name="weight"></param>
		public void SetLayerWeight(int layerIndex, float weight)
		{
			AnimatorControllerPlayable.SetLayerWeightInternal(ref this.handle, layerIndex, weight);
		}

		private static AnimatorStateInfo GetCurrentAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetCurrentAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorStateInfo INTERNAL_CALL_GetCurrentAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				return controller.GetCurrentAnimatorStateInfo(layerIndex);
            }
			return new AnimatorStateInfo();
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetCurrentAnimatorStateInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
		{
			return AnimatorControllerPlayable.GetCurrentAnimatorStateInfoInternal(ref this.handle, layerIndex);
		}

		private static AnimatorStateInfo GetNextAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetNextAnimatorStateInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorStateInfo INTERNAL_CALL_GetNextAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetNextAnimatorStateInfo(layerIndex);
            }
            return new AnimatorStateInfo();
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetNextAnimatorStateInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
		{
			return AnimatorControllerPlayable.GetNextAnimatorStateInfoInternal(ref this.handle, layerIndex);
		}

		private static AnimatorTransitionInfo GetAnimatorTransitionInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetAnimatorTransitionInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorTransitionInfo INTERNAL_CALL_GetAnimatorTransitionInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetAnimatorTransitionInfo(layerIndex);
            }
            return new AnimatorTransitionInfo();

        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetAnimatorTransitionInfo.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
		{
			return AnimatorControllerPlayable.GetAnimatorTransitionInfoInternal(ref this.handle, layerIndex);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0002B608 File Offset: 0x00029808
		private static AnimatorClipInfo[] GetCurrentAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetCurrentAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorClipInfo[] INTERNAL_CALL_GetCurrentAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetCurrentAnimatorClipInfo(layerIndex);
            }
            return new AnimatorClipInfo[0];

        }

        /// <summary>
        ///   <para>See IAnimatorControllerPlayable.GetCurrentAnimatorClipInfo.</para>
        /// </summary>
        /// <param name="layerIndex"></param>
        public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex)
		{
			return AnimatorControllerPlayable.GetCurrentAnimatorClipInfoInternal(ref this.handle, layerIndex);
		}

		public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			this.GetAnimatorClipInfoInternal(ref this.handle, layerIndex, true, clips);
		}

		public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
		{
			if (clips == null)
			{
				throw new ArgumentNullException("clips");
			}
			this.GetAnimatorClipInfoInternal(ref this.handle, layerIndex, false, clips);
		}

		private void GetAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex, bool isCurrent, object clips)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_GetAnimatorClipInfoInternal(this, ref handle, layerIndex, isCurrent, clips);
		}

		private static void INTERNAL_CALL_GetAnimatorClipInfoInternal(AnimatorControllerPlayable self, ref PlayableHandle handle, int layerIndex, bool isCurrent, object clips)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                var clipInfo = controller.GetCurrentAnimatorClipInfo(layerIndex);
            }
        }

		private static int GetAnimatorClipInfoCountInternal(ref PlayableHandle handle, int layerIndex, bool current)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetAnimatorClipInfoCountInternal(ref handle, layerIndex, current);
		}

		private static int INTERNAL_CALL_GetAnimatorClipInfoCountInternal(ref PlayableHandle handle, int layerIndex, bool current)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetCurrentAnimatorClipInfoCount(layerIndex);
            }
			return 0;
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetCurrentAnimatorClipInfoCount.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public int GetCurrentAnimatorClipInfoCount(int layerIndex)
		{
			return AnimatorControllerPlayable.GetAnimatorClipInfoCountInternal(ref this.handle, layerIndex, true);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.GetNextAnimatorClipInfoCount.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		public int GetNextAnimatorClipInfoCount(int layerIndex)
		{
			return AnimatorControllerPlayable.GetAnimatorClipInfoCountInternal(ref this.handle, layerIndex, false);
		}

		private static AnimatorClipInfo[] GetNextAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetNextAnimatorClipInfoInternal(ref handle, layerIndex);
		}

		private static AnimatorClipInfo[] INTERNAL_CALL_GetNextAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetNextAnimatorClipInfo(layerIndex);
            }
            return new AnimatorClipInfo[0];
        }

        /// <summary>
        ///   <para>See IAnimatorControllerPlayable.GetNextAnimatorClipInfo.</para>
        /// </summary>
        /// <param name="layerIndex"></param>

        public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex)
		{
			return AnimatorControllerPlayable.GetNextAnimatorClipInfoInternal(ref this.handle, layerIndex);
		}

		internal string ResolveHash(int hash)
		{
			return AnimatorControllerPlayable.ResolveHashInternal(ref this.handle, hash);
		}


		private static string ResolveHashInternal(ref PlayableHandle handle, int hash)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_ResolveHashInternal(ref handle, hash);
		}

		private static string INTERNAL_CALL_ResolveHashInternal(ref PlayableHandle handle, int hash)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				//TODO
            }

			return "";
        }

		private static bool IsInTransitionInternal(ref PlayableHandle handle, int layerIndex)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_IsInTransitionInternal(ref handle, layerIndex);
		}


		private static bool INTERNAL_CALL_IsInTransitionInternal(ref PlayableHandle handle, int layerIndex)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				return controller.IsInTransition(layerIndex);
            }
			return false;
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.IsInTransition.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		// Token: 0x0600266A RID: 9834 RVA: 0x0002B7A4 File Offset: 0x000299A4
		public bool IsInTransition(int layerIndex)
		{
			return AnimatorControllerPlayable.IsInTransitionInternal(ref this.handle, layerIndex);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0002B7C8 File Offset: 0x000299C8
		private static int GetParameterCountInternal(ref PlayableHandle handle)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetParameterCountInternal(ref handle);
		}

		private static int INTERNAL_CALL_GetParameterCountInternal(ref PlayableHandle handle)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				return controller.parameterCount;
            }
            return 0;
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.parameterCount.</para>
		/// </summary>
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x0002B7E4 File Offset: 0x000299E4
		public int parameterCount
		{
			get
			{
				return AnimatorControllerPlayable.GetParameterCountInternal(ref this.handle);
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0002B804 File Offset: 0x00029A04
		private static AnimatorControllerParameter[] GetParametersArrayInternal(ref PlayableHandle handle)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetParametersArrayInternal(ref handle);
		}

		private static AnimatorControllerParameter[] INTERNAL_CALL_GetParametersArrayInternal(ref PlayableHandle handle)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.parameters;
            }
            return new AnimatorControllerParameter[0];
        }

		/// <summary>
		///   <para>See AnimatorController.GetParameter.</para>
		/// </summary>
		/// <param name="index"></param>
		// Token: 0x06002670 RID: 9840 RVA: 0x0002B820 File Offset: 0x00029A20
		public AnimatorControllerParameter GetParameter(int index)
		{
			AnimatorControllerParameter[] parametersArrayInternal = AnimatorControllerPlayable.GetParametersArrayInternal(ref this.handle);
			if (index < 0 && index >= parametersArrayInternal.Length)
			{
				throw new IndexOutOfRangeException("index");
			}
			return parametersArrayInternal[index];
		}

		private static int StringToHash(string name)
		{
            if (string.IsNullOrEmpty(name))
                return 0;

            // Unity uses a 32-bit FNV-1a hash for Animator names/parameters
            uint hash = 2166136261u;
            for (int i = 0; i < name.Length; i++)
            {
                hash ^= (uint)name[i];
                hash *= 16777619u;
            }

            return (int)hash;
        }

		// Token: 0x06002672 RID: 9842 RVA: 0x0002B860 File Offset: 0x00029A60
		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			this.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0002B880 File Offset: 0x00029A80
		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(string stateName, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			this.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFadeInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002674 RID: 9844 RVA: 0x0002B8A0 File Offset: 0x00029AA0
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			AnimatorControllerPlayable.CrossFadeInFixedTimeInternal(ref this.handle, AnimatorControllerPlayable.StringToHash(stateName), transitionDuration, layer, fixedTime);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0002B8B8 File Offset: 0x00029AB8
		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			this.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0002B8D8 File Offset: 0x00029AD8
		[ExcludeFromDocs]
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			this.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFadeInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002677 RID: 9847 RVA: 0x0002B8F8 File Offset: 0x00029AF8
		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			AnimatorControllerPlayable.CrossFadeInFixedTimeInternal(ref this.handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0002B90C File Offset: 0x00029B0C
		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0002B91C File Offset: 0x00029B1C
		[ExcludeFromDocs]
		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer)
		{
			float fixedTime = 0f;
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0002B93C File Offset: 0x00029B3C
		[ExcludeFromDocs]
		private static void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration)
		{
			float fixedTime = 0f;
			int layer = -1;
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref handle, stateNameHash, transitionDuration, layer, fixedTime);
		}

		private static void INTERNAL_CALL_CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float fixedTime)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
            }
        }

		// Token: 0x0600267C RID: 9852 RVA: 0x0002B95C File Offset: 0x00029B5C
		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.CrossFade(stateName, transitionDuration, layer, negativeInfinity);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0002B97C File Offset: 0x00029B7C
		[ExcludeFromDocs]
		public void CrossFade(string stateName, float transitionDuration)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.CrossFade(stateName, transitionDuration, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFade.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x0600267E RID: 9854 RVA: 0x0002B99C File Offset: 0x00029B9C
		public void CrossFade(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.CrossFadeInternal(ref this.handle, AnimatorControllerPlayable.StringToHash(stateName), transitionDuration, layer, normalizedTime);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0002B9B4 File Offset: 0x00029BB4
		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.CrossFade(stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0002B9D4 File Offset: 0x00029BD4
		[ExcludeFromDocs]
		public void CrossFade(int stateNameHash, float transitionDuration)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.CrossFade(stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.CrossFade.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="transitionDuration"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002681 RID: 9857 RVA: 0x0002B9F4 File Offset: 0x00029BF4
		public void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.CrossFadeInternal(ref this.handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0002BA08 File Offset: 0x00029C08
		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, normalizedTime);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0002BA18 File Offset: 0x00029C18
		[ExcludeFromDocs]
		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0002BA38 File Offset: 0x00029C38
		[ExcludeFromDocs]
		private static void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			AnimatorControllerPlayable.INTERNAL_CALL_CrossFadeInternal(ref handle, stateNameHash, transitionDuration, layer, negativeInfinity);
		}

		private static void INTERNAL_CALL_CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float normalizedTime)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				controller.CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
            }
        }

		// Token: 0x06002686 RID: 9862 RVA: 0x0002BA58 File Offset: 0x00029C58
		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.PlayInFixedTime(stateName, layer, negativeInfinity);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0002BA78 File Offset: 0x00029C78
		[ExcludeFromDocs]
		public void PlayInFixedTime(string stateName)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.PlayInFixedTime(stateName, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.PlayInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002688 RID: 9864 RVA: 0x0002BA98 File Offset: 0x00029C98
		public void PlayInFixedTime(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			AnimatorControllerPlayable.PlayInFixedTimeInternal(ref this.handle, AnimatorControllerPlayable.StringToHash(stateName), layer, fixedTime);
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0002BAB0 File Offset: 0x00029CB0
		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.PlayInFixedTime(stateNameHash, layer, negativeInfinity);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0002BAD0 File Offset: 0x00029CD0
		[ExcludeFromDocs]
		public void PlayInFixedTime(int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.PlayInFixedTime(stateNameHash, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.PlayInFixedTime.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="fixedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x0600268B RID: 9867 RVA: 0x0002BAF0 File Offset: 0x00029CF0
		public void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			AnimatorControllerPlayable.PlayInFixedTimeInternal(ref this.handle, stateNameHash, layer, fixedTime);
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0002BB04 File Offset: 0x00029D04
		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, fixedTime);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0002BB10 File Offset: 0x00029D10
		[ExcludeFromDocs]
		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, negativeInfinity);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0002BB30 File Offset: 0x00029D30
		[ExcludeFromDocs]
		private static void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInFixedTimeInternal(ref handle, stateNameHash, layer, negativeInfinity);
		}

		private static void INTERNAL_CALL_PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, int layer, float fixedTime)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.PlayInFixedTime(stateNameHash, layer, fixedTime);
            }
        }

		// Token: 0x06002690 RID: 9872 RVA: 0x0002BB50 File Offset: 0x00029D50
		[ExcludeFromDocs]
		public void Play(string stateName, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.Play(stateName, layer, negativeInfinity);
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0002BB70 File Offset: 0x00029D70
		[ExcludeFromDocs]
		public void Play(string stateName)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.Play(stateName, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.Play.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002692 RID: 9874 RVA: 0x0002BB90 File Offset: 0x00029D90
		public void Play(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.PlayInternal(ref this.handle, AnimatorControllerPlayable.StringToHash(stateName), layer, normalizedTime);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0002BBA8 File Offset: 0x00029DA8
		[ExcludeFromDocs]
		public void Play(int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			this.Play(stateNameHash, layer, negativeInfinity);
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0002BBC8 File Offset: 0x00029DC8
		[ExcludeFromDocs]
		public void Play(int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			this.Play(stateNameHash, layer, negativeInfinity);
		}

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.Play.</para>
		/// </summary>
		/// <param name="stateName"></param>
		/// <param name="layer"></param>
		/// <param name="normalizedTime"></param>
		/// <param name="stateNameHash"></param>
		// Token: 0x06002695 RID: 9877 RVA: 0x0002BBE8 File Offset: 0x00029DE8
		public void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.PlayInternal(ref this.handle, stateNameHash, layer, normalizedTime);
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0002BBFC File Offset: 0x00029DFC
		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, normalizedTime);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0002BC08 File Offset: 0x00029E08
		[ExcludeFromDocs]
		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash, int layer)
		{
			float negativeInfinity = float.NegativeInfinity;
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, negativeInfinity);
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0002BC28 File Offset: 0x00029E28
		[ExcludeFromDocs]
		private static void PlayInternal(ref PlayableHandle handle, int stateNameHash)
		{
			float negativeInfinity = float.NegativeInfinity;
			int layer = -1;
			AnimatorControllerPlayable.INTERNAL_CALL_PlayInternal(ref handle, stateNameHash, layer, negativeInfinity);
		}

		private static void INTERNAL_CALL_PlayInternal(ref PlayableHandle handle, int stateNameHash, int layer, float normalizedTime)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.Play(stateNameHash, layer, normalizedTime);
            }
        }

		/// <summary>
		///   <para>See IAnimatorControllerPlayable.HasState.</para>
		/// </summary>
		/// <param name="layerIndex"></param>
		/// <param name="stateID"></param>
		// Token: 0x0600269A RID: 9882 RVA: 0x0002BC48 File Offset: 0x00029E48
		public bool HasState(int layerIndex, int stateID)
		{
			return AnimatorControllerPlayable.HasStateInternal(ref this.handle, layerIndex, stateID);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0002BC6C File Offset: 0x00029E6C
		private static bool HasStateInternal(ref PlayableHandle handle, int layerIndex, int stateID)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_HasStateInternal(ref handle, layerIndex, stateID);
		}

		private static bool INTERNAL_CALL_HasStateInternal(ref PlayableHandle handle, int layerIndex, int stateID)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.HasState(layerIndex, stateID);
            }
			return false;
        }

		// Token: 0x0600269D RID: 9885 RVA: 0x0002BC8C File Offset: 0x00029E8C
		private static void SetFloatString(ref PlayableHandle handle, string name, float value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetFloatString(ref handle, name, value);
		}

		private static void INTERNAL_CALL_SetFloatString(ref PlayableHandle handle, string name, float value)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetFloat(name, value);
            }
        }

		// Token: 0x0600269F RID: 9887 RVA: 0x0002BC98 File Offset: 0x00029E98
		private static void SetFloatID(ref PlayableHandle handle, int id, float value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetFloatID(ref handle, id, value);
		}

		private static void INTERNAL_CALL_SetFloatID(ref PlayableHandle handle, int id, float value)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetFloat(id, value);
            }
        }

        // Token: 0x060026A1 RID: 9889 RVA: 0x0002BCA4 File Offset: 0x00029EA4
        private static float GetFloatString(ref PlayableHandle handle, string name)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetFloatString(ref handle, name);
		}

		private static float INTERNAL_CALL_GetFloatString(ref PlayableHandle handle, string name)
		{ 
			Animator controller = GetAnimatorController(ref handle);
			if (controller != null)
			{
				return controller.GetFloat(name);
			}
            
            return 0f;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0002BCC0 File Offset: 0x00029EC0
		private static float GetFloatID(ref PlayableHandle handle, int id)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetFloatID(ref handle, id);
		}

		private static float INTERNAL_CALL_GetFloatID(ref PlayableHandle handle, int id)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetFloat(id);
            }

            return 0f;
        }

		// Token: 0x060026A5 RID: 9893 RVA: 0x0002BCDC File Offset: 0x00029EDC
		private static void SetBoolString(ref PlayableHandle handle, string name, bool value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetBoolString(ref handle, name, value);
		}

		private static void INTERNAL_CALL_SetBoolString(ref PlayableHandle handle, string name, bool value)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetBool(name, value);
            }
        }

        // Token: 0x060026A7 RID: 9895 RVA: 0x0002BCE8 File Offset: 0x00029EE8
        private static void SetBoolID(ref PlayableHandle handle, int id, bool value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetBoolID(ref handle, id, value);
		}

		private static void INTERNAL_CALL_SetBoolID(ref PlayableHandle handle, int id, bool value)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetBool(id, value);
            }
        }

        // Token: 0x060026A9 RID: 9897 RVA: 0x0002BCF4 File Offset: 0x00029EF4
        private static bool GetBoolString(ref PlayableHandle handle, string name)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetBoolString(ref handle, name);
		}

		private static bool INTERNAL_CALL_GetBoolString(ref PlayableHandle handle, string name)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetBool(name);
            }
			return false;
        }

		// Token: 0x060026AB RID: 9899 RVA: 0x0002BD10 File Offset: 0x00029F10
		private static bool GetBoolID(ref PlayableHandle handle, int id)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetBoolID(ref handle, id);
		}

		private static bool INTERNAL_CALL_GetBoolID(ref PlayableHandle handle, int id)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                return controller.GetBool(id);
            }
            return false;
        }

        // Token: 0x060026AD RID: 9901 RVA: 0x0002BD2C File Offset: 0x00029F2C
        private static void SetIntegerString(ref PlayableHandle handle, string name, int value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetIntegerString(ref handle, name, value);
		}

		private static void INTERNAL_CALL_SetIntegerString(ref PlayableHandle handle, string name, int value)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetInteger(name, value);
            }
        }

		// Token: 0x060026AF RID: 9903 RVA: 0x0002BD38 File Offset: 0x00029F38
		private static void SetIntegerID(ref PlayableHandle handle, int id, int value)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetIntegerID(ref handle, id, value);
		}

		private static void INTERNAL_CALL_SetIntegerID(ref PlayableHandle handle, int id, int value)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetInteger(id, value);
            }
        }

		// Token: 0x060026B1 RID: 9905 RVA: 0x0002BD44 File Offset: 0x00029F44
		private static int GetIntegerString(ref PlayableHandle handle, string name)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetIntegerString(ref handle, name);
		}

		private static int INTERNAL_CALL_GetIntegerString(ref PlayableHandle handle, string name)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.GetInteger(name);
            }
			return 0;
        }

		// Token: 0x060026B3 RID: 9907 RVA: 0x0002BD60 File Offset: 0x00029F60
		private static int GetIntegerID(ref PlayableHandle handle, int id)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_GetIntegerID(ref handle, id);
		}

		private static int INTERNAL_CALL_GetIntegerID(ref PlayableHandle handle, int id)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.GetInteger(id);
            }
            return 0;
        }

        // Token: 0x060026B5 RID: 9909 RVA: 0x0002BD7C File Offset: 0x00029F7C
        private static void SetTriggerString(ref PlayableHandle handle, string name)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetTriggerString(ref handle, name);
		}

		private static void INTERNAL_CALL_SetTriggerString(ref PlayableHandle handle, string name)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetTrigger(name);
            }
        }

        // Token: 0x060026B7 RID: 9911 RVA: 0x0002BD88 File Offset: 0x00029F88
        private static void SetTriggerID(ref PlayableHandle handle, int id)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_SetTriggerID(ref handle, id);
		}

		private static void INTERNAL_CALL_SetTriggerID(ref PlayableHandle handle, int id)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.SetTrigger(id);
            }
        }

        // Token: 0x060026B9 RID: 9913 RVA: 0x0002BD94 File Offset: 0x00029F94
        private static void ResetTriggerString(ref PlayableHandle handle, string name)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_ResetTriggerString(ref handle, name);
		}

		private static void INTERNAL_CALL_ResetTriggerString(ref PlayableHandle handle, string name)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
				controller.ResetTrigger(name);
            }
        }
        // Token: 0x060026BB RID: 9915 RVA: 0x0002BDA0 File Offset: 0x00029FA0
        private static void ResetTriggerID(ref PlayableHandle handle, int id)
		{
			AnimatorControllerPlayable.INTERNAL_CALL_ResetTriggerID(ref handle, id);
		}

		private static void INTERNAL_CALL_ResetTriggerID(ref PlayableHandle handle, int id)
        {
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.ResetTrigger(id);
            }
        }

        // Token: 0x060026BD RID: 9917 RVA: 0x0002BDAC File Offset: 0x00029FAC
        private static bool IsParameterControlledByCurveString(ref PlayableHandle handle, string name)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_IsParameterControlledByCurveString(ref handle, name);
		}

		private static bool INTERNAL_CALL_IsParameterControlledByCurveString(ref PlayableHandle handle, string name)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.IsParameterControlledByCurve(name);
            }

			return false;
        }

		// Token: 0x060026BF RID: 9919 RVA: 0x0002BDC8 File Offset: 0x00029FC8
		private static bool IsParameterControlledByCurveID(ref PlayableHandle handle, int id)
		{
			return AnimatorControllerPlayable.INTERNAL_CALL_IsParameterControlledByCurveID(ref handle, id);
		}

		private static bool INTERNAL_CALL_IsParameterControlledByCurveID(ref PlayableHandle handle, int id)
		{
            Animator controller = GetAnimatorController(ref handle);
            if (controller != null)
            {
                controller.IsParameterControlledByCurve(id);
            }

            return false;
        }
	}
}
