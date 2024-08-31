using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource drawSound;
    private AudioSource ballHit;
    private AudioSource dunk;
    private AudioSource click;
    private AudioSource whoosh;

    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        drawSound = gameObject.AddComponent<AudioSource>();
        drawSound.playOnAwake = false;
        drawSound.volume = 0.75f;
        drawSound.clip = Resources.Load("Draw") as AudioClip;

        ballHit = gameObject.AddComponent<AudioSource>();
        ballHit.playOnAwake = false;
        ballHit.volume = 0.75f;
        ballHit.clip = Resources.Load("BallHit") as AudioClip;

        dunk = gameObject.AddComponent<AudioSource>();
        dunk.playOnAwake = false;
        dunk.volume = 0.75f;
        dunk.pitch = 1.5f;
        dunk.clip = Resources.Load("Dunk") as AudioClip;

        click = gameObject.AddComponent<AudioSource>();
        click.playOnAwake = false;
        click.clip = Resources.Load("Click") as AudioClip;

        whoosh = gameObject.AddComponent<AudioSource>();
        whoosh.playOnAwake = false;
        whoosh.clip = Resources.Load("Whoosh") as AudioClip;
    }



    public void DrawSound(float pitch)
    {
        if (drawSound.isPlaying) return;
        drawSound.pitch = pitch;
        drawSound.Play();
    }
    public void BallHit(float volume)
    {
        ballHit.volume = volume;
        ballHit.Play();
    }
    public void Dunk()
    {
        dunk.Play();
    }
    public void Click()
    {
        click.Play();
    }
    public void Whoosh()
    {
        whoosh.Play();
    }


}
