using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Base class for all user-defined playables.</para>
	/// </summary>
	// Token: 0x020000EE RID: 238
	
	[Serializable]
	public abstract class ScriptPlayable : IPlayable, IScriptPlayable, ICloneable
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00016D18 File Offset: 0x00014F18
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x00016D34 File Offset: 0x00014F34
		public PlayableHandle playableHandle
		{
			get
			{
				return this.handle;
			}
			set
			{
				this.handle = value;
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00016D40 File Offset: 0x00014F40
		public static implicit operator PlayableHandle(ScriptPlayable b)
		{
			return b.handle;
		}

		/// <summary>
		///   <para>Is the playable a valid part of a valid PlayableGraph.</para>
		/// </summary>
		// Token: 0x06001128 RID: 4392 RVA: 0x00016D5C File Offset: 0x00014F5C
		public bool IsValid()
		{
			return this.handle.IsValid();
		}

		/// <summary>
		///   <para>Called when the PlayableGraph this Playable is owned by starts playing.</para>
		/// </summary>
		// Token: 0x06001129 RID: 4393 RVA: 0x00016D7C File Offset: 0x00014F7C
		public virtual void OnGraphStart()
		{
		}

		/// <summary>
		///   <para>Called when the PlayableGraph this Playable is owned by is stopped.</para>
		/// </summary>
		// Token: 0x0600112A RID: 4394 RVA: 0x00016D80 File Offset: 0x00014F80
		public virtual void OnGraphStop()
		{
		}

		/// <summary>
		///   <para>Called when the Playable is destroyed.</para>
		/// </summary>
		// Token: 0x0600112B RID: 4395 RVA: 0x00016D84 File Offset: 0x00014F84
		public virtual void OnDestroy()
		{
		}

		/// <summary>
		///   <para>Called during evaluation of the PlayableGraph.</para>
		/// </summary>
		/// <param name="info">Information about the current frame.</param>
		// Token: 0x0600112C RID: 4396 RVA: 0x00016D88 File Offset: 0x00014F88
		public virtual void PrepareFrame(FrameData info)
		{
		}

		/// <summary>
		///   <para>The ProcessFrame is the stage at which your Playable should do its work.</para>
		/// </summary>
		/// <param name="info">Information about the current frame.</param>
		/// <param name="playerData">Data that is set on the playable output userData.</param>
		// Token: 0x0600112D RID: 4397 RVA: 0x00016D8C File Offset: 0x00014F8C
		public virtual void ProcessFrame(FrameData info, object playerData)
		{
		}

		/// <summary>
		///   <para>Override this method to perform custom operations when the PlayState changes.</para>
		/// </summary>
		/// <param name="info">The current frame information.</param>
		/// <param name="newState">The new PlayState.</param>
		// Token: 0x0600112E RID: 4398 RVA: 0x00016D90 File Offset: 0x00014F90
		public virtual void OnPlayStateChanged(FrameData info, PlayState newState)
		{
		}

		/// <summary>
		///   <para>Clones a ScriptPlayable.</para>
		/// </summary>
		/// <returns>
		///   <para>The cloned object.</para>
		/// </returns>
		// Token: 0x0600112F RID: 4399 RVA: 0x00016D94 File Offset: 0x00014F94
		public virtual object Clone()
		{
			ScriptPlayable scriptPlayable = (ScriptPlayable)base.MemberwiseClone();
			scriptPlayable.handle = PlayableHandle.Null;
			return scriptPlayable;
		}

		/// <summary>
		///   <para>Returns the PlayableHandle for this playable.</para>
		/// </summary>
		// Token: 0x04000239 RID: 569
		public PlayableHandle handle;
	}
}
