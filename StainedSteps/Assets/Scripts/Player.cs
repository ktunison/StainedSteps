using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 inputMovement;
    private Rigidbody rigidbody;
    public float jumpVal;

    [SerializeField]
    private bool isGrounded;

    private int lives = 3;
    private int fallDepth;
    private Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(inputMovement * speed * Time.deltaTime);
    }

    public void WASD(InputAction.CallbackContext context)
    {
        Vector2 myVector = context.ReadValue<Vector2>();
        inputMovement = new Vector3(myVector.x, 0, myVector.y);
    }
}
