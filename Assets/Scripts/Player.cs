using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public string horizontalInput;
	public string verticalInput;
	public KeyCode bombKey;
	public float walkSpeed;

	public GameObject bombPrefab;
	public Sprite skeletonSprite;


	private Rigidbody2D rigid;
	private bool isDead = false;
	

	/// <summary>
	/// Important! Retrieve this object its own components in Awake, and other objects their 
	/// components in Start()!
	/// </summary>
	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
	}


	/// <summary>
	/// Movement for this player. Whenever you do any kind of movement with Physics/Rigidbodies,
	/// then ALWAYS use FixedUpdate! FixedUpdate is made to synchronise your movement with other
	/// physics changes in the game.
	/// </summary>
	void FixedUpdate ()
	{
		if (isDead)
			return;

		float horizontalMovement = Input.GetAxis(horizontalInput);
		float verticalMovement = Input.GetAxis(verticalInput);
		
		Vector2 movement = new Vector2(horizontalMovement, verticalMovement) * walkSpeed * Time.fixedDeltaTime;
		rigid.MovePosition(rigid.position + movement);
	}


	/// <summary>
	/// Bomb dropping for the player. Whenever you use GetKeyDown or GetKeyUp, ALWAYS use Update!
	/// These functions will only be 'true' for one Update-frame, and may fail to always work in
	/// FixedUpdate.
	/// </summary>
	void Update()
	{
		if (isDead)
			return;

		if (Input.GetKeyDown(bombKey))
		{
			// Align with grid
			float x = Mathf.FloorToInt(rigid.position.x) + 0.5f;
			float y = Mathf.FloorToInt(rigid.position.y) + 0.5f;
			Vector3 bombPos = new Vector3(x, y, 0);

			Instantiate(bombPrefab, bombPos, Quaternion.identity);
		}
	}


	/// <summary>
	/// Public method to kill this player.
	/// </summary>
	public void Kill()
	{
		if (isDead)
			return;

		isDead = true;

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = skeletonSprite;
	}
}
