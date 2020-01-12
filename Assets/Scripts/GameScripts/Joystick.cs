using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour {

    public GameObject joysticksObj;
    public GameObject rightStickBack;
    public GameObject rightStickFront;
    public GameObject leftStickBack;
    public GameObject cam_pos_obj;
    public GameObject draw_aim_obj;
    public GameObject leftStickFront;
    public Canvas canvas;

    public Vector2 leftStickPosition;
    public Vector2 rightStickPosition;

    public Animator _Animator;
    public Animator _Animator_mixamo;

    // Use this for initialization
    void Start () {
        joysticksObj.active = true;
        rightStickBack.active = true;
        rightStickFront.active = true;
        leftStickBack.active = true;
        leftStickFront.active = true;
        rightStickBack.GetComponent<Image>().enabled = true;
        rightStickFront.GetComponent<Image>().enabled = true;
        leftStickBack.GetComponent<Image>().enabled = true;
        leftStickFront.GetComponent<Image>().enabled = true;
        //leftStickPosition = new Vector2(1.5f*Screen.width/8,2*Screen.height/8);
        //rightStickPosition = new Vector2(6 * Screen.width / 8, -Screen.height / 8);
        leftStickPosition = RectTransformUtility.PixelAdjustPoint(leftStickBack.GetComponent<RectTransform>().position, leftStickBack.GetComponent<RectTransform>().transform,canvas);
        rightStickPosition = RectTransformUtility.PixelAdjustPoint(rightStickBack.GetComponent<RectTransform>().position, rightStickBack.GetComponent<RectTransform>().transform, canvas);
    }

    // Update is called once per frame
    void Update() {

        Touch myTouch;
        Vector2 rightFingerPos = new Vector2(0f, 0f);
        Vector2 leftFingerPos = new Vector2(0f, 0f);

        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            myTouch = Input.GetTouch(i);
            if (myTouch.position.x < Screen.width * 4f / 10f && myTouch.position.y < Screen.height * 2f / 3f && myTouch.position.x > Screen.width * 1f / 20f && myTouch.position.y > Screen.height * 1f / 20f)
            {
                leftFingerPos = myTouch.position;
            }
            else if (myTouch.position.x > Screen.width * 6f  / 10f && myTouch.position.y < Screen.height * 2f / 3f && myTouch.position.x < Screen.width * 19f  / 20f&& myTouch.position.y > Screen.height * 1f / 20f)
            {
                rightFingerPos = myTouch.position;
            }
        }
        if (Application.isEditor)
        { 
            if (Input.GetMouseButton(0))
            {
                leftFingerPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(1))
            {
                rightFingerPos = Input.mousePosition;
            }
        }

        float leftFingerPosx = 0f;
        float leftFingerPosy = 0f;
        float rightFingerPosx = 0f;
        float rightFingerPosy = 0f;

        if (leftFingerPos.x != 0f)
        {
            float leftDistance = Mathf.Pow(Mathf.Pow(leftFingerPos.x - leftStickPosition.x,2f) + Mathf.Pow(leftFingerPos.y - leftStickPosition.y, 2f),0.5f);
           
            if (leftDistance > Screen.width / 30f) { 
                leftFingerPosx = (Screen.width/30f) * (leftFingerPos.x - leftStickPosition.x) / leftDistance;
                leftFingerPosy = (Screen.width / 30f) * (leftFingerPos.y - leftStickPosition.y) / leftDistance;
            }
            else
            {
                leftFingerPosx = (leftFingerPos.x - leftStickPosition.x);
                leftFingerPosy = (leftFingerPos.y - leftStickPosition.y);
            }
        leftStickFront.GetComponent<RectTransform>().localPosition = new Vector3(leftFingerPosx, leftFingerPosy, 0f);
        }
        else
        {
            Managers.input.SetInput("horizontal_movement",0f);
            Managers.input.SetInput("vertical_movement", 0f);
            leftStickFront.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        }
        if (rightFingerPos.x != 0f)
        {
            float rightDistance = Mathf.Pow(Mathf.Pow(rightFingerPos.x - rightStickPosition.x, 2f) + Mathf.Pow(rightFingerPos.y - rightStickPosition.y, 2f), 0.5f);
            
            if (rightDistance > Screen.width / 30f)
            {
                rightFingerPosx = (Screen.width / 30f) * (rightFingerPos.x - rightStickPosition.x) / rightDistance;
                rightFingerPosy = (Screen.width / 30f) * (rightFingerPos.y - rightStickPosition.y) / rightDistance;
            }
            else
            {
                rightFingerPosx = (rightFingerPos.x - rightStickPosition.x);
                rightFingerPosy = (rightFingerPos.y - rightStickPosition.y);
            }
            rightStickFront.GetComponent<RectTransform>().localPosition = new Vector3(rightFingerPosx, rightFingerPosy, 0f);
        }
        else
        {
            Managers.input.SetInput("horizontal_shoot", 0f);
            Managers.input.SetInput("vertical_shoot", 0f);
            rightStickFront.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        }

        if (UIController.gameState.Equals("Main"))
        {
            rightStickBack.SetActive(true);
            rightStickFront.SetActive(true);
            leftStickBack.SetActive(true);
            leftStickFront.SetActive(true);
            Managers.input.SetInput("horizontal_movement", leftFingerPosx / (Screen.width / 30));
            Managers.input.SetInput("vertical_movement", leftFingerPosy / (Screen.height / 30));
            //cam_pos_obj.GetComponent<CameraPosition>().rotate_camera((1/cam_pos_obj.GetComponent<CameraPosition>().cameraHeight)*leftFingerPosx / (Screen.width / 30));


            Managers.input.SetInput("horizontal_shoot", rightFingerPosx / (Screen.width / 30));
            Managers.input.SetInput("vertical_shoot", rightFingerPosy / (Screen.height / 30));
        }
        else
        {
            rightStickBack.SetActive(false);
            rightStickFront.SetActive(false);
            leftStickBack.SetActive(false);
            leftStickFront.SetActive(false);
}
        if(new Vector2(rightFingerPosx,rightFingerPosy).magnitude >0){
            draw_aim_obj.GetComponent<DrawAim>().draw_aim();
        }else{
            draw_aim_obj.GetComponent<DrawAim>().hide_aim();
        }

    }
}
