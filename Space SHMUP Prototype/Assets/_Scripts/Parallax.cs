using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject poi;//Корабль игрока
    public GameObject[] panels;//Прокручиваемие панели
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;
    private float panelHT;//Высота каждой панели
    private float depth; //Глубина панелей (z)
    // Start is called before the first frame update
    void Start()
    {
        panelHT = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;

        //Установить панели на начальные позиции
        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHT, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHT + (panelHT * 0.5f);
        if(poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }
        // Сместить панель panels[0]
        panels[0].transform.position = new Vector3(tX, tY, depth);
        // Сместить панель panels[1], чтобы создать эффект непрерывности
        if(tY>=0)
        {
            panels[1].transform.position = new Vector3(tX, tY - panelHT, depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + panelHT, depth);
        }
    }
}
