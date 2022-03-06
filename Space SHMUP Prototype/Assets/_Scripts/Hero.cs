using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    static public Hero S;//одиночка
    [Header("Set in Inspector")]
    //Поля управляющие движением корабля 
    public float speed = 30;//Скорость движения корабля
    public float rollMult = -45;//Поворот кооабля по оси Х
    public float pitchMult = 30;//Поворот корабля по оси У
    public float RestartDelay = 2f;//Через сколько перезагрузить игру
    //public GameObject projectilePrefab;//Шаблон снаряда
    public int missileSize = 0;//Количество ракет
    private float missileTime = 0;//Время последнего выстрела
    private WeaponDefinion def;//Свойства ракеты 
    public float projectileSpeed = 40;//Скорость снаряда 
    public Weapon[] weapons;//Масив будет хранить ссылки на каждое оружие
    public GameObject[] Turrets;//Масив турелей
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1f;
    private float health = 1;
    private GameObject lastTriggerGo = null;//переменная хранит ссылку на последний столкнувшийся игровой объект

    [Header("Эфекты")]
    //еффекты 
    public GameObject effectParticles;//Игровой объект частици
    private Material[] materialsDamge;
    //Интерфейс
    public Text MissileText;//Для изображения количества ракет

    [Header("Управление")]
    public bool IsTouch = false;
    private bool readyMove;
    //Объявление нового делегата WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Создание поле типа WeaponFireDelegate с именем fireDelegate
    public WeaponFireDelegate fireDelegate;
    void Start()
    {
       materialsDamge = GetHeroMaterials.GetAllMaterials(gameObject);
       if(S==null)
       {
           S = this;//Сохранить ссылку на одиночку
       }
       else
       {
           Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");// Прописать ошибку если создан ещё один экземпляр Hero S
       }
       
       def = Main.GetWeaponDefinion(WeaponType.missile);
       MissileText.gameObject.SetActive(true);
       UI_Updaye();//Обновить интерфейс
       // Очистить массив weapons и начать игру с 1 бластером
       ClearWeapons();
       ClearTurrets();
       weapons[0].SetType(WeaponType.blaster);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsTouch)//Если это сенсорное управление
        {
            if(Input.touchCount>0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;
                if (touch.phase == TouchPhase.Began)
                {

                    if (touchPosition.y<=transform.position.y+5 && touchPosition.y >= transform.position.y - 5 && touchPosition.x<=transform.position.x+4 && touchPosition.x >= transform.position.x-4)
                    {
                        
                        readyMove = true;
                        
                    }
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    if (readyMove)
                    {
                       transform.position = touchPosition;
                       Instantiate(effectParticles, transform.position, Quaternion.identity);//Создать еффект
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    readyMove = false;
                }
                
            }
        }
        else//Если это через клавишы
        {
            //Извлечить информацию из класа Input
          float xAxis = Input.GetAxis("Horizontal");//собрать ифнормацию при нажатии кнопок лево или в право
          float yAxis = Input.GetAxis("Vertical");//собрать ифнормацию при нажатии кнопок в низ или в верх
            /*
             * Если нажать кнопку то  в xAxis запишет -1 и 1, в лево или в право соответственно
             * Если нажать кнопку то  в yAxis запишет -1 и 1, в низ или вверх
             */
            if (xAxis != 0f || yAxis != 0f)//Проверить нажата ли кнопка  в лево или в право в верх в низ
            {
                Instantiate(effectParticles, transform.position, Quaternion.identity);//Создать еффект
            }
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
        }


        //Позволить кораблю выстрелить 
        // if(Input.GetKeyDown(KeyCode.Space))// При нажатии пробела стрелять
        // {
        //     TempFire();
        //}
        if (IsTouch)
        {
            fireDelegate();//вызов делегата  к кторой подключон метод  TempFire       
            CreateRocket();//Создать ракету
        }
        else
        {
            //Произвести выстрел из всех видов оружия вызовом fireDelegate
            //Сначала произвести нажатие клавиши Axis("Jump")
            //Затем убедитса что fireDelegate не равно null
            //Чтобы избежать ошибки
            if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
            {
                //Если была нажата клавиша то Input.GetAxis("Jump") вернёт 1
                fireDelegate();//вызов делегата  к кторой подключон метод  TempFire

            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))//Если игрок нажал кнопку F
            {
                CreateRocket();//Создать ракету
            }
        }
       
    }
  

    /// <summary>
    /// Функция что создает ракету
    /// </summary>
    void CreateRocket()
    {
        if(missileSize == 0)//Если количество ракет равно 0
        {
            return;
        }
        else if((Time.time-missileTime)<def.delayBetwenshots)//Проверить прошло ли достаточно времени что было дозволено создать ракету
        {
            return;
        }
        else
        {
            GameObject missile = Instantiate<GameObject>(def.projectilePefab);//Создать ракету 
            missile.transform.position = transform.position;//Расположить на месте корабля 
            missileSize--;//отнять количество ракет
            UI_Updaye();//Обновить интерфейс
            missileTime = Time.time;
        }
       
    }

    /// <summary>
    /// Функция для обновления интерфейса
    /// </summary>
    public void UI_Updaye()
    {
        
        MissileText.text = "M: " + missileSize;
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
        else if(go.tag == "ProjectileEnemy")//Проверить если это вражеский снаряд
        {
            shieldLevel--;//Уменшить уровень защиты 
            Destroy(go);//Уничтожить снаряд
        }
        else if(go.tag == "PowerUp")//Проверить если игровой объект с которым столкнулся игрок являетса ли он бонусом
        {
            AbsorpPowerUp(go);
        }
        else//Если игрок столкнулся с другим объектом не имеющего тега Enemy
        {
            print("Triggered by non-Enemy: " + go.name);// написать ообщение, что б можно было узнать об этом
        }
    }

    /// <summary>
    /// Выполняетса когда игрок сталкиваетса с бонусом
    /// </summary>
    /// <param name="go">Игровой объект бонус</param>
    public void AbsorpPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();//получить ссылку на компонент PowerUp
        switch (pu.type)
        {
            case WeaponType.shield://Если тип бонуса щит 
                shieldLevel++;//Прибавить к щиту 1
                break;
            case WeaponType.missile:
                missileSize = missileSize+2;//Добавить 2 ракеты
                UI_Updaye();//Обновить интерфейс
                break;
            case WeaponType.turret:
                AddTuretSlot();
                break;
            default:
                if(pu.type == weapons[0].type)
                {
                    //Если оружие того же типа что и бонус
                    Weapon w = GetEmptyWeaponSlot();//Находим пустой слот оружия 
                    if(w!= null)
                    {
                        //Установить оружие на пустой слот
                        w.SetType(pu.type);
                    }
                }
                else//Если оружие другого типа 
                {
                    ClearWeapons();//Очистить все слоты оружия
                    weapons[0].SetType(pu.type);//Установить новое оружие
                }
                break;

        }
        pu.AbsorbedBy(this.gameObject);
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
                _shieldLevel = 0;
                health = health - 0.25f;//Убрать жизнь 
                DetectedDamage(health);//Нанести повреждения
               if(health == 0)//Если жизней 0 уничтожить корабль
               {
                    Destroy(this.gameObject);//уничтожить корабль игрока
                    Main.S.DelayedRestart(RestartDelay);//Перезагрузить игру
               }
                
            }
        }
    }

    /// <summary>
    /// Функция перебирает елементи масива в ктором находятса оружия корабля
    /// и находит оружия  с типом WeaponType.none
    /// </summary>
    /// <returns>Возвращает оружие в которого тип WeaponType.none</returns>
    Weapon GetEmptyWeaponSlot()
    {
        for(int i =0;i<weapons.Length; i++)//Обойти все елементи(оружия) масива
        {
            if(weapons[i].type == WeaponType.none)//Проверить если тип оружия равно WeaponType.none
            {
                return (weapons[i]);//Возвращать оружие в которого тип WeaponType.none
            }
        }
        return (null);//Возвращать null если нет оружия с типом WeaponType.none
    }

    /// <summary>
    /// Функция убирает оружие у всего корабля, 
    /// то есть присваивает WeaponType.none
    /// </summary>
    void ClearWeapons()
    {
        foreach(Weapon w in weapons)//Перебирает все елементи масива weapons
        {
            w.SetType(WeaponType.none);//Присваивает каждому елементу тип WeaponType.none
        }
    }


    /// <summary>
    /// Очищает все слоты турелей
    /// </summary>
    void ClearTurrets()
    {
        foreach(GameObject go in Turrets)
        {
            go.SetActive(false);
        }
    }


    /// <summary>
    /// Добавляет слот для турели
    /// </summary>
    void AddTuretSlot()
    {
        foreach(GameObject go in Turrets)
        {
            if(go.activeSelf == false)
            {
                go.SetActive(true);
                return;
            }
        }
    }


    void DetectedDamage(float damage)
    {
        foreach(Material mat in materialsDamge)
        {
            mat.SetFloat("Health", damage);
        }
    }
}
