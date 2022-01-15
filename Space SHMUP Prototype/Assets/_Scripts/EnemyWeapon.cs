using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public WeaponType type = WeaponType.blaster;//��� ���������� �������
    public WeaponDefinion def;//�������� ������ 
    public GameObject collarEnemy;//���� � �������� ����� �������� ����
    public float lastShot;//����� ���������� ��������
    public float projEnemy_4Speed = 30;//�������� �������
    public float tempEnemy_4Fire = 4f;//����������������� 1 ��������
    private Renderer colorEnemyCollar;
    // Start is called before the first frame update
    void Start()
    {
        collarEnemy = transform.Find("Collar").gameObject; //������� ������� ������ Collar ������������� ������� EnemyWeapon
        colorEnemyCollar = collarEnemy.GetComponent<Renderer>();//�������� ������ �� ��������� Renderer ��������� ������� Collar
        def = Main.GetWeaponDefinion(type);//����� �������� ��� �������� 
        colorEnemyCollar.material.color = def.color;//����������� ���� � �������������� ����
        GameObject enemy_4ROOT = transform.root.gameObject;//������� ������ �� ������������ ������� ������ Enemy_4
        if(enemy_4ROOT.GetComponent<Enemy_4>()!=null)//��������� ������� �������� Enemy_4 � ������������ ������� �������
        {
            enemy_4ROOT.GetComponent<Enemy_4>().fireEnemy += Fire; //��������� ����� Fire � ������� fireEnemy
        }
    }

    // Update is called once per frame
    void Fire()
    {
        if((Time.time-lastShot)<tempEnemy_4Fire)
        {
            return;
        }
        OneShot();
        
    }
    /// <summary>
    /// ������� ��� ������� ������ ��� ������ 
    /// </summary>
    void CreateProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//������� ������ �� ����� ���������� 
        go.tag = "ProjectileEnemy";//��� ����
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//��� ����
        go.transform.position = collarEnemy.transform.position;//���������� �������������� � ���� ������ 
        Projectile enemuProjectile = go.GetComponent<Projectile>();
        enemuProjectile.type = type;//���������� ��� �������
        enemuProjectile.rigid.velocity = Vector3.down * projEnemy_4Speed;//������� ��������� �������
        
    }

    /// <summary>
    /// ������� ��� ������ 1 �������
    /// </summary>
    void OneShot()
    {
        Invoke("CreateProjectile", 0.5f );
        lastShot = Time.time;//�������� ����� ���������� �������� 
    }

}
