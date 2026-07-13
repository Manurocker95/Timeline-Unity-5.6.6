namespace UnityEngine.Playables
{
    /// <summary>
    /// Base managed playable object used by Legacy Timeline.
    /// </summary>
    public class Playable : IPlayable
    {
        public static implicit operator PlayableHandle(Playable playable)
        {
            return playable != null
                ? playable.handle
                : PlayableHandle.Null;
        }

        public PlayableHandle playableHandle
        {
            get { return handle; }
            set { handle = value; }
        }

        public bool IsValid()
        {
            return handle.IsValid();
        }

        public void Play()
        {
            if (handle.IsValid())
                handle.playState = PlayState.Playing;
        }

        public void Pause()
        {
            if (handle.IsValid())
                handle.playState = PlayState.Paused;
        }

        public PlayState GetPlayState()
        {
            return handle.IsValid()
                ? handle.playState
                : PlayState.Paused;
        }

        public void SetTime(double value)
        {
            if (handle.IsValid())
                handle.time = value;
        }

        public double GetTime()
        {
            return handle.IsValid() ? handle.time : 0.0;
        }

        public void SetSpeed(double value)
        {
            if (handle.IsValid())
                handle.speed = value;
        }

        public double GetSpeed()
        {
            return handle.IsValid() ? handle.speed : 0.0;
        }

        public void SetDuration(double value)
        {
            if (handle.IsValid())
                handle.duration = value;
        }

        public double GetDuration()
        {
            return handle.IsValid() ? handle.duration : 0.0;
        }

        public Playable GetInput(int inputPort)
        {
            if (!handle.IsValid())
                return null;

            PlayableHandle input = handle.GetInput(inputPort);
            return input.IsValid()
                ? input.GetObject<Playable>()
                : null;
        }

        public PlayableHandle GetInputHandle(int inputPort)
        {
            return handle.IsValid()
                ? handle.GetInput(inputPort)
                : PlayableHandle.Null;
        }

        public bool SetInputWeight(int inputPort, float weight)
        {
            return handle.IsValid() &&
                   handle.SetInputWeight(inputPort, weight);
        }

        public bool SetInputWeight(Playable input, float weight)
        {
            return input != null &&
                   SetInputWeight(input.handle, weight);
        }

        public bool SetInputWeight(PlayableHandle input, float weight)
        {
            return handle.IsValid() &&
                   handle.SetInputWeight(input, weight);
        }

        public float GetInputWeight(int inputPort)
        {
            return handle.IsValid()
                ? handle.GetInputWeight(inputPort)
                : 0f;
        }

        public int GetInputCount()
        {
            return handle.IsValid() ? handle.inputCount : 0;
        }

        public void SetInputCount(int value)
        {
            if (handle.IsValid())
                handle.inputCount = value;
        }

        public PlayableHandle handle;
    }
}
