using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topping : MonoBehaviour
{
    public bool homing = false;
    public bool sine = false;
    public float sineFrequency = 1;
    [SerializeField]
    public float YSpeed = -10;
    [SerializeField]
    public float XSpeed = 4;
    [SerializeField]
    private Rigidbody2D myRB;
    private float DESTROY_LINE = -12;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < DESTROY_LINE) { Destroy(gameObject); }
    }
    private void FixedUpdate()
    {
        Vector2 lastVelocity = myRB.velocity;
        lastVelocity.y = YSpeed * Time.fixedDeltaTime;
        if (homing)
        {
            float playerPos = GameProgressManager.CurrentPlayer().transform.position.x;
            if (Mathf.Abs(transform.position.x - playerPos) > 0.2)
            {
                lastVelocity.x = Mathf.Sign(playerPos - transform.position.x) * Time.fixedDeltaTime * XSpeed;
            }
        }
        if (sine)
        {
            lastVelocity.x = Mathf.Sin(Time.timeSinceLevelLoad * sineFrequency) * XSpeed * Time.fixedDeltaTime;
        }
        myRB.velocity = lastVelocity;
    }
    private void OnDestroy()
    {
        SpawnManager.DeregisterItem(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null &&
            collision.gameObject.GetComponent<PlayerController>().isControllable)
        {
            OnContactPlayer();
            Destroy(gameObject);
        }        
    }
    protected virtual void OnContactPlayer()
    {

    }
}
