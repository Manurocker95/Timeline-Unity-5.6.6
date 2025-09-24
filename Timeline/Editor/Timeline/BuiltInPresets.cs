using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000006 RID: 6
	internal static class BuiltInPresets
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00003BA4 File Offset: 0x00001FA4
		internal static CurvePresetLibrary blendInPresets
		{
			get
			{
				if (BuiltInPresets.s_BlendInPresets == null)
				{
					BuiltInPresets.s_BlendInPresets = ScriptableObject.CreateInstance<CurvePresetLibrary>();
					BuiltInPresets.s_BlendInPresets.Add(new AnimationCurve(CurveEditorWindow.GetConstantKeys(1f)), "None");
					BuiltInPresets.s_BlendInPresets.Add(new AnimationCurve(CurveEditorWindow.GetLinearKeys()), "Linear");
					BuiltInPresets.s_BlendInPresets.Add(new AnimationCurve(CurveEditorWindow.GetEaseInKeys()), "EaseIn");
					BuiltInPresets.s_BlendInPresets.Add(new AnimationCurve(CurveEditorWindow.GetEaseOutKeys()), "EaseOut");
					BuiltInPresets.s_BlendInPresets.Add(new AnimationCurve(CurveEditorWindow.GetEaseInOutKeys()), "EaseInOut");
				}
				return BuiltInPresets.s_BlendInPresets;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003C5C File Offset: 0x0000205C
		internal static CurvePresetLibrary blendOutPresets
		{
			get
			{
				if (BuiltInPresets.s_BlendOutPresets == null)
				{
					BuiltInPresets.s_BlendOutPresets = ScriptableObject.CreateInstance<CurvePresetLibrary>();
					BuiltInPresets.s_BlendOutPresets.Add(new AnimationCurve(CurveEditorWindow.GetConstantKeys(1f)), "None");
					BuiltInPresets.s_BlendOutPresets.Add(BuiltInPresets.ReverseCurve(new AnimationCurve(CurveEditorWindow.GetLinearKeys())), "Linear");
					BuiltInPresets.s_BlendOutPresets.Add(BuiltInPresets.ReverseCurve(new AnimationCurve(CurveEditorWindow.GetEaseInKeys())), "EaseIn");
					BuiltInPresets.s_BlendOutPresets.Add(BuiltInPresets.ReverseCurve(new AnimationCurve(CurveEditorWindow.GetEaseOutKeys())), "EaseOut");
					BuiltInPresets.s_BlendOutPresets.Add(BuiltInPresets.ReverseCurve(new AnimationCurve(CurveEditorWindow.GetEaseInOutKeys())), "EaseInOut");
				}
				return BuiltInPresets.s_BlendOutPresets;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003D28 File Offset: 0x00002128
		private static AnimationCurve ReverseCurve(AnimationCurve curve)
		{
			Keyframe[] keys = curve.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				keys[i].value = 1f - keys[i].value;
				Keyframe[] array = keys;
				int num = i;
				array[num].inTangent = array[num].inTangent * -1f;
				Keyframe[] array2 = keys;
				int num2 = i;
				array2[num2].outTangent = array2[num2].outTangent * -1f;
			}
			curve.keys = keys;
			return curve;
		}

		// Token: 0x0400003D RID: 61
		private static CurvePresetLibrary s_BlendInPresets;

		// Token: 0x0400003E RID: 62
		private static CurvePresetLibrary s_BlendOutPresets;
	}
}
