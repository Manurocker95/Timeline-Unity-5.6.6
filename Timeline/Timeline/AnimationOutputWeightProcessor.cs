using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    internal class AnimationOutputWeightProcessor :
        ITimelineEvaluateCallback
    {
        public AnimationOutputWeightProcessor(
            AnimationPlayableOutput output)
        {
            m_Output = output;
            FindMixers();
        }

        private void FindMixers()
        {
            m_Mixers.Clear();
            m_LayerMixer = PlayableHandle.Null;

            PlayableHandle sourcePlayable =
                m_Output.sourcePlayable;

            int sourceInputPort =
                m_Output.sourceInputPort;

            if (!sourcePlayable.IsValid() ||
                sourceInputPort < 0 ||
                sourceInputPort >= sourcePlayable.inputCount)
            {
                return;
            }

            PlayableHandle node =
                sourcePlayable.GetInput(sourceInputPort);

            FindLayerMixer(node);
        }

        private void FindLayerMixer(PlayableHandle node)
        {
            if (!node.IsValid())
                return;

            if (LegacyPlayableRuntime.IsAnimationLayerMixer(node))
            {
                m_LayerMixer = node;

                for (int i = 0; i < node.inputCount; ++i)
                {
                    FindMixers(
                        node,
                        i,
                        node.GetInput(i));
                }

                return;
            }

            for (int i = 0; i < node.inputCount; ++i)
                FindLayerMixer(node.GetInput(i));
        }

        private void FindMixers(
            PlayableHandle parent,
            int port,
            PlayableHandle node)
        {
            if (!node.IsValid())
                return;

            Type playableType =
                PlayableHandle.GetPlayableTypeOf(ref node);

            bool isLayerMixer =
                LegacyPlayableRuntime.IsAnimationLayerMixer(node);

            bool isMixer =
                playableType == typeof(AnimationMixerPlayable) ||
                isLayerMixer;

            if (isMixer)
            {
                for (int i = 0; i < node.inputCount; ++i)
                {
                    FindMixers(
                        node,
                        i,
                        node.GetInput(i));
                }

                WeightInfo info = new WeightInfo();
                info.parentMixer = parent;
                info.mixer = node;
                info.port = port;
                info.modulate = isLayerMixer;

                m_Mixers.Add(info);
                return;
            }

            for (int i = 0; i < node.inputCount; ++i)
            {
                FindMixers(
                    parent,
                    port,
                    node.GetInput(i));
            }
        }

        public void Evaluate()
        {
            for (int i = 0; i < m_Mixers.Count; ++i)
            {
                WeightInfo info = m_Mixers[i];

                float parentWeight =
                    info.modulate
                        ? info.parentMixer.GetInputWeight(info.port)
                        : 1f;

                info.parentMixer.SetInputWeight(
                    info.port,
                    parentWeight *
                    WeightUtility.NormalizeMixer(info.mixer));
            }

            m_Output.weight =
                m_LayerMixer.IsValid()
                    ? WeightUtility.NormalizeMixer(m_LayerMixer)
                    : 1f;
        }

        private AnimationPlayableOutput m_Output;
        private PlayableHandle m_LayerMixer;

        private readonly List<WeightInfo> m_Mixers =
            new List<WeightInfo>();

        private struct WeightInfo
        {
            public PlayableHandle mixer;
            public PlayableHandle parentMixer;
            public int port;
            public bool modulate;
        }
    }
}
