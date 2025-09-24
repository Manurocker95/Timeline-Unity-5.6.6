using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000008 RID: 8
	internal static class CurveEditUtility
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00004868 File Offset: 0x00002C68
		private static bool IsRotationKey(EditorCurveBinding binding)
		{
			return binding.propertyName.Contains("localEulerAnglesRaw");
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004890 File Offset: 0x00002C90
		public static void AddKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
		{
			if (sourceBinding.isPPtrCurve)
			{
				CurveEditUtility.AddObjectKey(clip, sourceBinding, prop, time);
			}
			else if (CurveEditUtility.IsRotationKey(sourceBinding))
			{
				CurveEditUtility.AddRotationKey(clip, sourceBinding, prop, time);
			}
			else
			{
				CurveEditUtility.AddFloatKey(clip, sourceBinding, prop, time);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000048E0 File Offset: 0x00002CE0
		private static void AddObjectKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
		{
			if (prop.propertyType == 5)
			{
				ObjectReferenceKeyframe[] array = null;
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				int num = Array.IndexOf<EditorCurveBinding>(curveInfo.objectBindings, sourceBinding);
				if (num >= 0)
				{
					array = curveInfo.objectCurves[num];
					int num2 = CurveEditUtility.EvaluateIndex(array, (float)time);
					if (CurveEditUtility.KeyCompare(array[num2].time, (float)time, clip.frameRate) == 0)
					{
						array[num2].value = prop.objectReferenceValue;
					}
					else if (num2 < array.Length - 1 && CurveEditUtility.KeyCompare(array[num2 + 1].time, (float)time, clip.frameRate) == 0)
					{
						array[num2 + 1].value = prop.objectReferenceValue;
					}
					else
					{
						if (time > (double)array[0].time)
						{
							num2++;
						}
						ObjectReferenceKeyframe objectReferenceKeyframe = default(ObjectReferenceKeyframe);
						objectReferenceKeyframe.time = (float)time;
						objectReferenceKeyframe.value = prop.objectReferenceValue;
						ArrayUtility.Insert<ObjectReferenceKeyframe>(ref array, num2, objectReferenceKeyframe);
					}
				}
				else
				{
					array = new ObjectReferenceKeyframe[1];
					array[0].time = (float)time;
					array[0].value = prop.objectReferenceValue;
				}
				AnimationUtility.SetObjectReferenceCurve(clip, sourceBinding, array);
				EditorUtility.SetDirty(clip);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004A34 File Offset: 0x00002E34
		private static void AddRotationKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
		{
			if (prop.propertyType == 17)
			{
				List<AnimationCurve> list = new List<AnimationCurve>();
				List<EditorCurveBinding> list2 = new List<EditorCurveBinding>();
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				for (int i = 0; i < curveInfo.bindings.Length; i++)
				{
					if (sourceBind.type == curveInfo.bindings[i].type)
					{
						if (curveInfo.bindings[i].propertyName.Contains("localEuler"))
						{
							list2.Add(curveInfo.bindings[i]);
							list.Add(curveInfo.curves[i]);
						}
					}
				}
				Vector3 localEulerAngles = ((Transform)prop.serializedObject.targetObject).localEulerAngles;
				if (list2.Count == 0)
				{
					string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(sourceBind.propertyName);
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".x"));
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".y"));
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".z"));
					AnimationCurve animationCurve = new AnimationCurve();
					AnimationCurve animationCurve2 = new AnimationCurve();
					AnimationCurve animationCurve3 = new AnimationCurve();
					CurveEditUtility.AddKeyFrameToCurve(animationCurve, (float)time, clip.frameRate, localEulerAngles.x, false);
					CurveEditUtility.AddKeyFrameToCurve(animationCurve2, (float)time, clip.frameRate, localEulerAngles.y, false);
					CurveEditUtility.AddKeyFrameToCurve(animationCurve3, (float)time, clip.frameRate, localEulerAngles.z, false);
					list.Add(animationCurve);
					list.Add(animationCurve2);
					list.Add(animationCurve3);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					char c = list2[j].propertyName.Last<char>();
					float value = localEulerAngles.x;
					if (c == 'y')
					{
						value = localEulerAngles.y;
					}
					else if (c == 'z')
					{
						value = localEulerAngles.z;
					}
					CurveEditUtility.AddKeyFrameToCurve(list[j], (float)time, clip.frameRate, value, false);
				}
				CurveEditUtility.UpdateEditorCurves(clip, list2, list);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004C8C File Offset: 0x0000308C
		private static void AddFloatKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
		{
			List<AnimationCurve> list = new List<AnimationCurve>();
			List<EditorCurveBinding> list2 = new List<EditorCurveBinding>();
			bool flag = false;
			AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
			for (int i = 0; i < curveInfo.bindings.Length; i++)
			{
				EditorCurveBinding item = curveInfo.bindings[i];
				if (item.type == sourceBind.type)
				{
					SerializedProperty serializedProperty = null;
					AnimationCurve animationCurve = curveInfo.curves[i];
					if (prop.propertyPath.Equals(item.propertyName))
					{
						serializedProperty = prop;
					}
					else if (item.propertyName.Contains(prop.propertyPath))
					{
						serializedProperty = prop.serializedObject.FindProperty(item.propertyName);
					}
					if (serializedProperty != null)
					{
						float keyValue = CurveEditUtility.GetKeyValue(serializedProperty);
						if (!float.IsNaN(keyValue))
						{
							flag = true;
							CurveEditUtility.AddKeyFrameToCurve(animationCurve, (float)time, clip.frameRate, keyValue, serializedProperty.propertyType == 1);
							list.Add(animationCurve);
							list2.Add(item);
						}
					}
				}
			}
			if (!flag)
			{
				string propertyGroupName = AnimationWindowUtility.GetPropertyGroupName(sourceBind.propertyName);
				if (!prop.hasChildren)
				{
					float keyValue2 = CurveEditUtility.GetKeyValue(prop);
					if (!float.IsNaN(keyValue2))
					{
						list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName));
						AnimationCurve animationCurve2 = new AnimationCurve();
						CurveEditUtility.AddKeyFrameToCurve(animationCurve2, (float)time, clip.frameRate, keyValue2, prop.propertyType == 1);
						list.Add(animationCurve2);
					}
				}
				else if (prop.propertyType == 4)
				{
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".r"));
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".g"));
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".b"));
					list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, propertyGroupName + ".a"));
					Color colorValue = prop.colorValue;
					for (int j = 0; j < 4; j++)
					{
						AnimationCurve animationCurve3 = new AnimationCurve();
						CurveEditUtility.AddKeyFrameToCurve(animationCurve3, (float)time, clip.frameRate, colorValue[j], prop.propertyType == 1);
						list.Add(animationCurve3);
					}
				}
				else
				{
					prop = prop.Copy();
					IEnumerator enumerator = prop.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							SerializedProperty serializedProperty2 = (SerializedProperty)obj;
							list2.Add(EditorCurveBinding.FloatCurve(sourceBind.path, sourceBind.type, serializedProperty2.propertyPath));
							AnimationCurve animationCurve4 = new AnimationCurve();
							CurveEditUtility.AddKeyFrameToCurve(animationCurve4, (float)time, clip.frameRate, CurveEditUtility.GetKeyValue(serializedProperty2), serializedProperty2.propertyType == 1);
							list.Add(animationCurve4);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			CurveEditUtility.UpdateEditorCurves(clip, list2, list);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004FDC File Offset: 0x000033DC
		public static void RemoveKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
		{
			if (sourceBinding.isPPtrCurve)
			{
				CurveEditUtility.RemoveObjectKey(clip, sourceBinding, prop, time);
			}
			else if (CurveEditUtility.IsRotationKey(sourceBinding))
			{
				CurveEditUtility.RemoveRotationKey(clip, sourceBinding, prop, time);
			}
			else
			{
				CurveEditUtility.RemoveFloatKey(clip, sourceBinding, prop, time);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000502C File Offset: 0x0000342C
		private static void RemoveObjectKey(AnimationClip clip, EditorCurveBinding sourceBinding, SerializedProperty prop, double time)
		{
			if (prop.propertyType == 5)
			{
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				int num = Array.IndexOf<EditorCurveBinding>(curveInfo.objectBindings, sourceBinding);
				if (num >= 0)
				{
					ObjectReferenceKeyframe[] array = curveInfo.objectCurves[num];
					int keyframeAtTime = CurveEditUtility.GetKeyframeAtTime(array, (float)time, clip.frameRate);
					if (keyframeAtTime >= 0)
					{
						ArrayUtility.RemoveAt<ObjectReferenceKeyframe>(ref array, keyframeAtTime);
						AnimationUtility.SetObjectReferenceCurve(clip, sourceBinding, array);
						EditorUtility.SetDirty(clip);
					}
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000050A8 File Offset: 0x000034A8
		private static void RemoveRotationKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
		{
			if (prop.propertyType == 17)
			{
				List<AnimationCurve> list = new List<AnimationCurve>();
				List<EditorCurveBinding> list2 = new List<EditorCurveBinding>();
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				for (int i = 0; i < curveInfo.bindings.Length; i++)
				{
					if (sourceBind.type == curveInfo.bindings[i].type)
					{
						if (curveInfo.bindings[i].propertyName.Contains("localEuler"))
						{
							list2.Add(curveInfo.bindings[i]);
							list.Add(curveInfo.curves[i]);
						}
					}
				}
				foreach (AnimationCurve curve in list)
				{
					CurveEditUtility.RemoveKeyFrameFromCurve(curve, (float)time, clip.frameRate);
				}
				CurveEditUtility.UpdateEditorCurves(clip, list2, list);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000051C8 File Offset: 0x000035C8
		private static void RemoveFloatKey(AnimationClip clip, EditorCurveBinding sourceBind, SerializedProperty prop, double time)
		{
			List<AnimationCurve> list = new List<AnimationCurve>();
			List<EditorCurveBinding> list2 = new List<EditorCurveBinding>();
			AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
			for (int i = 0; i < curveInfo.bindings.Length; i++)
			{
				EditorCurveBinding item = curveInfo.bindings[i];
				if (item.type == sourceBind.type)
				{
					SerializedProperty serializedProperty = null;
					AnimationCurve animationCurve = curveInfo.curves[i];
					if (prop.propertyPath.Equals(item.propertyName))
					{
						serializedProperty = prop;
					}
					else if (item.propertyName.Contains(prop.propertyPath))
					{
						serializedProperty = prop.serializedObject.FindProperty(item.propertyName);
					}
					if (serializedProperty != null)
					{
						CurveEditUtility.RemoveKeyFrameFromCurve(animationCurve, (float)time, clip.frameRate);
						list.Add(animationCurve);
						list2.Add(item);
					}
				}
			}
			CurveEditUtility.UpdateEditorCurves(clip, list2, list);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000052C0 File Offset: 0x000036C0
		private static void UpdateEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
		{
			if (curve.keys.Length == 0)
			{
				AnimationUtility.SetEditorCurve(clip, binding, null);
			}
			else
			{
				AnimationUtility.SetEditorCurve(clip, binding, curve);
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000052E8 File Offset: 0x000036E8
		private static void UpdateEditorCurves(AnimationClip clip, List<EditorCurveBinding> bindings, List<AnimationCurve> curves)
		{
			if (curves.Count != 0)
			{
				for (int i = 0; i < curves.Count; i++)
				{
					CurveEditUtility.UpdateEditorCurve(clip, bindings[i], curves[i]);
				}
				EditorUtility.SetDirty(clip);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000533C File Offset: 0x0000373C
		public static void RemoveCurves(AnimationClip clip, SerializedProperty prop)
		{
			if (!(clip == null) && prop != null)
			{
				List<EditorCurveBinding> list = new List<EditorCurveBinding>();
				AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip);
				for (int i = 0; i < curveInfo.bindings.Length; i++)
				{
					EditorCurveBinding item = curveInfo.bindings[i];
					if (prop.propertyPath.Equals(item.propertyName) || item.propertyName.Contains(prop.propertyPath))
					{
						list.Add(item);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					AnimationUtility.SetEditorCurve(clip, list[j], null);
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00005404 File Offset: 0x00003804
		public static void AddKeyFrameToCurve(AnimationCurve curve, float time, float framerate, float value, bool stepped)
		{
			Keyframe keyframe = default(Keyframe);
			bool flag = true;
			int num = CurveEditUtility.GetKeyframeAtTime(curve, time, framerate);
			if (num != -1)
			{
				flag = false;
				keyframe = curve[num];
				curve.RemoveKey(num);
			}
			keyframe.value = value;
			keyframe.time = CurveEditUtility.GetKeyTime(time, framerate);
			num = curve.AddKey(keyframe);
			if (stepped)
			{
				AnimationUtility.SetKeyBroken(curve, num, stepped);
				AnimationUtility.SetKeyLeftTangentMode(curve, num, 3);
				AnimationUtility.SetKeyRightTangentMode(curve, num, 3);
				keyframe.outTangent = float.PositiveInfinity;
				keyframe.inTangent = float.PositiveInfinity;
			}
			else if (flag)
			{
				AnimationUtility.SetKeyLeftTangentMode(curve, num, 4);
				AnimationUtility.SetKeyRightTangentMode(curve, num, 4);
			}
			if (num != -1 && !stepped)
			{
				AnimationUtility.UpdateTangentsFromModeSurrounding(curve, num);
				AnimationUtility.SetKeyBroken(curve, num, false);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000054D4 File Offset: 0x000038D4
		public static bool RemoveKeyFrameFromCurve(AnimationCurve curve, float time, float framerate)
		{
			int keyframeAtTime = CurveEditUtility.GetKeyframeAtTime(curve, time, framerate);
			bool result;
			if (keyframeAtTime == -1)
			{
				result = false;
			}
			else
			{
				curve.RemoveKey(keyframeAtTime);
				result = true;
			}
			return result;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000550C File Offset: 0x0000390C
		private static float GetKeyValue(SerializedProperty prop)
		{
			float result;
			switch (prop.propertyType)
			{
			case 0:
				result = (float)prop.intValue;
				break;
			case 1:
				result = ((!prop.boolValue) ? 0f : 1f);
				break;
			case 2:
				result = prop.floatValue;
				break;
			default:
				Debug.LogError("Could not convert property type " + prop.propertyType.ToString() + " to float");
				result = float.NaN;
				break;
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000055AC File Offset: 0x000039AC
		public static int GetKeyframeAtTime(AnimationCurve curve, float time, float frameRate)
		{
			float num = 0.5f / frameRate;
			Keyframe[] keys = curve.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				Keyframe keyframe = keys[i];
				if (keyframe.time >= time - num && keyframe.time < time + num)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00005620 File Offset: 0x00003A20
		public static int GetKeyframeAtTime(ObjectReferenceKeyframe[] curve, float time, float frameRate)
		{
			int result;
			if (curve == null || curve.Length == 0)
			{
				result = -1;
			}
			else
			{
				float num = 0.5f / frameRate;
				for (int i = 0; i < curve.Length; i++)
				{
					float time2 = curve[i].time;
					if (time2 >= time - num && time2 < time + num)
					{
						return i;
					}
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005690 File Offset: 0x00003A90
		public static float GetKeyTime(float time, float frameRate)
		{
			return Mathf.Round(time * frameRate) / frameRate;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000056B0 File Offset: 0x00003AB0
		public static int KeyCompare(float timeA, float timeB, float frameRate)
		{
			int result;
			if (Mathf.Abs(timeA - timeB) <= 1f / frameRate)
			{
				result = 0;
			}
			else
			{
				result = ((timeA >= timeB) ? 1 : -1);
			}
			return result;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000056F0 File Offset: 0x00003AF0
		public static Object Evaluate(ObjectReferenceKeyframe[] curve, float time)
		{
			return curve[CurveEditUtility.EvaluateIndex(curve, time)].value;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005718 File Offset: 0x00003B18
		public static int EvaluateIndex(ObjectReferenceKeyframe[] curve, float time)
		{
			if (curve == null || curve.Length == 0)
			{
				throw new InvalidOperationException("Can not evaluate a PPtr curve with no entries");
			}
			int result;
			if (time <= curve[0].time)
			{
				result = 0;
			}
			else if (time >= curve.Last<ObjectReferenceKeyframe>().time)
			{
				result = curve.Length - 1;
			}
			else
			{
				int num = curve.Length - 1;
				int num2 = 0;
				while (num - num2 > 1)
				{
					int num3 = (num2 + num) / 2;
					if (Mathf.Approximately(curve[num3].time, time))
					{
						return num3;
					}
					if (curve[num3].time < time)
					{
						num2 = num3;
					}
					else if (curve[num3].time > time)
					{
						num = num3;
					}
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000057F0 File Offset: 0x00003BF0
		public static void ShiftBySeconds(this AnimationClip clip, float time)
		{
			EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
			EditorCurveBinding[] objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
			foreach (EditorCurveBinding editorCurveBinding in curveBindings)
			{
				AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, editorCurveBinding);
				Keyframe[] keys = editorCurve.keys;
				for (int j = 0; j < keys.Length; j++)
				{
					Keyframe[] array2 = keys;
					int num = j;
					array2[num].time = array2[num].time + time;
				}
				editorCurve.keys = keys;
				AnimationUtility.SetEditorCurve(clip, editorCurveBinding, editorCurve);
			}
			foreach (EditorCurveBinding editorCurveBinding2 in objectReferenceCurveBindings)
			{
				ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(clip, editorCurveBinding2);
				for (int l = 0; l < objectReferenceCurve.Length; l++)
				{
					ObjectReferenceKeyframe[] array4 = objectReferenceCurve;
					int num2 = l;
					array4[num2].time = array4[num2].time + time;
				}
				AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding2, objectReferenceCurve);
			}
			EditorUtility.SetDirty(clip);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005904 File Offset: 0x00003D04
		public static void ScaleTime(this AnimationClip clip, float scale)
		{
			EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
			EditorCurveBinding[] objectReferenceCurveBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
			foreach (EditorCurveBinding editorCurveBinding in curveBindings)
			{
				AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, editorCurveBinding);
				Keyframe[] keys = editorCurve.keys;
				for (int j = 0; j < keys.Length; j++)
				{
					Keyframe[] array2 = keys;
					int num = j;
					array2[num].time = array2[num].time * scale;
				}
				editorCurve.keys = (from x in keys
				orderby x.time
				select x).ToArray<Keyframe>();
				AnimationUtility.SetEditorCurve(clip, editorCurveBinding, editorCurve);
			}
			foreach (EditorCurveBinding editorCurveBinding2 in objectReferenceCurveBindings)
			{
				ObjectReferenceKeyframe[] array4 = AnimationUtility.GetObjectReferenceCurve(clip, editorCurveBinding2);
				for (int l = 0; l < array4.Length; l++)
				{
					ObjectReferenceKeyframe[] array5 = array4;
					int num2 = l;
					array5[num2].time = array5[num2].time * scale;
				}
				array4 = (from x in array4
				orderby x.time
				select x).ToArray<ObjectReferenceKeyframe>();
				AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding2, array4);
			}
			EditorUtility.SetDirty(clip);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005A68 File Offset: 0x00003E68
		public static AnimationCurve CreateMatchingCurve(AnimationCurve curve)
		{
			Keyframe[] keys = curve.keys;
			for (int num = 0; num != keys.Length; num++)
			{
				if (!float.IsPositiveInfinity(keys[num].inTangent))
				{
					keys[num].inTangent = -keys[num].inTangent;
				}
				if (!float.IsPositiveInfinity(keys[num].outTangent))
				{
					keys[num].outTangent = -keys[num].outTangent;
				}
				keys[num].value = 1f - keys[num].value;
			}
			return new AnimationCurve(keys);
		}
	}
}
