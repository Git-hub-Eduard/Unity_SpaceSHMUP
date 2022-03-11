using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Impact;
    private ParticleSystem _impact;
    private BoundsCheck bndCheck;
    private Renderer rend;// ��� ��������� ����
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;//��� ������ 
    private TrailRenderer _trailRenderer;
    
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
        _trailRenderer = GetComponent<TrailRenderer>();
        _impact = Impact.GetComponent<ParticleSystem>();
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
        _trailRenderer.material.SetColor("ShadeColor", def.projectileColor);
        var main = _impact.main;//������ �� ������������ 
        main.startColor = def.projectileColor;//������ �� ������������
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(Impact, transform.position,Quaternion.identity);
    }

}
