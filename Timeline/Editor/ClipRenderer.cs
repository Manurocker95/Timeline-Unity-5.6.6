using System;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace UnityEditor
{
	// Token: 0x02000053 RID: 83
	internal static class ClipRenderer
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x000193D0 File Offset: 0x000177D0
		private static void Initialize()
		{
			if (ClipRenderer.s_Quad == null)
			{
				ClipRenderer.s_Quad = new Mesh();
				ClipRenderer.s_Quad.hideFlags |= 52;
				ClipRenderer.s_Quad.name = "TimelineQuadMesh";
				Vector3[] vertices = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(1f, 0f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(0f, 1f, 0f)
				};
				Vector2[] uv = new Vector2[]
				{
					new Vector2(0f, 1f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f),
					new Vector2(0f, 0f)
				};
				int[] array = new int[]
				{
					0,
					1,
					2,
					0,
					2,
					3
				};
				Color32[] colors = new Color32[]
				{
					Color.white,
					Color.white,
					Color.white,
					Color.white
				};
				ClipRenderer.s_Quad.vertices = vertices;
				ClipRenderer.s_Quad.uv = uv;
				ClipRenderer.s_Quad.colors32 = colors;
				ClipRenderer.s_Quad.SetIndices(array, 0, 0);
			}
			if (ClipRenderer.s_BlendMaterial == null)
			{
				Shader shader = (Shader)EditorGUIUtility.LoadRequired("Editors/Timeline/DrawBlendShader.shader");
				ClipRenderer.s_BlendMaterial = new Material(shader);
			}
			if (ClipRenderer.s_ClipMaterial == null)
			{
				Shader shader2 = (Shader)EditorGUIUtility.LoadRequired("Editors/Timeline/ClipShader.shader");
				ClipRenderer.s_ClipMaterial = new Material(shader2);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00019614 File Offset: 0x00017A14
		public static void RenderTexture(Rect r, Texture mainTex, Texture mask, Color color, bool flipVertical)
		{
			ClipRenderer.Initialize();
			ClipRenderer.s_Vertices.Clear();
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMin, r.yMin, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMax, r.yMin, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMax, r.yMax, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMin, r.yMax, 0f));
			ClipRenderer.s_Quad.vertices = ClipRenderer.s_Vertices.ToArray();
			float num = 0f;
			float num2 = 1f;
			if (flipVertical)
			{
				num = 1f;
				num2 = 0f;
			}
			ClipRenderer.s_UVs.Clear();
			ClipRenderer.s_UVs.Add(new Vector2(0f, num2));
			ClipRenderer.s_UVs.Add(new Vector2(1f, num2));
			ClipRenderer.s_UVs.Add(new Vector2(1f, num));
			ClipRenderer.s_UVs.Add(new Vector2(0f, num));
			ClipRenderer.s_Quad.uv = ClipRenderer.s_UVs.ToArray();
			ClipRenderer.s_BlendMaterial.SetTexture("_MainTex", mainTex);
			ClipRenderer.s_BlendMaterial.SetTexture("_MaskTex", mask);
			ClipRenderer.s_BlendMaterial.SetColor("_Color", color);
			ClipRenderer.s_BlendMaterial.SetPass(0);
			Graphics.DrawMeshNow(ClipRenderer.s_Quad, Handles.matrix);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000197AC File Offset: 0x00017BAC
		public static void RenderTexture(Rect r, GUIStyle style, Color color)
		{
			ClipRenderer.Initialize();
			ClipRenderer.s_Vertices.Clear();
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMin, r.yMin, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMax, r.yMin, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMax, r.yMax, 0f));
			ClipRenderer.s_Vertices.Add(new Vector3(r.xMin, r.yMax, 0f));
			ClipRenderer.s_Quad.vertices = ClipRenderer.s_Vertices.ToArray();
			ClipRenderer.s_UVs.Clear();
			ClipRenderer.s_UVs.Add(new Vector2(0f, 1f));
			ClipRenderer.s_UVs.Add(new Vector2(1f, 1f));
			ClipRenderer.s_UVs.Add(new Vector2(1f, 0f));
			ClipRenderer.s_UVs.Add(new Vector2(0f, 0f));
			ClipRenderer.s_Quad.uv = ClipRenderer.s_UVs.ToArray();
			ClipRenderer.s_ClipMaterial.SetTexture("_MainTex", style.normal.background);
			ClipRenderer.s_ClipMaterial.SetColor("_Color", color);
			ClipRenderer.s_ClipMaterial.SetPass(0);
			Graphics.DrawMeshNow(ClipRenderer.s_Quad, Handles.matrix);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001992C File Offset: 0x00017D2C
		public static void RenderClip(Rect r, Color color)
		{
			ClipRenderer.RenderTexture(r, DirectorStyles.Instance.timelineClip, color);
		}

		// Token: 0x04000221 RID: 545
		private static Mesh s_Quad = null;

		// Token: 0x04000222 RID: 546
		private static Material s_BlendMaterial = null;

		// Token: 0x04000223 RID: 547
		private static Material s_ClipMaterial = null;

		// Token: 0x04000224 RID: 548
		private static List<Vector3> s_Vertices = new List<Vector3>(4);

		// Token: 0x04000225 RID: 549
		private static List<Vector2> s_UVs = new List<Vector2>(4);
	}
}
