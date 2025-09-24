using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000002 RID: 2
	internal struct BreadcrumbElement
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002344 File Offset: 0x00000744
		public override int GetHashCode()
		{
			return this.asset.GetHashCode() ^ this.clip.GetHashCode();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002370 File Offset: 0x00000770
		public override string ToString()
		{
			string result;
			if (this.asset != null)
			{
				result = this.asset.name;
			}
			else
			{
				result = "";
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		public PlayableAsset asset;

		// Token: 0x04000002 RID: 2
		public TimelineClip clip;
	}
}
