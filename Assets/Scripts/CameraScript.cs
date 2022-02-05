using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float cameraSpeed = 10f;
    float speedH = 2f;
    float speedV = 2f;
    float yaw = 0f;
    float pitch = 0f;
    //float lookFactor = 10f;
    //bool cameraSprint = false;
    Camera cam;
    Transform cameraTransform;
    //Vector3 cameraPosition;
    //Vector3 worldPos;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
           cameraTransform.position += Vector3.forward * Time.deltaTime * cameraSpeed;           
        }
        if(Input.GetKey(KeyCode.S))
        {
           cameraTransform.position += -Vector3.forward * Time.deltaTime * cameraSpeed;           
        }

        if(Input.GetKey(KeyCode.A))
        {
           cameraTransform.position += -Vector3.right * Time.deltaTime * cameraSpeed;           
        }

        if(Input.GetKey(KeyCode.D))
        {
           cameraTransform.position += Vector3.right * Time.deltaTime * cameraSpeed;           
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            cameraSpeed = 20f;
        }
        else
        {
            cameraSpeed = 10f;
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);     
        }

    }
}
