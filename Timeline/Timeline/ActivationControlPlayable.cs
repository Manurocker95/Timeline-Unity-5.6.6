using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Playable that controls activation (active state) of a GameObject.</para>
	/// </summary>
	// Token: 0x0200002D RID: 45
	public class ActivationControlPlayable : ScriptPlayable
	{
		// Token: 0x06000181 RID: 385 RVA: 0x0000815E File Offset: 0x0000635E
		public override void OnPlayStateChanged(FrameData info, PlayState newState)
		{
			if (!(this.gameObject == null))
			{
				this.gameObject.SetActive(newState == (PlayState)1);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008186 File Offset: 0x00006386
		public override void ProcessFrame(FrameData info, object userData)
		{
			if (this.gameObject != null)
			{
				this.gameObject.SetActive(true);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000081A6 File Offset: 0x000063A6
		public override void OnGraphStart()
		{
			if (this.gameObject != null)
			{
				if (this.m_InitialState == ActivationControlPlayable.InitialState.Unset)
				{
					this.m_InitialState = ((!this.gameObject.activeSelf) ? ActivationControlPlayable.InitialState.Inactive : ActivationControlPlayable.InitialState.Active);
				}
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000081E4 File Offset: 0x000063E4
		public override void OnGraphStop()
		{
			if (!(this.gameObject == null) && this.m_InitialState != ActivationControlPlayable.InitialState.Unset)
			{
				ActivationControlPlayable.PostPlaybackState postPlaybackState = this.postPlayback;
				if (postPlaybackState != ActivationControlPlayable.PostPlaybackState.Active)
				{
					if (postPlaybackState != ActivationControlPlayable.PostPlaybackState.Inactive)
					{
						if (postPlaybackState == ActivationControlPlayable.PostPlaybackState.Revert)
						{
							this.gameObject.SetActive(this.m_InitialState == ActivationControlPlayable.InitialState.Active);
						}
					}
					else
					{
						this.gameObject.SetActive(false);
					}
				}
				else
				{
					this.gameObject.SetActive(true);
				}
			}
		}

		/// <summary>
		///   <para>GameObject to control it active state.</para>
		/// </summary>
		// Token: 0x040000D4 RID: 212
		public GameObject gameObject = null;

		/// <summary>
		///   <para>Flag that determine what is the active state of the GameObject when the Playable Graph Stop.</para>
		/// </summary>
		// Token: 0x040000D5 RID: 213
		public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

		// Token: 0x040000D6 RID: 214
		private ActivationControlPlayable.InitialState m_InitialState;

		/// <summary>
		///   <para>The active state of a GameObject when a Playable Graph stops.</para>
		/// </summary>
		// Token: 0x0200002E RID: 46
		public enum PostPlaybackState
		{
			/// <summary>
			///   <para>Set the GameObject Active when the Playable Graph stops.</para>
			/// </summary>
			// Token: 0x040000D8 RID: 216
			Active,
			/// <summary>
			///   <para>Set the GameObject Inactive when the Playable Graph stops.</para>
			/// </summary>
			// Token: 0x040000D9 RID: 217
			Inactive,
			/// <summary>
			///   <para>Set the GameObject in the same state as before the Playable Graph starts.</para>
			/// </summary>
			// Token: 0x040000DA RID: 218
			Revert
		}

		// Token: 0x0200002F RID: 47
		private enum InitialState
		{
			// Token: 0x040000DC RID: 220
			Unset,
			// Token: 0x040000DD RID: 221
			Active,
			// Token: 0x040000DE RID: 222
			Inactive
		}
	}
}
