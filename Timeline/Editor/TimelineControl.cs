using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.Playables
{
	// Token: 0x020007B4 RID: 1972
	internal class TimelineControl
	{
		// Token: 0x0600479B RID: 18331 RVA: 0x001782A4 File Offset: 0x001764A4
		public TimelineControl()
		{
			this.Init();
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600479C RID: 18332 RVA: 0x001783BC File Offset: 0x001765BC
		// (set) Token: 0x0600479D RID: 18333 RVA: 0x001783D8 File Offset: 0x001765D8
		public List<TimelineControl.PivotSample> SrcPivotList
		{
			get
			{
				return this.m_SrcPivotList;
			}
			set
			{
				this.m_SrcPivotList = value;
				this.m_SrcPivotVectors = null;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600479E RID: 18334 RVA: 0x001783EC File Offset: 0x001765EC
		// (set) Token: 0x0600479F RID: 18335 RVA: 0x00178408 File Offset: 0x00176608
		public List<TimelineControl.PivotSample> DstPivotList
		{
			get
			{
				return this.m_DstPivotList;
			}
			set
			{
				this.m_DstPivotList = value;
				this.m_DstPivotVectors = null;
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x0017841C File Offset: 0x0017661C
		// (set) Token: 0x060047A1 RID: 18337 RVA: 0x00178438 File Offset: 0x00176638
		public bool srcLoop
		{
			get
			{
				return this.m_SrcLoop;
			}
			set
			{
				this.m_SrcLoop = value;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x00178444 File Offset: 0x00176644
		// (set) Token: 0x060047A3 RID: 18339 RVA: 0x00178460 File Offset: 0x00176660
		public bool dstLoop
		{
			get
			{
				return this.m_DstLoop;
			}
			set
			{
				this.m_DstLoop = value;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x0017846C File Offset: 0x0017666C
		// (set) Token: 0x060047A5 RID: 18341 RVA: 0x00178488 File Offset: 0x00176688
		public float Time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = value;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060047A6 RID: 18342 RVA: 0x00178494 File Offset: 0x00176694
		// (set) Token: 0x060047A7 RID: 18343 RVA: 0x001784B0 File Offset: 0x001766B0
		public float StartTime
		{
			get
			{
				return this.m_StartTime;
			}
			set
			{
				this.m_StartTime = value;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060047A8 RID: 18344 RVA: 0x001784BC File Offset: 0x001766BC
		// (set) Token: 0x060047A9 RID: 18345 RVA: 0x001784D8 File Offset: 0x001766D8
		public float StopTime
		{
			get
			{
				return this.m_StopTime;
			}
			set
			{
				this.m_StopTime = value;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060047AA RID: 18346 RVA: 0x001784E4 File Offset: 0x001766E4
		// (set) Token: 0x060047AB RID: 18347 RVA: 0x00178500 File Offset: 0x00176700
		public string SrcName
		{
			get
			{
				return this.m_SrcName;
			}
			set
			{
				this.m_SrcName = value;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x0017850C File Offset: 0x0017670C
		// (set) Token: 0x060047AD RID: 18349 RVA: 0x00178528 File Offset: 0x00176728
		public string DstName
		{
			get
			{
				return this.m_DstName;
			}
			set
			{
				this.m_DstName = value;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x00178534 File Offset: 0x00176734
		// (set) Token: 0x060047AF RID: 18351 RVA: 0x00178550 File Offset: 0x00176750
		public float SrcStartTime
		{
			get
			{
				return this.m_SrcStartTime;
			}
			set
			{
				this.m_SrcStartTime = value;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x0017855C File Offset: 0x0017675C
		// (set) Token: 0x060047B1 RID: 18353 RVA: 0x00178578 File Offset: 0x00176778
		public float SrcStopTime
		{
			get
			{
				return this.m_SrcStopTime;
			}
			set
			{
				this.m_SrcStopTime = value;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x00178584 File Offset: 0x00176784
		public float SrcDuration
		{
			get
			{
				return this.SrcStopTime - this.SrcStartTime;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060047B3 RID: 18355 RVA: 0x001785A8 File Offset: 0x001767A8
		// (set) Token: 0x060047B4 RID: 18356 RVA: 0x001785C4 File Offset: 0x001767C4
		public float DstStartTime
		{
			get
			{
				return this.m_DstStartTime;
			}
			set
			{
				this.m_DstStartTime = value;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x001785D0 File Offset: 0x001767D0
		// (set) Token: 0x060047B6 RID: 18358 RVA: 0x001785EC File Offset: 0x001767EC
		public float DstStopTime
		{
			get
			{
				return this.m_DstStopTime;
			}
			set
			{
				this.m_DstStopTime = value;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x001785F8 File Offset: 0x001767F8
		public float DstDuration
		{
			get
			{
				return this.DstStopTime - this.DstStartTime;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060047B8 RID: 18360 RVA: 0x0017861C File Offset: 0x0017681C
		// (set) Token: 0x060047B9 RID: 18361 RVA: 0x00178638 File Offset: 0x00176838
		public float TransitionStartTime
		{
			get
			{
				return this.m_TransitionStartTime;
			}
			set
			{
				this.m_TransitionStartTime = value;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x00178644 File Offset: 0x00176844
		// (set) Token: 0x060047BB RID: 18363 RVA: 0x00178660 File Offset: 0x00176860
		public float TransitionStopTime
		{
			get
			{
				return this.m_TransitionStopTime;
			}
			set
			{
				this.m_TransitionStopTime = value;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x0017866C File Offset: 0x0017686C
		// (set) Token: 0x060047BD RID: 18365 RVA: 0x00178688 File Offset: 0x00176888
		public bool HasExitTime
		{
			get
			{
				return this.m_HasExitTime;
			}
			set
			{
				this.m_HasExitTime = value;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x00178694 File Offset: 0x00176894
		public float TransitionDuration
		{
			get
			{
				return this.TransitionStopTime - this.TransitionStartTime;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060047BF RID: 18367 RVA: 0x001786B8 File Offset: 0x001768B8
		// (set) Token: 0x060047C0 RID: 18368 RVA: 0x001786D4 File Offset: 0x001768D4
		public float SampleStopTime
		{
			get
			{
				return this.m_SampleStopTime;
			}
			set
			{
				this.m_SampleStopTime = value;
			}
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x001786E0 File Offset: 0x001768E0
		public void ResetRange()
		{
			this.m_TimeArea.SetShownHRangeInsideMargins(0f, this.StopTime);
		}
        private static readonly int s_ControlHash = "TimelineControl".GetHashCode();


		// Token: 0x060047C2 RID: 18370 RVA: 0x001786FC File Offset: 0x001768FC
		private void Init()
		{
			if (this.id == -1)
			{
                this.id = GUIUtility.GetControlID(s_ControlHash, FocusType.Passive);
            }
			if (this.m_TimeArea == null)
			{
				this.m_TimeArea = new TimeArea(false);
				this.m_TimeArea.hRangeLocked = false;
				this.m_TimeArea.vRangeLocked = true;
				this.m_TimeArea.hSlider = false;
				this.m_TimeArea.vSlider = false;
				this.m_TimeArea.margin = 10f;
				this.m_TimeArea.scaleWithWindow = true;
				this.m_TimeArea.hTicks.SetTickModulosForFrameRate(30f);
			}
			if (this.styles == null)
			{
				this.styles = new TimelineControl.Styles();
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x001787B4 File Offset: 0x001769B4
		private List<Vector3> GetControls(List<Vector3> segmentPoints, float scale)
		{
			List<Vector3> list = new List<Vector3>();
			List<Vector3> result;
			if (segmentPoints.Count < 2)
			{
				result = list;
			}
			else
			{
				for (int i = 0; i < segmentPoints.Count; i++)
				{
					if (i == 0)
					{
						Vector3 vector = segmentPoints[i];
						Vector3 a = segmentPoints[i + 1];
						Vector3 a2 = a - vector;
						Vector3 vector2 = vector + scale * a2;
						list.Add(vector);
						list.Add(vector2);
					}
					else if (i == segmentPoints.Count - 1)
					{
						Vector3 b = segmentPoints[i - 1];
						Vector3 vector3 = segmentPoints[i];
						Vector3 a3 = vector3 - b;
						Vector3 vector4 = vector3 - scale * a3;
						list.Add(vector4);
						list.Add(vector3);
					}
					else
					{
						Vector3 b2 = segmentPoints[i - 1];
						Vector3 vector5 = segmentPoints[i];
						Vector3 a4 = segmentPoints[i + 1];
						Vector3 normalized = (a4 - b2).normalized;
						Vector3 vector6 = vector5 - scale * normalized * (vector5 - b2).magnitude;
						Vector3 vector7 = vector5 + scale * normalized * (a4 - vector5).magnitude;
						list.Add(vector6);
						list.Add(vector5);
						list.Add(vector7);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00178938 File Offset: 0x00176B38
		private Vector3 CalculatePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = num * num;
			float d = num3 * num;
			float d2 = num2 * t;
			Vector3 a = d * p0;
			a += 3f * num3 * t * p1;
			a += 3f * num * num2 * p2;
			return a + d2 * p3;
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x001789B8 File Offset: 0x00176BB8
		private Color[] GetPivotColors(Vector3[] vectors, float motionStart, float motionStop, Color fromColor, Color toColor, Color loopColor, float offset)
		{
			Color[] array = new Color[vectors.Length];
			float num = this.m_TimeArea.TimeToPixel(this.m_TransitionStartTime, this.m_Rect) + this.m_LeftThumbOffset;
			float num2 = this.m_TimeArea.TimeToPixel(this.m_TransitionStopTime, this.m_Rect) + this.m_RightThumbOffset;
			float num3 = num2 - num;
			for (int i = 0; i < array.Length; i++)
			{
				if (vectors[i].x >= num && vectors[i].x <= num2)
				{
					array[i] = Color.Lerp(fromColor, toColor, (vectors[i].x - num) / num3);
				}
				else if (vectors[i].x < num && vectors[i].x >= motionStart + offset)
				{
					array[i] = fromColor;
				}
				else if (vectors[i].x > num2 && vectors[i].x <= motionStop + offset)
				{
					array[i] = toColor;
				}
				else
				{
					array[i] = loopColor;
				}
			}
			return array;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x00178B10 File Offset: 0x00176D10
		private Vector3[] GetPivotVectors(TimelineControl.PivotSample[] samples, float width, Rect rect, float height, bool loop)
		{
			Vector3[] result;
			if (samples.Length == 0 || width < 0.33f)
			{
				result = new Vector3[0];
			}
			else
			{
				List<Vector3> list = new List<Vector3>();
				foreach (TimelineControl.PivotSample pivotSample in samples)
				{
					Vector3 zero = Vector3.zero;
					zero.x = this.m_TimeArea.TimeToPixel(pivotSample.m_Time, rect);
					zero.y = height / 16f + pivotSample.m_Weight * 12f * height / 16f;
					list.Add(zero);
				}
				if (loop && list[list.Count - 1].x <= rect.width)
				{
					float x = list[list.Count - 1].x;
					int num = 0;
					int num2 = 1;
					List<Vector3> list2 = new List<Vector3>();
					while (x < rect.width)
					{
						if (num > list.Count - 1)
						{
							num = 0;
							num2++;
						}
						Vector3 vector = list[num];
						vector.x += (float)num2 * width;
						x = vector.x;
						list2.Add(vector);
						num++;
					}
					list.AddRange(list2);
				}
				List<Vector3> controls = this.GetControls(list, 0.5f);
				list.Clear();
				for (int j = 0; j < controls.Count - 3; j += 3)
				{
					Vector3 p = controls[j];
					Vector3 p2 = controls[j + 1];
					Vector3 p3 = controls[j + 2];
					Vector3 p4 = controls[j + 3];
					if (j == 0)
					{
						list.Add(this.CalculatePoint(0f, p, p2, p3, p4));
					}
					for (int k = 1; k <= 10; k++)
					{
						list.Add(this.CalculatePoint((float)k / 10f, p, p2, p3, p4));
					}
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x00178D30 File Offset: 0x00176F30
		private Vector3[] OffsetPivotVectors(Vector3[] vectors, float offset)
		{
			for (int i = 0; i < vectors.Length; i++)
			{
				int num = i;
				vectors[num].x = vectors[num].x + offset;
			}
			return vectors;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x00178D70 File Offset: 0x00176F70
		private void DoPivotCurves()
		{
			Color white = Color.white;
			Color white2 = Color.white;
			Color toColor = new Color(1f, 1f, 1f, 0.1f);
			Color fromColor = new Color(1f, 1f, 1f, 0.1f);
			Color loopColor = new Color(0.75f, 0.75f, 0.75f, 0.2f);
			Color loopColor2 = new Color(0.75f, 0.75f, 0.75f, 0.2f);
			Rect rect = new Rect(0f, 18f, this.m_Rect.width, 66f);
			GUI.BeginGroup(rect);
			float num = this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect);
			float num2 = this.m_TimeArea.TimeToPixel(this.SrcStopTime, rect);
			float num3 = this.m_TimeArea.TimeToPixel(this.DstStartTime, rect);
			float num4 = this.m_TimeArea.TimeToPixel(this.DstStopTime, rect);
			if (this.m_SrcPivotVectors == null)
			{
				this.m_SrcPivotVectors = this.GetPivotVectors(this.m_SrcPivotList.ToArray(), num2 - num, rect, rect.height, this.srcLoop);
			}
			if (this.m_DstPivotVectors == null)
			{
				this.m_DstPivotVectors = this.GetPivotVectors(this.m_DstPivotList.ToArray(), num4 - num3, rect, rect.height, this.dstLoop);
			}
			this.m_DstPivotVectors = this.OffsetPivotVectors(this.m_DstPivotVectors, this.m_DstDragOffset + num3 - num);
			Color[] pivotColors = this.GetPivotColors(this.m_SrcPivotVectors, num, num2, white, toColor, loopColor, 0f);
			Color[] pivotColors2 = this.GetPivotColors(this.m_DstPivotVectors, num3, num4, fromColor, white2, loopColor2, this.m_DstDragOffset);
            Handles.color = pivotColors[0];
            Handles.DrawAAPolyLine(this.m_SrcPivotVectors);

            Handles.color = pivotColors2[0];
            Handles.DrawAAPolyLine(this.m_DstPivotVectors);
            GUI.EndGroup();
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x00178F54 File Offset: 0x00177154
		private void EnforceConstraints()
		{
			Rect rect = new Rect(0f, 0f, this.m_Rect.width, 150f);
			if (this.m_DragState == TimelineControl.DragStates.LeftSelection)
			{
				float min = this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect);
				float max = this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect);
				this.m_LeftThumbOffset = Mathf.Clamp(this.m_LeftThumbOffset, min, max);
			}
			if (this.m_DragState == TimelineControl.DragStates.RightSelection)
			{
				float num = this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect);
				if (this.m_RightThumbOffset < num)
				{
					this.m_RightThumbOffset = num;
				}
			}
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x00179038 File Offset: 0x00177238
		private bool WasDraggingData()
		{
			return this.m_DstDragOffset != 0f || this.m_LeftThumbOffset != 0f || this.m_RightThumbOffset != 0f;
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x00179080 File Offset: 0x00177280
		public bool DoTimeline(Rect timeRect)
		{
			bool result = false;
			this.Init();
			this.m_Rect = timeRect;
			float num = this.m_TimeArea.PixelToTime(timeRect.xMin, timeRect);
			float num2 = this.m_TimeArea.PixelToTime(timeRect.xMax, timeRect);
			if (!Mathf.Approximately(num, this.StartTime))
			{
				this.StartTime = num;
				GUI.changed = true;
			}
			if (!Mathf.Approximately(num2, this.StopTime))
			{
				this.StopTime = num2;
				GUI.changed = true;
			}
			this.Time = Mathf.Max(this.Time, 0f);
			if (Event.current.type == EventType.Repaint)
			{
				this.m_TimeArea.rect = timeRect;
			}
			this.m_TimeArea.BeginViewGUI();
			this.m_TimeArea.EndViewGUI();
			GUI.BeginGroup(timeRect);
			Event current = Event.current;
			Rect rect = new Rect(0f, 0f, timeRect.width, timeRect.height);
			Rect position = new Rect(0f, 0f, timeRect.width, 18f);
			Rect position2 = new Rect(0f, 18f, timeRect.width, 132f);
			float num3 = this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect);
			float num4 = this.m_TimeArea.TimeToPixel(this.SrcStopTime, rect);
			float num5 = this.m_TimeArea.TimeToPixel(this.DstStartTime, rect) + this.m_DstDragOffset;
			float num6 = this.m_TimeArea.TimeToPixel(this.DstStopTime, rect) + this.m_DstDragOffset;
			float num7 = this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect) + this.m_LeftThumbOffset;
			float num8 = this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect) + this.m_RightThumbOffset;
			float num9 = this.m_TimeArea.TimeToPixel(this.Time, rect);
			Rect rect2 = new Rect(num3, 85f, num4 - num3, 32f);
			Rect rect3 = new Rect(num5, 117f, num6 - num5, 32f);
			Rect position3 = new Rect(num7, 0f, num8 - num7, 18f);
			Rect position4 = new Rect(num7, 18f, num8 - num7, rect.height - 18f);
			Rect position5 = new Rect(num7 - 9f, 5f, 9f, 15f);
			Rect position6 = new Rect(num8, 5f, 9f, 15f);
			Rect position7 = new Rect(num9 - 7f, 4f, 15f, 15f);
			if (current.type == EventType.KeyDown)
			{
				if (GUIUtility.keyboardControl == this.id && this.m_DragState == TimelineControl.DragStates.Destination)
				{
					this.m_DstDragOffset = 0f;
				}
				if (this.m_DragState == TimelineControl.DragStates.LeftSelection)
				{
					this.m_LeftThumbOffset = 0f;
				}
				if (this.m_DragState == TimelineControl.DragStates.RightSelection)
				{
					this.m_RightThumbOffset = 0f;
				}
				if (this.m_DragState == TimelineControl.DragStates.FullSelection)
				{
					this.m_LeftThumbOffset = 0f;
					this.m_RightThumbOffset = 0f;
				}
			}
			if (current.type == EventType.MouseDown)
			{
				if (rect.Contains(current.mousePosition))
				{
					GUIUtility.hotControl = this.id;
					GUIUtility.keyboardControl = this.id;
					if (position7.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.Playhead;
					}
					else if (rect2.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.Source;
					}
					else if (rect3.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.Destination;
					}
					else if (position5.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.LeftSelection;
					}
					else if (position6.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.RightSelection;
					}
					else if (position3.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.FullSelection;
					}
					else if (position.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.TimeArea;
					}
					else if (position2.Contains(current.mousePosition))
					{
						this.m_DragState = TimelineControl.DragStates.TimeArea;
					}
					else
					{
						this.m_DragState = TimelineControl.DragStates.None;
					}
					current.Use();
				}
			}
			if (current.type == EventType.MouseDrag)
			{
				if (GUIUtility.hotControl == this.id)
				{
					switch (this.m_DragState)
					{
					case TimelineControl.DragStates.LeftSelection:
						if ((current.delta.x > 0f && current.mousePosition.x > num3) || (current.delta.x < 0f && current.mousePosition.x < num8))
						{
							this.m_LeftThumbOffset += current.delta.x;
						}
						this.EnforceConstraints();
						break;
					case TimelineControl.DragStates.RightSelection:
						if ((current.delta.x > 0f && current.mousePosition.x > num7) || current.delta.x < 0f)
						{
							this.m_RightThumbOffset += current.delta.x;
						}
						this.EnforceConstraints();
						break;
					case TimelineControl.DragStates.FullSelection:
						this.m_RightThumbOffset += current.delta.x;
						this.m_LeftThumbOffset += current.delta.x;
						this.EnforceConstraints();
						break;
					case TimelineControl.DragStates.Destination:
						this.m_DstDragOffset += current.delta.x;
						this.EnforceConstraints();
						break;
					case TimelineControl.DragStates.Source:
					{
						TimeArea timeArea = this.m_TimeArea;
						timeArea.m_Translation.x = timeArea.m_Translation.x + current.delta.x;
						break;
					}
					case TimelineControl.DragStates.Playhead:
						if ((current.delta.x > 0f && current.mousePosition.x > num3) || (current.delta.x < 0f && current.mousePosition.x <= this.m_TimeArea.TimeToPixel(this.SampleStopTime, rect)))
						{
							this.Time = this.m_TimeArea.PixelToTime(num9 + current.delta.x, rect);
						}
						break;
					case TimelineControl.DragStates.TimeArea:
					{
						TimeArea timeArea2 = this.m_TimeArea;
						timeArea2.m_Translation.x = timeArea2.m_Translation.x + current.delta.x;
						break;
					}
					}
					current.Use();
					GUI.changed = true;
				}
			}
			if (Event.current.GetTypeForControl(this.id) == EventType.MouseUp)
			{
				this.SrcStartTime = this.m_TimeArea.PixelToTime(num3, rect);
				this.SrcStopTime = this.m_TimeArea.PixelToTime(num4, rect);
				this.DstStartTime = this.m_TimeArea.PixelToTime(num5, rect);
				this.DstStopTime = this.m_TimeArea.PixelToTime(num6, rect);
				this.TransitionStartTime = this.m_TimeArea.PixelToTime(num7, rect);
				this.TransitionStopTime = this.m_TimeArea.PixelToTime(num8, rect);
				GUI.changed = true;
				this.m_DragState = TimelineControl.DragStates.None;
				result = this.WasDraggingData();
				this.m_LeftThumbOffset = 0f;
				this.m_RightThumbOffset = 0f;
				this.m_DstDragOffset = 0f;
				GUIUtility.hotControl = 0;
				current.Use();
			}
			GUI.Box(position, GUIContent.none, this.styles.header);
			GUI.Box(position2, GUIContent.none, this.styles.background);
			this.m_TimeArea.DrawMajorTicks(position2, 30f);
            GUIContent content = new GUIContent(this.SrcName);
            int num10 = (!this.srcLoop) ? 1 : (1 + (int)((num8 - rect2.xMin) / (rect2.xMax - rect2.xMin)));
			Rect position8 = rect2;
			if (rect2.width < 10f)
			{
				position8 = new Rect(rect2.x, rect2.y, (rect2.xMax - rect2.xMin) * (float)num10, rect2.height);
				num10 = 1;
			}
			for (int i = 0; i < num10; i++)
			{
				GUI.BeginGroup(position8, GUIContent.none, this.styles.leftBlock);
				float num11 = num7 - position8.xMin;
				float num12 = num8 - num7;
				float num13 = position8.xMax - position8.xMin - (num11 + num12);
				if (num11 > 0f)
				{
					GUI.Box(new Rect(0f, 0f, num11, rect2.height), GUIContent.none, this.styles.onLeft);
				}
				if (num12 > 0f)
				{
					GUI.Box(new Rect(num11, 0f, num12, rect2.height), GUIContent.none, this.styles.onOff);
				}
				if (num13 > 0f)
				{
					GUI.Box(new Rect(num11 + num12, 0f, num13, rect2.height), GUIContent.none, this.styles.offRight);
				}
				float b = 1f;
				float x = this.styles.block.CalcSize(content).x;
				float num14 = Mathf.Max(0f, num11) - 20f;
				float num15 = num14 + 15f;
				if (num14 < x && num15 > 0f && this.m_DragState == TimelineControl.DragStates.LeftSelection)
				{
					b = 0f;
				}
				GUI.EndGroup();
				float a = this.styles.leftBlock.normal.textColor.a;
				if (!Mathf.Approximately(a, b) && Event.current.type == EventType.Repaint)
				{
					a = Mathf.Lerp(a, b, 0.1f);
					this.styles.leftBlock.normal.textColor = new Color(this.styles.leftBlock.normal.textColor.r, this.styles.leftBlock.normal.textColor.g, this.styles.leftBlock.normal.textColor.b, a);
					HandleUtility.Repaint();
				}
				GUI.Box(position8, content, this.styles.leftBlock);
				position8 = new Rect(position8.xMax, 85f, position8.xMax - position8.xMin, 32f);
			}
            GUIContent content2 = new GUIContent(this.DstName);
            int num16 = (!this.dstLoop) ? 1 : (1 + (int)((num8 - rect3.xMin) / (rect3.xMax - rect3.xMin)));
			position8 = rect3;
			if (rect3.width < 10f)
			{
				position8 = new Rect(rect3.x, rect3.y, (rect3.xMax - rect3.xMin) * (float)num16, rect3.height);
				num16 = 1;
			}
			for (int j = 0; j < num16; j++)
			{
				GUI.BeginGroup(position8, GUIContent.none, this.styles.rightBlock);
				float num17 = num7 - position8.xMin;
				float num18 = num8 - num7;
				float num19 = position8.xMax - position8.xMin - (num17 + num18);
				if (num17 > 0f)
				{
					GUI.Box(new Rect(0f, 0f, num17, rect3.height), GUIContent.none, this.styles.offLeft);
				}
				if (num18 > 0f)
				{
					GUI.Box(new Rect(num17, 0f, num18, rect3.height), GUIContent.none, this.styles.offOn);
				}
				if (num19 > 0f)
				{
					GUI.Box(new Rect(num17 + num18, 0f, num19, rect3.height), GUIContent.none, this.styles.onRight);
				}
				float b2 = 1f;
				float x2 = this.styles.block.CalcSize(content2).x;
				float num20 = Mathf.Max(0f, num17) - 20f;
				float num21 = num20 + 15f;
				if (num20 < x2 && num21 > 0f && (this.m_DragState == TimelineControl.DragStates.LeftSelection || this.m_DragState == TimelineControl.DragStates.Destination))
				{
					b2 = 0f;
				}
				GUI.EndGroup();
				float a2 = this.styles.rightBlock.normal.textColor.a;
				if (!Mathf.Approximately(a2, b2) && Event.current.type == EventType.Repaint)
				{
					a2 = Mathf.Lerp(a2, b2, 0.1f);
					this.styles.rightBlock.normal.textColor = new Color(this.styles.rightBlock.normal.textColor.r, this.styles.rightBlock.normal.textColor.g, this.styles.rightBlock.normal.textColor.b, a2);
					HandleUtility.Repaint();
				}
				GUI.Box(position8, content2, this.styles.rightBlock);
				position8 = new Rect(position8.xMax, position8.yMin, position8.xMax - position8.xMin, 32f);
			}
			GUI.Box(position4, GUIContent.none, this.styles.select);
			GUI.Box(position3, GUIContent.none, this.styles.selectHead);
			this.m_TimeArea.TimeRuler(position, 30f);
			GUI.Box(position5, GUIContent.none, (!this.m_HasExitTime) ? this.styles.handLeftPrev : this.styles.handLeft);
			GUI.Box(position6, GUIContent.none, this.styles.handRight);
			GUI.Box(position7, GUIContent.none, this.styles.playhead);
			Color color = Handles.color;
			Handles.color = Color.white;
			Handles.DrawLine(new Vector3(num9, 19f, 0f), new Vector3(num9, rect.height, 0f));
			Handles.color = color;
			bool flag = this.SrcStopTime - this.SrcStartTime < 0.033333335f;
			bool flag2 = this.DstStopTime - this.DstStartTime < 0.033333335f;
            if (this.m_DragState == TimelineControl.DragStates.Destination && !flag2)
            {
                Rect position9 = new Rect(num7 - 50f, rect3.y, 45f, rect3.height);
                string t = string.Format("{0:0%}", (num7 - num5) / (num6 - num5));

                GUI.Box(position9, t, this.styles.timeBlockRight);
            }

            if (this.m_DragState == TimelineControl.DragStates.LeftSelection)
            {
                if (!flag)
                {
                    Rect position10 = new Rect(num7 - 50f, rect2.y, 45f, rect2.height);
                    string t2 = string.Format("{0:0%}", (num7 - num3) / (num4 - num3));

                    GUI.Box(position10, t2, this.styles.timeBlockRight);
                }

                if (!flag2)
                {
                    Rect position11 = new Rect(num7 - 50f, rect3.y, 45f, rect3.height);
                    string t3 = string.Format("{0:0%}", (num7 - num5) / (num6 - num5));

                    GUI.Box(position11, t3, this.styles.timeBlockRight);
                }
            }

            if (this.m_DragState == TimelineControl.DragStates.RightSelection)
            {
                if (!flag)
                {
                    Rect position12 = new Rect(num8 + 5f, rect2.y, 45f, rect2.height);
                    string t4 = string.Format("{0:0%}", (num8 - num3) / (num4 - num3));

                    GUI.Box(position12, t4, this.styles.timeBlockLeft);
                }

                if (!flag2)
                {
                    Rect position13 = new Rect(num8 + 5f, rect3.y, 45f, rect3.height);
                    string t5 = string.Format("{0:0%}", (num8 - num5) / (num6 - num5));

                    GUI.Box(position13, t5, this.styles.timeBlockLeft);
                }
            }
            this.DoPivotCurves();
			GUI.EndGroup();
			return result;
		}

		// Token: 0x04002995 RID: 10645
		private TimeArea m_TimeArea;

		// Token: 0x04002996 RID: 10646
		private float m_Time = float.PositiveInfinity;

		// Token: 0x04002997 RID: 10647
		private float m_StartTime = 0f;

		// Token: 0x04002998 RID: 10648
		private float m_StopTime = 1f;

		// Token: 0x04002999 RID: 10649
		private string m_SrcName = "Left";

		// Token: 0x0400299A RID: 10650
		private string m_DstName = "Right";

		// Token: 0x0400299B RID: 10651
		private bool m_SrcLoop = false;

		// Token: 0x0400299C RID: 10652
		private bool m_DstLoop = false;

		// Token: 0x0400299D RID: 10653
		private float m_SrcStartTime = 0f;

		// Token: 0x0400299E RID: 10654
		private float m_SrcStopTime = 0.75f;

		// Token: 0x0400299F RID: 10655
		private float m_DstStartTime = 0.25f;

		// Token: 0x040029A0 RID: 10656
		private float m_DstStopTime = 1f;

		// Token: 0x040029A1 RID: 10657
		private bool m_HasExitTime = false;

		// Token: 0x040029A2 RID: 10658
		private float m_TransitionStartTime = float.PositiveInfinity;

		// Token: 0x040029A3 RID: 10659
		private float m_TransitionStopTime = float.PositiveInfinity;

		// Token: 0x040029A4 RID: 10660
		private float m_SampleStopTime = float.PositiveInfinity;

		// Token: 0x040029A5 RID: 10661
		private float m_DstDragOffset = 0f;

		// Token: 0x040029A6 RID: 10662
		private float m_LeftThumbOffset = 0f;

		// Token: 0x040029A7 RID: 10663
		private float m_RightThumbOffset = 0f;

		// Token: 0x040029A8 RID: 10664
		private TimelineControl.DragStates m_DragState = TimelineControl.DragStates.None;

		// Token: 0x040029A9 RID: 10665
		private int id = -1;

		// Token: 0x040029AA RID: 10666
		private Rect m_Rect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x040029AB RID: 10667
		private Vector3[] m_SrcPivotVectors;

		// Token: 0x040029AC RID: 10668
		private Vector3[] m_DstPivotVectors;

		// Token: 0x040029AD RID: 10669
		private List<TimelineControl.PivotSample> m_SrcPivotList = new List<TimelineControl.PivotSample>();

		// Token: 0x040029AE RID: 10670
		private List<TimelineControl.PivotSample> m_DstPivotList = new List<TimelineControl.PivotSample>();

		// Token: 0x040029AF RID: 10671
		private TimelineControl.Styles styles;

		// Token: 0x020007B5 RID: 1973
		private enum DragStates
		{
			// Token: 0x040029B1 RID: 10673
			None,
			// Token: 0x040029B2 RID: 10674
			LeftSelection,
			// Token: 0x040029B3 RID: 10675
			RightSelection,
			// Token: 0x040029B4 RID: 10676
			FullSelection,
			// Token: 0x040029B5 RID: 10677
			Destination,
			// Token: 0x040029B6 RID: 10678
			Source,
			// Token: 0x040029B7 RID: 10679
			Playhead,
			// Token: 0x040029B8 RID: 10680
			TimeArea
		}

		// Token: 0x020007B6 RID: 1974
		private class Styles
		{
			// Token: 0x060047CC RID: 18380 RVA: 0x0017A1D8 File Offset: 0x001783D8
			public Styles()
			{
				this.timeBlockRight.alignment = TextAnchor.MiddleRight;
				this.timeBlockRight.normal.background = null;
				this.timeBlockLeft.normal.background = null;
			}

			// Token: 0x040029B9 RID: 10681
			public readonly GUIStyle block = new GUIStyle("MeTransitionBlock");

			// Token: 0x040029BA RID: 10682
			public GUIStyle leftBlock = new GUIStyle("MeTransitionBlock");

			// Token: 0x040029BB RID: 10683
			public GUIStyle rightBlock = new GUIStyle("MeTransitionBlock");

			// Token: 0x040029BC RID: 10684
			public GUIStyle timeBlockRight = new GUIStyle("MeTimeLabel");

			// Token: 0x040029BD RID: 10685
			public GUIStyle timeBlockLeft = new GUIStyle("MeTimeLabel");

			// Token: 0x040029BE RID: 10686
			public readonly GUIStyle offLeft = new GUIStyle("MeTransOffLeft");

			// Token: 0x040029BF RID: 10687
			public readonly GUIStyle offRight = new GUIStyle("MeTransOffRight");

			// Token: 0x040029C0 RID: 10688
			public readonly GUIStyle onLeft = new GUIStyle("MeTransOnLeft");

			// Token: 0x040029C1 RID: 10689
			public readonly GUIStyle onRight = new GUIStyle("MeTransOnRight");

			// Token: 0x040029C2 RID: 10690
			public readonly GUIStyle offOn = new GUIStyle("MeTransOff2On");

			// Token: 0x040029C3 RID: 10691
			public readonly GUIStyle onOff = new GUIStyle("MeTransOn2Off");

			// Token: 0x040029C4 RID: 10692
			public readonly GUIStyle background = new GUIStyle("MeTransitionBack");

			// Token: 0x040029C5 RID: 10693
			public readonly GUIStyle header = new GUIStyle("MeTransitionHead");

			// Token: 0x040029C6 RID: 10694
			public readonly GUIStyle handLeft = new GUIStyle("MeTransitionHandleLeft");

			// Token: 0x040029C7 RID: 10695
			public readonly GUIStyle handRight = new GUIStyle("MeTransitionHandleRight");

			// Token: 0x040029C8 RID: 10696
			public readonly GUIStyle handLeftPrev = new GUIStyle("MeTransitionHandleLeftPrev");

			// Token: 0x040029C9 RID: 10697
			public readonly GUIStyle playhead = new GUIStyle("MeTransPlayhead");

			// Token: 0x040029CA RID: 10698
			public readonly GUIStyle selectHead = new GUIStyle("MeTransitionSelectHead");

			// Token: 0x040029CB RID: 10699
			public readonly GUIStyle select = new GUIStyle("MeTransitionSelect");
		}

		// Token: 0x020007B7 RID: 1975
		internal class PivotSample
		{
			// Token: 0x040029CC RID: 10700
			public float m_Time;

			// Token: 0x040029CD RID: 10701
			public float m_Weight;
		}
	}
}
