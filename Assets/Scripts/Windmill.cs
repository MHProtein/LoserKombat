using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    public float rps; 
    [SerializeField] private Transform transform;
        
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, rps * Time.deltaTime));
    }
}
