using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Это перечисление всех возможных типов оружия
/// Также включает тип "shield", чтобы дать возможность совершенствувать защиту.
/// Абревиатурой [HP] ниже отмечены элементы, не риализованы в этой книге
/// </summary>

public enum WeaponType
{
    none,//По умолчанию / нет оружия
    blaster,// Простой бластер
    spread,//Веерная пушка, стреляющая несколькими снарядами
    phares,//[HP] Волновой фазер
    missile,//[HP] Самонаводящиеся ракеты 
    laser,//[HP] Наносит повреждение при долговременном воздействии
    shield,//Увеличивает shieldLevel
}

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
