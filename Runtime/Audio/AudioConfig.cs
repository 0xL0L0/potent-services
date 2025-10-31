using System.Collections.Generic;
using Potency.Services.Runtime.Configs;
using UnityEngine;
using UnityEngine.Audio;

namespace Potency.Services.Runtime.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Potency/Configs/AudioConfig", order = 0)]
    public class AudioConfig : ScriptableObject, IConfig
    {
        public GameObject AudioPlayerPrefab;
        public StringAudioClipDictionary StringAudioClips;
        public AudioMixer AudioMixer;
        public List<AudioMixerGroup> AudioMixerGroups;
    }
}