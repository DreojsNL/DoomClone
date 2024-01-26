using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenralAudio : MonoBehaviour
{
    public AudioSource shootingAudioSource;
    public AudioClip shootingClip;
    public bool AudioSpawner = false;
    public GameObject[] Prefab;

    // Start is called before the first frame update
    void Start()
    {
        shootingAudioSource = GetComponent<AudioSource>();
        shootingAudioSource.clip = shootingClip;
    }

  public void PlayAudio()
    {
        if (AudioSpawner == false && Prefab == null)
        {
            shootingAudioSource.Play();
        }
        else
        {
            Instantiate(Prefab[0], gameObject.transform.position, Quaternion.identity);
        }
       
    }
    public void SpawnPrefab(int number)
    {
        Instantiate(Prefab[number], gameObject.transform.position, Quaternion.identity);

    }
}
