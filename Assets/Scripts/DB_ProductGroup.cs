using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_ProductGroup : MonoBehaviour {
    private string[] arr_product_group;
    private string[] id, name = new string[20];
    private int jumlah_group;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        arr_product_group = new string[20];
        id = new string[20];
        name = new string[20];

        Get_Product_Group();
    }

    public string Get_Name_Group(int group_id)
    {
        string result = "";
        for (int i = 1; i < arr_product_group.Length; i++)
        {
            Debug.Log("a : " + group_id + " " + id[i]);
            
            if (id[i-1] == group_id.ToString())
                result = name[i-1];
        }

        Debug.Log("result : " + result);
        return result;
    }

    public void Get_Product_Group()
    {
        StartCoroutine(Post_Get_Product_Group());
    }

    string Get_Data_Value(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator Post_Get_Product_Group()
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/get_all_product_group.php";

        WWW www = new WWW(URL);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            //for (int i = 0; i < 10; i++)
            int i = 0;
            while (true)
            {
                string st_product_data = www.text;
                arr_product_group = st_product_data.Split(';');
                id[i] = Get_Data_Value(arr_product_group[i], "ID:");
                name[i] = Get_Data_Value(arr_product_group[i], "Name:");

                i++;
                jumlah_group = i;
            }
        }
    }
}
