using System;

namespace UnityEditor.Timeline
{
	// Token: 0x020000CB RID: 203
	internal enum TimelineTrackBindingState
	{
		// Token: 0x040003B9 RID: 953
		Valid,
		// Token: 0x040003BA RID: 954
		NoGameObjectBound,
		// Token: 0x040003BB RID: 955
		BoundGameObjectIsDisabled,
		// Token: 0x040003BC RID: 956
		NoValidComponentOnBoundGameObject,
		// Token: 0x040003BD RID: 957
		RequiredComponentOnBoundGameObjectIsDisabled
	}
}
