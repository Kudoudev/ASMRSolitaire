using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Sound systems.
/// </summary>
public class SoundSystems : SingletonMonoBehaviour<SoundSystems>
{
    /// <summary>
    /// Sound items.
    /// </summary>
    [System.Serializable]
    public struct SoundItems
    {
        /// <summary>
        /// The index of the sound.
        /// </summary>
        public Enums.SoundIndex soundIndex;

        /// /// <summary>
        /// The sound.
        /// </summary>
        public AudioClip sound;
    }

    /// /// <summary>
    /// Music items.
    /// </summary>
    [System.Serializable]
    public struct MusicItems
    {
        /// /// <summary>
        /// The index of the music.
        /// </summary>
        public Enums.MusicIndex musicIndex;

        /// <summary>
        /// The music.
        /// </summary>
        public AudioClip music;
    }

    // =============================== Audio Player ============================ //

    /// /// <summary>
    /// The sound player.
    /// </summary>
    private AudioSource SoundPlayer;

    /// <summary>
    /// The music player.
    /// </summary>
    private AudioSource MusicPlayer;

    // =============================== References ============================== //

    #region References

    /// <summary>
    /// The sound items.
    /// </summary>
    [SerializeField] private SoundItems[] soundItems;

    /// <summary>
    /// The music items.
    /// </summary>
    [SerializeField] private MusicItems[] musicItems;

    #endregion


    // ================================ Variables ========================= //

    #region Variables

    /// <summary>
    /// The sound library.
    /// </summary>
    protected Dictionary<Enums.SoundIndex, AudioClip> soundLibrary = new Dictionary<Enums.SoundIndex, AudioClip>();

    /// <summary>
    /// The music library.
    /// </summary>
    protected Dictionary<Enums.MusicIndex, AudioClip> musicLibrary = new Dictionary<Enums.MusicIndex, AudioClip>();

    #endregion

    // =============================== Monobehaviour =========================== //
    /// <summary>
    /// Awake this instance.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        if (ReferenceEquals(MusicPlayer, null))
        {
            MusicPlayer = gameObject.AddComponent<AudioSource>();

            MusicPlayer.loop = false;
        }

        if (ReferenceEquals(SoundPlayer, null))
        {
            SoundPlayer = gameObject.AddComponent<AudioSource>();

            SoundPlayer.loop = false;
        }

        InitCache();
    }

    /// <summary>
    /// Inits the cache.
    /// </summary>
    protected void InitCache()
    {
        for (int i = 0; i < soundItems.Length; i++)
        {
            if (!soundLibrary.ContainsKey(soundItems[i].soundIndex))
            {
                soundLibrary.Add(soundItems[i].soundIndex, soundItems[i].sound);
            }
        }

        for (int i = 0; i < musicItems.Length; i++)
        {
            if (!musicLibrary.ContainsKey(musicItems[i].musicIndex))
            {
                musicLibrary.Add(musicItems[i].musicIndex, musicItems[i].music);
            }
        }
    }


    #region Play

    /// <summary>
    /// Plaies the sound.
    /// </summary>
    /// <param name="soundIndex">Sound index.</param>
    public void PlaySound(Enums.SoundIndex soundIndex)
    {
        AudioClip audioFound;

        if (soundLibrary.TryGetValue(soundIndex, out audioFound))
        {
            SoundPlayer.PlayOneShot(audioFound);
        }
    }

    /// <summary>
    /// Players the music.
    /// </summary>
    /// <param name="musicIndex">Music index.</param>
    /// <param name="IsLoop">If set to <c>true</c> is loop.</param>
    public void PlayerMusic(Enums.MusicIndex musicIndex, bool IsLoop = false)
    {
        AudioClip audioFound;

        MusicPlayer.DOComplete(true);

        MusicPlayer.DOFade(0, Contains.DurationFade).OnComplete(() =>
        {
            if (musicLibrary.TryGetValue(musicIndex, out audioFound))
            {
                MusicPlayer.clip = audioFound;

                MusicPlayer.Play();

                MusicPlayer.loop = IsLoop;
            }

            MusicPlayer.DOFade(0.7f, Contains.DurationFade);
        });
    }

    /// <summary>
    /// Stops the music.
    /// </summary>
    public void StopMusic()
    {
        MusicPlayer.DOFade(0, Contains.DurationFade).OnComplete(() => { MusicPlayer.Stop(); });
    }

    #endregion

    #region Controller

    /// <summary>
    /// Disables the sound.
    /// </summary>
    public void DisableSound()
    {
        if (SoundPlayer != null)
        {
            SoundPlayer.DOKill();

            SoundPlayer.DOFade(0, Contains.DurationFade).OnComplete(() => { SoundPlayer.mute = true; });
            ;
        }
    }

    /// <summary>
    /// Disables the music.
    /// </summary>
    public void DisableMusic()
    {
        if (MusicPlayer != null)
        {
            MusicPlayer.DOKill();

            MusicPlayer.DOFade(0, Contains.DurationFade).OnComplete(() => { MusicPlayer.mute = true; });
        }
    }

    /// <summary>
    /// Enables the sound.
    /// </summary>
    public void EnableSound()
    {
        if (SoundPlayer != null)
        {
            SoundPlayer.mute = false;

            SoundPlayer.DOKill();

            SoundPlayer.DOFade(1, Contains.DurationFade);
        }
    }

    /// <summary>
    /// Enables the music.
    /// </summary>
    public void EnableMusic()
    {
        if (MusicPlayer != null)
        {
            MusicPlayer.mute = false;

            MusicPlayer.DOKill();

            MusicPlayer.DOFade(0.7f, Contains.DurationFade);
        }
    }

    #endregion
}