using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000004 RID: 4
	[TrackClipType(typeof(TrackAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Group)]
	[Serializable]
	internal class GroupTrack : TrackAsset
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000031AC File Offset: 0x000013AC
		internal override bool compilable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000031C4 File Offset: 0x000013C4
		public override PlayableBinding[] outputs
		{
			get
			{
				return PlayableBinding.None;
			}
		}
	}
}
