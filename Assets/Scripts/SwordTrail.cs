using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrail : MonoBehaviour
{
    private TrailRenderer renderer;
    void Start()
    {
        renderer = GetComponent<TrailRenderer>();
        renderer.enabled = false;
    }


    void Update()
    {
    }

    public void SetTrailEnabled()
    {
        renderer.enabled = true;
    }
    
    public void SetTrailDisabled()
    {
        renderer.enabled = false;
    }
}
