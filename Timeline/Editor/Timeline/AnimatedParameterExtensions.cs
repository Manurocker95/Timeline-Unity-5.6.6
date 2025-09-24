using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000037 RID: 55
	internal static class AnimatedParameterExtensions
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00011800 File Offset: 0x0000FC00
		private static SerializedObject GetSerializedObject(TimelineClip clip)
		{
			SerializedObject result;
			if (clip == null)
			{
				result = null;
			}
			else if (!(clip.asset is IPlayableAsset))
			{
				result = null;
			}
			else
			{
				ScriptableObject scriptableObject = clip.asset as ScriptableObject;
				if (scriptableObject == null)
				{
					result = null;
				}
				else
				{
					if (AnimatedParameterExtensions.s_CachedObject == null || AnimatedParameterExtensions.s_CachedObject.targetObject != clip.asset)
					{
						AnimatedParameterExtensions.s_CachedObject = new SerializedObject(scriptableObject);
					}
					result = AnimatedParameterExtensions.s_CachedObject;
				}
			}
			return result;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00011890 File Offset: 0x0000FC90
		private static bool IsKeyable(Type t, string parameterName)
		{
			string name = parameterName;
			int num = parameterName.IndexOf('.');
			if (num > 0)
			{
				name = parameterName.Substring(0, num);
			}
			FieldInfo fieldInfo = t.GetField(name) ?? t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
			return fieldInfo == null || !fieldInfo.IsDefined(typeof(NotKeyableAttribute), true);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000118F8 File Offset: 0x0000FCF8
		public static bool IsAnimatable(SerializedPropertyType t)
		{
			switch (t)
			{
			case 1:
			case 2:
			case 4:
			case 8:
			case 9:
			case 10:
				break;
			default:
				if (t != 17)
				{
					return false;
				}
				break;
			}
			return true;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00011954 File Offset: 0x0000FD54
		private static bool MatchBinding(EditorCurveBinding binding, string parameterName)
		{
			bool result;
			if (binding.propertyName == parameterName)
			{
				result = true;
			}
			else
			{
				int num = binding.propertyName.IndexOf('.');
				result = (num > 0 && parameterName.Length == num && binding.propertyName.StartsWith(parameterName));
			}
			return result;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000119B4 File Offset: 0x0000FDB4
		public static bool HasAnyAnimatableParameters(this TimelineClip clip)
		{
			bool result;
			if (clip.asset == null || Attribute.IsDefined(clip.asset.GetType(), typeof(NotKeyableAttribute)))
			{
				result = false;
			}
			else if (!clip.HasScriptPlayable())
			{
				result = false;
			}
			else
			{
				SerializedObject serializedObject = AnimatedParameterExtensions.GetSerializedObject(clip);
				if (serializedObject == null)
				{
					result = false;
				}
				else
				{
					SerializedProperty iterator = serializedObject.GetIterator();
					bool flag = true;
					bool flag2 = clip.asset is IScriptPlayable;
					while (iterator.NextVisible(flag))
					{
						if (AnimatedParameterExtensions.IsAnimatable(iterator.propertyType) && AnimatedParameterExtensions.IsKeyable(clip.asset.GetType(), iterator.propertyPath))
						{
							return flag2 || clip.IsAnimatablePath(iterator.propertyPath);
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00011A9C File Offset: 0x0000FE9C
		public static bool IsParameterAnimatable(this TimelineClip clip, string parameterName)
		{
			bool result;
			if (clip.asset == null || Attribute.IsDefined(clip.asset.GetType(), typeof(NotKeyableAttribute)))
			{
				result = false;
			}
			else if (!clip.HasScriptPlayable())
			{
				result = false;
			}
			else
			{
				SerializedObject serializedObject = AnimatedParameterExtensions.GetSerializedObject(clip);
				if (serializedObject == null)
				{
					result = false;
				}
				else
				{
					bool flag = clip.asset is IScriptPlayable;
					SerializedProperty serializedProperty = serializedObject.FindProperty(parameterName);
					result = (serializedProperty != null && AnimatedParameterExtensions.IsAnimatable(serializedProperty.propertyType) && AnimatedParameterExtensions.IsKeyable(clip.asset.GetType(), parameterName) && (flag || clip.IsAnimatablePath(serializedProperty.propertyPath)));
				}
			}
			return result;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00011B70 File Offset: 0x0000FF70
		public static bool IsParameterAnimated(this TimelineClip clip, string parameterName)
		{
			bool result;
			if (clip == null)
			{
				result = false;
			}
			else if (clip.curves == null)
			{
				result = false;
			}
			else
			{
				EditorCurveBinding binding = clip.GetCurveBinding(parameterName);
				EditorCurveBinding[] bindings = AnimationClipCurveCache.Instance.GetCurveInfo(clip.curves).bindings;
				result = bindings.Any((EditorCurveBinding x) => AnimatedParameterExtensions.MatchBinding(x, binding.propertyName));
			}
			return result;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00011BE8 File Offset: 0x0000FFE8
		public static EditorCurveBinding GetCurveBinding(this TimelineClip clip, string parameterName)
		{
			string animatedParameterBindingName = AnimatedParameterExtensions.GetAnimatedParameterBindingName(clip, parameterName);
			return EditorCurveBinding.FloatCurve(string.Empty, AnimatedParameterExtensions.GetAnimationType(clip), animatedParameterBindingName);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00011C18 File Offset: 0x00010018
		private static Type GetAnimationType(TimelineClip clip)
		{
			Type result;
			if (clip != null && clip.asset != null && clip.asset != null)
			{
				result = clip.asset.GetType();
			}
			else
			{
				result = typeof(TimelineAsset);
			}
			return result;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00011C6C File Offset: 0x0001006C
		private static string GetAnimatedParameterBindingName(TimelineClip clip, string parameterName)
		{
			string result;
			if (clip == null || clip.asset == null || clip.asset is IScriptPlayable)
			{
				result = parameterName;
			}
			else
			{
				IEnumerable<FieldInfo> scriptPlayableFields = AnimatedParameterExtensions.GetScriptPlayableFields(clip.asset as IPlayableAsset);
				foreach (FieldInfo fieldInfo in scriptPlayableFields)
				{
					if (parameterName.StartsWith(fieldInfo.Name))
					{
						if (parameterName.Length > fieldInfo.Name.Length && parameterName[fieldInfo.Name.Length] == '.')
						{
							return parameterName.Substring(fieldInfo.Name.Length + 1);
						}
					}
				}
				result = parameterName;
			}
			return result;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00011D60 File Offset: 0x00010160
		public static bool AddAnimatedParameterValueAt(this TimelineClip clip, string parameterName, float value, float time)
		{
			bool result;
			if (!clip.IsParameterAnimatable(parameterName))
			{
				result = false;
			}
			else
			{
				AnimatedParameterExtensions.CreateCurvesIfRequired(clip, null);
				EditorCurveBinding curveBinding = clip.GetCurveBinding(parameterName);
				AnimationCurve animationCurve = AnimationUtility.GetEditorCurve(clip.curves, curveBinding) ?? new AnimationCurve();
				SerializedObject serializedObject = AnimatedParameterExtensions.GetSerializedObject(clip);
				SerializedProperty serializedProperty = serializedObject.FindProperty(parameterName);
				bool stepped = serializedProperty.propertyType == 1 || serializedProperty.propertyType == null || serializedProperty.propertyType == 7;
				CurveEditUtility.AddKeyFrameToCurve(animationCurve, time, clip.curves.frameRate, value, stepped);
				AnimationUtility.SetEditorCurve(clip.curves, curveBinding, animationCurve);
				result = true;
			}
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00011E0C File Offset: 0x0001020C
		internal static void CreateCurvesIfRequired(TimelineClip clip, TrackAsset parentTrack = null)
		{
			if (clip.curves == null)
			{
				if (parentTrack == null)
				{
					parentTrack = clip.parentTrack;
				}
				clip.AllocateAnimatedParameterCurves();
				clip.curves.name = AnimationTrackRecorder.GetUniqueRecordedClipName(clip.parentTrack, AnimatedParameterExtensions.kDefaultClipName);
				string assetPath = AssetDatabase.GetAssetPath(clip.parentTrack);
				if (!string.IsNullOrEmpty(assetPath))
				{
					TimelineHelpers.SaveAnimClipIntoObject(clip.curves, clip.parentTrack);
					EditorUtility.SetDirty(clip.parentTrack);
					AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(clip.parentTrack));
					AssetDatabase.Refresh();
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00011EAC File Offset: 0x000102AC
		private static bool InternalAddParameter(TimelineClip clip, string parameterName, ref EditorCurveBinding binding, out SerializedProperty property)
		{
			property = null;
			bool result;
			if (clip.IsParameterAnimated(parameterName))
			{
				result = false;
			}
			else
			{
				SerializedObject serializedObject = AnimatedParameterExtensions.GetSerializedObject(clip);
				if (serializedObject == null)
				{
					result = false;
				}
				else
				{
					property = serializedObject.FindProperty(parameterName);
					if (property == null || !AnimatedParameterExtensions.IsAnimatable(property.propertyType))
					{
						result = false;
					}
					else
					{
						AnimatedParameterExtensions.CreateCurvesIfRequired(clip, null);
						binding = clip.GetCurveBinding(parameterName);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00011F28 File Offset: 0x00010328
		public static bool AddAnimatedParameter(this TimelineClip clip, string parameterName)
		{
			EditorCurveBinding sourceBinding = default(EditorCurveBinding);
			SerializedProperty prop;
			bool result;
			if (!AnimatedParameterExtensions.InternalAddParameter(clip, parameterName, ref sourceBinding, out prop))
			{
				result = false;
			}
			else
			{
				float num = (float)clip.duration;
				CurveEditUtility.AddKey(clip.curves, sourceBinding, prop, 0.0);
				CurveEditUtility.AddKey(clip.curves, sourceBinding, prop, (double)num);
				result = true;
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00011F8C File Offset: 0x0001038C
		public static bool RemoveAnimatedParameter(this TimelineClip clip, string parameterName)
		{
			bool result;
			if (!clip.IsParameterAnimated(parameterName) || clip.curves == null)
			{
				result = false;
			}
			else
			{
				EditorCurveBinding curveBinding = clip.GetCurveBinding(parameterName);
				AnimationUtility.SetEditorCurve(clip.curves, curveBinding, null);
				result = true;
			}
			return result;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00011FDC File Offset: 0x000103DC
		public static AnimationCurve GetAnimatedParameter(this TimelineClip clip, string parameterName)
		{
			AnimationCurve result;
			if (clip == null || clip.curves == null)
			{
				result = null;
			}
			else
			{
				ScriptableObject scriptableObject = clip.asset as ScriptableObject;
				if (scriptableObject == null)
				{
					result = null;
				}
				else
				{
					EditorCurveBinding curveBinding = clip.GetCurveBinding(parameterName);
					result = AnimationUtility.GetEditorCurve(clip.curves, curveBinding);
				}
			}
			return result;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00012044 File Offset: 0x00010444
		public static bool SetAnimatedParameter(this TimelineClip clip, string parameterName, AnimationCurve curve)
		{
			bool result;
			if (!clip.IsParameterAnimated(parameterName) && !clip.AddAnimatedParameter(parameterName))
			{
				result = false;
			}
			else
			{
				EditorCurveBinding curveBinding = clip.GetCurveBinding(parameterName);
				AnimationUtility.SetEditorCurve(clip.curves, curveBinding, curve);
				result = true;
			}
			return result;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00012090 File Offset: 0x00010490
		internal static bool HasScriptPlayable(this TimelineClip clip)
		{
			bool result;
			if (clip.asset == null)
			{
				result = false;
			}
			else
			{
				IScriptPlayable scriptPlayable = clip.asset as IScriptPlayable;
				result = (scriptPlayable != null || AnimatedParameterExtensions.GetScriptPlayableFields(clip.asset as IPlayableAsset).Any<FieldInfo>());
			}
			return result;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000120EC File Offset: 0x000104EC
		internal static bool IsAnimatablePath(this TimelineClip clip, string path)
		{
			return !(clip.asset == null) && AnimatedParameterExtensions.GetScriptPlayableFields(clip.asset as IPlayableAsset).Any((FieldInfo f) => path.StartsWith(f.Name) && path.Length > f.Name.Length && path[f.Name.Length] == '.');
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00012148 File Offset: 0x00010548
		internal static IEnumerable<FieldInfo> GetScriptPlayableFields(IPlayableAsset asset)
		{
			IEnumerable<FieldInfo> result;
			if (asset == null)
			{
				result = new FieldInfo[0];
			}
			else
			{
				result = from f in asset.GetType().GetFields()
				where f.IsPublic && !f.IsStatic && typeof(IScriptPlayable).IsAssignableFrom(f.FieldType)
				select f;
			}
			return result;
		}

		// Token: 0x04000197 RID: 407
		private static readonly string kDefaultClipName = "Parameters";

		// Token: 0x04000198 RID: 408
		private static SerializedObject s_CachedObject = null;
	}
}
