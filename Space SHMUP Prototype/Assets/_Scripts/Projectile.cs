using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private BoundsCheck bndCheck;
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();//�������� ������ �� ��������� BoundsCheck
    }
    // Update is called once per frame
    void Update()
    {
        if(bndCheck.offUp)// ���� ������ ������ ������� ������ 
        {
            Destroy(gameObject);//���������� ������
        }
    }
}
