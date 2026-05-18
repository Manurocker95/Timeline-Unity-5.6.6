using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Playable that manages the forward of calls to an object implementing ITimeControl.</para>
	/// </summary>
	// Token: 0x02000035 RID: 53
	public class TimeControlPlayable : ScriptPlayable
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00008937 File Offset: 0x00006B37
		public void Initialize(ITimeControl timeControl)
		{
			this.m_timeControl = timeControl;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008941 File Offset: 0x00006B41
		public override void PrepareFrame(FrameData info)
		{
			if (this.m_timeControl != null)
			{
				this.m_timeControl.SetTime(this.handle.time);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008968 File Offset: 0x00006B68
		public override void OnPlayStateChanged(FrameData info, PlayState newState)
		{
			if (this.m_timeControl != null)
			{
				if (newState == (PlayState)1)
				{
					if (!this.m_started)
					{
						this.m_timeControl.OnControlTimeStart();
						this.m_started = true;
					}
				}
				else if (this.m_started)
				{
					this.m_timeControl.OnControlTimeStop();
					this.m_started = false;
				}
			}
		}

		// Token: 0x040000E7 RID: 231
		private ITimeControl m_timeControl;

		// Token: 0x040000E8 RID: 232
		private bool m_started = false;
	}
}
