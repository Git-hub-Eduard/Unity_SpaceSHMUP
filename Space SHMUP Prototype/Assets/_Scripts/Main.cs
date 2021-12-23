using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;// объект одиночка
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;// Масив шадлонов Enemy
    public float enemySpawnPerSecond = 0.5f;// Создание вражеских кораблей за еденицу времени
    public float enemyDefaultPadding = 1.5f;
    //Отступ для позиционирования.
    private BoundsCheck bndCheck;
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //Записать в  bndCheck ссылку на компонент BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        //Вызвать SpawnEnemy
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void SpawnEnemy()
    {
        // Выбрать случайный шаблон Enemy для создания 
        int ndx = Random.Range(0, prefabEnemies.Length);//выбрать сулчайный шаблон 
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);// создать объект из масива prefabEnemies под индексом ndx
        //Разместить вражеский корабль над экраном в случайной позиции х
        float enemyPadding = enemyDefaultPadding;// записать enemyPadding начальный отступ от границ
        if (go.GetComponent<BoundsCheck>()!= null)//Проверить есть ли у объекта компонент BoundsCheck 
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
        //Снова вызвать SpawnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        //  Вызвать метод Restart() через delay секунд
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
