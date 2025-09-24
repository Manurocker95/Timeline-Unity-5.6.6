using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000023 RID: 35
	internal static class CurveBindingGroupExtensions
	{
		// Token: 0x06000172 RID: 370 RVA: 0x0000E914 File Offset: 0x0000CD14
		public static bool IsEnableGroup(this CurveBindingGroup curves)
		{
			return curves.isFloatCurve && curves.count == 1 && curves.curveBindingPairs[0].binding.propertyName == "m_Enabled";
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000E964 File Offset: 0x0000CD64
		public static bool IsVectorGroup(this CurveBindingGroup curves)
		{
			bool result;
			if (!curves.isFloatCurve)
			{
				result = false;
			}
			else if (curves.count <= 1 || curves.count > 4)
			{
				result = false;
			}
			else
			{
				char c = curves.curveBindingPairs[0].binding.propertyName.Last<char>();
				result = (c == 'x' || c == 'y' || c == 'z' || c == 'w');
			}
			return result;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000E9E8 File Offset: 0x0000CDE8
		public static bool IsColorGroup(this CurveBindingGroup curves)
		{
			bool result;
			if (!curves.isFloatCurve)
			{
				result = false;
			}
			else if (curves.count != 3 && curves.count != 4)
			{
				result = false;
			}
			else
			{
				char c = curves.curveBindingPairs[0].binding.propertyName.Last<char>();
				result = (c == 'r' || c == 'g' || c == 'b' || c == 'a');
			}
			return result;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000EA6C File Offset: 0x0000CE6C
		public static string GetDescription(this CurveBindingGroup group, float t)
		{
			string text = string.Empty;
			if (group.isFloatCurve)
			{
				if (group.count > 1)
				{
					text = text + "(" + group.curveBindingPairs[0].curve.Evaluate(t).ToString("0.##");
					for (int i = 1; i < group.curveBindingPairs.Length; i++)
					{
						text = text + "," + group.curveBindingPairs[i].curve.Evaluate(t).ToString("0.##");
					}
					text += ")";
				}
				else
				{
					text = group.curveBindingPairs[0].curve.Evaluate(t).ToString("0.##");
				}
			}
			else if (group.isObjectCurve)
			{
				Object @object = null;
				if (group.curveBindingPairs[0].objectCurve.Length > 0)
				{
					@object = CurveEditUtility.Evaluate(group.curveBindingPairs[0].objectCurve, t);
				}
				text = ((!(@object == null)) ? @object.name : "None");
			}
			return text;
		}
	}
}
