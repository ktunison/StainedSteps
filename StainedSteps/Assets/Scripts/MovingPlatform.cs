using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject leftPoint;
    public GameObject rightPoint;
    public GameObject forwardPoint;
    public GameObject backPoint;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 backPos;
    private Vector3 forwardPos;
    public int speed;
    public bool goingLeft;
    public bool goingBack;
    public bool goingX;

    // use interpolation for better movement
    //public Vector3 p01;
    //public float easingMod = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //Left position grabs the transform from the leftPoint GameObject
        leftPos = leftPoint.transform.position;
        //Right position grabs the transform from the rightPoint GameObject
        rightPos = rightPoint.transform.position;
        //back position grabs the transform from the backPoint GameObject
        backPos = backPoint.transform.position;
        //Forward position grabs the transform from the forwardPoint GameObject
        forwardPos = forwardPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Move left and right for enemy

    private void Move()
    {
        if (goingX)
        {
            if (goingLeft)
            {
                //If the platform's position is less than or equal to the leftPos then do this
                if (transform.position.x <= leftPos.x)
                {
                    goingLeft = false;
                }
                else
                {
                    transform.position += Vector3.left * Time.deltaTime * speed;
                }
            }
            //If the platform is not goingLeft then do this
            else
            {
                //If the enemy's position is greater than or equal to the rightPos then do this
                if (transform.position.x >= rightPos.x)
                {
                    goingLeft = true;
                }
                else
                {
                    transform.position += Vector3.right * Time.deltaTime * speed;
                }
            }
        }
        else
        {
            if (goingBack)
            {
                //If the platform's position is less than or equal to the backPos then do this
                if (transform.position.z <= backPos.z)
                {
                    goingBack = false;
                }
                else
                {
                    transform.position += Vector3.back * Time.deltaTime * speed;
                }
            }
            //If the platform is not goingBack then do this
            else
            {
                //If the enemy's position is greater than or equal to the forwardPos then do this
                if (transform.position.z >= forwardPos.z)
                {
                    goingBack = true;
                }
                else
                {
                    transform.position += Vector3.forward * Time.deltaTime * speed;
                }
            }
        }
    }

    /*
    private void EasingIn(Transform pos1, Transform pos2, float moveType)
    {
        float u2 = moveType;

        p01 = (1 - u2) * pos1.position + u2 * pos2.position;

        transform.position = p01;
    }
    */
}
