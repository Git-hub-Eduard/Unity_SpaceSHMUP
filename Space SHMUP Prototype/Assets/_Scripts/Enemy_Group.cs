using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Group : Enemy
{
    public AnimationCurve trayctori;//���������� ���������� �������� AnimationCurve
    public bool IsRight = false;//���������� � ����� ������� ����� �������� ����
    public float currenttime;//�����  ����������  ����������� ��� ��������� ��������� �
    public float lasttime;//��������� ����� �������  trayctori;
    public float speedG = 10f;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsRight)//���� ���� �������� � ���� 
        {
            currenttime = trayctori.keys[0].time;//���������� ������ ����� ������� �� �
            lasttime = trayctori.keys[trayctori.keys.Length - 1].time;//���������� ��������� ����� ������� �� �
        }
        else//���� ���� �������� � �����
        {
            currenttime = trayctori.keys[trayctori.length - 1].time;//���������� ������ ����� ������� �� �
            lasttime = trayctori.keys[0].time;//���������� ��������� ����� ������� �� �
        }
        
    }

    // Update is called once per frame
    public override void Move()
    {
        Vector3 tempos = new Vector3(currenttime, trayctori.Evaluate(currenttime));//���������� ����� ����� �����������
        pos = tempos;//����������� ������ 
        if (!IsRight)//���� ���� �������� � ���� 
        {
            currenttime += Time.deltaTime * speed;//�������� �������� ��� �(�������)
            if (currenttime > lasttime)//����  ����� �� ������� �� � �� ��������� �����  
            {
                Destroy(gameObject);//���������� ������ 
            }
        }
        else//���� ���� �������� � �����
        {
            currenttime -= Time.deltaTime * speed;//�������� �������� ��� �(�������)
            if (currenttime < lasttime)//����  ����� �� ������� �� � �� ��������� ����� 
            {
                Destroy(gameObject);//���������� ������ 
            }
        }
        
    }
}
