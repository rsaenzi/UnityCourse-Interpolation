using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileControl : MonoBehaviour {

    void Update() {

        // Moves the missiles to forward
        float speedFactor = Time.deltaTime * 20.0f;
        transform.Translate(Vector3.forward * speedFactor);

        // Destroy the missile after 10 seconds
        Invoke("DestroyMissile", 10);
    }

    void DestroyMissile() {
        Destroy(this.gameObject);
    }
}
