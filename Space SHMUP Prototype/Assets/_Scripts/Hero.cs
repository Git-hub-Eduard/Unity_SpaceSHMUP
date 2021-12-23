using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;//��������
    [Header("Set in Inspector")]
    //���� ����������� ��������� ������� 
    public float speed = 30;//�������� �������� �������
    public float rollMult = -45;//������� ������� �� ��� �
    public float pitchMult = 30;//������� ������� �� ��� �
    public float RestartDelay = 2f;//����� ������� ������������� ����
    public GameObject projectilePrefab;//������ �������
    public float projectileSpeed = 40;//�������� ������� 
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;//���������� ������ ������ �� ��������� ������������� ������� ������  
     void Awake()
     {
       if(S==null)
        {
            S = this;//��������� ������ �� ��������
        }
       else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");// ��������� ������ ���� ������ ��� ���� ��������� Hero S
        }
     }
    // Update is called once per frame
    void Update()
    {
        //��������� ���������� �� ����� Input
        float xAxis = Input.GetAxis("Horizontal");//������� ���������� ��� ������� ������ ���� ��� � �����
        float yAxis = Input.GetAxis("Vertical");//������� ���������� ��� ������� ������ � ��� ��� � ����
        /*
         * ���� ������ ������ ��  � xAxis ������� -1 � 1, � ���� ��� � ����� ��������������
         * ���� ������ ������ ��  � yAxis ������� -1 � 1, � ��� ��� �����
         */

        //�������� transform.position, �������� �� ���������� �� ���� 
        Vector3 pos = transform.position;// �������� ������� ���������� ������� 
        pos.x += xAxis * speed * Time.deltaTime;//�������� ���������� �����������
                                                //������� �� ��� � ������ ��������� xAxis * speed * Time.deltaTime
        pos.y += yAxis * speed * Time.deltaTime;//�������� ���������� �����������
                                                //������� �� ��� Y ������ ��������� yAxis * speed * Time.deltaTime
        transform.position = pos;//����������� ������� � ������ ��� ��������� ����������
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        /*
         * ������ transform.rotation... �������� ������� ��� ����� ����� ��������� ������� ��� ��������
         * ������� Quaternion.Euler - 1 ����������  - ������������ �������, ����� �� ���������   ����� ��� ����, �� ��� �
         * ������� Quaternion.Euler - 2 ���������� - ������������ ������� ����� �� ��������� ����� ��� � �����, �� ��� � 
         */

        //��������� ������� ���������� 
        if(Input.GetKeyDown(KeyCode.Space))// ��� ������� ������� ��������
        {
            TempFire();
        }
    }
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);//������� ������
        projGO.transform.position = transform.position;//���������� ��������� ��� � � ������� 
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();// �������� ��������� Rigidbody � �������
        rigidB.velocity = Vector3.up * projectileSpeed;// ���� ������� ���������
    }
    void OnTriggerEnter(Collider other)//����������� ��� ������������ ��������� ������ � ������� ��������� 
    {
        Transform rootT = other.gameObject.transform.root;//�������� ��������� Transform ������� ��������
        GameObject go = rootT.gameObject;//�������� ������ ������������� ������� � ���������� go
        //print("triggered: " + go.name);
        if(go == lastTriggerGo)
        {
            //���� lastTriggerGo ��������� �� ��� �� ������ ��� � go
            //��� ������������ ������������ - ��� ���������
            return;// � ��������� �������
        }
        lastTriggerGo = go;//�������� ������ ������� ��  go � lastTriggerGo
        if(go.tag=="Enemy")//��������� ���� ������� ������ � ������� ���������� ����� �������� �� �� ������
        {
            shieldLevel--;//��������� ������� ������ �� 1
            Destroy(go);//���������� �����
        }
        else//���� ����� ���������� � ������ �������� �� �������� ���� Enemy
        {
            print("Triggered by non-Enemy: " + go.name);// �������� ��������, ��� � ����� ���� ������ �� ����
        }
    }
    public float shieldLevel
    {
        get//������ �������� ������� ���������� �������� _shieldLevel
        {
            return (_shieldLevel);
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);// ����������� ��� _shieldLevel ������� �� �������� �������� ���� 4
            //���� ������� ���� ���� �� ���� ��� ���� 
            if (value<0)
            {
                Destroy(this.gameObject);//���������� ������� ������
                Main.S.DelayedRestart(RestartDelay);//������������� ����
            }
        }
    }
}
