using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public WeaponType type = WeaponType.blaster;//Тип вооружения бластер
    public WeaponDefinion def;//свойства оружия 
    public GameObject collarEnemy;//Дуло с которого будет стрелять враг
    public float lastShot;//время последнего выстрела
    public float projEnemy_4Speed = 30;//Скорость снаряда
    public float tempEnemy_4Fire = 4f;//Продолжительность 1 выстрела
    private Renderer colorEnemyCollar;
    // Start is called before the first frame update
    void Start()
    {
        collarEnemy = transform.Find("Collar").gameObject; //Находим дочерий объект Collar родительского объекта EnemyWeapon
        colorEnemyCollar = collarEnemy.GetComponent<Renderer>();//Получить ссылку на компонент Renderer дочернего объекта Collar
        def = Main.GetWeaponDefinion(type);//Найти свойства для бластера 
        colorEnemyCollar.material.color = def.color;//Перекрасить дуло в соотвецтвующий цвет
        GameObject enemy_4ROOT = transform.root.gameObject;//Получит ссылку на родительский игровой объект Enemy_4
        if(enemy_4ROOT.GetComponent<Enemy_4>()!=null)//Проверить наличие сценария Enemy_4 в родительском игровом объекте
        {
            enemy_4ROOT.GetComponent<Enemy_4>().fireEnemy += Fire; //Добавляем метод Fire в делегат fireEnemy
        }
    }

    // Update is called once per frame
    void Fire()
    {
        if((Time.time-lastShot)<tempEnemy_4Fire)
        {
            return;
        }
        OneShot();
        
    }
    /// <summary>
    /// Функция что создает снаряд для оружия 
    /// </summary>
    void CreateProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePefab);//Создать снаряд за типом вооружения 
        go.tag = "ProjectileEnemy";//имя тега
        go.layer = LayerMask.NameToLayer("ProjectileEnemy");//имя слоя
        go.transform.position = collarEnemy.transform.position;//Установить созданыйснаряд у Дула оружия 
        Projectile enemuProjectile = go.GetComponent<Projectile>();
        enemuProjectile.type = type;//Установить тип снаряда
        enemuProjectile.rigid.velocity = Vector3.down * projEnemy_4Speed;//Придать ускорению снаряду
        
    }

    /// <summary>
    /// Функция что делает 1 выстрел
    /// </summary>
    void OneShot()
    {
        Invoke("CreateProjectile", 0.5f );
        lastShot = Time.time;//Записать время последнего выстрела 
    }

}
