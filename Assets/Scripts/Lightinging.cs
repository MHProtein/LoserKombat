using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lightinging : MonoBehaviour
{
    public float interval = .5f;
    
    private LineRenderer renderer;
    private float timer;
    private Vector3 lastPos;
    private int index;
    private bool stop;
    [SerializeField] private GameObject FirePrefab;
    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
        lastPos = transform.position;
    }
    
    void Update()
    {
        if (!stop)
        {
            if (renderer.positionCount < 100)
            {
                timer += Time.deltaTime;
                if (timer >= interval)
                {
                    index = ++renderer.positionCount - 1;
                    renderer.SetPosition(index, (new Vector3(2.0f * Random.value, lastPos.y - 2.0f *  Random.value, 0.0f)));
                    lastPos = renderer.GetPosition(index);
                    if (lastPos.y <= 0.0f)
                    {
                        lastPos.y = 0.0f;
                        renderer.SetPosition(index, lastPos);
                        stop = true;
                        Instantiate(FirePrefab, lastPos, Quaternion.identity);
                    }
                    timer = 0.0f;
                }
            }
        }
    }
}
