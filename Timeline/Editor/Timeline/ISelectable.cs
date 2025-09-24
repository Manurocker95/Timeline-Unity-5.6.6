using System;

namespace UnityEditor.Timeline
{
	// Token: 0x0200000F RID: 15
	internal interface ISelectable
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D1 RID: 209
		// (set) Token: 0x060000D2 RID: 210
		bool selected { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D3 RID: 211
		object selectableObject { get; }

		// Token: 0x060000D4 RID: 212
		void Delete(ITimelineState state);

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D5 RID: 213
		bool selectable { get; }
	}
}
