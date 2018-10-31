using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour {
    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position

    public GameObject max_y_gameobject;
    private float first_y;
    
    private void Start()
    {
        first_y = transform.position.y;
    }

    // Update is called once per frame
    void Update ()
    {
        RectTransform rt = (RectTransform)max_y_gameobject.transform;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;

                if ((fp.y > lp.y) && (transform.position.y >= first_y)) // up swipe
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + (lp.y - fp.y));
                }
                else if ((fp.y < lp.y) && (max_y_gameobject.transform.position.y <= (0 + 0.3f * Screen.height))) // down swipe
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + (lp.y - fp.y));
                }

                fp = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (transform.position.y < first_y) // up swipe
                {
                    transform.position = new Vector2(transform.position.x, first_y);
                }
            }
        }
    }
}
