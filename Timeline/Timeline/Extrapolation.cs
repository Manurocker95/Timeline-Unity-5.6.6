using System;
using System.Linq;

namespace UnityEngine.Timeline
{
	// Token: 0x02000038 RID: 56
	public static class Extrapolation
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00008B54 File Offset: 0x00006D54
		internal static void CalculateExtrapolationTimes(this TrackAsset asset)
		{
			TimelineClip[] clips = asset.clips;
			if (clips != null && clips.Length != 0)
			{
				if (clips[0].SupportsExtrapolation())
				{
					TimelineClip[] array = (from x in clips
					orderby x.start
					select x).ToArray<TimelineClip>();
					if (array.Length > 0)
					{
						for (int i = 0; i < array.Length; i++)
						{
							double num = double.PositiveInfinity;
							for (int j = 0; j < array.Length; j++)
							{
								if (i != j)
								{
									double num2 = array[j].start - array[i].end;
									if (num2 >= -TimeUtility.kTimeEpsilon && num2 < num)
									{
										num = Math.Min(num, num2);
									}
									if (array[j].start <= array[i].end && array[j].end > array[i].end)
									{
										num = 0.0;
									}
								}
							}
							num = ((num > Extrapolation.kMinExtrapolationTime) ? num : 0.0);
							array[i].SetPostExtrapolationTime(num);
						}
						array[0].SetPreExtrapolationTime(Math.Max(0.0, array[0].start));
						for (int k = 1; k < array.Length; k++)
						{
							double num3 = 0.0;
							int num4 = -1;
							for (int l = 0; l < k; l++)
							{
								if (array[l].end > array[k].start)
								{
									num4 = -1;
									num3 = 0.0;
									break;
								}
								double num5 = array[k].start - array[l].end;
								if (num4 == -1 || num5 < num3)
								{
									num3 = num5;
									num4 = l;
								}
							}
							if (num4 >= 0)
							{
								if (array[num4].postExtrapolationMode != TimelineClip.ClipExtrapolation.None)
								{
									num3 = 0.0;
								}
							}
							num3 = ((num3 > Extrapolation.kMinExtrapolationTime) ? num3 : 0.0);
							array[k].SetPreExtrapolationTime(num3);
						}
					}
				}
			}
		}

		// Token: 0x040000E9 RID: 233
		public static readonly double kMinExtrapolationTime = TimeUtility.kTimeEpsilon * 1000.0;
	}
}
