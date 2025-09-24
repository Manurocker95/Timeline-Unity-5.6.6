using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200003E RID: 62
	[CustomEditor(typeof(AnimationTrack))]
	internal class AnimationTrackInspector : TrackAssetInspector
	{
		// Token: 0x0600022A RID: 554 RVA: 0x00013D94 File Offset: 0x00012194
		private void Evaluate()
		{
			if (base.sequencerWindow.state != null && base.sequencerWindow.state.currentDirector != null)
			{
				base.sequencerWindow.state.currentDirector.Evaluate();
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00013DE4 File Offset: 0x000121E4
		private void RebuildGraph()
		{
			if (base.sequencerWindow.state != null)
			{
				base.sequencerWindow.state.rebuildGraph = true;
				base.sequencerWindow.Repaint();
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00013E18 File Offset: 0x00012218
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			base.serializedObject.Update();
			bool flag = this.m_lastPosition != this.m_TrackPositionProperty.vector3Value || this.m_lastRotation != this.m_TrackRotationProperty.quaternionValue;
			this.m_lastPosition = this.m_TrackPositionProperty.vector3Value;
			this.m_lastRotation = this.m_TrackRotationProperty.quaternionValue;
			AnimationTrack animationTrack = (AnimationTrack)base.target;
			Animator animator = (!(this.GetTransform() != null)) ? null : this.GetTransform().GetComponent<Animator>();
			bool flag2 = base.sequencerWindow.state == null || !TimelineAnimationUtilities.ValidateOffsetAvailabitity(base.sequencerWindow.state.currentDirector, animator);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.m_ApplyOffsetsProperty, AnimationTrackInspector.Styles.TrackOffsetTitle, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.RebuildGraph();
			}
			flag2 |= (this.m_ApplyOffsetsProperty.hasMultipleDifferentValues || !this.m_ApplyOffsetsProperty.boolValue);
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(flag2);
			try
			{
				EditorGUI.indentLevel++;
				float num = 0f;
				float num2 = 0f;
				GUI.skin.button.CalcMinMaxWidth(AnimationTrackInspector.Styles.PositionIcon, ref num, ref num2);
				AnimationTrackInspector.ShowMotionOffsetEditModeToolbar(ref this.m_OffsetEditMode);
				SceneView.RepaintAll();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_TrackPositionProperty, new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				AnimationPlayableAssetInspector.ShowRotationField(this.m_TrackRotationProperty);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUI.indentLevel--;
				flag |= EditorGUI.EndChangeCheck();
				if (flag)
				{
					animationTrack.UpdateClipOffsets();
					this.Evaluate();
				}
			}
			finally
			{
				disabledScope.Dispose();
			}
			AnimationTrackInspector.MatchTargetsField(this.m_MatchFieldsProperty, null, null, false);
			base.serializedObject.ApplyModifiedProperties();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0001403C File Offset: 0x0001243C
		public static void ShowMotionOffsetEditModeToolbar(ref TimelineAnimationUtilities.OffsetEditMode motionOffset)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.FlexibleSpace();
			int num = GUILayout.Toolbar((int)motionOffset, new GUIContent[]
			{
				AnimationTrackInspector.Styles.PositionIcon,
				AnimationTrackInspector.Styles.RotationIcon
			}, new GUILayoutOption[0]);
			if (GUI.changed)
			{
				if (motionOffset == (TimelineAnimationUtilities.OffsetEditMode)num)
				{
					motionOffset = TimelineAnimationUtilities.OffsetEditMode.None;
				}
				else
				{
					motionOffset = (TimelineAnimationUtilities.OffsetEditMode)num;
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(3f);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000140B8 File Offset: 0x000124B8
		public override void OnEnable()
		{
			base.OnEnable();
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
			this.m_MatchFieldsProperty = base.serializedObject.FindProperty("m_MatchTargetFields");
			this.m_MatchFieldsProperty.isExpanded = true;
			this.m_TrackPositionProperty = base.serializedObject.FindProperty("m_Position");
			this.m_TrackPositionProperty.isExpanded = true;
			this.m_TrackRotationProperty = base.serializedObject.FindProperty("m_Rotation");
			this.m_ApplyOffsetsProperty = base.serializedObject.FindProperty("m_ApplyOffsets");
			this.m_lastPosition = this.m_TrackPositionProperty.vector3Value;
			this.m_lastRotation = this.m_TrackRotationProperty.quaternionValue;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0001417E File Offset: 0x0001257E
		public override void OnDestroy()
		{
			base.OnDestroy();
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Remove(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000141A7 File Offset: 0x000125A7
		private void OnSceneGUI(SceneView sceneView)
		{
			if (this.m_ApplyOffsetsProperty != null && !this.m_ApplyOffsetsProperty.hasMultipleDifferentValues && this.m_ApplyOffsetsProperty.boolValue)
			{
				this.DoManipulators();
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000141DC File Offset: 0x000125DC
		private Transform GetTransform()
		{
			Transform result;
			if (this.m_Binding != null)
			{
				result = this.m_Binding.transform;
			}
			else
			{
				TrackAsset trackAsset = base.target as TrackAsset;
				if (trackAsset != null && base.sequencerWindow.state != null && base.sequencerWindow.state.currentDirector != null)
				{
					GameObject sceneGameObject = TimelineUtility.GetSceneGameObject(base.sequencerWindow.state.currentDirector, trackAsset);
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

		// Token: 0x06000232 RID: 562 RVA: 0x00014290 File Offset: 0x00012690
		private void DoManipulators()
		{
			AnimationTrack animationTrack = base.target as AnimationTrack;
			Transform transform = this.GetTransform();
			if (transform != null && animationTrack != null && this.m_OffsetEditMode != TimelineAnimationUtilities.OffsetEditMode.None)
			{
				TimelineAnimationUtilities.RigidTransform trackOffsets = TimelineAnimationUtilities.GetTrackOffsets(animationTrack, transform);
				EditorGUI.BeginChangeCheck();
				if (this.m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Translation)
				{
					trackOffsets.position = Handles.PositionHandle(trackOffsets.position, (Tools.pivotRotation != 1) ? trackOffsets.rotation : Quaternion.identity);
				}
				else if (this.m_OffsetEditMode == TimelineAnimationUtilities.OffsetEditMode.Rotation)
				{
					trackOffsets.rotation = Handles.RotationHandle(trackOffsets.rotation, trackOffsets.position);
				}
				if (EditorGUI.EndChangeCheck())
				{
					TimelineAnimationUtilities.UpdateTrackOffset(animationTrack, transform, trackOffsets);
					this.Evaluate();
					base.Repaint();
				}
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00014370 File Offset: 0x00012770
		public static void MatchTargetsField(SerializedProperty property, SerializedProperty alternate, SerializedProperty disableOptions, bool showHelp = false)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, AnimationTrackInspector.Styles.MatchTargetFieldsTitle);
			GUILayout.EndHorizontal();
			int num = 0;
			if (property.isExpanded)
			{
				if (showHelp)
				{
					string text = string.Format(AnimationTrackInspector.Styles.MatchTargetsFieldHelp.text, AnimationTrackInspector.Styles.DisableOptionsTitle.text);
					EditorGUILayout.HelpBox(text, 1);
				}
				EditorGUI.indentLevel++;
				bool flag = false;
				if (alternate != null)
				{
					EditorGUILayout.PropertyField(disableOptions, AnimationTrackInspector.Styles.DisableOptionsTitle, new GUILayoutOption[0]);
					flag = !disableOptions.boolValue;
					if (flag)
					{
						property = alternate;
					}
				}
				EditorGUI.DisabledScope disabledScope;
				disabledScope..ctor(flag);
				try
				{
					MatchTargetFields intValue = property.intValue;
					EditorGUI.BeginChangeCheck();
					Rect controlRect = EditorGUILayout.GetControlRect(false, 32f, new GUILayoutOption[0]);
					Rect rect;
					rect..ctor(controlRect.x, controlRect.y, controlRect.width, 16f);
					EditorGUI.BeginProperty(controlRect, AnimationTrackInspector.Styles.MatchTargetFieldsTitle, property);
					float num2 = 0f;
					float num3 = 0f;
					EditorStyles.label.CalcMinMaxWidth(AnimationTrackInspector.Styles.XTitle, ref num2, ref num3);
					float num4 = num2 + 20f;
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					Rect rect2 = EditorGUI.PrefixLabel(rect, AnimationTrackInspector.Styles.PositionTitle);
					int indentLevel = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
					rect2.width = num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.XTitle, MatchTargetFieldConstants.HasAny(intValue, 1))) ? 0 : 1);
					rect2.x += num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.YTitle, MatchTargetFieldConstants.HasAny(intValue, 2))) ? 0 : 2);
					rect2.x += num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.ZTitle, MatchTargetFieldConstants.HasAny(intValue, 4))) ? 0 : 4);
					EditorGUI.indentLevel = indentLevel;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					rect.y += 16f;
					rect2 = EditorGUI.PrefixLabel(rect, AnimationTrackInspector.Styles.RotationTitle);
					EditorGUI.indentLevel = 0;
					rect2.width = num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.XTitle, MatchTargetFieldConstants.HasAny(intValue, 8))) ? 0 : 8);
					rect2.x += num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.YTitle, MatchTargetFieldConstants.HasAny(intValue, 16))) ? 0 : 16);
					rect2.x += num4;
					num |= ((!EditorGUI.ToggleLeft(rect2, AnimationTrackInspector.Styles.ZTitle, MatchTargetFieldConstants.HasAny(intValue, 32))) ? 0 : 32);
					EditorGUI.indentLevel = indentLevel;
					GUILayout.EndHorizontal();
					if (EditorGUI.EndChangeCheck())
					{
						property.intValue = num;
					}
					EditorGUI.EndProperty();
				}
				finally
				{
					disabledScope.Dispose();
				}
				EditorGUI.indentLevel--;
			}
		}

		// Token: 0x040001B6 RID: 438
		private TimelineAnimationUtilities.OffsetEditMode m_OffsetEditMode = TimelineAnimationUtilities.OffsetEditMode.None;

		// Token: 0x040001B7 RID: 439
		private SerializedProperty m_MatchFieldsProperty;

		// Token: 0x040001B8 RID: 440
		private SerializedProperty m_TrackPositionProperty;

		// Token: 0x040001B9 RID: 441
		private SerializedProperty m_TrackRotationProperty;

		// Token: 0x040001BA RID: 442
		private SerializedProperty m_ApplyOffsetsProperty;

		// Token: 0x040001BB RID: 443
		private GameObject m_Binding;

		// Token: 0x040001BC RID: 444
		private Vector3 m_lastPosition;

		// Token: 0x040001BD RID: 445
		private Quaternion m_lastRotation;

		// Token: 0x0200003F RID: 63
		internal static class Styles
		{
			// Token: 0x040001BE RID: 446
			public static GUIContent MatchTargetFieldsTitle = EditorGUIUtility.TextContent("Clip Offset Match Fields|Specify which transform fields to match");

			// Token: 0x040001BF RID: 447
			public static readonly GUIContent PositionIcon = EditorGUIUtility.IconContent("MoveTool");

			// Token: 0x040001C0 RID: 448
			public static readonly GUIContent RotationIcon = EditorGUIUtility.IconContent("RotateTool");

			// Token: 0x040001C1 RID: 449
			public static GUIContent XTitle = EditorGUIUtility.TextContent("X");

			// Token: 0x040001C2 RID: 450
			public static GUIContent YTitle = EditorGUIUtility.TextContent("Y");

			// Token: 0x040001C3 RID: 451
			public static GUIContent ZTitle = EditorGUIUtility.TextContent("Z");

			// Token: 0x040001C4 RID: 452
			public static GUIContent PositionTitle = EditorGUIUtility.TextContent("Position");

			// Token: 0x040001C5 RID: 453
			public static GUIContent RotationTitle = EditorGUIUtility.TextContent("Rotation");

			// Token: 0x040001C6 RID: 454
			public static readonly GUIContent TrackOffsetTitle = EditorGUIUtility.TextContent("Apply Track Offsets|Root Motion Offsets values will be applied globally to all animation clips on the track that support root motion.\nThe offsets applied are in addition to any offsets on the clips.");

			// Token: 0x040001C7 RID: 455
			public static readonly GUIContent DisableOptionsTitle = EditorGUIUtility.TextContent("Override Track Matching Fields");

			// Token: 0x040001C8 RID: 456
			public static readonly GUIContent Blank = new GUIContent(" ");

			// Token: 0x040001C9 RID: 457
			public static readonly GUIContent MatchTargetsFieldHelp = EditorGUIUtility.TextContent("Use \"{0}\" to set different Matching Fields for a \u0003single clip on a track. Apply Matching through the clip's right-click contextual menu.");
		}
	}
}
