using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private const float turnSpeed = 45.0f;
    [SerializeField] float speed;
    [SerializeField] float rpm;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody playerRb;
    [SerializeField] private float horsePower = 0;
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] TextMeshProUGUI gearText;
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;
    private int currentGear = 1;
    private const float maxRpmPerGear = 6000f;
    private const float minRpmPerGear = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        UpdateGearText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Applying forward horsepower force
        playerRb.AddRelativeForce(Vector3.forward * horsePower * verticalInput);

        //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // Calculating speed into mph change 2.237 into 3.6 to get kph
        speed = Mathf.RoundToInt(playerRb.velocity.magnitude * 2.237f);
        speedometerText.SetText("Speed: " + speed + "mph");

        rpmText.SetText("RPM: " + Mathf.RoundToInt(rpm));

        // Change colour to red if speed exceeds 100 mph
        if (speed > 100)
        {
            speedometerText.color = Color.red;
        }
        else
        {
            speedometerText.color = Color.black; // Reset to default colour
        }

        // Change colour to red if speed exceeds 100 mph
        if (rpm > 5000)
        {
            rpmText.color = Color.red;
        }
        else
        {
            rpmText.color = Color.black; // Reset to default colour
        }

        // Check if RPM exceeds 6000 and allow gear shift
        if (rpm >= 6000 && Input.GetKeyDown(KeyCode.F))
        {
            ShiftUpGear();
        }
    }

    void CalculateRpm(float verticalInput)
    {
        // Simulate RPM increase with throttle input
        if (verticalInput > 0)
        {
            rpm += verticalInput * 1000f * Time.deltaTime;
        }
        else
        {
            rpm -= 500f * Time.deltaTime; // Decrease RPM gradually when not accelerating
        }

        rpm = Mathf.Clamp(rpm, minRpmPerGear, maxRpmPerGear); // Clamp RPM between min and max values
    }

    void ShiftUpGear()
    {
        currentGear++;
        rpm = 2000;
        UpdateGearText(); // Update gear display
    }
    void UpdateGearText()
    {
        gearText.SetText("Gear: " + currentGear);
    }


    bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }

        if (wheelsOnGround == 4)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
