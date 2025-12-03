using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tacada : MonoBehaviour {

    public float maxX, maxY;
    private float x, y;
    private Vector2 pi;
    private Vector2 pf;

    LineRenderer lr;

    // Start is called before the first frame update
    void Start() {
        lr = GetComponent<LineRenderer>();

        if (lr != null) {
            lr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        int i;

        for (i = 0; i < Input.touchCount; i++) { 

            Touch touch = Input.GetTouch(i);

            if(touch.phase == TouchPhase.Began) {
                pi = touch.position;
                pf = touch.position;
                x = 0;
                y = 0;
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position);
            }

            if (touch.phase == TouchPhase.Moved) {
                pf = touch.position;
                x = (pi.x - pf.x) * 0.03f;
                y = (pi.y - pf.y) * 0.03f;

                if (x > maxX) {
                    x = maxX;
                }
                if (y > maxY) {
                    y = maxY;
                    lr.SetPosition(1, new Vector3 (transform.position.x + x, transform.position.y, transform.position.z + y));
                }
                if (touch.phase == TouchPhase.Ended) { 
                    GetComponent<Rigidbody>().AddForce(new Vector3(2 * x, 0, 2 * y), ForceMode.Impulse);
                    lr.enabled = false;
                }            
            }
        }
    }
}
