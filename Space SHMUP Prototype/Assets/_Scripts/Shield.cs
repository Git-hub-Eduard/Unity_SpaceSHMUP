using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;//�������� �������� ����
    [Header("Set Dynamically")]
    public int levelShown = 0;
    // ������� ���������� �� ���������� � ����������
    Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;// �������� ��������� Renderer ��� ����� 
    }

    // Update is called once per frame
    void Update()
    {
        //��������� ������� �������� ��������� ���� 
        // �� �������  - �������� Hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);//�������� ������� �������� ���� 
        //�������  Mathf.FloorToInt - ��������� ���� �� ���������� ������, � �������� ������������� currLevel
        //���� ������� ��������� ���� ������(currLevel) ���������� �� levelShown
        if(levelShown != currLevel)
        {
            levelShown = currLevel;// ������������ ������� ��������� ���� � levelShown
            //��������������  �������� � ��������, ��� �� ���������� ����  � ������ ��������
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //������������ ���� � ������ ����� � ���������� ���������
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
