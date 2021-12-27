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
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;//Передать ссылку в otherGO на игровой объект что столкнулся с ним
        if(otherGO.tag == "ProjectileHero")//Проверит являетса ли игровой объект что столкнулся с ним - снаряд
        {
            Destroy(otherGO);//Если это снаряд то уничтожить сам снаряд 
            Destroy(gameObject);// И объект что столкнулся с снарядом
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);// В ином случае сообщить об этом
        }
    }
}
