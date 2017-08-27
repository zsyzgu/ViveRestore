using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePanelControl : MonoBehaviour {
    public GameObject mainCamera;
    public RectTransform timePanel;
    public RectTransform timePanelBackground;
    public float duration = 5f;
    private CaliControl caliControl;

    private bool started = false;
    private float startTime = 0f;

    void Start()
    {
        caliControl = mainCamera.GetComponent<CaliControl>();
    }

    private float escapedTime()
    {
        return Time.time - startTime;
    }

    public void startTimeKeeping()
    {
        started = true;
        startTime = Time.time;
    }

    void Update()
    {
        if (started)
        {
            float schedule = escapedTime() / duration;
            if (schedule > 1f)
            {
                started = false;
                caliControl.timeOut();
            }
            else
            {
                timePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(timePanelBackground.rect.width * schedule, timePanelBackground.rect.height);
            }
        } else
        {
            timePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, timePanelBackground.rect.height);
        }
    }
}
