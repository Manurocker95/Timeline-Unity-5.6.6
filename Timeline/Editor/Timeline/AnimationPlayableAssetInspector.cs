using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200003C RID: 60
	[CustomEditor(typeof(AnimationPlayableAsset))]
	internal class AnimationPlayableAssetInspector : Editor
	{
		// Token: 0x0600021C RID: 540 RVA: 0x000133A4 File Offset: 0x000117A4
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			if (!this.m_TimelineWindow)
			{
				this.m_TimelineWindow = TimelineWindow.instance;
			}
			AnimationPlayableAsset animationPlayableAsset = base.target as AnimationPlayableAsset;
			if (!(animationPlayableAsset == null))
			{
				Animator animator = (!(this.GetTransform() != null)) ? null : this.GetTransform().GetComponent<Animator>();
				bool flag = this.m_EditorClip != null && !TimelineAnimationUtilities.ValidateOffsetAvailabitity(this.m_EditorClip.director, animator);
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(flag);
				try
				{
					bool enabled = GUI.enabled;
					GUI.enabled = false;
					EditorGUILayout.PropertyField(this.m_AnimClipProperty, AnimationPlayableAssetInspector.Styles.AnimClipText, new GUILayoutOption[0]);
					GUI.enabled = enabled;
					this.ShowRecordableClipRename();
					this.ShowAnimationClipWarnings();
					EditorGUI.DisabledScope disabledScope2;
					disabledScope2..ctor(!this.ShouldShowOffsets());
					try
					{
						this.m_PositionProperty.isExpanded = EditorGUILayout.Foldout(this.m_PositionProperty.isExpanded, AnimationPlayableAssetInspector.Styles.ClipOffsetTitle);
						EditorGUI.BeginChangeCheck();
						if (this.m_PositionProperty.isExpanded)
						{
							float num = 0f;
							float num2 = 0f;
							EditorGUI.indentLevel++;
							TimelineAnimationUtilities.OffsetEditMode offsetEditMode = this.m_OffsetEditMode;
							AnimationTrackInspector.ShowMotionOffsetEditModeToolbar(ref this.m_OffsetEditMode);
							if (offsetEditMode != this.m_OffsetEditMode)
							{
								this.SetTimeToClip();
								SceneView.RepaintAll();
							}
							EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
							EditorGUILayout.PropertyField(this.m_PositionProperty, new GUILayoutOption[0]);
							GUI.skin.button.CalcMinMaxWidth(AnimationPlayableAssetInspector.Styles.PositionIcon, ref num, ref num2);
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
							AnimationPlayableAssetInspector.ShowRotationField(this.m_RotationProperty);
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
							EditorGUILayout.Space();
							AnimationTrackInspector.MatchTargetsField(this.m_MatchTargetFieldsProperty, this.m_TrackMatchTargetFieldsProperty, this.m_UseTrackMatchFieldsProperty, true);
							EditorGUI.indentLevel--;
						}
						if (this.m_RemoveStartOffsetProperty != null)
						{
							EditorGUILayout.PropertyField(this.m_RemoveStartOffsetProperty, new GUILayoutOption[0]);
						}
						bool flag2 = EditorGUI.EndChangeCheck() || this.m_LastPosition != this.m_PositionProperty.vector3Value || this.m_LastRotation != this.m_RotationProperty.quaternionValue;
						this.m_LastPosition = this.m_PositionProperty.vector3Value;
						this.m_LastRotation = this.m_RotationProperty.quaternionValue;
						if (flag2)
						{
							base.serializedObject.ApplyModifiedProperties();
							((AnimationPlayableAsset)base.target).LiveLink();
						}
					}
					finally
					{
						disabledScope2.Dispose();
					}
				}
				finally
				{
					disabledScope.Dispose();
				}
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000136A4 File Offset: 0x00011AA4
		private void Reevaluate()
		{
			if (this.m_TimelineWindow != null && this.m_TimelineWindow.state != null)
			{
				this.m_TimelineWindow.state.EvaluateImmediate();
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000136DC File Offset: 0x00011ADC
		private void SetTimeToClip()
		{
			if (this.m_TimelineWindow != null && this.m_TimelineWindow.state != null)
			{
				this.m_TimelineWindow.state.time = Math.Min(this.m_EditorClip.clip.end, Math.Max(this.m_EditorClip.clip.start, this.m_TimelineWindow.state.time));
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00013758 File Offset: 0x00011B58
		public void OnEnable()
		{
			this.m_EditorClip = (Selection.activeObject as EditorClip);
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
			this.m_PositionProperty = base.serializedObject.FindProperty("m_Position");
			this.m_PositionProperty.isExpanded = true;
			this.m_RotationProperty = base.serializedObject.FindProperty("m_Rotation");
			this.m_RemoveStartOffsetProperty = base.serializedObject.FindProperty("m_RemoveStartOffset");
			this.m_AnimClipProperty = base.serializedObject.FindProperty("m_Clip");
			this.m_UseTrackMatchFieldsProperty = base.serializedObject.FindProperty("m_UseTrackMatchFields");
			this.m_UseTrackMatchFieldsProperty.isExpanded = true;
			this.m_MatchTargetFieldsProperty = base.serializedObject.FindProperty("m_MatchTargetFields");
			this.m_MatchTargetFieldsProperty.isExpanded = true;
			this.m_LastPosition = this.m_PositionProperty.vector3Value;
			this.m_LastRotation = this.m_RotationProperty.quaternionValue;
			if (this.m_EditorClip != null && this.m_EditorClip.clip != null)
			{
				this.m_TrackSerializedObject = new SerializedObject(this.m_EditorClip.track);
				this.m_TrackMatchTargetFieldsProperty = this.m_TrackSerializedObject.FindProperty("m_MatchTargetFields");
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000138AF File Offset: 0x00011CAF
		private void OnDestroy()
		{
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Remove(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x000138D2 File Offset: 0x00011CD2
		private void OnSceneGUI(SceneView sceneView)
		{
			this.DoManipulators();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000138DC File Offset: 0x00011CDC
		private Transform GetTransform()
		{
			Transform result;
			if (this.m_Binding != null)
			{
				result = this.m_Binding.transform;
			}
			else
			{
				if (this.m_TimelineWindow != null && this.m_TimelineWindow.state != null && this.m_TimelineWindow.state.currentDirector != null && this.m_EditorClip != null && this.m_EditorClip.clip != null)
				{
					GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(this.m_TimelineWindow.state.currentDirector, this.m_EditorClip.clip.parentTrack);
					this.m_Binding = sceneGameObject;
					if (sceneGameObject != null)
					{
						return sceneGameObject.transform;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000139B8 File Offset: 0x00011DB8
		private void DoManipulators()
		{
			if (!(this.m_EditorClip == null) && this.m_EditorClip.clip != null)
			{
				AnimationPlayableAsset animationPlayableAsset = this.m_EditorClip.clip.asset as AnimationPlayableAsset;
				AnimationTrack animationTrack = this.m_EditorClip.clip.parentTrack as AnimationTrack;
				Transform transform = this.GetTransform();
				if (transform != null && animationPlayableAsset != null && this.m_OffsetEditMode != TimelineAnimationUtilities.OffsetEditMode.None && animationTrack != null)
				{
					Vector3 vector = transform.position;
					Quaternion quaternion = transform.rotation;
					EditorGUI.BeginChangeCheck();
					if (this.m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Translation)
					{
						vector = Handles.PositionHandle(vector, (Tools.pivotRotation != 1) ? quaternion : Quaternion.identity);
					}
					else if (this.m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Rotation)
					{
						quaternion = Handles.RotationHandle(quaternion, vector);
					}
					if (EditorGUI.EndChangeCheck())
					{
						TimelineAnimationUtilities.RigidTransform rigidTransform = TimelineAnimationUtilities.UpdateClipOffsets(animationPlayableAsset, animationTrack, transform, vector, quaternion);
						animationPlayableAsset.position = rigidTransform.position;
						animationPlayableAsset.rotation = rigidTransform.rotation;
						this.Reevaluate();
						base.Repaint();
					}
				}
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00013AEC File Offset: 0x00011EEC
		public static void ShowRotationField(SerializedProperty rotation)
		{
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			GUIContent guicontent = EditorGUI.BeginProperty(controlRect, AnimationPlayableAssetInspector.Styles.RotationText, rotation);
			EditorGUI.BeginChangeCheck();
			Vector3 vector = EditorGUI.Vector3Field(controlRect, guicontent, rotation.quaternionValue.eulerAngles);
			if (EditorGUI.EndChangeCheck())
			{
				rotation.quaternionValue = Quaternion.Euler(vector);
			}
			EditorGUI.EndProperty();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00013B4C File Offset: 0x00011F4C
		private void ShowAnimationClipWarnings()
		{
			AnimationClip animationClip = this.m_AnimClipProperty.objectReferenceValue as AnimationClip;
			if (!(animationClip == null))
			{
				if (animationClip.legacy)
				{
					EditorGUILayout.HelpBox(AnimationPlayableAssetInspector.Styles.LegacyError.text, 3);
				}
				else
				{
					bool flag = AnimationUtility.HasGenericRootTransform(animationClip);
					bool flag2 = AnimationUtility.HasMotionCurves(animationClip);
					bool flag3 = AnimationUtility.HasRootCurves(animationClip);
					if (flag && !flag2 && !flag3)
					{
						EditorGUILayout.HelpBox(AnimationPlayableAssetInspector.Styles.MotionCurveWarning.text, 2);
					}
				}
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00013BD8 File Offset: 0x00011FD8
		private bool ShouldShowOffsets()
		{
			AnimationClip animationClip = this.m_AnimClipProperty.objectReferenceValue as AnimationClip;
			return !(animationClip == null) && (AnimationUtility.HasGenericRootTransform(animationClip) || AnimationUtility.HasMotionCurves(animationClip) || AnimationUtility.HasRootCurves(animationClip));
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00013C30 File Offset: 0x00012030
		private void ShowRecordableClipRename()
		{
			if (base.targets.Length <= 1 && this.m_EditorClip.clip.recordable)
			{
				AnimationClip animationClip = this.m_AnimClipProperty.objectReferenceValue as AnimationClip;
				if (!(animationClip == null) && AssetDatabase.IsSubAsset(animationClip))
				{
					if (this.m_SerializedAnimClip == null)
					{
						this.m_SerializedAnimClip = new SerializedObject(animationClip);
						this.m_SerializedAnimClipName = this.m_SerializedAnimClip.FindProperty("m_Name");
					}
					if (this.m_SerializedAnimClipName != null)
					{
						this.m_SerializedAnimClip.Update();
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.DelayedTextField(this.m_SerializedAnimClipName, AnimationPlayableAssetInspector.Styles.AnimationClipName, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							this.m_SerializedAnimClip.ApplyModifiedProperties();
						}
					}
				}
			}
		}

		// Token: 0x0400019F RID: 415
		private TimelineWindow m_TimelineWindow = null;

		// Token: 0x040001A0 RID: 416
		private GameObject m_Binding;

		// Token: 0x040001A1 RID: 417
		private TimelineAnimationUtilities.OffsetEditMode m_OffsetEditMode = TimelineAnimationUtilities.OffsetEditMode.None;

		// Token: 0x040001A2 RID: 418
		private EditorClip m_EditorClip;

		// Token: 0x040001A3 RID: 419
		private SerializedProperty m_PositionProperty;

		// Token: 0x040001A4 RID: 420
		private SerializedProperty m_RotationProperty;

		// Token: 0x040001A5 RID: 421
		private SerializedProperty m_RemoveStartOffsetProperty;

		// Token: 0x040001A6 RID: 422
		private SerializedProperty m_AnimClipProperty;

		// Token: 0x040001A7 RID: 423
		private SerializedProperty m_UseTrackMatchFieldsProperty;

		// Token: 0x040001A8 RID: 424
		private SerializedProperty m_MatchTargetFieldsProperty;

		// Token: 0x040001A9 RID: 425
		private SerializedProperty m_TrackMatchTargetFieldsProperty;

		// Token: 0x040001AA RID: 426
		private SerializedObject m_TrackSerializedObject;

		// Token: 0x040001AB RID: 427
		private SerializedObject m_SerializedAnimClip;

		// Token: 0x040001AC RID: 428
		private SerializedProperty m_SerializedAnimClipName;

		// Token: 0x040001AD RID: 429
		private Vector3 m_LastPosition;

		// Token: 0x040001AE RID: 430
		private Quaternion m_LastRotation;

		// Token: 0x0200003D RID: 61
		private static class Styles
		{
			// Token: 0x040001AF RID: 431
			public static readonly GUIContent RotationText = EditorGUIUtility.TextContent("Rotation");

			// Token: 0x040001B0 RID: 432
			public static readonly GUIContent AnimClipText = EditorGUIUtility.TextContent("Animation Clip");

			// Token: 0x040001B1 RID: 433
			public static readonly GUIContent PositionIcon = EditorGUIUtility.IconContent("MoveTool");

			// Token: 0x040001B2 RID: 434
			public static readonly GUIContent ClipOffsetTitle = EditorGUIUtility.TextContent("Clip Root Motion Offsets");

			// Token: 0x040001B3 RID: 435
			public static readonly GUIContent MotionCurveWarning = EditorGUIUtility.TextContent("The animation clip does not have any motion curves, and may not playback as expected. Assign a Root Motion Node, or Generate Root Motion curves");

			// Token: 0x040001B4 RID: 436
			public static readonly GUIContent LegacyError = EditorGUIUtility.TextContent("Legacy animation clips are not supported");

			// Token: 0x040001B5 RID: 437
			public static readonly GUIContent AnimationClipName = EditorGUIUtility.TextContent("Animation Clip Name");
		}
	}
}
