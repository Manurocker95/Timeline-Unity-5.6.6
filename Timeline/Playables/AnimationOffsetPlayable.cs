using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
    internal sealed class AnimationOffsetPlayable : AnimationPlayable
	{
        private static readonly Dictionary<PlayableHandle, Vector3> s_Positions = new Dictionary<PlayableHandle, Vector3>();
        private static readonly Dictionary<PlayableHandle, Quaternion> s_Rotations = new Dictionary<PlayableHandle, Quaternion>();

        public Vector3 position
		{
			get
			{
				return AnimationOffsetPlayable.GetPosition(ref this.handle);
			}
			set
			{
				AnimationOffsetPlayable.SetPosition(ref this.handle, value);
			}
		}

		public Quaternion rotation
		{
			get
			{
				return AnimationOffsetPlayable.GetRotation(ref this.handle);
			}
			set
			{
				AnimationOffsetPlayable.SetRotation(ref this.handle, value);
			}
		}

		private static Vector3 GetPosition(ref PlayableHandle handle)
		{
			Vector3 result;
			AnimationOffsetPlayable.INTERNAL_CALL_GetPosition(ref handle, out result);
			return result;
		}

        private static void INTERNAL_CALL_GetPosition(ref PlayableHandle handle, out Vector3 value)
        {
            if (!s_Positions.TryGetValue(handle, out value))
                value = Vector3.zero;
        }

        private static void SetPosition(ref PlayableHandle handle, Vector3 value)
		{
			AnimationOffsetPlayable.INTERNAL_CALL_SetPosition(ref handle, ref value);
		}

        private static void INTERNAL_CALL_SetPosition(ref PlayableHandle handle, ref Vector3 value)
        {
            s_Positions[handle] = value;
        }

        private static Quaternion GetRotation(ref PlayableHandle handle)
		{
			Quaternion result;
			AnimationOffsetPlayable.INTERNAL_CALL_GetRotation(ref handle, out result);
			return result;
		}

        private static void INTERNAL_CALL_GetRotation(ref PlayableHandle handle, out Quaternion value)
        {
            if (!s_Rotations.TryGetValue(handle, out value))
                value = Quaternion.identity;
        }

        private static void SetRotation(ref PlayableHandle handle, Quaternion value)
		{
			AnimationOffsetPlayable.INTERNAL_CALL_SetRotation(ref handle, ref value);
		}

        private static void INTERNAL_CALL_SetRotation(ref PlayableHandle handle, ref Quaternion value)
        {
            s_Rotations[handle] = value;
        }
    }
}
