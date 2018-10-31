using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DB_Product : MonoBehaviour
{
    public int max_prod = 10;

    private string[] arr_product;
    public string[] id, name, desc1, desc2, url_photo, create_date = new string[100];
    public string[] brand, model, processor, ram, capacity, screen_size, resolution, warranty_type, os = new string[100]; //detail
    public int[] stock, likes, id_group;
    public float[] price, fp_browse;
    public Texture2D[] photo = new Texture2D[100];
    public int jumlah_item;

    private Scene scene;
    public Vector2 scrollPosition = Vector2.zero;
    private float space_x;

    public int index = -1;
    
    private GUI_Controller gui_control;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        arr_product = new string[100];
        id = new string[100];
        name = new string[100];
        desc1 = new string[100];
        desc2 = new string[100];
        stock = new int[100];
        likes = new int[100];
        id_group = new int[100];
        price = new float[100];
        photo = new Texture2D[100];
        url_photo = new string[100];
        create_date = new string[100];
        brand = new string[100];
        model = new string[100];
        processor = new string[100];
        ram = new string[100];
        capacity = new string[100];
        screen_size = new string[100];
        resolution = new string[100];
        warranty_type = new string[100];
        os = new string[100];

        Get_Product();

        gui_control = FindObjectOfType<GUI_Controller>();
    }

    public void Get_Image_Product(string dbID, int idx)
    {
        StartCoroutine(Post_Get_Image_Product(dbID, idx));
    }

    IEnumerator Post_Get_Image_Product(string dbID, int idx)
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/get_image_product.php";
        WWWForm form = new WWWForm();
        form.AddField("id_Post", dbID);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            WWW url_image = new WWW(www.downloadHandler.text);
            yield return url_image;
            photo[idx] = url_image.texture;
        }
    }

    public void Get_Product()
    {
        StartCoroutine(Post_Get_Product());
    }

    string Get_Data_Value(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator Post_Get_Product()
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/get_all_product.php";

        WWW www = new WWW(URL);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            for (int i = 0; i < max_prod; i++)
            {
                string st_product_data = www.text;
                arr_product = st_product_data.Split(';');
                id[i] = Get_Data_Value(arr_product[i], "ID:");
                name[i] = Get_Data_Value(arr_product[i], "Name:");
                desc1[i] = Get_Data_Value(arr_product[i], "Desc1:");
                desc2[i] = Get_Data_Value(arr_product[i], "Desc2:");
                string tmp_stock = Get_Data_Value(arr_product[i], "Stock:");
                stock[i] = int.Parse(tmp_stock);
                string tmp_likes = Get_Data_Value(arr_product[i], "Likes:");
                likes[i] = int.Parse(tmp_likes);
                string tmp_price = Get_Data_Value(arr_product[i], "Price:");
                price[i] = float.Parse(tmp_price);
                string tmp_id_group = Get_Data_Value(arr_product[i], "Group:");
                id_group[i] = int.Parse(tmp_id_group);
                desc1[i] = Get_Data_Value(arr_product[i], "Desc1:");
                url_photo[i] = Get_Data_Value(arr_product[i], "Photo:");
                create_date[i] = Get_Data_Value(arr_product[i], "CreateDate:");
                brand[i] = Get_Data_Value(arr_product[i], "Brand:");
                model[i] = Get_Data_Value(arr_product[i], "Model:");
                processor[i] = Get_Data_Value(arr_product[i], "Processor:");
                ram[i] = Get_Data_Value(arr_product[i], "RAM:");
                capacity[i] = Get_Data_Value(arr_product[i], "Capacity:");
                screen_size[i] = Get_Data_Value(arr_product[i], "ScreenSize:");
                resolution[i] = Get_Data_Value(arr_product[i], "Resolution:");
                warranty_type[i] = Get_Data_Value(arr_product[i], "WarrantyType:");
                os[i] = Get_Data_Value(arr_product[i], "OS:");

                Debug.Log(i + ": " + name[i]);

                Get_Image_Product(id[i], i);
                //i++;
                jumlah_item = i;
            }
        }
    }
    
    int get_idx_prod_choosen(float pos)
    {
        int i = 0;
        bool found = false;
        int result = -1;

        while ((i < max_prod) || (!found))
        {
            if (fp_browse[i] < pos)
                result = i - 1;

            i++;
        }

        return result;
    }
    
    void OnGUI()
    {
        scene = SceneManager.GetActiveScene();

        GUIStyle backgroundStyle = new GUIStyle(GUI.skin.label);

        GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
        headingStyle.normal.textColor = Color.black;
        headingStyle.fontSize = Screen.width / 33;
        headingStyle.wordWrap = false;
        headingStyle.fontStyle = FontStyle.Bold;

        GUIStyle subHeadingStyle = new GUIStyle(GUI.skin.label);
        subHeadingStyle.normal.textColor = Color.black;
        subHeadingStyle.fontSize = Screen.width / 50;

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.normal.textColor = Color.black;
        titleStyle.fontSize = Screen.width / 22;
        titleStyle.fontStyle = FontStyle.Bold;
    }
}
