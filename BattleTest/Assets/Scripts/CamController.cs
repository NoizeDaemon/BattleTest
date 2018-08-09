using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{

    public float mapScrollSpeed;
    public float mapZoomSensitivity;
    public float zoomClampMin, zoomClampMax;

    private Camera cam;

	// Use this for initialization
	void Start ()
	{
	    cam = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    if (Input.GetKey(KeyCode.Mouse2))
	    {
	        float speed = mapScrollSpeed * Time.deltaTime;
	        Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
        }

        //cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * mapZoomSensitivity;
	    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * mapZoomSensitivity, zoomClampMin, zoomClampMax);
	}
}
