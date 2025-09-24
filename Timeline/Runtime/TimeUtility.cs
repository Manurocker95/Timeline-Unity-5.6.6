using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003B RID: 59
	internal static class TimeUtility
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00008DCE File Offset: 0x00006FCE
		private static void ValidateFrameRate(double frameRate)
		{
			if (frameRate <= TimeUtility.kTimeEpsilon)
			{
				throw new ArgumentException("frame rate cannot be 0 or negative");
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008DE8 File Offset: 0x00006FE8
		public static int ToFrames(double time, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			double num = TimeUtility.kTimeEpsilon * time;
			int result;
			if (time < 0.0)
			{
				result = (int)Math.Ceiling(time * frameRate + num);
			}
			else
			{
				result = (int)Math.Floor(time * frameRate + num);
			}
			return result;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008E38 File Offset: 0x00007038
		public static double ToExactFrames(double time, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return time * frameRate;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008E58 File Offset: 0x00007058
		public static double FromFrames(int frames, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return (double)frames / frameRate;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008E78 File Offset: 0x00007078
		public static double FromFrames(double frames, double frameRate)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			return frames / frameRate;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008E98 File Offset: 0x00007098
		public static bool OnFrameBoundary(double time, double frameRate)
		{
			return TimeUtility.OnFrameBoundary(time, frameRate, Math.Max(time, 1.0) * frameRate * TimeUtility.kTimeEpsilon);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008ECC File Offset: 0x000070CC
		public static bool OnFrameBoundary(double time, double frameRate, double epsilon)
		{
			TimeUtility.ValidateFrameRate(frameRate);
			double num = TimeUtility.ToExactFrames(time, frameRate);
			double num2 = Math.Round(num);
			return Math.Abs(num - num2) < epsilon;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008F04 File Offset: 0x00007104
		public static string TimeAsFrames(double timeValue, double frameRate, string format = "F2")
		{
			string result;
			if (TimeUtility.OnFrameBoundary(timeValue, frameRate))
			{
				result = TimeUtility.ToFrames(timeValue, frameRate).ToString();
			}
			else
			{
				result = TimeUtility.ToExactFrames(timeValue, frameRate).ToString(format);
			}
			return result;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008F50 File Offset: 0x00007150
		public static string TimeAsTimeCode(double timeValue, double frameRate, string format = "F2")
		{
			TimeUtility.ValidateFrameRate(frameRate);
			int num = (int)Math.Abs(timeValue);
			int num2 = num / 3600;
			int num3 = num % 3600 / 60;
			int num4 = num % 60;
			string str = (timeValue >= 0.0) ? string.Empty : "-";
			string str2;
			if (num2 > 0)
			{
				str2 = string.Concat(new string[]
				{
					num2.ToString(),
					":",
					num3.ToString("D2"),
					":",
					num4.ToString("D2")
				});
			}
			else if (num3 > 0)
			{
				str2 = num3.ToString() + ":" + num4.ToString("D2");
			}
			else
			{
				str2 = num4.ToString();
			}
			int totalWidth = (int)Math.Floor(Math.Log10(frameRate) + 1.0);
			string text = (TimeUtility.ToFrames(timeValue, frameRate) - TimeUtility.ToFrames((double)num, frameRate)).ToString().PadLeft(totalWidth, '0');
			if (!TimeUtility.OnFrameBoundary(timeValue, frameRate))
			{
				string text2 = TimeUtility.ToExactFrames(timeValue, frameRate).ToString(format);
				int num5 = text2.IndexOf('.');
				if (num5 >= 0)
				{
					text = text + " [" + text2.Substring(num5) + "]";
				}
			}
			return str + str2 + ":" + text;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000090E8 File Offset: 0x000072E8
		public static double ParseTimeCode(string timeCode, double frameRate, double defaultValue)
		{
			timeCode = new string((from c in timeCode
			where !char.IsWhiteSpace(c)
			select c).ToArray<char>());
			string[] array = timeCode.Split(new char[]
			{
				':'
			});
			double result;
			if (array.Length == 0 || array.Length > 4)
			{
				result = defaultValue;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				double num3 = 0.0;
				double num4 = 0.0;
				try
				{
					if (Regex.Match(array.Last<string>(), "^\\d+\\.\\d+$").Success)
					{
						num3 = double.Parse(array.Last<string>());
						if (array.Length > 3)
						{
							return defaultValue;
						}
						if (array.Length > 1)
						{
							num2 = int.Parse(array[array.Length - 2]);
						}
						if (array.Length > 2)
						{
							num = int.Parse(array[array.Length - 3]);
						}
					}
					else
					{
						if (Regex.Match(array.Last<string>(), "^\\d+\\[\\.\\d+\\]$").Success)
						{
							string s = new string((from c in array.Last<string>()
							where c != '[' && c != ']'
							select c).ToArray<char>());
							num4 = double.Parse(s);
						}
						else
						{
							if (!Regex.Match(array.Last<string>(), "^\\d*$").Success)
							{
								return defaultValue;
							}
							num4 = (double)int.Parse(array.Last<string>());
						}
						if (array.Length > 1)
						{
							num3 = (double)int.Parse(array[array.Length - 2]);
						}
						if (array.Length > 2)
						{
							num2 = int.Parse(array[array.Length - 3]);
						}
						if (array.Length > 3)
						{
							num = int.Parse(array[array.Length - 4]);
						}
					}
				}
				catch (FormatException)
				{
					return defaultValue;
				}
				result = num4 / frameRate + num3 + (double)(num2 * 60) + (double)(num * 3600);
			}
			return result;
		}

		// Token: 0x040000EB RID: 235
		public static readonly double kTimeEpsilon = 1E-14;
	}
}
