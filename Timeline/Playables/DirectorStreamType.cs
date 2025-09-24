using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Describes the type of information that flows in and out of a Playable. This also specifies that this Playable is connectable to others of the same type.</para>
	/// </summary>
	// Token: 0x020000EB RID: 235
	public enum DirectorStreamType
	{
		/// <summary>
		///   <para>Describes that the information flowing in and out of the Playable is of type Animation.</para>
		/// </summary>
		// Token: 0x04000231 RID: 561
		Animation,
		/// <summary>
		///   <para>Describes that the information flowing in and out of the Playable is Audio.</para>
		/// </summary>
		// Token: 0x04000232 RID: 562
		Audio,
		// Token: 0x04000233 RID: 563
		Video,
		/// <summary>
		///   <para>Describes that the Playable does not have any particular type. This is use for Playables that execute script code, or that create their own playable graphs, such as the Sequence.</para>
		/// </summary>
		// Token: 0x04000234 RID: 564
		None
	}
}
