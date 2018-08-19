using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{

    public float mapScrollSpeed;
    public float mapZoomSensitivity;
    public float zoomClampMin, zoomClampMax;
    public bool canBeMoved;

    private Camera cam;
    private GameObject charToFollow;
    private bool isFollowingChar;

	// Use this for initialization
	void Start ()
	{
	    cam = this.GetComponent<Camera>();
        canBeMoved = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    if (canBeMoved)
	    {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                float speed = mapScrollSpeed * Time.deltaTime;
                cam.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
            }
        }
        else
        {
            if (isFollowingChar) cam.transform.position = new Vector3(charToFollow.transform.position.x, charToFollow.transform.position.y, cam.transform.position.z);
        }

        //cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * mapZoomSensitivity;
	    cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * mapZoomSensitivity, zoomClampMin, zoomClampMax);
	}

    public void FollowCharacter(GameObject chara)
    {
        charToFollow = chara;
        isFollowingChar = true;
    }
}
