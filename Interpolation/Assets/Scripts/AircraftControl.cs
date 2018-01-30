using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftControl : MonoBehaviour {

    float motionSpeed = 20.0f;
    float rotationSpeed = 0.08f;
    float stabilizationSpeed = 0.04f;
    bool arrowKeyWasPressed = false;

    // Launch pivots
    Transform launchedLeft;
    Transform launchedRight;
    bool missileLaunchedFromRight = false;

    void Awake() {
        launchedLeft = transform.Find("LauncherLeft");
        launchedRight = transform.Find("LauncherRight");
    }

    void Update() {

        // This flag will be used to detect if we should reset the rotation of the aircraft
        arrowKeyWasPressed = false;

        // This prevents translation to have different speed on different CPUs
        float speedFactor = Time.deltaTime * motionSpeed;

        // If right arrow key was pressed
        if(Input.GetKey(KeyCode.RightArrow)) {

            // Moves the aircraft to the right
            transform.Translate(Vector3.right * speedFactor, Space.World);

            // Interpolates the current rotation to a specific rotation on Z axis
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -30), rotationSpeed);

            // Rise the flag
            arrowKeyWasPressed = true;
        }

        // Same algorithm for other axis...
        if(Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(Vector3.left * speedFactor, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 30), rotationSpeed);
            arrowKeyWasPressed = true;
        }
        if(Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(Vector3.up * speedFactor, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-50, 0, 0), rotationSpeed);
            arrowKeyWasPressed = true;
        }
        if(Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(Vector3.down * speedFactor, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(50, 0, 0), rotationSpeed);
            arrowKeyWasPressed = true;
        }

        // If no key was pressed, the aircraft should reset it position automatically
        if(arrowKeyWasPressed == false) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), stabilizationSpeed);
        }

        // If fire key was pressed
        if(Input.GetKeyDown(KeyCode.Space)) {

            // Instantiate a new missile
            GameObject newMissile = Instantiate(Resources.Load("Prefabs/Missile")) as GameObject;
            newMissile.name = "Missile";

            // Makes the missile a child of a launcher
            if(missileLaunchedFromRight == true) {
                newMissile.transform.SetParent(launchedLeft);
            } else {
                newMissile.transform.SetParent(launchedRight);
            }
            missileLaunchedFromRight = !missileLaunchedFromRight;

            // Reset its transform, doing it will be positioned under the wing
            newMissile.transform.localPosition = Vector3.zero;
            newMissile.transform.localScale = Vector3.one;
            newMissile.transform.localRotation = Quaternion.identity;

            // If we do not reset the parent, the missile will inherit all rotation and 
            // position changes of the aircraft
            newMissile.transform.parent = null;
        }
    }
}