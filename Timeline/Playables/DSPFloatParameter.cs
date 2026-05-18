using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000219 RID: 537
	
	public struct DSPFloatParameter
	{
		// Token: 0x0600232F RID: 9007 RVA: 0x0002882C File Offset: 0x00026A2C
		public DSPFloatParameter(string driverNameParam, string internalNameParam)
		{
			this.driverName = driverNameParam;
			this.internalName = internalNameParam;
		}

		// Token: 0x0400063D RID: 1597
		public string driverName;

		// Token: 0x0400063E RID: 1598
		public string internalName;
	}
}
