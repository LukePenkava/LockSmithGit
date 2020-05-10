using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class PlayerInput : MonoBehaviour
{


    /*
     Setup 1 ( 1x deltaX )
     spinSpeed 8
     maxSpeed 22
     smoothing 0.1
     lockSpeed 7

     Setup 2 ( 2x deltaX )
     spinSpeed 1
     maxSpeed 10
     smoothing 0.1
     lockSpeed 5
     *
     * */


    Game_Manager gameManager;

    public Camera cam;

    Vector2 lastTouchPos;
    [SerializeField]float spinSpeed = 0.2f;
    [SerializeField]float maxSpinSpeed = 30f;
    [SerializeField]float spinSmoothing = 0.05f;
    float smoothedSpinValue = 0f;
    float spinVelocity = 0;

    bool spinning = false;
    public bool Spinning
    {
        get { return spinning; }
    }

    float inputValue = 0;
    public float Inputvalue
    {
        get { return inputValue; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        float spinValue = 0f;
        
        

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);            

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
                spinning = true;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                /*float deltaY = touch.position.y - lastTouchPos.y;
                print("Delta " + deltaY + " PosY " + touch.position.y + " LastPos " + lastTouchPos.y);
                lastTouchPos = touch.position;*/

                float deltaX = touch.deltaPosition.x * -1f;              


                float finalDelta = Mathf.Abs(deltaX * deltaX) * Mathf.Sign(deltaX);
                spinValue = finalDelta * Time.deltaTime * spinSpeed;

                if(Mathf.Abs(spinValue) > maxSpinSpeed)
                {
                    spinValue = (maxSpinSpeed * Mathf.Sign(spinValue));
                }
                spinning = true;

            }
            else if(touch.phase == TouchPhase.Ended)
            {
                spinValue = 0;
                spinning = false;
                RaycastTouch(touch.position);
            }

        }
        else
        {
            spinning = false;
        }

        smoothedSpinValue = Mathf.SmoothDamp(smoothedSpinValue, spinValue, ref spinVelocity, spinSmoothing);

        inputValue = spinValue;

        /*if (Mathf.Abs(smoothedSpinValue) > 0.01f)
        {
            print(smoothedSpinValue);
        }*/


        //print("TouchCount " + Input.touchCount + " Value " + spinValue);

        gameManager.Spin(smoothedSpinValue);
    }

    void RaycastTouch(Vector2 touchPos)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(touchPos);

        if(Physics.Raycast(ray, out hit))
        {
            //print(hit.collider.transform.parent.name);
            if (hit.collider.transform.parent.tag == "Number")
            {
                int index = hit.collider.transform.parent.GetComponent<SpinnerNumber>().Index;
                gameManager.NumberSelected(index);
            }
        }
        
    }
}
