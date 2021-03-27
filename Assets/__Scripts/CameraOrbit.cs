using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    //Control the mouse movement sensitivity
    private float _lookSensitivity = 1f;

    //Max and min height the player can look at
    private float _minXLook = -60f;
    private float _maxXLook = 60f;

    //Reference the camera anchor GO attached to the player
    //This is where the camera is located
    public Transform camAnchor;

    //Allows the user to invert the rotation when moving around
    //Ex) When you move the mouse down, the camera looks up
    public bool invertXRotation;

    //Variable to store the current x rotation
    private float _currentXRotation;

    void Start()
    {
        //Lock the mouse cursor to the center of the game window
        Cursor.lockState = CursorLockMode.Locked;
    }


    //Gets called at the end of the frame
    //Want to rotate camera after all other updates
    void LateUpdate()
    {
        //Variables to store the mouse's movements
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //Reference the player's transform rotation
        //Rotates the player along the y-axis, which is the horizontal axis
        transform.eulerAngles += Vector3.up * x * _lookSensitivity;

        //If the inverted x rotation is selected
        if (invertXRotation)
        {
            //Store the current x rotation by how far the player moved the mouse times the sensitivity (in opposite direction)
            _currentXRotation += y * _lookSensitivity;
        }
        else
        {
            //Store the current x rotation by how far the player moved the mouse times the sensitivity 
            _currentXRotation -= y * _lookSensitivity;
        }

        //The current x rotation can't go below the minXLook value or above the maxXLook value
        _currentXRotation = Mathf.Clamp(_currentXRotation, _minXLook, _maxXLook);

        //Store the camera's anchor's rotation
        Vector3 clampedAngle = camAnchor.eulerAngles;

        //Set the x rotation
        clampedAngle.x = _currentXRotation;

        //Set the camera anchor rotation
        camAnchor.eulerAngles = clampedAngle;
    }
}
