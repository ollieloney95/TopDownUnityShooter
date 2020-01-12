using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
    [SerializeField] private Transform player;
    public float cameraHeight;
    public float minHeight = 8;
    public float maxHeight = 40;
    public float cameraRadius;
    [SerializeField] private float rotationSpeed=2f;


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        //cameraHeight = 50;
        //cameraRadius = 10;
	}
	
	// Update is called once per frame
	void Update () {
        float _rotY = 0f;
        float _height = 0f;
        cameraRadius = cameraHeight/4 +3;

        if (Application.isEditor)
        {
            if (Input.GetMouseButton(1) && Input.mousePosition.y>Screen.height/2f)
            {
                _rotY += Input.GetAxis("Mouse X") * rotationSpeed;
                _height += Input.GetAxis("Mouse Y") * rotationSpeed / 10f;

                transform.Rotate(0, _rotY, 0);
            }
            //cameraHeight += _height;
            if (cameraHeight > maxHeight) { cameraHeight = maxHeight; }
            if (cameraHeight < minHeight) { cameraHeight = minHeight; }
            Vector3 dir2d = transform.forward;
            dir2d.y = 0;
            dir2d /= Vector3.Magnitude(dir2d);
            Vector3 posLag = player.position + (-1 * dir2d * cameraRadius + Vector3.up * cameraHeight);
            transform.position = posLag;
            transform.LookAt(player);
            transform.Rotate(-90, 0, 0);
        }

        Touch myTouch;
        Vector2 fingerPos = new Vector2(0f, 0f);

        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            myTouch = Input.GetTouch(i);
            if (myTouch.position.y > Screen.height * 2f / 3f)
            {
                fingerPos = myTouch.position;


                _rotY += myTouch.deltaPosition.x * rotationSpeed / 100f;
                _height += myTouch.deltaPosition.y * rotationSpeed / 1000f;

            }
        }
     

        transform.Rotate(0, _rotY, 0);
        cameraHeight += _height;
        if (cameraHeight > maxHeight) { cameraHeight = maxHeight; }
        if (cameraHeight < minHeight) { cameraHeight = minHeight; }
        Vector3 direction2d = transform.forward;
        direction2d.y = 0;
        direction2d /= Vector3.Magnitude(direction2d);
        Vector3 positionLag = player.position + (-1 * direction2d * cameraRadius + Vector3.up * cameraHeight);
        transform.position = positionLag;
        transform.LookAt(player);
        transform.Rotate(-90, 0, 0);

    }
    public void rotate_camera(float speed)
    {
        transform.Rotate(0, speed, 0);
    }
        
}
