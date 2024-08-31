using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesColorController : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;
    private ParticleSystem.MainModule module;

    public Color StartColor
    {
        set
        {
            module = system.main;
            module.startColor = value;
        }
    }
}
