using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000010 RID: 16
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackMediaType : Attribute
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00004571 File Offset: 0x00002771
		public TrackMediaType(TimelineAsset.MediaType mt)
		{
			this.m_MediaType = mt;
		}

		// Token: 0x0400003C RID: 60
		public TimelineAsset.MediaType m_MediaType;
	}
}
