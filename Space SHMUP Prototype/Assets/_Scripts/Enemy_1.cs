using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Set in Inspector: Enemy_1")]
    //����� ������ ������� ����� ���������
    public float waveFrequency = 2;

    //������ ��������� � ������
    public float waveWidth = 4;
    public float waveRotY = 45;
    private float x0;// ��������� �������� ��������
    private float birthTime;

    private float lastShot;//����� ���������� ��������
    WeaponDefinion def;//�������� ������ 
    public float projSpeed = 30;//�������� �������
    WeaponType type = WeaponType.blaster;//��� ������
    // ����� ����� Start() ������ ��� �� ������������ ����� ������ Enemy
    void Start()
    {
        // ���������� ��������� ������� ��������� � ������� Enemy_1
        x0 = pos.x;
        birthTime = Time.time;
        //������ �������� ������ ��� ��������
        def = Main.GetWeaponDefinion(type);
    }
    //��������������  ������� Move ����������� Enemy
    public override void Move()
    {
        Vector3 temPos = pos;// �������� ���������� pos �� ������������� ����� Enemy
        float age = Time.time - birthTime;
        //�������� theta ���������� � �������� �������  
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        temPos.x = x0 + waveWidth * sin;
        pos = temPos;
        //��������� ������� ������������ ���  Y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);


        if ((Time.time - lastShot) > waveFrequency)//��������� ���� ����� � ������� ���������� �������� ������ �� waveFrequency
        {
            FireEnemy();//��������
        }
        
        
        //
        // �������� �� ��� y
        base.Move();
    }
    /// <summary>
    /// ������� �������� �����
    /// </summary>
    void FireEnemy()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//������� ������ �� ����� ������ 
        go.tag = "ProjectileEnemy";//��� ����
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//��� ����
        go.transform.position = transform.position;//���������� ���������� 
        Projectile enem = go.GetComponent<Projectile>();
        enem.type = type;//���������� ��� �������
        enem.rigid.velocity = Vector3.down * projSpeed;//������� ��������� �������
        lastShot = Time.time;//�������� ����� ���������� ��������
    }

}
