using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Transform target;
    public float snap;

    void Update()
    {
        Vector3 pos = new Vector3(Mathf.Round(target.position.x / snap) * snap, 0, Mathf.Round(target.position.z / snap) * snap);
        transform.position = pos;
    }
}
