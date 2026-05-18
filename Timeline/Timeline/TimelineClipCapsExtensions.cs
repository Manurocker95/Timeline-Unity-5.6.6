using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000003 RID: 3
	internal static class TimelineClipCapsExtensions
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool SupportsLooping(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Looping) != ClipCaps.None;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
		public static bool SupportsExtrapolation(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Extrapolation) != ClipCaps.None;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A8 File Offset: 0x000002A8
		public static bool SupportsClipIn(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.ClipIn) != ClipCaps.None;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D4 File Offset: 0x000002D4
		public static bool SupportsSpeedMultiplier(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.SpeedMultiplier) != ClipCaps.None;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002100 File Offset: 0x00000300
		public static bool SupportsBlending(this TimelineClip clip)
		{
			return clip != null && (clip.clipCaps & ClipCaps.Blending) != ClipCaps.None;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002130 File Offset: 0x00000330
		public static bool HasAll(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) == flags;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000214C File Offset: 0x0000034C
		public static bool HasAny(this ClipCaps caps, ClipCaps flags)
		{
			return (caps & flags) != ClipCaps.None;
		}
	}
}
