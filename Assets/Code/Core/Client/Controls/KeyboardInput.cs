using UnityEngine;
using System.Collections;

public class KeyboardInput : Monosingleton<KeyboardInput> {

	void FixedUpdate () {
        bool rotateLeft = Input.GetKey(KeyCode.A);
        bool rotateRight = Input.GetKey(KeyCode.S);

        if(rotateLeft)
            CameraController.Instance.rotation += .05f;

        if(rotateRight)
            CameraController.Instance.rotation -= .05f;
	}
}
