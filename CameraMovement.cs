using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float SensX;
    public float SensY;

    public Transform Orientation;

    float xRot;
    float yRot;
    public bool isPaused;

    [SerializeField] HealthManager healthManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isPaused && !healthManager.Dead)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SensY;

            yRot += mouseX;

            xRot -= mouseY;

            xRot = Mathf.Clamp(xRot, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRot, yRot, 0);
            Orientation.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }
}
