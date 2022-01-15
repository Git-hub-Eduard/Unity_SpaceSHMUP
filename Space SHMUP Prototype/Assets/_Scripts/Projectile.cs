using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private BoundsCheck bndCheck;
    private Renderer rend;// ��� ��������� ����
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;//��� ������ 
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);//������� ������������
        }
    }
    void Awake()// ����� ��������� ������� �� �����
    {
        bndCheck = GetComponent<BoundsCheck>();//�������� ������ �� ��������� BoundsCheck
        rend = GetComponent<Renderer>();// �������� ������ �� ��������� Renderer
        rigid = GetComponent<Rigidbody>();//�������� ������ �� ��������� Rigidbody
    }
    // Update is called once per frame
    void Update()
    {
        if(bndCheck.offUp)// ���� ������ ������ ������� ������ 
        {
            Destroy(gameObject);//���������� ������
        }
        if(bndCheck.offDown)// ���� ������ ������ ������� ������
        {
            Destroy(gameObject);//���������� ������
        }
    }

    /// <summary>
    /// �������� ������� ���� _type � ������������� ���� ����� �������;
    /// ��� ���������� � WeaponDefinition.
    /// </summary>
    /// <param name="eType"> ��� WeaponType ������������� ������ </param>
    public void SetType(WeaponType eType)
    {
        //���������� _type
        _type = eType;//�������� ��� ������ 
        WeaponDefinion def = Main.GetWeaponDefinion(_type);// �������� ��������� WeaponDefinion ��� ������������ ���� ������
        rend.material.color = def.projectileColor;//�������� ���� ������ ������� ����� � ������
    }
}
