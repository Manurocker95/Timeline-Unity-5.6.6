using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor.Timeline;

namespace UnityEngine.Timeline
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	internal class DirectorNamedColor : ScriptableObject
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00005D08 File Offset: 0x00004108
		public void SetDefault()
		{
			this.colorPlayhead = DirectorStyles.Instance.timeCursor.normal.textColor;
			this.colorSelection = DirectorStyles.Instance.selectedStyle.normal.textColor;
			this.colorEndmarker = DirectorStyles.Instance.endmarker.normal.textColor;
			this.colorGroup = DirectorStyles.Instance.sequenceGroup.normal.textColor;
			this.colorAnimation = new Color(0.3f, 0.39f, 0.46f, 1f);
			this.colorAnimationRecorded = new Color(this.colorAnimation.r * 0.75f, this.colorAnimation.g * 0.75f, this.colorAnimation.b * 0.75f, 1f);
			this.colorAudio = DirectorStyles.Instance.sequenceTrackHeaderAudio.normal.textColor;
			this.colorScripting = DirectorStyles.Instance.sequenceTrackHeaderScript.normal.textColor;
			this.colorVideo = DirectorStyles.Instance.sequenceTrackHeaderVideo.normal.textColor;
			this.colorEvent = DirectorStyles.Instance.IconEvent.normal.textColor;
			this.colorActivation = Color.green;
			this.colorDropTarget = DirectorStyles.Instance.sequenceOutput.focused.textColor;
			this.colorClipFont = DirectorStyles.Instance.fontClip.normal.textColor;
			this.colorTrackBackground = new Color(0.2f, 0.2f, 0.2f, 1f);
			this.colorTrackBackgroundSelected = DirectorStyles.Instance.sequenceTrack.focused.textColor;
			this.colorTrackFont = DirectorStyles.Instance.sequenceTrackHeaderFont.normal.textColor;
			this.colorCurveSelected = new Color(1f, 1f, 1f, 0.6f);
			this.colorCurveModeSelection = new Color(0.447f, 0.447f, 0.447f, 1f);
			this.colorClipUnion = new Color(0.72f, 0.72f, 0.72f, 0.8f);
			this.colorCurveSelected = new Color(1f, 1f, 1f, 0.6f);
			this.colorCurveModeSelection = new Color(0.447f, 0.447f, 0.447f, 1f);
			this.colorClipUnion = new Color(0.72f, 0.72f, 0.72f, 0.8f);
			this.colorTopOutline1 = new Color(0.152f, 0.152f, 0.152f, 1f);
			this.colorTopOutline2 = new Color(0.184f, 0.184f, 0.184f, 1f);
			this.colorTopOutline3 = new Color(0.274f, 0.274f, 0.274f, 1f);
			this.colorTimecodeBackground = new Color(0.219f, 0.219f, 0.219f, 1f);
			this.colorDurationLine = new Color(0.12941177f, 0.42745098f, 0.47058824f);
			this.colorRange = new Color(0.733f, 0.733f, 0.733f, 0.7f);
			this.colorSequenceBackground = new Color(0.16f, 0.16f, 0.16f, 1f);
			this.colorTooltipBackground = new Color(0.11372549f, 0.1254902f, 0.12941177f);
			this.colorBindingSelectorItemBackground = new Color(0.2f, 0.5f, 1f, 0.5f);
			this.colorInfiniteClipLine = new Color(0.28235295f, 0.30588236f, 0.32156864f);
			this.colorRectangleSelect = new Color(1f, 0.6f, 0f, 0.8f);
			this.colorTrackBackgroundRecording = new Color(1f, 0f, 0f, 0.1f);
			this.colorClipTrimLine = new Color(1f, 1f, 1f, 0.5f);
			this.colorClipBackground = new Color(0.1f, 0.1f, 0.2f, 0.2f);
			this.colorTrackDarken = new Color(0f, 0f, 0f, 0.4f);
			this.colorTrackHeaderBackground = new Color(0.2f, 0.2f, 0.2f, 1f);
			this.colorSnapLine = new Color(1f, 0.6f, 0f, 0.3f);
			this.colorDefaultTrackDrawer = new Color(0.85490197f, 0.8627451f, 0.87058824f);
			this.colorValidDropTarget = Color.yellow;
			this.colorInvalidDropTarget = Color.yellow;
			this.colorInvalidDropTarget.a = 0.3f;
			this.colorRecordingClipOutline = new Color(1f, 0f, 0f, 0.9f);
			this.colorInlineCurveVerticalLines = new Color(1f, 1f, 1f, 0.2f);
			this.colorInlineCurveOutOfRangeOverlay = new Color(0f, 0f, 0f, 0.5f);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00006234 File Offset: 0x00004634
		public void ToText(string path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FieldInfo[] fields = base.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType == typeof(Color))
				{
					Color color = (Color)fieldInfo.GetValue(this);
					stringBuilder.AppendLine(fieldInfo.Name + "," + color.ToString());
				}
			}
			string path2 = Application.dataPath + "/Editor Default Resources/" + path;
			File.WriteAllText(path2, stringBuilder.ToString());
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000062E4 File Offset: 0x000046E4
		public void FromText(string text)
		{
			string[] array = text.Split(new char[]
			{
				'\n',
				'\r'
			}, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, Color> dictionary = new Dictionary<string, Color>();
			foreach (string text2 in array)
			{
				string[] array3 = text2.Replace("RGBA(", "").Replace(")", "").Split(new char[]
				{
					','
				});
				if (array3.Length == 5)
				{
					string key = array3[0].Trim();
					Color black = Color.black;
					bool flag = float.TryParse(array3[1], out black.r) && float.TryParse(array3[2], out black.g) && float.TryParse(array3[3], out black.b) && float.TryParse(array3[4], out black.a);
					if (flag)
					{
						dictionary[key] = black;
					}
				}
			}
			FieldInfo[] fields = typeof(DirectorNamedColor).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType == typeof(Color))
				{
					Color black2 = Color.black;
					if (dictionary.TryGetValue(fieldInfo.Name, out black2))
					{
						fieldInfo.SetValue(this, black2);
					}
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000646C File Offset: 0x0000486C
		public static DirectorNamedColor CreateAndLoadFromText(string text)
		{
			DirectorNamedColor directorNamedColor = ScriptableObject.CreateInstance<DirectorNamedColor>();
			directorNamedColor.FromText(text);
			return directorNamedColor;
		}

		// Token: 0x04000050 RID: 80
		[SerializeField]
		public Color colorPlayhead;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		public Color colorSelection;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		public Color colorEndmarker;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		public Color colorTimelineItem;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		public Color colorGroup;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		public Color colorAnimation;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		public Color colorAnimationRecorded;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		public Color colorAudio;

		// Token: 0x04000058 RID: 88
		[SerializeField]
		public Color colorScripting;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		public Color colorVideo;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		public Color colorEvent;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		public Color colorActivation;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		public Color colorDropTarget;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		public Color colorClipFont;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		public Color colorClipBackground;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		public Color colorClipTrimLine;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		public Color colorTrackBackground;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		public Color colorTrackHeaderBackground;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		public Color colorTrackDarken;

		// Token: 0x04000063 RID: 99
		[SerializeField]
		public Color colorTrackBackgroundRecording;

		// Token: 0x04000064 RID: 100
		[SerializeField]
		public Color colorInfiniteTrackBackgroundRecording;

		// Token: 0x04000065 RID: 101
		[SerializeField]
		public Color colorTrackBackgroundSelected;

		// Token: 0x04000066 RID: 102
		[SerializeField]
		public Color colorTrackFont;

		// Token: 0x04000067 RID: 103
		[SerializeField]
		public Color colorCurveSelected;

		// Token: 0x04000068 RID: 104
		[SerializeField]
		public Color colorCurveModeSelection;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		public Color colorClipUnion;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		public Color colorTopOutline1;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		public Color colorTopOutline2;

		// Token: 0x0400006C RID: 108
		[SerializeField]
		public Color colorTopOutline3;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		public Color colorTimecodeBackground;

		// Token: 0x0400006E RID: 110
		[SerializeField]
		public Color colorDurationLine;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		public Color colorRange;

		// Token: 0x04000070 RID: 112
		[SerializeField]
		public Color colorSequenceBackground;

		// Token: 0x04000071 RID: 113
		[SerializeField]
		public Color colorTooltipBackground;

		// Token: 0x04000072 RID: 114
		[SerializeField]
		public Color colorBindingSelectorItemBackground;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		public Color colorInfiniteClipLine;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		public Color colorRectangleSelect;

		// Token: 0x04000075 RID: 117
		[SerializeField]
		public Color colorSnapLine;

		// Token: 0x04000076 RID: 118
		[SerializeField]
		public Color colorDefaultTrackDrawer;

		// Token: 0x04000077 RID: 119
		[SerializeField]
		public Color colorValidDropTarget = new Color(1f, 0.92156863f, 0.015686275f, 1f);

		// Token: 0x04000078 RID: 120
		[SerializeField]
		public Color colorInvalidDropTarget = new Color(1f, 0.92156863f, 0.015686275f, 0.3f);

		// Token: 0x04000079 RID: 121
		[SerializeField]
		public Color colorBreadCrumb = new Color(0.29411766f, 0.4392157f, 0.69803923f);

		// Token: 0x0400007A RID: 122
		[SerializeField]
		public Color colorBreadCrumbInactive = new Color(1f, 1f, 0f);

		// Token: 0x0400007B RID: 123
		[SerializeField]
		public Color colorDuration = new Color(0.66f, 0.66f, 0.66f, 1f);

		// Token: 0x0400007C RID: 124
		[SerializeField]
		public Color colorRecordingClipOutline = new Color(1f, 0f, 0f, 0.9f);

		// Token: 0x0400007D RID: 125
		[SerializeField]
		public Color colorAnimEditorBinding = new Color(0.21176471f, 0.21176471f, 0.21176471f);

		// Token: 0x0400007E RID: 126
		[SerializeField]
		public Color colorInifiniteTrack = new Color(0.039215688f, 0.039215688f, 0.039215688f);

		// Token: 0x0400007F RID: 127
		[SerializeField]
		public Color colorTimelineBackground = new Color(0.2f, 0.2f, 0.2f, 1f);

		// Token: 0x04000080 RID: 128
		[SerializeField]
		public Color colorKeyFrame = Color.white;

		// Token: 0x04000081 RID: 129
		[SerializeField]
		public Color colorCompound = Color.red;

		// Token: 0x04000082 RID: 130
		[SerializeField]
		public Color colorLockTextBG = Color.red;

		// Token: 0x04000083 RID: 131
		[SerializeField]
		public Color colorInlineCurveVerticalLines = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04000084 RID: 132
		[SerializeField]
		public Color colorInlineCurveOutOfRangeOverlay = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04000085 RID: 133
		[SerializeField]
		public Color colorClipHighlight = new Color(1f, 1f, 1f, 0.2f);

		// Token: 0x04000086 RID: 134
		[SerializeField]
		public Color colorClipShadow = new Color(0f, 0f, 0f, 0.2f);
	}
}
