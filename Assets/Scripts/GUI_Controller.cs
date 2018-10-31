using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUI_Controller : MonoBehaviour {
    private string id, username, password, retype_password, full_name, country, address, phone, email, website, about_me, gender, birthday; //id is for username / email on login
    private bool update_pp;

    private AppMessage apps;
    private DB_User users;
    private DB_Product products;
    private DB_ProductGroup product_groups;
    private DB_Country countries;
    private SceneHistory scene_history;
    private Scene scene;

    public string url_webview;

    private bool set_timer;
    private float touch_timer;

    //scene browse
    private Vector2 fp_touch; // first finger position
    private Vector2 lp_touch; // last finger position
    public GameObject panel_horizontal, panel_vertical;

    private bool get_fp_horizontal_vertical;
    private float fp_horizontal, fp_vertical;
    private float lp_horizontal, lp_vertical;

    private void Update()
    {
        if (scene.name == "scene_my_account")
        {
            RawImage pp = GameObject.Find("Img_Profile_Picture").GetComponent<RawImage>();
            Debug.Log("my acc : " + pp.texture);

            if (pp.texture == null)
                Set_Profile_Picture(users.photo);
        }
        if (scene.name == "scene_browse")
        {
            if (set_timer)
                touch_timer += Time.deltaTime;

            float pos_y = 0.355f * Screen.height;

            if (!get_fp_horizontal_vertical)
            {
                fp_horizontal = panel_horizontal.transform.position.x;
                fp_vertical = panel_vertical.transform.position.y;
                get_fp_horizontal_vertical = true;
            }

            //GUI.Label(new Rect(15, Screen.height * 0.325f, Screen.width - 10f, 50f), "OUR PRODUCT", titleStyle);

            //swipe controller
            foreach (Touch touch in Input.touches)
            {
                float fp_swipe_horizontal = panel_horizontal.transform.position.x;
                RectTransform rt_horizontal = (RectTransform)panel_horizontal.transform;
                float lp_swipe_horizontal = panel_horizontal.transform.position.x - rt_horizontal.rect.width - (0.5f * Screen.width);

                float fp_swipe_vertical = panel_vertical.transform.position.y;
                RectTransform rt_vertical = (RectTransform)panel_vertical.transform;
                float lp_swipe_vertical = panel_vertical.transform.position.y - rt_vertical.rect.height - (0.5f * Screen.height);
               
                if (touch.phase == TouchPhase.Began)
                {
                    fp_touch = touch.position;
                    lp_touch = touch.position;
                    touch_timer = 0f;
                    set_timer = true;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    lp_touch = touch.position;

                    if (Touch_Area_Product(lp_touch.y, pos_y, pos_y + rt_horizontal.rect.height))
                    {
                       float delta_horizontal = 0f;
                    
                        //left swipe
                        if ((fp_touch.x > lp_touch.x) && (lp_swipe_horizontal >= (0 - fp_horizontal - (rt_horizontal.rect.width * 0.9f))))
                        {
                            delta_horizontal = lp_touch.x - fp_touch.x;
                            panel_horizontal.transform.Translate(delta_horizontal, 0, 0);
                        }

                        //right swipe
                        if ((fp_touch.x < lp_touch.x) && (fp_swipe_horizontal <= fp_horizontal))
                        {
                            delta_horizontal = lp_touch.x - fp_touch.x;
                            panel_horizontal.transform.Translate(delta_horizontal, 0, 0);
                        }
                    }
                    else
                    {
                        //VERTICAL SWIPE
                        float delta_vertical = 0f;

                        GameObject.Find("Text1").GetComponent<Text>().text = "UP";
                        GameObject.Find("Text2").GetComponent<Text>().text = fp_swipe_vertical.ToString();

                        //up swipe
                        if ((fp_touch.y < lp_touch.y) && (lp_vertical >= (0 - fp_vertical - (rt_vertical.rect.width * 0.9f))))
                        {
                            //GameObject.Find("Text1").GetComponent<Text>().text = "UP";
                            delta_vertical = lp_touch.y - fp_touch.y;
                            panel_vertical.transform.Translate(0, delta_vertical, 0);
                        }

                        //down swipe
                        if ((fp_touch.y > lp_touch.y) && (fp_swipe_vertical >= fp_vertical))
                        {
                            //GameObject.Find("Text1").GetComponent<Text>().text = "DOWN";
                            delta_vertical = lp_touch.y - fp_touch.y;
                            panel_vertical.transform.Translate(0, delta_vertical, 0);
                        }
                    }
                    
                    fp_touch = touch.position;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if ((fp_touch.x < lp_touch.x) && (fp_swipe_horizontal > fp_horizontal)) // right swipe
                    {
                        panel_horizontal.transform.position = new Vector2(fp_horizontal, panel_horizontal.transform.position.y);
                    }

                    set_timer = false;
                }
            }
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        apps = FindObjectOfType<AppMessage>();
        users = FindObjectOfType<DB_User>();
        products = FindObjectOfType<DB_Product>();
        countries = FindObjectOfType<DB_Country>();
        product_groups = FindObjectOfType<DB_ProductGroup>();
        scene_history = FindObjectOfType<SceneHistory>();

        scene = SceneManager.GetActiveScene();
        
        Initialized();
    }

    string Generate_Random_String()
    {
        string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int charAmount = Random.Range(5, 10);
        string result = "";

        for (int i = 0; i < charAmount; i++)
        {
            result += characters[Random.Range(0, characters.Length)];
        }

        return result;
    }

    private void Initialized()
    {
        if (scene.name == "scene_login")
        {
            username = null;
            password = null;
            retype_password = null;
            full_name = null;
            country = null;
            address = null;
            phone = null;
            email = null;
            website = null;
            about_me = null;
            gender = null;
            birthday = null;
        }
        else if (scene.name == "scene_sign_up")
        {
            username = null;
            password = null;
            retype_password = null;
            full_name = null;
            country = null;
            address = null;
            phone = null;
            email = null;
            website = null;
            about_me = null;
            gender = null;
            birthday = null;
        }
        else if (scene.name == "scene_my_account")
        {
            GameObject.Find("Img_ProfilePicture").GetComponent<RawImage>().texture = users.photo;
            GameObject.Find("Txt_Username").GetComponent<Text>().text = "@" + users.username;
            GameObject.Find("Txt_FullName").GetComponent<Text>().text = users.full_name;
            GameObject.Find("Txt_Country").GetComponent<Text>().text = users.country;
            GameObject.Find("Txt_AboutMe").GetComponent<Text>().text = users.about_me;
            GameObject.Find("Txt_Joined").GetComponent<Text>().text = "Joined On " + users.join_date;

            //if (users.verif_email)
            //{
            //    img_verif_email.SetActive(true);
            //    btn_verif_email.SetActive(false);
            //}
            //else
            //{
            //    img_verif_email.SetActive(false);
            //    btn_verif_email.SetActive(true);
            //}

            //users.Get_Profile_Picture(users.id);
        }
        else if (scene.name == "scene_edit_profile")
        {
            GameObject.Find("Input_Username").GetComponent<InputField>().text = users.username;
            GameObject.Find("Input_FullName").GetComponent<InputField>().text = users.full_name;
            GameObject.Find("Input_Website").GetComponent<InputField>().text = users.website;
            GameObject.Find("Input_AboutMe").GetComponent<InputField>().text = users.about_me;
            GameObject.Find("Input_Address").GetComponent<InputField>().text = users.address;
            GameObject.Find("Input_Email").GetComponent<InputField>().text = users.email;
            GameObject.Find("Input_PhoneNo").GetComponent<InputField>().text = users.phone;
            GameObject.Find("Input_Birthday").GetComponent<InputField>().text = users.birthday;
            countries.Set_Data_Value();

            switch (users.gender)
            {
                case "Male":
                    GameObject.Find("Dd_Gender").GetComponent<Dropdown>().value = 1;
                    break;
                case "Female":
                    GameObject.Find("Dd_Gender").GetComponent<Dropdown>().value = 2;
                    break;
                default:
                    GameObject.Find("Dd_Gender").GetComponent<Dropdown>().value = 0;
                    break;
            }
 
            //country immidiately on DB_Country.cs (Set_Data_Value())
        }
        else if (scene.name == "scene_detail_product_other")
        {
            GameObject.Find("Img_Product").GetComponent<RawImage>().texture = products.photo[products.index];
            GameObject.Find("Lbl_Name").GetComponent<Text>().text = "" + products.name[products.index];
            GameObject.Find("Lbl_CreateDate").GetComponent<Text>().text = "Posted from " + products.create_date[products.index];
            //GameObject.Find("Lbl_Price").GetComponent<Text>().text = products.price[products.index] + " SGD";
            GameObject.Find("Lbl_Price").GetComponent<Text>().text = "Price by Negotiation";
            GameObject.Find("Lbl_Likes").GetComponent<Text>().text = products.likes[products.index] + " Like(s)";
            //GameObject.Find("Lbl_Group").GetComponent<Text>().text = "In " + product_groups.Get_Name_Group(products.id_group[products.index]);
            GameObject.Find("Lbl_Description").GetComponent<Text>().text = "" + products.desc1[products.index];
        }
        else if (scene.name == "scene_detail_product_desktop")
        {
            Debug.Log("Asdasdas " + products.brand[products.index]);
            GameObject.Find("Img_Product").GetComponent<RawImage>().texture = products.photo[products.index];
            GameObject.Find("Lbl_Name").GetComponent<Text>().text = "" + products.name[products.index];
            GameObject.Find("Lbl_CreateDate").GetComponent<Text>().text = "Posted from " + products.create_date[products.index];
            //GameObject.Find("Lbl_Price").GetComponent<Text>().text = products.price[products.index] + " SGD";
            GameObject.Find("Lbl_Price").GetComponent<Text>().text = "Price by Negotiation";
            GameObject.Find("Lbl_Likes").GetComponent<Text>().text = products.likes[products.index] + " Like(s)";
            //GameObject.Find("Lbl_Group").GetComponent<Text>().text = "In " + product_groups.Get_Name_Group(products.id_group[products.index]);
            GameObject.Find("Lbl_Brand").GetComponent<Text>().text = "" + products.brand[products.index];
            GameObject.Find("Lbl_Model").GetComponent<Text>().text = "" + products.model[products.index];
            GameObject.Find("Lbl_OS").GetComponent<Text>().text = "" + products.os[products.index];
            GameObject.Find("Lbl_Processor").GetComponent<Text>().text = "" + products.processor[products.index];
            GameObject.Find("Lbl_RAM").GetComponent<Text>().text = "" + products.ram[products.index];
            GameObject.Find("Lbl_Capacity").GetComponent<Text>().text = "" + products.capacity[products.index];
            GameObject.Find("Lbl_Warranty").GetComponent<Text>().text = "" + products.warranty_type[products.index];
        }
        else if (scene.name == "scene_detail_product_handphone")
        {
            GameObject.Find("Img_Product").GetComponent<RawImage>().texture = products.photo[products.index];
            GameObject.Find("Lbl_Name").GetComponent<Text>().text = "" + products.name[products.index];
            GameObject.Find("Lbl_CreateDate").GetComponent<Text>().text = "Posted from " + products.create_date[products.index];
            //GameObject.Find("Lbl_Price").GetComponent<Text>().text = products.price[products.index] + " SGD";
            GameObject.Find("Lbl_Price").GetComponent<Text>().text = "Price by Negotiation";
            GameObject.Find("Lbl_Likes").GetComponent<Text>().text = products.likes[products.index] + " Like(s)";
            //GameObject.Find("Lbl_Group").GetComponent<Text>().text = "In " + product_groups.Get_Name_Group(products.id_group[products.index]);
            GameObject.Find("Lbl_Brand").GetComponent<Text>().text = "" + products.brand[products.index];
            GameObject.Find("Lbl_Model").GetComponent<Text>().text = "" + products.model[products.index];
            GameObject.Find("Lbl_Capacity").GetComponent<Text>().text = "" + products.capacity[products.index];
            GameObject.Find("Lbl_Warranty").GetComponent<Text>().text = "" + products.warranty_type[products.index];
        }
        else if (scene.name == "scene_detail_product_laptop")
        {
            GameObject.Find("Img_Product").GetComponent<RawImage>().texture = products.photo[products.index];
            GameObject.Find("Lbl_Name").GetComponent<Text>().text = "" + products.name[products.index];
            GameObject.Find("Lbl_CreateDate").GetComponent<Text>().text = "Posted from " + products.create_date[products.index];
            //GameObject.Find("Lbl_Price").GetComponent<Text>().text = products.price[products.index] + " SGD";
            GameObject.Find("Lbl_Price").GetComponent<Text>().text = "Price by Negotiation";
            GameObject.Find("Lbl_Likes").GetComponent<Text>().text = products.likes[products.index] + " Like(s)";
            //GameObject.Find("Lbl_Group").GetComponent<Text>().text = "In " + product_groups.Get_Name_Group(products.id_group[products.index]);
            GameObject.Find("Lbl_Brand").GetComponent<Text>().text = "" + products.brand[products.index];
            GameObject.Find("Lbl_Model").GetComponent<Text>().text = "" + products.model[products.index];
            GameObject.Find("Lbl_Processor").GetComponent<Text>().text = "" + products.processor[products.index];
            GameObject.Find("Lbl_RAM").GetComponent<Text>().text = "" + products.ram[products.index];
            GameObject.Find("Lbl_Capacity").GetComponent<Text>().text = "" + products.capacity[products.index];
            GameObject.Find("Lbl_ScreenSize").GetComponent<Text>().text = "" + products.screen_size[products.index];
            GameObject.Find("Lbl_Resolution").GetComponent<Text>().text = "" + products.resolution[products.index];
            GameObject.Find("Lbl_Warranty").GetComponent<Text>().text = "" + products.warranty_type[products.index];
        }
        else if (scene.name == "scene_detail_product_vr")
        {
            GameObject.Find("Img_Product").GetComponent<RawImage>().texture = products.photo[products.index];
            GameObject.Find("Lbl_Name").GetComponent<Text>().text = "" + products.name[products.index];
            GameObject.Find("Lbl_CreateDate").GetComponent<Text>().text = "Posted from " + products.create_date[products.index];
            //GameObject.Find("Lbl_Price").GetComponent<Text>().text = products.price[products.index] + " SGD";
            GameObject.Find("Lbl_Price").GetComponent<Text>().text = "Price by Negotiation";
            GameObject.Find("Lbl_Likes").GetComponent<Text>().text = products.likes[products.index] + " Like(s)";
            //GameObject.Find("Lbl_Group").GetComponent<Text>().text = "In " + product_groups.Get_Name_Group(products.id_group[products.index]);
            GameObject.Find("Lbl_Brand").GetComponent<Text>().text = "" + products.brand[products.index];
        }
        else if (scene.name == "scene_browse")
        {
            get_fp_horizontal_vertical = false;
            set_timer = false;
            touch_timer = 0f;

            for (int i = 0; i < products.max_prod; i++)
            {
                GameObject.Find("Name_Fav_Product_" + i).GetComponent<Text>().text = "" + products.name[i];

                if (products.price[i] > 0)
                    GameObject.Find("Price_Fav_Product_" + i).GetComponent<Text>().text = "" + products.price[i];
                else
                    GameObject.Find("Price_Fav_Product_" + i).GetComponent<Text>().text = "Price by Negotiation";

                GameObject.Find("Img_Fav_Product_" + i).GetComponent<RawImage>().texture = products.photo[i];
            }
        }
    }

    public void Clean_Profile_Picture()
    {
        GameObject.Find("Img_Profile_Picture").GetComponent<RawImage>().texture = null;
    }

    public void Set_Profile_Picture(Texture2D photo)
    {
        GameObject.Find("Img_Profile_Picture").GetComponent<RawImage>().texture = photo;
    }

    public void On_Change_Scene(string scene)
    {
        if (scene == "last_scene")
        {
            string last_scene = scene_history.last_scene;
            Debug.Log(last_scene);
            SceneManager.LoadScene(last_scene, LoadSceneMode.Single);
            scene_history.Set_History_Scene(last_scene);
        }
        else
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            scene_history.Set_History_Scene(scene);
        }
    }

    public void On_Value_Changed(string type)
    {
        switch (type)
        {
            case "id":
                id = GameObject.Find("Input_ID").GetComponent<InputField>().text;
                break;
            case "username":
                username = GameObject.Find("Input_Username").GetComponent<InputField>().text;
                break;
            case "password":
                password = GameObject.Find("Input_Password").GetComponent<InputField>().text;
                break;
            case "retype_password":
                retype_password = GameObject.Find("Input_RetypePassword").GetComponent<InputField>().text;
                break;
            case "full_name":
                full_name = GameObject.Find("Input_FullName").GetComponent<InputField>().text;
                break;
            case "address":
                address = GameObject.Find("Input_Address").GetComponent<InputField>().text;
                break;
            case "phone":
                phone = GameObject.Find("Input_PhoneNo").GetComponent<InputField>().text;
                break;
            case "email":
                email = GameObject.Find("Input_Email").GetComponent<InputField>().text;
                break;
            case "website":
                website = GameObject.Find("Input_Website").GetComponent<InputField>().text;
                break;
            case "about_me":
                about_me = GameObject.Find("Input_AboutMe").GetComponent<InputField>().text;
                break;
            case "birthday":
                birthday = GameObject.Find("Input_Birthday").GetComponent<InputField>().text;
                break;
            case "gender":
                gender = GameObject.Find("Input_Gender").GetComponent<InputField>().text;
                break;
            case "country":
                Set_Prefix_Phone_Country();
                break;
        }
    }

    public void Set_Prefix_Phone_Country()
    {
        int chosenList = GameObject.Find("Dd_Country").GetComponent<Dropdown>().value;

        InputField txt_phone = GameObject.Find("Input_PhoneNo").GetComponent<InputField>();
        txt_phone.text = "+" + countries.prefix_phone[chosenList];
    }

    public void On_Req_Detail_Product(int idx)
    {
        if (touch_timer < 0.2f)
        {
            products.index = idx;
            switch (products.id_group[idx])
            {
                case 16100000: On_Change_Scene("scene_detail_product_handphone"); break;
                case 16200000: On_Change_Scene("scene_detail_product_desktop"); break;
                case 16300000: On_Change_Scene("scene_detail_product_laptop"); break;
                case 16400000: On_Change_Scene("scene_detail_product_vr"); break;
                default: On_Change_Scene("scene_detail_product_other"); break;
            }
        }
    }

    public void On_Req_Detail_Partnership(string web)
    {
        if (touch_timer < 0.2f)
        {
            SampleWebView.Url = web;
            On_Change_Scene("scene_web_view");
        }
    }

    bool Touch_Area_Product(float pos_touch, float first_area, float last_area)
    {
        bool result = false;

        if ((pos_touch >= first_area) && (pos_touch <= last_area))
            result = true;

        return result;
    }

    bool Error_Inputted()
    {
        bool result = false;
        int chosenIndex = GameObject.Find("Dd_Country").GetComponent<Dropdown>().value;

        Debug.Log("Test");

        if ((username == null) ||
            (password == null) ||
            (retype_password == null) ||
            (full_name == null) ||
            (address == null) ||
            (phone.Length <= countries.prefix_phone[chosenIndex].Length) ||
            (email == null))
        {
            //apps.Show_Message("null_inputted");
            result = true;
        }

        if ((password != retype_password) && (retype_password != null))
        {
            //apps.Show_Message("no_internet_found");
            result = true;
        }

        return result;
    }

    public void On_Sign_Up()
    {
        if (!Error_Inputted())
        {
            int chosenIndex = GameObject.Find("Dd_Country").GetComponent<Dropdown>().value;
            users.Sign_Up(username, password, full_name, address, countries.id[chosenIndex], phone, email);
        }
    }

    public void On_Login()
    {
        if ((id.Length <= 0) || (password.Length <= 0))
        {
            //apps.Show_Message("null_inputted");
        }
        else
        {
            users.Login(id, password);
        }
    }

    public void On_Logout()
    {
        users.Logout(users.username, SystemInfo.deviceUniqueIdentifier);
    }

    public void On_Change_Account_Data()
    {
        //users.Check_Multi_Login(users.id);

        string new_username, new_fullName, new_country, new_website, new_aboutMe, new_address, new_email, new_phoneNo, new_gender, new_birthday;

        int chosenCountryIndex = GameObject.Find("Dd_Country").GetComponent<Dropdown>().value;
        int chosenGenderIndex = GameObject.Find("Dd_Gender").GetComponent<Dropdown>().value;

        new_username = GameObject.Find("Input_Username").GetComponent<InputField>().text;
        new_fullName = GameObject.Find("Input_FullName").GetComponent<InputField>().text;
        new_country = countries.id[chosenCountryIndex];
        new_website = GameObject.Find("Input_Website").GetComponent<InputField>().text;
        new_aboutMe = GameObject.Find("Input_AboutMe").GetComponent<InputField>().text;
        new_address = GameObject.Find("Input_Address").GetComponent<InputField>().text;
        new_email = GameObject.Find("Input_Email").GetComponent<InputField>().text;
        new_phoneNo = GameObject.Find("Input_PhoneNo").GetComponent<InputField>().text;
        new_birthday = GameObject.Find("Input_Birthday").GetComponent<InputField>().text;

        if (chosenGenderIndex > 0)
            new_gender = GameObject.Find("Dd_Gender").GetComponent<Dropdown>().captionText.text;
        else
            new_gender = "";
        
        users.Change_Account_Data(users.username, new_username, new_fullName, new_country, new_website, new_aboutMe, new_address, new_email, new_phoneNo, new_gender, new_birthday);
    }

    public void On_Send_Deactivate_Account()
    {
        users.Send_Deactivate_Account();
    }

    public void On_Show_Web_View (string url)
    {
        //url_webview = url;
        //Debug.Log ("Before URL : " + url_webview);
        SampleWebView.Url = url;
        On_Change_Scene("scene_web_view");
    }

    public void On_Change_Profile_Picture()
    {
        //users.Check_Multi_Login(users.id);

        //update_pp = true;
        users.Change_Profile_Picture();
    }
}
