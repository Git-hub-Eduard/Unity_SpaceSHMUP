using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public WeaponType type = WeaponType.blaster;
    GameObject Collar;//����
    WeaponDefinion def;
    private float timeShoot;
    public float delayShot = 1f;
    // Start is called before the first frame update
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//�������� �������� ������ 
        Collar = transform.Find("Collar").gameObject;//������� ������� ������ Collar ������������� �������
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
            transform.rotation = Quaternion.Euler(0, 0, rotate-91);//��������� � ����������� �����
            if ((Time.time - timeShoot) < delayShot)
            {
                return;
            }
            Fire();//��������
        }
    }


    /// <summary>
    /// ������� �������� �� ������ 
    /// </summary>
    void Fire()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//������� ������
        go.tag = "ProjectileHero";//������ ���
        go.layer = LayerMask.NameToLayer("ProjectileHero");//������ ���������� ����
        go.transform.position = Collar.transform.position;//����������� ������ � ����
        Projectile p = go.GetComponent<Projectile>();//�������� ��������� Projectile
        p.type = type;//���������� ��� ������� 
        Vector3 vel = Vector3.up * def.velocity;//���� ��������� ������
        p.transform.rotation = transform.rotation;//��������� ������ � ����������� �����
        p.rigid.velocity = p.transform.rotation * vel;//���� ��������� ������� � ����������� �����
        timeShoot = Time.time;//��������� ����� ��������
    }
}
