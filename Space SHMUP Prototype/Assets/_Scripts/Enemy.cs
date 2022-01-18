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

    //Реакция на попадание параметры 
    public float showDamageDuration = 0.1f;//Длительность эфекта попадания в секунду
    public float powerUpDropChance = 1f;//Шанс сбросить бонус
    [Header("Set Dynamically")]
    public Color[] originalColors;//Здесь будут хранитса оригинальные цвета игрового объета Enemy и его дочерних класов
    public Material[] materials;//Все материалы игрового объекта и его потомков
    public bool showingDamage = false;//Сообщает если true - значит игровой объект окрашен в красный, если false - нет
    public float damageDoneTime;//Время прекращение отображение эфекта
    public bool notifiedOfDestruction = false;// Будет использоватса позже - когда нада збросить бонус
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
        //Получить ссылку на компонент сценария BoundsCheck что подключон к этому игровому объекту

        //Получить материалы и цвет этого объекта и его потомков
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];//Инициализировать масив для записи оригинальных цветов объекта
        for(int i =0; i<materials.Length; i++)
        {
            /*
             * С помощью цикла выполняетса обход всех материалов и сохраняет их исходные цвета 
             * в масив originalColors
             */
            originalColors[i] = materials[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();// Перемещать корабль вниз 
        if(showingDamage && Time.time> damageDoneTime)
        {
            //Если showingDamage = true; и
            //Time.time> damageDoneTime то есть
            //если время больше за время прекращение ефекта отображение попадания  - прекратить ефект
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)
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
                ShowDamage();//Отобразить попадание
                //Получить разрушающую силу в класе Main
                health -= Main.GetWeaponDefinion(p.type).damageOnHit;//Нанести урон
                if(health<=0)
                {
                    //Сообщить Main про уничтожение корабля
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);//Создать бонус
                    }
                    notifiedOfDestruction = true;
                    //Если жизни меньше чем 0 уничтожить врага
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);//Уничтожить снаряд
                break;
            case "Rocket"://Если столкнулся с ракетой 
                Rocket r = otherGO.GetComponent<Rocket>();//Добуть компонент Rocket(код) игрового объекта
                //Если вражеский корабль за границами экрана
                //Не наносить ему повреждений

                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);//Уничтожить снаряд
                    break;
                }

                //Поразить вражеский корабль
                ShowDamage();//Отобразить попадание
                //Получить разрушающую силу ракеты в класе Main
                health -= Main.GetWeaponDefinion(r.type).damageOnHit;
                if (health <= 0)
                {
                    //Сообщить Main про уничтожение корабля
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);//Создать бонус
                    }
                    notifiedOfDestruction = true;
                    //Если жизни меньше чем 0 уничтожить врага
                    Destroy(this.gameObject);
                }
                print("Boom");
                Destroy(otherGO);//Уничтожить снаряд
                break;
            default:
                print("Enemy hit by non-Projectile " + otherGO.name);
                break;
        }
    }

    /// <summary>
    /// Метод ShowDamage - отображает реакцию на попадание 
    /// путьом перекрашивание всех материалов объекта в красный цвет
    /// </summary>
    void ShowDamage()
    {
        foreach(Material m in materials)
        {
            //В цикле происходит обход всех материалов и перекрашивают в красний цвет
            m.color = Color.red;
        }
        showingDamage = true;//Установить что попадание отображено
        damageDoneTime = Time.time + showDamageDuration;//Вычислить время окончания ефекта
    }

    /// <summary>
    /// Функция UnShowDamage - прекращает ефект реакции попадания
    /// путьом перекрашивание всех материалов объекта в исходный цвет
    /// </summary>
    void UnShowDamage()
    {
        for(int i = 0; i<materials.Length; i++)
        {
            //В цикле происходит обход всех материалов и перекрашивают в исходный цвет
            materials[i].color = originalColors[i];
        }
        showingDamage = false;//Устанавливает что эфект закончился
    }
}
