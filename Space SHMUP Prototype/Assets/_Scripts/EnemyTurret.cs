using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    private WeaponType type = WeaponType.blaster;
    public GameObject Collar;//����
    WeaponDefinion def;
    private float timeShootEnemy;
    public float delayShotEnemy = 1f;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        def = Main.GetWeaponDefinion(type);//�������� �������� ������ 
        target = GameObject.FindGameObjectWithTag("Hero").transform;//����� ������� ������ Hero
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 duration = target.position - transform.position;//���������� ������ ����������� �� ������� ������ Hero
        float rotate = Mathf.Atan2(duration.y, duration.x) * Mathf.Rad2Deg;//�������� �������� ��� z
        transform.rotation = Quaternion.Euler(0, 0, rotate - 91);//��������� � ����������� ������
        if ((Time.time - timeShootEnemy) < delayShotEnemy)
        {
            return;
        }
        FireEnemy();

    }
    void FireEnemy()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//������� ������
        go.tag = "ProjectileEnemy";//������ ���
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//������ ���������� ����
        go.transform.position = Collar.transform.position;//����������� ������ � ����
        Projectile p = go.GetComponent<Projectile>();//�������� ��������� Projectile
        p.type = type;//���������� ��� ������� 
        Vector3 vel = Vector3.up * def.velocity;//���� ��������� ������
        p.transform.rotation = transform.rotation;//��������� ������ � ����������� �����
        p.rigid.velocity = p.transform.rotation * vel;//���� ��������� ������� � ����������� �����
        timeShootEnemy = Time.time;//��������� ����� ��������
    }
}
