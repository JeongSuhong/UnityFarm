using UnityEngine;
using System.Collections;

/*
 * 시민이 집에서 잠들면 뜨는 Sleep Gauge의 스크립트. 
 */

public class House_Sleep_Gauge_Action : MonoBehaviour {

    public UISlider Gauge;

    public void Set_Gauge(float max, float value, Vector3 house_pos)
    {
        Vector3 p = Camera.main.WorldToViewportPoint(house_pos);
        transform.position = UICamera.mainCamera.ViewportToWorldPoint(p);

        p = transform.localPosition;
        p.x = Mathf.RoundToInt(p.x) - 110f ;
        p.y = Mathf.RoundToInt(p.y) + 200f;
        p.z = 0f;
        transform.localPosition = p;
        Gauge.value = 1 - (value / max) ;
    }
}
