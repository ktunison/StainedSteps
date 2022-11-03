using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 10f;
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

    private int currentLevel;
    private int nextLevel;

    private float playerYValue;

    //make a list with ints instead
    private List<int> levels;

    //We might want a ground zero level where there are no puzzles so before you enter the tower the levels can be randomized
    //That way I can grab the first build number then randomize and delete the rest as we go to properly track progress

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        setCountText();
        winText.text = "";
        gameOverText.text = "";
        rigidBody = GetComponent<Rigidbody>();
        playerYValue = this.transform.position.y;
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
       
        /*
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

        }
        */

        //Jumping using space key
        if (Input.GetKey("space") && isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce);
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            isGrounded = true;
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    */

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

    private void randomNextLevel()
    {

    }
}
