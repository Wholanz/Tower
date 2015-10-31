using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public PathNode m_currentNode;

    // 生命
    public int m_life = 15;

    public int m_maxlife = 15;

    // 移动速度
    public float m_speed = 2;

    // 敌人的类型
    public enum TYPE_ID
    {
        GROUND,
        AIR,
    }
    public TYPE_ID m_type = TYPE_ID.GROUND;

    public EnemySpawner m_spawn;

    // 生命条
    public Transform m_lifeBarObject;
    protected LifeBar m_bar;

	// Use this for initialization
	void Start () {

        m_spawn.m_liveEnemy++;

        GameManager.Instance.m_EnemyList.Add(this);

        // 创建生命条
        Transform obj=(Transform)Instantiate(m_lifeBarObject, this.transform.position, Quaternion.identity);
        m_bar = obj.GetComponent<LifeBar>();
        m_bar.Ini(m_life, m_maxlife, 2, 0.2f);
      
	}

    void OnDisable()
    {
        if ( m_spawn )
            m_spawn.m_liveEnemy--;

        if ( GameManager.Instance )
            GameManager.Instance.m_EnemyList.Remove(this);

        if ( m_bar )
            Destroy(m_bar.gameObject);
    }
	
	// Update is called once per frame
	void Update () {

        RotateTo();
        MoveTo();

       
	}

    public void RotateTo()
    {
        float current= this.transform.eulerAngles.y;

        this.transform.LookAt(m_currentNode.transform);

        Vector3 target = this.transform.eulerAngles;

        float next=Mathf.MoveTowardsAngle(current, target.y, 120 * Time.deltaTime);

        this.transform.eulerAngles = new Vector3(0, next, 0);
       
    }


    public void MoveTo()
    {
        Vector3 pos1 = this.transform.position;
        Vector3 pos2 = m_currentNode.transform.position;
        float dist = Vector2.Distance(new Vector2(pos1.x,pos1.z),new Vector2(pos2.x,pos2.z));
        if (dist < 1.0f)
        {
            if (m_currentNode.m_next == null)
            {
                GameManager.Instance.SetDamage(1);
                Destroy(this.gameObject);
            }
            else
                m_currentNode = m_currentNode.m_next;
        }

        this.transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));

        m_bar.SetPosition(this.transform.position, 4.0f);
    }

    public void SetDamage(int damage)
    {
        m_life -= damage;

        if (m_life <= 0)
        {
            
            GameManager.Instance.SetPoint(2);
            Destroy(this.gameObject);
        }
        else
            m_bar.UpdateLife(m_life, m_maxlife);
    }
}
