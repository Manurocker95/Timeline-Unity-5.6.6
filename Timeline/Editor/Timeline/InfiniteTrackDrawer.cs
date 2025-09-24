using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x02000095 RID: 149
	internal class InfiniteTrackDrawer : TrackDrawer
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x00028F86 File Offset: 0x00027386
		public InfiniteTrackDrawer(IPropertyKeyDataSource dataSource)
		{
			this.m_DataSource = dataSource;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00028F98 File Offset: 0x00027398
		public bool CanDraw(TrackAsset track, TimelineWindow.TimelineState state)
		{
			float[] keys = this.m_DataSource.GetKeys();
			bool flag = track.clips.Length == 0;
			return keys != null || (state.IsArmedForRecord(track) && flag);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00028FE0 File Offset: 0x000273E0
		private void DrawRecordBackground(Rect trackRect)
		{
			DirectorStyles instance = DirectorStyles.Instance;
			EditorGUI.DrawRect(trackRect, instance.customSkin.colorInfiniteTrackBackgroundRecording);
			Graphics.ShadowLabel(trackRect, instance.Elipsify(DirectorStyles.Instance.recordingLabel.text, trackRect, instance.fontClip), instance.fontClip, Color.white, Color.black);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00029038 File Offset: 0x00027438
		public override bool DrawTrack(Rect trackRect, TrackAsset track, Vector2 visibleTime, ITimelineState state)
		{
			this.m_TrackRect = trackRect;
			float[] keys = this.m_DataSource.GetKeys();
			TimelineWindow.TimelineState timelineState = state as TimelineWindow.TimelineState;
			bool result;
			if (!this.CanDraw(track, timelineState))
			{
				result = true;
			}
			else
			{
				if (keys != this.m_Keys)
				{
					this.m_Keys = keys;
				}
				if (timelineState.recording && timelineState.IsArmedForRecord(track))
				{
					this.DrawRecordBackground(trackRect);
				}
				GUI.Box(trackRect, GUIContent.none, DirectorStyles.Instance.infiniteTrack);
				Rect rect = trackRect;
				rect.yMin = rect.yMax;
				rect.height = 15f;
				GUI.DrawTexture(rect, DirectorStyles.Instance.bottomShadow.normal.background, 0);
				if (this.m_Keys != null && this.m_Keys.Length > 0)
				{
					foreach (float key in this.m_Keys)
					{
						this.DrawKeyFrame(key, state as TimelineWindow.TimelineState);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00029150 File Offset: 0x00027550
		private void DrawKeyFrame(float key, TimelineWindow.TimelineState state)
		{
			float num = state.TimeToPixel((double)key);
			Rect rect;
			rect..ctor(num, this.m_TrackRect.yMin + 3f, 1f, this.m_TrackRect.height - 6f);
			if (this.m_TrackRect.Overlaps(rect))
			{
				float fixedWidth = DirectorStyles.Instance.keyframeStyle.fixedWidth;
				float fixedHeight = DirectorStyles.Instance.keyframeStyle.fixedHeight;
				Rect rect2 = rect;
				rect2.width = fixedWidth;
				rect2.height = fixedHeight;
				rect2.xMin -= fixedWidth / 2f;
				rect2.yMin = this.m_TrackRect.yMin + (this.m_TrackRect.height - fixedHeight) / 2f;
				GUI.Box(rect2, GUIContent.none, DirectorStyles.Instance.keyframeStyle);
				EditorGUI.DrawRect(rect, DirectorStyles.Instance.customSkin.colorInfiniteClipLine);
			}
		}

		// Token: 0x04000375 RID: 885
		private readonly IPropertyKeyDataSource m_DataSource;

		// Token: 0x04000376 RID: 886
		private Rect m_ClientRect;

		// Token: 0x04000377 RID: 887
		private float[] m_Keys;

		// Token: 0x04000378 RID: 888
		private Rect m_TrackRect;
	}
}
