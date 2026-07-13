using System;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed PlayableDirector for the standalone Legacy Timeline runtime.
    /// </summary>
    public class PlayableDirector : MonoBehaviour, IExposedPropertyTable
    {
        [SerializeField]
        private PlayableAsset m_PlayableAsset;

        [SerializeField]
        private DirectorWrapMode m_WrapMode = DirectorWrapMode.Hold;

        [SerializeField]
        private DirectorUpdateMode m_TimeUpdateMode =
            DirectorUpdateMode.GameTime;

        [SerializeField]
        private double m_InitialTime;

        [SerializeField]
        private bool m_PlayOnAwake;

        private PlayableGraph m_Graph;
        private PlayableHandle m_RootPlayable;
        private PlayState m_State = PlayState.Paused;
        private double m_Time;
        private bool m_DeferredEvaluate;

        private readonly Dictionary<PropertyName, UnityEngine.Object>
            m_ReferenceValues =
                new Dictionary<PropertyName, UnityEngine.Object>();

        private readonly Dictionary<UnityEngine.Object, UnityEngine.Object>
            m_GenericBindings =
                new Dictionary<UnityEngine.Object, UnityEngine.Object>();

        public PlayState state
        {
            get { return m_State; }
        }

        public PlayableAsset playableAsset
        {
            get { return m_PlayableAsset; }
            set
            {
                if (m_PlayableAsset == value)
                    return;

                Stop();
                m_PlayableAsset = value;
            }
        }

        [Obsolete("Use wrapMode property instead")]
        public DirectorWrapMode extrapolationMode
        {
            get { return m_WrapMode; }
            set { m_WrapMode = value; }
        }

        public DirectorWrapMode wrapMode
        {
            get { return m_WrapMode; }
            set { m_WrapMode = value; }
        }

        public DirectorUpdateMode timeUpdateMode
        {
            get { return m_TimeUpdateMode; }
            set { m_TimeUpdateMode = value; }
        }

        public double time
        {
            get { return m_Time; }
            set
            {
                m_Time = Math.Max(0.0, value);

                if (m_RootPlayable.IsValid())
                    m_RootPlayable.time = m_Time;
            }
        }

        public double initialTime
        {
            get { return m_InitialTime; }
            set { m_InitialTime = Math.Max(0.0, value); }
        }

        public double duration
        {
            get
            {
                return m_PlayableAsset != null
                    ? m_PlayableAsset.duration
                    : 0.0;
            }
        }

        public PlayableGraph playableGraph
        {
            get { return m_Graph; }
        }

        private void Awake()
        {
            if (m_PlayOnAwake && m_PlayableAsset != null)
                Play();
        }

        private void Update()
        {
            if (m_DeferredEvaluate)
            {
                m_DeferredEvaluate = false;
                Evaluate();
            }
        }

        private void LateUpdate()
        {
            if (m_State != PlayState.Playing ||
                !m_Graph.IsValid() ||
                m_TimeUpdateMode == DirectorUpdateMode.Manual)
            {
                return;
            }

            float deltaTime =
                m_TimeUpdateMode == DirectorUpdateMode.UnscaledGameTime ||
                m_TimeUpdateMode == DirectorUpdateMode.DSPClock
                    ? Time.unscaledDeltaTime
                    : Time.deltaTime;

            Advance(deltaTime);
        }

        private void OnDisable()
        {
            if (m_State == PlayState.Playing)
                Pause();
        }

        private void OnDestroy()
        {
            DestroyGraph();
        }

        public void Evaluate()
        {
            if (!EnsureGraph())
                return;

            if (m_RootPlayable.IsValid())
                m_RootPlayable.time = m_Time;

            m_Graph.Evaluate(0f);
            ApplyBindings();
        }

        public void DeferredEvaluate()
        {
            m_DeferredEvaluate = true;
        }

        public void Play(PlayableAsset asset)
        {
            if (asset == null)
                throw new ArgumentNullException("asset");

            Play(asset, m_WrapMode);
        }

        public void Play(
            PlayableAsset asset,
            DirectorWrapMode mode)
        {
            if (asset == null)
                throw new ArgumentNullException("asset");

            playableAsset = asset;
            wrapMode = mode;
            Play();
        }

        public void Play()
        {
            if (!EnsureGraph())
                return;

            m_Time = Math.Max(0.0, m_InitialTime);

            if (m_RootPlayable.IsValid())
            {
                m_RootPlayable.duration =
                    Math.Max(0.0, duration);

                m_RootPlayable.time = m_Time;
            }

            ApplyBindings();
            m_Graph.Play();
            m_State = PlayState.Playing;
            m_Graph.Evaluate(0f);
        }

        public void Stop()
        {
            if (m_Graph.IsValid())
            {
                m_Graph.Stop();
                DestroyGraph();
            }

            m_State = PlayState.Paused;
            m_Time = 0.0;
        }

        public void Pause()
        {
            if (m_State != PlayState.Playing)
                return;

            if (m_Graph.IsValid())
                m_Graph.Stop();

            m_State = PlayState.Paused;
        }

        public void Resume()
        {
            if (!EnsureGraph())
                return;

            ApplyBindings();
            m_Graph.Play();
            m_State = PlayState.Playing;
        }

        public void SetReferenceValue(
            PropertyName id,
            UnityEngine.Object value)
        {
            m_ReferenceValues[id] = value;
        }

        public UnityEngine.Object GetReferenceValue(
            PropertyName id,
            out bool idValid)
        {
            UnityEngine.Object value;
            idValid = m_ReferenceValues.TryGetValue(id, out value);
            return value;
        }

        public void ClearReferenceValue(PropertyName id)
        {
            m_ReferenceValues.Remove(id);
        }

        public void SetGenericBinding(
            UnityEngine.Object key,
            UnityEngine.Object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (value == null)
                m_GenericBindings.Remove(key);
            else
                m_GenericBindings[key] = value;

            if (m_Graph.IsValid())
                ApplyBindings();
        }

        public UnityEngine.Object GetGenericBinding(
            UnityEngine.Object key)
        {
            UnityEngine.Object value;

            return key != null &&
                   m_GenericBindings.TryGetValue(key, out value)
                ? value
                : null;
        }

        private bool EnsureGraph()
        {
            if (m_Graph.IsValid())
                return true;

            if (m_PlayableAsset == null)
                return false;

            m_Graph = PlayableGraph.Create();
            m_Graph.resolver = this;

            m_RootPlayable =
                m_PlayableAsset.CreatePlayable(
                    m_Graph,
                    gameObject);

            if (!m_RootPlayable.IsValid())
            {
                DestroyGraph();
                return false;
            }

            m_RootPlayable.duration =
                Math.Max(0.0, duration);

            m_RootPlayable.time = m_Time;
            ApplyBindings();
            return true;
        }

        private void DestroyGraph()
        {
            if (m_Graph.IsValid())
                m_Graph.Destroy();

            m_Graph = default(PlayableGraph);
            m_RootPlayable = PlayableHandle.Null;
        }

        private void Advance(float deltaTime)
        {
            double assetDuration = duration;
            double nextTime = m_Time + deltaTime;

            if (assetDuration > 0.0 &&
                !Double.IsInfinity(assetDuration) &&
                nextTime >= assetDuration)
            {
                switch (m_WrapMode)
                {
                    case DirectorWrapMode.Loop:
                        nextTime %= assetDuration;
                        ResetGraphTime(nextTime);
                        m_Graph.Play();
                        break;

                    case DirectorWrapMode.Hold:
                        nextTime = assetDuration;
                        ResetGraphTime(nextTime);
                        m_Graph.Evaluate(0f);
                        m_Graph.Stop();
                        m_State = PlayState.Paused;
                        m_Time = nextTime;
                        return;

                    case DirectorWrapMode.None:
                        Stop();
                        return;
                }
            }

            m_Graph.Evaluate(deltaTime);
            m_Time = m_RootPlayable.IsValid()
                ? m_RootPlayable.time
                : nextTime;
        }

        private void ResetGraphTime(double value)
        {
            m_Time = value;

            if (m_RootPlayable.IsValid())
                m_RootPlayable.time = value;
        }

        private void ApplyBindings()
        {
            if (!m_Graph.IsValid())
                return;

            int animationCount =
                m_Graph.GetAnimationOutputCount();

            for (int i = 0; i < animationCount; ++i)
            {
                AnimationPlayableOutput output =
                    m_Graph.GetAnimationOutput(i);

                UnityEngine.Object key =
                    output.referenceObject;

                UnityEngine.Object binding =
                    GetGenericBinding(key);

                Animator animator = binding as Animator;

                if (animator == null)
                {
                    GameObject targetObject =
                        binding as GameObject;

                    if (targetObject != null)
                        animator =
                            targetObject.GetComponent<Animator>();
                }

                output.target = animator;

                Debug.Log(
                    "[PlayableDirector] Animation output " +
                    i +
                    " binding key=" +
                    (key != null ? key.name : "null") +
                    " target=" +
                    (animator != null ? animator.name : "null") +
                    " sourceValid=" +
                    output.sourcePlayable.IsValid() +
                    " weight=" +
                    output.weight.ToString("F3"));
            }

            int audioCount =
                LegacyPlayableRuntime.GetAudioOutputCount(m_Graph);

            for (int i = 0; i < audioCount; ++i)
            {
                PlayableOutput rawOutput;

                if (!LegacyPlayableRuntime.GetAudioOutput(
                    m_Graph,
                    i,
                    out rawOutput))
                {
                    continue;
                }

                UnityEngine.Object key =
                    LegacyPlayableRuntime
                        .GetOutputReferenceObject(rawOutput);

                UnityEngine.Object binding =
                    GetGenericBinding(key);

                AudioMixerGroup mixerGroup =
                    binding as AudioMixerGroup;

                AudioSource audioSource =
                    binding as AudioSource;

                if (audioSource != null)
                    mixerGroup =
                        audioSource.outputAudioMixerGroup;

                LegacyPlayableRuntime.SetAudioOutputTarget(
                    rawOutput,
                    mixerGroup);
            }
        }
    }
}
