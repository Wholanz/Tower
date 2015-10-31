using UnityEngine;
using System.Collections;

public class GUITextFadeIn : MonoBehaviour {

    public bool m_isPlay = false;
    public float m_fadeSpeed = 0.2f;

    GUIText m_text;

    float m_alpha = 0;

    

	// Use this for initialization
	void Start () {

        m_text = this.GetComponent<GUIText>();

        m_text.material.color = new Color(0.5f, 1, 1, m_alpha);

        if (!m_isPlay)
            m_text.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!m_isPlay)
            return;

        m_alpha += m_fadeSpeed * Time.deltaTime;
        if (m_alpha > 1.0f)
            m_alpha = 1.0f;

        m_text.material.color = new Color(0.5f, 1, 1, m_alpha);
        
	}

    public void PlayAnimation()
    {
        m_isPlay = true;

        m_text.material.color = new Color(0.5f, 1, 1, m_alpha);

        m_text.enabled = true; ;
    }
}
