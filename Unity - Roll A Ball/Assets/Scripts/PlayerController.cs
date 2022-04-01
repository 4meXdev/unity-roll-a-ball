using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public ParticleSystem deadFx;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public float TimeLeft;
    private bool TimerOn = false;
    public TextMeshProUGUI timerText;
    public GameObject loseTextObject;
    public GameObject restartBtn;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;

                // Kill the player & display lose text
                loseTextObject.SetActive(true);
                restartBtn.SetActive(true);
                Destroy(gameObject);
                Instantiate(deadFx, transform.position, Quaternion.identity);
            }
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 13) {
            winTextObject.SetActive(true);
            TimerOn = false;
        }
     }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) {
            Debug.Log("Player left the ground");
            TimeLeft = 0;
            TimerOn = false;

            // Kill the player & display lose text
            loseTextObject.SetActive(true);
            restartBtn.SetActive(true);
            Destroy(gameObject);
            Instantiate(deadFx, transform.position, Quaternion.identity);
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
