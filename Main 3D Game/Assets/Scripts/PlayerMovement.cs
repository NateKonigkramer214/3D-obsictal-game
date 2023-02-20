using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float jumpForce = 10.0f;
    private GameObject PlatformFall;
    private string Fall;
    public float bounce = 5.0f;
    private float timePlayed;

    public LayerMask groundLayer;
    public LayerMask End;
    private bool canJump = true;
    public GameObject EndScreen;

    public float timeLimit = 20f; // Time limit in seconds
    public string Complete; // Name of the scene to load when the player wins
    private bool hasWon = false; // Flag to track if the player has already won
    public Text timeText;
    private float timer = 0f;
    public Rigidbody rigidbodyToSetStatic;
    public AudioSource main_music;

    void Start()
    {
        Elements();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        EndScreen.SetActive(false);
        main_music.Play();
    }

    public float speed = 10.0f;

    void Update()
    {
        
        timePlayed += Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        //if (Input.GetButtonDown("Jump"))
        //{
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

        // Check if the player is on the ground
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // Allow the player to jump if they are on the ground and can currently jump
        if (Input.GetButtonDown("Jump") && canJump /*&& isGrounded*/)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            canJump = false;
            Invoke("Jump", 1f);
        }

        timer += Time.deltaTime;

        // Format the time as minutes and seconds
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");

        // Update the text component with the formatted time
        timeText.text = string.Format("{0}:{1}", minutes, seconds);

    }

    private void Elements()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fall"))
        {
            //Invoke("restart", 3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Check if the player collided with the object
        else if (collision.gameObject.CompareTag("Bounce"))
        {
            // Calculate the direction of the bounce
            Vector3 bounceDirection = collision.contacts[0].normal;
            // Apply the bounce force to the player's Rigidbody
            GetComponent<Rigidbody>().AddForce(bounceDirection * bounce, ForceMode.Impulse);
        }
        // Reset the ability to jump if the player lands on the ground
        if (collision.gameObject.layer == groundLayer)
        {
            canJump = true;
        }

        //if (collision.gameObject.CompareTag("end"))
        //    { 
        //    float timeElapsed = Time.timeSinceLevelLoad; // Get the time elapsed since the level started
        //    if (timeElapsed <= timeLimit) // Check if the time elapsed is less than or equal to the time limit
        //    {
        //        SceneManager.LoadScene(Complete); // Load the win scene
        //    }
        //}

    }


    private void Jump()
    {
        canJump = true;
    }

    void OnTriggerEnter(Collider other)
    {
        float timeElapsed = Time.timeSinceLevelLoad;
        if (other.gameObject.CompareTag("end") && timePlayed <= 20f)
        {

            //SceneManager.LoadScene("Complete");
            EndScreen.SetActive(true);
            // Set the rigidbody to be static
            //rigidbodyToSetStatic.isKinematic = true;
            //rigidbodyToSetStatic.useGravity = false;
        }
    }
}
