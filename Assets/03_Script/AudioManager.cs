using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager: MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<Sound> sounds;
    private AudioSource backGroundAudioSource;
    private AudioMixerGroup masterAudioMixer;

    private void Awake()
    {
        InitializeAudioMixer();
        InitializeBackgroundAudioSource();
        //PlayBackGroundMusic();
        MenuSceneManager.VolumeChangeEmitter += OnVolumeChange;
        AudioHelper.PlayOneShotEmitter += PlayOneShotSound;
        AudioHelper.PlayBackGroundEmitter += PlayBackGroundMusic;
        AudioHelper.PauseBackGroundEmitter += PauseBackGroundMusic;
    }

    private void InitializeAudioMixer()
    {
        AudioMixerGroup[] audioMixers = audioMixer.FindMatchingGroups("Master");
        if(audioMixers == null || audioMixers.Length == 0)
            Debug.Log("No audio mixers group found!");
        else
            masterAudioMixer = audioMixers[0];
    }

    private void InitializeBackgroundAudioSource()
    {
        if (backGroundAudioSource == null)
        {
            backGroundAudioSource = gameObject.AddComponent<AudioSource>();
            backGroundAudioSource.playOnAwake = true;
            backGroundAudioSource.loop = true;
            backGroundAudioSource.outputAudioMixerGroup = masterAudioMixer;
        }
    }

    private void Start()
    {
        OnVolumeChange(PersistentDataManager.Instance.GetCurrentPlayerData().GetVolume());
    }

    private void OnVolumeChange(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    private void PlayBackGroundMusic(AudioHelper.Sounds name)
    {
        backGroundAudioSource.clip = GetAudioClip(name);
        backGroundAudioSource.Play();
    }

    private void PauseBackGroundMusic()
    {
        backGroundAudioSource.Pause();
    }

    public void PlayOneShotSound(AudioHelper.Sounds name)
    {
        StartCoroutine(PlayOneShot(name));
    }

    private IEnumerator PlayOneShot(AudioHelper.Sounds name)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = masterAudioMixer;
        audioSource.PlayOneShot(GetAudioClip(name));
        while (audioSource.isPlaying) yield return null;
        Destroy(audioSource);
    } 

    private AudioClip GetAudioClip(AudioHelper.Sounds name)
    {
        if(sounds == null || sounds.Count == 0)
        {
            Debug.Log("Nessun sound referenziato");
            return null;
        }
        else
        {
            Sound s = sounds.Find(sound => sound.name == name);
            if (s != null)
                return s.audioClip;
            else
            {
                Debug.Log("Audio clip non trovato per " + name);
                return null;
            }
        }
    }


    private void OnDestroy()
    {
        MenuSceneManager.VolumeChangeEmitter -= OnVolumeChange;
        AudioHelper.PlayOneShotEmitter -= PlayOneShotSound;
        AudioHelper.PlayBackGroundEmitter -= PlayBackGroundMusic;
        AudioHelper.PauseBackGroundEmitter -= PauseBackGroundMusic;
    }
}
