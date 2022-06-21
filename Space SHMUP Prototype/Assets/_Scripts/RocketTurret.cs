using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : MonoBehaviour
{

    private WeaponType type = WeaponType.missile;
    public GameObject Collar;
    private WeaponDefinion def;
    private float misselTime;
    private Vector3 pos;
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//�������� �������� ������ 
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Enemy");//����� ������� ������ Enemy
        if (target == null)//���� ��� ���
        {
            return;//����������� 
        }
        else
        {
            Vector3 duration = target.transform.position - transform.position;//���������� ������ ����������� �� ������� ������ Enemy
            float rotate = Mathf.Atan2(duration.y, duration.x) * Mathf.Rad2Deg;//�������� �������� ��� z
            transform.rotation = Quaternion.Euler(0, 0, rotate - 91);//��������� � ����������� �����
            CreateRocket();
        }
    }
    void CreateRocket()
    {
        if ((Time.time - misselTime) < def.delayBetwenshots)//��������� ������ �� ���������� ������� ��� ���� ��������� ������� ������
        {
            return;
        }
        else
        {
            GameObject missile = Instantiate<GameObject>(def.projectilePefab);//������� ������ 
            pos = Collar.transform.position;//������������ ���������
            pos.z = 0;// ���������� z = 0
            missile.transform.position = pos;//����������� �� ����� ����� ���������
            misselTime = Time.time;//�������� �����
        }
        
    }
}
