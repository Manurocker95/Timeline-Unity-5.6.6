using System;
using System.Collections.Generic;

namespace UnityEditor.Timeline
{
	// Token: 0x02000094 RID: 148
	internal interface IPropertyKeyDataSource
	{
		// Token: 0x06000597 RID: 1431
		float[] GetKeys();

		// Token: 0x06000598 RID: 1432
		Dictionary<float, string> GetDescriptions();
	}
}
