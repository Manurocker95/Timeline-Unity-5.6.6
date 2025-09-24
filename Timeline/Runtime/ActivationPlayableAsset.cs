using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200001F RID: 31
	internal class ActivationPlayableAsset : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005FD0 File Offset: 0x000041D0
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.None;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005FE8 File Offset: 0x000041E8
		public override PlayableHandle CreatePlayable(PlayableGraph graph, GameObject go)
		{
			return graph.CreatePlayable();
		}
	}
}
