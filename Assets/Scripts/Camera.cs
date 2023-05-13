using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] int sensHor, sensVer, lockVerMin, lockVerMax;
    [SerializeField] bool invertY;
    float xRotation, mouseX, mouseY;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (!gameManager.instance.shopMenu.activeSelf)
        {
            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;
            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);
            transform.localRotation = Quaternion.Euler(-xRotation, 0, 0);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    } 
}
