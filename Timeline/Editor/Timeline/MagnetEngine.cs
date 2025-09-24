using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Timeline;

namespace UnityEditor.Timeline
{
	// Token: 0x0200007B RID: 123
	internal class MagnetEngine
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x0001F110 File Offset: 0x0001D510
		public MagnetEngine(TimelineWindow.TimelineState state, TimelineClip clip, OnAttractedEdge onAttractedEdgeCB)
		{
			this.m_Clip = clip;
			this.m_State = state;
			this.m_OnAttractedEdge = onAttractedEdgeCB;
			Rect timeAreaBounds = TimelineWindow.instance.timeAreaBounds;
			timeAreaBounds.height = float.MaxValue;
			List<IBounds> source = state.quadTree.ContainedBy(timeAreaBounds);
			List<TimelineClipGUI> list = (from c in source.OfType<TimelineClipGUI>()
			where c.clip != clip
			select c).ToList<TimelineClipGUI>();
			this.m_Magnets.Add(new MagnetEngine.MagnetInfo
			{
				time = 0.0,
				active = false
			});
			this.m_Magnets.Add(new MagnetEngine.MagnetInfo
			{
				time = state.time,
				active = false
			});
			foreach (TimelineClipGUI timelineClipGUI in list)
			{
				if (timelineClipGUI.clip != clip)
				{
					this.m_Magnets.Add(new MagnetEngine.MagnetInfo
					{
						time = timelineClipGUI.start,
						active = false
					});
					this.m_Magnets.Add(new MagnetEngine.MagnetInfo
					{
						time = timelineClipGUI.end,
						active = false
					});
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0001F2D8 File Offset: 0x0001D6D8
		public bool isAttracted
		{
			get
			{
				return this.m_ActiveMagnet != null;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001F2FC File Offset: 0x0001D6FC
		public void Snap(float pixelOffsetInPixels)
		{
			this.m_MagnetCoolDown -= Mathf.Abs(pixelOffsetInPixels);
			if (this.m_MagnetCoolDown <= 0f)
			{
				if (this.m_ActiveMagnet != null)
				{
					this.m_ExitForce += Mathf.Abs(pixelOffsetInPixels);
					if (this.m_ExitForce > this.k_MagnetInfluenceInPixels)
					{
						this.m_ExitForce = 0f;
						this.m_ActiveMagnet.active = false;
						this.m_ActiveMagnet = null;
						this.m_MagnetCoolDown = this.k_MagnetInfluenceInPixels + 2f;
					}
					else if (this.m_OnAttractedEdge != null)
					{
						this.m_OnAttractedEdge(this.m_Clip, AttractedEdge.None, this.m_ActiveMagnet.clipTime, this.m_ActiveMagnet.clipDuration);
					}
				}
				else
				{
					foreach (MagnetEngine.MagnetInfo magnetInfo in this.m_Magnets)
					{
						if (this.IsAttractedToMagnet(this.m_Clip.start, magnetInfo))
						{
							this.m_ActiveMagnet = magnetInfo;
							magnetInfo.active = true;
							if (this.m_OnAttractedEdge != null)
							{
								this.m_OnAttractedEdge(this.m_Clip, AttractedEdge.Left, magnetInfo.time, this.m_Clip.duration);
							}
							magnetInfo.clipTime = magnetInfo.time;
							magnetInfo.clipDuration = this.m_Clip.duration;
							break;
						}
						if (this.IsAttractedToMagnet(this.m_Clip.end, magnetInfo))
						{
							this.m_ActiveMagnet = magnetInfo;
							magnetInfo.active = true;
							if (this.m_OnAttractedEdge != null)
							{
								this.m_OnAttractedEdge(this.m_Clip, AttractedEdge.Right, magnetInfo.time, this.m_Clip.duration);
							}
							magnetInfo.clipTime = this.m_Clip.start;
							magnetInfo.clipDuration = this.m_Clip.duration;
							break;
						}
					}
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001F518 File Offset: 0x0001D918
		private bool IsAttractedToMagnet(double time, MagnetEngine.MagnetInfo magnet)
		{
			float num = this.m_State.TimeToPixel(time);
			float num2 = this.m_State.TimeToPixel(magnet.time);
			return num > num2 - this.k_MagnetInfluenceInPixels && num < num2 + this.k_MagnetInfluenceInPixels;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001F568 File Offset: 0x0001D968
		public bool IsSnappedAtTime(double time)
		{
			bool result;
			if (this.m_ActiveMagnet == null)
			{
				result = false;
			}
			else
			{
				float num = this.m_State.TimeToPixel(time);
				float num2 = this.m_State.TimeToPixel(this.m_ActiveMagnet.time);
				result = (num - num2 <= this.k_Epsilon);
			}
			return result;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001F5C4 File Offset: 0x0001D9C4
		public void OnGUI()
		{
			if (this.m_ActiveMagnet != null)
			{
				TimelineWindow instance = TimelineWindow.instance;
				Vector2 vector;
				vector..ctor(this.m_State.TimeToPixel(this.m_ActiveMagnet.time), instance.timeAreaBounds.yMax);
				Vector2 vector2;
				vector2..ctor(1f, this.m_State.windowHeight);
				EditorGUI.DrawRect(new Rect(vector, vector2), Color.white);
			}
		}

		// Token: 0x0400028B RID: 651
		private TimelineClip m_Clip;

		// Token: 0x0400028C RID: 652
		private TimelineWindow.TimelineState m_State;

		// Token: 0x0400028D RID: 653
		private List<MagnetEngine.MagnetInfo> m_Magnets = new List<MagnetEngine.MagnetInfo>();

		// Token: 0x0400028E RID: 654
		private float m_ExitForce = 0f;

		// Token: 0x0400028F RID: 655
		private MagnetEngine.MagnetInfo m_ActiveMagnet = null;

		// Token: 0x04000290 RID: 656
		private float m_MagnetCoolDown = 0f;

		// Token: 0x04000291 RID: 657
		private OnAttractedEdge m_OnAttractedEdge = null;

		// Token: 0x04000292 RID: 658
		private readonly float k_MagnetInfluenceInPixels = 10f;

		// Token: 0x04000293 RID: 659
		private readonly float k_Epsilon = 0.001f;

		// Token: 0x0200007C RID: 124
		private class MagnetInfo
		{
			// Token: 0x17000077 RID: 119
			// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001F640 File Offset: 0x0001DA40
			// (set) Token: 0x060003EA RID: 1002 RVA: 0x0001F65A File Offset: 0x0001DA5A
			public double time { get; set; }

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x060003EB RID: 1003 RVA: 0x0001F664 File Offset: 0x0001DA64
			// (set) Token: 0x060003EC RID: 1004 RVA: 0x0001F67E File Offset: 0x0001DA7E
			public bool active { get; set; }

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001F688 File Offset: 0x0001DA88
			// (set) Token: 0x060003EE RID: 1006 RVA: 0x0001F6A2 File Offset: 0x0001DAA2
			public double clipTime { get; set; }

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x060003EF RID: 1007 RVA: 0x0001F6AC File Offset: 0x0001DAAC
			// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0001F6C6 File Offset: 0x0001DAC6
			public double clipDuration { get; set; }
		}
	}
}
