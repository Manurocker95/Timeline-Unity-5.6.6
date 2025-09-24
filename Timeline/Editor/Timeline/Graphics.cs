using System;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x020000AC RID: 172
	internal static class Graphics
	{
		// Token: 0x06000606 RID: 1542 RVA: 0x0002AEF0 File Offset: 0x000292F0
		public static bool ShadowedButton(Rect rect, string text, GUIStyle style, Color shadowColor)
		{
			Rect rect2 = rect;
			rect2.xMin += 2f;
			rect2.yMin += 2f;
			style.normal.textColor = shadowColor;
			GUI.Label(rect2, text, style);
			return GUI.Button(rect, text, style);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002AF4C File Offset: 0x0002934C
		public static void ShadowLabel(Rect rect, string text, GUIStyle style, Color textColor, Color shadowColor)
		{
			GUIContent content = new GUIContent(text);
			Graphics.ShadowLabel(rect, content, style, textColor, shadowColor);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002AF6C File Offset: 0x0002936C
		public static void ShadowLabel(Rect rect, GUIContent content, GUIStyle style, Color textColor, Color shadowColor)
		{
			Rect rect2 = rect;
			rect2.xMin += 2f;
			rect2.yMin += 2f;
			style.normal.textColor = Color.black;
			GUI.Label(rect2, content, style);
			style.normal.textColor = textColor;
			GUI.Label(rect, content, style);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0002AFD0 File Offset: 0x000293D0
		public static void DrawOutlineRect(Rect rect, Color color)
		{
			Graphics.DrawLine(new Vector3(rect.x, rect.y, 0f), new Vector3(rect.x + rect.width, rect.y, 0f), color);
			Graphics.DrawLine(new Vector3(rect.x + rect.width, rect.y, 0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0f), color);
			Graphics.DrawLine(new Vector3(rect.x, rect.y, 0f), new Vector3(rect.x, rect.y + rect.height, 0f), color);
			Graphics.DrawLine(new Vector3(rect.x, rect.y + rect.height, 0f), new Vector3(rect.x + rect.width, rect.y + rect.height, 0f), color);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0002B0F8 File Offset: 0x000294F8
		public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
		{
			Color color2 = Handles.color;
			Handles.color = color;
			Handles.DrawLine(p1, p2);
			Handles.color = color2;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002B120 File Offset: 0x00029520
		public static void DrawLineAA(Vector3 p1, Vector3 p2, Color col)
		{
			Color color = Handles.color;
			Handles.color = col;
			Handles.DrawAAPolyLine(1f, new Vector3[]
			{
				p1,
				p2
			});
			Handles.color = color;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002B16A File Offset: 0x0002956A
		public static void DrawLineAA(float width, Vector3 p1, Vector3 p2, Color color)
		{
			Graphics.DrawAAPolyLine(width, new Vector3[]
			{
				p1,
				p2
			}, color);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002B194 File Offset: 0x00029594
		public static void DrawAAPolyLine(float width, Vector3[] points, Color color)
		{
			Color color2 = Handles.color;
			Handles.color = color;
			Handles.DrawAAPolyLine(width, points);
			Handles.color = color2;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002B1BC File Offset: 0x000295BC
		public static void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
		{
			HandleUtility.ApplyWireMaterial();
			GL.Begin(1);
			GL.Color(col);
			float num = Vector3.Distance(p1, p2);
			int num2 = Mathf.CeilToInt(num / segmentsLength);
			for (int i = 0; i < num2; i += 2)
			{
				GL.Vertex(Vector3.Lerp(p1, p2, (float)i * segmentsLength / num));
				GL.Vertex(Vector3.Lerp(p1, p2, (float)(i + 1) * segmentsLength / num));
			}
			GL.End();
		}

		// Token: 0x04000387 RID: 903
		private static Texture2D m_StaticRectTexture;

		// Token: 0x04000388 RID: 904
		private static GUIStyle m_StaticRectStyle;
	}
}
