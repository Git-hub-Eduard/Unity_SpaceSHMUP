using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHeroMaterials : MonoBehaviour
{
    // Start is called before the first frame update
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer ren1 = go.transform.GetChild(0).gameObject.GetComponent<Renderer>();
        Renderer ren2 = go.transform.GetChild(1).gameObject.GetComponentInChildren<Renderer>();
        List<Material> mats = new List<Material>();//создать список с материалами 
        mats.Add(ren1.material);
        mats.Add(ren2.material);
        return mats.ToArray();
    }
}
