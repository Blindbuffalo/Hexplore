using UnityEngine;
using System.Collections;

public class testRot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.Rotate(Vector3.forward, 55);
    }
	
	// Update is called once per frame
	void Update () {
        float speed = 1;
        Vector3 vectorToTarget = new Vector3(1,1,0) - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle * speed / Time.deltaTime, Vector3.forward);
        //transform.rotation = q;
        
        this.transform.Rotate(Vector3.forward, 90);
    }
}
