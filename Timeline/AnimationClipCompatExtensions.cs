using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public static class AnimationClipCompatExtensions
    {
        public static bool HasRootMotion(this AnimationClip clip)
        {
            return false; // Comment: Unity 5.6 cannot know, so always false.
        }
    }
}