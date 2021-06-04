using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D marioBody;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public Transform enemyLocation;
    public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;

    public float maxSpeed = 10;
    public float upSpeed;

    private bool onGroundState = true;

    public Button restartButton; //Lab 1 Checkoff

    private Animator marioAnimator; //Lab 2: How do to transition between running and jumping, sound getting played overlap
    private AudioSource marioAudio;


// Start is called before the first frame update
    void  Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();

        //Flip mario
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        restartButton.gameObject.SetActive(false); //restart button doesn't show in start screen
    }

    
  // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        
        // dynamic rigidbody
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        

        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            countScoreState = true; //check if Gomba is underneath
        }
    }

    void Update()
    {
        //flip Mario
              // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
            if (Mathf.Abs(marioBody.velocity.x) > 1.0) //enable skidding
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) > 1.0) //enable skidding
                marioAnimator.SetTrigger("onSkid");
        }

        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }

        //always update the xSpeed parameter to match Mario’s current speed along the x-axis.
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);

    }

    

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        //if (col.gameObject.CompareTag("Ground")) onGroundState = true;

        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles"))
        {
            onGroundState = true; // back on ground
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        };
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            Time.timeScale = 0.0f; //Freeze game, game over
            restartButton.gameObject.SetActive(true); //make restart button visible

        }
    }

    void PlayJumpSound()
    {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    
}
