using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000267 RID: 615
	
	internal sealed class AnimationOffsetPlayable : AnimationPlayable
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x0002C468 File Offset: 0x0002A668
		// (set) Token: 0x06002711 RID: 10001 RVA: 0x0002C488 File Offset: 0x0002A688
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

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x0002C498 File Offset: 0x0002A698
		// (set) Token: 0x06002713 RID: 10003 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
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

		// Token: 0x06002714 RID: 10004 RVA: 0x0002C4C8 File Offset: 0x0002A6C8
		private static Vector3 GetPosition(ref PlayableHandle handle)
		{
			Vector3 result;
			AnimationOffsetPlayable.INTERNAL_CALL_GetPosition(ref handle, out result);
			return result;
		}

		// Token: 0x06002715 RID: 10005
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_GetPosition(ref PlayableHandle handle, out Vector3 value);

		// Token: 0x06002716 RID: 10006 RVA: 0x0002C4E8 File Offset: 0x0002A6E8
		private static void SetPosition(ref PlayableHandle handle, Vector3 value)
		{
			AnimationOffsetPlayable.INTERNAL_CALL_SetPosition(ref handle, ref value);
		}

		// Token: 0x06002717 RID: 10007
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetPosition(ref PlayableHandle handle, ref Vector3 value);

		// Token: 0x06002718 RID: 10008 RVA: 0x0002C4F4 File Offset: 0x0002A6F4
		private static Quaternion GetRotation(ref PlayableHandle handle)
		{
			Quaternion result;
			AnimationOffsetPlayable.INTERNAL_CALL_GetRotation(ref handle, out result);
			return result;
		}

		// Token: 0x06002719 RID: 10009
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_GetRotation(ref PlayableHandle handle, out Quaternion value);

		// Token: 0x0600271A RID: 10010 RVA: 0x0002C514 File Offset: 0x0002A714
		private static void SetRotation(ref PlayableHandle handle, Quaternion value)
		{
			AnimationOffsetPlayable.INTERNAL_CALL_SetRotation(ref handle, ref value);
		}

		// Token: 0x0600271B RID: 10011
		
		[MethodImpl(4096)]
		private static extern void INTERNAL_CALL_SetRotation(ref PlayableHandle handle, ref Quaternion value);
	}
}
