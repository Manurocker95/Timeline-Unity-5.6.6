using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000017 RID: 23
	internal class TimelineItem : Control
	{
		// Token: 0x06000122 RID: 290 RVA: 0x0000B708 File Offset: 0x00009B08
		public TimelineItem(GUIStyle style, Action<TimelineWindow.TimelineState, double, bool> onDrag)
		{
			this.m_Style = style;
			this.dottedLine = false;
			this.headColor = Color.white;
			Scrub m = new Scrub(onDrag);
			base.AddManipulator(m);
			this.lineColor = this.m_Style.normal.textColor;
			this.drawLine = true;
			this.drawHead = true;
			this.canMoveHead = false;
			this.tooltip = string.Empty;
			this.alignment = TimelineItem.Alignment.Center;
			this.boundOffset = Vector2.zero;
			this.m_Tooltip = new Tooltip(DirectorStyles.Instance.sequenceClip, DirectorStyles.Instance.tinyFont);
			base.MouseUp += delegate(object target, Event evt, TimelineWindow.TimelineState state)
			{
				if (evt.button == 0 && this.showTooltip)
				{
					this.showTooltip = false;
				}
				return false;
			};
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000B7E4 File Offset: 0x00009BE4
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000B7FE File Offset: 0x00009BFE
		public TimelineItem.Alignment alignment { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000B808 File Offset: 0x00009C08
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000B822 File Offset: 0x00009C22
		public Color headColor { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000B82C File Offset: 0x00009C2C
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000B846 File Offset: 0x00009C46
		public Color lineColor { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000B850 File Offset: 0x00009C50
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000B86A File Offset: 0x00009C6A
		public bool dottedLine { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000B874 File Offset: 0x00009C74
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000B88E File Offset: 0x00009C8E
		public bool drawLine { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000B898 File Offset: 0x00009C98
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000B8B2 File Offset: 0x00009CB2
		public bool drawHead { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000B8BC File Offset: 0x00009CBC
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000B8D6 File Offset: 0x00009CD6
		public bool canMoveHead { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000B8E0 File Offset: 0x00009CE0
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000B8FA File Offset: 0x00009CFA
		public string tooltip { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000B904 File Offset: 0x00009D04
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000B91E File Offset: 0x00009D1E
		public Vector2 boundOffset { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000B928 File Offset: 0x00009D28
		private float widgetHeight
		{
			get
			{
				float fixedHeight = this.m_Style.fixedHeight;
				float result;
				if (fixedHeight < 1f)
				{
					result = (float)this.m_Style.normal.background.height;
				}
				else
				{
					result = fixedHeight;
				}
				return result;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000B974 File Offset: 0x00009D74
		private float widgetWidth
		{
			get
			{
				float fixedWidth = this.m_Style.fixedWidth;
				float result;
				if (fixedWidth < 1f)
				{
					result = (float)this.m_Style.normal.background.width;
				}
				else
				{
					result = fixedWidth;
				}
				return result;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000B9C0 File Offset: 0x00009DC0
		public override Rect bounds
		{
			get
			{
				Rect boundingRect = this.m_BoundingRect;
				boundingRect.y = TimelineWindow.instance.state.timeAreaRect.yMax - this.widgetHeight;
				boundingRect.position += this.boundOffset;
				return boundingRect;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000BA1C File Offset: 0x00009E1C
		// (set) Token: 0x06000139 RID: 313 RVA: 0x0000BA37 File Offset: 0x00009E37
		public bool showTooltip
		{
			get
			{
				return this.m_ShowTooltip;
			}
			set
			{
				this.m_ShowTooltip = value;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000BA44 File Offset: 0x00009E44
		public void Draw(Rect rect, TimelineWindow.TimelineState state, double time)
		{
			Vector2 min = rect.min;
			min.y += 4f;
			min.x = state.TimeToPixel(time);
			TimelineItem.Alignment alignment = this.alignment;
			if (alignment != TimelineItem.Alignment.Center)
			{
				if (alignment != TimelineItem.Alignment.Left)
				{
					if (alignment == TimelineItem.Alignment.Right)
					{
						this.m_BoundingRect = new Rect(min.x, min.y, this.widgetWidth, this.widgetHeight);
					}
				}
				else
				{
					this.m_BoundingRect = new Rect(min.x - this.widgetWidth, min.y, this.widgetWidth, this.widgetHeight);
				}
			}
			else
			{
				this.m_BoundingRect = new Rect(min.x - this.widgetWidth / 2f, min.y, this.widgetWidth, this.widgetHeight);
			}
			if (Event.current.type == 7)
			{
				if (this.m_BoundingRect.xMax < state.timeAreaRect.xMin)
				{
					return;
				}
				if (this.m_BoundingRect.xMin > state.timeAreaRect.xMax)
				{
					return;
				}
			}
			float num = state.timeAreaRect.yMax - TimelineWindowStyles.kDurationGuiThickness;
			Vector3 p;
			p..ctor(min.x, num, 0f);
			Vector3 p2;
			p2..ctor(min.x, num + Mathf.Min(rect.height, state.windowHeight), 0f);
			if (this.drawLine)
			{
				if (this.dottedLine)
				{
					Graphics.DrawDottedLine(p, p2, 5f, this.lineColor);
				}
				else
				{
					Rect rect2 = Rect.MinMaxRect(p.x - 0.5f, p.y, p2.x + 0.5f, p2.y);
					EditorGUI.DrawRect(rect2, this.lineColor);
				}
			}
			if (this.drawHead)
			{
				Color color = GUI.color;
				GUI.color = this.headColor;
				GUI.Box(this.bounds, this.m_HeaderContent, this.m_Style);
				GUI.color = color;
				if (this.canMoveHead)
				{
					EditorGUIUtility.AddCursorRect(this.bounds, 8);
				}
			}
			if (this.showTooltip)
			{
				this.m_Tooltip.text = state.TimeAsString(time, "F2");
				Vector2 position = this.bounds.position;
				position.y = state.timeAreaRect.y;
				position.y -= this.m_Tooltip.bounds.height;
				position.x -= Mathf.Abs(this.m_Tooltip.bounds.width - this.bounds.width) / 2f;
				Rect bounds = this.bounds;
				bounds.position = position;
				this.m_Tooltip.bounds = bounds;
				this.m_Tooltip.Draw();
			}
		}

		// Token: 0x04000133 RID: 307
		private GUIContent m_HeaderContent = new GUIContent();

		// Token: 0x04000134 RID: 308
		private GUIStyle m_Style;

		// Token: 0x04000135 RID: 309
		private Rect m_BoundingRect = default(Rect);

		// Token: 0x04000136 RID: 310
		private Tooltip m_Tooltip = null;

		// Token: 0x04000137 RID: 311
		private Action<TimelineWindow.TimelineState, double, bool> m_OnDrag;

		// Token: 0x04000138 RID: 312
		private bool m_ShowTooltip = false;

		// Token: 0x02000018 RID: 24
		public enum Alignment
		{
			// Token: 0x0400013A RID: 314
			Center,
			// Token: 0x0400013B RID: 315
			Left,
			// Token: 0x0400013C RID: 316
			Right
		}
	}
}
