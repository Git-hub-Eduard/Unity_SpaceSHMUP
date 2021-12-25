using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    //Число секунд полного цикла синусоиды
    public float waveFrequency = 2;

    //Ширина синусоиды в метрах
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float x0;// начальное значение кординат
    private float birthTime;

    // Метод страт Start() потому что не используетса супер класом Enemy
    void Start()
    {
        // Установить начальную позицию координат х объекта Enemy_1
        x0 = pos.x;
        birthTime = Time.time;
    }
    //Переопределить  функцию Move суперкласса Enemy
    public override void Move()
    {
        Vector3 temPos = pos;// записать переменную pos из родительского класа Enemy
        float age = Time.time - birthTime;
        //Значение theta изменяетса с течением времени  
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        temPos.x = x0 + waveWidth * sin;
        pos = temPos;
        //Повернуть немного относительно оси  Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        //
        // Движение по оси y
        base.Move();
    }

}
