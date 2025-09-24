using System;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Describes the timeline features supported by clips representing this playable.</para>
	/// </summary>
	// Token: 0x02000002 RID: 2
	[Flags]
	public enum ClipCaps
	{
		/// <summary>
		///   <para>No features are supported.</para>
		/// </summary>
		// Token: 0x04000002 RID: 2
		None = 0,
		/// <summary>
		///   <para>The clip representing this playable supports loops.</para>
		/// </summary>
		// Token: 0x04000003 RID: 3
		Looping = 1,
		/// <summary>
		///   <para>The clip representing this playable supports clip extrapolation.</para>
		/// </summary>
		// Token: 0x04000004 RID: 4
		Extrapolation = 2,
		/// <summary>
		///   <para>The clip representing the playable supports starting times greater than zero.</para>
		/// </summary>
		// Token: 0x04000005 RID: 5
		ClipIn = 4,
		/// <summary>
		///   <para>The clip representing this playable supports time scaling.</para>
		/// </summary>
		// Token: 0x04000006 RID: 6
		SpeedMultiplier = 8,
		/// <summary>
		///   <para>The clips representing the playables supports blending between clips.</para>
		/// </summary>
		// Token: 0x04000007 RID: 7
		Blending = 16,
		/// <summary>
		///   <para>All features are supported.</para>
		/// </summary>
		// Token: 0x04000008 RID: 8
		All = 31
	}
}
