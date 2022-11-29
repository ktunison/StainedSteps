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
    private float jumpForce = 2;
    public float maxFuel = .75f;
    public float jetpackForce = .5f;
    [SerializeField]
    private float currentFuel = .75f;
    [SerializeField]
    private bool hasJetpack;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        normalSpeed = playerSpeed;
        sprintSpeed = playerSpeed * 2;
        currentFuel = maxFuel;
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = sprintSpeed;
        }
        else
        {
            playerSpeed = normalSpeed;
        }

        if (Input.GetKey("space") && hasJetpack && currentFuel > 0f)
        {
            currentFuel -= Time.deltaTime;
            playerVelocity.y += Mathf.Sqrt(.005f * -3.0f * gravityValue);
        }

        if (groundedPlayer && currentFuel < maxFuel)
        {
            currentFuel += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jetpack")
        {
            other.gameObject.SetActive(false);
            hasJetpack = true;
        }
    }

}
