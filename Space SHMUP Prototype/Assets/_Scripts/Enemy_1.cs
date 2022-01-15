using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    //Число секунд полного цикла синусоиды
    public float waveFrequency = 2;

    //Ширина синусоиды в метрах
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float x0;// начальное значение кординат
    private float birthTime;

    private float lastShot;//Время последнего выстрела
    WeaponDefinion def;//Свойства оружия 
    public float projSpeed = 30;//Скорость снаряда
    WeaponType type = WeaponType.blaster;//тип оружия
    // Метод страт Start() потому что не используетса супер класом Enemy
    void Start()
    {
        // Установить начальную позицию координат х объекта Enemy_1
        x0 = pos.x;
        birthTime = Time.time;
        //Добуть свойства оружия для бластера
        def = Main.GetWeaponDefinion(type);
    }
    //Переопределить  функцию Move суперкласса Enemy
    public override void Move()
    {
        Vector3 temPos = pos;// записать переменную pos из родительского класа Enemy
        float age = Time.time - birthTime;
        //Значение theta изменяетса с течением времени  
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        temPos.x = x0 + waveWidth * sin;
        pos = temPos;
        //Повернуть немного относительно оси  Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);


        if ((Time.time - lastShot) > waveFrequency)//Проверить если время с момента последнего выстрела больше за waveFrequency
        {
            FireEnemy();//Стрелять
        }
        
        
        //
        // Движение по оси y
        base.Move();
    }
    /// <summary>
    /// Функция стрельбы врага
    /// </summary>
    void FireEnemy()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//Создать снаряд за типом оружия 
        go.tag = "ProjectileEnemy";//имя тега
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//имя слоя
        go.transform.position = transform.position;//установить координаты 
        Projectile enem = go.GetComponent<Projectile>();
        enem.type = type;//установить тип снаряда
        enem.rigid.velocity = Vector3.down * projSpeed;//придать ускорение снаряда
        lastShot = Time.time;//записать время последнего выстрела
    }

}
