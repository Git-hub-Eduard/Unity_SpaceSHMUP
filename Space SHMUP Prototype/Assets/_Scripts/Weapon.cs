using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Это перечисление всех возможных типов оружия
/// Также включает тип "shield", чтобы дать возможность совершенствувать защиту.
/// Абревиатурой [HP] ниже отмечены элементы, не риализованы в этой книге
/// </summary>

public enum WeaponType
{
    none,//По умолчанию / нет оружия
    blaster,// Простой бластер
    spread,//Веерная пушка, стреляющая несколькими снарядами
    phares,//[HP] Волновой фазер
    missile,//[HP] Самонаводящиеся ракеты 
    laser,//[HP] Наносит повреждение при долговременном воздействии
    shield,//Увеличивает shieldLevel
}
/// <summary>
/// Класс WeaponDefinition позволяет настраивать свойства
/// конкретного вида оружия в инспекторе. Для этого класс Main
/// будет хранить массив элементов типа WeaponDefinition.
/// </summary>
[System.Serializable]
public class WeaponDefinion
{
    public WeaponType type = WeaponType.none;
    public string letter;// Буква на кубике, изображающего бонус.
    public Color color = Color.white;// Цвет ствола оружия, и кубика бонуса
    public GameObject projectilePefab;// Шаблон снарядов 
    public Color projectileColor = Color.white;// цвет снаряда
    public float damageOnHit = 0;//разрушительная мощность
    public float continuousDamage = 0;//Степень разрушения в секунду для Laser
    public float delayBetwenshots = 0;
    public float velocity = 20;//Скорость полета снаряда 
}
public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;//Играет роль родителя для всез созданых снарядов
    [Header("Set Dyamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;// Начальный тип оружия 
    public WeaponDefinion def;//Свойства оружия(скорость, цвет, шаблон текстури урон)
    public GameObject collar;// Игровой объект  - Дуло с которого корабль будет стрелять
    public float lastShotTime;// Время последнего выстрела
    private Renderer collarRend;// Что б изменять цвет дула в соотвецтвии с типом оружия
    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;//Находим дочерий объект Collar родительского объекта Weapon 
        collarRend = collar.GetComponent<Renderer>();//Получить ссылку на компонент Renderer дочернего объекта Collar

        //Вызвать SetType() что б изменить тип оружия по умолчанию на WeaponType.none;
        SetType(_type);//Установить тип оружия по умолчанию
        //Динамически создать точку привязки для всех снарядов
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        GameObject rootGO = transform.root.gameObject;//Получит ссылку на родительский игровой объект Hero
        if(rootGO.GetComponent<Hero>() != null)//Проверить наличие сценария Hero в игровом объекте Hero
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;//Добавляем метод Fire в делегат fireDelegate класса Hero
        }
    }
    public WeaponType type//Для изменения _type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    /// <summary>
    /// Этот метод изменяет тип оружия, делает его активным либо нет, изменяет цвет дула оружия 
    /// </summary>
    /// <param name="wt"> Тип оружия на который нужно изменить сейчас</param>
    public void SetType(WeaponType wt)
    {
        _type = wt;//Изменить тип оружия на новый тип что хранитса в переменной переданной через WeaponType wt
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);//Зделать оруже не активным 
        }
        else
        {
            this.gameObject.SetActive(true);//В другом случае зделать активным
        }
        def = Main.GetWeaponDefinion(_type);//Найты экземпляр WeaponDefinition что соотвецтвует типу оружия _type
        collarRend.material.color = def.color;//Изменить цвет дула в соотвецтвии с типом оружия 
        lastShotTime = 0; //Сразу после установкиморужия  можна выстрелить 
    }

    /// <summary>
    /// Метод что дает возможность стрелять 
    /// </summary>
    public void Fire()
    {
        //Если this.gameObject не активен выйти 
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        //Если между выстрелам прошло не достаточно времени выйти 
        if ((Time.time - lastShotTime) < def.delayBetwenshots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;//Устанавливаетса начальная скорость снаряда 
        if(transform.up.y<0)
        {
            vel.y = -vel.y;
        }
        switch(type)
        {
            case WeaponType.blaster://Если тип оружия бластер
                p = MakeProjectile();//Создать 1 снаряд 
                p.rigid.velocity = vel;//Придать снаряду ускорения 
                break;
            case WeaponType.spread:
                p = MakeProjectile();//Снаряд летящий прямо
                p.rigid.velocity = vel;//Придать снаряду ускорения 
                p = MakeProjectile();//Снаряд летящий в право
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);//Повернуть снаряд в право на 10 градусов
                p.rigid.velocity = p.transform.rotation * vel;//Придать снаряду ускорения под углом в право
                p = MakeProjectile();//Снаряд летящий в лево
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);//Повернуть снаряд в лево на 10 градусов
                p.rigid.velocity = p.transform.rotation * vel;//Придать снаряду ускорения под углом в лево
                break;
        }
    }

    /// <summary>
    /// Метод что создает снаряд для определенного типа оружия 
    /// </summary>
    /// <returns> возвращает ссылку на сценарий Projectile созданого снаряда </returns>
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//Получить ссылку на снаряд и создать его для соотвецтвующего типа оружия 
        if(transform.parent.gameObject.tag == "Hero")//Проверить если родительский объект имеет тег Hero
        {
            //Снаряду присвоить соотвецтвующий теш и физичексий уровень
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;//Установить созданыйснаряд у Дула оружия 
        go.transform.SetParent(PROJECTILE_ANCHOR,true);
        Projectile p = go.GetComponent<Projectile>();//Установить ссылку на сценарий Projectile снаряда
        p.type = type;// Присвоить тип оружия  снаряду
        lastShotTime = Time.time;//Присвоить текущее время 
        return (p);// возвращает ссылку на сценарий Projectile созданого снаряда
    }
}
