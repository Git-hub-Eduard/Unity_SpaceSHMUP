using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public WeaponType type = WeaponType.missile;//��� ������� 
    public WeaponDefinion def;//�������� ������� 
    public Renderer colorMissile;//��������� Renderer
    private Rigidbody rigidMissile;// ��������� Missile
    public float rotatespeed = 200f;//�������� ��������

    //������� 
    public GameObject particleMissile;//������� ������ ������
    // Start is called before the first frame update
    void Awake()
    {
        rigidMissile = GetComponent<Rigidbody>();//������ ��������� Rigidbody �������� ������� Rocket
        colorMissile = GetComponent<Renderer>();//������ ��������� Renderer �������� ������� Rocket
        def = Main.GetWeaponDefinion(type);//�������� �������� ���� ������ 
        colorMissile.material.color = def.color;//��������� � ���� ��� ������������ ���� ������ 
    }

    // Update is called once per frame
    void Update()
    {
        rigidMissile.velocity = Vector3.up * def.velocity;//���� ������ ��������� �����
        Instantiate(particleMissile, transform.position, Quaternion.identity);//������� �������
        GameObject target = GameObject.FindGameObjectWithTag("Enemy");//����� ������� ������ Enemy
        if (target == null)//���� ��� ���
        {
            return;//����������� 
        }
        else
        {
            Vector3 duration = target.transform.position - rigidMissile.position;//���������� ������ ����������� �� ������� ������ Enemy
            duration.Normalize();//���������� ����� �������  = 1, �������� �����������
            float rotate = Vector3.Cross(duration, transform.up).z;//����� ��� z, ������� ���������������� ��� �������� ������ � Enemy
            rigidMissile.angularVelocity = new Vector3(0, 0, -rotate * rotatespeed);//��������� � ����������� Enemy
            rigidMissile.velocity = duration * def.velocity;//������������ ����
        }
        
       
    }
}
