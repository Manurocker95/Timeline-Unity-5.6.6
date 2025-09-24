using System;

namespace UnityEditor
{
	// Token: 0x02000059 RID: 89
	internal interface IClipCurveEditorOwner
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000337 RID: 823
		ClipCurveEditor clipCurveEditor { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000338 RID: 824
		// (set) Token: 0x06000339 RID: 825
		bool inlineCurvesSelected { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600033A RID: 826
		bool supportsLooping { get; }
	}
}
