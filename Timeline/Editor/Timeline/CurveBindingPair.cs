using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x0200001E RID: 30
	internal struct CurveBindingPair
	{
		// Token: 0x04000144 RID: 324
		public EditorCurveBinding binding;

		// Token: 0x04000145 RID: 325
		public AnimationCurve curve;

		// Token: 0x04000146 RID: 326
		public ObjectReferenceKeyframe[] objectCurve;
	}
}
