using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.Audio
{
    public class AudioService : IAudioInternalService
    {
        private readonly Transform _audioPoolRoot;
        private readonly GameObject _audioPlayerPrefab;
        private readonly Dictionary<string, AudioMixerGroup> _mixerGroups = new();
        private readonly Dictionary<string, AudioClipData> _loadedClips = new();
        private AudioSourceMonoComponent _activeMusicSource;
        private AudioSourceMonoComponent _transitionMusicSource;
        private AudioSourceMonoComponent _activeAmbientSource;
        private AudioSourceMonoComponent _transitionAmbientSource;
        private AudioMixer _audioMixer;
        private AudioListener _listener;

        public const float MAX_VOLUME = 1f;

        // Convert from decibels to linear 0-1 scale
        // see https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
        private float DecibelToLinear(float valueDb)
        {
            return Mathf.Pow(10f, valueDb / 20f);
        }

        // Convert from linear 0-1 scale to decibels
        private float LinearToDecibel(float value01)
        {
            return Mathf.Log10(value01 + 0.0001f) * 20f;
        }

        public float MusicVolume
        {
            get
            {
                float volume = 1f; // Default value

                if (_audioMixer != null)
                {
                    _audioMixer.GetFloat("MusicVolume", out volume);
                    volume = DecibelToLinear(volume);
                }

                return Mathf.Clamp01(volume);
            }
            set
            {
                float clampedVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat("MusicVolume", value);
                if (_audioMixer != null)
                {
                    float volumeInDecibels = LinearToDecibel(clampedVolume);
                    _audioMixer.SetFloat("MusicVolume", volumeInDecibels);
                }
            }
        }

        public float SfxVolume { get; set; } = 1f;
        public float AmbientVolume { get; set; } = 1f;

        public AudioService(AudioConfig audioConfig)
        {
            _audioPlayerPrefab = audioConfig.AudioPlayerPrefab;
            _audioPoolRoot = Object.Instantiate(new GameObject("Audio Container")).transform;
            _listener = Object.Instantiate(new GameObject("Audio Listener")).AddComponent<AudioListener>();
            _listener.transform.SetParent(_audioPoolRoot.transform);

            Object.DontDestroyOnLoad(_audioPoolRoot.gameObject);
            
            _activeMusicSource = LeanPool.Spawn(audioConfig.AudioPlayerPrefab, _audioPoolRoot).GetComponent<AudioSourceMonoComponent>();
            _transitionMusicSource = LeanPool.Spawn(audioConfig.AudioPlayerPrefab, _audioPoolRoot).GetComponent<AudioSourceMonoComponent>();
            _activeAmbientSource = LeanPool.Spawn(audioConfig.AudioPlayerPrefab, _audioPoolRoot).GetComponent<AudioSourceMonoComponent>();
            _transitionAmbientSource = LeanPool.Spawn(audioConfig.AudioPlayerPrefab, _audioPoolRoot).GetComponent<AudioSourceMonoComponent>();
            
            LoadClipData(audioConfig.StringAudioClips);
            LoadAudioMixers(audioConfig.AudioMixer, audioConfig.AudioMixerGroups);
        }

        public void LoadClipData(StringAudioClipDictionary clips)
        {
            foreach(var kvp in clips)
            {
                _loadedClips.Add(kvp.Key, kvp.Value);
            }
        }
        
        public void UnloadClipData(StringAudioClipDictionary clips)
        {
            foreach(var kvp in clips)
            {
                _loadedClips.Remove(kvp.Key);
            }
        }
        
        public void LoadAudioMixers(AudioMixer mixer, List<AudioMixerGroup> mixerGroups)
        {
            _audioMixer = mixer;
            
            foreach(var group in mixerGroups)
            {
                _mixerGroups.Add(group.name, group);
            }
        }

        public void PlaySfx2D(string id)
        {
            var newPlayer = LeanPool.Spawn(_audioPlayerPrefab, _audioPoolRoot.transform).GetComponent<AudioSourceMonoComponent>();
            newPlayer.Play(_loadedClips[id], SfxVolume, _mixerGroups[_loadedClips[id].MixerId]);
        }

        public async void PlayMusic(string id, float fadeDuration)
        {
            if (_activeMusicSource.IsPlaying)
            {
                _activeMusicSource.LerpVolume(_activeMusicSource.CurrentVolume, 0, fadeDuration);
                _transitionMusicSource.LerpVolume(0, _loadedClips[id].Volume, fadeDuration);
                _transitionMusicSource.Play(_loadedClips[id], MAX_VOLUME, _mixerGroups[_loadedClips[id].MixerId]);

                await Task.Delay((int)fadeDuration * 1000);
                
                (_activeMusicSource, _transitionMusicSource) = (_transitionMusicSource, _activeMusicSource);
                _transitionMusicSource.StopPlayback();
            }
            else
            {
                _activeMusicSource.LerpVolume(0, _loadedClips[id].Volume, fadeDuration);
                _activeMusicSource.Play(_loadedClips[id], MAX_VOLUME, _mixerGroups[_loadedClips[id].MixerId]);
            }
        }

        public void StopMusic(float fadeDuration)
        {
            if (fadeDuration <= 0)
            {
                _activeMusicSource.StopPlayback();
                _transitionMusicSource.StopPlayback();
            }
            else
            {
                _activeMusicSource.LerpVolume(_activeMusicSource.CurrentVolume, 0, fadeDuration);
                _transitionMusicSource.LerpVolume(_transitionMusicSource.CurrentVolume, 0, fadeDuration);
            }
        }

        public async void PlayAmbience(string id, float fadeDuration)
        {
            if (_activeAmbientSource.IsPlaying)
            {
                _activeAmbientSource.LerpVolume(_activeMusicSource.CurrentVolume, 0, fadeDuration);
                _transitionAmbientSource.LerpVolume(0, _loadedClips[id].Volume, fadeDuration);
                _transitionAmbientSource.Play(_loadedClips[id], AmbientVolume, _mixerGroups[_loadedClips[id].MixerId]);

                await Task.Delay((int)fadeDuration * 1000);
                
                (_activeAmbientSource, _transitionAmbientSource) = (_transitionAmbientSource, _activeAmbientSource);
                _transitionAmbientSource.StopPlayback();
            }
            else
            {
                _activeAmbientSource.LerpVolume(0, _loadedClips[id].Volume, fadeDuration);
                _activeAmbientSource.Play(_loadedClips[id], AmbientVolume, _mixerGroups[_loadedClips[id].MixerId]);
            }
        }

        public void StopAmbience(float fadeDuration)
        {
            if (fadeDuration <= 0)
            {
                _activeAmbientSource.StopPlayback();
                _transitionAmbientSource.StopPlayback();
            }
            else
            {
                _activeAmbientSource.LerpVolume(_activeAmbientSource.CurrentVolume, 0, fadeDuration);
                _transitionAmbientSource.LerpVolume(_transitionAmbientSource.CurrentVolume, 0, fadeDuration);
            }
        }

        public void Dispose()
        {
        }
    }
}