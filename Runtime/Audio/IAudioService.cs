using System;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Potency.Services.Runtime.Audio
{
    public interface IAudioService : IDisposable
    {
        float MusicVolume { get; set; }
        float SfxVolume { get; set; }
        float AmbientVolume { get; set; }

        /// <summary>
        /// Requests to play a clip in 2D
        /// </summary>
        void PlaySfx2D(string id);

        /// <summary>
        /// Requests to play a looped music clip in 2D
        /// </summary>
        void PlayMusic(string id, float fadeDuration);

        /// <summary>
        /// Requests to play a looped ambience clip in 2D
        /// </summary>
        void PlayAmbience(string id, float fadeDuration);
        
        /// <summary>
        /// Requests to stop playing music
        /// </summary>
        void StopMusic(float fadeDuration);
        
        /// <summary>
        /// Requests to stop playing ambience
        /// </summary>
        void StopAmbience(float fadeDuration);
    }

    public interface IAudioInternalService : IAudioService
    {
        /// <summary>
        /// Loads audio clips into the service using <see cref="IAudioInternalService{T}.AddAudioClips"/>
        /// </summary>
        void LoadClipData(StringAudioClipDictionary clips);

        /// <summary>
        /// Unloads audio clips from the service using <see cref="IAudioInternalService{T}.RemoveAudioClips"/>
        /// </summary>
        void UnloadClipData(StringAudioClipDictionary clips);

        /// <summary>
        /// Initializes a list of audio mixer groups
        /// </summary>
        void LoadAudioMixers(AudioMixer mixer, List<AudioMixerGroup> mixerGroups);
    }
}