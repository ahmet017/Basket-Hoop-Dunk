using UnityEngine;
using System;

namespace UnityUtils
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class HiddenAttribute : PropertyAttribute
    {
        public HiddenAttribute()
        {
        }
    }
}

