using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour 
{
    private Rigidbody2D rgdBody2D;
    [SerializeField] private float acceceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float airAcceceleration;
    [SerializeField] private float airDecceleration;
    [SerializeField] private float maxHorizontalSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask checkGroundMask;
    [SerializeField] private float checkGroundDistance;
    [SerializeField] private float checkGroundSize;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Vector3 tileMapOffset1;
    [SerializeField] private Vector3 tileMapOffset2;
    private float move;
    private Vector2 velocity;
    private float horizontalInput;
    private float jump;
    private Transform trsf;
    RaycastHit2D[] checkGroundRay = new RaycastHit2D[1];
    private BoxCollider2D boxCollider2D;

	void Awake () 
    {
        rgdBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        trsf = transform;
	}

	void Update () 
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
	}

    void FixedUpdate()
    {
        if (CheckGround() == true)
        {
            if (horizontalInput > 0.25f || horizontalInput < -0.25f)
            {
                move += acceceleration * horizontalInput * Time.fixedDeltaTime;
            }
            else
            {
                move = Mathf.Lerp(move, 0.0f, Time.fixedDeltaTime * decceleration);
            }

            if (jump > 0.5f)
            {
                velocity.y = jump * jumpForce;
                Vector3 p = trsf.position;
                tileMap.SetTile(tileMap.WorldToCell(p + tileMapOffset1), null);
                tileMap.SetTile(tileMap.WorldToCell(p + tileMapOffset2), null);
            }
        }
        else
        {
            if (horizontalInput > 0.25f || horizontalInput < -0.25f)
            {
                move += airAcceceleration * horizontalInput * Time.fixedDeltaTime;
            }
            else
            {
                move = Mathf.Lerp(move, 0.0f, Time.fixedDeltaTime * airDecceleration);
            }

            velocity.y -= gravity * Time.fixedDeltaTime;
        }

        if (CheckCeil() == true && velocity.y > 0.0f)
        {
            velocity.y = 0.0f;
        }

        velocity.x = Mathf.Clamp(move, -maxHorizontalSpeed, maxHorizontalSpeed);

        rgdBody2D.velocity = velocity;
    }

    bool CheckGround()
    {
        int i = Physics2D.BoxCastNonAlloc(trsf.position, boxCollider2D.size * checkGroundSize, 0.0f, new Vector2(0.0f, -1.0f), checkGroundRay, checkGroundDistance, checkGroundMask, -1.0f, 1.0f);

        if (i > 0)
        {
            spriteRenderer.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            return true;
        }
        else
        {
            spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            return false;
        }
    }

    bool CheckCeil()
    {
        int i = Physics2D.BoxCastNonAlloc(trsf.position, boxCollider2D.size * checkGroundSize, 0.0f, new Vector2(0.0f, 1.0f), checkGroundRay, checkGroundDistance, checkGroundMask, -1.0f, 1.0f);

        if (i > 0)
        {
            spriteRenderer.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            return true;
        }
        else
        {
            spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            return false;
        }
    }
}
