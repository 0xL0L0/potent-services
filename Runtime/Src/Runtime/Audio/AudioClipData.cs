using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potency.Services.Runtime.Audio
{
    [Serializable]
    public struct AudioClipData
    {
        public List<AudioClip> AudioClips;
        public string MixerId;
        public float Volume;
        public float Pitch;
        public bool Loop;
    }
}
