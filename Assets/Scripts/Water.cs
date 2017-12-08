using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
	public Transform[] waterPlane;
	public float speed;
	public float threshold;
	void Update() {
		for (int i = 0; i < waterPlane.Length; i++) {
			Vector3 newPos = waterPlane [i].localPosition + (Vector3.right * speed);

			if (newPos.x >= threshold) {
				newPos = new Vector3(newPos.x-106.8f,newPos.y,newPos.z);
			}

			waterPlane [i].localPosition = newPos;

		}
	}

}
