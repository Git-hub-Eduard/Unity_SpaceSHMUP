using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPortal : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    private GameObject HeroTriiger = null;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(pos.y > 20)
        {
            pos.y -= speed*Time.deltaTime;
        }
        transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.gameObject.transform.root;
        GameObject go = root.gameObject;
        if(go == HeroTriiger)
        {
            return;
        }
        HeroTriiger = go;
        Main.S.Finish();
    }
}
