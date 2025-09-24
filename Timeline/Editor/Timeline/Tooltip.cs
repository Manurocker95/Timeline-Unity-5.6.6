using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000013 RID: 19
	internal class Tooltip
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00009BD8 File Offset: 0x00007FD8
		public Tooltip(GUIStyle theStyle, GUIStyle font)
		{
			this.style = theStyle;
			this.m_Font = font;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00009C28 File Offset: 0x00008028
		public Tooltip()
		{
			this.style = null;
			this.m_Font = null;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00009C78 File Offset: 0x00008078
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00009C92 File Offset: 0x00008092
		public GUIStyle style { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00009C9C File Offset: 0x0000809C
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00009CB6 File Offset: 0x000080B6
		public string text { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00009CC0 File Offset: 0x000080C0
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00009D29 File Offset: 0x00008129
		public GUIStyle font
		{
			get
			{
				GUIStyle result;
				if (this.m_Font != null)
				{
					result = this.m_Font;
				}
				else if (this.style != null)
				{
					result = this.style;
				}
				else
				{
					this.m_Font = new GUIStyle();
					this.m_Font.font = EditorStyles.label.font;
					result = this.m_Font;
				}
				return result;
			}
			set
			{
				this.m_Font = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00009D34 File Offset: 0x00008134
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00009D4F File Offset: 0x0000814F
		public float pad
		{
			get
			{
				return this.m_Pad;
			}
			set
			{
				this.m_Pad = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00009D5C File Offset: 0x0000815C
		private GUIContent textContent
		{
			get
			{
				if (this.m_TextContent == null)
				{
					this.m_TextContent = new GUIContent();
				}
				this.m_TextContent.text = this.text;
				return this.m_TextContent;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00009DA0 File Offset: 0x000081A0
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00009DBB File Offset: 0x000081BB
		public Color foreColor
		{
			get
			{
				return this.m_ForeColor;
			}
			set
			{
				this.m_ForeColor = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00009DC8 File Offset: 0x000081C8
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00009E2C File Offset: 0x0000822C
		public Rect bounds
		{
			get
			{
				Vector2 vector = this.font.CalcSize(this.textContent);
				this.m_Bounds.width = vector.x + 2f * this.pad;
				this.m_Bounds.height = vector.y + 2f;
				return this.m_Bounds;
			}
			set
			{
				this.m_Bounds = value;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00009E38 File Offset: 0x00008238
		public void Draw()
		{
			if (!string.IsNullOrEmpty(this.text))
			{
				if (this.style != null)
				{
					using (new GUIColorOverride(DirectorStyles.Instance.customSkin.colorTooltipBackground))
					{
						GUI.Label(this.bounds, GUIContent.none, this.style);
					}
				}
				Rect bounds = this.bounds;
				bounds.x += this.pad;
				bounds.width -= this.pad;
				using (new GUIColorOverride(this.foreColor))
				{
					GUI.Label(bounds, this.textContent, this.font);
				}
			}
		}

		// Token: 0x04000120 RID: 288
		private string m_Text;

		// Token: 0x04000122 RID: 290
		private GUIStyle m_Font;

		// Token: 0x04000123 RID: 291
		private float m_Pad = 4f;

		// Token: 0x04000124 RID: 292
		private GUIContent m_TextContent = null;

		// Token: 0x04000125 RID: 293
		private Color m_ForeColor = Color.white;

		// Token: 0x04000126 RID: 294
		private Rect m_Bounds = default(Rect);
	}
}
