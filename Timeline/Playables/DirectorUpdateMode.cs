using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Defines what time source is used to update a Director graph.</para>
	/// </summary>
	// Token: 0x02000304 RID: 772
	public enum DirectorUpdateMode
	{
		/// <summary>
		///   <para>Update is based on DSP (Digital Sound Processing) clock. Use this for graphs that need to be synchronized with Audio.</para>
		/// </summary>
		// Token: 0x04000A5A RID: 2650
		DSPClock,
		/// <summary>
		///   <para>Update is based on Time.time. Use this for graphs that need to be synchronized on gameplay, and that need to be paused when the game is paused.</para>
		/// </summary>
		// Token: 0x04000A5B RID: 2651
		GameTime,
		/// <summary>
		///   <para>Update is based on Time.unscaledTime. Use this for graphs that need to be updated even when gameplay is paused. Example: Menus transitions need to be updated even when the game is paused.</para>
		/// </summary>
		// Token: 0x04000A5C RID: 2652
		UnscaledGameTime,
		/// <summary>
		///   <para>Update mode is manual. You need to manually call PlayerController.Tick with your own deltaTime. This can be useful for graphs that can be completely disconnected from the rest of the the game. Example: Localized Bullet time.</para>
		/// </summary>
		// Token: 0x04000A5D RID: 2653
		Manual
	}
}
