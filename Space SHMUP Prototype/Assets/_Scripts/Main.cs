using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    public GameObject[] EnemyPrefabs;//���� ������ �� �����
    public int sizeWave; // ���������� ������
}
public class Main : MonoBehaviour
{
    static public Main S;// ������ ��������
    static Dictionary<WeaponType, WeaponDefinion> WEAP_DICT;// ���������� �������
    [Header("Set in Inspector")]
    public float enemySpawnPerSecond = 0.5f;// �������� ��������� �������� �� ������� �������
    public float enemyDefaultPadding = 1.5f;
    //������ ��� ����������������.
    public WeaponDefinion[] weaponDefinions;// ����� �����
    //������
    public GameObject prefabPowerUp;//������ ��� ���� �������
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };
    /*
     * powerUpFrequency - ����� ����� ������ ��� �������
     */
    //�����
    public Wave[] Level;//����� ����
    private BoundsCheck bndCheck;
    /// <summary>
    /// ������ ����� ������� ����� �� ����� ������������ �������
    /// </summary>
    /// <param name="e">��������� ���� ������ ��� ���������</param>
    public void ShipDestroyed(Enemy e)
    {
        //������������� ����� � ������� �����������
        if(Random.value <= e.powerUpDropChance)
        {
            //������� ��� ������
            //������� ���� �� ��������� � powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            //������� ��������� PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            //���������� �������������� ��� WeaponType
            pu.SetType(puType);
            //��������� � �����, ��� ��������� ���������� �������
            pu.transform.position = e.transform.position;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //�������� �  bndCheck ������ �� ��������� BoundsCheck
        bndCheck = GetComponent<BoundsCheck>();
        //������� Scenary()
        StartCoroutine(Scenary());

        //������� � ������� ���� WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinion>();//���������������� �������
        foreach(WeaponDefinion def in weaponDefinions)
        {
            WEAP_DICT[def.type] = def;//�������� � ������� ������ ��� ������������ ��� ���� ���������� 
        }
    }

    /// <summary>
    /// ������� ��� �������� �� ������� ��������� ������ �� ������
    /// </summary>
   IEnumerator Scenary()
   {
        foreach(Wave spawnWave in Level)//��������� ��� ����� �� ������ Level
        {
            for(int i = 0; i<spawnWave.sizeWave; i++)//����� ���� �������� _SpawnEnemy � ��������� WaitForSeconds(1f / enemySpawnPerSecond);
            {
                //������� spawnWave.sizeWave(���������� ������ �� �����) ������ � �����
                yield return new WaitForSeconds(1f / enemySpawnPerSecond);//��������� 1f / enemySpawnPerSecond
                _SpawnEnemy(spawnWave);//������� �����
            }
        }
    }

    void _SpawnEnemy(Wave spawnWave)
    {
        // ������� ��������� ������ Enemy ��� �������� 
        int ndx = Random.Range(0, spawnWave.EnemyPrefabs.Length);//������� ��������� ������ 
        GameObject go = Instantiate<GameObject>(spawnWave.EnemyPrefabs[ndx]);// ������� ������ �� ������ prefabEnemies ��� �������� ndx
        //���������� ��������� ������� ��� ������� � ��������� ������� �
        float enemyPadding = enemyDefaultPadding;// �������� enemyPadding ��������� ������ �� ������
        if (go.GetComponent<BoundsCheck>() != null)//��������� ���� �� � ������� ��������� BoundsCheck 
        {
            //���� ���� �������� � enemyPadding  - radius ����������� �������� ����������  
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;// ������� ���������� pos ���� Vector3 � ��������� ��������� ���������� (0,0,0)
        float xMin = -bndCheck.camWidth + enemyPadding;//���������� ����������� �������� ������� �� ��� � ��� ����� ��������� ������
        float xMax = bndCheck.camWidth - enemyPadding;//���������� ������������ �������� ������� �� ��� � ��� ����� ��������� ������
        pos.x = Random.Range(xMin, xMax);//������� ��������� ���������� ����� xMin � xMax ������������
        pos.y = bndCheck.camHeight + enemyPadding;// ���������� ������ �� ������� ����� ��������� ������
        go.transform.position = pos;//����������� ������ �� ������� ���������� pos
    }
  

    /// <summary>
    /// ������������� ����� ����� ����������� ���������� �������
    /// </summary>
    /// <param name="delay"></param>
    public void DelayedRestart(float delay)
    {
        //  ������� ����� Restart() ����� delay ������
        Invoke("Restart", delay);
    }

    /// <summary>
    /// ������������� �����
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }



    /// <summary>
    /// ����������� �������, ������������ WeaponDefinition �� ������������
    /// ����������� ���� WEAP_DICT ������ Main.
    /// </summary>
    /// <returns>
    /// ��������� WeaponDefinition ���, ���� ��� ������ �����������
    /// ��� ���������� WeaponType, ���������� ����� ��������� WeaponDefinition
    /// � ����� none
    /// </returns>
    /// <param name="wt"> Tnn ������ WeaponType, ��� �������� ��������� �������� WeaponDefinition</param>
    /// <returns></returns>
    static public WeaponDefinion GetWeaponDefinion(WeaponType wt)
    {
        //�������� ������� ��������� ������ � ������� 
        if(WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);// ���������� ��������� ��� ������������� ������������� ���� ������
        }

        //���������� ����� �������� � ����� ������ WeaponType.none  ���� �� ������� ����� � ������� 
        return (new WeaponDefinion());
    }

}
