using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundScript : MonoBehaviour
{
    private AudioSource audioSource; // reference to the AudioSource component

    private void Start() {
        // get a reference to the AudioSource component attached to this game object
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound() {
        // play the sound effect using the PlayOneShot method of the AudioSource component
        audioSource.PlayOneShot(audioSource.clip);
    }
}
