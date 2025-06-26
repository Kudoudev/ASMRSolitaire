using System.Collections.Generic;
using UnityEngine;

public class SFXM : MonoBehaviour
{
    public AudioClip dealCard;
    AudioSource aus;

    public static SFXM I;
    void Awake()
    {
        I = this;
    }
    void Start()
    {
        aus = GetComponent<AudioSource>();
    }
    public void Play(AudioClip c)
    {
        aus.PlayOneShot(c);
    }
}
