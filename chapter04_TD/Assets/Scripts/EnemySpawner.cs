using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    // ����
    public EnemyTable[] m_enemies;

    // ��ʼ·��
    public PathNode m_startNode;

    // �洢���˳���˳���XML
    public TextAsset xmldata;

    // �������еĴ�XML��ȡ������
    ArrayList m_enemylist;

    // ������һ�����˳�����ʱ��
    float m_timer = 0;

    // �������˵����к�
    int m_index = 0;

    // ��ǰ���ĵ�������,ֻ�����ٵ�ǰ�������е���,���ܽ�����һ��
    public int m_liveEnemy = 0;


	// Use this for initialization
	void Start () {

        // ��ȡXML
        ReadXML();

        // ��ȡ��һ������
        SpawnData data = (SpawnData)m_enemylist[m_index];
        m_timer = data.wait;
	
	}

    // ��ȡXML
    void ReadXML()
    {
        m_enemylist = new ArrayList();

        XMLParser xmlparse = new XMLParser();
        XMLNode node = xmlparse.Parse(xmldata.text);

        XMLNodeList list = node.GetNodeList("ROOT>0>table");
        for (int i = 0; i < list.Count; i++)
        {
          
            string wave = node.GetValue("ROOT>0>table>" + i + ">@wave");
            string enemyname = node.GetValue("ROOT>0>table>" + i + ">@enemyname");
            string level = node.GetValue("ROOT>0>table>" + i + ">@level");
            string wait = node.GetValue("ROOT>0>table>" + i + ">@wait");

            SpawnData data = new SpawnData();
            data.wave = int.Parse(wave);
            data.enemyname = enemyname;
            data.level = int.Parse(level);
            data.wait = float.Parse(wait);

            m_enemylist.Add(data);
        }
    }

	
	// Update is called once per frame
	void Update () {

        SpawnEnemy();
	
	}

    // ÿ��һ��ʱ������һ������
    void SpawnEnemy()
    {
        // ����Ѿ��������е���
        if (m_index >= m_enemylist.Count)
            return;

        // ��ȡ��һ������
        SpawnData data = (SpawnData)m_enemylist[m_index];

        // �����һ����������һ�� ��Ҫ�ȴ�ǰһ���ĵ���ȫ������
        if (GameManager.Instance.m_wave < data.wave && m_liveEnemy > 0)
        {
            return;
        
        }

        // �ȴ�
        m_timer -= Time.deltaTime;
        if (m_timer > 0)
            return;

        if (GameManager.Instance.m_wave < data.wave)
        {
            // ����һ��
            GameManager.Instance.SetWave(data.wave);
        }

        // ���ҵ���
        Transform enemyprefab = FindEnemy(data.enemyname);

        // ���ɵ���
        if (enemyprefab != null)
        {
            Transform trans=(Transform)Instantiate(enemyprefab, this.transform.position, Quaternion.identity);

            Enemy enemy = trans.GetComponent<Enemy>();

            // ���õ��˵ĳ���·��
            enemy.m_currentNode = m_startNode;

            // ���õ��˵����ɵ�
            enemy.m_spawn = this;

            // ���õ��˳�ʼ��ת����
            enemy.transform.LookAt(m_startNode.transform);
            float ry = enemy.transform.eulerAngles.y;
            enemy.transform.eulerAngles = new Vector3(0,ry,0);


            // ����data.level���õ��˵ȼ�����ʾ�����ԣ�ֻ�Ǽ򵥵ĸ��ݲ������ӵ��˵�����
            enemy.m_life = data.level * 3;
            enemy.m_maxlife = data.level * 3;

        }


        // ��һ��
        m_index++;
        if (m_index >= m_enemylist.Count)
            return;

        // �����һ�����˵�����
        SpawnData nextdata = (SpawnData)m_enemylist[m_index];

        // ������һ��������Ҫ�ȴ���ʱ��
        m_timer = data.wait;


    }

    // ��EnemyTable����enemy��prefab
    Transform FindEnemy(string enemyname)
    {
        foreach (EnemyTable enemy in m_enemies)
        {
            if (enemy.enemyName.CompareTo(enemyname) == 0)
            {
                return enemy.enemyPrefab;
            }
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.tif");
    }
 
    // ������˱�ʶ
    [System.Serializable]
    public class EnemyTable
    {
        public string enemyName = "";
        public Transform enemyPrefab;
    }

    // XML����
    public class SpawnData
    {
        // ����
        public int wave = 1;
        public string enemyname = "";
        public int level = 1;
        public float wait = 1.0f;
    }
}
