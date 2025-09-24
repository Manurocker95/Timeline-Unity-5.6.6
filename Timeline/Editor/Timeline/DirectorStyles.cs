using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200000A RID: 10
	internal class DirectorStyles
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00006490 File Offset: 0x00004890
		private DirectorStyles()
		{
			this.background = this.GetStyle("OL Box");
			this.soloToggle = this.GetStyle("SoloToggle");
			this.header = this.GetStyle("OL title");
			this.label = this.GetStyle("OL label");
			this.entryEven = this.GetStyle("OL EntryBackEven");
			this.entryOdd = this.GetStyle("OL EntryBackOdd");
			this.profilerGraphBackground = this.GetStyle("ProfilerScrollviewBackground");
			this.handLeft = this.GetStyle("MeTransitionHandleLeft");
			this.handRight = this.GetStyle("MeTransitionHandleRight");
			this.groupBackground = this.GetStyle("groupBackground");
			this.timecode = this.GetStyle("timecodeTextField");
			this.timecodeSmall = this.GetStyle("timecodeTextFieldSmall");
			this.settingsButton = this.GetStyle("settingsButton");
			this.addButton = this.GetStyle("Icon.Add");
			this.sequenceClip = this.GetStyle("sequenceClip");
			this.fontClip = this.GetStyle("Font.Clip");
			this.fontClipTiny = this.GetStyle("Font.ClipTiny");
			this.sequenceClipSmall = this.GetStyle("sequenceClipSmall");
			this.sequenceTrackHeader = this.GetStyle("sequenceTrackHeader");
			this.sequenceTrackHeaderAudio = this.GetStyle("sequenceTrackHeaderAudio");
			this.sequenceTrackHeaderVideo = this.GetStyle("sequenceTrackHeaderVideo");
			this.sequenceTrackHeaderScript = this.GetStyle("sequenceTrackHeaderScript");
			this.sequenceTrackHeaderFont = this.GetStyle("sequenceTrackHeaderFont");
			this.sequenceTrackLocked = this.GetStyle("sequenceTrackLocked");
			this.sequenceTrack = this.GetStyle("sequenceTrack");
			this.sequenceGroup = this.GetStyle("sequenceGroup");
			this.sequenceGroupBackground = this.GetStyle("groupBackground");
			this.sequenceOutput = this.GetStyle("sequenceOutput");
			this.sequenceGroupFont = this.GetStyle("sequenceGroupFont");
			this.sequenceClipHandle = this.GetStyle("sequenceClipHandle");
			this.trackArmedRecord = this.GetStyle("sequenceTrackArmed");
			this.sequenceClipTrimmed = this.GetStyle("sequenceClipTrimmed");
			this.sequenceClipTrimmedLeft = this.GetStyle("sequenceClipTrimmedLeft");
			this.timeCursor = this.GetStyle("Icon.TimeCursor");
			this.endmarker = this.GetStyle("Icon.Endmarker");
			this.tinyFont = this.GetStyle("tinyFont");
			this.largeFont = this.GetStyle("largeFont");
			this.mediumFont = this.GetStyle("mediumFont");
			this.regularFont12Pts = this.GetStyle("Font.Regular12Pts");
			this.IconFx = this.GetStyle("Icon.Fx");
			this.IconPrefab = this.GetStyle("Icon.Prefab");
			this.foldout = this.GetStyle("Icon.Foldout");
			this.mute = this.GetStyle("Icon.Mute");
			this.locked = this.GetStyle("Icon.Locked");
			this.autoKey = this.GetStyle("Icon.AutoKey");
			this.curveMode = this.GetStyle("Icon.Fcurve");
			this.customObjectFieldWidget = this.GetStyle("customObjectFieldWidget");
			this.customObjectField = this.GetStyle("customObjectField");
			this.convertToClipMode.image = this.GetStyle("closedclip").normal.background;
			this.convertFromClipMode.image = this.GetStyle("openclip").normal.background;
			this.infiniteClip = this.GetStyle("infiniteClip");
			this.newActorStyle = this.GetStyle("newActor");
			this.playSequence = this.GetStyle("Icon.Play");
			this.gotoBeginSeq = this.GetStyle("Icon.GotoBeginingSeq");
			this.gotoEndSeq = this.GetStyle("Icon.GotoEndSeq");
			this.prevFrame = this.GetStyle("Icon.PrevFrame");
			this.nextFrame = this.GetStyle("Icon.NextFrame");
			this.breadCrumbMidFont = this.GetStyle("breadCrumbMidFont");
			this.breadCrumbIconStyle = this.GetStyle("breadCrumbIcon");
			this.breadCrumbTailStyle = this.GetStyle("breadCrumbTail");
			this.breadCrumbMidStyle = this.GetStyle("breadCrumbMid");
			this.breadCrumbTipStyle = this.GetStyle("breadCrumbTip");
			this.instanceTargetStyle = this.GetStyle("instanceTarget");
			this.playTimeRangeStart = this.GetStyle("Icon.PlayAreaStart");
			this.playTimeRangeEnd = this.GetStyle("Icon.PlayAreaEnd");
			this.optionsStyle = this.GetStyle("Icon.Options");
			this.selectedStyle = this.GetStyle("Color.Selected");
			this.trackSwatchStyle = this.GetStyle("Icon.TrackHeaderSwatch");
			this.connector = this.GetStyle("Icon.Connector");
			this.keyframeStyle = this.GetStyle("Icon.Keyframe");
			this.warningStyle = this.GetStyle("Icon.Warning");
			this.extrapolationHold = this.GetStyle("Icon.ExtrapolationHold");
			this.extrapolationLoop = this.GetStyle("Icon.ExtrapolationLoop");
			this.extrapolationPingPong = this.GetStyle("Icon.ExtrapolationPingPong");
			this.extrapolationContinue = this.GetStyle("Icon.ExtrapolationContinue");
			this.eventTrackIcon = this.GetStyle("Icon.EventTrack");
			this.IconEvent = this.GetStyle("Icon.Event");
			this.eventWhite = this.GetStyle("Icon.EventWhite");
			this.eventIconSelected = this.GetStyle("Icon.EventSelected");
			this.segmentBegin = this.GetStyle("Icon.SegmentBegin");
			this.segmentCenter = this.GetStyle("Icon.SegmentCenter");
			this.segmentEnd = this.GetStyle("Icon.SegmentEnd");
			this.segmentOutlineLeftEdge = this.GetStyle("Icon.SegmentOutlineLeftEdge");
			this.segmentOutlineCenter = this.GetStyle("Icon.SegmentOutlineCenter");
			this.segmentOutlineRightEdge = this.GetStyle("Icon.SegmentOutlineRightEdge");
			this.sequenceClipBackground = this.GetStyle("Icon.SequenceClip");
			this.sequenceClipBackgroundBoth = this.GetStyle("Icon.SequenceClip.Both");
			this.sequenceClipBackgroundLeft = this.GetStyle("Icon.SequenceClip.Left");
			this.sequenceClipBackgroundRight = this.GetStyle("Icon.SequenceClip.Right");
			this.sequenceClipKeyFrameIcon = new GUIContent(this.keyframeStyle.normal.background);
			this.indent = this.GetStyle("sequencerTreeViewIndent");
			this.outlineBorder = this.GetStyle("Icon.OutlineBorder");
			this.sequenceSelector = new GUIStyle(EditorStyles.toolbarButton)
			{
				fixedHeight = TimelineWindow.kBreadcrumbHeight
			};
			this.timelineClip = this.GetStyle("Icon.Clip");
			this.bottomShadow = this.GetStyle("Icon.Shadow");
			this.trackOptions = this.GetStyle("Icon.TrackOptions");
			this.infiniteTrack = this.GetStyle("Icon.InfiniteTrack");
			this.blendingIn = this.GetStyle("Icon.BlendingIn");
			this.blendingOut = this.GetStyle("Icon.BlendingOut");
			this.clipOut = this.GetStyle("Icon.ClipOut");
			this.clipIn = this.GetStyle("Icon.ClipIn");
			this.compound = this.GetStyle("Icon.Compound");
			this.curves = this.GetStyle("Icon.Curves");
			this.lockedBG = this.GetStyle("Icon.LockedBG");
			this.activation = this.GetStyle("Icon.Activation");
			this.playrange = this.GetStyle("Icon.Playrange");
			this.lockButton = this.GetStyle("IN LockButton");
			this.playrangeContent = new GUIContent(this.playrange.normal.background);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00006D6C File Offset: 0x0000516C
		public static DirectorStyles Instance
		{
			get
			{
				if (DirectorStyles.ms_Instance == null)
				{
					DirectorStyles.ms_Instance = new DirectorStyles();
					DirectorStyles.ms_Instance.Initialize();
				}
				return DirectorStyles.ms_Instance;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00006DA8 File Offset: 0x000051A8
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00006DD8 File Offset: 0x000051D8
		public DirectorNamedColor customSkin
		{
			get
			{
				return (!EditorGUIUtility.isProSkin) ? this.m_LightSkinColors : this.m_DarkSkinColors;
			}
			internal set
			{
				if (EditorGUIUtility.isProSkin)
				{
					this.m_DarkSkinColors = value;
				}
				else
				{
					this.m_LightSkinColors = value;
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006DF8 File Offset: 0x000051F8
		private DirectorNamedColor LoadColorSkin(string path)
		{
			TextAsset textAsset = EditorGUIUtility.LoadRequired(path) as TextAsset;
			DirectorNamedColor result;
			if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
			{
				result = DirectorNamedColor.CreateAndLoadFromText(textAsset.text);
			}
			else
			{
				result = this.m_DefaultSkinColors;
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006E50 File Offset: 0x00005250
		private DirectorNamedColor CreateDefaultSkin()
		{
			DirectorNamedColor directorNamedColor = ScriptableObject.CreateInstance<DirectorNamedColor>();
			directorNamedColor.SetDefault();
			return directorNamedColor;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006E74 File Offset: 0x00005274
		public void ExportSkinToFile()
		{
			if (this.customSkin == this.m_DarkSkinColors)
			{
				this.customSkin.ToText(DirectorStyles.s_DarkSkinPath);
			}
			if (this.customSkin == this.m_LightSkinColors)
			{
				this.customSkin.ToText(DirectorStyles.s_LightSkinPath);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006ED0 File Offset: 0x000052D0
		public void ReloadSkin()
		{
			if (this.customSkin == this.m_DarkSkinColors)
			{
				this.m_DarkSkinColors = this.LoadColorSkin(DirectorStyles.s_DarkSkinPath);
			}
			else if (this.customSkin == this.m_LightSkinColors)
			{
				this.m_LightSkinColors = this.LoadColorSkin(DirectorStyles.s_LightSkinPath);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00006F38 File Offset: 0x00005338
		public string cutomSkinContext
		{
			get
			{
				string result;
				if (this.customSkin == this.m_DarkSkinColors)
				{
					result = "Dark Skin";
				}
				else if (this.customSkin == this.m_LightSkinColors)
				{
					result = "Light Skin";
				}
				else
				{
					result = "Default";
				}
				return result;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00006F94 File Offset: 0x00005394
		public void Initialize()
		{
			this.m_DefaultSkinColors = this.CreateDefaultSkin();
			this.m_DarkSkinColors = this.LoadColorSkin(DirectorStyles.s_DarkSkinPath);
			this.m_LightSkinColors = this.LoadColorSkin(DirectorStyles.s_LightSkinPath);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00006FC8 File Offset: 0x000053C8
		public GUIStyle GetStyle(string s)
		{
			return new GUIStyle(s);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00006FE8 File Offset: 0x000053E8
		public float indentWidth
		{
			get
			{
				return this.indent.fixedWidth;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00007008 File Offset: 0x00005408
		public string Elipsify(string label, Rect rect, GUIStyle style)
		{
			string result;
			if (label.Length == 0)
			{
				result = label;
			}
			else
			{
				this.m_TempContent.text = label;
				float num = style.CalcSize(this.m_TempContent).x - rect.width;
				if (num > 0f)
				{
					float num2 = style.CalcSize(this.m_TempContent).x / (float)label.Length;
					float num3 = rect.width / num2;
					if (num3 - 3f > 0f)
					{
						label = label.Substring(0, (int)num3 - 3);
						label += "...";
					}
					else
					{
						int num4 = (int)Mathf.Floor(num3);
						if (num4 <= 0)
						{
							num4 = 1;
						}
						label = label.Substring(0, num4) + "...";
					}
				}
				result = label;
			}
			return result;
		}

		// Token: 0x04000087 RID: 135
		private DirectorNamedColor m_DarkSkinColors;

		// Token: 0x04000088 RID: 136
		private DirectorNamedColor m_LightSkinColors;

		// Token: 0x04000089 RID: 137
		private DirectorNamedColor m_DefaultSkinColors;

		// Token: 0x0400008A RID: 138
		private static readonly string s_DarkSkinPath = "Editors/Storyteller/Storyteller_DarkSkin.txt";

		// Token: 0x0400008B RID: 139
		private static readonly string s_LightSkinPath = "Editors/Storyteller/Storyteller_LightSkin.txt";

		// Token: 0x0400008C RID: 140
		public readonly GUIContent textContentEditWithAnimationEditor = EditorGUIUtility.TextContent("Edit With Animation Editor");

		// Token: 0x0400008D RID: 141
		public readonly GUIContent recordContent = EditorGUIUtility.IconContent("Animation.Record");

		// Token: 0x0400008E RID: 142
		public readonly GUIContent addIcon = EditorGUIUtility.IconContent("Toolbar Plus");

		// Token: 0x0400008F RID: 143
		public readonly GUIContent addFCurve = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel"), "Add FCurve");

		// Token: 0x04000090 RID: 144
		public readonly GUIContent sequenceClipKeyFrameIcon;

		// Token: 0x04000091 RID: 145
		public readonly GUIContent soloContent = EditorGUIUtility.TextContent("Solo this track");

		// Token: 0x04000092 RID: 146
		public readonly GUIContent muteContent = EditorGUIUtility.TextContent("Mute this track");

		// Token: 0x04000093 RID: 147
		public readonly GUIContent convertToClipMode = EditorGUIUtility.TextContent("Convert to closed clips");

		// Token: 0x04000094 RID: 148
		public readonly GUIContent convertFromClipMode = EditorGUIUtility.TextContent("Convert to open clip");

		// Token: 0x04000095 RID: 149
		public readonly GUIContent referenceTrackLabel = EditorGUIUtility.TextContent("R|This track references an external asset");

		// Token: 0x04000096 RID: 150
		public readonly GUIContent recordingLabel = EditorGUIUtility.TextContent("Recording...");

		// Token: 0x04000097 RID: 151
		public readonly GUIContent sequenceSelectorIcon = EditorGUIUtility.IconContent("DirectorSelector");

		// Token: 0x04000098 RID: 152
		public readonly GUIContent playrangeContent = null;

		// Token: 0x04000099 RID: 153
		public GUIStyle groupBackground;

		// Token: 0x0400009A RID: 154
		public GUIStyle timecode;

		// Token: 0x0400009B RID: 155
		public GUIStyle timecodeSmall;

		// Token: 0x0400009C RID: 156
		public GUIStyle settingsButton;

		// Token: 0x0400009D RID: 157
		public GUIStyle addButton;

		// Token: 0x0400009E RID: 158
		public GUIStyle mute;

		// Token: 0x0400009F RID: 159
		public GUIStyle background = "OL Box";

		// Token: 0x040000A0 RID: 160
		public GUIStyle soloToggle = "SoloToggle";

		// Token: 0x040000A1 RID: 161
		public GUIStyle header = "OL title";

		// Token: 0x040000A2 RID: 162
		public GUIStyle label = "OL label";

		// Token: 0x040000A3 RID: 163
		public GUIStyle entryEven = "OL EntryBackEven";

		// Token: 0x040000A4 RID: 164
		public GUIStyle entryOdd = "OL EntryBackOdd";

		// Token: 0x040000A5 RID: 165
		public GUIStyle profilerGraphBackground = "ProfilerScrollviewBackground";

		// Token: 0x040000A6 RID: 166
		public GUIStyle handLeft = "MeTransitionHandleLeft";

		// Token: 0x040000A7 RID: 167
		public GUIStyle handRight = "MeTransitionHandleRight";

		// Token: 0x040000A8 RID: 168
		public GUIStyle foldout;

		// Token: 0x040000A9 RID: 169
		public GUIStyle locked;

		// Token: 0x040000AA RID: 170
		public GUIStyle autoKey;

		// Token: 0x040000AB RID: 171
		public GUIStyle timeCursor;

		// Token: 0x040000AC RID: 172
		public GUIStyle playTimeRangeEnd;

		// Token: 0x040000AD RID: 173
		public GUIStyle playTimeRangeStart;

		// Token: 0x040000AE RID: 174
		public GUIStyle endmarker;

		// Token: 0x040000AF RID: 175
		public GUIStyle sequenceClip;

		// Token: 0x040000B0 RID: 176
		public GUIStyle fontClip;

		// Token: 0x040000B1 RID: 177
		public GUIStyle fontClipTiny;

		// Token: 0x040000B2 RID: 178
		public GUIStyle sequenceClipSmall;

		// Token: 0x040000B3 RID: 179
		public GUIStyle sequenceTrackHeader;

		// Token: 0x040000B4 RID: 180
		public GUIStyle sequenceTrackHeaderAudio;

		// Token: 0x040000B5 RID: 181
		public GUIStyle sequenceTrackHeaderVideo;

		// Token: 0x040000B6 RID: 182
		public GUIStyle sequenceTrackHeaderScript;

		// Token: 0x040000B7 RID: 183
		public GUIStyle sequenceTrackHeaderFont;

		// Token: 0x040000B8 RID: 184
		public GUIStyle sequenceTrackLocked;

		// Token: 0x040000B9 RID: 185
		public GUIStyle sequenceTrack;

		// Token: 0x040000BA RID: 186
		public GUIStyle sequenceGroup;

		// Token: 0x040000BB RID: 187
		public GUIStyle sequenceGroupBackground;

		// Token: 0x040000BC RID: 188
		public GUIStyle sequenceOutput;

		// Token: 0x040000BD RID: 189
		public GUIStyle sequenceGroupFont;

		// Token: 0x040000BE RID: 190
		public GUIStyle sequenceClipHandle;

		// Token: 0x040000BF RID: 191
		public GUIStyle trackArmedRecord;

		// Token: 0x040000C0 RID: 192
		public GUIStyle sequenceClipTrimmed;

		// Token: 0x040000C1 RID: 193
		public GUIStyle sequenceClipTrimmedLeft;

		// Token: 0x040000C2 RID: 194
		public GUIStyle customObjectFieldWidget;

		// Token: 0x040000C3 RID: 195
		public GUIStyle customObjectField;

		// Token: 0x040000C4 RID: 196
		public GUIStyle tinyFont;

		// Token: 0x040000C5 RID: 197
		public GUIStyle mediumFont;

		// Token: 0x040000C6 RID: 198
		public GUIStyle regularFont12Pts;

		// Token: 0x040000C7 RID: 199
		public GUIStyle IconFx;

		// Token: 0x040000C8 RID: 200
		public GUIStyle IconPrefab;

		// Token: 0x040000C9 RID: 201
		public GUIStyle largeFont;

		// Token: 0x040000CA RID: 202
		public GUIStyle playSequence;

		// Token: 0x040000CB RID: 203
		public GUIStyle gotoBeginSeq;

		// Token: 0x040000CC RID: 204
		public GUIStyle gotoEndSeq;

		// Token: 0x040000CD RID: 205
		public GUIStyle prevFrame;

		// Token: 0x040000CE RID: 206
		public GUIStyle nextFrame;

		// Token: 0x040000CF RID: 207
		public GUIStyle newActorStyle;

		// Token: 0x040000D0 RID: 208
		public GUIStyle infiniteClip;

		// Token: 0x040000D1 RID: 209
		public GUIStyle breadCrumbMidFont;

		// Token: 0x040000D2 RID: 210
		public GUIStyle breadCrumbIconStyle;

		// Token: 0x040000D3 RID: 211
		public GUIStyle breadCrumbTailStyle;

		// Token: 0x040000D4 RID: 212
		public GUIStyle breadCrumbMidStyle;

		// Token: 0x040000D5 RID: 213
		public GUIStyle breadCrumbTipStyle;

		// Token: 0x040000D6 RID: 214
		public GUIStyle instanceTargetStyle;

		// Token: 0x040000D7 RID: 215
		public GUIStyle optionsStyle;

		// Token: 0x040000D8 RID: 216
		public GUIStyle selectedStyle;

		// Token: 0x040000D9 RID: 217
		public GUIStyle trackSwatchStyle;

		// Token: 0x040000DA RID: 218
		public GUIStyle connector;

		// Token: 0x040000DB RID: 219
		public GUIStyle keyframeStyle;

		// Token: 0x040000DC RID: 220
		public GUIStyle warningStyle;

		// Token: 0x040000DD RID: 221
		public GUIStyle extrapolationHold;

		// Token: 0x040000DE RID: 222
		public GUIStyle extrapolationLoop;

		// Token: 0x040000DF RID: 223
		public GUIStyle extrapolationPingPong;

		// Token: 0x040000E0 RID: 224
		public GUIStyle extrapolationContinue;

		// Token: 0x040000E1 RID: 225
		public GUIStyle eventTrackIcon;

		// Token: 0x040000E2 RID: 226
		public GUIStyle IconEvent;

		// Token: 0x040000E3 RID: 227
		public GUIStyle eventWhite;

		// Token: 0x040000E4 RID: 228
		public GUIStyle eventIconSelected;

		// Token: 0x040000E5 RID: 229
		public GUIStyle segmentBegin;

		// Token: 0x040000E6 RID: 230
		public GUIStyle segmentCenter;

		// Token: 0x040000E7 RID: 231
		public GUIStyle segmentEnd;

		// Token: 0x040000E8 RID: 232
		public GUIStyle segmentOutlineLeftEdge;

		// Token: 0x040000E9 RID: 233
		public GUIStyle segmentOutlineCenter;

		// Token: 0x040000EA RID: 234
		public GUIStyle segmentOutlineRightEdge;

		// Token: 0x040000EB RID: 235
		public GUIStyle indent;

		// Token: 0x040000EC RID: 236
		public GUIStyle outlineBorder;

		// Token: 0x040000ED RID: 237
		public GUIStyle sequenceClipBackground;

		// Token: 0x040000EE RID: 238
		public GUIStyle sequenceClipBackgroundBoth;

		// Token: 0x040000EF RID: 239
		public GUIStyle sequenceClipBackgroundLeft;

		// Token: 0x040000F0 RID: 240
		public GUIStyle sequenceClipBackgroundRight;

		// Token: 0x040000F1 RID: 241
		public GUIStyle curveMode;

		// Token: 0x040000F2 RID: 242
		public GUIStyle sequenceSelector;

		// Token: 0x040000F3 RID: 243
		public GUIStyle timelineClip;

		// Token: 0x040000F4 RID: 244
		public GUIStyle bottomShadow;

		// Token: 0x040000F5 RID: 245
		public GUIStyle trackOptions;

		// Token: 0x040000F6 RID: 246
		public GUIStyle infiniteTrack;

		// Token: 0x040000F7 RID: 247
		public GUIStyle blendingIn;

		// Token: 0x040000F8 RID: 248
		public GUIStyle blendingOut;

		// Token: 0x040000F9 RID: 249
		public GUIStyle clipOut;

		// Token: 0x040000FA RID: 250
		public GUIStyle clipIn;

		// Token: 0x040000FB RID: 251
		public GUIStyle compound;

		// Token: 0x040000FC RID: 252
		public GUIStyle curves;

		// Token: 0x040000FD RID: 253
		public GUIStyle lockedBG;

		// Token: 0x040000FE RID: 254
		public GUIStyle activation;

		// Token: 0x040000FF RID: 255
		public GUIStyle playrange;

		// Token: 0x04000100 RID: 256
		public GUIStyle lockButton;

		// Token: 0x04000101 RID: 257
		private static DirectorStyles ms_Instance;

		// Token: 0x04000102 RID: 258
		private GUIContent m_TempContent = new GUIContent();
	}
}
