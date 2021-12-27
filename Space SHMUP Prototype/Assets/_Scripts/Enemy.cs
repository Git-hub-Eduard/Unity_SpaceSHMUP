using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;// �������� � �/�
    public float fireRate = 0.3f;// ������� ����� ����������
    public float health = 10;// ���������� ������
    public int score = 100;// ���� �� ����������� �������
    protected BoundsCheck bndCheck;// ������ �� ��������� BoundsCheck, ��� ��������� � ����� �������� �������
    //��� ��������: �����, ����������� ��� ����
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);// ���������� ���������� �������� �������  
        }
        set
        {
            this.transform.position = value;// �������������� ����������
        }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        //�������� ������ �� ��������� �������� BoundsCheck ��� ��������� � ����� �������� �������
    }

    // Update is called once per frame
    void Update()
    {
        Move();// ���������� ������� ���� 
        if(bndCheck != null && bndCheck.offDown)
        {

            //��������, ��� ������� ����� �� ������ ������� - ����������
            Destroy(gameObject);
        }
    }
    public virtual void Move()
    {
        Vector3 temPos = pos; // �������� ������� ���������� � ���������� temPos
        temPos.y -= speed * Time.deltaTime;// ����������� ������ ����
        pos = temPos;//����������� �������
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;//�������� ������ � otherGO �� ������� ������ ��� ���������� � ���
        if(otherGO.tag == "ProjectileHero")//�������� �������� �� ������� ������ ��� ���������� � ��� - ������
        {
            Destroy(otherGO);//���� ��� ������ �� ���������� ��� ������ 
            Destroy(gameObject);// � ������ ��� ���������� � ��������
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);// � ���� ������ �������� �� ����
        }
    }
}
