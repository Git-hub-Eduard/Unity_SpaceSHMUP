using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 25);//для хранения минимального и максимального значения для метода Random.Range
    public Vector2 driftMinMax = new Vector2(.25f, 2); //для хранения минимального и максимального значения для метода Random.Range
    public float lifeTime = 6f;//Время в секундах существования PowerUp
    public float fadeTime = 4f;//Время исчезновения PowerUp

    [Header("Set Dynamically")]
    public WeaponType type;//Тип бонуса
    public GameObject cube;//Ссылка на вложенный куб
    public TextMesh letter;// Ссылка на TextMesh
    public Vector3 rotPerSecond;//Скорость вращения
    public float birthTime;// Время создания
    private Rigidbody rigid;//Ссылка на компонент Rigidbody
    private BoundsCheck bndCheck;//Ссылка на компонент BoundsCheck
    private Renderer cubeRend;
    // Start is called before the first frame update
    void Awake()
    {
        //Получить ссылку на куб
        cube = transform.Find("Cube").gameObject;
        //Получить ссылку на TextMesh
        letter = GetComponent<TextMesh>();
        //Получить ссылку на Rigidbody
        rigid = GetComponent<Rigidbody>();
        //Получить ссылку на BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //Выбрать случайную скорость 
        Vector3 vel = Random.onUnitSphere;//Получить случайную скорость XYZ
        /*
         * Random.onUnitSphere - возвращает вектор, указывающий на случайную точку,
         * находящуюся на поверхности сферы  с радиусом 1 и центром в точке начала кординат
         */
        vel.z = 0;//Отображать vel на плоскости XY
        vel.Normalize();//Нормализация устанавливаем длину  Vector3 равной 1 
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);//Выбор случайной скорости 
        rigid.velocity = vel;

        //Установить угол поворота этого игрового объекта равным R[0,0,0]
        transform.rotation = Quaternion.identity;// Отсутствие поворота 
        //Выбрать случайную скорость вращения куба
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;//Время создания 
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);// Поворот куба в каждом кадре

        //Эфект растворения куба PowerUp с течением времени
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //В течении lifeTime секунд значение u <=0.
        //Если текущее время будет больше за birthTime + lifeTime u>=0
        //Если текущее время будет больше за (birthTime + lifeTime)+fadeTime u>=1
        if(u>=1)
        {
            Destroy(this.gameObject);
            return;

        }
        if(u>0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;

            //Буква тоже должна растворятса но медленнее
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if(!bndCheck.isOnScreen)//Если бонус за экраном уничтожить
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Функция для изменения типа оружия
    /// для бонуса
    /// </summary>
    /// <param name="wt"> Тип на которий нужно изменить бонус</param>
    public void SetType(WeaponType wt)
    {
        //Получить weaponDefinion из Main
        WeaponDefinion def = Main.GetWeaponDefinion(wt);
        //Установить цвет дочернего куба 
        cubeRend.material.color = def.color;
        //letter.color = def.color;
        letter.text = def.letter;
        type = wt;//Установить фактический тип оружия 
    }

    /// <summary>
    /// Эта функция вызывается классом Него, когда игрок подбирает бонус
    /// </summary>
    /// <param name="target"></param>
    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
