using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move_Hero : Enemy
{
    // Start is called before the first frame update
    private Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Hero").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public override void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position,speed*Time.deltaTime);
    }
}
