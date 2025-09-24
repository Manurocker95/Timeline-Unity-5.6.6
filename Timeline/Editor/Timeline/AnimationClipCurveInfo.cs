using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Timeline
{
	// Token: 0x02000020 RID: 32
	internal class AnimationClipCurveInfo
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000DE74 File Offset: 0x0000C274
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000DE8F File Offset: 0x0000C28F
		public bool dirty
		{
			get
			{
				return this.m_CurveDirty;
			}
			set
			{
				this.m_CurveDirty = value;
				if (this.m_CurveDirty)
				{
					this.m_KeysDirty = true;
					if (this.m_groupings != null)
					{
						this.m_groupings.Clear();
					}
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000DEC4 File Offset: 0x0000C2C4
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000DEDE File Offset: 0x0000C2DE
		public int version { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000DEE8 File Offset: 0x0000C2E8
		public float[] keyTimes
		{
			get
			{
				if (this.m_KeysDirty || this.m_KeyTimes == null)
				{
					this.RebuildKeyCache();
				}
				return this.m_KeyTimes;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000DF24 File Offset: 0x0000C324
		public float[] GetCurveTimes(EditorCurveBinding curve)
		{
			return this.GetCurveTimes(new EditorCurveBinding[]
			{
				curve
			});
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000DF54 File Offset: 0x0000C354
		public float[] GetCurveTimes(EditorCurveBinding[] curves)
		{
			if (this.m_KeysDirty || this.m_KeyTimes == null)
			{
				this.RebuildKeyCache();
			}
			List<float> list = new List<float>();
			foreach (EditorCurveBinding key in curves)
			{
				if (this.m_individualBindinsKey.ContainsKey(key))
				{
					list.AddRange(this.m_individualBindinsKey[key]);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000DFDC File Offset: 0x0000C3DC
		private void RebuildKeyCache()
		{
			this.m_individualBindinsKey = new Dictionary<EditorCurveBinding, float[]>();
			List<float> list = (from z in this.curves.SelectMany((AnimationCurve y) => y.keys)
			select z.time).ToList<float>();
			for (int i = 0; i < this.objectCurves.Count; i++)
			{
				ObjectReferenceKeyframe[] source = this.objectCurves[i];
				list.AddRange(from x in source
				select x.time);
			}
			for (int j = 0; j < this.bindings.Count<EditorCurveBinding>(); j++)
			{
				this.m_individualBindinsKey.Add(this.bindings[j], (from k in this.curves[j].keys
				select k.time).Distinct<float>().ToArray<float>());
			}
			this.m_KeyTimes = (from x in list
			orderby x
			select x).Distinct<float>().ToArray<float>();
			this.m_KeysDirty = false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000E148 File Offset: 0x0000C548
		public void Update(AnimationClip clip)
		{
			List<EditorCurveBinding> list = new List<EditorCurveBinding>();
			foreach (EditorCurveBinding editorCurveBinding in AnimationUtility.GetCurveBindings(clip))
			{
				if (!editorCurveBinding.propertyName.Contains("LocalRotation.w"))
				{
					list.Add(RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(editorCurveBinding, clip));
				}
			}
			this.bindings = list.ToArray();
			this.curves = new AnimationCurve[this.bindings.Length];
			for (int j = 0; j < this.bindings.Length; j++)
			{
				this.curves[j] = AnimationUtility.GetEditorCurve(clip, this.bindings[j]);
			}
			this.objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
			this.objectCurves = new List<ObjectReferenceKeyframe[]>(this.objectBindings.Length);
			for (int k = 0; k < this.objectBindings.Length; k++)
			{
				this.objectCurves.Add(AnimationUtility.GetObjectReferenceCurve(clip, this.objectBindings[k]));
			}
			this.m_CurveDirty = false;
			this.m_KeysDirty = true;
			this.version++;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000E288 File Offset: 0x0000C688
		public bool GetBindingForCurve(AnimationCurve curve, ref EditorCurveBinding binding)
		{
			for (int i = 0; i < this.curves.Length; i++)
			{
				if (curve == this.curves[i])
				{
					binding = this.bindings[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000E2E8 File Offset: 0x0000C6E8
		public AnimationCurve GetCurveForBinding(EditorCurveBinding binding)
		{
			for (int i = 0; i < this.curves.Length; i++)
			{
				if (binding.Equals(this.bindings[i]))
				{
					return this.curves[i];
				}
			}
			return null;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000E350 File Offset: 0x0000C750
		public ObjectReferenceKeyframe[] GetObjectCurveForBinding(EditorCurveBinding binding)
		{
			ObjectReferenceKeyframe[] result;
			if (this.objectCurves == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < this.objectCurves.Count; i++)
				{
					if (binding.Equals(this.objectBindings[i]))
					{
						return this.objectCurves[i];
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000E3D4 File Offset: 0x0000C7D4
		public CurveBindingGroup GetGroupBinding(string groupID)
		{
			if (this.m_groupings == null)
			{
				this.m_groupings = new Dictionary<string, CurveBindingGroup>();
			}
			CurveBindingGroup curveBindingGroup = null;
			if (!this.m_groupings.TryGetValue(groupID, out curveBindingGroup))
			{
				curveBindingGroup = new CurveBindingGroup();
				curveBindingGroup.timeRange = new Vector2(float.MaxValue, float.MinValue);
				curveBindingGroup.valueRange = new Vector2(float.MaxValue, float.MinValue);
				List<CurveBindingPair> list = new List<CurveBindingPair>();
				for (int i = 0; i < this.bindings.Length; i++)
				{
					if (this.bindings[i].GetGroupID() == groupID)
					{
						list.Add(new CurveBindingPair
						{
							binding = this.bindings[i],
							curve = this.curves[i]
						});
						for (int j = 0; j < this.curves[i].keys.Length; j++)
						{
							Keyframe keyframe = this.curves[i].keys[j];
							curveBindingGroup.timeRange = new Vector2(Mathf.Min(keyframe.time, curveBindingGroup.timeRange.x), Mathf.Max(keyframe.time, curveBindingGroup.timeRange.y));
							curveBindingGroup.valueRange = new Vector2(Mathf.Min(keyframe.value, curveBindingGroup.valueRange.x), Mathf.Max(keyframe.value, curveBindingGroup.valueRange.y));
						}
					}
				}
				for (int k = 0; k < this.objectBindings.Length; k++)
				{
					if (this.objectBindings[k].GetGroupID() == groupID)
					{
						list.Add(new CurveBindingPair
						{
							binding = this.objectBindings[k],
							objectCurve = this.objectCurves[k]
						});
						for (int l = 0; l < this.objectCurves[k].Length; l++)
						{
							ObjectReferenceKeyframe objectReferenceKeyframe = this.objectCurves[k][l];
							curveBindingGroup.timeRange = new Vector2(Mathf.Min(objectReferenceKeyframe.time, curveBindingGroup.timeRange.x), Mathf.Max(objectReferenceKeyframe.time, curveBindingGroup.timeRange.y));
						}
					}
				}
				curveBindingGroup.curveBindingPairs = (from x in list
				orderby AnimationWindowUtility.GetComponentIndex(x.binding.propertyName)
				select x).ToArray<CurveBindingPair>();
				this.m_groupings.Add(groupID, curveBindingGroup);
			}
			return curveBindingGroup;
		}

		// Token: 0x0400014A RID: 330
		private bool m_CurveDirty = true;

		// Token: 0x0400014B RID: 331
		private bool m_KeysDirty = true;

		// Token: 0x0400014C RID: 332
		public AnimationCurve[] curves;

		// Token: 0x0400014D RID: 333
		public EditorCurveBinding[] bindings;

		// Token: 0x0400014E RID: 334
		public EditorCurveBinding[] objectBindings;

		// Token: 0x0400014F RID: 335
		public List<ObjectReferenceKeyframe[]> objectCurves;

		// Token: 0x04000150 RID: 336
		private Dictionary<string, CurveBindingGroup> m_groupings;

		// Token: 0x04000152 RID: 338
		private float[] m_KeyTimes;

		// Token: 0x04000153 RID: 339
		private Dictionary<EditorCurveBinding, float[]> m_individualBindinsKey;
	}
}
