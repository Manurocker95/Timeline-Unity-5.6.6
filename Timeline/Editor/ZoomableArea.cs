using System;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.Playables
{
	// Token: 0x02000383 RID: 899
	[Serializable]
	internal class ZoomableArea
	{
		// Token: 0x06002595 RID: 9621 RVA: 0x00069434 File Offset: 0x00067634
		public ZoomableArea()
		{
			this.m_MinimalGUI = false;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x0006955C File Offset: 0x0006775C
		public ZoomableArea(bool minimalGUI)
		{
			this.m_MinimalGUI = minimalGUI;
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00069684 File Offset: 0x00067884
		public ZoomableArea(bool minimalGUI, bool enableSliderZoom)
		{
			this.m_MinimalGUI = minimalGUI;
			this.m_EnableSliderZoom = enableSliderZoom;
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000697B4 File Offset: 0x000679B4
		// (set) Token: 0x06002599 RID: 9625 RVA: 0x000697D0 File Offset: 0x000679D0
		public bool hRangeLocked
		{
			get
			{
				return this.m_HRangeLocked;
			}
			set
			{
				this.m_HRangeLocked = value;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000697DC File Offset: 0x000679DC
		// (set) Token: 0x0600259B RID: 9627 RVA: 0x000697F8 File Offset: 0x000679F8
		public bool vRangeLocked
		{
			get
			{
				return this.m_VRangeLocked;
			}
			set
			{
				this.m_VRangeLocked = value;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x00069804 File Offset: 0x00067A04
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x00069820 File Offset: 0x00067A20
		public float hBaseRangeMin
		{
			get
			{
				return this.m_HBaseRangeMin;
			}
			set
			{
				this.m_HBaseRangeMin = value;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x0006982C File Offset: 0x00067A2C
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x00069848 File Offset: 0x00067A48
		public float hBaseRangeMax
		{
			get
			{
				return this.m_HBaseRangeMax;
			}
			set
			{
				this.m_HBaseRangeMax = value;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x00069854 File Offset: 0x00067A54
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x00069870 File Offset: 0x00067A70
		public float vBaseRangeMin
		{
			get
			{
				return this.m_VBaseRangeMin;
			}
			set
			{
				this.m_VBaseRangeMin = value;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x0006987C File Offset: 0x00067A7C
		// (set) Token: 0x060025A3 RID: 9635 RVA: 0x00069898 File Offset: 0x00067A98
		public float vBaseRangeMax
		{
			get
			{
				return this.m_VBaseRangeMax;
			}
			set
			{
				this.m_VBaseRangeMax = value;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000698A4 File Offset: 0x00067AA4
		// (set) Token: 0x060025A5 RID: 9637 RVA: 0x000698C0 File Offset: 0x00067AC0
		public bool hAllowExceedBaseRangeMin
		{
			get
			{
				return this.m_HAllowExceedBaseRangeMin;
			}
			set
			{
				this.m_HAllowExceedBaseRangeMin = value;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x000698CC File Offset: 0x00067ACC
		// (set) Token: 0x060025A7 RID: 9639 RVA: 0x000698E8 File Offset: 0x00067AE8
		public bool hAllowExceedBaseRangeMax
		{
			get
			{
				return this.m_HAllowExceedBaseRangeMax;
			}
			set
			{
				this.m_HAllowExceedBaseRangeMax = value;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000698F4 File Offset: 0x00067AF4
		// (set) Token: 0x060025A9 RID: 9641 RVA: 0x00069910 File Offset: 0x00067B10
		public bool vAllowExceedBaseRangeMin
		{
			get
			{
				return this.m_VAllowExceedBaseRangeMin;
			}
			set
			{
				this.m_VAllowExceedBaseRangeMin = value;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x0006991C File Offset: 0x00067B1C
		// (set) Token: 0x060025AB RID: 9643 RVA: 0x00069938 File Offset: 0x00067B38
		public bool vAllowExceedBaseRangeMax
		{
			get
			{
				return this.m_VAllowExceedBaseRangeMax;
			}
			set
			{
				this.m_VAllowExceedBaseRangeMax = value;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x00069944 File Offset: 0x00067B44
		// (set) Token: 0x060025AD RID: 9645 RVA: 0x00069974 File Offset: 0x00067B74
		public float hRangeMin
		{
			get
			{
				return (!this.hAllowExceedBaseRangeMin) ? this.hBaseRangeMin : float.NegativeInfinity;
			}
			set
			{
				this.SetAllowExceed(ref this.m_HBaseRangeMin, ref this.m_HAllowExceedBaseRangeMin, value);
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x0006998C File Offset: 0x00067B8C
		// (set) Token: 0x060025AF RID: 9647 RVA: 0x000699BC File Offset: 0x00067BBC
		public float hRangeMax
		{
			get
			{
				return (!this.hAllowExceedBaseRangeMax) ? this.hBaseRangeMax : float.PositiveInfinity;
			}
			set
			{
				this.SetAllowExceed(ref this.m_HBaseRangeMax, ref this.m_HAllowExceedBaseRangeMax, value);
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000699D4 File Offset: 0x00067BD4
		// (set) Token: 0x060025B1 RID: 9649 RVA: 0x00069A04 File Offset: 0x00067C04
		public float vRangeMin
		{
			get
			{
				return (!this.vAllowExceedBaseRangeMin) ? this.vBaseRangeMin : float.NegativeInfinity;
			}
			set
			{
				this.SetAllowExceed(ref this.m_VBaseRangeMin, ref this.m_VAllowExceedBaseRangeMin, value);
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x00069A1C File Offset: 0x00067C1C
		// (set) Token: 0x060025B3 RID: 9651 RVA: 0x00069A4C File Offset: 0x00067C4C
		public float vRangeMax
		{
			get
			{
				return (!this.vAllowExceedBaseRangeMax) ? this.vBaseRangeMax : float.PositiveInfinity;
			}
			set
			{
				this.SetAllowExceed(ref this.m_VBaseRangeMax, ref this.m_VAllowExceedBaseRangeMax, value);
			}
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x00069A64 File Offset: 0x00067C64
		private void SetAllowExceed(ref float rangeEnd, ref bool allowExceed, float value)
		{
			if (value == float.NegativeInfinity || value == float.PositiveInfinity)
			{
				rangeEnd = (float)((value != float.NegativeInfinity) ? 1 : 0);
				allowExceed = true;
			}
			else
			{
				rangeEnd = value;
				allowExceed = false;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00069AA4 File Offset: 0x00067CA4
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x00069AC0 File Offset: 0x00067CC0
		public float hScaleMin
		{
			get
			{
				return this.m_HScaleMin;
			}
			set
			{
				this.m_HScaleMin = Mathf.Clamp(value, 1E-05f, 100000f);
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x00069ADC File Offset: 0x00067CDC
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x00069AF8 File Offset: 0x00067CF8
		public float hScaleMax
		{
			get
			{
				return this.m_HScaleMax;
			}
			set
			{
				this.m_HScaleMax = Mathf.Clamp(value, 1E-05f, 100000f);
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x00069B14 File Offset: 0x00067D14
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x00069B30 File Offset: 0x00067D30
		public float vScaleMin
		{
			get
			{
				return this.m_VScaleMin;
			}
			set
			{
				this.m_VScaleMin = Mathf.Clamp(value, 1E-05f, 100000f);
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x00069B4C File Offset: 0x00067D4C
		// (set) Token: 0x060025BC RID: 9660 RVA: 0x00069B68 File Offset: 0x00067D68
		public float vScaleMax
		{
			get
			{
				return this.m_VScaleMax;
			}
			set
			{
				this.m_VScaleMax = Mathf.Clamp(value, 1E-05f, 100000f);
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x00069B84 File Offset: 0x00067D84
		// (set) Token: 0x060025BE RID: 9662 RVA: 0x00069BA0 File Offset: 0x00067DA0
		public bool scaleWithWindow
		{
			get
			{
				return this.m_ScaleWithWindow;
			}
			set
			{
				this.m_ScaleWithWindow = value;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x00069BAC File Offset: 0x00067DAC
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x00069BC8 File Offset: 0x00067DC8
		public bool hSlider
		{
			get
			{
				return this.m_HSlider;
			}
			set
			{
				Rect rect = this.rect;
				this.m_HSlider = value;
				this.rect = rect;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x00069BEC File Offset: 0x00067DEC
		// (set) Token: 0x060025C2 RID: 9666 RVA: 0x00069C08 File Offset: 0x00067E08
		public bool vSlider
		{
			get
			{
				return this.m_VSlider;
			}
			set
			{
				Rect rect = this.rect;
				this.m_VSlider = value;
				this.rect = rect;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x00069C2C File Offset: 0x00067E2C
		// (set) Token: 0x060025C4 RID: 9668 RVA: 0x00069C48 File Offset: 0x00067E48
		public bool ignoreScrollWheelUntilClicked
		{
			get
			{
				return this.m_IgnoreScrollWheelUntilClicked;
			}
			set
			{
				this.m_IgnoreScrollWheelUntilClicked = value;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060025C5 RID: 9669 RVA: 0x00069C54 File Offset: 0x00067E54
		// (set) Token: 0x060025C6 RID: 9670 RVA: 0x00069C70 File Offset: 0x00067E70
		public bool enableMouseInput
		{
			get
			{
				return this.m_EnableMouseInput;
			}
			set
			{
				this.m_EnableMouseInput = value;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060025C7 RID: 9671 RVA: 0x00069C7C File Offset: 0x00067E7C
		// (set) Token: 0x060025C8 RID: 9672 RVA: 0x00069C98 File Offset: 0x00067E98
		public bool uniformScale
		{
			get
			{
				return this.m_UniformScale;
			}
			set
			{
				this.m_UniformScale = value;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x00069CA4 File Offset: 0x00067EA4
		// (set) Token: 0x060025CA RID: 9674 RVA: 0x00069CC0 File Offset: 0x00067EC0
		public ZoomableArea.YDirection upDirection
		{
			get
			{
				return this.m_UpDirection;
			}
			set
			{
				if (this.m_UpDirection != value)
				{
					this.m_UpDirection = value;
					this.m_Scale.y = -this.m_Scale.y;
				}
			}
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00069CF0 File Offset: 0x00067EF0
		internal void SetDrawRectHack(Rect r)
		{
			this.m_DrawArea = r;
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x00069CFC File Offset: 0x00067EFC
		public Vector2 scale
		{
			get
			{
				return this.m_Scale;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x00069D18 File Offset: 0x00067F18
		public Vector2 translation
		{
			get
			{
				return this.m_Translation;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (set) Token: 0x060025CE RID: 9678 RVA: 0x00069D34 File Offset: 0x00067F34
		public float margin
		{
			set
			{
				this.m_MarginBottom = value;
				this.m_MarginTop = value;
				this.m_MarginRight = value;
				this.m_MarginLeft = value;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060025CF RID: 9679 RVA: 0x00069D64 File Offset: 0x00067F64
		// (set) Token: 0x060025D0 RID: 9680 RVA: 0x00069D80 File Offset: 0x00067F80
		public float leftmargin
		{
			get
			{
				return this.m_MarginLeft;
			}
			set
			{
				this.m_MarginLeft = value;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060025D1 RID: 9681 RVA: 0x00069D8C File Offset: 0x00067F8C
		// (set) Token: 0x060025D2 RID: 9682 RVA: 0x00069DA8 File Offset: 0x00067FA8
		public float rightmargin
		{
			get
			{
				return this.m_MarginRight;
			}
			set
			{
				this.m_MarginRight = value;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x00069DB4 File Offset: 0x00067FB4
		// (set) Token: 0x060025D4 RID: 9684 RVA: 0x00069DD0 File Offset: 0x00067FD0
		public float topmargin
		{
			get
			{
				return this.m_MarginTop;
			}
			set
			{
				this.m_MarginTop = value;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060025D5 RID: 9685 RVA: 0x00069DDC File Offset: 0x00067FDC
		// (set) Token: 0x060025D6 RID: 9686 RVA: 0x00069DF8 File Offset: 0x00067FF8
		public float bottommargin
		{
			get
			{
				return this.m_MarginBottom;
			}
			set
			{
				this.m_MarginBottom = value;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x00069E04 File Offset: 0x00068004
		private ZoomableArea.Styles styles
		{
			get
			{
				if (this.m_Styles == null)
				{
					this.m_Styles = new ZoomableArea.Styles(this.m_MinimalGUI);
				}
				return this.m_Styles;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060025D8 RID: 9688 RVA: 0x00069E3C File Offset: 0x0006803C
		// (set) Token: 0x060025D9 RID: 9689 RVA: 0x00069ED4 File Offset: 0x000680D4
		public Rect rect
		{
			get
			{
				return new Rect(this.drawRect.x, this.drawRect.y, this.drawRect.width + ((!this.m_VSlider) ? 0f : this.styles.visualSliderWidth), this.drawRect.height + ((!this.m_HSlider) ? 0f : this.styles.visualSliderWidth));
			}
			set
			{
				Rect rect = new Rect(value.x, value.y, value.width - ((!this.m_VSlider) ? 0f : this.styles.visualSliderWidth), value.height - ((!this.m_HSlider) ? 0f : this.styles.visualSliderWidth));
				if (rect != this.m_DrawArea)
				{
					if (this.m_ScaleWithWindow)
					{
						this.m_DrawArea = rect;
						this.shownAreaInsideMargins = this.m_LastShownAreaInsideMargins;
					}
					else
					{
						this.m_Translation += new Vector2((rect.width - this.m_DrawArea.width) / 2f, (rect.height - this.m_DrawArea.height) / 2f);
						this.m_DrawArea = rect;
					}
				}
				this.EnforceScaleAndRange();
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x00069FD8 File Offset: 0x000681D8
		public Rect drawRect
		{
			get
			{
				return this.m_DrawArea;
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00069FF4 File Offset: 0x000681F4
		public void SetShownHRangeInsideMargins(float min, float max)
		{
			float num = this.drawRect.width - this.leftmargin - this.rightmargin;
			if (num < 0.05f)
			{
				num = 0.05f;
			}
			float num2 = max - min;
			if (num2 < 0.05f)
			{
				num2 = 0.05f;
			}
			this.m_Scale.x = num / num2;
			this.m_Translation.x = -min * this.m_Scale.x + this.leftmargin;
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0006A07C File Offset: 0x0006827C
		public void SetShownHRange(float min, float max)
		{
			float num = max - min;
			if (num < 0.05f)
			{
				num = 0.05f;
			}
			this.m_Scale.x = this.drawRect.width / num;
			this.m_Translation.x = -min * this.m_Scale.x;
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0006A0DC File Offset: 0x000682DC
		public void SetShownVRangeInsideMargins(float min, float max)
		{
			if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
			{
				this.m_Scale.y = -(this.drawRect.height - this.topmargin - this.bottommargin) / (max - min);
				this.m_Translation.y = this.drawRect.height - min * this.m_Scale.y - this.topmargin;
			}
			else
			{
				this.m_Scale.y = (this.drawRect.height - this.topmargin - this.bottommargin) / (max - min);
				this.m_Translation.y = -min * this.m_Scale.y - this.bottommargin;
			}
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x0006A1AC File Offset: 0x000683AC
		public void SetShownVRange(float min, float max)
		{
			if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
			{
				this.m_Scale.y = -this.drawRect.height / (max - min);
				this.m_Translation.y = this.drawRect.height - min * this.m_Scale.y;
			}
			else
			{
				this.m_Scale.y = this.drawRect.height / (max - min);
				this.m_Translation.y = -min * this.m_Scale.y;
			}
			this.EnforceScaleAndRange();
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x0006A3B8 File Offset: 0x000685B8
		// (set) Token: 0x060025DF RID: 9695 RVA: 0x0006A250 File Offset: 0x00068450
		public Rect shownArea
		{
			get
			{
				Rect result;
				if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
				{
					result = new Rect(-this.m_Translation.x / this.m_Scale.x, -(this.m_Translation.y - this.drawRect.height) / this.m_Scale.y, this.drawRect.width / this.m_Scale.x, this.drawRect.height / -this.m_Scale.y);
				}
				else
				{
					result = new Rect(-this.m_Translation.x / this.m_Scale.x, -this.m_Translation.y / this.m_Scale.y, this.drawRect.width / this.m_Scale.x, this.drawRect.height / this.m_Scale.y);
				}
				return result;
			}
			set
			{
				float num = (value.width >= 0.05f) ? value.width : 0.05f;
				float num2 = (value.height >= 0.05f) ? value.height : 0.05f;
				if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
				{
					this.m_Scale.x = this.drawRect.width / num;
					this.m_Scale.y = -this.drawRect.height / num2;
					this.m_Translation.x = -value.x * this.m_Scale.x;
					this.m_Translation.y = this.drawRect.height - value.y * this.m_Scale.y;
				}
				else
				{
					this.m_Scale.x = this.drawRect.width / num;
					this.m_Scale.y = this.drawRect.height / num2;
					this.m_Translation.x = -value.x * this.m_Scale.x;
					this.m_Translation.y = -value.y * this.m_Scale.y;
				}
				this.EnforceScaleAndRange();
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060025E2 RID: 9698 RVA: 0x0006A4D4 File Offset: 0x000686D4
		// (set) Token: 0x060025E1 RID: 9697 RVA: 0x0006A4C4 File Offset: 0x000686C4
		public Rect shownAreaInsideMargins
		{
			get
			{
				return this.shownAreaInsideMarginsInternal;
			}
			set
			{
				this.shownAreaInsideMarginsInternal = value;
				this.EnforceScaleAndRange();
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060025E4 RID: 9700 RVA: 0x0006A69C File Offset: 0x0006889C
		// (set) Token: 0x060025E3 RID: 9699 RVA: 0x0006A4F0 File Offset: 0x000686F0
		private Rect shownAreaInsideMarginsInternal
		{
			get
			{
				float num = this.leftmargin / this.m_Scale.x;
				float num2 = this.rightmargin / this.m_Scale.x;
				float num3 = this.topmargin / this.m_Scale.y;
				float num4 = this.bottommargin / this.m_Scale.y;
				Rect shownArea = this.shownArea;
				shownArea.x += num;
				shownArea.y -= num3;
				shownArea.width -= num + num2;
				shownArea.height += num3 + num4;
				return shownArea;
			}
			set
			{
				float num = (value.width >= 0.05f) ? value.width : 0.05f;
				float num2 = (value.height >= 0.05f) ? value.height : 0.05f;
				float num3 = this.drawRect.width - this.leftmargin - this.rightmargin;
				if (num3 < 0.05f)
				{
					num3 = 0.05f;
				}
				float num4 = this.drawRect.height - this.topmargin - this.bottommargin;
				if (num4 < 0.05f)
				{
					num4 = 0.05f;
				}
				if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
				{
					this.m_Scale.x = num3 / num;
					this.m_Scale.y = -num4 / num2;
					this.m_Translation.x = -value.x * this.m_Scale.x + this.leftmargin;
					this.m_Translation.y = this.drawRect.height - value.y * this.m_Scale.y - this.topmargin;
				}
				else
				{
					this.m_Scale.x = num3 / num;
					this.m_Scale.y = num4 / num2;
					this.m_Translation.x = -value.x * this.m_Scale.x + this.leftmargin;
					this.m_Translation.y = -value.y * this.m_Scale.y + this.topmargin;
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x0006A74C File Offset: 0x0006894C
		public virtual Bounds drawingBounds
		{
			get
			{
				return new Bounds(new Vector3((this.hBaseRangeMin + this.hBaseRangeMax) * 0.5f, (this.vBaseRangeMin + this.vBaseRangeMax) * 0.5f, 0f), new Vector3(this.hBaseRangeMax - this.hBaseRangeMin, this.vBaseRangeMax - this.vBaseRangeMin, 1f));
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x0006A7BC File Offset: 0x000689BC
		public Matrix4x4 drawingToViewMatrix
		{
			get
			{
				return Matrix4x4.TRS(this.m_Translation, Quaternion.identity, new Vector3(this.m_Scale.x, this.m_Scale.y, 1f));
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x0006A808 File Offset: 0x00068A08
		public Vector2 DrawingToViewTransformPoint(Vector2 lhs)
		{
			return new Vector2(lhs.x * this.m_Scale.x + this.m_Translation.x, lhs.y * this.m_Scale.y + this.m_Translation.y);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x0006A860 File Offset: 0x00068A60
		public Vector3 DrawingToViewTransformPoint(Vector3 lhs)
		{
			return new Vector3(lhs.x * this.m_Scale.x + this.m_Translation.x, lhs.y * this.m_Scale.y + this.m_Translation.y, 0f);
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x0006A8C0 File Offset: 0x00068AC0
		public Vector2 ViewToDrawingTransformPoint(Vector2 lhs)
		{
			return new Vector2((lhs.x - this.m_Translation.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0006A918 File Offset: 0x00068B18
		public Vector3 ViewToDrawingTransformPoint(Vector3 lhs)
		{
			return new Vector3((lhs.x - this.m_Translation.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y, 0f);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0006A978 File Offset: 0x00068B78
		public Vector2 DrawingToViewTransformVector(Vector2 lhs)
		{
			return new Vector2(lhs.x * this.m_Scale.x, lhs.y * this.m_Scale.y);
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0006A9B8 File Offset: 0x00068BB8
		public Vector3 DrawingToViewTransformVector(Vector3 lhs)
		{
			return new Vector3(lhs.x * this.m_Scale.x, lhs.y * this.m_Scale.y, 0f);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0006AA00 File Offset: 0x00068C00
		public Vector2 ViewToDrawingTransformVector(Vector2 lhs)
		{
			return new Vector2(lhs.x / this.m_Scale.x, lhs.y / this.m_Scale.y);
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0006AA40 File Offset: 0x00068C40
		public Vector3 ViewToDrawingTransformVector(Vector3 lhs)
		{
			return new Vector3(lhs.x / this.m_Scale.x, lhs.y / this.m_Scale.y, 0f);
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060025EF RID: 9711 RVA: 0x0006AA88 File Offset: 0x00068C88
		public Vector2 mousePositionInDrawing
		{
			get
			{
				return this.ViewToDrawingTransformPoint(Event.current.mousePosition);
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0006AAB0 File Offset: 0x00068CB0
		public Vector2 NormalizeInViewSpace(Vector2 vec)
		{
			vec = Vector2.Scale(vec, this.m_Scale);
			vec /= vec.magnitude;
			return Vector2.Scale(vec, new Vector2(1f / this.m_Scale.x, 1f / this.m_Scale.y));
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0006AB10 File Offset: 0x00068D10
		private bool IsZoomEvent()
		{
			return Event.current.button == 1 && Event.current.alt;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x0006AB44 File Offset: 0x00068D44
		private bool IsPanEvent()
		{
			return (Event.current.button == 0 && Event.current.alt) || (Event.current.button == 2 && !Event.current.command);
		}
        private static readonly int s_MinMaxSliderHash = "MinMaxSlider".GetHashCode();
        // Token: 0x060025F3 RID: 9715 RVA: 0x0006AB9C File Offset: 0x00068D9C
        public void BeginViewGUI()
		{
			if (this.styles.horizontalScrollbar == null)
			{
				this.styles.InitGUIStyles(this.m_MinimalGUI, this.m_EnableSliderZoom);
			}
			if (this.enableMouseInput)
			{
				this.HandleZoomAndPanEvents(this.m_DrawArea);
			}
            this.horizontalScrollbarID = GUIUtility.GetControlID(s_MinMaxSliderHash, FocusType.Passive);
            this.verticalScrollbarID = GUIUtility.GetControlID(s_MinMaxSliderHash, FocusType.Passive);
            if (!this.m_MinimalGUI || Event.current.type != EventType.Repaint)
			{
				this.SliderGUI();
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0006AC2C File Offset: 0x00068E2C
		public void HandleZoomAndPanEvents(Rect area)
		{
			GUILayout.BeginArea(area);
			area.x = 0f;
			area.y = 0f;
			int controlID = GUIUtility.GetControlID(ZoomableArea.zoomableAreaHash, FocusType.Passive, area);
			switch (Event.current.GetTypeForControl(controlID))
			{
			case EventType.MouseDown:
				if (area.Contains(Event.current.mousePosition))
				{
					GUIUtility.keyboardControl = controlID;
					if (this.IsZoomEvent() || this.IsPanEvent())
					{
						GUIUtility.hotControl = controlID;
						ZoomableArea.m_MouseDownPosition = this.mousePositionInDrawing;
						Event.current.Use();
					}
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					ZoomableArea.m_MouseDownPosition = new Vector2(-1000000f, -1000000f);
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					if (this.IsZoomEvent())
					{
						this.HandleZoomEvent(ZoomableArea.m_MouseDownPosition, false);
						Event.current.Use();
					}
					else if (this.IsPanEvent())
					{
						this.Pan();
						Event.current.Use();
					}
				}
				break;
			case EventType.ScrollWheel:
				if (area.Contains(Event.current.mousePosition))
				{
					if (!this.m_IgnoreScrollWheelUntilClicked || GUIUtility.keyboardControl == controlID)
					{
						this.HandleZoomEvent(this.mousePositionInDrawing, true);
						Event.current.Use();
					}
				}
				break;
			}
			GUILayout.EndArea();
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0006ADCC File Offset: 0x00068FCC
		public void EndViewGUI()
		{
			if (this.m_MinimalGUI && Event.current.type == EventType.Repaint)
			{
				this.SliderGUI();
			}
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0006ADF0 File Offset: 0x00068FF0
		private void SliderGUI()
		{
			if (this.m_HSlider || this.m_VSlider)
			{
				using (new EditorGUI.DisabledScope(!this.enableMouseInput))
				{
					Bounds drawingBounds = this.drawingBounds;
					Rect shownAreaInsideMargins = this.shownAreaInsideMargins;
					float num = this.styles.sliderWidth - this.styles.visualSliderWidth;
					float num2 = (!this.vSlider || !this.hSlider) ? 0f : num;
					Vector2 a = this.m_Scale;
					if (this.m_HSlider)
					{
						Rect position = new Rect(this.drawRect.x + 1f, this.drawRect.yMax - num, this.drawRect.width - num2, this.styles.sliderWidth);
						float width = shownAreaInsideMargins.width;
						float num3 = shownAreaInsideMargins.xMin;
                        if (this.m_EnableSliderZoom)
                        {
                            num3 = GUI.HorizontalScrollbar(
                                position,
                                num3,
                                width,
                                drawingBounds.min.x,
                                drawingBounds.max.x,
                                this.styles.horizontalScrollbar
                            );
                        }
                        else
                        {
                            num3 = GUI.HorizontalScrollbar(
								  position,
								  num3,
								  width,
								  drawingBounds.min.x,
								  drawingBounds.max.x,
								  this.styles.horizontalScrollbar
							  );
                        }
                        float num4 = num3;
						float num5 = num3 + width;
						if (num4 > shownAreaInsideMargins.xMin)
						{
							num4 = Mathf.Min(num4, num5 - this.rect.width / this.m_HScaleMax);
						}
						if (num5 < shownAreaInsideMargins.xMax)
						{
							num5 = Mathf.Max(num5, num4 + this.rect.width / this.m_HScaleMax);
						}
						this.SetShownHRangeInsideMargins(num4, num5);
					}
					if (this.m_VSlider)
					{
						if (this.m_UpDirection == ZoomableArea.YDirection.Positive)
						{
							Rect position2 = new Rect(this.drawRect.xMax - num, this.drawRect.y, this.styles.sliderWidth, this.drawRect.height - num2);
							float height = shownAreaInsideMargins.height;
							float num6 = -shownAreaInsideMargins.yMax;
                            if (this.m_EnableSliderZoom)
                            {
                                num6 = GUI.VerticalScrollbar(
                                    position2,
                                    num6,
                                    height,
                                    -drawingBounds.max.y,
                                    -drawingBounds.min.y,
                                    this.styles.verticalScrollbar
                                );
                            }
                            else
                            {
                                num6 = GUI.VerticalScrollbar(
                                    position2,
                                    num6,
                                    height,
                                    -drawingBounds.max.y,
                                    -drawingBounds.min.y,
                                    this.styles.verticalScrollbar
                                );
                            }
                            float num4 = -(num6 + height);
							float num5 = -num6;
							if (num4 > shownAreaInsideMargins.yMin)
							{
								num4 = Mathf.Min(num4, num5 - this.rect.height / this.m_VScaleMax);
							}
							if (num5 < shownAreaInsideMargins.yMax)
							{
								num5 = Mathf.Max(num5, num4 + this.rect.height / this.m_VScaleMax);
							}
							this.SetShownVRangeInsideMargins(num4, num5);
						}
						else
						{
							Rect position3 = new Rect(this.drawRect.xMax - num, this.drawRect.y, this.styles.sliderWidth, this.drawRect.height - num2);
							float height2 = shownAreaInsideMargins.height;
							float num7 = shownAreaInsideMargins.yMin;
                            if (this.m_EnableSliderZoom)
                            {
                                num7 = GUI.VerticalScrollbar(
                                    position3,
                                    num7,
                                    height2,
                                    drawingBounds.min.y,
                                    drawingBounds.max.y,
                                    this.styles.verticalScrollbar
                                );
                            }
                            else
                            {
                                num7 = GUI.VerticalScrollbar(
                                    position3,
                                    num7,
                                    height2,
                                    drawingBounds.min.y,
                                    drawingBounds.max.y,
                                    this.styles.verticalScrollbar
                                );
                            }
                            float num4 = num7;
							float num5 = num7 + height2;
							if (num4 > shownAreaInsideMargins.yMin)
							{
								num4 = Mathf.Min(num4, num5 - this.rect.height / this.m_VScaleMax);
							}
							if (num5 < shownAreaInsideMargins.yMax)
							{
								num5 = Mathf.Max(num5, num4 + this.rect.height / this.m_VScaleMax);
							}
							this.SetShownVRangeInsideMargins(num4, num5);
						}
					}
					if (this.uniformScale)
					{
						float num8 = this.drawRect.width / this.drawRect.height;
						a -= this.m_Scale;
						Vector2 b = new Vector2(-a.y * num8, -a.x / num8);
						this.m_Scale -= b;
						this.m_Translation.x = this.m_Translation.x - a.y / 2f;
						this.m_Translation.y = this.m_Translation.y - a.x / 2f;
						this.EnforceScaleAndRange();
					}
				}
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0006B470 File Offset: 0x00069670
		private void Pan()
		{
			if (!this.m_HRangeLocked)
			{
				this.m_Translation.x = this.m_Translation.x + Event.current.delta.x;
			}
			if (!this.m_VRangeLocked)
			{
				this.m_Translation.y = this.m_Translation.y + Event.current.delta.y;
			}
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0006B4E4 File Offset: 0x000696E4
		private void HandleZoomEvent(Vector2 zoomAround, bool scrollwhell)
		{
			float num = Event.current.delta.x + Event.current.delta.y;
			if (scrollwhell)
			{
				num = -num;
			}
			float num2 = Mathf.Max(0.01f, 1f + num * 0.01f);
			float width = this.shownAreaInsideMargins.width;
			if (width / num2 > 0.05f)
			{
				this.SetScaleFocused(zoomAround, num2 * this.m_Scale, Event.current.shift, EditorGUI.actionKey);
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0006B580 File Offset: 0x00069780
		public void SetScaleFocused(Vector2 focalPoint, Vector2 newScale)
		{
			this.SetScaleFocused(focalPoint, newScale, false, false);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0006B590 File Offset: 0x00069790
		public void SetScaleFocused(Vector2 focalPoint, Vector2 newScale, bool lockHorizontal, bool lockVertical)
		{
			if (this.uniformScale)
			{
				lockVertical = (lockHorizontal = false);
			}
			if (!this.m_HRangeLocked && !lockHorizontal)
			{
				this.m_Translation.x = this.m_Translation.x - focalPoint.x * (newScale.x - this.m_Scale.x);
				this.m_Scale.x = newScale.x;
			}
			if (!this.m_VRangeLocked && !lockVertical)
			{
				this.m_Translation.y = this.m_Translation.y - focalPoint.y * (newScale.y - this.m_Scale.y);
				this.m_Scale.y = newScale.y;
			}
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0006B65C File Offset: 0x0006985C
		public void SetTransform(Vector2 newTranslation, Vector2 newScale)
		{
			this.m_Scale = newScale;
			this.m_Translation = newTranslation;
			this.EnforceScaleAndRange();
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0006B674 File Offset: 0x00069874
		public void EnforceScaleAndRange()
		{
			float num = this.rect.width / this.m_HScaleMin;
			float num2 = this.rect.height / this.m_VScaleMin;
			if (this.hRangeMax != float.PositiveInfinity && this.hRangeMin != float.NegativeInfinity)
			{
				num = Mathf.Min(num, this.hRangeMax - this.hRangeMin);
			}
			if (this.vRangeMax != float.PositiveInfinity && this.vRangeMin != float.NegativeInfinity)
			{
				num2 = Mathf.Min(num2, this.vRangeMax - this.vRangeMin);
			}
			Rect lastShownAreaInsideMargins = this.m_LastShownAreaInsideMargins;
			Rect shownAreaInsideMargins = this.shownAreaInsideMargins;
			if (!(shownAreaInsideMargins == lastShownAreaInsideMargins))
			{
				float num3 = 1E-05f;
				if (shownAreaInsideMargins.width < lastShownAreaInsideMargins.width - num3)
				{
					float t = Mathf.InverseLerp(lastShownAreaInsideMargins.width, shownAreaInsideMargins.width, this.rect.width / this.m_HScaleMax);
					shownAreaInsideMargins = new Rect(Mathf.Lerp(lastShownAreaInsideMargins.x, shownAreaInsideMargins.x, t), shownAreaInsideMargins.y, Mathf.Lerp(lastShownAreaInsideMargins.width, shownAreaInsideMargins.width, t), shownAreaInsideMargins.height);
				}
				if (shownAreaInsideMargins.height < lastShownAreaInsideMargins.height - num3)
				{
					float t2 = Mathf.InverseLerp(lastShownAreaInsideMargins.height, shownAreaInsideMargins.height, this.rect.height / this.m_VScaleMax);
					shownAreaInsideMargins = new Rect(shownAreaInsideMargins.x, Mathf.Lerp(lastShownAreaInsideMargins.y, shownAreaInsideMargins.y, t2), shownAreaInsideMargins.width, Mathf.Lerp(lastShownAreaInsideMargins.height, shownAreaInsideMargins.height, t2));
				}
				if (shownAreaInsideMargins.width > lastShownAreaInsideMargins.width + num3)
				{
					float t3 = Mathf.InverseLerp(lastShownAreaInsideMargins.width, shownAreaInsideMargins.width, num);
					shownAreaInsideMargins = new Rect(Mathf.Lerp(lastShownAreaInsideMargins.x, shownAreaInsideMargins.x, t3), shownAreaInsideMargins.y, Mathf.Lerp(lastShownAreaInsideMargins.width, shownAreaInsideMargins.width, t3), shownAreaInsideMargins.height);
				}
				if (shownAreaInsideMargins.height > lastShownAreaInsideMargins.height + num3)
				{
					float t4 = Mathf.InverseLerp(lastShownAreaInsideMargins.height, shownAreaInsideMargins.height, num2);
					shownAreaInsideMargins = new Rect(shownAreaInsideMargins.x, Mathf.Lerp(lastShownAreaInsideMargins.y, shownAreaInsideMargins.y, t4), shownAreaInsideMargins.width, Mathf.Lerp(lastShownAreaInsideMargins.height, shownAreaInsideMargins.height, t4));
				}
				if (shownAreaInsideMargins.xMin < this.hRangeMin)
				{
					shownAreaInsideMargins.x = this.hRangeMin;
				}
				if (shownAreaInsideMargins.xMax > this.hRangeMax)
				{
					shownAreaInsideMargins.x = this.hRangeMax - shownAreaInsideMargins.width;
				}
				if (shownAreaInsideMargins.yMin < this.vRangeMin)
				{
					shownAreaInsideMargins.y = this.vRangeMin;
				}
				if (shownAreaInsideMargins.yMax > this.vRangeMax)
				{
					shownAreaInsideMargins.y = this.vRangeMax - shownAreaInsideMargins.height;
				}
				this.shownAreaInsideMarginsInternal = shownAreaInsideMargins;
				this.m_LastShownAreaInsideMargins = shownAreaInsideMargins;
			}
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0006B9BC File Offset: 0x00069BBC
		public float PixelToTime(float pixelX, Rect rect)
		{
			return (pixelX - rect.x) * this.shownArea.width / rect.width + this.shownArea.x;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0006BA00 File Offset: 0x00069C00
		public float TimeToPixel(float time, Rect rect)
		{
			return (time - this.shownArea.x) / this.shownArea.width * rect.width + rect.x;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0006BA44 File Offset: 0x00069C44
		public float PixelDeltaToTime(Rect rect)
		{
			return this.shownArea.width / rect.width;
		}

		// Token: 0x04000F89 RID: 3977
		private static Vector2 m_MouseDownPosition = new Vector2(-1000000f, -1000000f);

		// Token: 0x04000F8A RID: 3978
		private static int zoomableAreaHash = "ZoomableArea".GetHashCode();

		// Token: 0x04000F8B RID: 3979
		[SerializeField]
		private bool m_HRangeLocked;

		// Token: 0x04000F8C RID: 3980
		[SerializeField]
		private bool m_VRangeLocked;

		// Token: 0x04000F8D RID: 3981
		[SerializeField]
		private float m_HBaseRangeMin = 0f;

		// Token: 0x04000F8E RID: 3982
		[SerializeField]
		private float m_HBaseRangeMax = 1f;

		// Token: 0x04000F8F RID: 3983
		[SerializeField]
		private float m_VBaseRangeMin = 0f;

		// Token: 0x04000F90 RID: 3984
		[SerializeField]
		private float m_VBaseRangeMax = 1f;

		// Token: 0x04000F91 RID: 3985
		[SerializeField]
		private bool m_HAllowExceedBaseRangeMin = true;

		// Token: 0x04000F92 RID: 3986
		[SerializeField]
		private bool m_HAllowExceedBaseRangeMax = true;

		// Token: 0x04000F93 RID: 3987
		[SerializeField]
		private bool m_VAllowExceedBaseRangeMin = true;

		// Token: 0x04000F94 RID: 3988
		[SerializeField]
		private bool m_VAllowExceedBaseRangeMax = true;

		// Token: 0x04000F95 RID: 3989
		private const float kMinScale = 1E-05f;

		// Token: 0x04000F96 RID: 3990
		private const float kMaxScale = 100000f;

		// Token: 0x04000F97 RID: 3991
		private float m_HScaleMin = 1E-05f;

		// Token: 0x04000F98 RID: 3992
		private float m_HScaleMax = 100000f;

		// Token: 0x04000F99 RID: 3993
		private float m_VScaleMin = 1E-05f;

		// Token: 0x04000F9A RID: 3994
		private float m_VScaleMax = 100000f;

		// Token: 0x04000F9B RID: 3995
		private const float kMinWidth = 0.05f;

		// Token: 0x04000F9C RID: 3996
		private const float kMinHeight = 0.05f;

		// Token: 0x04000F9D RID: 3997
		[SerializeField]
		private bool m_ScaleWithWindow = false;

		// Token: 0x04000F9E RID: 3998
		[SerializeField]
		private bool m_HSlider = true;

		// Token: 0x04000F9F RID: 3999
		[SerializeField]
		private bool m_VSlider = true;

		// Token: 0x04000FA0 RID: 4000
		[SerializeField]
		private bool m_IgnoreScrollWheelUntilClicked = false;

		// Token: 0x04000FA1 RID: 4001
		[SerializeField]
		private bool m_EnableMouseInput = true;

		// Token: 0x04000FA2 RID: 4002
		[SerializeField]
		private bool m_EnableSliderZoom = true;

		// Token: 0x04000FA3 RID: 4003
		public bool m_UniformScale;

		// Token: 0x04000FA4 RID: 4004
		[SerializeField]
		private ZoomableArea.YDirection m_UpDirection = ZoomableArea.YDirection.Positive;

		// Token: 0x04000FA5 RID: 4005
		[SerializeField]
		private Rect m_DrawArea = new Rect(0f, 0f, 100f, 100f);

		// Token: 0x04000FA6 RID: 4006
		[SerializeField]
		internal Vector2 m_Scale = new Vector2(1f, -1f);

		// Token: 0x04000FA7 RID: 4007
		[SerializeField]
		internal Vector2 m_Translation = new Vector2(0f, 0f);

		// Token: 0x04000FA8 RID: 4008
		[SerializeField]
		private float m_MarginLeft;

		// Token: 0x04000FA9 RID: 4009
		[SerializeField]
		private float m_MarginRight;

		// Token: 0x04000FAA RID: 4010
		[SerializeField]
		private float m_MarginTop;

		// Token: 0x04000FAB RID: 4011
		[SerializeField]
		private float m_MarginBottom;

		// Token: 0x04000FAC RID: 4012
		[SerializeField]
		private Rect m_LastShownAreaInsideMargins = new Rect(0f, 0f, 100f, 100f);

		// Token: 0x04000FAD RID: 4013
		private int verticalScrollbarID;

		// Token: 0x04000FAE RID: 4014
		private int horizontalScrollbarID;

		// Token: 0x04000FAF RID: 4015
		[SerializeField]
		private bool m_MinimalGUI;

		// Token: 0x04000FB0 RID: 4016
		private ZoomableArea.Styles m_Styles;

		// Token: 0x02000384 RID: 900
		public enum YDirection
		{
			// Token: 0x04000FB2 RID: 4018
			Positive,
			// Token: 0x04000FB3 RID: 4019
			Negative
		}

		// Token: 0x02000385 RID: 901
		public class Styles
		{
			// Token: 0x06002601 RID: 9729 RVA: 0x0006BA98 File Offset: 0x00069C98
			public Styles(bool minimalGUI)
			{
				if (minimalGUI)
				{
					this.visualSliderWidth = 0f;
					this.sliderWidth = 15f;
				}
				else
				{
					this.visualSliderWidth = 15f;
					this.sliderWidth = 15f;
				}
			}

			// Token: 0x06002602 RID: 9730 RVA: 0x0006BAE8 File Offset: 0x00069CE8
			public void InitGUIStyles(bool minimalGUI, bool enableSliderZoom)
			{
				if (minimalGUI)
				{
					this.horizontalMinMaxScrollbarThumb = ((!enableSliderZoom) ? "MiniSliderhorizontal" : "MiniMinMaxSliderHorizontal");
					this.horizontalScrollbarLeftButton = GUIStyle.none;
					this.horizontalScrollbarRightButton = GUIStyle.none;
					this.horizontalScrollbar = GUIStyle.none;
					this.verticalMinMaxScrollbarThumb = ((!enableSliderZoom) ? "MiniSliderVertical" : "MiniMinMaxSlidervertical");
					this.verticalScrollbarUpButton = GUIStyle.none;
					this.verticalScrollbarDownButton = GUIStyle.none;
					this.verticalScrollbar = GUIStyle.none;
				}
				else
				{
					this.horizontalMinMaxScrollbarThumb = ((!enableSliderZoom) ? "horizontalscrollbarthumb" : "horizontalMinMaxScrollbarThumb");
					this.horizontalScrollbarLeftButton = "horizontalScrollbarLeftbutton";
					this.horizontalScrollbarRightButton = "horizontalScrollbarRightbutton";
					this.horizontalScrollbar = GUI.skin.horizontalScrollbar;
					this.verticalMinMaxScrollbarThumb = ((!enableSliderZoom) ? "verticalscrollbarthumb" : "verticalMinMaxScrollbarThumb");
					this.verticalScrollbarUpButton = "verticalScrollbarUpbutton";
					this.verticalScrollbarDownButton = "verticalScrollbarDownbutton";
					this.verticalScrollbar = GUI.skin.verticalScrollbar;
				}
			}

			// Token: 0x04000FB4 RID: 4020
			public GUIStyle horizontalScrollbar;

			// Token: 0x04000FB5 RID: 4021
			public GUIStyle horizontalMinMaxScrollbarThumb;

			// Token: 0x04000FB6 RID: 4022
			public GUIStyle horizontalScrollbarLeftButton;

			// Token: 0x04000FB7 RID: 4023
			public GUIStyle horizontalScrollbarRightButton;

			// Token: 0x04000FB8 RID: 4024
			public GUIStyle verticalScrollbar;

			// Token: 0x04000FB9 RID: 4025
			public GUIStyle verticalMinMaxScrollbarThumb;

			// Token: 0x04000FBA RID: 4026
			public GUIStyle verticalScrollbarUpButton;

			// Token: 0x04000FBB RID: 4027
			public GUIStyle verticalScrollbarDownButton;

			// Token: 0x04000FBC RID: 4028
			public float sliderWidth;

			// Token: 0x04000FBD RID: 4029
			public float visualSliderWidth;
		}
	}
}
