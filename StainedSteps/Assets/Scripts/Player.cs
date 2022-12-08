using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //private Vector3 inputMovement;

    private int lives = 3;
    public Text livesText;
    private float CoinCount = 0;
    public Text coinsText;
    public Text keyText;
    public Text gameOverText;
    public Text winText;
    public Text requiredKeyText;

    public int fallDepth;
    private Vector3 spawnPos;


    [SerializeField]
    private bool isGrounded;
    //private float distToGround = 1.1f;

    [SerializeField]
    private int keyCount;

    private PlayerController pCont;


    // Start is called before the first frame update
    void Start()
    {
        keyCount = 0;
        spawnPos = transform.position;
        setCountText();
        winText.text = "";
        keyText.text = "";
        gameOverText.text = "";
        requiredKeyText.text = "";
        pCont = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move();

        /*
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
        */


        if (transform.position.y < fallDepth)
        {
            Respawn();
        }
    }

    //Will bring the player back to the spawn position that was instantiated at the beginning of the level
    private void Respawn()
    {
        GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = spawnPos;
        GetComponent<CharacterController>().enabled = true;
        lives--;
        setCountText();

        if (lives <= 0)
        {
            this.enabled = false;
            pCont.enabled = false;
        }
        //StartCoroutine(Blink());
    }

    public void ManualRespawn()
    {
        GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = spawnPos;
        GetComponent<CharacterController>().enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            CoinCount++;
            setCountText();

            if (CoinCount >= 30)
            {
                CoinCount -= 40;
                lives++;
            }
        }

        if (other.tag == "Exit")
        {
            SceneSwitch.instance.switchScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.tag == "Key")
        {
            other.gameObject.SetActive(false);
            keyCount++;
            keyText.text = "Keys: " + keyCount.ToString();
        }

        if (other.tag == "Door")
        {
            if (other.gameObject.GetComponent<Door>().requiredKeys <= keyCount)
            {
                other.gameObject.SetActive(false);
                keyCount -= other.gameObject.GetComponent<Door>().requiredKeys;
                keyText.text = "Keys: " + keyCount.ToString();
            }
            else
            {
                int required = other.gameObject.GetComponent<Door>().requiredKeys - keyCount;
                StartCoroutine(requiredKeyTimer(required));
            }
        }

        if (other.tag == "Spike")
        {
            Respawn();
        }

        if (other.tag == "CheckPoint")
        {
            spawnPos = other.transform.position;
            other.gameObject.SetActive(false);
            //Respawn();
        }

        if (other.tag == "MovObj")
        {
            Respawn();
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

    public IEnumerator requiredKeyTimer(int required)
    {
        Debug.Log("hey");
        requiredKeyText.text = "Keys needed: " + required.ToString();
        yield return new WaitForSeconds(2f);
        requiredKeyText.text = "";
    }

    /*
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
*/

    /*
        //actually moves the player from the WASD function and will respawn the player if they fall too far
        private void Move()
        {
            transform.Translate(inputMovement * speed * Time.deltaTime);
        }

        //uses the input action to figure out what key is being pressed and then adds a value to the player based off of that to move the player around in the game.
        public void WASD(InputAction.CallbackContext context)
        {
            Vector2 myVector = context.ReadValue<Vector2>();
            inputMovement = new Vector3(myVector.x, 0, myVector.y);
        }
    */
}