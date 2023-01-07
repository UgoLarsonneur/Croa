using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource source;
    [SerializeField] SoundClips ribbits;
    [SerializeField] SoundClips splashs;

    public static float Volume {get; set;} = 1f;

    public override bool UseDontDestroyOnLoad => true;

    public void Start()
    {
        source.loop = true;
        source.Play();
        
        EventManager.StartListening("Jump", OnJump);
        EventManager.StartListening("Land", OnLand);
    }
    
    private void OnJump()
    {
        PlayRibbit();
    }

    private void OnLand()
    {
        PlaySplash();
    }
    
    public static void PlayRibbit()
    {
        SoundClips.SoundClip clip = Instance.ribbits.getRandomSound();
        PlaySound(clip.clip, Volume * clip.volume);
    }

    public static void PlaySplash()
    {
        SoundClips.SoundClip clip = Instance.splashs.getRandomSound();
        PlaySound(clip.clip, Volume * clip.volume);
    }

    public static void PlaySound(AudioClip sound, float soundVolume = 1f)
    {
        Instance.source.PlayOneShot(sound, Volume * soundVolume);
    }

    public static void PlayMusic(AudioClip music)
    {
        Instance.source.clip = music;
        Instance.source.Play();
    }

    protected override void OnDestroy() {
        EventManager.StopListening("Jump", OnJump);
        EventManager.StopListening("Land", OnLand);
        base.OnDestroy();
    }
}


