using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003C RID: 60
	internal static class WeightUtility
	{
		// Token: 0x060001CB RID: 459 RVA: 0x00009350 File Offset: 0x00007550
		public static float NormalizeMixer(PlayableHandle mixer)
		{
			float result;
			if (!mixer.IsValid())
			{
				result = 0f;
			}
			else
			{
				int inputCount = mixer.inputCount;
				float num = 0f;
				for (int i = 0; i < inputCount; i++)
				{
					num += mixer.GetInputWeight(i);
				}
				if (num > Mathf.Epsilon && num < 1f)
				{
					for (int j = 0; j < inputCount; j++)
					{
						mixer.SetInputWeight(j, mixer.GetInputWeight(j) / num);
					}
				}
				result = Mathf.Clamp01(num);
			}
			return result;
		}
	}
}
