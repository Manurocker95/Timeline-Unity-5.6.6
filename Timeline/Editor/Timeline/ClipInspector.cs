using System;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000003 RID: 3
	[ExecuteInEditMode]
	[CustomEditor(typeof(EditorClip))]
	internal class ClipInspector : Editor
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000023CC File Offset: 0x000007CC
		private void InitializeProperties()
		{
			this.m_DisplayNameProperty = base.serializedObject.FindProperty("m_Clip.m_DisplayName");
			this.m_StartProperty = base.serializedObject.FindProperty("m_Clip.m_Start");
			this.m_DurationProperty = base.serializedObject.FindProperty("m_Clip.m_Duration");
			this.m_BlendInDurationProperty = base.serializedObject.FindProperty("m_Clip.m_BlendInDuration");
			this.m_BlendOutDurationProperty = base.serializedObject.FindProperty("m_Clip.m_BlendOutDuration");
			this.m_EaseInDurationProperty = base.serializedObject.FindProperty("m_Clip.m_EaseInDuration");
			this.m_EaseOutDurationProperty = base.serializedObject.FindProperty("m_Clip.m_EaseOutDuration");
			this.m_ClipInProperty = base.serializedObject.FindProperty("m_Clip.m_ClipIn");
			this.m_TimeScaleProperty = base.serializedObject.FindProperty("m_Clip.m_TimeScale");
			this.m_PostExtrapolationModeProperty = base.serializedObject.FindProperty("m_Clip.m_PostExtrapolationMode");
			this.m_PreExtrapolationModeProperty = base.serializedObject.FindProperty("m_Clip.m_PreExtrapolationMode");
			this.m_PostExtrapolationTimeProperty = base.serializedObject.FindProperty("m_Clip.m_PostExtrapolationTime");
			this.m_PreExtrapolationTimeProperty = base.serializedObject.FindProperty("m_Clip.m_PreExtrapolationTime");
			this.m_MixInCurveProperty = base.serializedObject.FindProperty("m_Clip.m_MixInCurve");
			this.m_MixOutCurveProperty = base.serializedObject.FindProperty("m_Clip.m_MixOutCurve");
			this.m_BlendInCurveModeProperty = base.serializedObject.FindProperty("m_Clip.m_BlendInCurveMode");
			this.m_BlendOutCurveModeProperty = base.serializedObject.FindProperty("m_Clip.m_BlendOutCurveMode");
			this.m_AssetProperty = base.serializedObject.FindProperty("m_Clip.m_Asset");
			if (this.m_AssetProperty != null && this.m_AssetProperty.objectReferenceValue != null)
			{
				this.m_PlayableAssetObject = new SerializedObject(this.m_AssetProperty.objectReferenceValue);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000025A0 File Offset: 0x000009A0
		private Editor GetInspector(Object asset)
		{
			try
			{
				if (asset == null)
				{
					return null;
				}
				Editor.CreateCachedEditorWithContext(asset, this.m_EditorClip.director, null, ref this.m_PlayableAssetInspector);
				return this.m_PlayableAssetInspector;
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002608 File Offset: 0x00000A08
		internal override void OnHeaderControlsGUI()
		{
			this.m_EditorClip = (base.target as EditorClip);
			if (this.m_EditorClip == null)
			{
				base.OnHeaderControlsGUI();
			}
			else
			{
				string text = "";
				TrackAsset parentTrack = this.m_EditorClip.clip.parentTrack;
				if (parentTrack != null)
				{
					text += parentTrack.name;
				}
				string text2 = "";
				if (this.m_EditorClip != null && this.m_EditorClip.timeline != null)
				{
					text2 = string.Concat(new string[]
					{
						this.m_EditorClip.timeline.name,
						" :: ",
						text,
						" :: ",
						this.m_EditorClip.clip.displayName
					});
				}
				GUILayout.Label(text2, new GUILayoutOption[0]);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000026F4 File Offset: 0x00000AF4
		internal override void DrawHeaderHelpAndSettingsGUI(Rect r)
		{
			Vector2 vector = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
			Object target = base.target;
			EditorGUI.HelpIconButton(new Rect(r.xMax - vector.x, r.y + 5f, vector.x, vector.y), target);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002750 File Offset: 0x00000B50
		internal override void OnHeaderIconGUI(Rect iconRect)
		{
			if (this.m_IconCache == null)
			{
				Texture2D miniThumbnail = AssetPreview.GetMiniThumbnail(this.m_EditorClip.clip.underlyingAsset);
				if (EditorGUIUtility.IsDefaultAssetIcon(miniThumbnail))
				{
					TimelineTrackBaseGUI timelineTrackBaseGUI = TimelineWindow.instance.allTracks.Find((TimelineTrackBaseGUI uiTrack) => uiTrack.track == this.m_EditorClip.track);
					if (timelineTrackBaseGUI != null && timelineTrackBaseGUI.drawer.GetIcon() != GUIContent.none)
					{
						this.m_IconCache = timelineTrackBaseGUI.drawer.GetIcon();
					}
				}
				if (this.m_IconCache == null)
				{
					this.m_IconCache = new GUIContent(miniThumbnail);
				}
			}
			GUI.Label(iconRect, this.m_IconCache);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000027FC File Offset: 0x00000BFC
		public override bool RequiresConstantRepaint()
		{
			return this.m_PreviewCurves != null;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002820 File Offset: 0x00000C20
		public void OnEnable()
		{
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Remove(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
			this.m_ClipCurveEditor = new ClipInspectorCurveEditor();
			this.m_EditorClip = (base.target as EditorClip);
			if (this.m_EditorClip != null && this.m_EditorClip.clip.asset != null)
			{
				this.m_SceneViewGUI = ClipSceneViewCache.GetViewForType(this.m_EditorClip.clip.asset.GetType());
				SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
			}
			this.InitializeProperties();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028DE File Offset: 0x00000CDE
		public void OnDisable()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000028E4 File Offset: 0x00000CE4
		private void DrawClipProperties()
		{
			if (this.m_TimelineWindow == null)
			{
				this.m_TimelineWindow = EditorWindow.GetWindow<TimelineWindow>();
			}
			double minValue = -TimelineClip.kMaxTimeValue;
			double kMaxTimeValue = TimelineClip.kMaxTimeValue;
			double minValue2 = 0.03333333333333333;
			if (this.m_TimelineWindow.state != null && this.m_TimelineWindow.state.frameRate > 1E-45f)
			{
				minValue2 = 1.0 / (double)this.m_TimelineWindow.state.frameRate;
			}
			this.m_EditorClip = (base.target as EditorClip);
			EditorGUI.DisabledScope disabledScope;
			disabledScope..ctor(this.m_EditorClip.clip.locked);
			try
			{
				bool flag = this.m_EditorClip.GetHashCode() != this.m_EditorClip.lastHash;
				bool flag2 = false;
				this.UnselectCurves();
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(this.m_DisplayNameProperty, ClipInspector.Styles.DisplayName, new GUILayoutOption[0]);
				EditorGUILayout.Space();
				GUILayout.Label(ClipInspector.Styles.ClipTimingTitle, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				TimelineInspectorUtility.TimeField(this.m_StartProperty, ClipInspector.Styles.StartName, false, this.m_TimelineWindow.state, minValue, kMaxTimeValue);
				this.EndTimePropertyField();
				TimelineInspectorUtility.TimeField(this.m_DurationProperty, ClipInspector.Styles.DurationName, false, this.m_TimelineWindow.state, minValue2, kMaxTimeValue);
				if (TimelineClipCapsExtensions.SupportsBlending(this.m_EditorClip.clip))
				{
					EditorGUILayout.Space();
					bool flag3 = base.serializedObject.isEditingMultipleObjects || this.m_EditorClip.clip.hasBlendIn;
					double maxValue = (!flag3) ? (this.m_DurationProperty.doubleValue * 0.49) : kMaxTimeValue;
					TimelineInspectorUtility.TimeField((!flag3) ? this.m_EaseInDurationProperty : this.m_BlendInDurationProperty, ClipInspector.Styles.EaseInDurationName, flag3, this.m_TimelineWindow.state, 0.0, maxValue);
					bool flag4 = base.serializedObject.isEditingMultipleObjects || this.m_EditorClip.clip.hasBlendOut;
					maxValue = ((!flag4) ? (this.m_DurationProperty.doubleValue * 0.49) : kMaxTimeValue);
					TimelineInspectorUtility.TimeField((!flag4) ? this.m_EaseOutDurationProperty : this.m_BlendOutDurationProperty, ClipInspector.Styles.EaseOutDurationName, flag4, this.m_TimelineWindow.state, 0.0, maxValue);
				}
				if (TimelineClipCapsExtensions.SupportsClipIn(this.m_EditorClip.clip))
				{
					EditorGUILayout.Space();
					TimelineInspectorUtility.TimeField(this.m_ClipInProperty, ClipInspector.Styles.ClipInName, false, this.m_TimelineWindow.state, 0.0, Math.Min(this.m_EditorClip.clip.clipAssetDuration, kMaxTimeValue));
				}
				EditorGUILayout.Space();
				if (TimelineClipCapsExtensions.SupportsSpeedMultiplier(this.m_EditorClip.clip))
				{
					this.TimeScalePropertyField();
				}
				EditorGUI.indentLevel--;
				if (EditorGUI.EndChangeCheck() || flag)
				{
					EditorUtility.SetDirty(base.target);
					this.m_TimelineWindow.Repaint();
					flag2 = true;
				}
				this.DrawExtrapolationOptions();
				if (TimelineClipCapsExtensions.SupportsBlending(this.m_EditorClip.clip))
				{
					this.DrawMixCurves();
				}
				this.m_ToInspect = this.m_EditorClip.clip.asset;
				this.m_EditorClip.asset = this.m_ToInspect;
				this.ClipAssetGui();
				if (flag2)
				{
					this.m_EditorClip.lastHash = this.m_EditorClip.GetHashCode();
				}
			}
			finally
			{
				disabledScope.Dispose();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002CAC File Offset: 0x000010AC
		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			this.DrawClipProperties();
			base.serializedObject.ApplyModifiedProperties();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002CCC File Offset: 0x000010CC
		private bool ShouldShowExtrapolation()
		{
			return base.targets.Cast<EditorClip>().All((EditorClip x) => TimelineClipCapsExtensions.SupportsExtrapolation(x.clip));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002D10 File Offset: 0x00001110
		private void TimeScalePropertyField()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = (enabled && !base.serializedObject.isEditingMultipleObjects);
			double num = Math.Max(TimelineClip.kTimeScaleMin, Math.Min(TimelineClip.kTimeScaleMax, this.m_TimeScaleProperty.doubleValue));
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.m_TimeScaleProperty, ClipInspector.Styles.TimeScaleName, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_TimeScaleProperty.doubleValue = Math.Max(TimelineClip.kTimeScaleMin, Math.Min(TimelineClip.kTimeScaleMax, this.m_TimeScaleProperty.doubleValue));
				this.m_DurationProperty.doubleValue = this.m_DurationProperty.doubleValue * num / this.m_TimeScaleProperty.doubleValue;
			}
			GUI.enabled = enabled;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002DDC File Offset: 0x000011DC
		private void EndTimePropertyField()
		{
			bool showMixed = this.m_StartProperty.hasMultipleDifferentValues || this.m_DurationProperty.hasMultipleDifferentValues;
			EditorGUI.BeginChangeCheck();
			double time = this.m_StartProperty.doubleValue + this.m_DurationProperty.doubleValue;
			double num = TimelineInspectorUtility.TimeField(this.m_TimelineWindow.state, ClipInspector.Styles.EndName, time, false, showMixed, -TimelineClip.kMaxTimeValue, TimelineClip.kMaxTimeValue * 2.0);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_DurationProperty.doubleValue = Math.Max(1E-06, num - this.m_StartProperty.doubleValue);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002E88 File Offset: 0x00001288
		protected void ClipAssetGui()
		{
			if (!(this.m_ToInspect == null) && !(this.m_ToInspect is AnimationClip) && this.m_AssetProperty != null)
			{
				this.m_AssetProperty.isExpanded = EditorGUILayout.InspectorTitlebar(this.m_AssetProperty.isExpanded, this.m_ToInspect);
				if (this.m_AssetProperty.isExpanded)
				{
					EditorGUILayout.Space();
					this.ShowPlayableAssetInspector();
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002F08 File Offset: 0x00001308
		private void DrawExtrapolationOptions()
		{
			if (this.ShouldShowExtrapolation())
			{
				EditorGUILayout.Space();
				GUILayout.Label(ClipInspector.Styles.AnimationExtrapolationTitle, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				EditorGUI.BeginChangeCheck();
				double doubleValue = this.m_PreExtrapolationTimeProperty.doubleValue;
				bool flag = doubleValue > 0.0;
				if (!flag)
				{
					EditorGUILayout.HelpBox(ClipInspector.Styles.PreExtrapolationHelpBoxText.text, 1);
				}
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_PreExtrapolationModeProperty, (!flag) ? ClipInspector.Styles.PreExtrapolateLabelIgnored : ClipInspector.Styles.PreExtrapolateLabel, new GUILayoutOption[]
				{
					GUILayout.MinWidth(75f)
				});
				EditorGUI.showMixedValue = this.m_PreExtrapolationTimeProperty.hasMultipleDifferentValues;
				EditorGUILayout.DoubleField(doubleValue, EditorStyles.label, new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_PostExtrapolationModeProperty, ClipInspector.Styles.PostExtrapolateLabel, new GUILayoutOption[]
				{
					GUILayout.MinWidth(75f)
				});
				EditorGUI.showMixedValue = this.m_PostExtrapolationTimeProperty.hasMultipleDifferentValues;
				EditorGUILayout.DoubleField(this.m_PostExtrapolationTimeProperty.doubleValue, EditorStyles.label, new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					if (this.m_TimelineWindow != null && this.m_TimelineWindow.state != null)
					{
						this.m_TimelineWindow.state.Refresh();
					}
				}
				EditorGUI.indentLevel--;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003094 File Offset: 0x00001494
		private void OnDestroy()
		{
			this.m_SceneViewGUI = null;
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Remove(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUI));
			Object.DestroyImmediate(this.m_PlayableAssetInspector);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000030CC File Offset: 0x000014CC
		private void OnSceneGUI(SceneView sceneView)
		{
			GameObject sceneReference = null;
			if (this.m_TimelineWindow != null && this.m_TimelineWindow.state != null)
			{
				sceneReference = this.m_TimelineWindow.state.GetSceneReference(this.m_EditorClip.clip.parentTrack);
			}
			if (this.m_SceneViewGUI != null && this.m_SceneViewGUI.OnSceneGUI(this.m_EditorClip.clip, sceneView, sceneReference))
			{
				this.m_TimelineWindow.state.Evaluate();
				base.Repaint();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003160 File Offset: 0x00001560
		public override GUIContent GetPreviewTitle()
		{
			return ClipInspector.Styles.PreviewTitle;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000317C File Offset: 0x0000157C
		public override bool HasPreviewGUI()
		{
			return this.m_PreviewCurves != null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000319D File Offset: 0x0000159D
		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
		{
			if (this.m_PreviewCurves != null && this.m_ClipCurveEditor != null)
			{
				this.SetCurveEditorTrackHead();
				this.m_ClipCurveEditor.OnGUI(r, this.m_CurvePresets);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000031D0 File Offset: 0x000015D0
		private void SetCurveEditorTrackHead()
		{
			if (!(this.m_EditorClip == null) && !(this.m_EditorClip.director == null))
			{
				double num = this.m_EditorClip.director.time;
				switch (this.m_CurveEditorSelection)
				{
				case ClipInspector.CurveEditorPreviewSelection.None:
				case ClipInspector.CurveEditorPreviewSelection.BlendCurves:
					num = ClipInspectorCurveEditor.kDisableTrackTime;
					break;
				case ClipInspector.CurveEditorPreviewSelection.AnimCurves:
				case ClipInspector.CurveEditorPreviewSelection.ScriptCurve:
					num = this.m_EditorClip.clip.ToLocalTime(num);
					break;
				}
				this.m_ClipCurveEditor.trackTime = num;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000326B File Offset: 0x0000166B
		private void UnselectCurves()
		{
			if (Event.current.type == null)
			{
				this.m_PreviewCurves = null;
				if (this.m_ClipCurveEditor != null)
				{
					this.m_ClipCurveEditor.SetUpdateCurveCallback(null);
				}
				this.m_CurveEditorSelection = ClipInspector.CurveEditorPreviewSelection.None;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000032A4 File Offset: 0x000016A4
		private void OnMixCurveSelected(string title, CurvePresetLibrary library, SerializedProperty curveSelected, bool easeIn)
		{
			this.m_CurveEditorSelection = ClipInspector.CurveEditorPreviewSelection.BlendCurves;
			this.m_CurvePresets = library;
			this.m_PreviewCurves = new AnimationCurve[]
			{
				curveSelected.animationCurveValue
			};
			this.m_ClipCurveEditor.headerString = title;
			this.m_ClipCurveEditor.SetCurves(this.m_PreviewCurves, null);
			this.m_ClipCurveEditor.SetSelected(curveSelected.animationCurveValue);
			if (easeIn)
			{
				this.m_ClipCurveEditor.SetUpdateCurveCallback(new Action<AnimationCurve, EditorCurveBinding>(this.MixInCurveUpdated));
			}
			else
			{
				this.m_ClipCurveEditor.SetUpdateCurveCallback(new Action<AnimationCurve, EditorCurveBinding>(this.MixOutCurveUpdated));
			}
			base.Repaint();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00003344 File Offset: 0x00001744
		private void MixInCurveUpdated(AnimationCurve curve, EditorCurveBinding binding)
		{
			curve.keys = ClipInspector.SanitizeCurveKeys(curve.keys, true);
			this.m_MixInCurveProperty.animationCurveValue = curve;
			base.serializedObject.ApplyModifiedProperties();
			this.m_EditorClip.lastHash = this.m_EditorClip.GetHashCode();
			this.RefreshCurves();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003398 File Offset: 0x00001798
		private void MixOutCurveUpdated(AnimationCurve curve, EditorCurveBinding binding)
		{
			curve.keys = ClipInspector.SanitizeCurveKeys(curve.keys, false);
			this.m_MixOutCurveProperty.animationCurveValue = curve;
			base.serializedObject.ApplyModifiedProperties();
			this.m_EditorClip.lastHash = this.m_EditorClip.GetHashCode();
			this.RefreshCurves();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000033EC File Offset: 0x000017EC
		private void DrawMixCurves()
		{
			EditorGUILayout.Space();
			GUILayout.Label(ClipInspector.Styles.BlendCurvesTitle, new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			EditorGUI.indentLevel++;
			this.DrawBlendCurve(ClipInspector.Styles.BlendInCurveName, this.m_BlendInCurveModeProperty, this.m_MixInCurveProperty, delegate(SerializedProperty x)
			{
				this.OnMixCurveSelected("Blend In", BuiltInPresets.blendInPresets, x, true);
			});
			this.DrawBlendCurve(ClipInspector.Styles.BlendOutCurveName, this.m_BlendOutCurveModeProperty, this.m_MixOutCurveProperty, delegate(SerializedProperty x)
			{
				this.OnMixCurveSelected("Blend Out", BuiltInPresets.blendOutPresets, x, false);
			});
			EditorGUI.indentLevel--;
			if (EditorGUI.EndChangeCheck())
			{
				this.m_TimelineWindow.Repaint();
			}
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003491 File Offset: 0x00001891
		private void RefreshCurves()
		{
			AnimationCurvePreviewCache.ClearCache();
			this.m_TimelineWindow.Repaint();
			base.Repaint();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000034AC File Offset: 0x000018AC
		private void DrawBlendCurve(GUIContent title, SerializedProperty modeProperty, SerializedProperty curveProperty, Action<SerializedProperty> onCurveClick)
		{
			bool enabled = GUI.enabled;
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(modeProperty, title, new GUILayoutOption[]
			{
				GUILayout.MinWidth(75f)
			});
			GUI.enabled = (!modeProperty.hasMultipleDifferentValues && modeProperty.intValue == 1);
			ClipInspector.CurveField(GUIContent.none, curveProperty, onCurveClick);
			EditorGUILayout.EndHorizontal();
			GUI.enabled = enabled;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000351C File Offset: 0x0000191C
		private static void CurveField(GUIContent title, SerializedProperty property, Action<SerializedProperty> onClick)
		{
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			EditorGUI.BeginProperty(controlRect, title, property);
			ClipInspector.DrawCurve(controlRect, property, onClick, EditorGUI.kCurveColor, EditorGUI.kCurveBGColor);
			EditorGUI.EndProperty();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003558 File Offset: 0x00001958
		private static Rect DrawCurve(Rect controlRect, SerializedProperty property, Action<SerializedProperty> onClick, Color fgColor, Color bgColor)
		{
			if (GUI.Button(controlRect, GUIContent.none))
			{
				if (onClick != null)
				{
					onClick(property);
				}
			}
			EditorGUIUtility.DrawCurveSwatch(controlRect, null, property, fgColor, bgColor);
			return controlRect;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003598 File Offset: 0x00001998
		private static Rect DrawCurve(Rect controlRect, AnimationCurve curve, Action<AnimationCurve> onClick, Color fgColor, Color bgColor)
		{
			if (GUI.Button(controlRect, GUIContent.none))
			{
				if (onClick != null)
				{
					onClick(curve);
				}
			}
			EditorGUIUtility.DrawCurveSwatch(controlRect, curve, null, fgColor, bgColor);
			return controlRect;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000035D8 File Offset: 0x000019D8
		private static Keyframe[] SanitizeCurveKeys(Keyframe[] keys, bool easeIn)
		{
			if (keys.Length < 2)
			{
				if (easeIn)
				{
					keys = new Keyframe[]
					{
						new Keyframe(0f, 0f),
						new Keyframe(1f, 1f)
					};
				}
				else
				{
					keys = new Keyframe[]
					{
						new Keyframe(0f, 1f),
						new Keyframe(1f, 0f)
					};
				}
			}
			else if (easeIn)
			{
				keys[0].time = 0f;
				keys[keys.Length - 1].time = 1f;
				keys[keys.Length - 1].value = 1f;
			}
			else
			{
				keys[0].time = 0f;
				keys[0].value = 1f;
				keys[keys.Length - 1].time = 1f;
			}
			return keys;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003708 File Offset: 0x00001B08
		private void LiveLink(Object target)
		{
			ITimelineClipLink timelineClipLink = target as ITimelineClipLink;
			if (timelineClipLink != null)
			{
				timelineClipLink.LiveLink();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000372C File Offset: 0x00001B2C
		private void ShowPlayableAssetInspector()
		{
			Editor inspector = this.GetInspector(this.m_ToInspect);
			if (inspector != null)
			{
				this.PreparePlayableAsset();
				EditorGUI.BeginChangeCheck();
				inspector.OnInspectorGUI();
				if (EditorGUI.EndChangeCheck())
				{
					if (this.m_TimelineWindow)
					{
						if (this.m_TimelineWindow.state != null)
						{
							this.LiveLink(this.m_ToInspect);
						}
						this.m_TimelineWindow.state.rebuildGraph = true;
						this.m_TimelineWindow.Repaint();
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000037C0 File Offset: 0x00001BC0
		private void PreparePlayableAsset()
		{
			if (Event.current.type == 7)
			{
				if (this.m_PlayableAssetObject != null)
				{
					if (!(this.m_EditorClip == null) && this.m_EditorClip.clip != null && !(this.m_EditorClip.clip.curves == null))
					{
						if (!(this.m_TimelineWindow == null) && this.m_TimelineWindow.state != null)
						{
							if (!this.m_TimelineWindow.state.previewMode)
							{
								this.m_LastEvalTime = -1.0;
							}
							else
							{
								TimelineClip clip = this.m_EditorClip.clip;
								double num = this.m_TimelineWindow.state.time;
								num = clip.ToLocalTime(num);
								if (this.m_LastEvalTime == num)
								{
									int version = AnimationClipCurveCache.Instance.GetCurveInfo(this.m_EditorClip.clip.curves).version;
									if (version == this.m_LastCurveVersion)
									{
										return;
									}
									this.m_LastCurveVersion = version;
								}
								this.m_LastEvalTime = num;
								AnimationClipCurveInfo curveInfo = AnimationClipCurveCache.Instance.GetCurveInfo(clip.curves);
								if (curveInfo.bindings.Length != 0)
								{
									this.m_PlayableAssetObject.Update();
									SerializedProperty iterator = this.m_PlayableAssetObject.GetIterator();
									while (iterator.NextVisible(true))
									{
										if (this.m_EditorClip.clip.IsParameterAnimated(iterator.propertyPath))
										{
											AnimationCurve animatedParameter = this.m_EditorClip.clip.GetAnimatedParameter(iterator.propertyPath);
											SerializedPropertyType propertyType = iterator.propertyType;
											if (propertyType != 1)
											{
												if (propertyType != 2)
												{
													if (propertyType == null)
													{
														iterator.intValue = Mathf.FloorToInt(animatedParameter.Evaluate((float)num));
													}
												}
												else
												{
													iterator.floatValue = animatedParameter.Evaluate((float)num);
												}
											}
											else
											{
												iterator.boolValue = (animatedParameter.Evaluate((float)num) > 0f);
											}
										}
									}
									this.m_PlayableAssetObject.ApplyModifiedPropertiesWithoutUndo();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private SerializedProperty m_DisplayNameProperty;

		// Token: 0x04000004 RID: 4
		private SerializedProperty m_StartProperty;

		// Token: 0x04000005 RID: 5
		private SerializedProperty m_DurationProperty;

		// Token: 0x04000006 RID: 6
		private SerializedProperty m_BlendInDurationProperty;

		// Token: 0x04000007 RID: 7
		private SerializedProperty m_BlendOutDurationProperty;

		// Token: 0x04000008 RID: 8
		private SerializedProperty m_EaseInDurationProperty;

		// Token: 0x04000009 RID: 9
		private SerializedProperty m_EaseOutDurationProperty;

		// Token: 0x0400000A RID: 10
		private SerializedProperty m_ClipInProperty;

		// Token: 0x0400000B RID: 11
		private SerializedProperty m_TimeScaleProperty;

		// Token: 0x0400000C RID: 12
		private SerializedProperty m_PostExtrapolationModeProperty;

		// Token: 0x0400000D RID: 13
		private SerializedProperty m_PreExtrapolationModeProperty;

		// Token: 0x0400000E RID: 14
		private SerializedProperty m_PostExtrapolationTimeProperty;

		// Token: 0x0400000F RID: 15
		private SerializedProperty m_PreExtrapolationTimeProperty;

		// Token: 0x04000010 RID: 16
		private SerializedProperty m_MixInCurveProperty;

		// Token: 0x04000011 RID: 17
		private SerializedProperty m_MixOutCurveProperty;

		// Token: 0x04000012 RID: 18
		private SerializedProperty m_BlendInCurveModeProperty;

		// Token: 0x04000013 RID: 19
		private SerializedProperty m_BlendOutCurveModeProperty;

		// Token: 0x04000014 RID: 20
		private SerializedProperty m_AssetProperty;

		// Token: 0x04000015 RID: 21
		private SerializedObject m_PlayableAssetObject;

		// Token: 0x04000016 RID: 22
		private EditorClip m_EditorClip;

		// Token: 0x04000017 RID: 23
		private Editor m_PlayableAssetInspector;

		// Token: 0x04000018 RID: 24
		private ClipInspectorCurveEditor m_ClipCurveEditor;

		// Token: 0x04000019 RID: 25
		private AnimationCurve[] m_PreviewCurves;

		// Token: 0x0400001A RID: 26
		private CurvePresetLibrary m_CurvePresets;

		// Token: 0x0400001B RID: 27
		private IClipSceneView m_SceneViewGUI;

		// Token: 0x0400001C RID: 28
		private ClipInspector.CurveEditorPreviewSelection m_CurveEditorSelection;

		// Token: 0x0400001D RID: 29
		private EditorCurveBinding[] m_AnimationModeCache;

		// Token: 0x0400001E RID: 30
		private GUIContent m_IconCache;

		// Token: 0x0400001F RID: 31
		private double m_LastEvalTime = -1.0;

		// Token: 0x04000020 RID: 32
		private int m_LastCurveVersion = -1;

		// Token: 0x04000021 RID: 33
		protected Object m_ToInspect;

		// Token: 0x04000022 RID: 34
		protected TimelineWindow m_TimelineWindow;

		// Token: 0x02000004 RID: 4
		private static class Styles
		{
			// Token: 0x04000024 RID: 36
			public static readonly GUIContent DisplayName = EditorGUIUtility.TextContent("Display Name");

			// Token: 0x04000025 RID: 37
			public static readonly GUIContent StartName = EditorGUIUtility.TextContent("Start|The start time of the clip");

			// Token: 0x04000026 RID: 38
			public static readonly GUIContent DurationName = EditorGUIUtility.TextContent("Duration|The length of the clip");

			// Token: 0x04000027 RID: 39
			public static readonly GUIContent EndName = EditorGUIUtility.TextContent("End|The end time of the clip");

			// Token: 0x04000028 RID: 40
			public static readonly GUIContent EaseInDurationName = EditorGUIUtility.TextContent("Ease In Duration|The length of the blend in");

			// Token: 0x04000029 RID: 41
			public static readonly GUIContent EaseOutDurationName = EditorGUIUtility.TextContent("Ease Out Duration|The length of the blend out");

			// Token: 0x0400002A RID: 42
			public static readonly GUIContent ClipInName = EditorGUIUtility.TextContent("Clip In|Start the clip at this local time");

			// Token: 0x0400002B RID: 43
			public static readonly GUIContent TimeScaleName = EditorGUIUtility.TextContent("Speed Multiplier|Time scale of the playback speed");

			// Token: 0x0400002C RID: 44
			public static readonly GUIContent PreExtrapolateLabel = EditorGUIUtility.TextContent("Pre-Extrapolate|Extrapolation used prior to the first clip");

			// Token: 0x0400002D RID: 45
			public static readonly GUIContent PreExtrapolateLabelIgnored = EditorGUIUtility.TextContent("Pre-Extrapolate (ignored)|Value presently ignored");

			// Token: 0x0400002E RID: 46
			public static readonly GUIContent PostExtrapolateLabel = EditorGUIUtility.TextContent("Post-Extrapolate|Extrapolation used after a clip ends");

			// Token: 0x0400002F RID: 47
			public static readonly GUIContent BlendInCurveName = EditorGUIUtility.TextContent("In|Blend In Curve");

			// Token: 0x04000030 RID: 48
			public static readonly GUIContent BlendOutCurveName = EditorGUIUtility.TextContent("Out|Blend Out Curve");

			// Token: 0x04000031 RID: 49
			public static readonly GUIContent PreviewTitle = EditorGUIUtility.TextContent("Curve Editor");

			// Token: 0x04000032 RID: 50
			public static readonly GUIContent ClipTimingTitle = EditorGUIUtility.TextContent("Clip Timing");

			// Token: 0x04000033 RID: 51
			public static readonly GUIContent AnimationExtrapolationTitle = EditorGUIUtility.TextContent("Animation Extrapolation");

			// Token: 0x04000034 RID: 52
			public static readonly GUIContent PreExtrapolationHelpBoxText = EditorGUIUtility.TextContent("Pre-Extrapolation is only applied to the first clip on a track, the selected clip during recording, or if the previous clip has no extrapolation");

			// Token: 0x04000035 RID: 53
			public static readonly GUIContent BlendCurvesTitle = EditorGUIUtility.TextContent("Blend Curves");

			// Token: 0x04000036 RID: 54
			public static readonly GUIContent EventAssetText = EditorGUIUtility.TextContent("Event Asset");

			// Token: 0x04000037 RID: 55
			public static readonly float k_minMaxToggleWidth = 13f;
		}

		// Token: 0x02000005 RID: 5
		private enum CurveEditorPreviewSelection
		{
			// Token: 0x04000039 RID: 57
			None,
			// Token: 0x0400003A RID: 58
			BlendCurves,
			// Token: 0x0400003B RID: 59
			AnimCurves,
			// Token: 0x0400003C RID: 60
			ScriptCurve
		}
	}
}
