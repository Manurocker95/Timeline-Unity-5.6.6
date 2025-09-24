using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x020000B2 RID: 178
	internal class PropertyCollector : IPropertyCollector
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x0002BA46 File Offset: 0x00029E46
		public void Reset()
		{
			this.m_ObjectStack.Clear();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002BA54 File Offset: 0x00029E54
		public void PushActiveGameObject(GameObject gameObject)
		{
			this.m_ObjectStack.Push(gameObject);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002BA63 File Offset: 0x00029E63
		public void PopActiveGameObject()
		{
			this.m_ObjectStack.Pop();
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0002BA74 File Offset: 0x00029E74
		public void AddFromClip(AnimationClip clip)
		{
			GameObject gameObject = this.m_ObjectStack.Peek();
			if (gameObject != null && clip != null)
			{
				this.AddFromClip(gameObject, clip);
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002BAB0 File Offset: 0x00029EB0
		public void AddFromName<T>(string name) where T : Component
		{
			GameObject gameObject = this.m_ObjectStack.Peek();
			if (gameObject != null)
			{
				this.AddFromName<T>(gameObject, name);
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0002BAE0 File Offset: 0x00029EE0
		public void AddFromName(string name)
		{
			GameObject gameObject = this.m_ObjectStack.Peek();
			if (gameObject != null)
			{
				this.AddFromName(gameObject, name);
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002BB0E File Offset: 0x00029F0E
		public void AddFromClip(GameObject obj, AnimationClip clip)
		{
			if (!Application.isPlaying)
			{
				PropertyCollector.AddPropertiesFromClip(obj, clip);
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002BB22 File Offset: 0x00029F22
		public void AddFromName<T>(GameObject obj, string name) where T : Component
		{
			if (!Application.isPlaying)
			{
				PropertyCollector.AddPropertiesFromName(obj, typeof(T), name);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002BB40 File Offset: 0x00029F40
		public void AddFromName(GameObject obj, string name)
		{
			if (!Application.isPlaying)
			{
				PropertyCollector.AddPropertiesFromName(obj, name);
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002BB54 File Offset: 0x00029F54
		public void AddFromComponent(GameObject obj, Component component)
		{
			if (!Application.isPlaying)
			{
				if (!(obj == null) && !(component == null))
				{
					Type type = component.GetType();
					SerializedObject serializedObject = new SerializedObject(component);
					SerializedProperty iterator = serializedObject.GetIterator();
					while (iterator.NextVisible(true))
					{
						if (!iterator.hasVisibleChildren && AnimatedParameterExtensions.IsAnimatable(iterator.propertyType))
						{
							string propertyPath = iterator.propertyPath;
							EditorCurveBinding editorCurveBinding = EditorCurveBinding.FloatCurve(string.Empty, type, propertyPath);
							if (iterator.propertyType == 5)
							{
								editorCurveBinding = EditorCurveBinding.PPtrCurve(string.Empty, type, propertyPath);
							}
							AnimationMode.AddPropertyModificationFromCurveBinding(obj, editorCurveBinding);
						}
					}
				}
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002BC0D File Offset: 0x0002A00D
		private static void AddPropertiesFromClip(GameObject go, AnimationClip clip)
		{
			AnimationMode.InitializePropertyModificationForGameObject(go, clip);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002BC18 File Offset: 0x0002A018
		private static void AddPropertiesFromName(GameObject go, string property)
		{
			if (!(go == null))
			{
				EditorCurveBinding editorCurveBinding = EditorCurveBinding.FloatCurve(string.Empty, typeof(GameObject), property);
				AnimationMode.AddPropertyModificationFromCurveBinding(go, editorCurveBinding);
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002BC54 File Offset: 0x0002A054
		private static void AddPropertiesFromName(GameObject go, Type compType, string property)
		{
			if (!(go == null))
			{
				Component component = go.GetComponent(compType);
				if (!(component == null))
				{
					SerializedObject serializedObject = new SerializedObject(component);
					SerializedProperty serializedProperty = serializedObject.FindProperty(property);
					if (serializedProperty != null)
					{
						EditorCurveBinding editorCurveBinding = EditorCurveBinding.FloatCurve(string.Empty, compType, property);
						if (serializedProperty.propertyType == 5)
						{
							editorCurveBinding = EditorCurveBinding.PPtrCurve(string.Empty, compType, property);
						}
						AnimationMode.AddPropertyModificationFromCurveBinding(go, editorCurveBinding);
					}
				}
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002BCD0 File Offset: 0x0002A0D0
		public void AddObjectProperties(Object obj, AnimationClip clip)
		{
			if (!(obj == null) && !(clip == null))
			{
				AnimationMode.InitializePropertyModificationForObject(obj, clip);
				IPlayableAsset playableAsset = obj as IPlayableAsset;
				IScriptPlayable scriptPlayable = obj as IScriptPlayable;
				if (playableAsset != null && scriptPlayable == null)
				{
					this.AddSerializedPlayableModifications(playableAsset, clip);
				}
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002BD28 File Offset: 0x0002A128
		private void AddSerializedPlayableModifications(IPlayableAsset asset, AnimationClip clip)
		{
			Object @object = asset as Object;
			if (!(@object == null))
			{
				EditorCurveBinding[] bindings = AnimationClipCurveCache.Instance.GetCurveInfo(clip).bindings;
				List<FieldInfo> list = AnimatedParameterExtensions.GetScriptPlayableFields(asset).ToList<FieldInfo>();
				foreach (EditorCurveBinding editorCurveBinding in bindings)
				{
					foreach (FieldInfo fieldInfo in list)
					{
						EditorCurveBinding editorCurveBinding2 = editorCurveBinding;
						editorCurveBinding2.propertyName = fieldInfo.Name + "." + editorCurveBinding2.propertyName;
						AnimationMode.InitializePropertyModificationForObject(@object, editorCurveBinding2);
					}
				}
			}
		}

		// Token: 0x0400039B RID: 923
		private readonly Stack<GameObject> m_ObjectStack = new Stack<GameObject>();
	}
}
