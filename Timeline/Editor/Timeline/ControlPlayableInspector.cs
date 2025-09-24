using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000048 RID: 72
	[CustomEditor(typeof(ControlPlayableAsset))]
	internal class ControlPlayableInspector : Editor
	{
		// Token: 0x06000261 RID: 609 RVA: 0x000153D4 File Offset: 0x000137D4
		public void OnEnable()
		{
			this.m_SourceObject = base.serializedObject.FindProperty("sourceGameObject");
			this.m_PrefabObject = base.serializedObject.FindProperty("prefabGameObject");
			this.m_UpdateParticle = base.serializedObject.FindProperty("updateParticle");
			this.m_UpdateDirector = base.serializedObject.FindProperty("updateDirector");
			this.m_UpdateITimeControl = base.serializedObject.FindProperty("updateITimeControl");
			this.m_SearchHierarchy = base.serializedObject.FindProperty("searchHierarchy");
			this.m_UseActivation = base.serializedObject.FindProperty("active");
			this.m_PostPlayback = base.serializedObject.FindProperty("postPlayback");
			this.m_RandomSeed = base.serializedObject.FindProperty("particleRandomSeed");
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000154A8 File Offset: 0x000138A8
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			this.sourceObjectLabel.text = this.m_SourceObject.displayName;
			if (this.m_PrefabObject.objectReferenceValue != null)
			{
				this.sourceObjectLabel.text = "Parent Object";
			}
			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.m_SourceObject, this.sourceObjectLabel, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.DisablePlayOnAwake(this.m_SourceObject.exposedReferenceValue as GameObject);
			}
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(this.m_PrefabObject, ControlPlayableInspector.Styles.prefabContent, new GUILayoutOption[0]);
			EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField(this.m_UseActivation, ControlPlayableInspector.Styles.activationContent, new GUILayoutOption[0]);
			if (this.m_UseActivation.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.m_PostPlayback, ControlPlayableInspector.Styles.postPlayableContent, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
			}
			this.m_SourceObject.isExpanded = EditorGUILayout.Foldout(this.m_SourceObject.isExpanded, ControlPlayableInspector.Styles.advancedContent);
			if (this.m_SourceObject.isExpanded)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.m_UpdateParticle, ControlPlayableInspector.Styles.updateParticleSystemsContent, new GUILayoutOption[0]);
				if (this.m_UpdateParticle.boolValue)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_RandomSeed, ControlPlayableInspector.Styles.randomSeedContent, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.PropertyField(this.m_UpdateDirector, ControlPlayableInspector.Styles.updatePlayableDirectorContent, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_UpdateITimeControl, ControlPlayableInspector.Styles.updateITimeControlContent, new GUILayoutOption[0]);
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(!this.m_UpdateDirector.boolValue && !this.m_UpdateParticle.boolValue && !this.m_UpdateITimeControl.boolValue);
				try
				{
					EditorGUILayout.PropertyField(this.m_SearchHierarchy, ControlPlayableInspector.Styles.updateHierarchy, new GUILayoutOption[0]);
				}
				finally
				{
					disabledScope.Dispose();
				}
				EditorGUI.indentLevel--;
			}
			if (EditorGUI.EndChangeCheck())
			{
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00015724 File Offset: 0x00013B24
		public void DisablePlayOnAwake(GameObject sourceObject)
		{
			if (sourceObject != null && this.m_UpdateDirector.boolValue)
			{
				if (this.m_SearchHierarchy.boolValue)
				{
					PlayableDirector[] componentsInChildren = sourceObject.GetComponentsInChildren<PlayableDirector>();
					foreach (PlayableDirector director in componentsInChildren)
					{
						this.DisablePlayOnAwake(director);
					}
				}
				else
				{
					this.DisablePlayOnAwake(sourceObject.GetComponent<PlayableDirector>());
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000157A0 File Offset: 0x00013BA0
		public void DisablePlayOnAwake(PlayableDirector director)
		{
			if (!(director == null))
			{
				SerializedObject serializedObject = new SerializedObject(director);
				SerializedProperty serializedProperty = serializedObject.FindProperty("m_InitialState");
				serializedProperty.enumValueIndex = 0;
				serializedObject.ApplyModifiedProperties();
			}
		}

		// Token: 0x040001E1 RID: 481
		private SerializedProperty m_SourceObject;

		// Token: 0x040001E2 RID: 482
		private SerializedProperty m_PrefabObject;

		// Token: 0x040001E3 RID: 483
		private SerializedProperty m_UpdateParticle;

		// Token: 0x040001E4 RID: 484
		private SerializedProperty m_UpdateDirector;

		// Token: 0x040001E5 RID: 485
		private SerializedProperty m_UpdateITimeControl;

		// Token: 0x040001E6 RID: 486
		private SerializedProperty m_SearchHierarchy;

		// Token: 0x040001E7 RID: 487
		private SerializedProperty m_UseActivation;

		// Token: 0x040001E8 RID: 488
		private SerializedProperty m_PostPlayback;

		// Token: 0x040001E9 RID: 489
		private SerializedProperty m_RandomSeed;

		// Token: 0x040001EA RID: 490
		private GUIContent sourceObjectLabel = new GUIContent();

		// Token: 0x02000049 RID: 73
		private static class Styles
		{
			// Token: 0x040001EB RID: 491
			public static readonly GUIContent activationContent = EditorGUIUtility.TextContent("Control Activation|When checked the clip will control the active state of the source game object");

			// Token: 0x040001EC RID: 492
			public static readonly GUIContent prefabContent = EditorGUIUtility.TextContent("Prefab|A prefab to instantiate as a child object of the source game object");

			// Token: 0x040001ED RID: 493
			public static readonly GUIContent advancedContent = EditorGUIUtility.TextContent("Advanced");

			// Token: 0x040001EE RID: 494
			public static readonly GUIContent updateParticleSystemsContent = EditorGUIUtility.TextContent("Control Particle Systems|Synchronize the time between the clip and any particle systems on the game object");

			// Token: 0x040001EF RID: 495
			public static readonly GUIContent updatePlayableDirectorContent = EditorGUIUtility.TextContent("Control Playable Directors|Synchronize the time between the clip and any playable directors on the game object");

			// Token: 0x040001F0 RID: 496
			public static readonly GUIContent updateITimeControlContent = EditorGUIUtility.TextContent("Control ITimeControl|Synchronize the time between the clip and any Script that implements the ITimeControl interface on the game object");

			// Token: 0x040001F1 RID: 497
			public static readonly GUIContent updateHierarchy = EditorGUIUtility.TextContent("Control Children|Search child game objects for particle systems and playable directors");

			// Token: 0x040001F2 RID: 498
			public static readonly GUIContent randomSeedContent = EditorGUIUtility.TextContent("Random Seed|A random seem to provide the particle systems for consistent previews");

			// Token: 0x040001F3 RID: 499
			public static readonly GUIContent postPlayableContent = EditorGUIUtility.TextContent("Post Playback|The active state to the leave the game object when the timeline is finished. \n\nRevert will leave the game object in the state it was prior to the timeline being run");
		}
	}
}
