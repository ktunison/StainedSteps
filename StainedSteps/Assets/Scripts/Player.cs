using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float speed;
    public float normalSpeed;
    public float sprintSpeed;
    private Vector3 inputMovement;

    private int lives = 3;
    public Text livesText;
    private float CoinCount = 0;
    public Text coinsText;
    public Text gameOverText;
    public Text winText;

    public int fallDepth;
    private Vector3 spawnPos;

    public float jumpForce;
    [SerializeField]
    private bool isGrounded;
    private float distToGround = 1.1f;
    private Rigidbody rigidBody;

    public float maxFuel = 2.5f;
    public float jetpackForce = .5f;
    [SerializeField]
    private float currentFuel;
    [SerializeField]
    private bool hasJetpack;
    [SerializeField]
    private int keyCount;

    //We might want a ground zero level where there are no puzzles so before you enter the tower the levels can be randomized
    //That way I can grab the first build number then randomize and delete the rest as we go to properly track progress

    // Start is called before the first frame update
    void Start()
    {
        keyCount = 0;
        speed = normalSpeed;
        spawnPos = transform.position;
        setCountText();
        winText.text = "";
        gameOverText.text = "";
        rigidBody = GetComponent<Rigidbody>();
        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        //Debug.Log(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), distToGround));

        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distToGround))
        {
            if (!(hit.collider.tag == "Player"))
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

        //Jumping using space key
        if (Input.GetKey("space") && isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce);
        }
        else if (Input.GetKey("space") && hasJetpack && currentFuel > 0f)
        {
            currentFuel -= Time.deltaTime;
            rigidBody.AddForce(rigidBody.transform.up * jetpackForce, ForceMode.Impulse);
        }

        if (isGrounded && currentFuel < maxFuel)
        {
            currentFuel += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
    }

    //actually moves the player from the WASD function and will respawn the player if they fall too far
    private void Move()
    {
        transform.Translate(inputMovement * speed * Time.deltaTime);

        if (transform.position.y < fallDepth)
        {
            Respawn();
        }
    }

    //uses the input action to figure out what key is being pressed and then adds a value to the player based off of that to move the player around in the game.
    public void WASD(InputAction.CallbackContext context)
    {
        Vector2 myVector = context.ReadValue<Vector2>();
        inputMovement = new Vector3(myVector.x, 0, myVector.y);
    }

    //Will bring the player back to the spawn position that was instantiated at the beginning of the level
    private void Respawn()
    {
        transform.position = spawnPos;
        lives--;
        setCountText();

        if (lives <= 0)
        {
            this.enabled = false;
        }
        StartCoroutine(Blink());
    }

    //turns on and off the meshRenderer to give the feeling that you have started anew (blinks half the amount of times of the < # for the index (i) integer)
    public IEnumerator Blink()
    {
        for (int i = 0; i < 24; i++)
        {
            if (i % 2 == 0)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(.1f);
        }

        GetComponent<MeshRenderer>().enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            CoinCount++;
            setCountText();
        }

        if (other.tag == "Exit")
        {
            SceneSwitch.instance.switchScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.tag == "Jetpack")
        {
            other.gameObject.SetActive(false);
            hasJetpack = true;
        }

        if (other.tag == "Key")
        {
            other.gameObject.SetActive(false);
            keyCount++;
        }

        if (other.tag == "Door" && keyCount > 0)
        {
            other.gameObject.SetActive(false);
            keyCount--;
        }
    }

    public void setCountText()
    {
        coinsText.text = "Coins: " + CoinCount.ToString();
        livesText.text = "lives: " + lives.ToString();

        if (lives <= 0)
        {
            gameOverText.text = "GAME OVER";
        }
    }
}
