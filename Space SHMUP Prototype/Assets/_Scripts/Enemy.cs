using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;// Скорость в м/с
    public float fireRate = 0.3f;// Секунды между выстрелами
    public float health = 10;// количество жизней
    public int score = 100;// Очки за уничтожение корабля
    protected BoundsCheck bndCheck;// Ссылка на компонент BoundsCheck, что подключон к этому игровому объекту
    //Это свойство: метод, действующий как поле
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);// Возвращает координаты текущего объекта  
        }
        set
        {
            this.transform.position = value;// Перезаписывает координаты
        }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        //Получить ссылку га компонент сценария BoundsCheck что подключон к этому игровому объекту
    }

    // Update is called once per frame
    void Update()
    {
        Move();// Перемещать корабль вниз 
        if(bndCheck != null && bndCheck.offDown)
        {

            //Убедитса, что корабль вышел за нижнюю границу - уничтожить
            Destroy(gameObject);
        }
    }
    public virtual void Move()
    {
        Vector3 temPos = pos; // Записать текущие координаты в переменную temPos
        temPos.y -= speed * Time.deltaTime;// переместить объект вниз
        pos = temPos;//Переместить корабль
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;// Получить ссылку на игровой объект что столкнулся с врагом
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();//Добуть компонент Projectile(код) игрового объекта

                //Если вражеский корабль за границами экрана
                //Не наносить ему повреждений

                if(!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);//Уничтожить снаряд
                    break;
                }

                //Поразить вражеский корабль 
                //Получить разрушающую силу в класе Main
                health -= Main.GetWeaponDefinion(p.type).damageOnHit;//Нанести урон
                if(health<=0)
                {
                    //Если жизни меньше чем 0 уничтожить врага
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);//Уничтожить снаряд
                break;
            default:
                print("Enemy hit by non-Projectile " + otherGO.name);
                break;
        }
    }
}
