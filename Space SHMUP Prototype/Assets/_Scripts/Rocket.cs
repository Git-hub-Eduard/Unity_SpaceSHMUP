using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public WeaponType type = WeaponType.missile;//Тип снаряда 
    public WeaponDefinion def;//Свойства снаряда 
    public Renderer colorMissile;//компонент Renderer
    private Rigidbody rigidMissile;// Компонент Missile
    public float rotatespeed = 200f;
    // Start is called before the first frame update
    void Awake()
    {
        rigidMissile = GetComponent<Rigidbody>();
        colorMissile = GetComponent<Renderer>();
        def = Main.GetWeaponDefinion(type);
    }

    // Update is called once per frame
    void Update()
    {
        rigidMissile.velocity = Vector3.up * def.velocity;
        GameObject target = GameObject.FindGameObjectWithTag("Enemy");
        if (target == null)
        {
            return;
        }
        else
        {
            Vector3 duration = target.transform.position - rigidMissile.position;
            duration.Normalize();
            float rotate = Vector3.Cross(duration, transform.up).z;
            rigidMissile.angularVelocity = new Vector3(0, 0, -rotate * rotatespeed);
            rigidMissile.velocity = duration * def.velocity;
        }
        
       
    }
}
