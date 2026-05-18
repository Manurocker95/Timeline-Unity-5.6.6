using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Playable used to control other PlayableDirector.</para>
	/// </summary>
	// Token: 0x02000031 RID: 49
	public class DirectorControlPlayable : ScriptPlayable
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00008348 File Offset: 0x00006548
		public override void PrepareFrame(FrameData info)
		{
			if (!(this.director == null) && this.director.isActiveAndEnabled && !(this.director.playableAsset == null))
			{
				this.UpdateTime();
				if (info.evaluationType == (FrameData.EvaluationType)0)
				{
					this.director.Evaluate();
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000083B4 File Offset: 0x000065B4
		public override void OnPlayStateChanged(FrameData info, PlayState newState)
		{
			if (this.director != null && this.director.playableAsset != null)
			{
				if (newState == (PlayState)1)
				{
					this.UpdateTime();
					this.director.Play();
				}
				else
				{
					this.director.Stop();
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008418 File Offset: 0x00006618
		private void UpdateTime()
		{
			double num = Math.Max(0.1, this.director.playableAsset.duration);
			DirectorWrapMode wrapMode = this.director.wrapMode;
			if (wrapMode != (DirectorWrapMode)0)
			{
				if (wrapMode != (DirectorWrapMode)1)
				{
					if (wrapMode == (DirectorWrapMode)2)
					{
						this.director.time = this.handle.time;
					}
				}
				else
				{
					this.director.time = Math.Max(0.0, this.handle.time % num);
				}
			}
			else
			{
				this.director.time = Math.Min(num, Math.Max(0.0, this.handle.time));
			}
		}

		/// <summary>
		///   <para>This is the DirectorPlayable to control.</para>
		/// </summary>
		// Token: 0x040000E0 RID: 224
		public PlayableDirector director;
	}
}
