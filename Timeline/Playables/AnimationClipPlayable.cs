using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Playable that plays an AnimationClip. Can be used as an input to an AnimationPlayable.</para>
	/// </summary>
	// Token: 0x02000262 RID: 610
	
	public sealed class AnimationClipPlayable : AnimationPlayable
	{
		/// <summary>
		///   <para>AnimationClip played by this Playable.</para>
		/// </summary>
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x0002BDFC File Offset: 0x00029FFC
		public AnimationClip clip
		{
			get
			{
				return AnimationClipPlayable.GetAnimationClip(ref this.handle);
			}
		}

		/// <summary>
		///   <para>The speed at which the AnimationClip is played.</para>
		/// </summary>
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x0002BE1C File Offset: 0x0002A01C
		// (set) Token: 0x060026C6 RID: 9926 RVA: 0x0002BE3C File Offset: 0x0002A03C
		public float speed
		{
			get
			{
				return AnimationClipPlayable.GetSpeed(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetSpeed(ref this.handle, value);
			}
		}

		/// <summary>
		///   <para>Applies Humanoid FootIK solver.</para>
		/// </summary>
		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x0002BE4C File Offset: 0x0002A04C
		// (set) Token: 0x060026C8 RID: 9928 RVA: 0x0002BE6C File Offset: 0x0002A06C
		public bool applyFootIK
		{
			get
			{
				return AnimationClipPlayable.GetApplyFootIK(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetApplyFootIK(ref this.handle, value);
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x0002BE7C File Offset: 0x0002A07C
		// (set) Token: 0x060026CA RID: 9930 RVA: 0x0002BE9C File Offset: 0x0002A09C
		internal bool removeStartOffset
		{
			get
			{
				return AnimationClipPlayable.GetRemoveStartOffset(ref this.handle);
			}
			set
			{
				AnimationClipPlayable.SetRemoveStartOffset(ref this.handle, value);
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0002BEAC File Offset: 0x0002A0AC
		private static AnimationClip GetAnimationClip(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetAnimationClip(ref handle);
		}

		// Token: 0x060026CC RID: 9932
		
		[MethodImpl(4096)]
		private static extern AnimationClip INTERNAL_CALL_GetAnimationClip(ref PlayableHandle handle);

		// Token: 0x060026CD RID: 9933 RVA: 0x0002BEC8 File Offset: 0x0002A0C8
		private static float GetSpeed(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetSpeed(ref handle);
		}

		// Token: 0x060026CE RID: 9934
		
		[MethodImpl(4096)]
		private static extern float INTERNAL_CALL_GetSpeed(ref PlayableHandle handle);

		// Token: 0x060026CF RID: 9935 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
		private static void SetSpeed(ref PlayableHandle handle, float value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetSpeed(ref handle, value);
		}

		// Token: 0x060026D0 RID: 9936
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetSpeed(ref PlayableHandle handle, float value);

		// Token: 0x060026D1 RID: 9937 RVA: 0x0002BEF0 File Offset: 0x0002A0F0
		private static bool GetApplyFootIK(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetApplyFootIK(ref handle);
		}

		// Token: 0x060026D2 RID: 9938
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_GetApplyFootIK(ref PlayableHandle handle);

		// Token: 0x060026D3 RID: 9939 RVA: 0x0002BF0C File Offset: 0x0002A10C
		private static void SetApplyFootIK(ref PlayableHandle handle, bool value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetApplyFootIK(ref handle, value);
		}

		// Token: 0x060026D4 RID: 9940
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetApplyFootIK(ref PlayableHandle handle, bool value);

		// Token: 0x060026D5 RID: 9941 RVA: 0x0002BF18 File Offset: 0x0002A118
		private static bool GetRemoveStartOffset(ref PlayableHandle handle)
		{
			return AnimationClipPlayable.INTERNAL_CALL_GetRemoveStartOffset(ref handle);
		}

		// Token: 0x060026D6 RID: 9942
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_GetRemoveStartOffset(ref PlayableHandle handle);

		// Token: 0x060026D7 RID: 9943 RVA: 0x0002BF34 File Offset: 0x0002A134
		private static void SetRemoveStartOffset(ref PlayableHandle handle, bool value)
		{
			AnimationClipPlayable.INTERNAL_CALL_SetRemoveStartOffset(ref handle, value);
		}

		// Token: 0x060026D8 RID: 9944
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetRemoveStartOffset(ref PlayableHandle handle, bool value);
	}
}
