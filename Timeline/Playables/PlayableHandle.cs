using System;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Managed handle representing a Playable created in a managed PlayableGraph.
    /// This implementation has no dependency on Unity's native Playables runtime.
    /// </summary>
    public struct PlayableHandle
    {
        public T GetObject<T>() where T : IPlayable
        {
            if (!IsValid())
                return default(T);

            object instance = GetScriptInstance(ref this);
            if (instance != null)
                return (T)instance;

            Type playableType = GetPlayableTypeOf(ref this);
            if (playableType == null || !typeof(T).IsAssignableFrom(playableType))
                playableType = typeof(T);

            object created = Activator.CreateInstance(playableType);
            T playable = (T)created;
            playable.playableHandle = this;
            SetScriptInstance(ref this, playable);
            return playable;
        }

        private static object GetScriptInstance(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetScriptInstance(playable);
        }

        private static void SetScriptInstance(
            ref PlayableHandle playable,
            object scriptInstance)
        {
            LegacyPlayableRuntime.SetScriptInstance(playable, scriptInstance);
        }

        public bool IsValid()
        {
            return IsValidInternal(ref this);
        }

        private static bool IsValidInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.IsPlayableValid(playable);
        }

        internal static Type GetPlayableTypeOf(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetPlayableType(playable);
        }

        internal static void SetPlayableTypeOf(
            ref PlayableHandle playable,
            Type playableType)
        {
            LegacyPlayableRuntime.SetPlayableType(playable, playableType);
        }

        public static PlayableHandle Null
        {
            get
            {
                PlayableHandle handle = default(PlayableHandle);
                handle.m_Handle = IntPtr.Zero;
                handle.m_Version = 0;
                return handle;
            }
        }

        public PlayableGraph graph
        {
            get
            {
                PlayableGraph result = default(PlayableGraph);
                GetGraphInternal(ref this, ref result);
                return result;
            }
        }

        public int inputCount
        {
            get { return GetInputCountInternal(ref this); }
            set { SetInputCountInternal(ref this, value); }
        }

        public int outputCount
        {
            get { return GetOutputCountInternal(ref this); }
            set { SetOutputCountInternal(ref this, value); }
        }

        public PlayState playState
        {
            get { return GetPlayStateInternal(ref this); }
            set { SetPlayStateInternal(ref this, value); }
        }

        public double speed
        {
            get { return GetSpeedInternal(ref this); }
            set { SetSpeedInternal(ref this, value); }
        }

        public double time
        {
            get { return GetTimeInternal(ref this); }
            set { SetTimeInternal(ref this, value); }
        }

        public bool isDone
        {
            get { return InternalGetDone(ref this); }
            set { InternalSetDone(ref this, value); }
        }

        public bool propagateSetTime
        {
            get { return InternalGetPropagateSetTime(ref this); }
            set { InternalSetPropagateSetTime(ref this, value); }
        }

        internal bool canChangeInputs
        {
            get { return CanChangeInputsInternal(ref this); }
        }

        internal bool canSetWeights
        {
            get { return CanSetWeightsInternal(ref this); }
        }

        internal bool canDestroy
        {
            get { return CanDestroyInternal(ref this); }
        }

        private static bool CanChangeInputsInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.CanChangeInputs(playable);
        }

        private static bool CanSetWeightsInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.CanSetWeights(playable);
        }

        private static bool CanDestroyInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.CanDestroy(playable);
        }

        private static PlayState GetPlayStateInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetPlayState(playable);
        }

        private static void SetPlayStateInternal(
            ref PlayableHandle playable,
            PlayState playState)
        {
            LegacyPlayableRuntime.SetPlayState(playable, playState);
        }

        private static double GetSpeedInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetSpeed(playable);
        }

        private static void SetSpeedInternal(
            ref PlayableHandle playable,
            double speed)
        {
            LegacyPlayableRuntime.SetSpeed(playable, speed);
        }

        private static double GetTimeInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetTime(playable);
        }

        private static void SetTimeInternal(
            ref PlayableHandle playable,
            double time)
        {
            LegacyPlayableRuntime.SetTime(playable, time);
        }

        private static bool InternalGetDone(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetDone(playable);
        }

        private static void InternalSetDone(
            ref PlayableHandle playable,
            bool isDone)
        {
            LegacyPlayableRuntime.SetDone(playable, isDone);
        }

        public double duration
        {
            get { return GetDurationInternal(ref this); }
            set { SetDurationInternal(ref this, value); }
        }

        private static double GetDurationInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetDuration(playable);
        }

        private static void SetDurationInternal(
            ref PlayableHandle playable,
            double duration)
        {
            LegacyPlayableRuntime.SetDuration(playable, duration);
        }

        private static bool InternalGetPropagateSetTime(
            ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetPropagateSetTime(playable);
        }

        private static void InternalSetPropagateSetTime(
            ref PlayableHandle playable,
            bool value)
        {
            LegacyPlayableRuntime.SetPropagateSetTime(playable, value);
        }

        private static void GetGraphInternal(
            ref PlayableHandle playable,
            ref PlayableGraph graph)
        {
            graph = LegacyPlayableRuntime.GetGraph(playable);
        }

        private static int GetInputCountInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetInputCount(playable);
        }

        private static void SetInputCountInternal(
            ref PlayableHandle playable,
            int count)
        {
            LegacyPlayableRuntime.SetInputCount(playable, count);
        }

        private static int GetOutputCountInternal(ref PlayableHandle playable)
        {
            return LegacyPlayableRuntime.GetOutputCount(playable);
        }

        private static void SetOutputCountInternal(
            ref PlayableHandle playable,
            int count)
        {
            LegacyPlayableRuntime.SetOutputCount(playable, count);
        }

        public PlayableHandle GetInput(int inputPort)
        {
            return GetInputInternal(ref this, inputPort);
        }

        private static PlayableHandle GetInputInternal(
            ref PlayableHandle playable,
            int index)
        {
            return LegacyPlayableRuntime.GetInput(playable, index);
        }

        public PlayableHandle GetOutput(int outputPort)
        {
            return GetOutputInternal(ref this, outputPort);
        }

        private static PlayableHandle GetOutputInternal(
            ref PlayableHandle playable,
            int index)
        {
            return LegacyPlayableRuntime.GetOutput(playable, index);
        }

        private static void SetInputWeightFromIndexInternal(
            ref PlayableHandle playable,
            int index,
            float weight)
        {
            LegacyPlayableRuntime.SetInputWeight(playable, index, weight);
        }

        public bool SetInputWeight(int inputIndex, float weight)
        {
            if (!CheckInputBounds(inputIndex))
                return false;

            SetInputWeightFromIndexInternal(ref this, inputIndex, weight);
            return true;
        }

        private static float GetInputWeightFromIndexInternal(
            ref PlayableHandle playable,
            int index)
        {
            return LegacyPlayableRuntime.GetInputWeight(playable, index);
        }

        public float GetInputWeight(int inputIndex)
        {
            if (!CheckInputBounds(inputIndex))
                return 0f;

            return GetInputWeightFromIndexInternal(ref this, inputIndex);
        }

        private static void SetInputWeightInternal(
            ref PlayableHandle playable,
            ref PlayableHandle input,
            float weight)
        {
            LegacyPlayableRuntime.SetInputWeight(playable, input, weight);
        }

        public void SetInputWeightOld(PlayableHandle input, float weight)
        {
            SetInputWeightInternal(ref this, ref input, weight);
        }

        public bool SetInputWeight(
            PlayableHandle input,
            float weight)
        {
            if (!IsValid() || !input.IsValid())
                return false;

            SetInputWeightInternal(ref this, ref input, weight);
            return true;
        }

        public void Destroy()
        {
            PlayableGraph owner = graph;
            owner.DestroyPlayable(this);
        }

        public static bool operator ==(PlayableHandle x, PlayableHandle y)
        {
            return CompareVersion(x, y);
        }

        public static bool operator !=(PlayableHandle x, PlayableHandle y)
        {
            return !CompareVersion(x, y);
        }

        public override bool Equals(object p)
        {
            return p is PlayableHandle &&
                   CompareVersion(this, (PlayableHandle)p);
        }

        public override int GetHashCode()
        {
            return m_Handle.GetHashCode() ^ m_Version.GetHashCode();
        }

        internal static bool CompareVersion(
            PlayableHandle lhs,
            PlayableHandle rhs)
        {
            return lhs.m_Handle == rhs.m_Handle &&
                   lhs.m_Version == rhs.m_Version;
        }

        internal bool CheckInputBounds(int inputIndex)
        {
            return CheckInputBounds(inputIndex, false);
        }

        internal bool CheckInputBounds(int inputIndex, bool acceptAny)
        {
            if (inputIndex == -1 && acceptAny)
                return true;

            if (inputIndex < 0)
                throw new IndexOutOfRangeException(
                    "Index must be greater than or equal to 0.");

            if (inputCount <= inputIndex)
            {
                throw new IndexOutOfRangeException(
                    "inputIndex " + inputIndex +
                    " is greater than the number of available inputs (" +
                    inputCount + ").");
            }

            return true;
        }

        internal IntPtr m_Handle;
        internal int m_Version;
    }
}
