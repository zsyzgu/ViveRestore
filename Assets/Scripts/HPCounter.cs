using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCounter : MonoBehaviour {
    public RectTransform hpPanel;
    public RectTransform hpPanelBackground;
    private float hp = 1f;

    public void demage(float p)
    {
        hp -= p;
        if (hp < 0f)
        {
            hp = 0f;
        }
    }

    public bool gameOver()
    {
        return hp <= 0f;
    }

    void Start () {
		
	}
	
	void Update () {
        hpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(hpPanelBackground.rect.width * hp, hpPanelBackground.rect.height);
    }
}
