using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000037 RID: 55
	[TrackClipType(typeof(IPlayableAsset))]
	[TrackMediaType(TimelineAsset.MediaType.Script)]
	[Serializable]
	internal class PlayableTrack : TrackAsset
	{
		// Token: 0x060001AF RID: 431 RVA: 0x00008B2F File Offset: 0x00006D2F
		internal override void OnCreateClipFromAsset(Object asset, TimelineClip newClip)
		{
			base.OnCreateClipFromAsset(asset, newClip);
			if (newClip != null)
			{
				newClip.displayName = asset.GetType().Name;
			}
		}
	}
}
