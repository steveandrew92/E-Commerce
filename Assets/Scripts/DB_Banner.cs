using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DB_Banner : MonoBehaviour
{
    public Texture2D[] arr_banner;
    public Texture2D btn_next, btn_prev;
    public int index;

    private float sWidth, sHeight;
    private bool OnUpdate;
    private int CountClick; //CountClick for flag click button prev and next

    private Scene scene;
    private GUI_Controller gui_control;

    public void Start()
    {
        index = 0;
        sWidth = Screen.width;
        sHeight = Screen.height;
        OnUpdate = true;
        CountClick = 0;

        scene = SceneManager.GetActiveScene();
        gui_control = FindObjectOfType<GUI_Controller>();
    }

    public void Update()
    {
        if (OnUpdate)
            StartCoroutine(Update_Index_Banner());
    }

    IEnumerator Update_Index_Banner()
    {
        OnUpdate = false;

        yield return new WaitForSeconds(3);

        if (CountClick == 0)
        {
            if (index == 2)
                index = 0;
            else
                index++;

            //GameObject.Find("Img_Banner").GetComponent<RawImage>().texture = arr_banner[index];
            OnUpdate = true;
        }
        else
            CountClick--;
    }

    public void OpenURL (string URL)
    {
        WWW www = new WWW(URL);
        Application.OpenURL(URL);
    }

    public void OnGUI()
    {
        /*
        if (scene.name == "scene_browse")
        {
            float sizeBtn = sWidth * 0.05f;
            float widthBanner = sWidth;
            float heightBanner = sHeight * 0.232f;

            GUI.BeginGroup(new Rect(0, sHeight * 0.075f, widthBanner, heightBanner));
            if (GUI.Button(new Rect(0, 0, widthBanner, heightBanner), arr_banner[index]))
            {
                switch (index)
                {
                    case 0:
                        gui_control.On_Change_Scene("scene_whatsyourneed");
                        break;
                    case 1:
                        SampleWebView.Url = "http://espeed.sg/en/promos.aspx";
                        gui_control.On_Change_Scene("scene_web_view");
                        break;
                    case 2:
                        SampleWebView.Url = "https://goo.gl/maps/UddtsPy7HJp";
                        gui_control.On_Change_Scene("scene_web_view");
                        break;
                }
            }
            GUI.EndGroup();
        }    
        */
    }
}
