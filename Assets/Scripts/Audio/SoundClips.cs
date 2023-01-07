
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundClips", menuName = "ScriptableObject/SoundClips", order = 0)]
public class SoundClips : ScriptableObject {
    
    //TODO: ajouter volume pour chaque son
    [System.Serializable]
    public struct SoundClip
    {
        public AudioClip clip;
        public float volume;
    }

    [SerializeField] List<SoundClip> sounds;
    

    public SoundClip getRandomSound()
    {
        if(sounds.Count == 0)
        {
            throw new Exception("No sound to play");
        }

        int choice = UnityEngine.Random.Range(0, sounds.Count);
        return sounds[choice];
    }

}