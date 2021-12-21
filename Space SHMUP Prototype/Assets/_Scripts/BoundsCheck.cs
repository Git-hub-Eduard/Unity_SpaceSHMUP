using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     ������������� ����� �������� ������� �� ������� ������. 
///     �����: �������� ������ � ��������������� ������� Main Camera � [ 0, 0, 0 ].
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;// �� ������� ������� ����� �������� �� ������� ������
    public bool keepOnScreen = true;
    //���������� keepOnScreen �������� ������ �� ������ ��������� �� ������
    /*
     * true - ������ ��������� �� ������ ������(Hero)
     * false - �� ������ ��������� �� ������(Enemy)
     */
    [Header("Set Dynamically")]
    public bool isOnScreen = true;//���������� ��� �������� �� �� ������ ��� ���
    /*
     * true - ������ �� ������
     * false - ������ �� �� ������
     */
    public float camWidth;// ������ �� ������ ����� [ 0, 0, 0 ] �� �������� ���� ������ ��� �� ������� ����
    public float camHeight;//������ �� ������ ����� [ 0, 0, 0 ] �� ������ ��� ������� ����

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;
    /*
     * ����� ����������� ������ ���������� offRight, offLeft, offUp, offDown
     * �� ����� ��� ������ ������� ������, ������� ������ ������� ������
     */
    void Awake()
    {
        camHeight = Camera.main.orthographicSize;//��������� ������ ������ �� ����� (0,0,0) �� �������� ���� ������ ��� �� ������� ����
        camWidth = camHeight * Camera.main.aspect; // ��������� �� ������ ����� [ 0, 0, 0 ] �� ������ ��� ������� ����
        /*
         * ������ ������ ������ = camHeight*2
         * ������ ������ ������ = camWidth *2
         */
    }
    // Start is called before the first frame update
    void LateUpdate()// ����������� ����� Update(� ������ ���������� �����������), ������ �������� ����� ����������� �������  
    {
        Vector3 pos = transform.position;// �������� ������� ���������� 
        isOnScreen = true;// ������ �� ������
        offRight = offLeft = offUp = offDown = false;// ������ ��� �� ������ �� ���� ������� ������ 
        if (pos.x > camWidth-radius)//��������� ������� �� ������� �� ������� �������
        {
            pos.x = camWidth - radius;//���������� ���������� ��� � ������ ��� ����� ������ �������� �� �������
            offRight = true;// ������ ������ ������ ������� ������
        }
        if(pos.x < -camWidth+radius)//��������� ������� �� ������� �� ������ �������
        {
            pos.x = -camWidth + radius;//���������� ���������� ��� � ������ ��� ����� ������ �������� �� �������
            offLeft = true;// ������ ������ ����� ������� ������
        }
        if(pos.y > camHeight-radius)//��������� ������� �� ������� �� ������ �������
        {
            pos.y = camHeight - radius;//���������� ���������� ��� � ������ ��� ����� ������ �������� �� �������
            offUp = true;// ������ ������ ������� ������� ������
        }
        if(pos.y < -camHeight+radius)//��������� ������� �� ������� �� ����� �������
        {
            pos.y = -camHeight + radius;//���������� ���������� ��� � ������ ��� ����� ������ �������� �� �������
            offDown = true;// ������ ������ ������ ������� ������
        }
        isOnScreen = !(offRight || offLeft || offUp || offDown);
        /*
         * ��������:
         * 1 �������� (offRight || offLeft || offUp || offDown): � ������� ������� 
         * �� ���� ���������� ����������� ���������� ��� - (||), ���� ���� � ���� �� ��� ����� �������� true,
         * �� �� ��������� ����� ����� �������� true.
         * ����� � ���������� ����������� ���������� �������� �� - (!),
         * ������ isOnScreen = ������� �������� false/
         */
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;// ����������� ������� �� ������� ���������� pos
            isOnScreen = true;// ������ �� ������
            offRight = offLeft = offUp = offDown = false;//������ �� ������ �������
        }
        /*
         * ��� ��� ��������:
         * ���������� isOnScreen - ��������� � ����� ��������� ������,
         * �� ��������� ������ ��� ���
         * ���������� keepOnScreen - �������� ����� �� ����������
         * ������ ����� ������� ������ 
         * ������ ���������� offRight, offLeft, offUp, offDown - �������� ����� ������� ������ ������
         * ������� Update:
         * � ������ ����� ���� ��������� isOnScreen = true;,
         * ���� ���� ���� ������� ��������  �� ������������ off__ = true;,
         * ��� ������ ������ ������ �������
         * ����� ���� ������� if(keepOnScreen && !isOnScreen),
         * �������� ���� �� ������ ���� �� ������  - 1 ����������, � ���� �� �� ������� - 2 ���������� 
         * �� ��������� ��������� ��� �� ������, � ���������� �������� isOnScreen = true;,
         * offRight = offLeft = offUp = offDown = false;
         */

    }
    void OnDrawGizmos()//����� ��� ���������� ������� ������ 
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
