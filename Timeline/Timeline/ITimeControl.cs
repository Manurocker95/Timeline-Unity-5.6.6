using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000032 RID: 50
	public interface ITimeControl
	{
		/// <summary>
		///   <para>Called each frame the Timeline clip is active.</para>
		/// </summary>
		/// <param name="time">The local time of the associated Timeline clip.</param>
		// Token: 0x06000197 RID: 407
		void SetTime(double time);

		/// <summary>
		///   <para>Called when the associated Timeline clip becomes active.</para>
		/// </summary>
		// Token: 0x06000198 RID: 408
		void OnControlTimeStart();

		/// <summary>
		///   <para>Called when the associated Timeline clip becomes deactivated.</para>
		/// </summary>
		// Token: 0x06000199 RID: 409
		void OnControlTimeStop();
	}
}
