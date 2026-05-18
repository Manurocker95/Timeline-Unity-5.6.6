using System;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.Playables
{
	// Token: 0x02000386 RID: 902
	[Serializable]
	internal class TimeArea : ZoomableArea
	{
		// Token: 0x06002603 RID: 9731 RVA: 0x0006BC28 File Offset: 0x00069E28
		public TimeArea(bool minimalGUI) : base(minimalGUI)
		{
			float[] tickModulos = new float[]
			{
				1E-07f,
				5E-07f,
				1E-06f,
				5E-06f,
				1E-05f,
				5E-05f,
				0.0001f,
				0.0005f,
				0.001f,
				0.005f,
				0.01f,
				0.05f,
				0.1f,
				0.5f,
				1f,
				5f,
				10f,
				50f,
				100f,
				500f,
				1000f,
				5000f,
				10000f,
				50000f,
				100000f,
				500000f,
				1000000f,
				5000000f,
				10000000f
			};
			this.hTicks = new TickHandler();
			this.hTicks.SetTickModulos(tickModulos);
			this.vTicks = new TickHandler();
			this.vTicks.SetTickModulos(tickModulos);
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x0006BC80 File Offset: 0x00069E80
		// (set) Token: 0x06002605 RID: 9733 RVA: 0x0006BC9C File Offset: 0x00069E9C
		public TickHandler hTicks
		{
			get
			{
				return this.m_HTicks;
			}
			set
			{
				this.m_HTicks = value;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x0006BCA8 File Offset: 0x00069EA8
		// (set) Token: 0x06002607 RID: 9735 RVA: 0x0006BCC4 File Offset: 0x00069EC4
		public TickHandler vTicks
		{
			get
			{
				return this.m_VTicks;
			}
			set
			{
				this.m_VTicks = value;
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0006BCD0 File Offset: 0x00069ED0
		private static void InitStyles()
		{
			if (TimeArea.styles == null)
			{
				TimeArea.styles = new TimeArea.Styles2();
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0006BCE8 File Offset: 0x00069EE8
		public void SetTickMarkerRanges()
		{
			this.hTicks.SetRanges(base.shownArea.xMin, base.shownArea.xMax, base.drawRect.xMin, base.drawRect.xMax);
			this.vTicks.SetRanges(base.shownArea.yMin, base.shownArea.yMax, base.drawRect.yMin, base.drawRect.yMax);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0006BD80 File Offset: 0x00069F80
		public void DrawMajorTicks(Rect position, float frameRate)
		{
			Color color = Handles.color;
			GUI.BeginGroup(position);
			if (Event.current.type != EventType.Repaint)
			{
				GUI.EndGroup();
			}
			else
			{
				TimeArea.InitStyles();
				this.SetTickMarkerRanges();
				this.hTicks.SetTickStrengths(3f, 80f, true);
				Color textColor = TimeArea.styles.TimelineTick.normal.textColor;
				textColor.a = 0.1f;
				Handles.color = textColor;
				Rect shownArea = base.shownArea;
				for (int i = 0; i < this.hTicks.tickLevels; i++)
				{
					float num = this.hTicks.GetStrengthOfLevel(i) * 0.9f;
					if (num > 0.5f)
					{
						float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(i, true);
						for (int j = 0; j < ticksAtLevel.Length; j++)
						{
							if (ticksAtLevel[j] >= 0f)
							{
								int num2 = Mathf.RoundToInt(ticksAtLevel[j] * frameRate);
								float x = this.FrameToPixel((float)num2, frameRate, position, shownArea);
								Handles.DrawLine(new Vector3(x, 0f, 0f), new Vector3(x, position.height, 0f));
							}
						}
					}
				}
				GUI.EndGroup();
				Handles.color = color;
			}
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0006BED4 File Offset: 0x0006A0D4
		public void TimeRuler(Rect position, float frameRate)
		{
			this.TimeRuler(position, frameRate, true, false, 1f, TimeArea.TimeFormat.TimeFrame);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0006BEE8 File Offset: 0x0006A0E8
		public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha)
		{
			this.TimeRuler(position, frameRate, labels, useEntireHeight, alpha, TimeArea.TimeFormat.TimeFrame);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0006BEFC File Offset: 0x0006A0FC
		public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha, TimeArea.TimeFormat timeFormat)
		{
			Color color = GUI.color;
			GUI.BeginGroup(position);
			TimeArea.InitStyles();
			//HandleUtility.ApplyWireMaterial();
			Color backgroundColor = GUI.backgroundColor;
			this.SetTickMarkerRanges();
			this.hTicks.SetTickStrengths(3f, 80f, true);
			Color textColor = TimeArea.styles.TimelineTick.normal.textColor;
			textColor.a = 0.75f * alpha;
			if (Event.current.type == EventType.Repaint)
			{
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					GL.Begin(7);
				}
				else
				{
					GL.Begin(1);
				}
				Rect shownArea = base.shownArea;
				for (int i = 0; i < this.hTicks.tickLevels; i++)
				{
					float num = this.hTicks.GetStrengthOfLevel(i) * 0.9f;
					float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(i, true);
					for (int j = 0; j < ticksAtLevel.Length; j++)
					{
						if (ticksAtLevel[j] >= base.hRangeMin && ticksAtLevel[j] <= base.hRangeMax)
						{
							int num2 = Mathf.RoundToInt(ticksAtLevel[j] * frameRate);
							float num3 = (!useEntireHeight) ? (position.height * Mathf.Min(1f, num) * 0.7f) : position.height;
							float x = this.FrameToPixel((float)num2, frameRate, position, shownArea);
							TimeArea.DrawVerticalLineFast(x, position.height - num3 + 0.5f, position.height - 0.5f, new Color(1f, 1f, 1f, num / 0.5f) * textColor);
						}
					}
				}
				GL.End();
			}
			if (labels)
			{
				int levelWithMinSeparation = this.hTicks.GetLevelWithMinSeparation(40f);
				float[] ticksAtLevel2 = this.hTicks.GetTicksAtLevel(levelWithMinSeparation, false);
				for (int k = 0; k < ticksAtLevel2.Length; k++)
				{
					if (ticksAtLevel2[k] >= base.hRangeMin && ticksAtLevel2[k] <= base.hRangeMax)
					{
						int num4 = Mathf.RoundToInt(ticksAtLevel2[k] * frameRate);
						float num5 = Mathf.Floor(this.FrameToPixel((float)num4, frameRate, position));
						string text = this.FormatTime(ticksAtLevel2[k], frameRate, timeFormat);
						GUI.Label(new Rect(num5 + 3f, -3f, 40f, 20f), text, TimeArea.styles.TimelineTick);
					}
				}
			}
			GUI.EndGroup();
			GUI.backgroundColor = backgroundColor;
			GUI.color = color;
		}

        // Token: 0x0600260E RID: 9742 RVA: 0x0006C190 File Offset: 0x0006A390
        public static void DrawVerticalLine(float x, float minY, float maxY, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            Handles.color = color;

            Handles.DrawLine(
                new Vector3(x, minY, 0f),
                new Vector3(x, maxY, 0f)
            );
        }

        // Token: 0x0600260F RID: 9743 RVA: 0x0006C1E4 File Offset: 0x0006A3E4
        public static void DrawVerticalLineFast(float x, float minY, float maxY, Color color)
		{
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				GL.Color(color);
				GL.Vertex(new Vector3(x - 0.5f, minY, 0f));
				GL.Vertex(new Vector3(x + 0.5f, minY, 0f));
				GL.Vertex(new Vector3(x + 0.5f, maxY, 0f));
				GL.Vertex(new Vector3(x - 0.5f, maxY, 0f));
			}
			else
			{
				GL.Color(color);
				GL.Vertex(new Vector3(x, minY, 0f));
				GL.Vertex(new Vector3(x, maxY, 0f));
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x0006C290 File Offset: 0x0006A490
		public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
		{
			int controlID = GUIUtility.GetControlID(3126789, FocusType.Passive);
			return this.BrowseRuler(position, controlID, ref time, frameRate, pickAnywhere, thumbStyle);
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x0006C2C0 File Offset: 0x0006A4C0
		public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, int id, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
		{
			Event current = Event.current;
			Rect position2 = position;
			if (time != -1f)
			{
				position2.x = Mathf.Round(base.TimeToPixel(time, position)) - (float)thumbStyle.overflow.left;
				position2.width = thumbStyle.fixedWidth + (float)thumbStyle.overflow.horizontal;
			}
			switch (current.GetTypeForControl(id))
			{
			case EventType.MouseDown:
				if (position2.Contains(current.mousePosition))
				{
					GUIUtility.hotControl = id;
					TimeArea.s_PickOffset = current.mousePosition.x - base.TimeToPixel(time, position);
					current.Use();
					return TimeArea.TimeRulerDragMode.Start;
				}
				if (pickAnywhere && position.Contains(current.mousePosition))
				{
					GUIUtility.hotControl = id;
					float num = this.SnapTimeToWholeFPS(base.PixelToTime(current.mousePosition.x, position), frameRate);
					TimeArea.s_OriginalTime = time;
					if (num != time)
					{
						GUI.changed = true;
					}
					time = num;
					TimeArea.s_PickOffset = 0f;
					current.Use();
					return TimeArea.TimeRulerDragMode.Start;
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == id)
				{
					GUIUtility.hotControl = 0;
					current.Use();
					return TimeArea.TimeRulerDragMode.End;
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == id)
				{
					float num2 = this.SnapTimeToWholeFPS(base.PixelToTime(current.mousePosition.x - TimeArea.s_PickOffset, position), frameRate);
					if (num2 != time)
					{
						GUI.changed = true;
					}
					time = num2;
					current.Use();
					return TimeArea.TimeRulerDragMode.Dragging;
				}
				break;
			case EventType.KeyDown:
				if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
				{
					if (time != TimeArea.s_OriginalTime)
					{
						GUI.changed = true;
					}
					time = TimeArea.s_OriginalTime;
					GUIUtility.hotControl = 0;
					current.Use();
					return TimeArea.TimeRulerDragMode.Cancel;
				}
				break;
			case EventType.Repaint:
				if (time != -1f)
				{
					bool flag = position.Contains(current.mousePosition);
					position2.x += (float)thumbStyle.overflow.left;
					thumbStyle.Draw(position2, id == GUIUtility.hotControl, flag || id == GUIUtility.hotControl, false, false);
				}
				break;
			}
			return TimeArea.TimeRulerDragMode.None;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0006C544 File Offset: 0x0006A744
		private void DrawLine(Vector2 lhs, Vector2 rhs)
		{
			GL.Vertex(base.DrawingToViewTransformPoint(new Vector3(lhs.x, lhs.y, 0f)));
			GL.Vertex(base.DrawingToViewTransformPoint(new Vector3(rhs.x, rhs.y, 0f)));
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0006C598 File Offset: 0x0006A798
		private float FrameToPixel(float i, float frameRate, Rect rect, Rect theShownArea)
		{
			return (i - theShownArea.xMin * frameRate) * rect.width / (theShownArea.width * frameRate);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0006C5CC File Offset: 0x0006A7CC
		public float FrameToPixel(float i, float frameRate, Rect rect)
		{
			return this.FrameToPixel(i, frameRate, rect, base.shownArea);
		}

        // Token: 0x06002615 RID: 9749 RVA: 0x0006C5F0 File Offset: 0x0006A7F0
        public float TimeField(Rect rect, int id, float time, float frameRate, TimeArea.TimeFormat timeFormat)
        {
            if (timeFormat == TimeArea.TimeFormat.None)
            {
                float time2 = EditorGUI.FloatField(rect, time, EditorStyles.numberField);
                return this.SnapTimeToWholeFPS(time2, frameRate);
            }

            if (timeFormat == TimeArea.TimeFormat.Frame)
            {
                int value = Mathf.RoundToInt(time * frameRate);
                int frame = EditorGUI.IntField(rect, value, EditorStyles.numberField);

                if (frameRate <= 0f)
                    return time;

                return (float)frame / frameRate;
            }

            string text = this.FormatTime(time, frameRate, TimeArea.TimeFormat.TimeFrame);

            EditorGUI.BeginChangeCheck();
            text = EditorGUI.TextField(rect, text, EditorStyles.numberField);

            if (EditorGUI.EndChangeCheck())
            {
                GUI.changed = true;
                text = text.Replace(',', '.');

                int separatorIndex = text.IndexOf(':');

                if (separatorIndex >= 0)
                {
                    string secondsText = text.Substring(0, separatorIndex);
                    string framesText = text.Substring(separatorIndex + 1);

                    int seconds;
                    int frames;

                    if (int.TryParse(secondsText, out seconds) &&
                        int.TryParse(framesText, out frames) &&
                        frameRate > 0f)
                    {
                        return (float)seconds + ((float)frames / frameRate);
                    }
                }
                else
                {
                    float parsedTime;

                    if (float.TryParse(text, out parsedTime))
                        return this.SnapTimeToWholeFPS(parsedTime, frameRate);
                }
            }

            return time;
        }

        // Token: 0x06002616 RID: 9750 RVA: 0x0006C784 File Offset: 0x0006A984
        public float ValueField(Rect rect, int id, float value)
        {
            return EditorGUI.FloatField(rect, value, EditorStyles.numberField);
        }

        // Token: 0x06002617 RID: 9751 RVA: 0x0006C7CC File Offset: 0x0006A9CC
        public string FormatTime(float time, float frameRate, TimeArea.TimeFormat timeFormat)
        {
            if (timeFormat == TimeArea.TimeFormat.None)
            {
                int numberOfDecimalsForMinimumDifference;

                if (frameRate != 0f)
                {
                    numberOfDecimalsForMinimumDifference =
                        GetNumberOfDecimalsForMinimumDifference(1f / frameRate);
                }
                else
                {
                    numberOfDecimalsForMinimumDifference =
                       GetNumberOfDecimalsForMinimumDifference(
                            base.shownArea.width / base.drawRect.width);
                }

                return time.ToString("N" + numberOfDecimalsForMinimumDifference);
            }

            int frame = Mathf.RoundToInt(time * frameRate);

            if (timeFormat == TimeArea.TimeFormat.TimeFrame)
            {
                int frameRateInt = Mathf.RoundToInt(frameRate);
                int length = frameRateInt.ToString().Length;

                string sign = string.Empty;

                if (frame < 0)
                {
                    sign = "-";
                    frame = -frame;
                }

                int seconds = 0;
                int frames = 0;

                if (frameRateInt > 0)
                {
                    seconds = frame / frameRateInt;
                    frames = frame % frameRateInt;
                }

                return sign +
                       seconds.ToString() +
                       ":" +
                       frames.ToString().PadLeft(length, '0');
            }

            return frame.ToString();
        }

        public static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
        {
            minDifference = Mathf.Abs(minDifference);

            if (minDifference <= 0f)
                return 0;

            int decimals = 0;

            while (minDifference < 1f && decimals < 15)
            {
                minDifference *= 10f;
                decimals++;
            }

            return decimals;
        }

        // Token: 0x06002618 RID: 9752 RVA: 0x0006C8E0 File Offset: 0x0006AAE0
        public string FormatValue(float value)
		{
			int numberOfDecimalsForMinimumDifference = GetNumberOfDecimalsForMinimumDifference(base.shownArea.height / base.drawRect.height);
			return value.ToString("N" + numberOfDecimalsForMinimumDifference);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0006C930 File Offset: 0x0006AB30
		public float SnapTimeToWholeFPS(float time, float frameRate)
		{
			float result;
			if (frameRate == 0f)
			{
				result = time;
			}
			else
			{
				result = Mathf.Round(time * frameRate) / frameRate;
			}
			return result;
		}

		// Token: 0x04000FBE RID: 4030
		[SerializeField]
		private TickHandler m_HTicks;

		// Token: 0x04000FBF RID: 4031
		[SerializeField]
		private TickHandler m_VTicks;

		// Token: 0x04000FC0 RID: 4032
		internal const int kTickRulerDistMin = 3;

		// Token: 0x04000FC1 RID: 4033
		internal const int kTickRulerDistFull = 80;

		// Token: 0x04000FC2 RID: 4034
		internal const int kTickRulerDistLabel = 40;

		// Token: 0x04000FC3 RID: 4035
		internal const float kTickRulerHeightMax = 0.7f;

		// Token: 0x04000FC4 RID: 4036
		internal const float kTickRulerFatThreshold = 0.5f;

		// Token: 0x04000FC5 RID: 4037
		private static TimeArea.Styles2 styles;

		// Token: 0x04000FC6 RID: 4038
		private static float s_OriginalTime;

		// Token: 0x04000FC7 RID: 4039
		private static float s_PickOffset;

		// Token: 0x02000387 RID: 903
		public enum TimeFormat
		{
			// Token: 0x04000FC9 RID: 4041
			None,
			// Token: 0x04000FCA RID: 4042
			TimeFrame,
			// Token: 0x04000FCB RID: 4043
			Frame
		}

		// Token: 0x02000388 RID: 904
		private class Styles2
		{
			// Token: 0x04000FCC RID: 4044
			public GUIStyle TimelineTick = "AnimationTimelineTick";

			// Token: 0x04000FCD RID: 4045
			public GUIStyle labelTickMarks = "CurveEditorLabelTickMarks";
		}

		// Token: 0x02000389 RID: 905
		public enum TimeRulerDragMode
		{
			// Token: 0x04000FCF RID: 4047
			None,
			// Token: 0x04000FD0 RID: 4048
			Start,
			// Token: 0x04000FD1 RID: 4049
			End,
			// Token: 0x04000FD2 RID: 4050
			Dragging,
			// Token: 0x04000FD3 RID: 4051
			Cancel
		}
	}
}
