using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMessage : MonoBehaviour {
    public Texture2D[] imgMessage;
    public int idx_message;
    public bool flag_gui;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show_Message(string message)
    {
        switch (message)
        {
            case "confirmation_email":
                idx_message = 0; break;
            case "id_not_found":
                idx_message = 1; break;
            case "no_enough_balance":
                idx_message = 2; break;
            case "no_internet_found":
                idx_message = 3; break;
            case "null_inputted":
                idx_message = 4; break;
            case "passwords_dont_match":
                idx_message = 5; break;
            case "success_change_account_data":
                idx_message = 6; break;
            case "success_change_password":
                idx_message = 7; break;
            case "success_login":
                idx_message = 8; break;
            case "success_sign_up":
                idx_message = 9; break;
            case "wrong_password":
                idx_message = 10; break;
            case "multi_login":
                idx_message = 99; break;
        }

        flag_gui = true;
    }

    public void OnGUI( )
    {
        if (flag_gui)
        {
            if (GUI.Button(new Rect(Screen.width * 0.05f, Screen.height * 0.35f, Screen.width * 0.9f, Screen.height * 0.3f), imgMessage[idx_message], GUIStyle.none))
            {
                flag_gui = false;

                if (idx_message == 99)
                    Application.Quit();
            }
                
        }       
    }
}
