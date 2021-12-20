using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;//Скорость вращение щита
    [Header("Set Dynamically")]
    public int levelShown = 0;
    // Скрытая переменная не появляетса в инспекторе
    Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;// получить компонент Renderer для счита 
    }

    // Update is called once per frame
    void Update()
    {
        //Прочитать текущую мощность защитного поля 
        // из объекта  - одиночки Hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);//Получить текущую мощность щита 
        //Функция  Mathf.FloorToInt - округляет вниз до ближайшено целого, и резултат присваиваетса currLevel
        //Если текущие состояние щита игрока(currLevel) отличаетса от levelShown
        if(levelShown != currLevel)
        {
            levelShown = currLevel;// Перезаписать текущее состояние щита в levelShown
            //Скоректировать  смещение в текстуре, что бы отобразить поле  с другой мощностю
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //Поварачивать поле в каждом кадре с постоянной скоростью
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
