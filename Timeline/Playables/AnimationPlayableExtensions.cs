using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Animation specific extensions to the Playable Handle type.</para>
	/// </summary>
	// Token: 0x02000264 RID: 612
	public static class AnimationPlayableExtensions
	{
		/// <summary>
		///   <para>Get the animated clip that animates the fields of the playable.</para>
		/// </summary>
		/// <param name="handle">The handle to retrieve the Animation Clip from.</param>
		// Token: 0x060026DA RID: 9946 RVA: 0x0002BF48 File Offset: 0x0002A148
		public static AnimationClip GetAnimatedProperties(this PlayableHandle handle)
		{
			return AnimationPlayableExtensions.GetAnimatedPropertiesInternal(ref handle);
		}

		/// <summary>
		///   <para>Sets an animation clip to animate properties on the playable.</para>
		/// </summary>
		/// <param name="handle">Handle of the playable to set.</param>
		/// <param name="clip">Animation clip containing animated properties.</param>
		// Token: 0x060026DB RID: 9947 RVA: 0x0002BF64 File Offset: 0x0002A164
		public static void SetAnimatedProperties(this PlayableHandle handle, AnimationClip clip)
		{
			AnimationPlayableExtensions.SetAnimatedPropertiesInternal(ref handle, clip);
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0002BF70 File Offset: 0x0002A170
		internal static AnimationClip GetAnimatedPropertiesInternal(ref PlayableHandle playable)
		{
			return AnimationPlayableExtensions.INTERNAL_CALL_GetAnimatedPropertiesInternal(ref playable);
		}

		// Token: 0x060026DD RID: 9949
		
		[MethodImpl(4096)]
		private static extern AnimationClip INTERNAL_CALL_GetAnimatedPropertiesInternal(ref PlayableHandle playable);

		// Token: 0x060026DE RID: 9950 RVA: 0x0002BF8C File Offset: 0x0002A18C
		internal static void SetAnimatedPropertiesInternal(ref PlayableHandle playable, AnimationClip animatedProperties)
		{
			AnimationPlayableExtensions.INTERNAL_CALL_SetAnimatedPropertiesInternal(ref playable, animatedProperties);
		}

		// Token: 0x060026DF RID: 9951
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetAnimatedPropertiesInternal(ref PlayableHandle playable, AnimationClip animatedProperties);
	}
}
