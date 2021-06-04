using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummableMushroom : MonoBehaviour
{
    private Rigidbody2D consummableMushroom;
    
    private Vector2 currentPosition;
    private Vector2 currentDirection; 
    private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        consummableMushroom = GetComponent<Rigidbody2D>();
        consummableMushroom.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

        currentDirection = new Vector2(1.0f, 0.0f); //how to make random?
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 nextPosition = consummableMushroom.position + speed * currentDirection.normalized * Time.fixedDeltaTime;
        consummableMushroom.MovePosition(nextPosition);
       
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Pipe"))
        {
            currentDirection *= -1.0f; //change direction
        }

        if (col.gameObject.CompareTag("Player"))
        {
            currentDirection *= 0.0f; //stop
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
