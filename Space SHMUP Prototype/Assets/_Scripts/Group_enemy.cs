using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group_enemy : MonoBehaviour
{
    public float Delay = 0.5f;//Время между активацией врагов
    private float start_time;//Начальное время
    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;//Завиксировать начальное время 
        StartCoroutine(StartEnemy());//Запустить акивацию игровых объектов
    }


    /// <summary>
    /// Функция что через переиод времени делает дочерние объекты активными
    /// </summary>
    /// <returns></returns>
    IEnumerator StartEnemy()
    {
        for (int i = 0;i<transform.childCount;i++)//Цикл что проходит по всем дочерним объектам иерархии
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);//Зделать объект активным
            yield return new WaitForSeconds(Delay);//Подождать Delay времени 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if((Time.time-start_time)>9f)//Проверить если прошло больше 9 секунд 
        {
            Destroy(gameObject);//Уничтожить игровой объект 
        }
    }
}
