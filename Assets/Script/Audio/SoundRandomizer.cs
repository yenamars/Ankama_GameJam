using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomizer : MonoBehaviour 
{
    [SerializeField] private AudioSource adSource;
    [SerializeField] private Vector2 randomPitch;

    void OnEnable()
    {
        adSource.pitch = Random.Range(randomPitch.x, randomPitch.y);
        adSource.Play();
    }
}
