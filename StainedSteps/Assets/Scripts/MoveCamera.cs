using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float visX;
    public float visY;

    public Transform orientation;

    private float rotationX;
    private float rotationY;

    private float lookBorder = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * visX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * visY;

        rotationY += mouseX;
        rotationX -= mouseY;

        //Makes sure you cannot look behind yourself by looking up continuously and vise versa
        rotationX = Mathf.Clamp(rotationX, -lookBorder, lookBorder);

        //This rotates the Camera and the Orientation of the player
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationX, 0);
    }
}
