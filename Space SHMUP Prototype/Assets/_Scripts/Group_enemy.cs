using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group_enemy : MonoBehaviour
{
    public float Delay = 0.5f;//����� ����� ���������� ������
    private float start_time;//��������� �����
    // Start is called before the first frame update
    void Start()
    {
        start_time = Time.time;//������������� ��������� ����� 
        StartCoroutine(StartEnemy());//��������� �������� ������� ��������
    }


    /// <summary>
    /// ������� ��� ����� ������� ������� ������ �������� ������� ���������
    /// </summary>
    /// <returns></returns>
    IEnumerator StartEnemy()
    {
        for (int i = 0;i<transform.childCount;i++)//���� ��� �������� �� ���� �������� �������� ��������
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);//������� ������ ��������
            yield return new WaitForSeconds(Delay);//��������� Delay ������� 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if((Time.time-start_time)>9f)//��������� ���� ������ ������ 9 ������ 
        {
            Destroy(gameObject);//���������� ������� ������ 
        }
    }
}
