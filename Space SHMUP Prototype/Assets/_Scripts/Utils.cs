using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //Возвращать  список всех материалов в данном игровом объекте 
    //и его дочерних объектах
    // Start is called before the first frame update
    /// <summary>
    /// Статический метод GetAllMaterials что возвращает масив материалов из игрового объекта
    /// </summary>
    /// <param name="go">Игрововой объект из которого нужно извлечь масив материалов</param>
    /// <returns>Возвращает масив материалов даного игрового объекта </returns>
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();//Выполняет обход самого игрового объекта и всех дочерних объектов
        //и возращает масив компоненты с типами Renderer
        List<Material> mats = new List<Material>();//создать список с материалами 
        foreach(Renderer rend in rends)
        {
            /*
             * Этот цикл выполянет обход всех компонентов Renderer в масиве rends
             * и извлекает значение поля каждого из них 
             * Затем полученый материал добавляетса в спискок mats
             * */
            mats.Add(rend.material);
        }
        return (mats.ToArray());// список mats преобразуетса в масив и возвращает вызывающему коду
    }


}
