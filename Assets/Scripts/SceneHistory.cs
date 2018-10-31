using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHistory : MonoBehaviour {
    public string[] scene = new string[100];
    public string last_scene;
    public int idx;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        idx = 0;
        last_scene = "";
        scene = new string[100];
    }

    public void Set_History_Scene(string data)
    {
        if (idx > 0)
            last_scene = scene[idx-1];

        scene[idx] = data;
        idx++;
    }
}
