using System;
using System.Runtime.InteropServices;

namespace UnityEditor.Timeline
{
	// Token: 0x020000CC RID: 204
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct TrackBindingValidationResult
	{
		// Token: 0x060006A7 RID: 1703 RVA: 0x0002DB84 File Offset: 0x0002BF84
		public TrackBindingValidationResult(TimelineTrackBindingState state, string bindName = null)
		{
			this.bindingState = state;
			this.bindingName = bindName;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0002DB98 File Offset: 0x0002BF98
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0002DBB2 File Offset: 0x0002BFB2
		public TimelineTrackBindingState bindingState { get; private set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0002DBBC File Offset: 0x0002BFBC
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x0002DBD6 File Offset: 0x0002BFD6
		public string bindingName { get; private set; }

		// Token: 0x060006AC RID: 1708 RVA: 0x0002DBE0 File Offset: 0x0002BFE0
		public static implicit operator bool(TrackBindingValidationResult result)
		{
			return result.IsValid();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002DBFC File Offset: 0x0002BFFC
		public bool IsValid()
		{
			return this.bindingState == TimelineTrackBindingState.Valid;
		}
	}
}
