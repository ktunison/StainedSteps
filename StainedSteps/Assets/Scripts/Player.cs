using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 inputMovement;

    [SerializeField]
    private bool isGrounded;

    private int lives = 3;
    public int fallDepth;
    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
}
