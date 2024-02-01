using System;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    [Serializable]
    public struct Speed
    {
        [SerializeField] public float forward;
        [SerializeField] public float side;
        [SerializeField] public float backward;

        public override string ToString()
        {
            return $"Speed ({forward}, {side}, {backward})";
        }
    }
}