using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private BoundsCheck bndCheck;
    private Renderer rend;// Для изменения цвет
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;//Тип оружия 
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);//Функция присваивание
        }
    }
    void Awake()// Перед созданием объекта на сцене
    {
        bndCheck = GetComponent<BoundsCheck>();//получить ссылку на компонент BoundsCheck
        rend = GetComponent<Renderer>();// получить ссылку на компонент Renderer
        rigid = GetComponent<Rigidbody>();//получить ссылку на компонент Rigidbody
    }
    // Update is called once per frame
    void Update()
    {
        if(bndCheck.offUp)// Если пересёк вехнюю границу экрана 
        {
            Destroy(gameObject);//Уничтожить снаряд
        }
        if(bndCheck.offDown)// Если пересёк нижнюю границу экрана
        {
            Destroy(gameObject);//Уничтожить снаряд
        }
    }

    /// <summary>
    /// Изменяет скрытое поле _type и устанавливает цвет этого снаряда;
    /// как определено в WeaponDefinition.
    /// </summary>
    /// <param name="eType"> Тип WeaponType используемого оружия </param>
    public void SetType(WeaponType eType)
    {
        //установить _type
        _type = eType;//Изменить тип оружия 
        WeaponDefinion def = Main.GetWeaponDefinion(_type);// Получить экземпляр WeaponDefinion что соотвецтвует типу оружия
        rend.material.color = def.projectileColor;//Изменить цвет оружия который задан в масиве
    }
}
