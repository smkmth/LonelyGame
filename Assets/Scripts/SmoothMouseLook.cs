using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class SmoothMouseLook : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    public float minimumY;
    public float maximumY;
    Quaternion quatRot;
    private float yRotation;
    public float smoothing;
    float xRotation;
    public bool cameraControl =true;

    public void LateUpdate()
    {
        if (cameraControl)
        {
            float HorizontalAxis = Input.GetAxis("Mouse X");
            float VerticalAxis = Input.GetAxis("Mouse Y");

            //float xRotation = transform.localEulerAngles.y + HorizontalAxis * sensitivityX;
            xRotation += HorizontalAxis * sensitivityX * Time.deltaTime;

            yRotation += VerticalAxis * sensitivityY * Time.deltaTime;
            yRotation = Mathf.Clamp(yRotation, minimumY, maximumY);

            quatRot = Quaternion.Euler(new Vector3(-yRotation, xRotation, 0));
            if (transform.rotation != quatRot)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, quatRot, Time.deltaTime * smoothing);
            }
        }
    }

}