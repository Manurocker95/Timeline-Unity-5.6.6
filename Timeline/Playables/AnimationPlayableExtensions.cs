using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Animation specific extensions to the Playable Handle type.</para>
	/// </summary>
	public static class AnimationPlayableExtensions
	{
        
		private static readonly Dictionary<PlayableHandle, AnimationClip> s_AnimatedProperties = new Dictionary<PlayableHandle, AnimationClip>();

        /// <summary>
        ///   <para>Get the animated clip that animates the fields of the playable.</para>
        /// </summary>
        /// <param name="handle">The handle to retrieve the Animation Clip from.</param>
        public static AnimationClip GetAnimatedProperties(this PlayableHandle handle)
		{
			return AnimationPlayableExtensions.GetAnimatedPropertiesInternal(ref handle);
		}

		/// <summary>
		///   <para>Sets an animation clip to animate properties on the playable.</para>
		/// </summary>
		/// <param name="handle">Handle of the playable to set.</param>
		/// <param name="clip">Animation clip containing animated properties.</param>
		public static void SetAnimatedProperties(this PlayableHandle handle, AnimationClip clip)
		{
			AnimationPlayableExtensions.SetAnimatedPropertiesInternal(ref handle, clip);
		}

		internal static AnimationClip GetAnimatedPropertiesInternal(ref PlayableHandle playable)
		{
			return AnimationPlayableExtensions.INTERNAL_CALL_GetAnimatedPropertiesInternal(ref playable);
		}

        private static AnimationClip INTERNAL_CALL_GetAnimatedPropertiesInternal(ref PlayableHandle playable)
        {
            AnimationClip clip;
            if (s_AnimatedProperties.TryGetValue(playable, out clip))
                return clip;
            return null;
        }

        internal static void SetAnimatedPropertiesInternal(ref PlayableHandle playable, AnimationClip animatedProperties)
		{
			AnimationPlayableExtensions.INTERNAL_CALL_SetAnimatedPropertiesInternal(ref playable, animatedProperties);
		}

        private static void INTERNAL_CALL_SetAnimatedPropertiesInternal(ref PlayableHandle playable, AnimationClip animatedProperties)
        {
            s_AnimatedProperties[playable] = animatedProperties;
        }
    }
}
