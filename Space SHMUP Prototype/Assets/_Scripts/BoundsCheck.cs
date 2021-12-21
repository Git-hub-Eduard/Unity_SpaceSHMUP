using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Предотвращает выход игрового объекта за границы экрана. 
///     Важно: работает ТОЛЬКО с ортографической камерой Main Camera в [ 0, 0, 0 ].
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;// На сколько корабль может заходить за границу экрана
    public bool keepOnScreen = true;
    //Переменная keepOnScreen сообщает должен ли объект оставатса на экране
    /*
     * true - должен оставатса на экране всегда(Hero)
     * false - не должен оставатса на экране(Enemy)
     */
    [Header("Set Dynamically")]
    public bool isOnScreen = true;//переменная что сообщает он на экране или нет
    /*
     * true - объект на екране
     * false - объект НЕ на екране
     */
    public float camWidth;// Высота от центра точки [ 0, 0, 0 ] до верхнего края камеры или до нижнего края
    public float camHeight;//ширина от центра точки [ 0, 0, 0 ] до левого или правого края

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;
    /*
     * Здесь объявляютса четыре переменные offRight, offLeft, offUp, offDown
     * по одной для каждой граници экрана, котооую пересёк игровой объект
     */
    void Awake()
    {
        camHeight = Camera.main.orthographicSize;//получение высоты экрана от ценра (0,0,0) до верхнего края камеры или до нижнего края
        camWidth = camHeight * Camera.main.aspect; // получение от центра точки [ 0, 0, 0 ] до левого или правого края
        /*
         * Полная высота экрана = camHeight*2
         * Полная ширина экрана = camWidth *2
         */
    }
    // Start is called before the first frame update
    void LateUpdate()// Выполняетса после Update(в ктором происходит перемещение), тоесть работает после перемещения корабля  
    {
        Vector3 pos = transform.position;// записать текущие координаты 
        isOnScreen = true;// объект на екране
        offRight = offLeft = offUp = offDown = false;// объект ещё не пересёк ни одну границу экрана 
        if (pos.x > camWidth-radius)//Проверить заходит ли корабль за верхнюю границу
        {
            pos.x = camWidth - radius;//Установить координаты что б корбль мог зайти только частично за границу
            offRight = true;// объект пересёк правую границу экрана
        }
        if(pos.x < -camWidth+radius)//Проверить заходит ли корабль за нижнюю границу
        {
            pos.x = -camWidth + radius;//Установить координаты что б корбль мог зайти только частично за границу
            offLeft = true;// объект пересёк левую границу экрана
        }
        if(pos.y > camHeight-radius)//Проверить заходит ли корабль за правую границу
        {
            pos.y = camHeight - radius;//Установить координаты что б корбль мог зайти только частично за границу
            offUp = true;// объект пересёк верхнюю границу экрана
        }
        if(pos.y < -camHeight+radius)//Проверить заходит ли корабль за левую границу
        {
            pos.y = -camHeight + radius;//Установить координаты что б корбль мог зайти только частично за границу
            offDown = true;// объект пересёк нижнюю границу экрана
        }
        isOnScreen = !(offRight || offLeft || offUp || offDown);
        /*
         * Проверка:
         * 1 действие (offRight || offLeft || offUp || offDown): в круглых скобках 
         * ко всем переменным применяетса логическое ИЛИ - (||), если хотя б одна из них имеет значение true,
         * то всё выражение будет иметь значение true.
         * Затем к результату применяетса логическая операция НЕ - (!),
         * тоесть isOnScreen = получит значение false/
         */
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;// Переместить корабль на заданые координаты pos
            isOnScreen = true;// объект на екране
            offRight = offLeft = offUp = offDown = false;//объект не пересёк границу
        }
        /*
         * Как это работает:
         * переменная isOnScreen - сообощает в каком положении объект,
         * за границами камеры или нет
         * переменная keepOnScreen - сообщает нужно ли пропускать
         * объект через границу камеры 
         * четыре переменные offRight, offLeft, offUp, offDown - сообщают какую границу пересёк объект
         * Функция Update:
         * В начале кадра было объявлено isOnScreen = true;,
         * если хоть одно условие проходит  то записиваетса off__ = true;,
         * это значит объект пересёк границу
         * далее идет условие if(keepOnScreen && !isOnScreen),
         * проверка если он должен быть на экране  - 1 переменная, и если он за камерой - 2 переменная 
         * то заставить перенести его на камеру, и установить значение isOnScreen = true;,
         * offRight = offLeft = offUp = offDown = false;
         */

    }
    void OnDrawGizmos()//Метод для прорисовки границы камеры 
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
