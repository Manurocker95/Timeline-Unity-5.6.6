using System;

namespace UnityEngine.Timeline
{
	/// <summary>
	///   <para>Attribute to apply to a ScriptPlayable class or property to indicate that it not animatable.</para>
	/// </summary>
	// Token: 0x02000011 RID: 17
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class NotKeyableAttribute : Attribute
	{
	}
}
