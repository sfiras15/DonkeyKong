using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite climbAnimation;
    public Sprite[] walkingAnimation;
    public int spriteIndex;
    private Rigidbody2D rigidB;
    private Vector2 direction;
    public float mouveSpeed = 1f;
    public float jumpStrength = 10f;
    private bool isGrounded;
    private bool climbing = false;
    public new Collider2D collider;
    // The array will be filled with the objects that the player character has collided with.
    private Collider2D[] results;
    private int x ;

    
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable(){
        InvokeRepeating(nameof(Anim),1f/12f,1f/12f);
    }
    // Animating mario through all of his states
    private void Anim(){
        if (climbing){
            spriteRenderer.sprite = climbAnimation;
        }
        else if (direction.x != 0){
            spriteIndex++;
            if (spriteIndex >= walkingAnimation.Length){
                spriteIndex = 0 ;
            }
            spriteRenderer.sprite = walkingAnimation[spriteIndex];
        }
        else {
            spriteRenderer.sprite = walkingAnimation[0];
        }
    }
    
    private void OnDisable(){
        CancelInvoke();
    }
    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        direction.x = Input.GetAxis("Horizontal") * mouveSpeed;
        if (direction.x >0f){
            this.transform.eulerAngles = new Vector3(0,0,0);
        }
        else if (direction.x < 0f) {
            this.transform.eulerAngles = new Vector3(0,180,0);
        }
        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * mouveSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded )
        {
            direction = Vector2.up * jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }
        if (isGrounded){
            direction.y = Mathf.Max(direction.y,-1f);
        }
    }

    private void CheckCollision()
    {
        isGrounded = false;
        climbing = false;
        Vector2 size = collider.bounds.size;
        size.y += 0.2f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;
            // If mario collides with a platform
            if (hit.layer == 7)
            {
                // true if the platform is under him // false if it's above him
                isGrounded = hit.transform.position.y < (transform.position.y - 0.5f);
                // ignore the collision if the platform is above him so he doesn't hit his head
                Physics2D.IgnoreCollision(collider, results[i], !isGrounded);
            }
            //// If mario collides with stairs
            else if (hit.layer == 6)
            {
                climbing = true;
            }

        }
    }
    private void FixedUpdate()
    {
        rigidB.MovePosition(rigidB.position + direction * mouveSpeed * Time.fixedDeltaTime);
    }   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            enabled = false; 
            FindObjectOfType<GameManager>().LevelFailed();
        }
        else if (other.gameObject.CompareTag("objective"))
        {
            enabled = false;// the behaviour of the player will be stopped
            FindObjectOfType<GameManager>().LevelCompleted();
        }
    }

}
