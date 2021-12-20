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
    [Header("Set Dynamically")]
    public float shieldLevel = 1;
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
         */
    }
}
