using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedRotation = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float Z = -(speedRotation * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0,0,Z);
    }
}
