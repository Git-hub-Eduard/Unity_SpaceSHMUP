using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public WeaponType type = WeaponType.blaster;
    GameObject Collar;//Дуло
    WeaponDefinion def;
    private float timeShoot;
    public float delayShot = 1f;
    // Start is called before the first frame update
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//Получить свойства оружия 
        Collar = transform.Find("Collar").gameObject;//Находим дочерий объект Collar родительского объекта
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Enemy");//Найти игровой объект Enemy
        if (target == null)//Если его нет
        {
            return;//возвращятса 
        }
        else
        {
            Vector3 duration = target.transform.position - transform.position;//Установить вектор напрваления на игровой объект Enemy
            float rotate = Mathf.Atan2(duration.y, duration.x) * Mathf.Rad2Deg;//Получить значение оси z
            transform.rotation = Quaternion.Euler(0, 0, rotate-91);//Повернуть в направление врага
            if ((Time.time - timeShoot) < delayShot)
            {
                return;
            }
            Fire();//Стрелять
        }
    }


    /// <summary>
    /// Функция стрельбы по врагам 
    /// </summary>
    void Fire()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//Создать снаряд
        go.tag = "ProjectileHero";//Задать тег
        go.layer = LayerMask.NameToLayer("ProjectileHero");//Задать физический слой
        go.transform.position = Collar.transform.position;//Расположить снаряд у дула
        Projectile p = go.GetComponent<Projectile>();//Получить компонент Projectile
        p.type = type;//Установить тип снаряда 
        Vector3 vel = Vector3.up * def.velocity;//дать ускорение снарду
        p.transform.rotation = transform.rotation;//Повернуть снаряд в направление врага
        p.rigid.velocity = p.transform.rotation * vel;//Дать ускорение снаряда в направление врага
        timeShoot = Time.time;//Последнее время выстрела
    }
}
