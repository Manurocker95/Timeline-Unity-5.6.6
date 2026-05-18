using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000013 RID: 19
	public interface ITimelineClipAsset
	{
		/// <summary>
		///   <para>Returns a description of the features supported by clips representing playables implementing this interface.</para>
		/// </summary>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000060 RID: 96
		ClipCaps clipCaps { get; }
	}
}
