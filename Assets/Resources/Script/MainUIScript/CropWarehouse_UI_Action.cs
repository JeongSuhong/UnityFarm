using UnityEngine;
using System.Collections;

public class CropWarehouse_UI_Action : Base_Button_Action {

    public GameObject Crop_UI_Prefab;
    public UITable Table_Crops;

    private static CropWarehouse_UI_Action instance = null;
    public static CropWarehouse_UI_Action Get_Inctance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(CropWarehouse_UI_Action)) as CropWarehouse_UI_Action;
        }

        if (null == instance)
        {
            GameObject obj = new GameObject("CropWarehouse_UI_Action ");
            instance = obj.AddComponent(typeof(CropWarehouse_UI_Action)) as CropWarehouse_UI_Action;

            Debug.Log("Fail to get CropWarehouse_UI_Action Instance");
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }


    public void Set_Crop_UI(int CropID)
    {
        int Crop_Count = UserManager.Get_Inctance().Get_Crop_Count(CropID);

        Transform ChildObj =  Table_Crops.transform.FindChild(CropID.ToString());

        if (ChildObj != null)
        {
            // Crop이 인벤에 없는데 창고에 남아있다면 .( 작물을 판매 )
            if (Crop_Count == -99)
            {
                ChildObj.gameObject.SetActive(false);
                Table_Crops.repositionNow = true;
            }
            else
            {

                ChildObj.gameObject.SetActive(true);

                if (Crop_Count > 99)
                {

                }

                ChildObj.GetComponent<CropWarehouse_Crop_UI_Action>().Set_Crop_Info(CropID, Crop_Count);
            }
            
            return;

        }

        GameObject Crop_UI = Instantiate(Crop_UI_Prefab, Table_Crops.transform) as GameObject;
        Crop_UI.transform.localScale = Vector3.one;
        Crop_UI.name = CropID.ToString();

        Crop_UI.GetComponent<CropWarehouse_Crop_UI_Action>().Set_Crop_Info(CropID, Crop_Count);

        Table_Crops.repositionNow = true;
    }


    public void View_CropWarehouseUI()
    {
        GetComponent<UIPanel>().alpha = 1;
        StartCoroutine(C_Check_View_Menu());
    }
    public void NotView_CropWarehouseUI()
    {
        StartCoroutine(C_Check_View_Menu());
    }
    IEnumerator C_Check_View_Menu()
    {
        float y = transform.localPosition.y;

        if (y <= -500f)
        {
            while (y <= 450f)
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

            GetComponent<UIPanel>().alpha = 0;
        }

        yield break;
    }
}
