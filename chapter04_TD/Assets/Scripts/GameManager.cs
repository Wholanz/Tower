using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public bool m_debug = false;

    public ArrayList m_PathNodes;

    // �����б�
    public ArrayList m_EnemyList=new ArrayList();

    // ����
    [HideInInspector]
    public int m_wave = 0;

    // ����
    public int m_life = 10;

    // ����
    public int m_point = 10;

    // ����
    GUIText m_txt_wave;
    GUIText m_txt_life;
    GUIText m_txt_point;

    // ��ť
    GUIButton m_button;

    // ��ǰѡ�еİ�ťID
    int m_ID;

    // ���ص�λprefab
    public Transform m_guardPrefab;

    // �������ײ��
    public LayerMask m_groundlayer;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

        // ��ȡ����
        m_txt_wave = this.transform.FindChild("txt_wave").GetComponent<GUIText>();
        m_txt_life = this.transform.FindChild("txt_life").GetComponent<GUIText>();
        m_txt_point = this.transform.FindChild("txt_point").GetComponent<GUIText>();

        // ��ʼ������
        m_txt_wave.text = "<color=red>wave</color> " + m_wave;
        m_txt_life.text = "<color=red>life</color> " + m_life;
        m_txt_point.text = "<color=red>point</color> " + m_point;

        BuildPath();

        m_button = this.transform.FindChild("button_0").GetComponent<GUIButton>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //�������Ϊ0
        if (m_life <= 0)
            return;
	
        // ����������
        bool press=Input.GetMouseButton(0);

        // �ɿ�������
        bool mouseup = Input.GetMouseButtonUp(0);

        // ������λ��
        Vector3 mousepos = Input.mousePosition;

        // �������ƶ�����
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // �����ǰ��ťID����0�����Ҵ����ɿ�������
        if (m_ID > 0 && mouseup )
        {
            //���С��5�����������أ�ʲôҲ����
            if (m_point < 5)
            {
                m_ID = 0;
                m_button.SetOnActiv(false);
                return;
            }

            //����һ������������������
            Ray ray = Camera.main.ScreenPointToRay(mousepos);

            //����������������ײ
            RaycastHit hit;
            if ( Physics.Raycast(ray, out hit, 100, m_groundlayer) )
            {
                //�����ײ���λ��
                int ix = (int)hit.point.x;
                int iz = (int)hit.point.z;

                if (ix >= GridMap.Instance.MapSizeX || iz >= GridMap.Instance.MapSizeZ || ix<0 || iz<0 )
                    return;

                // �����ǰ��Ԫ����԰ڷŷ��ص�λ
                if (GridMap.Instance.m_map[ix, iz].fieldtype == MapData.FieldTypeID.GuardPosition)
                {
                    Vector3 pos = new Vector3((int)hit.point.x + 0.5f, 0, (int)hit.point.z + 0.5f);

                    // �������ص�λ
                    Instantiate(m_guardPrefab, pos, Quaternion.identity);
                    m_ID = 0;

                    // ��ť���»ָ�������״̬
                    m_button.SetOnActiv(false);

                    // ����5������
                    SetPoint(-5);
                }
            }
        }

        // ��ð�ť��ID
        int id = m_button.UpdateState(mouseup, Input.mousePosition);
        if (id > 0)
        {
            m_ID = id;
            m_button.SetOnActiv(true);
            return;
        }

        // �ƶ������
        GameCamera.Inst.Control(press, mx, my);
	}

    // ���²���
    public void SetWave(int wave)
    {
        m_wave= wave;

        m_txt_wave.text = "<color=red>wave</color> " + m_wave;

    }

    // ��������
    public void SetDamage(int damage)
    {
        m_life -= damage;

        m_txt_life.text = "<color=red>life</color> " + m_life;

    }

    // ���µ���
    public void SetPoint(int point)
    {
        m_point += point;

        m_txt_point.text = "<color=red>point</color> " + m_point;

    }

    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNodes = new ArrayList();

        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");

        for (int i = 0; i < objs.Length; i++)
        {
            PathNode node = objs[i].GetComponent<PathNode>();

            m_PathNodes.Add(node);
        }
    }

    void OnGUI()
    {
        if (m_life <= 0 || ( m_wave == 10 && m_EnemyList.Count==0))
        {
            if (GUI.Button(new Rect(Screen.width * 0.5f - 100, Screen.height * 0.5f - 25, 200, 50), "REPLAY"))
                Application.LoadLevel(Application.loadedLevelName);
        }
    }

    /// <summary>
    /// Debug
    /// </summary>
    void OnDrawGizmos()
    {
        if (!m_debug || m_PathNodes==null )
            return;

        Gizmos.color = Color.blue;

        foreach (PathNode node in m_PathNodes)
        {
            if (node.m_next != null)
            {
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }
}
