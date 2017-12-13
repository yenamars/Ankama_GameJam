using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour 
{
    private Rigidbody2D rgdBody2D;
    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private LayerMask checkGroundMask;
    [SerializeField] private float checkGroundDistance;
    [SerializeField] private float checkGroundSize;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Vector3 tileMapOffset1;
    [SerializeField] private Vector3 tileMapOffset2;
    private Vector2 velocity;
    private float horizontalInput;
    private float jump;
    private Transform trsf;
    RaycastHit2D[] checkGroundRay = new RaycastHit2D[1];
    private CircleCollider2D circleCollider2D;

	void Awake () 
    {
        rgdBody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        trsf = transform;
	}

	void Update () 
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
	}

    void FixedUpdate()
    {
        if (CheckGround(new Vector2(0.0f, -1.0f)) == true)
        {
            velocity.x = horizontalInput * speed;

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
            velocity.x = horizontalInput * airSpeed;
            velocity.y -= gravity * Time.fixedDeltaTime;
        }

        if (CheckGround(new Vector2(0.0f, 1.0f)) == true && velocity.y > 0.0f)
        {
            velocity.y = 0.0f;
        }

        rgdBody2D.velocity = velocity;
    }

    bool CheckGround(Vector2 dir)
    {
        int i = Physics2D.CircleCastNonAlloc(rgdBody2D.position, circleCollider2D.radius, dir, checkGroundRay, checkGroundDistance, checkGroundMask, -1.0f, 1.0f);

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
