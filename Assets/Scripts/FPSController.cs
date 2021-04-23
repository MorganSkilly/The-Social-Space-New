using QuickStart;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Mirror;

public class FPSController : NetworkBehaviour
{
    [SerializeField]
    public float walkSpeed = 3.0f;

    [SerializeField]
    public GameObject characterModel;

    [SerializeField]
    public float runSpeed = 6.0f;

    [SerializeField]
    public float smoothMoveTime = 0.1f; //smoothing

    [SerializeField]
    public float jumpHeight = 8.0f;

    [SerializeField]
    public float gravity = 18.0f;

    [SerializeField]
    public bool lockCursor;

    [SerializeField]
    public float mouseSensitivity = 0.2f;

    [SerializeField]
    public Vector2 pitchMinMax = new Vector2(-40.0f, 85.0f);

    [SerializeField]
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;

    [SerializeField]
    public float yaw;

    [SerializeField]
    public float pitch;

    private float smoothYaw;
    private float smoothPitch;

    private float yawSmoothV;
    private float pitchSmoothV;
    private float verticalVelocity;

    private Vector3 velocity;
    private Vector3 smoothV;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    private bool jumping;
    private float lastGrounedTime;

    private SceneScript sceneScript;

    void Awake()
    {
        //allow all players to run this
        sceneScript = GameObject.FindObjectOfType<SceneScript>();
    }

    [Command]
    public void CmdSendPlayerMessage()
    {
        if (sceneScript)
        {
            sceneScript.statusText = sceneScript.canvasInputText.text;
            sceneScript.cinemaController.Play(sceneScript.statusText);
        }
    }
    public override void OnStartLocalPlayer()
    {
        sceneScript.playerScript = this;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0.3f, 0);

        characterModel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        LockCursor();

        controller = GetComponent<CharacterController>();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x; //local = relative to parent's position
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        Movement();

        MouseInput();

        CheckCursor();
    }


    private void Movement()
    {
        if (lockCursor)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
            Vector3 worldInputDir = transform.TransformDirection(inputDir); //local to world space

            float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

            verticalVelocity -= gravity * Time.deltaTime;
            velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

            var flags = controller.Move(velocity * Time.deltaTime);
            if (flags == CollisionFlags.Below)
            {
                jumping = false;
                lastGrounedTime = Time.time;
                verticalVelocity = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float timerSinceGrounded = Time.time - lastGrounedTime;
                if (controller.isGrounded || (!jumping && timerSinceGrounded < 0.15f))
                {
                    jumping = true;
                    verticalVelocity = jumpHeight;
                }
            }
        }
    }

    private void MouseInput()
    {
        if (lockCursor)
        {
            float mX = Input.GetAxisRaw("Mouse X");
            float mY = Input.GetAxisRaw("Mouse Y");

            //gross hack to stop camera swinging down at start
            /*float mMag = Mathf.Sqrt(mX * mX + mY * mY);
            if (mMag > 5)
            {
                mX = 0;
                mY = 0;
            }*/

            yaw += mX * mouseSensitivity;
            pitch -= mY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

            transform.eulerAngles = Vector3.up * smoothYaw;
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
        }
    }

    private void CheckCursor()
    {
        if(Input.GetKeyDown(KeyCode.U) && lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            lockCursor = false;
        }
        else if (Input.GetKeyDown(KeyCode.U) && !lockCursor)
        {
            lockCursor = true;
            LockCursor();
        }
    }

    private void LockCursor()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
