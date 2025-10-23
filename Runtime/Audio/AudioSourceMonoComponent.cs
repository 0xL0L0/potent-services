using System.Collections;
using Lean.Pool;
using Potency.Services.Runtime.Utils.Rng;
using UnityEngine;
using UnityEngine.Audio;

namespace Potency.Services.Runtime.Audio
{
    public class AudioSourceMonoComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource Source;
        public string MixerGroupID { get; private set; }
        private UnityEngine.Coroutine _playSoundCoroutine;
        private UnityEngine.Coroutine _lerpVolumeCoroutine;
        private bool _canDespawn;

        public bool IsPlaying => Source.isPlaying;
        public float CurrentVolume => Source.volume;

        /// <summary>
        /// Initialize the audio source of the object with relevant properties
        /// </summary>
        public void Play(AudioClipData clipData, float volume, AudioMixerGroup mixerGroup)
        {
            Source.outputAudioMixerGroup = mixerGroup;
            Source.clip = clipData.AudioClips[Rng.Range(0, clipData.AudioClips.Count-1)];
            Source.volume = volume;
            Source.pitch = clipData.Pitch;
            Source.loop = clipData.Loop;
            MixerGroupID = clipData.MixerId;

            _canDespawn = !clipData.Loop;
            
            StartPreparedPlayback();
        }

        /// <summary>
        /// Starts playback with currently initialized audio source values
        /// </summary>
        public void StartPreparedPlayback()
        {
            if(_playSoundCoroutine != null)
            {
                StopCoroutine(_playSoundCoroutine);
            }
            
            _playSoundCoroutine = StartCoroutine(PlaySoundCoroutine());
        }
        
        public void StopPlayback()
        {
            if(_playSoundCoroutine != null)
            {
                StopCoroutine(_playSoundCoroutine);
            }
            
            Source.Stop();
        }

        /// <summary>
        /// Instantly stops the playing sound, and despawns component back to pool if possible
        /// </summary>
        public void StopAndDespawn()
        {
            Source.Stop();
            
            if(_playSoundCoroutine != null)
            {
                StopCoroutine(_playSoundCoroutine);
            }
            
            if (_lerpVolumeCoroutine != null)
            {
                StopCoroutine(_lerpVolumeCoroutine);
            }

            LeanPool.Despawn(this);
        }
        
        private IEnumerator PlaySoundCoroutine()
        {
            Source.Play();

            do
            {
                yield return new WaitForSeconds(Source.clip.length);
            } 
            while(!_canDespawn);
            
            StopAndDespawn();
        }
        
        public void LerpVolume(float fromVolume, float toVolume, float fadeDuration)
        {
            if (_lerpVolumeCoroutine != null)
            {
                StopCoroutine(_lerpVolumeCoroutine);
            }
            
            _lerpVolumeCoroutine = StartCoroutine(LerpVolumeCoroutine(fromVolume, toVolume, fadeDuration));
        }
        
        private IEnumerator LerpVolumeCoroutine(float fromVolume, float toVolume, float fadeDuration)
        {
            var currentTimeProgress = 0f;
           
            while (currentTimeProgress < fadeDuration)
            {
                yield return null;

                currentTimeProgress += Time.deltaTime;

                var fadePercent = currentTimeProgress / fadeDuration;
                Source.volume = Mathf.Lerp(fromVolume, toVolume, fadePercent);
            }
            
            if (toVolume <= 0)
            {
                StopPlayback();
            }
        }
    }
}
