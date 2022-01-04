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
/// <summary>
/// ����� WeaponDefinition ��������� ����������� ��������
/// ����������� ���� ������ � ����������. ��� ����� ����� Main
/// ����� ������� ������ ��������� ���� WeaponDefinition.
/// </summary>
[System.Serializable]
public class WeaponDefinion
{
    public WeaponType type = WeaponType.none;
    public string letter;// ����� �� ������, ������������� �����.
    public Color color = Color.white;// ���� ������ ������, � ������ ������
    public GameObject projectilePefab;// ������ �������� 
    public Color projectileColor = Color.white;// ���� �������
    public float damageOnHit = 0;//�������������� ��������
    public float continuousDamage = 0;//������� ���������� � ������� ��� Laser
    public float delayBetwenshots = 0;
    public float velocity = 20;//�������� ������ ������� 
}
public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;//������ ���� �������� ��� ���� �������� ��������
    [Header("Set Dyamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;// ��������� ��� ������ 
    public WeaponDefinion def;//�������� ������(��������, ����, ������ �������� ����)
    public GameObject collar;// ������� ������  - ���� � �������� ������� ����� ��������
    public float lastShotTime;// ����� ���������� ��������
    private Renderer collarRend;// ��� � �������� ���� ���� � ����������� � ����� ������
    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;//������� ������� ������ Collar ������������� ������� Weapon 
        collarRend = collar.GetComponent<Renderer>();//�������� ������ �� ��������� Renderer ��������� ������� Collar

        //������� SetType() ��� � �������� ��� ������ �� ��������� �� WeaponType.none;
        SetType(_type);//���������� ��� ������ �� ���������
        //����������� ������� ����� �������� ��� ���� ��������
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        GameObject rootGO = transform.root.gameObject;//������� ������ �� ������������ ������� ������ Hero
        if(rootGO.GetComponent<Hero>() != null)//��������� ������� �������� Hero � ������� ������� Hero
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;//��������� ����� Fire � ������� fireDelegate ������ Hero
        }
    }
    public WeaponType type//��� ��������� _type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    /// <summary>
    /// ���� ����� �������� ��� ������, ������ ��� �������� ���� ���, �������� ���� ���� ������ 
    /// </summary>
    /// <param name="wt"> ��� ������ �� ������� ����� �������� ������</param>
    public void SetType(WeaponType wt)
    {
        _type = wt;//�������� ��� ������ �� ����� ��� ��� �������� � ���������� ���������� ����� WeaponType wt
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);//������� ����� �� �������� 
        }
        else
        {
            this.gameObject.SetActive(true);//� ������ ������ ������� ��������
        }
        def = Main.GetWeaponDefinion(_type);//����� ��������� WeaponDefinition ��� ������������ ���� ������ _type
        collarRend.material.color = def.color;//�������� ���� ���� � ����������� � ����� ������ 
        lastShotTime = 0; //����� ����� ����������������  ����� ���������� 
    }

    /// <summary>
    /// ����� ��� ���� ����������� �������� 
    /// </summary>
    public void Fire()
    {
        //���� this.gameObject �� ������� ����� 
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        //���� ����� ��������� ������ �� ���������� ������� ����� 
        if ((Time.time - lastShotTime) < def.delayBetwenshots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;//��������������� ��������� �������� ������� 
        if(transform.up.y<0)
        {
            vel.y = -vel.y;
        }
        switch(type)
        {
            case WeaponType.blaster://���� ��� ������ �������
                p = MakeProjectile();//������� 1 ������ 
                p.rigid.velocity = vel;//������� ������� ��������� 
                break;
            case WeaponType.spread:
                p = MakeProjectile();//������ ������� �����
                p.rigid.velocity = vel;//������� ������� ��������� 
                p = MakeProjectile();//������ ������� � �����
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);//��������� ������ � ����� �� 10 ��������
                p.rigid.velocity = p.transform.rotation * vel;//������� ������� ��������� ��� ����� � �����
                p = MakeProjectile();//������ ������� � ����
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);//��������� ������ � ���� �� 10 ��������
                p.rigid.velocity = p.transform.rotation * vel;//������� ������� ��������� ��� ����� � ����
                break;
        }
    }

    /// <summary>
    /// ����� ��� ������� ������ ��� ������������� ���� ������ 
    /// </summary>
    /// <returns> ���������� ������ �� �������� Projectile ��������� ������� </returns>
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//�������� ������ �� ������ � ������� ��� ��� ��������������� ���� ������ 
        if(transform.parent.gameObject.tag == "Hero")//��������� ���� ������������ ������ ����� ��� Hero
        {
            //������� ��������� �������������� ��� � ���������� �������
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;//���������� �������������� � ���� ������ 
        go.transform.SetParent(PROJECTILE_ANCHOR,true);
        Projectile p = go.GetComponent<Projectile>();//���������� ������ �� �������� Projectile �������
        p.type = type;// ��������� ��� ������  �������
        lastShotTime = Time.time;//��������� ������� ����� 
        return (p);// ���������� ������ �� �������� Projectile ��������� �������
    }
}
