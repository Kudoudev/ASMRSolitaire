using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public List<AudioClip> sfxs;

    void Start()
    {
        if (sfxs.Count > 0)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.clip = sfxs[Random.Range(0, sfxs.Count)];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
