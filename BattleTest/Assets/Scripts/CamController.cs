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

    public void FollowCharacter(GameObject chara, bool state)
    {
        if (state)
        {
            charToFollow = chara;
            canBeMoved = false;
            isFollowingChar = true;
        }
        else
        {
            isFollowingChar = false;
            canBeMoved = true;
        }

    }

    public IEnumerator SmoothFocus(GameObject chara)
    {
        Vector3 deltaVec = new Vector3(cam.transform.position.x, cam.transform.position.y, 0) - new Vector3(chara.transform.position.x, chara.transform.position.y, 0);
        Vector3 direction = -deltaVec;
        while (deltaVec.magnitude > 0.5f)
        {
            cam.transform.Translate(direction * 0.1f);
            deltaVec = new Vector3(cam.transform.position.x, cam.transform.position.y, 0) - new Vector3(chara.transform.position.x, chara.transform.position.y, 0);
            yield return null;
        }
    }
}
