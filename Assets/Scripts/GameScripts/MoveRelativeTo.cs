using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MoveRelativeTo : MonoBehaviour {
    public float moveSpeed = 4.0f;
    public float yvel=0f;
    public float jumpHeight=0.2f;
    public bool canJump=false;
    public CharacterController _charController;

	[SerializeField] public float testvariable;

	[SerializeField] public Transform gunTrans;
	//public Animator _Animator;
    public Animator _Animator_mixamo;

    [SerializeField] public Transform target;
	[SerializeField] public GameObject rig;

    // Use this for initialization
    void Start () { 
		//_Animator = rig.GetComponent<Animator> (); 
		_charController = GetComponent<CharacterController>();
	}

	void SetAnimationStates(){
        
        if (Application.isEditor)
        {
            float relativeRotation = 0;
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetAxis("Vertical") >= 0)
                {
                    relativeRotation = (180 / Mathf.PI) * (Mathf.Atan(Input.GetAxis("Horizontal") / Input.GetAxis("Vertical")));
                }
                else
                {
                    relativeRotation = 180 + (180 / Mathf.PI) * (Mathf.Atan(Input.GetAxis("Horizontal") / Input.GetAxis("Vertical")));
                }
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetAxis("Vertical") >= 0)
                {
                    relativeRotation = (180 / Mathf.PI) * (Mathf.Atan(Input.GetAxis("Horizontal") / Input.GetAxis("Vertical")));
                }
                else
                {
                    relativeRotation = -180 + (180 / Mathf.PI) * (Mathf.Atan(Input.GetAxis("Horizontal") / Input.GetAxis("Vertical")));
                }
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                if (Input.GetAxis("Vertical") >= 0)
                {
                    relativeRotation = 0;
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    relativeRotation = 180;
                }
            }

            float aimRotation = gunTrans.rotation.eulerAngles.y % 360;
            if (aimRotation > 180) { aimRotation -= 360; }
            relativeRotation = relativeRotation - aimRotation;
            while (relativeRotation < -180) { relativeRotation += 360; }
            while (relativeRotation > 180) { relativeRotation -= 360; }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Mathf.Abs(relativeRotation) < 45)
                {
                    //_Animator.SetBool("Forward", true);
                    //_Animator.SetBool("Right", false);
                    //_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runForward");

                }
                if (135 > relativeRotation && relativeRotation > 45)
                {
                    //_Animator.SetBool("Forward", false);
                    //_Animator.SetBool("Right", true);
                    //_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runRight");
                }
                if (-135 < relativeRotation && relativeRotation < -45)
                {
                    ///_Animator.SetBool("Forward", false);
                    //_Animator.SetBool("Right", false);
                    //_Animator.SetBool("Left", true);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runLeft");
                }
                if (Mathf.Abs(relativeRotation) > 135)
                {
                    //_Animator.SetBool("Forward", false);
                    //_Animator.SetBool("Right", false);
                    //_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", true);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runBack");
                }
            }
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                //_Animator.SetBool("Forward", false);
                //_Animator.SetBool("Right", false);
                //_Animator.SetBool("Left", false);
                //_Animator.SetBool("Backward", false);
                //_Animator.SetBool("Idle", true);
                _Animator_mixamo.SetTrigger("idle");
            }
            testvariable = aimRotation;
        }
        else                                                      //-------------------------------------------------------------------------------- not in editor-----------------------------------------------
        {
            float relativeRotation = 0;
            if (Managers.input.GetInput("horizontal_movement") > 0)
            {
                if (Managers.input.GetInput("vertical_movement") >= 0)
                {
                    relativeRotation = (180 / Mathf.PI) * (Mathf.Atan(Managers.input.GetInput("horizontal_movement") / Managers.input.GetInput("vertical_movement")));
                }
                else
                {
                    relativeRotation = 180 + (180 / Mathf.PI) * (Mathf.Atan(Managers.input.GetInput("horizontal_movement") / Managers.input.GetInput("vertical_movement")));
                }
            }
            if (Managers.input.GetInput("horizontal_movement") < 0)
            {
                if (Managers.input.GetInput("vertical_movement") >= 0)
                {
                    relativeRotation = (180 / Mathf.PI) * (Mathf.Atan(Managers.input.GetInput("horizontal_movement") / Managers.input.GetInput("vertical_movement")));
                }
                else
                {
                    relativeRotation = -180 + (180 / Mathf.PI) * (Mathf.Atan(Managers.input.GetInput("horizontal_movement") / Managers.input.GetInput("vertical_movement")));
                }
            }
            if (Managers.input.GetInput("horizontal_movement") == 0)
            {
                if (Managers.input.GetInput("vertical_movement") >= 0)
                {
                    relativeRotation = 0;
                }
                if (Managers.input.GetInput("vertical_movement") < 0)
                {
                    relativeRotation = 180;
                }
            }

            float aimRotation = gunTrans.rotation.eulerAngles.y % 360;
            if (aimRotation > 180) { aimRotation -= 360; }
            relativeRotation = relativeRotation - aimRotation;
            while (relativeRotation < -180) { relativeRotation += 360; }
            while (relativeRotation > 180) { relativeRotation -= 360; }
            if (Managers.input.GetInput("horizontal_movement") != 0 || Managers.input.GetInput("vertical_movement") != 0)
            {
                if (Mathf.Abs(relativeRotation) < 45)
                {
                    //_Animator.SetBool("Forward", true);
                    //_Animator.SetBool("Right", false);
                    //_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runForward");

                }
                if (135 > relativeRotation && relativeRotation > 45)
                {
                    //_Animator.SetBool("Forward", false);
                    //_Animator.SetBool("Right", true);
                    //_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runRight");
                }
                if (-135 < relativeRotation && relativeRotation < -45)
                {
                    //_Animator.SetBool("Forward", false);
                    //_Animator.SetBool("Right", false);
                    //_Animator.SetBool("Left", true);
                    //_Animator.SetBool("Backward", false);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runLeft");
                }
                if (Mathf.Abs(relativeRotation) > 135)
                {
                    //_Animator.SetBool("Forward", false);
                    ///_Animator.SetBool("Right", false);
                    ///_Animator.SetBool("Left", false);
                    //_Animator.SetBool("Backward", true);
                    //_Animator.SetBool("Idle", false);
                    _Animator_mixamo.SetTrigger("runBack");
                }
            }
            if (Managers.input.GetInput("horizontal_movement") == 0 && Managers.input.GetInput("vertical_movement") == 0)
            {
                //_Animator.SetBool("Forward", false);
                //_Animator.SetBool("Right", false);
                //_Animator.SetBool("Left", false);
                //_Animator.SetBool("Backward", false);
                //_Animator.SetBool("Idle", true);
                _Animator_mixamo.SetTrigger("idle");
            }
            testvariable = aimRotation;
        }
	}
	
	// Update is called once per frame
	void Update () {
    
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (Application.isEditor == false)
        {
            horInput = Managers.input.GetInput("horizontal_movement");
            vertInput = Managers.input.GetInput("vertical_movement");
        }

        if (horInput!=0 || vertInput != 0)
        {
            Vector3 forwardDirection = transform.position-target.position;
            forwardDirection.y = 0;
            forwardDirection /= Vector3.Magnitude(forwardDirection);
            Vector3 sideDirection = Vector3.Cross(forwardDirection,Vector3.down);

            movement += horInput * moveSpeed*Managers.gun.speed* sideDirection;
            movement += vertInput * moveSpeed * Managers.gun.speed * forwardDirection;
            movement = Vector3.ClampMagnitude(movement, moveSpeed * Managers.gun.speed);
            Debug.Log("Managers.gun.speed   "+Managers.gun.speed);
        }

        movement *= Time.deltaTime;
        if (Physics.Raycast(transform.position, Vector3.down, 0.3f) && yvel<0)
        {
            yvel = 0f;
            if (Input.GetButtonDown("Jump") && canJump==true) { yvel = jumpHeight; }
        }
        movement += Vector3.up*yvel;
        yvel -= 1f * Time.deltaTime;
        _charController.Move(movement);
        if (gunTrans.gameObject.activeSelf == true)
        {
            SetAnimationStates();
        }
    }

}
