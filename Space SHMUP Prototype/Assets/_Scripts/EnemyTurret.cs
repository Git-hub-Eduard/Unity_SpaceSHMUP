using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    private WeaponType type = WeaponType.blaster;
    public GameObject Collar;//Дуло
    WeaponDefinion def;
    private float timeShootEnemy;
    public float delayShotEnemy = 1f;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//Получить свойства оружия 
        target = GameObject.FindGameObjectWithTag("Hero").transform;//Найти игровой объект Hero
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 duration = target.position - transform.position;//Установить вектор напрваления на игровой объект Hero
        float rotate = Mathf.Atan2(duration.y, duration.x) * Mathf.Rad2Deg;//Получить значение оси z
        transform.rotation = Quaternion.Euler(0, 0, rotate - 91);//Повернуть в направление игрока
        if ((Time.time - timeShootEnemy) < delayShotEnemy)
        {
            return;
        }
        FireEnemy();

    }
    void FireEnemy()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//Создать снаряд
        go.tag = "ProjectileEnemy";//Задать тег
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//Задать физический слой
        go.transform.position = Collar.transform.position;//Расположить снаряд у дула
        Projectile p = go.GetComponent<Projectile>();//Получить компонент Projectile
        p.type = type;//Установить тип снаряда 
        Vector3 vel = Vector3.up * def.velocity;//дать ускорение снарду
        p.transform.rotation = transform.rotation;//Повернуть снаряд в направление врага
        p.rigid.velocity = p.transform.rotation * vel;//Дать ускорение снаряда в направление врага
        timeShootEnemy = Time.time;//Последнее время выстрела
    }
}
