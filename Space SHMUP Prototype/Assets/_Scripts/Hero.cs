using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    static public Hero S;//��������
    [Header("Set in Inspector")]
    //���� ����������� ��������� ������� 
    public float speed = 30;//�������� �������� �������
    public float rollMult = -45;//������� ������� �� ��� �
    public float pitchMult = 30;//������� ������� �� ��� �
    public float RestartDelay = 2f;//����� ������� ������������� ����
    //public GameObject projectilePrefab;//������ �������
    public int missileSize = 0;//���������� �����
    private float missileTime = 0;//����� ���������� ��������
    private WeaponDefinion def;//�������� ������ 
    public float projectileSpeed = 40;//�������� ������� 
    public Weapon[] weapons;//����� ����� ������� ������ �� ������ ������
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;//���������� ������ ������ �� ��������� ������������� ������� ������

    //���������
    public Text MissileText;//��� ����������� ���������� �����

    //���������� ������ �������� WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //�������� ���� ���� WeaponFireDelegate � ������ fireDelegate
    public WeaponFireDelegate fireDelegate;
    void Start()
    {
       if(S==null)
       {
           S = this;//��������� ������ �� ��������
       }
       else
       {
           Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");// ��������� ������ ���� ������ ��� ���� ��������� Hero S
       }
       def = Main.GetWeaponDefinion(WeaponType.missile);
       UI_Updaye();//�������� ���������
       // �������� ������ weapons � ������ ���� � 1 ���������
       ClearWeapons();
       weapons[0].SetType(WeaponType.blaster);
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
        // if(Input.GetKeyDown(KeyCode.Space))// ��� ������� ������� ��������
        // {
        //     TempFire();
        //}

        //���������� ������� �� ���� ����� ������ ������� fireDelegate
        //������� ���������� ������� ������� Axis("Jump")
        //����� �������� ��� fireDelegate �� ����� null
        //����� �������� ������ 
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            //���� ���� ������ ������� �� Input.GetAxis("Jump") ������ 1
            fireDelegate();//����� ��������  � ������ ��������� �����  TempFire
            
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))//���� ����� ����� ������ F
        {
            CreateRocket();//������� ������
        }
    }
  

    /// <summary>
    /// ������� ��� ������� ������
    /// </summary>
    void CreateRocket()
    {
        if(missileSize == 0)//���� ���������� ����� ����� 0
        {
            return;
        }
        else if((Time.time-missileTime)<def.delayBetwenshots)//��������� ������ �� ���������� ������� ��� ���� ��������� ������� ������
        {
            return;
        }
        else
        {
            GameObject missile = Instantiate<GameObject>(def.projectilePefab);//������� ������ 
            missile.transform.position = transform.position;//����������� �� ����� ������� 
            missileSize--;//������ ���������� �����
            UI_Updaye();//�������� ���������
            missileTime = Time.time;
        }
       
    }

    /// <summary>
    /// ������� ��� ���������� ����������
    /// </summary>
    public void UI_Updaye()
    {
        MissileText.text = "M: " + missileSize;
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
        else if(go.tag == "ProjectileEnemy")//��������� ���� ��� ��������� ������
        {
            shieldLevel--;//�������� ������� ������ 
            Destroy(go);//���������� ������
        }
        else if(go.tag == "PowerUp")//��������� ���� ������� ������ � ������� ���������� ����� �������� �� �� �������
        {
            AbsorpPowerUp(go);
        }
        else//���� ����� ���������� � ������ �������� �� �������� ���� Enemy
        {
            print("Triggered by non-Enemy: " + go.name);// �������� ��������, ��� � ����� ���� ������ �� ����
        }
    }

    /// <summary>
    /// ����������� ����� ����� ������������ � �������
    /// </summary>
    /// <param name="go">������� ������ �����</param>
    public void AbsorpPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();//�������� ������ �� ��������� PowerUp
        switch (pu.type)
        {
            case WeaponType.shield://���� ��� ������ ��� 
                shieldLevel++;//��������� � ���� 1
                break;
            case WeaponType.missile:
                missileSize = missileSize+2;//�������� 2 ������
                UI_Updaye();//�������� ���������
                print("��������� ������ = " + missileSize);
                break;
            default:
                if(pu.type == weapons[0].type)
                {
                    //���� ������ ���� �� ���� ��� � �����
                    Weapon w = GetEmptyWeaponSlot();//������� ������ ���� ������ 
                    if(w!= null)
                    {
                        //���������� ������ �� ������ ����
                        w.SetType(pu.type);
                    }
                }
                else//���� ������ ������� ���� 
                {
                    ClearWeapons();//�������� ��� ����� ������
                    weapons[0].SetType(pu.type);//���������� ����� ������
                }
                break;

        }
        pu.AbsorbedBy(this.gameObject);
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

    /// <summary>
    /// ������� ���������� �������� ������ � ������ ��������� ������ �������
    /// � ������� ������  � ����� WeaponType.none
    /// </summary>
    /// <returns>���������� ������ � �������� ��� WeaponType.none</returns>
    Weapon GetEmptyWeaponSlot()
    {
        for(int i =0;i<weapons.Length; i++)//������ ��� ��������(������) ������
        {
            if(weapons[i].type == WeaponType.none)//��������� ���� ��� ������ ����� WeaponType.none
            {
                return (weapons[i]);//���������� ������ � �������� ��� WeaponType.none
            }
        }
        return (null);//���������� null ���� ��� ������ � ����� WeaponType.none
    }

    /// <summary>
    /// ������� ������� ������ � ����� �������, 
    /// �� ���� ����������� WeaponType.none
    /// </summary>
    void ClearWeapons()
    {
        foreach(Weapon w in weapons)//���������� ��� �������� ������ weapons
        {
            w.SetType(WeaponType.none);//����������� ������� �������� ��� WeaponType.none
        }
    }
}
