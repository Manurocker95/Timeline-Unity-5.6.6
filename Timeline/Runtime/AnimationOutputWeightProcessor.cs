using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000022 RID: 34
	internal class AnimationOutputWeightProcessor : ITimelineEvaluateCallback
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00006126 File Offset: 0x00004326
		public AnimationOutputWeightProcessor(AnimationPlayableOutput output)
		{
			this.m_Output = output;
			this.FindMixers();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006148 File Offset: 0x00004348
		private void FindMixers()
		{
			this.m_Mixers.Clear();
			this.m_LayerMixer = PlayableHandle.Null;
			PlayableHandle sourcePlayable = this.m_Output.sourcePlayable;
			int sourceInputPort = this.m_Output.sourceInputPort;
			if (sourcePlayable.IsValid() && sourceInputPort >= 0 && sourceInputPort < sourcePlayable.inputCount)
			{
				PlayableHandle input = sourcePlayable.GetInput(sourceInputPort).GetInput(0);
				if (input.IsValid() && PlayableHandle.GetPlayableTypeOf(ref input) == typeof(AnimationLayerMixerPlayable))
				{
					this.m_LayerMixer = input;
					int inputCount = this.m_LayerMixer.inputCount;
					for (int i = 0; i < inputCount; i++)
					{
						this.FindMixers(this.m_LayerMixer, i, this.m_LayerMixer.GetInput(i));
					}
				}
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000622C File Offset: 0x0000442C
		private void FindMixers(PlayableHandle parent, int port, PlayableHandle node)
		{
			if (node.IsValid())
			{
				Type playableTypeOf = PlayableHandle.GetPlayableTypeOf(ref node);
				if (playableTypeOf == typeof(AnimationMixerPlayable) || playableTypeOf == typeof(AnimationLayerMixerPlayable))
				{
					int inputCount = node.inputCount;
					for (int i = 0; i < inputCount; i++)
					{
						this.FindMixers(node, i, node.GetInput(i));
					}
					this.m_Mixers.Add(new AnimationOutputWeightProcessor.WeightInfo
					{
						parentMixer = parent,
						mixer = node,
						port = port,
						modulate = (playableTypeOf == typeof(AnimationLayerMixerPlayable))
					});
				}
				else
				{
					int inputCount2 = node.inputCount;
					for (int j = 0; j < inputCount2; j++)
					{
						this.FindMixers(parent, port, node.GetInput(j));
					}
				}
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000631C File Offset: 0x0000451C
		public void Evaluate()
		{
			for (int i = 0; i < this.m_Mixers.Count; i++)
			{
				AnimationOutputWeightProcessor.WeightInfo weightInfo = this.m_Mixers[i];
				float num = (!weightInfo.modulate) ? 1f : weightInfo.parentMixer.GetInputWeight(weightInfo.port);
				weightInfo.parentMixer.SetInputWeight(weightInfo.port, num * WeightUtility.NormalizeMixer(weightInfo.mixer));
			}
			this.m_Output.weight = WeightUtility.NormalizeMixer(this.m_LayerMixer);
		}

		// Token: 0x0400009D RID: 157
		private AnimationPlayableOutput m_Output;

		// Token: 0x0400009E RID: 158
		private PlayableHandle m_LayerMixer;

		// Token: 0x0400009F RID: 159
		private readonly List<AnimationOutputWeightProcessor.WeightInfo> m_Mixers = new List<AnimationOutputWeightProcessor.WeightInfo>();

		// Token: 0x02000023 RID: 35
		private struct WeightInfo
		{
			// Token: 0x040000A0 RID: 160
			public PlayableHandle mixer;

			// Token: 0x040000A1 RID: 161
			public PlayableHandle parentMixer;

			// Token: 0x040000A2 RID: 162
			public int port;

			// Token: 0x040000A3 RID: 163
			public bool modulate;
		}
	}
}
