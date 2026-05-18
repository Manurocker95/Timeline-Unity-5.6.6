using System;
using System.Runtime.CompilerServices;
using UnityEngine.Audio;
using UnityEngine.Scripting;

namespace UnityEngine.Playables.Audio
{
	/// <summary>
	///   <para>A playable which plays an AudioClip.</para>
	/// </summary>
	// Token: 0x02000218 RID: 536
	
	public sealed class AudioClipPlayable : AudioPlayable
	{
		/// <summary>
		///   <para>The AudioClip assigned to this AudioClipPlayable.</para>
		/// </summary>
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x00028740 File Offset: 0x00026940
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x00028760 File Offset: 0x00026960
		public AudioClip clip
		{
			get
			{
				return AudioClipPlayable.GetClip(ref this.handle);
			}
			set
			{
				AudioClipPlayable.SetClip(ref this.handle, value);
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x00028770 File Offset: 0x00026970
		private static AudioClip GetClip(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetClip(ref hdl);
		}

		// Token: 0x06002323 RID: 8995
		
		[MethodImpl(4096)]
		private static extern AudioClip INTERNAL_CALL_GetClip(ref PlayableHandle hdl);

		// Token: 0x06002324 RID: 8996 RVA: 0x0002878C File Offset: 0x0002698C
		private static void SetClip(ref PlayableHandle hdl, AudioClip clip)
		{
			AudioClipPlayable.INTERNAL_CALL_SetClip(ref hdl, clip);
		}

		// Token: 0x06002325 RID: 8997
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetClip(ref PlayableHandle hdl, AudioClip clip);

		/// <summary>
		///   <para>Is the AudioClip set to loop.</para>
		/// </summary>
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x00028798 File Offset: 0x00026998
		// (set) Token: 0x06002327 RID: 8999 RVA: 0x000287B8 File Offset: 0x000269B8
		public bool looped
		{
			get
			{
				return AudioClipPlayable.GetLooped(ref this.handle);
			}
			set
			{
				AudioClipPlayable.SetLooped(ref this.handle, value);
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000287C8 File Offset: 0x000269C8
		private static bool GetLooped(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetLooped(ref hdl);
		}

		// Token: 0x06002329 RID: 9001
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_GetLooped(ref PlayableHandle hdl);

		// Token: 0x0600232A RID: 9002 RVA: 0x000287E4 File Offset: 0x000269E4
		private static void SetLooped(ref PlayableHandle hdl, bool looped)
		{
			AudioClipPlayable.INTERNAL_CALL_SetLooped(ref hdl, looped);
		}

		// Token: 0x0600232B RID: 9003
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetLooped(ref PlayableHandle hdl, bool looped);

		/// <summary>
		///   <para>Is the AudioClip currently playing.</para>
		/// </summary>
		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x000287F0 File Offset: 0x000269F0
		public bool isPlaying
		{
			get
			{
				return AudioClipPlayable.GetIsPlaying(ref this.handle);
			}
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00028810 File Offset: 0x00026A10
		private static bool GetIsPlaying(ref PlayableHandle hdl)
		{
			return AudioClipPlayable.INTERNAL_CALL_GetIsPlaying(ref hdl);
		}

		// Token: 0x0600232E RID: 9006
		
		[MethodImpl(4096)]
		private static extern bool INTERNAL_CALL_GetIsPlaying(ref PlayableHandle hdl);
	}
}
