using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 10;
    [SerializeField]
    private Rigidbody2D myRB;
    public bool isControllable = false;
    private int myLastInternalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isControllable) { return; }
        myLastInternalSpeed = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            myLastInternalSpeed--;
        }
        if (Input.GetKey(KeyCode.D ) || Input.GetKey(KeyCode.RightArrow))
        {
            myLastInternalSpeed++;
        }
        myLastInternalSpeed *= (int)MovementSpeed;
    }
    private void FixedUpdate()
    {
        Vector2 lastVelocity = myRB.velocity;
        lastVelocity.x = myLastInternalSpeed * Time.fixedDeltaTime;
        myRB.velocity = lastVelocity;
    }
}
