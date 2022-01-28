using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    public GameObject[] EnemyPrefabs;//Виды врагов на волне
    public int sizeWave; // Количество врагов
}
public class Main : MonoBehaviour
{
    static public Main S;// объект одиночка
    static Dictionary<WeaponType, WeaponDefinion> WEAP_DICT;// объявление словаря
    [Header("Set in Inspector")]
    public float enemySpawnPerSecond = 0.5f;// Создание вражеских кораблей за еденицу времени
    public float enemyDefaultPadding = 1.5f;
    //Отступ для позиционирования.
    public WeaponDefinion[] weaponDefinions;// Масив оржия
    //Бонусы
    public GameObject prefabPowerUp;//шаблон для всех бонусов
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };
    /*
     * powerUpFrequency - масив типов оружия для бонусов
     */
    //Волны
    public Wave[] Level;//Масив волн
    private BoundsCheck bndCheck;
    /// <summary>
    /// Данный метод создает бонус на месте уничтоженого корабля
    /// </summary>
    /// <param name="e">Экземпляр врга кторый был уничтожен</param>
    public void ShipDestroyed(Enemy e)
    {
        //Сгенерировать бонус с заданой вероятностю
        if(Random.value <= e.powerUpDropChance)
        {
            //Выбрать тип бонуса
            //Выбрать один из елементов в powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            //Создать экземпляр PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            //Установить соотвецтвующий тип WeaponType
            pu.SetType(puType);
            //Поместить в место, где находитса разрушеный корабль
            pu.transform.position = e.transform.position;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //Записать в  bndCheck ссылку на компонент BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        //Вызвать Scenary()
        StartCoroutine(Scenary());

        //Словарь с ключами типа WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinion>();//Инициализировать словарь
        foreach(WeaponDefinion def in weaponDefinions)
        {
            WEAP_DICT[def.type] = def;//Записать в словарь елмент что соотвецтвует его типу вооружения 
        }
    }

    /// <summary>
    /// Функция что отвечает за перебор появление врагов по волнам
    /// </summary>
   IEnumerator Scenary()
   {
        foreach(Wave spawnWave in Level)//Перебрать все волны из масива Level
        {
            for(int i = 0; i<spawnWave.sizeWave; i++)//Через цикл вызывать _SpawnEnemy с задержкой WaitForSeconds(1f / enemySpawnPerSecond);
            {
                //Создать spawnWave.sizeWave(количество врагов на волне) врагов в волне
                yield return new WaitForSeconds(1f / enemySpawnPerSecond);//подождать 1f / enemySpawnPerSecond
                _SpawnEnemy(spawnWave);//Создать врага
            }
        }
    }

    void _SpawnEnemy(Wave spawnWave)
    {
        // Выбрать случайный шаблон Enemy для создания 
        int ndx = Random.Range(0, spawnWave.EnemyPrefabs.Length);//выбрать сулчайный шаблон 
        GameObject go = Instantiate<GameObject>(spawnWave.EnemyPrefabs[ndx]);// создать объект из масива prefabEnemies под индексом ndx
        //Разместить вражеский корабль над экраном в случайной позиции х
        float enemyPadding = enemyDefaultPadding;// записать enemyPadding начальный отступ от границ
        if (go.GetComponent<BoundsCheck>() != null)//Проверить есть ли у объекта компонент BoundsCheck 
        {
            //Если есть записать в enemyPadding  - radius собственный параметр компонента  
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;// Созадть переменную pos типа Vector3 и присвоить начальные координаты (0,0,0)
        float xMin = -bndCheck.camWidth + enemyPadding;//Определить минимальное значение отрезка по оси х где может находитса объект
        float xMax = bndCheck.camWidth - enemyPadding;//Определить максимальное значение отрезка по оси х где может находитса объект
        pos.x = Random.Range(xMin, xMax);//Выбрать случайную координату между xMin и xMax включительно
        pos.y = bndCheck.camHeight + enemyPadding;// Определить высоту на которой может находитса объект
        go.transform.position = pos;//Расположить объект на заданые координаты pos
    }
  

    /// <summary>
    /// Перезагружает сцену через определённое количество времени
    /// </summary>
    /// <param name="delay"></param>
    public void DelayedRestart(float delay)
    {
        //  Вызвать метод Restart() через delay секунд
        Invoke("Restart", delay);
    }

    /// <summary>
    /// Перезагружает сцену
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }



    /// <summary>
    /// Статическая функция, возвращающая WeaponDefinition из статического
    /// защищенного поля WEAP_DICT класса Main.
    /// </summary>
    /// <returns>
    /// Экземпляр WeaponDefinition или, если нет такого определения
    /// для указанного WeaponType, возвращает новый экземпляр WeaponDefinition
    /// с типом none
    /// </returns>
    /// <param name="wt"> Tnn оружия WeaponType, для которого требуется получить WeaponDefinition</param>
    /// <returns></returns>
    static public WeaponDefinion GetWeaponDefinion(WeaponType wt)
    {
        //Проверит наличие указаного клуюча в словаре 
        if(WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);// Возвращать экземпляр что соответствует определееному типу орижия
        }

        //Возвратить новый экзмпляр с типом оружия WeaponType.none  если не удалось найти в словаре 
        return (new WeaponDefinion());
    }

}
