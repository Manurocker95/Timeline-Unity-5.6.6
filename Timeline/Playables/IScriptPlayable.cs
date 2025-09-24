using System;

namespace UnityEngine.Playables
{
	// Token: 0x020004B4 RID: 1204
	public interface IScriptPlayable
	{
		/// <summary>
		///   <para>Called when the PlayableGraph this ScriptPlayable is owned by starts playing.</para>
		/// </summary>
		// Token: 0x060038ED RID: 14573
		void OnGraphStart();

		/// <summary>
		///   <para>Called when the PlayableGraph this ScriptPlayable is owned by is stopped.</para>
		/// </summary>
		// Token: 0x060038EE RID: 14574
		void OnGraphStop();

		/// <summary>
		///   <para>Called during evaluation of the PlayableGraph.</para>
		/// </summary>
		/// <param name="info">Information about the current frame.</param>
		// Token: 0x060038EF RID: 14575
		void PrepareFrame(FrameData info);

		/// <summary>
		///   <para>The ProcessFrame is the stage at which your Playable should do its work.</para>
		/// </summary>
		/// <param name="info">Information about the current frame.</param>
		/// <param name="playerData">Data that is set on the playable output userData.</param>
		// Token: 0x060038F0 RID: 14576
		void ProcessFrame(FrameData info, object playerData);

		/// <summary>
		///   <para>Override this method to perform custom operations when the PlayState changes.</para>
		/// </summary>
		/// <param name="info">The current frame information.</param>
		/// <param name="newState">The new PlayState.</param>
		// Token: 0x060038F1 RID: 14577
		void OnPlayStateChanged(FrameData info, PlayState newState);
	}
}
