namespace UnityEngine.Playables
{
    internal sealed class AnimationOffsetPlayable : AnimationPlayable
    {
        public Vector3 position
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationOffsetPosition(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAnimationOffsetPosition(handle, value);
            }
        }

        public Quaternion rotation
        {
            get
            {
                return LegacyPlayableRuntime.GetAnimationOffsetRotation(handle);
            }
            set
            {
                LegacyPlayableRuntime.SetAnimationOffsetRotation(handle, value);
            }
        }
    }
}
