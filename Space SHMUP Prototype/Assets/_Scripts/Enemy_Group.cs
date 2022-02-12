using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Group : Enemy
{
    public AnimationCurve trayctori;//Переменная траектории движения AnimationCurve
    public bool IsRight = false;//Определить с какой стороны будет вылетать враг
    public float currenttime;//Время  траектории  применяется для изменения кординаты х
    public float lasttime;//Последняя точка графика  trayctori;
    public float speedG = 10f;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsRight)//Если враг вылетает с лева 
        {
            currenttime = trayctori.keys[0].time;//Установить первую точку графика по х
            lasttime = trayctori.keys[trayctori.keys.Length - 1].time;//Установить последнюю точку графика по х
        }
        else//Если враг вылетает с права
        {
            currenttime = trayctori.keys[trayctori.length - 1].time;//Установить первую точку графика по х
            lasttime = trayctori.keys[0].time;//Установить последнюю точку графика по х
        }
        
    }

    // Update is called once per frame
    public override void Move()
    {
        Vector3 tempos = new Vector3(currenttime, trayctori.Evaluate(currenttime));//Определить новую точку перемещения
        pos = tempos;//Переместить объект 
        if (!IsRight)//Если враг вылетает с лева 
        {
            currenttime += Time.deltaTime * speed;//Изменить значение оси х(времени)
            if (currenttime > lasttime)//Если  вышло за граници по х за последнюю точку  
            {
                Destroy(gameObject);//Уничтожить объект 
            }
        }
        else//Если враг вылетает с права
        {
            currenttime -= Time.deltaTime * speed;//Изменить значение оси х(времени)
            if (currenttime < lasttime)//Если  вышло за граници по х за последнюю точку 
            {
                Destroy(gameObject);//Уничтожить объект 
            }
        }
        
    }
}
