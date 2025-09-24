using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline.Utilities
{
	// Token: 0x020000AF RID: 175
	internal class KeyTraverser
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0002B2CB File Offset: 0x000296CB
		public KeyTraverser(TimelineAsset timeline, float epsilon)
		{
			this.m_Asset = timeline;
			this.m_Epsilon = epsilon;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0002B2F8 File Offset: 0x000296F8
		public int lastIndex
		{
			get
			{
				return this.m_LastIndex;
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0002B314 File Offset: 0x00029714
		public static IEnumerable<float> GetClipKeyTimes(TimelineClip clip)
		{
			IEnumerable<float> result;
			if (clip == null || clip.animationClip == null || clip.animationClip.empty)
			{
				result = new float[0];
			}
			else
			{
				result = from k in AnimationClipCurveCache.Instance.GetCurveInfo(clip.animationClip).keyTimes
				select (float)(clip.start + ((double)k - clip.clipIn) / clip.timeScale) into k
				where (double)k >= clip.start && (double)k <= clip.end
				select k;
			}
			return result;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002B3B4 File Offset: 0x000297B4
		public static IEnumerable<float> GetTrackKeyTimes(AnimationTrack track)
		{
			if (track != null)
			{
				if (track.inClipMode)
				{
					return (from c in track.clips
					where c.recordable
					select c).SelectMany((TimelineClip x) => KeyTraverser.GetClipKeyTimes(x));
				}
				if (track.animClip != null && !track.animClip.empty)
				{
					return AnimationClipCurveCache.Instance.GetCurveInfo(track.animClip).keyTimes;
				}
			}
			return new float[0];
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002B474 File Offset: 0x00029874
		private static int CalcAnimClipHash(TrackAsset asset)
		{
			int num = 0;
			if (asset != null)
			{
				AnimationTrack animationTrack = asset as AnimationTrack;
				if (animationTrack != null)
				{
					for (int num2 = 0; num2 != animationTrack.clips.Length; num2++)
					{
						num ^= animationTrack.clips[num2].Hash();
					}
				}
				for (int num3 = 0; num3 != asset.subTracks.Count; num3++)
				{
					num ^= KeyTraverser.CalcAnimClipHash(asset.subTracks[num3]);
				}
			}
			return num;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0002B510 File Offset: 0x00029910
		internal static int CalcAnimClipHash(TimelineAsset asset)
		{
			int num = 0;
			foreach (TrackAsset asset2 in asset.tracks)
			{
				num ^= KeyTraverser.CalcAnimClipHash(asset2);
			}
			return num;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002B57C File Offset: 0x0002997C
		private void RebuildKeyCache()
		{
			this.m_KeyCache = (from x in (from x in this.m_Asset.flattenedTracks
			where x as AnimationTrack != null
			select x).Cast<AnimationTrack>().SelectMany((AnimationTrack t) => KeyTraverser.GetTrackKeyTimes(t))
			orderby x
			select x).ToArray<float>();
			if (this.m_KeyCache.Length > 0)
			{
				float[] array = new float[this.m_KeyCache.Length];
				array[0] = this.m_KeyCache[0];
				int num = 0;
				for (int i = 1; i < this.m_KeyCache.Length; i++)
				{
					if (this.m_KeyCache[i] - array[num] > this.m_Epsilon)
					{
						num++;
						array[num] = this.m_KeyCache[i];
					}
				}
				this.m_KeyCache = array;
				Array.Resize<float>(ref this.m_KeyCache, num + 1);
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0002B690 File Offset: 0x00029A90
		private void CheckCache(int dirtyStamp)
		{
			int num = KeyTraverser.CalcAnimClipHash(this.m_Asset);
			if (dirtyStamp != this.m_DirtyStamp || num != this.m_LastHash)
			{
				this.RebuildKeyCache();
				this.m_DirtyStamp = dirtyStamp;
				this.m_LastHash = num;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0002B6D8 File Offset: 0x00029AD8
		public float GetNextKey(float key, int dirtyStamp)
		{
			this.CheckCache(dirtyStamp);
			if (this.m_KeyCache.Length > 0)
			{
				if (key < this.m_KeyCache.Last<float>() - this.m_Epsilon)
				{
					if (key > this.m_KeyCache[0] - this.m_Epsilon)
					{
						float num = key + this.m_Epsilon;
						int num2 = this.m_KeyCache.Length - 1;
						int num3 = 0;
						while (num2 - num3 > 1)
						{
							int num4 = (num3 + num2) / 2;
							if (num > this.m_KeyCache[num4])
							{
								num3 = num4;
							}
							else
							{
								num2 = num4;
							}
						}
						this.m_LastIndex = num2;
						return this.m_KeyCache[num2];
					}
					this.m_LastIndex = 0;
					return this.m_KeyCache[0];
				}
				else if (key < this.m_KeyCache.Last<float>() + this.m_Epsilon)
				{
					this.m_LastIndex = this.m_KeyCache.Length - 1;
					return Mathf.Max(key, this.m_KeyCache.Last<float>());
				}
			}
			this.m_LastIndex = -1;
			return key;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0002B7F0 File Offset: 0x00029BF0
		public float GetPrevKey(float key, int dirtyStamp)
		{
			this.CheckCache(dirtyStamp);
			if (this.m_KeyCache.Length > 0)
			{
				if (key > this.m_KeyCache[0] + this.m_Epsilon)
				{
					if (key < this.m_KeyCache.Last<float>() + this.m_Epsilon)
					{
						float num = key - this.m_Epsilon;
						int num2 = this.m_KeyCache.Length - 1;
						int num3 = 0;
						while (num2 - num3 > 1)
						{
							int num4 = (num3 + num2) / 2;
							if (num < this.m_KeyCache[num4])
							{
								num2 = num4;
							}
							else
							{
								num3 = num4;
							}
						}
						this.m_LastIndex = num3;
						return this.m_KeyCache[num3];
					}
					this.m_LastIndex = this.m_KeyCache.Length - 1;
					return this.m_KeyCache.Last<float>();
				}
				else if (key >= this.m_KeyCache[0] - this.m_Epsilon)
				{
					this.m_LastIndex = 0;
					return Mathf.Min(key, this.m_KeyCache[0]);
				}
			}
			this.m_LastIndex = -1;
			return key;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0002B904 File Offset: 0x00029D04
		public int GetKeyCount(int dirtyStamp)
		{
			this.CheckCache(dirtyStamp);
			return this.m_KeyCache.Length;
		}

		// Token: 0x0400038B RID: 907
		private float[] m_KeyCache;

		// Token: 0x0400038C RID: 908
		private int m_DirtyStamp = -1;

		// Token: 0x0400038D RID: 909
		private int m_LastHash = -1;

		// Token: 0x0400038E RID: 910
		private readonly TimelineAsset m_Asset;

		// Token: 0x0400038F RID: 911
		private readonly float m_Epsilon;

		// Token: 0x04000390 RID: 912
		private int m_LastIndex = -1;
	}
}
