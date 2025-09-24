using System;

namespace UnityEditor.Timeline
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(AttributeTargets.Class)]
	internal class ClipSceneViewAttribute : Attribute
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x00018DF0 File Offset: 0x000171F0
		public ClipSceneViewAttribute(Type type)
		{
			this.typeInspected = type;
		}

		// Token: 0x0400021B RID: 539
		public Type typeInspected;
	}
}
