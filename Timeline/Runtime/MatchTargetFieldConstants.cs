using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000026 RID: 38
	internal static class MatchTargetFieldConstants
	{
		// Token: 0x0600013D RID: 317 RVA: 0x000067FC File Offset: 0x000049FC
		public static bool HasAny(this MatchTargetFields me, MatchTargetFields fields)
		{
			return (me & fields) != MatchTargetFieldConstants.None;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006820 File Offset: 0x00004A20
		public static MatchTargetFields Toggle(this MatchTargetFields me, MatchTargetFields flag)
		{
			return me ^ flag;
		}

		// Token: 0x040000B3 RID: 179
		public static MatchTargetFields All = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ | MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;

		// Token: 0x040000B4 RID: 180
		public static MatchTargetFields None = (MatchTargetFields)0;

		// Token: 0x040000B5 RID: 181
		public static MatchTargetFields Position = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ;

		// Token: 0x040000B6 RID: 182
		public static MatchTargetFields Rotation = MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;
	}
}
