using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��� ������������ ���� ��������� ����� ������
/// ����� �������� ��� "shield", ����� ���� ����������� ���������������� ������.
/// ������������ [HP] ���� �������� ��������, �� ����������� � ���� �����
/// </summary>

public enum WeaponType
{
    none,//�� ��������� / ��� ������
    blaster,// ������� �������
    spread,//������� �����, ���������� ����������� ���������
    phares,//[HP] �������� �����
    missile,//[HP] ��������������� ������ 
    laser,//[HP] ������� ����������� ��� �������������� �����������
    shield,//����������� shieldLevel
}

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
