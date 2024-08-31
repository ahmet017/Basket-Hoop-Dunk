using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem system;

    void Start()
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.velocity = new Vector3(0.0f, 0.0f, 2.0f);
        
        for (int i = 0; i < 100; i++)
        {
            emitParams.position = new Vector3(Random.Range(-1, 1), 0.0f, 0.0f);
            system.Emit(emitParams, 1);
        }

       // system.Play(); // Continue normal emissions
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
