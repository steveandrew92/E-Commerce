using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DB_User : MonoBehaviour {
    private string[] arr_user = new string[100];
    public string username, full_name, address, country, phone, email, join_date, website, about_me, gender, birthday, id_device;
    public Texture2D photo;
    public string hash = "espeed@sg!";

    private GUI_Controller gui_control;
    private DB_Country countries;

    void Start () {
        Debug.Log(Decrypt("MbeH8lPmhaI="));

        countries = FindObjectOfType<DB_Country>();
        gui_control = FindObjectOfType<GUI_Controller>();
    }

    public string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(result, 0, result.Length);
            }
        }
    }

    public string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(result);
            }
        }
    }

    public void Send_Confirm_Email(string dbUsername, string dbFullName, string dbEmail, string dbAddress, string dbIDRegion, string dbPhone)
    {
        StartCoroutine(Post_Send_Confirm_Email(dbUsername, dbFullName, dbEmail, dbAddress, dbIDRegion, dbPhone));
    }

    IEnumerator Post_Send_Confirm_Email(string dbUsername, string dbFullName, string dbEmail, string dbAddress, string dbIDRegion, string dbPhone)
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/send_confirm_email.php";

        WWWForm form = new WWWForm();
        form.AddField("username_Post", dbUsername);
        form.AddField("full_name_Post", dbFullName);
        form.AddField("email_Post", dbEmail);
        form.AddField("address_Post", dbAddress);
        form.AddField("region_Post", dbIDRegion);
        form.AddField("phone_Post", dbPhone);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            //apps.Show_Message("confirmation_email");
        }
    }

    public void Send_Deactivate_Account()
    {
        StartCoroutine(Post_Send_Deactivate_Account(username, full_name, email));
    }

    IEnumerator Post_Send_Deactivate_Account(string dbUsername, string dbFullName, string dbEmail)
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/send_deactivate_account.php";

        WWWForm form = new WWWForm();
        form.AddField("username_Post", dbUsername);
        form.AddField("full_name_Post", dbFullName);
        form.AddField("email_Post", dbEmail);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            //apps.Show_Message("confirmation_email");
        }
    }

    public void Sign_Up(string dbUsername, string dbPassword, string dbFullName, string dbAddress, string dbIDRegion, string dbPhone, string dbEmail)
    {
        StartCoroutine(Post_Sign_Up(dbUsername, dbPassword, dbFullName, dbAddress, dbIDRegion, dbPhone, dbEmail));
    }

    IEnumerator Post_Sign_Up(string dbUsername, string dbPassword, string dbFullName, string dbAddress, string dbIDRegion, string dbPhone, string dbEmail)
    {
        string URL = "http://www.steventandrean.com/espeed_app/sign_up.php";
        WWWForm form = new WWWForm();
        form.AddField("username_Post", dbUsername);
        form.AddField("password_Post", Encrypt(dbPassword));
        form.AddField("full_name_Post", dbFullName);
        form.AddField("address_Post", dbAddress);
        form.AddField("id_region_Post", dbIDRegion);
        form.AddField("phone_Post", dbPhone);
        form.AddField("email_Post", dbEmail);
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
            print(www.error);
        else
        {
            username = dbUsername;
            full_name = dbFullName;
            address = dbAddress;
            phone = dbPhone;
            email = dbEmail;

            Send_Confirm_Email(dbUsername, dbFullName, dbEmail, dbAddress, dbIDRegion, dbPhone);
            Set_Active_Session(username, SystemInfo.deviceUniqueIdentifier);
            print("email : " + dbEmail);

            gui_control.On_Change_Scene("scene_browse");
        }
    }

    public void Logout(string dbID, string dbIDDevice)
    {
        StartCoroutine(Post_Logout(dbID, dbIDDevice));
    }

    IEnumerator Post_Logout(string dbID, string dbIDDevice)
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/logout.php";
        WWWForm form = new WWWForm();
        form.AddField("id_Post", dbID);
        form.AddField("id_device_Post", dbIDDevice);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        //hist_act = FindObjectOfType<DB_Activity>();
        //hist_act.Add_Activity_History(id, "Sign Out");

        Application.Quit();
    }

    public void Get_Profile_Picture(string dbUsername)
    {
        StartCoroutine(Post_Get_Profile_Picture(dbUsername));
    }

    IEnumerator Post_Get_Profile_Picture(string dbUsername)
    {
        //apps = FindObjectOfType<AppMessage>();

        string URL = "http://www.steventandrean.com/espeed_app/get_image_user.php";
        WWWForm form = new WWWForm();
        form.AddField("username_Post", dbUsername);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            WWW url_image = new WWW(www.downloadHandler.text);
            yield return url_image;
            photo = url_image.texture;
        }
    }

    public void Login(string dbId, string dbPassword)
    {
        StartCoroutine(Post_Login(dbId, dbPassword));
    }

    string Get_Data_Value(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }

    IEnumerator Post_Login(string dbId, string dbPassword)
    {
        //apps = FindObjectOfType<AppMessage>();
        string URL = "http://www.steventandrean.com/espeed_app/login.php";
        WWWForm form = new WWWForm();
        form.AddField("id_Post", dbId);
        form.AddField("password_Post", Encrypt(dbPassword));

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            if (www.downloadHandler.text.Equals("ID Not Found"))
                Debug.Log("ID Not Found");
                //apps.Show_Message("id_not_found");
            else if (www.downloadHandler.text.Equals("Wrong Password"))
                Debug.Log("Wrong Password");
                //apps.Show_Message("wrong_password");
            else
            {
                string st_user_data = www.downloadHandler.text;
                arr_user = st_user_data.Split(';');
                username = Get_Data_Value(arr_user[0], "Username:");
                full_name = Get_Data_Value(arr_user[0], "FullName:");
                address = Get_Data_Value(arr_user[0], "Address:");
                country = Get_Data_Value(arr_user[0], "Country:");
                phone = Get_Data_Value(arr_user[0], "Phone:");
                email = Get_Data_Value(arr_user[0], "Email:");
                gender = Get_Data_Value(arr_user[0], "Gender:");
                website = Get_Data_Value(arr_user[0], "Website:");
                about_me = Get_Data_Value(arr_user[0], "AboutMe:");
                birthday = Get_Data_Value(arr_user[0], "Birthday:");
                join_date = Get_Data_Value(arr_user[0], "JoinDate:");

                Debug.Log("Username : " + username);

                Get_Profile_Picture(username);
                Set_Active_Session(username, SystemInfo.deviceUniqueIdentifier);

                //hist_act = FindObjectOfType<DB_Activity>();
                //hist_act.Add_Activity_History(id, "Login");

                gui_control.On_Change_Scene("scene_browse");
            }
        }
    }

    public void Set_Active_Session(string dbUsername, string dbIDDevice)
    {
        StartCoroutine(Post_Set_Active_Session(dbUsername, dbIDDevice));
    }

    IEnumerator Post_Set_Active_Session(string dbUsername, string dbIDDevice)
    {
        //apps = FindObjectOfType<AppMessage>();
        Debug.Log(dbIDDevice);
        string URL = "http://www.steventandrean.com/espeed_app/set_active_session.php";

        WWWForm form = new WWWForm();
        form.AddField("username_Post", dbUsername);
        form.AddField("id_device_Post", dbIDDevice);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        www.chunkedTransfer = false;

        yield return www.SendWebRequest();

        print(www.downloadHandler.text);

        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
            id_device = dbIDDevice;
    }

    public void Change_Account_Data(string old_username, string new_username, string new_fullName, string new_country, string new_website, string new_aboutMe, string new_address, string new_email, string new_phoneNo, string new_gender, string new_birthday)
    {
        StartCoroutine(Post_Change_Account_Data(old_username, new_username, new_fullName, new_country, new_website, new_aboutMe, new_address, new_email, new_phoneNo, new_gender, new_birthday));
    }

    IEnumerator Post_Change_Account_Data(string old_username, string new_username, string new_fullName, string new_country, string new_website, string new_aboutMe, string new_address, string new_email, string new_phoneNo, string new_gender, string new_birthday)
    {
        //apps = FindObjectOfType<AppMessage>();

        string new_verifEmail = "0";

        if ((new_username == username) &&
            (new_fullName == full_name) &&
            (new_country == country) &&
            (new_website == website) &&
            (new_aboutMe == about_me) &&
            (new_address == address) &&
            (new_email == email) &&
            (new_phoneNo == phone) &&
            (new_gender == gender) &&
            (new_birthday == birthday))
        {
            //Show Message No Change Detected
        }

        if ((new_username == "") ||
            (new_fullName == "") ||
            (new_country == "") ||
            (new_email == "") ||
            (new_phoneNo == ""))
        {
            //apps.Show_Message("null_inputted");
        }   
        else
        {
            if (email != new_email)
                new_verifEmail = "0";
            else
                new_verifEmail = "1";

            string URL = "http://www.steventandrean.com/espeed_app/change_account_data.php";
            WWWForm form = new WWWForm();
            form.AddField("old_username_Post", old_username);
            form.AddField("new_username_Post", new_username);
            form.AddField("full_name_Post", new_fullName);
            form.AddField("country_Post", new_country);
            form.AddField("website_Post", new_website);
            form.AddField("about_me_Post", new_aboutMe);
            form.AddField("address_Post", new_address);
            form.AddField("email_Post", new_email);
            form.AddField("phone_no_Post", new_phoneNo);
            form.AddField("gender_Post", new_gender);
            form.AddField("birthday_Post", new_birthday);
            form.AddField("verif_email_Post", new_verifEmail);

            UnityWebRequest www = UnityWebRequest.Post(URL, form);
            www.chunkedTransfer = false;

            yield return www.SendWebRequest();

            print(www.downloadHandler.text);

            if (!string.IsNullOrEmpty(www.error))
            {
                print(www.error);
                //apps.Show_Message("no_internet_found");
            }
            else if (www.downloadHandler.text.Equals("success"))
            {
                username = new_username;
                full_name = new_fullName;
                Debug.Log("coba  " + new_country);
                country = countries.Get_Name_Country(new_country);
                website = new_website;
                about_me = new_aboutMe;
                address = new_address;
                email = new_email;
                phone = new_phoneNo;
                gender = new_gender;
                birthday = new_birthday;

                //hist_act = FindObjectOfType<DB_Activity>();
                //hist_act.Add_Activity_History(id, "Change Account Data");
                gui_control.On_Change_Scene("scene_my_account");
            }
        }
    }

    public void Change_Profile_Picture()
    {
        StartCoroutine(Post_Change_Profile_Picture(username));
    }

    IEnumerator Post_Change_Profile_Picture(string dbUsername)
    {
        //apps = FindObjectOfType<AppMessage>();

        string prefixURL = "http://www.steventandrean.com/espeed_app/upload/preupload_user_photo.php?";
        string suffixURL = "p_username=" + dbUsername;
        string URL = prefixURL + suffixURL;

        WWW www = new WWW(URL);
        Application.OpenURL(URL);

        yield return www;

        print(www.text);
        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            //apps.Show_Message("no_internet_found");
        }
        else
        {
            Get_Profile_Picture(username);
            gui_control.Clean_Profile_Picture();
        }            
    }
}
