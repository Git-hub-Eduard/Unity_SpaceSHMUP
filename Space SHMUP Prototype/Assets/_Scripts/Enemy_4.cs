using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Part - клас который хранит данные разных частей корабля
/// </summary>
[System.Serializable]
public class Part
{
    //Значение этих трёх олей должны определятса в инспекторе
    public string name;//Имя этой части
    public float health;//Степень стойкости этой части 
    public string[] protectedBy;//Другие части, защищают эту

    //Эти два пол инициализируютса автоматически в Start()
    [HideInInspector]//Не позволяет следующему полю появлятса в инспекторе
    public GameObject go;//Игровой объект этой части 
    [HideInInspector]
    public Material mat;// Материал для отображения повреждений

    
}
/// <summary>
/// Enemy_4 создаетса за верхней границей, выбирает случайную точку  на экране 
/// и перемищаетса к ней. Добравшись до места выбираем  другую  случайную точку
/// и продолжает двигатса  пока игрок его  не уничтожает
/// </summary>
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_4")]
    public Part[] parts;//Масив частей составляющих корабль
    private Vector3 p0, p1;//Две точки для интерполции 
    private float timeStart;//Время создания этого корабля 
    private float duration = 4;//Продолжительность перемещения 
    //Создание нового делегата 
    public delegate void WeaponEnemyDelegate();
    //Создать поле типа WeaponEnemyDelegate с именем fireEnemy
    public WeaponEnemyDelegate fireEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //Начальная позиция уже выбрана в Main.SpawnEnemy()
        //поэтому запишем ее как начальные значения в p0, p1
        p0 = p1 = pos;
        InitMovement();

        //Записать в кеш игровой объект и материал каждой части в parts
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);//Найти игровой объект по имени
            if(t!= null)//Проверить если он существует
            {
                prt.go = t.gameObject;//Передать ссылку на объект 
                prt.mat = prt.go.GetComponent<Renderer>().material;//Добуть копонент  Renderer.material
            }
        }

    }

    void InitMovement()
    {
        p0 = p1;//Переписать p1 в p0
        //Выбрать новую точку p1 на экране
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //Сбросить время
        timeStart = Time.time;
    }
    // Update is called once per frame

    public override void Move()
    {
        //Этот метод переопределяет метод Enemy.Move()
        //И реализует линейную интерполяцию
        float u = (Time.time - timeStart) / duration;
        if(u >= 1)
        {
            InitMovement();
            fireEnemy();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);//Применить плавное замедление
        pos = (1 - u) * p0 + u * p1;//Простая линейная интерполяция
    }

    /// <summary>
    /// Эта функция выполняет поиск части корабля в масиве parts
    /// по имени либо по ссылке на игровой объект 
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if(prt.name == n)
            {
                return (prt);
            }
        }
        return (null);
    }
    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if(prt.go == go)
            {
                return (prt);
            }
        }
        return (null);
    }

    /// <summary>
    /// Эти функции возвращают true если данная часть уничтожена
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }
    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }
    bool Destroyed(Part prt)
    {
        if(prt == null)//Если ссылка на часть не была передана 
        {
            return (true);//Часть была уничтожена
        }
        //Вернуть результат сравнения  prt.health <= 0 - если это действительно так то вернуть true, если нет false
        return (prt.health <= 0);
    }
    /// <summary>
    /// Окрашивает в красный цвет только одну часть, а не весь корабль
    /// </summary>
    /// <param name="m"></param>
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                //Если корабль не за границами не наносить урон
                if(!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //Поразить вражеский корабль
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if(prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //Проверить защищена ли эта часть корабля 
                if(prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        //Если хотябы одна из частей ещё не разрушена 
                        if(!Destroyed(s))
                        {
                            //Не наносить повреждения этой части 
                            Destroy(other);
                            return;
                        }
                    }
                }
                //Эта часть не защищена, нанести ей повреждения
                //Получить разрушеную силу
                prtHit.health -= Main.GetWeaponDefinion(p.type).damageOnHit;
                //Показать эффект попадания в часть 
                ShowLocalizedDamage(prtHit.mat);
                if(prtHit.health<=0)
                {
                    //Вместо разрушения всего корабля 
                    //Деактивировать уничтоженную часть
                    prtHit.go.SetActive(false);
                }
                //Проверить был ли корабльполностю разрушен 
                bool allDestroyed = true;//Предположить что разрушен
                foreach(Part prt in parts)
                {
                    if(!Destroyed(prt))//Если какае-то часть еще уцелела
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if(allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    //Уничтожить этот объект 
                    Destroy(this.gameObject);
                }
                Destroy(other);//Уничтожить снаряд
                break;
            case "Rocket":
                Rocket r = other.GetComponent<Rocket>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }
                //Поразить вражеский корабль
                GameObject _goHit = coll.contacts[0].thisCollider.gameObject;
                Part _prtHit = FindPart(_goHit);
                if (_prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }
                //Проверить защищена ли эта часть корабля 
                if (_prtHit.protectedBy != null)
                {
                    foreach (string s in _prtHit.protectedBy)
                    {
                        //Если хотябы одна из частей ещё не разрушена 
                        if (!Destroyed(s))
                        {
                            //Не наносить повреждения этой части 
                            Destroy(other);
                            return;
                        }
                    }
                }
                //Эта часть не защищена, нанести ей повреждения
                //Получить разрушеную силу
                _prtHit.health -= Main.GetWeaponDefinion(r.type).damageOnHit;
                //Показать эффект попадания в часть 
                ShowLocalizedDamage(_prtHit.mat);
                if (_prtHit.health <= 0)
                {
                    //Вместо разрушения всего корабля 
                    //Деактивировать уничтоженную часть
                    _prtHit.go.SetActive(false);
                }
                //Проверить был ли корабльполностю разрушен 
                bool _allDestroyed = true;//Предположить что разрушен
                foreach (Part prt in parts)
                {
                    if (!Destroyed(prt))//Если какае-то часть еще уцелела
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (_allDestroyed)
                {
                    Main.S.ShipDestroyed(this);
                    //Уничтожить этот объект 
                    Destroy(this.gameObject);
                }
                Destroy(other);//Уничтожить снаряд
                break;
        }
    }
}
