using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : MonoBehaviour
{

    private WeaponType type = WeaponType.missile;
    public GameObject Collar;
    private WeaponDefinion def;
    private float misselTime;
    private Vector3 pos;
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//Получить свойства оружия 
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
            transform.rotation = Quaternion.Euler(0, 0, rotate - 91);//Повернуть в направление врага
            CreateRocket();
        }
    }
    void CreateRocket()
    {
        if ((Time.time - misselTime) < def.delayBetwenshots)//Проверить прошло ли достаточно времени что было дозволено создать ракету
        {
            return;
        }
        else
        {
            GameObject missile = Instantiate<GameObject>(def.projectilePefab);//Создать ракету 
            pos = Collar.transform.position;//Перезаписать кординаты
            pos.z = 0;// установить z = 0
            missile.transform.position = pos;//Расположить на месте конца платформи
            misselTime = Time.time;//Обновить время
        }
        
    }
}
