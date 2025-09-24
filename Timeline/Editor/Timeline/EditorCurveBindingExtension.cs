using System;
using UnityEditorInternal;

namespace UnityEditor.Timeline
{
	// Token: 0x02000022 RID: 34
	internal static class EditorCurveBindingExtension
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0000E8E0 File Offset: 0x0000CCE0
		public static string GetGroupID(this EditorCurveBinding binding)
		{
			return binding.type.ToString() + AnimationWindowUtility.GetPropertyGroupName(binding.propertyName);
		}
	}
}
