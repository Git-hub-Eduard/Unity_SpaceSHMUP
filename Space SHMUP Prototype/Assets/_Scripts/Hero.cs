using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;//одиночка
    [Header("Set in Inspector")]
    //Поля управляющие движением корабля 
    public float speed = 30;//Скорость движения корабля
    public float rollMult = -45;//Поворот кооабля по оси Х
    public float pitchMult = 30;//Поворот корабля по оси У
    public float RestartDelay = 2f;//Через сколько перезагрузить игру
    public GameObject projectilePrefab;//Шаблон снаряда
    public float projectileSpeed = 40;//Скорость снаряда 
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;//переменная хранит ссылку на последний столкнувшийся игровой объект  
     void Awake()
     {
       if(S==null)
        {
            S = this;//Сохранить ссылку на одиночку
        }
       else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");// Прописать ошибку если создан ещё один экземпляр Hero S
        }
     }
    // Update is called once per frame
    void Update()
    {
        //Извлечить информацию из класа Input
        float xAxis = Input.GetAxis("Horizontal");//собрать ифнормацию при нажатии кнопок лево или в право
        float yAxis = Input.GetAxis("Vertical");//собрать ифнормацию при нажатии кнопок в низ или в верх
        /*
         * Если нажать кнопку то  в xAxis запишет -1 и 1, в лево или в право соответственно
         * Если нажать кнопку то  в yAxis запишет -1 и 1, в низ или вверх
         */

        //Изменить transform.position, опираясь на информацию по осям 
        Vector3 pos = transform.position;// записать текущии координаты корабля 
        pos.x += xAxis * speed * Time.deltaTime;//Записать координаты перемещения
                                                //объекта по оси Х путьом умножения xAxis * speed * Time.deltaTime
        pos.y += yAxis * speed * Time.deltaTime;//Записать координаты перемещения
                                                //объекта по оси Y путьом умножения yAxis * speed * Time.deltaTime
        transform.position = pos;//переместить корабль в только что записаные координати
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        /*
         * Строка transform.rotation... сообщает объекту под каким углом наклонять корабль при движении
         * Функция Quaternion.Euler - 1 переменная  - поварачивает корабль, когда он двигаетса   вверх или вниз, по оси Х
         * Функция Quaternion.Euler - 2 переменная - поварачивает корабль когда он двигаетса влево или в право, по оси У 
         */

        //Позволить кораблю выстрелить 
        if(Input.GetKeyDown(KeyCode.Space))// При нажатии пробела стрелять
        {
            TempFire();
        }
    }
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);//Создать снаряд
        projGO.transform.position = transform.position;//Установить кординаты что и у корабля 
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();// Получить компонент Rigidbody у снаряда
        rigidB.velocity = Vector3.up * projectileSpeed;// Дать снаряду ускорения
    }
    void OnTriggerEnter(Collider other)//Срабатывает при столкновении колайдера игрока с другими объектами 
    {
        Transform rootT = other.gameObject.transform.root;//Передать компонент Transform объекта родителя
        GameObject go = rootT.gameObject;//Передать ссылку родительского объекта в переменную go
        //print("triggered: " + go.name);
        if(go == lastTriggerGo)
        {
            //Если lastTriggerGo ссылаетса на тот же объект что и go
            //Это столкновение игнорируетса - как повторное
            return;// и завершает функцию
        }
        lastTriggerGo = go;//передать ссылку объекта из  go в lastTriggerGo
        if(go.tag=="Enemy")//Проверить если игровой объект с которым столкнулся игрок являетса ли он врагом
        {
            shieldLevel--;//Уменьшить уровень защиты на 1
            Destroy(go);//Уничтожить врага
        }
        else//Если игрок столкнулся с другим объектом не имеющего тега Enemy
        {
            print("Triggered by non-Enemy: " + go.name);// написать ообщение, что б можно было узнать об этом
        }
    }
    public float shieldLevel
    {
        get//читать свойство которое возвращает занчение _shieldLevel
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);// гарантирует что _shieldLevel никогда не получить значение выше 4
            //Если уровень поля упал до нуля или ниже 
            if (value<0)
            {
                Destroy(this.gameObject);//уничтожить корабль игрока
                Main.S.DelayedRestart(RestartDelay);//Перезагрузить игру
            }
        }
    }
}
