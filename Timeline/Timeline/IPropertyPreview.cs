using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003A RID: 58
	public interface IPropertyPreview
	{
		/// <summary>
		///   <para>Called by the Timeline Editor to gather properties requiring preview.</para>
		/// </summary>
		/// <param name="director"></param>
		/// <param name="driver"></param>
		// Token: 0x060001BD RID: 445
		void GatherProperties(PlayableDirector director, IPropertyCollector driver);
	}
}
