using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public WeaponType type = WeaponType.missile;//Тип снаряда 
    public WeaponDefinion def;//Свойства снаряда 
    public Renderer colorMissile;//компонент Renderer
    private Rigidbody rigidMissile;// Компонент Missile
    public float rotatespeed = 200f;//скорость поворота

    //Эффекты 
    public GameObject particleMissile;//эффекты частиц ракеты
    // Start is called before the first frame update
    void Awake()
    {
        rigidMissile = GetComponent<Rigidbody>();//Добуть компонент Rigidbody игрового оьъекта Rocket
        colorMissile = GetComponent<Renderer>();//Добуть компонент Renderer игрового оьъекта Rocket
        def = Main.GetWeaponDefinion(type);//Получить свойства типа оружия 
        colorMissile.material.color = def.color;//Покрасить в цвет что соотвецтвуэт типу оружия 
    }

    // Update is called once per frame
    void Update()
    {
        rigidMissile.velocity = Vector3.up * def.velocity;//Дать ракете ускорение вверх
        Instantiate(particleMissile, transform.position, Quaternion.identity);//Создать частицу
        GameObject target = GameObject.FindGameObjectWithTag("Enemy");//Найти игровой объект Enemy
        if (target == null)//Если его нет
        {
            return;//возвращятса 
        }
        else
        {
            Vector3 duration = target.transform.position - rigidMissile.position;//Установить вектор напрваления на игровой объект Enemy
            duration.Normalize();//Установить длину вектора  = 1, сохранив направление
            float rotate = Vector3.Cross(duration, transform.up).z;//найти ось z, которая перепендикулярна оси движения ракеты и Enemy
            rigidMissile.angularVelocity = new Vector3(0, 0, -rotate * rotatespeed);//Повернуть в направлении Enemy
            rigidMissile.velocity = duration * def.velocity;//Приследовать цель
        }
        
       
    }
}
