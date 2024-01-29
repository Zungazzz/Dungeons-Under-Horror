using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void DestroySelfAnimEvent(){
        Destroy(gameObject);
    }
    private ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update(){
        if ( GetComponent<ParticleSystem>() && !ps.IsAlive())
        {
            DestroySelfAnimEvent();
        }
    }
}
