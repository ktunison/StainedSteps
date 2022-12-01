using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;
    private Transform cameraTransform;
    private float normalSpeed;
    private float sprintSpeed;

    private Rigidbody rigidBody;
    //private float jumpForce = 2;
    [SerializeField]
    private float maxJPFuel = .75f;
    public float jetpackForce = .5f;
    [SerializeField]
    private float currentJPFuel = .75f;
    [SerializeField]
    private bool hasJetpack;

    [SerializeField]
    private float maxRunFuel = 2f;
    [SerializeField]
    private float currentRunFuel = 2f;
    [SerializeField]
    private bool hasRun;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        normalSpeed = playerSpeed;
        sprintSpeed = playerSpeed * 2;
        currentJPFuel = maxJPFuel;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && hasRun && currentRunFuel > 0f)
        {
            currentRunFuel -= Time.deltaTime;
            playerSpeed = sprintSpeed;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = normalSpeed;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && currentRunFuel < maxRunFuel)
        {
            currentRunFuel += Time.deltaTime;
        }

        if (Input.GetKey("space") && hasJetpack && currentJPFuel > 0f)
        {
            currentJPFuel -= Time.deltaTime;
            playerVelocity.y += Mathf.Sqrt(.005f * -3.0f * gravityValue);
        }

        if (groundedPlayer && currentJPFuel < maxJPFuel)
        {
            currentJPFuel += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jetpack")
        {
            other.gameObject.SetActive(false);
            hasJetpack = true;
        }
        
        if (other.tag == "Run")
        {
            other.gameObject.SetActive(false);
            hasRun = true;
        }

        if (other.tag == "Coin")
        {
            if (currentJPFuel < maxJPFuel)
            {
                currentJPFuel += .05f;
            }
            if (currentRunFuel < maxRunFuel)
            {
                currentRunFuel += .05f;
            }
        }
    }

}
