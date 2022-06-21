using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;// Скорость в м/с
    protected BoundsCheck bndCheck;
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }
    // Update is called once per frame
    void Update()
    {
        EnemyMove();
        if (bndCheck != null && bndCheck.offDown)
        {

            //Убедитса, что корабль вышел за нижнюю границу - уничтожить
            Destroy(gameObject);
        }
    }
    void EnemyMove()
    {
        Vector3 cordinats = transform.position;
        cordinats.y -= speed * Time.deltaTime;// переместить объект вниз
        transform.position = cordinats;
    }
}
