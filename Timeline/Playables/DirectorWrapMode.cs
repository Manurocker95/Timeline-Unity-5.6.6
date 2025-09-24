using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Wrap mode for Playables.</para>
	/// </summary>
	// Token: 0x02000305 RID: 773
	public enum DirectorWrapMode
	{
		/// <summary>
		///   <para>Hold the last frame when the playable time reaches it's duration.</para>
		/// </summary>
		// Token: 0x04000A5F RID: 2655
		Hold,
		/// <summary>
		///   <para>Loop back to zero time and continue playing.</para>
		/// </summary>
		// Token: 0x04000A60 RID: 2656
		Loop,
		/// <summary>
		///   <para>Do not keep playing when the time reaches the duration.</para>
		/// </summary>
		// Token: 0x04000A61 RID: 2657
		None
	}
}
