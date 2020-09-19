using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]

public class ShipController : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 45f;
    public Transform cameraPosition;
    public Transform spaceshipRoot;
    public float rotationSpeed = 1.5f;
    public float cameraSmooth = 2f;
    public RectTransform crosshairTexture;
    public ParticleSystem afterburner;

    public bool useMouse = false;


    float speed;
    Rigidbody r;
    Quaternion lookRotation;
    float rotationZ = 0;
    float mouseXSmooth = 0;
    float mouseYSmooth = 0;
    Vector3 defaultShipRotation;

    // AutoPilot Settings
    private bool onAutoPilot = false;
    public GameObject autoPilotTarget;
    private float autoLookTime = 3f;
    private float currentAutoLookTime;
    public AnimationCurve autoLookCurve = AnimationCurve.EaseInOut(0f, 0f, 3f, 3f);

    // Thrustertime
    float thrusterTime = 0f;
    ShipResources shipResources;

    private Camera mainCamera;
    private FixedJoystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        shipResources = GetComponent<ShipResources>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        lookRotation = transform.rotation;
        defaultShipRotation = spaceshipRoot.localEulerAngles;
        rotationZ = defaultShipRotation.z;

        mainCamera.transform.position = cameraPosition.position;
        mainCamera.transform.rotation = cameraPosition.rotation;

        if (useMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private float AutoPilotDistanceSpeedModifier()
    {
        Vector3 colliderEdge = autoPilotTarget.GetComponent<SphereCollider>().ClosestPoint(transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, colliderEdge);
        float stoppingPoint = 100f;

        // Start slowing down within 300 units
        float modifier;
        if (distanceToTarget < 1000f)
        {
            modifier = (distanceToTarget - stoppingPoint) / stoppingPoint;
        }
        else
        {
            modifier = 1f;
        }

        Debug.Log("Distance " + distanceToTarget + ", Modifier " + modifier);

        return modifier;
    }

    void FixedUpdate()
    {
        // Ship spins wildly if mouse is moving on start
        (float horizontalInput, float verticalInput) = GetInput();

        if (horizontalInput != 0 || verticalInput != 0 && onAutoPilot)
        {
            DisableAutoPilot();
        }

        //Press Right Mouse Button to accelerate
        bool isAccelerating = useMouse && Input.GetMouseButton(1) || verticalInput > 0.2f || onAutoPilot;

        float accelerationModifier = onAutoPilot ? AutoPilotDistanceSpeedModifier() : verticalInput;
        speed = GetSpeed(isAccelerating, accelerationModifier);

        //Set moveDirection to the vertical axis (up and down keys) * speed
        Vector3 moveDirection = new Vector3(0, 0, speed);

        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        // moveDirection.y = 0;
        //Set the velocity, so you can move
        r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        //Camera follow
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, 1f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPosition.rotation, 1f);

        //Rotation
        if (!onAutoPilot)
        {
            float rotationZTmp = 0;

            // Look movement
            mouseXSmooth = Mathf.Lerp(mouseXSmooth, horizontalInput * rotationSpeed, Time.deltaTime * cameraSmooth);
            mouseYSmooth = Mathf.Lerp(mouseYSmooth, verticalInput * rotationSpeed, Time.deltaTime * cameraSmooth);

            Quaternion localRotation;
            if (!isAccelerating)
            {
                localRotation = Quaternion.Euler(-mouseYSmooth, mouseXSmooth, rotationZTmp * rotationSpeed);

            }
            else
            {
                localRotation = Quaternion.Euler(0, mouseXSmooth, rotationZTmp * rotationSpeed);

            }

            lookRotation = lookRotation * localRotation;
            transform.rotation = lookRotation;
        }
        else // Autopilot look
        {
            currentAutoLookTime += Time.deltaTime;
            float percentage = currentAutoLookTime / autoLookTime;
            Vector3 lookTarget = autoPilotTarget.transform.position - transform.position;
            // Vector3.Lerp(transform.rotation, lookTarget, autoLookCurve.Evaluate(percentage));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget), autoLookCurve.Evaluate(percentage));
        }



        /** Unused A/D rotation **/
        // rotationZ -= mouseXSmooth;
        // rotationZ = Mathf.Clamp(rotationZ, -45, 45);
        // spaceshipRoot.transform.localEulerAngles = new Vector3(defaultShipRotation.x, defaultShipRotation.y, rotationZ);
        // rotationZ = Mathf.Lerp(rotationZ, defaultShipRotation.z, Time.deltaTime * cameraSmooth);

        //Update crosshair texture
        if (crosshairTexture)
        {
            crosshairTexture.position = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 105);
        }
    }

    private (float, float) GetInput()
    {
        float x, y;
        if (useMouse)
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
        }
        else
        {
            x = joystick.Horizontal;
            y = joystick.Vertical;
        }

        return (x, y);
    }

    public void EnableAutoPilot(GameObject target)
    {
        currentAutoLookTime = 0f;
        onAutoPilot = true;
        autoPilotTarget = target;
    }

    public void DisableAutoPilot()
    {
        onAutoPilot = false;
        autoPilotTarget = null;
    }

    public void Stop()
    {
        DisableAutoPilot();
    }

    // Smooth speed
    private float GetSpeed(bool accelerating, float modifier = 1f)
    {
        if (accelerating)
        {
            afterburner.Play(true);
            thrusterTime += Time.deltaTime * 3;
            if (thrusterTime > 0.01f)
            {
                shipResources.UpdateFuel(thrusterTime);
                thrusterTime = 0;
            }

            return Mathf.Lerp(speed, accelerationSpeed * modifier, Time.deltaTime * 3);
        }
        else
        {
            afterburner.Stop(true);
            thrusterTime = 0f;
            return Mathf.Lerp(speed, normalSpeed, Time.deltaTime * 4);
        }
    }
}