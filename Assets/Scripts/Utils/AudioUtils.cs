using UnityEngine;

namespace Utils
{
    public static class AudioUtils
    {
        private const string SFXTag = "SFXAudioSource";

        public static AudioSource FindSfxSource()
        {
            return GameObject.FindWithTag(SFXTag).GetComponent<AudioSource>();
        }
    }
}