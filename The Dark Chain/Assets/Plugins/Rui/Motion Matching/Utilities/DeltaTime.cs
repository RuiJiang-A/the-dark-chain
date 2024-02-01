using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    public static class DeltaTime
    {
        public enum Mode
        {
            DeltaTime,
            FixedDeltaTime
        }

        public static float Get(Mode mode)
        {
            return mode switch
            {
                Mode.DeltaTime => Time.deltaTime,
                Mode.FixedDeltaTime => Time.fixedDeltaTime,
                _ => 0.0f
            };
        }
    }

}