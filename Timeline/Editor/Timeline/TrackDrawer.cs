using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200008F RID: 143
	internal class TrackDrawer : GUIDrawer
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0000BFE0 File Offset: 0x0000A3E0
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x0000BFFA File Offset: 0x0000A3FA
		internal ITimelineState sequencerState { get; set; }

		// Token: 0x0600056B RID: 1387 RVA: 0x0000C004 File Offset: 0x0000A404
		public static TrackDrawer CreateInstance(TrackAsset trackAsset)
		{
			TrackDrawer result;
			if (trackAsset == null)
			{
				result = Activator.CreateInstance<TrackDrawer>();
			}
			else
			{
				TrackDrawer trackDrawer = null;
				try
				{
					trackDrawer = (TrackDrawer)Activator.CreateInstance(TimelineHelpers.GetCustomDrawer(trackAsset.GetType()));
				}
				catch (Exception)
				{
					trackDrawer = Activator.CreateInstance<TrackDrawer>();
				}
				result = trackDrawer;
			}
			return result;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0000C070 File Offset: 0x0000A470
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0000C08B File Offset: 0x0000A48B
		public TrackAsset track
		{
			get
			{
				return this.m_Track;
			}
			set
			{
				this.m_Track = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0000C098 File Offset: 0x0000A498
		public virtual Color trackColor
		{
			get
			{
				if (!this.m_HasCheckForColorAttribute)
				{
					object[] customAttributes = this.track.GetType().GetCustomAttributes(typeof(TrackColorAttribute), true);
					if (customAttributes.Length > 0)
					{
						this.m_ColorAttribute = (customAttributes[0] as TrackColorAttribute);
					}
					this.m_HasCheckForColorAttribute = true;
				}
				Color result;
				if (this.m_ColorAttribute != null)
				{
					result = this.m_ColorAttribute.color;
				}
				else
				{
					result = DirectorStyles.Instance.customSkin.colorDefaultTrackDrawer;
				}
				return result;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000C124 File Offset: 0x0000A524
		public virtual Color GetTrackBackgroundColor(TrackAsset trackAsset)
		{
			return DirectorStyles.Instance.customSkin.colorTrackBackground;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0000C148 File Offset: 0x0000A548
		public virtual bool canDrawExtrapolationIcon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0000C160 File Offset: 0x0000A560
		private DirectorStyles styles
		{
			get
			{
				return DirectorStyles.Instance;
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000C17C File Offset: 0x0000A57C
		public virtual float GetHeight(TrackAsset t)
		{
			return this.DefaultTrackHeight;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000C198 File Offset: 0x0000A598
		public virtual GUIContent GetIcon()
		{
			if (!this.m_HasCheckForIconInGizmosFolder)
			{
				Texture2D texture2D = TrackDrawer.LoadIconInGizmosFolder(this.track.GetType().Name);
				if (texture2D != null)
				{
					this.m_IconGizmosContent = new GUIContent(texture2D);
				}
				this.m_HasCheckForIconInGizmosFolder = true;
			}
			GUIContent result;
			if (this.m_IconGizmosContent != null)
			{
				result = this.m_IconGizmosContent;
			}
			else
			{
				result = EditorGUIUtility.IconContent("ScriptableObject Icon");
			}
			return result;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000C210 File Offset: 0x0000A610
		private static Texture2D LoadIconInGizmosFolder(string filename)
		{
			string text = "Assets/Gizmos/" + filename + ".png";
			return AssetDatabase.LoadAssetAtPath<Texture2D>(text);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0000C23C File Offset: 0x0000A63C
		protected void AddCreateAssetMenuItem(GenericMenu menu, Type assetType, TrackAsset track, ITimelineState state)
		{
			if (!assetType.IsAbstract)
			{
				menu.AddItem(EditorGUIUtility.TextContent("Create " + ObjectNames.NicifyVariableName(assetType.Name) + " Clip"), false, delegate(object typeOfClip)
				{
					if (this.trackMenuContext.clipTimeCreation == TrackDrawer.TrackMenuContext.ClipTimeCreation.Mouse)
					{
						TimelineHelpers.CreateClipOnTrack(typeOfClip as Type, track, state, this.trackMenuContext.mousePosition);
					}
					else
					{
						TimelineHelpers.CreateClipOnTrack(typeOfClip as Type, track, state);
					}
					this.trackMenuContext.clipTimeCreation = TrackDrawer.TrackMenuContext.ClipTimeCreation.TimeCursor;
				}, assetType);
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0000C2AC File Offset: 0x0000A6AC
		protected void AddAddAssetMenuItem(GenericMenu menu, Type assetType, TrackAsset track, ITimelineState state)
		{
			if (!assetType.IsAbstract)
			{
				menu.AddItem(EditorGUIUtility.TextContent(this.sAddClipContent.text + " " + ObjectNames.NicifyVariableName(assetType.Name)), false, delegate(object typeOfClip)
				{
					this.AddAssetOnTrack(typeOfClip as Type, track, state);
				}, assetType);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0000C320 File Offset: 0x0000A720
		private void AddAssetOnTrack(Type typeOfClip, TrackAsset track, ITimelineState state)
		{
			state.ObjectPickerSelectionCallback = delegate(ITimelineState istate, Object obj)
			{
				state.ObjectPickerSelectionCallback = null;
				TimelineClip timelineClip = TimelineHelpers.CreateClipOnTrack(obj, track, state, TimelineHelpers.InvalidMousePosition);
				if (timelineClip != null && timelineClip.asset != null)
				{
					TimelineHelpers.SaveAssetIntoObject(timelineClip.asset, track);
				}
			};
			ObjectSelector.get.Show(null, typeOfClip, null, false);
			ObjectSelector.get.objectSelectorID = 0;
			ObjectSelector.get.searchFilter = "";
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0000C384 File Offset: 0x0000A784
		internal static string GetDisplayName(Type t)
		{
			string text = "";
			string str = ObjectNames.NicifyVariableName(t.Name);
			foreach (object obj in t.GetCustomAttributes(true))
			{
				if (obj is CategoryAttribute)
				{
					CategoryAttribute categoryAttribute = obj as CategoryAttribute;
					text = categoryAttribute.Category;
				}
				else if (obj is DisplayNameAttribute)
				{
					DisplayNameAttribute displayNameAttribute = obj as DisplayNameAttribute;
					str = displayNameAttribute.DisplayName;
				}
			}
			if (text.Length > 0 && text[text.Length - 1] != '/')
			{
				text += '/';
			}
			return text + str;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0000C44C File Offset: 0x0000A84C
		public virtual void OnBuildTrackContextMenu(GenericMenu menu, TrackAsset track, ITimelineState state)
		{
			bool flag = track is AnimationTrack || track is AudioTrack;
			if (flag)
			{
				List<Type> list = TimelineHelpers.GetTypesHandledByTrackType(TimelineHelpers.TrackTypeFromType(track.GetType())).ToList<Type>();
				for (int i = 0; i < list.Count; i++)
				{
					Type assetType = list[i];
					this.AddAddAssetMenuItem(menu, assetType, track, state);
				}
			}
			else if (TimelineHelpers.GetMediaTypeFromType(track.GetType()) == 3)
			{
				Type customPlayableType = track.GetCustomPlayableType();
				if (customPlayableType != null)
				{
					string displayName = TrackDrawer.GetDisplayName(customPlayableType);
					GUIContent guicontent = new GUIContent("Add " + displayName + " Clip");
					menu.AddItem(new GUIContent(guicontent), false, delegate(object userData)
					{
						TimelineHelpers.CreateClipOnTrack(userData as Type, track, state);
					}, customPlayableType);
				}
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0000C55D File Offset: 0x0000A95D
		public virtual void OnBuildClipContextMenu(GenericMenu menu, TimelineClip[] clips, ITimelineState state)
		{
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0000C560 File Offset: 0x0000A960
		public virtual bool DrawTrack(Rect trackRect, TrackAsset track, Vector2 visibleTime, ITimelineState state)
		{
			return false;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0000C578 File Offset: 0x0000A978
		protected void DrawClipErrorIcon(TrackDrawer.ClipDrawData clip, GUIContent content)
		{
			Rect targetRect = clip.targetRect;
			float num = Mathf.Min(targetRect.height - 4f, (float)content.image.height);
			Rect rect;
			rect..ctor(targetRect.xMax - 2f - (float)content.image.width, targetRect.y + (targetRect.height - num) * 0.5f, (float)content.image.width, num);
			GUI.Label(rect, content);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000C5FC File Offset: 0x0000A9FC
		private bool HasErrors(TrackDrawer.ClipDrawData drawData)
		{
			return drawData.clip.asset == null;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000C624 File Offset: 0x0000AA24
		private Rect DrawLoops(TrackDrawer.ClipDrawData drawData, Rect containerRect)
		{
			Rect result;
			if (!drawData.selected && !drawData.inlineCurvesSelected)
			{
				result = containerRect;
			}
			else
			{
				Color color = GUI.color;
				GUI.color = Color.white;
				for (int i = 1; i < drawData.uiClip.loopRects.Count; i++)
				{
					Rect rect = drawData.uiClip.loopRects[i];
					rect.x -= drawData.unclippedRect.x;
					rect.x += 1f;
					rect.width -= 2f;
					rect.y = 5f;
					rect.height -= 4f;
					if (!drawData.uiClip.supportsLooping)
					{
						rect.xMin += 2f;
						rect.xMax = drawData.targetRect.xMax;
						rect.width -= 4f;
					}
					GUI.color = new Color(0f, 0f, 0f, 0.2f);
					GUI.Box(rect, GUIContent.none, DirectorStyles.Instance.segmentCenter);
					if (rect.width > 30f)
					{
						GUI.color = Color.white;
						Graphics.ShadowLabel(rect, (!drawData.uiClip.supportsLooping) ? "Hold" : ("L" + i.ToString()), DirectorStyles.Instance.fontClip, Color.white, Color.black);
					}
					if (!drawData.uiClip.supportsLooping)
					{
						break;
					}
				}
				GUI.color = color;
				if (drawData.uiClip.loopRects.Count > 0)
				{
					float num = drawData.uiClip.loopRects.Sum((Rect x) => x.width) - drawData.uiClip.loopRects[0].width;
					result = new Rect(drawData.targetRect.position, new Vector2(drawData.targetRect.width - num, drawData.targetRect.height));
				}
				else
				{
					result = containerRect;
				}
			}
			return result;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000C8A0 File Offset: 0x0000ACA0
		private Rect DrawClipBody(TrackDrawer.ClipDrawData drawData, Color color)
		{
			DirectorStyles instance = DirectorStyles.Instance;
			Color colorClipHighlight = instance.customSkin.colorClipHighlight;
			Color colorClipShadow = instance.customSkin.colorClipShadow;
			if (drawData.selected)
			{
				color = TrackDrawer.GetHighlightColor(color);
			}
			using (new GUIColorOverride(color))
			{
				GUI.Box(drawData.clipCenterSection, GUIContent.none, instance.timelineClip);
			}
			Rect targetRect = drawData.targetRect;
			targetRect.yMin = targetRect.yMax - TrackDrawer.kClipColoredLineThickness;
			color = this.GetClipBaseColor(drawData.clip);
			EditorGUI.DrawRect(targetRect, color);
			float num = 2f;
			EditorGUI.DrawRect(new Rect(drawData.targetRect.xMin, drawData.targetRect.yMin, drawData.targetRect.width - num, num), colorClipHighlight);
			EditorGUI.DrawRect(new Rect(drawData.targetRect.xMin, drawData.targetRect.yMin + num, num, drawData.targetRect.height), colorClipHighlight);
			EditorGUI.DrawRect(new Rect(drawData.targetRect.xMax - num, drawData.targetRect.yMin, num, drawData.targetRect.height), colorClipShadow);
			EditorGUI.DrawRect(new Rect(drawData.targetRect.xMin, drawData.targetRect.yMax - num, drawData.targetRect.width, num), colorClipShadow);
			return drawData.clipCenterSection;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000CA40 File Offset: 0x0000AE40
		private void DrawCompoundClipBody(TrackDrawer.ClipDrawData drawData, Color color)
		{
			float num = 10f;
			float num2 = 7f;
			Rect clipCenterSection = drawData.clipCenterSection;
			clipCenterSection.x += num;
			clipCenterSection.y += num2;
			clipCenterSection.width -= 2f * num;
			clipCenterSection.height -= 2f * num2;
			using (new GUIColorOverride(color))
			{
				GUI.Box(clipCenterSection, GUIContent.none, DirectorStyles.Instance.timelineClip);
			}
			TrackDrawer.s_TitleContent.text = drawData.title;
			if (drawData.clip.asset is TimelineAsset)
			{
				TimelineAsset timelineAsset = drawData.clip.asset as TimelineAsset;
				if (timelineAsset.tracks.Count > 0)
				{
					if (timelineAsset.tracks[0].clips.Length == 1)
					{
						TrackDrawer.s_TitleContent.text = timelineAsset.tracks[0].clips[0].displayName;
					}
					else if (timelineAsset.tracks[0].clips.Length == 2)
					{
						TrackDrawer.s_TitleContent.text = timelineAsset.tracks[0].clips[0].displayName + " | " + timelineAsset.tracks[0].clips[1].displayName;
					}
					else
					{
						TrackDrawer.s_TitleContent.text = timelineAsset.tracks[0].clips[0].displayName + " | " + timelineAsset.tracks[0].clips[1].displayName + " | ...";
					}
				}
			}
			DirectorStyles instance = DirectorStyles.Instance;
			GUILayout.BeginArea(clipCenterSection);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(2f);
			GUILayout.FlexibleSpace();
			GUILayout.Box(GUIContent.none, instance.compound, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Space(2f);
			GUILayout.EndVertical();
			GUILayout.Space(5f);
			Rect rect = GUILayoutUtility.GetRect(TrackDrawer.s_TitleContent, instance.fontClip);
			rect.y += (clipCenterSection.height - instance.fontClip.CalcHeight(TrackDrawer.s_TitleContent, rect.width)) / 2f;
			this.DrawClipText(TrackDrawer.s_TitleContent.text, rect, 1);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000CD10 File Offset: 0x0000B110
		private void DrawClipRecorded(TrackDrawer.ClipDrawData drawData)
		{
			if (drawData.state.recording && drawData.clip.recordable && drawData.clip.parentTrack.IsRecordingToClip(drawData.clip))
			{
				using (new GUIColorOverride(DirectorStyles.Instance.customSkin.colorRecordingClipOutline))
				{
					GUI.Box(drawData.targetRect, GUIContent.none, DirectorStyles.Instance.outlineBorder);
				}
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000CDB8 File Offset: 0x0000B1B8
		private void DrawClipSelected(TrackDrawer.ClipDrawData drawData)
		{
			if (drawData.clip.selected)
			{
				Rect rect = drawData.clipCenterSection;
				EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, 2f), Color.white);
				EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - 2f, rect.width, 2f), Color.white);
				if (drawData.uiClip.mixInRect.width < 1f)
				{
					EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, 2f, rect.height), Color.white);
				}
				if (drawData.uiClip.mixOutRect.width < 1f)
				{
					EditorGUI.DrawRect(new Rect(rect.xMax - 2f, rect.yMin, 2f, rect.height), Color.white);
				}
				if (drawData.uiClip.blendInKind == TimelineClipGUI.BlendKind.Ease)
				{
					rect = drawData.uiClip.mixInRect;
					rect.position = Vector2.zero;
					EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - 2f, rect.width, 2f), Color.white);
				}
				if (drawData.uiClip.blendInKind == TimelineClipGUI.BlendKind.Mix)
				{
					rect = drawData.uiClip.mixInRect;
					rect.position = Vector2.zero;
					EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, 2f), Color.white);
					Graphics.DrawLineAA(4f, new Vector3(rect.xMin, rect.yMin, 0f), new Vector3(rect.xMax, rect.yMax - 1f, 0f), Color.white);
					if (drawData.uiClip.previousClip != null && drawData.uiClip.previousClip.selected)
					{
						EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - 2f, rect.width, 2f), Color.white);
						EditorGUI.DrawRect(new Rect(rect.xMax - 2f, rect.yMin, 2f, rect.height), Color.white);
						EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, 2f, rect.height), Color.white);
					}
				}
				if (drawData.uiClip.blendOutKind == TimelineClipGUI.BlendKind.Ease || drawData.uiClip.blendOutKind == TimelineClipGUI.BlendKind.Mix)
				{
					rect = drawData.uiClip.mixOutRect;
					rect.x = drawData.targetRect.xMax - rect.width;
					rect.y = 0f;
					EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - 2f, rect.width, 2f), Color.white);
					Graphics.DrawLineAA(4f, new Vector3(rect.xMin, rect.yMin, 0f), new Vector3(rect.xMax, rect.yMax - 1f, 0f), Color.white);
				}
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000D148 File Offset: 0x0000B548
		private static void DrawClipTimescale(TrackDrawer.ClipDrawData drawData)
		{
			if (drawData.clip.timeScale != 1.0)
			{
				float num = 4f;
				float num2 = 6f;
				float segmentsLength = (drawData.clip.timeScale <= 1.0) ? 15f : 5f;
				Vector3 vector;
				vector..ctor(drawData.targetRect.min.x + num, drawData.targetRect.min.y + num2, 0f);
				Vector3 vector2;
				vector2..ctor(drawData.targetRect.max.x - num, drawData.targetRect.min.y + num2, 0f);
				Graphics.DrawDottedLine(vector, vector2, segmentsLength, DirectorStyles.Instance.customSkin.colorClipFont);
				Graphics.DrawDottedLine(vector + new Vector3(0f, 1f, 0f), vector2 + new Vector3(0f, 1f, 0f), segmentsLength, DirectorStyles.Instance.customSkin.colorClipFont);
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000D280 File Offset: 0x0000B680
		private static void DrawClipInOut(TrackDrawer.ClipDrawData drawData)
		{
			if (drawData.clip.duration > drawData.clip.clipAssetDuration)
			{
				GUIStyle clipOut = DirectorStyles.Instance.clipOut;
				Rect targetRect = drawData.targetRect;
				targetRect.xMin = targetRect.xMax - clipOut.fixedWidth - 2f;
				targetRect.width = clipOut.fixedWidth;
				targetRect.yMin += (targetRect.height - clipOut.fixedHeight) / 2f;
				targetRect.height = clipOut.fixedHeight;
				GUI.Box(targetRect, GUIContent.none, clipOut);
			}
			if (drawData.clip.clipIn > 0.0)
			{
				GUIStyle clipIn = DirectorStyles.Instance.clipIn;
				Rect targetRect2 = drawData.targetRect;
				targetRect2.xMin = clipIn.fixedWidth;
				targetRect2.width = clipIn.fixedWidth;
				targetRect2.yMin += (targetRect2.height - clipIn.fixedHeight) / 2f;
				targetRect2.height = clipIn.fixedHeight;
				GUI.Box(targetRect2, GUIContent.none, clipIn);
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000D3AC File Offset: 0x0000B7AC
		private void DrawClipText(string text, Rect centerRect, TextAlignment alignment)
		{
			TrackDrawer.s_TitleContent.text = text;
			if (DirectorStyles.Instance.fontClip.CalcSize(TrackDrawer.s_TitleContent).x > centerRect.width)
			{
				TrackDrawer.s_TitleContent.text = DirectorStyles.Instance.Elipsify(TrackDrawer.s_TitleContent.text, centerRect, DirectorStyles.Instance.fontClip);
			}
			TextAnchor alignment2 = DirectorStyles.Instance.fontClip.alignment;
			if (alignment != null)
			{
				if (alignment != 2)
				{
					if (alignment == 1)
					{
						DirectorStyles.Instance.fontClip.alignment = 4;
					}
				}
				else
				{
					DirectorStyles.Instance.fontClip.alignment = 5;
				}
			}
			else
			{
				DirectorStyles.Instance.fontClip.alignment = 3;
			}
			Graphics.ShadowLabel(centerRect, TrackDrawer.s_TitleContent, DirectorStyles.Instance.fontClip, Color.white, Color.black);
			DirectorStyles.Instance.fontClip.alignment = alignment2;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000D4B0 File Offset: 0x0000B8B0
		public static Color GetHighlightColor(Color clipColor)
		{
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(clipColor, ref num, ref num2, ref num3);
			num3 *= 1.3f;
			return Color.HSVToRGB(num, num2, num3);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000D4E8 File Offset: 0x0000B8E8
		private void DrawDefaultClip(TrackDrawer.ClipDrawData drawData)
		{
			if (drawData.targetRect.width < TrackDrawer.kMinClipWidth)
			{
				drawData.targetRect.width = TrackDrawer.kMinClipWidth;
				drawData.clipCenterSection.width = TrackDrawer.kMinClipWidth;
				this.DrawClipBody(drawData, Color.white);
				this.DrawClipSelected(drawData);
				this.DrawClipText(drawData.title, drawData.targetRect, 1);
			}
			else if (drawData.clip.isNestedAsset)
			{
				this.DrawClipBody(drawData, DirectorStyles.Instance.customSkin.colorCompound);
				this.DrawCompoundClipBody(drawData, Color.white);
				this.DrawClipSelected(drawData);
				TrackDrawer.DrawClipTimescale(drawData);
				TrackDrawer.DrawClipInOut(drawData);
			}
			else
			{
				Rect containerRect = this.DrawClipBody(drawData, Color.white);
				this.DrawClipSelected(drawData);
				this.DrawClipRecorded(drawData);
				TrackDrawer.DrawClipTimescale(drawData);
				TrackDrawer.DrawClipInOut(drawData);
				if (containerRect.width < 20f)
				{
					if (drawData.targetRect.width > 20f)
					{
						this.DrawClipText(drawData.title, drawData.targetRect, 1);
					}
				}
				else
				{
					Rect centerRect = this.DrawLoops(drawData, containerRect);
					this.DrawClipText(drawData.title, centerRect, 1);
				}
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000D62C File Offset: 0x0000BA2C
		public virtual Color GetClipBaseColor(TimelineClip clip)
		{
			return this.trackColor;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000D648 File Offset: 0x0000BA48
		public Color GetClipColor(TrackDrawer.ClipDrawData drawData)
		{
			return (!drawData.selected && !drawData.inlineCurvesSelected) ? this.GetClipBaseColor(drawData.clip) : this.GetClipSelectedColor(drawData.clip);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000D694 File Offset: 0x0000BA94
		public Color GetClipColor(bool selected, TimelineClip clip)
		{
			return (!selected) ? this.GetClipBaseColor(clip) : this.GetClipSelectedColor(clip);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000D6C4 File Offset: 0x0000BAC4
		public Color GetClipSelectedColor(TimelineClip clip)
		{
			Color clipBaseColor = this.GetClipBaseColor(clip);
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(clipBaseColor, ref num, ref num2, ref num3);
			num3 *= 1.3f;
			return Color.HSVToRGB(num, num2, num3);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000D6FF File Offset: 0x0000BAFF
		public virtual void DrawClip(TrackDrawer.ClipDrawData drawData)
		{
			this.DrawDefaultClip(drawData);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000D70C File Offset: 0x0000BB0C
		public virtual string GetCustomTitle(TrackAsset track)
		{
			return string.Empty;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0000D726 File Offset: 0x0000BB26
		internal virtual void ConfigureUIClip(TimelineClipGUI uiClip)
		{
			uiClip.AddManipulator(new DragClip());
			uiClip.AddManipulator(new ClipContextMenu());
			uiClip.AddManipulator(new ClipActionsShortcutManipulator());
			uiClip.AddManipulator(new DrillIntoClip());
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0000D755 File Offset: 0x0000BB55
		internal virtual void ConfigureUITrack(TimelineTrackBaseGUI uiTrack)
		{
			uiTrack.ClearManipulators();
			uiTrack.AddManipulator(new SelectorTool());
			uiTrack.AddManipulator(new TrackContextMenuManipulator());
			uiTrack.AddManipulator(new TrackDoubleClick());
			uiTrack.AddManipulator(new TrackShortcutManipulator());
		}

		// Token: 0x04000355 RID: 853
		protected readonly GUIContent sAddClipContent = EditorGUIUtility.TextContent("Add From ");

		// Token: 0x04000356 RID: 854
		protected readonly GUIContent sCreateCipContent = EditorGUIUtility.TextContent("Create Clip/");

		// Token: 0x04000357 RID: 855
		private static GUIContent s_TitleContent = new GUIContent();

		// Token: 0x04000358 RID: 856
		private static float kMinClipWidth = 7f;

		// Token: 0x04000359 RID: 857
		private static readonly float kClipColoredLineThickness = 3f;

		// Token: 0x0400035A RID: 858
		public float DefaultTrackHeight = -1f;

		// Token: 0x0400035B RID: 859
		public TrackDrawer.TrackMenuContext trackMenuContext = new TrackDrawer.TrackMenuContext();

		// Token: 0x0400035D RID: 861
		private TrackColorAttribute m_ColorAttribute = null;

		// Token: 0x0400035E RID: 862
		private bool m_HasCheckForColorAttribute = false;

		// Token: 0x0400035F RID: 863
		private GUIContent m_IconGizmosContent = null;

		// Token: 0x04000360 RID: 864
		private bool m_HasCheckForIconInGizmosFolder = false;

		// Token: 0x04000361 RID: 865
		private TrackAsset m_Track = null;

		// Token: 0x02000090 RID: 144
		public struct ClipDrawData
		{
			// Token: 0x04000363 RID: 867
			public TimelineClip clip;

			// Token: 0x04000364 RID: 868
			public Rect targetRect;

			// Token: 0x04000365 RID: 869
			public Rect unclippedRect;

			// Token: 0x04000366 RID: 870
			public Rect clipCenterSection;

			// Token: 0x04000367 RID: 871
			public string title;

			// Token: 0x04000368 RID: 872
			public bool selected;

			// Token: 0x04000369 RID: 873
			public bool inlineCurvesSelected;

			// Token: 0x0400036A RID: 874
			public GUIStyle style;

			// Token: 0x0400036B RID: 875
			public ITimelineState state;

			// Token: 0x0400036C RID: 876
			public Vector2 visibleTime;

			// Token: 0x0400036D RID: 877
			public GUIStyle selectedStyle;

			// Token: 0x0400036E RID: 878
			internal TimelineClipGUI uiClip;
		}

		// Token: 0x02000091 RID: 145
		public class TrackMenuContext
		{
			// Token: 0x0400036F RID: 879
			public TrackDrawer.TrackMenuContext.ClipTimeCreation clipTimeCreation = TrackDrawer.TrackMenuContext.ClipTimeCreation.TimeCursor;

			// Token: 0x04000370 RID: 880
			public Vector2 mousePosition = Vector2.zero;

			// Token: 0x02000092 RID: 146
			public enum ClipTimeCreation
			{
				// Token: 0x04000372 RID: 882
				TimeCursor,
				// Token: 0x04000373 RID: 883
				Mouse
			}
		}
	}
}
