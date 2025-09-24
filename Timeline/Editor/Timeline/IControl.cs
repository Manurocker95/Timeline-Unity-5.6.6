using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000055 RID: 85
	internal interface IControl
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060002DE RID: 734
		// (remove) Token: 0x060002DF RID: 735
		event TimelineUIEvent MouseDown;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002E0 RID: 736
		// (remove) Token: 0x060002E1 RID: 737
		event TimelineUIEvent MouseDrag;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002E2 RID: 738
		// (remove) Token: 0x060002E3 RID: 739
		event TimelineUIEvent MouseWheel;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002E4 RID: 740
		// (remove) Token: 0x060002E5 RID: 741
		event TimelineUIEvent MouseMove;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002E6 RID: 742
		// (remove) Token: 0x060002E7 RID: 743
		event TimelineUIEvent MouseUp;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060002E8 RID: 744
		// (remove) Token: 0x060002E9 RID: 745
		event TimelineUIEvent DoubleClick;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060002EA RID: 746
		// (remove) Token: 0x060002EB RID: 747
		event TimelineUIEvent KeyDown;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060002EC RID: 748
		// (remove) Token: 0x060002ED RID: 749
		event TimelineUIEvent KeyUp;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002EE RID: 750
		// (remove) Token: 0x060002EF RID: 751
		event TimelineUIEvent DragPerform;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002F0 RID: 752
		// (remove) Token: 0x060002F1 RID: 753
		event TimelineUIEvent DragExited;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002F2 RID: 754
		// (remove) Token: 0x060002F3 RID: 755
		event TimelineUIEvent DragUpdated;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002F4 RID: 756
		// (remove) Token: 0x060002F5 RID: 757
		event TimelineUIEvent Overlay;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002F6 RID: 758
		// (remove) Token: 0x060002F7 RID: 759
		event TimelineUIEvent ContextClick;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002F8 RID: 760
		// (remove) Token: 0x060002F9 RID: 761
		event TimelineUIEvent ValidateCommand;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060002FA RID: 762
		// (remove) Token: 0x060002FB RID: 763
		event TimelineUIEvent ExecuteCommand;

		// Token: 0x060002FC RID: 764
		bool OnEvent(Event evt, TimelineWindow.TimelineState state, bool isCaptureSession);

		// Token: 0x060002FD RID: 765
		void DrawOverlays(Event evt, TimelineWindow.TimelineState state);

		// Token: 0x060002FE RID: 766
		bool IsMouseOver(Vector2 mousePosition);
	}
}
