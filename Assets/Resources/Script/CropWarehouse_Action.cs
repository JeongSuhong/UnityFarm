using UnityEngine;
using System.Collections;

public class CropWarehouse_Action : MonoBehaviour {



    public void View_CropWarehouseUI()
    {
        GetComponent<UIPanel>().alpha = 1;
        StartCoroutine(C_Check_View_Menu());
    }
    public void NotView_CropWarehouseUI()
    {
        StartCoroutine(C_Check_View_Menu());
        GetComponent<UIPanel>().alpha = 0;
    }

    IEnumerator C_Check_View_Menu()
    {
        float y = transform.localPosition.y;

        if (y <= -500f)
        {
            while (y <= 490f)
            {
                transform.position += Vector3.up * Time.deltaTime * 4f;
                y = transform.localPosition.y;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (y >= -500)
            {
                transform.position += Vector3.down * Time.deltaTime * 4f;
                y = transform.localPosition.y;

                yield return new WaitForSeconds(0.01f);
            }
        }

        yield break;
    }
}
