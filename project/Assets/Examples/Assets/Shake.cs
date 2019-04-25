using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Random.insideUnitSphere * 180 * Time.deltaTime);
    }
}
