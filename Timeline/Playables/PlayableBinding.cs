using System;

namespace UnityEngine.Playables
{
	/// <summary>
	///   <para>Struct that holds information regarding an output of a PlayableAsset.</para>
	/// </summary>
	// Token: 0x020004B5 RID: 1205
	public struct PlayableBinding
	{
		/// <summary>
		///   <para>A constant to represent a PlayableAsset has no bindings.</para>
		/// </summary>
		// Token: 0x04001121 RID: 4385
		public static readonly PlayableBinding[] None = new PlayableBinding[0];

		/// <summary>
		///   <para>The default duration used when a PlayableOutput has no fixed duration.</para>
		/// </summary>
		// Token: 0x04001122 RID: 4386
		public static readonly double DefaultDuration = double.PositiveInfinity;

		/// <summary>
		///   <para>The name of the output or input stream.</para>
		/// </summary>
		// Token: 0x04001123 RID: 4387
		public string streamName;

		/// <summary>
		///   <para>The type of the output or input stream.</para>
		/// </summary>
		// Token: 0x04001124 RID: 4388
		public DirectorStreamType streamType;

		/// <summary>
		///   <para>A reference to a UnityEngine.Object that acts a key for this binding.</para>
		/// </summary>
		// Token: 0x04001125 RID: 4389
		public Object sourceObject;

		/// <summary>
		///   <para>When the StreamType is set to None, a binding can be represented using System.Type.</para>
		/// </summary>
		// Token: 0x04001126 RID: 4390
		public Type sourceBindingType;
	}
}
