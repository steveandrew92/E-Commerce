using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DB_Country : MonoBehaviour
{
    private string[] arr_country;
    public string[] id, region_name, prefix_phone, show_list;
    private Scene scene;

    private DB_User users;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);

        users = FindObjectOfType<DB_User>();
        scene = SceneManager.GetActiveScene();

        Req_Data_Country();
    }

    public void Req_Data_Country()
    {
        StartCoroutine(Get_Data_Country());
    }

    string Get_Data_Value(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    int Get_ID_Country(string country_name)
    {
        int result = 0;
        for (int i = 1; i < arr_country.Length; i++)
        {
            
            if (region_name[i] == country_name)
                result = i;
        }
        return result;
    }

    public string Get_Name_Country(string country_id)
    {
        string result = "";
        for (int i = 1; i < arr_country.Length; i++)
        { 
            Debug.Log(i + " " + id[i] + " " + country_id);
            if (id[i] == country_id)
                result = region_name[i];
        }
        return result;
    }

    public void Set_Data_Value()
    {
        Dropdown dropdown = GameObject.Find("Dd_Country").GetComponent<Dropdown>();
        dropdown.options.Clear();
        foreach (string c in show_list)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = c });
        }

        scene = SceneManager.GetActiveScene();
        if (scene.name == "scene_edit_profile")
        {
            Debug.Log("1 " + users.country);
            Debug.Log("2 " + Get_ID_Country(users.country));
            GameObject.Find("Dd_Country").GetComponent<Dropdown>().value = Get_ID_Country(users.country);
        }
    }

    IEnumerator Get_Data_Country()
    {
        WWW www = new WWW ("http://www.steventandrean.com/espeed_app/master_country.php");
        yield return www;
        string st_country_data = www.text;
        arr_country = st_country_data.Split(';');
        Debug.Log(www.text);
        id = new string[arr_country.Length];
        region_name = new string[arr_country.Length];
        prefix_phone = new string[arr_country.Length];
        show_list = new string[arr_country.Length];
        for (int i = 1; i < arr_country.Length; i++)
        {
            id[i] = Get_Data_Value(arr_country[i-1], "ID:");
            region_name[i] = Get_Data_Value(arr_country[i-1], "Name:");
            prefix_phone[i] = Get_Data_Value(arr_country[i-1], "PrefixPhone:");
            show_list[i] = region_name[i] + " [" + prefix_phone[i] + "]";
        }
        Set_Data_Value();
    }
}
