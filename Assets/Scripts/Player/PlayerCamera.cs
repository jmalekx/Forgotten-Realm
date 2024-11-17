using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public Transform playerBody; //player

    float xRotation; //camera
    float yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //locking cursor
        Cursor.visible = false;
        sensitivityX = MainMenu.ChosenSensitivity;
        sensitivityY = MainMenu.ChosenSensitivity;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensitivityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //ensuring cant look more than 180 up/down

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
