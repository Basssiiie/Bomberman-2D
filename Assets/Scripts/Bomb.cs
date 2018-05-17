using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public GameObject explosionPrefab;
	public float detonationTime = 3;
	public float detonationRange = 2;

	private bool isExploded = false;
	private Rigidbody2D rigid;



	/// <summary>
	/// When it spawns, start a timer to detonate it.
	/// </summary>
	void Awake ()
	{
		rigid = GetComponent<Rigidbody2D>();

		Invoke("Detonate", detonationTime);
	}


	/// <summary>
	/// When the bomb is dropped, it has no collision. But once the player leaves its trigger, 
	/// collision is turned on. (So you can block people!)
	/// </summary>
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Collider2D collider = GetComponent<Collider2D>();
			collider.isTrigger = false;
		}
	}


	/// <summary>
	/// Public function to detonate this bomb.
	/// </summary>
	public void Detonate()
	{
		if (isExploded)
			return;

		isExploded = true;

		// Change the bomb sprite to an explosion and disable and collider (for raycast purposes)
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.enabled = false;

		Collider2D collider = GetComponent<Collider2D>();
		collider.enabled = false;


		// Destroy everything in all directions
		Instantiate(explosionPrefab, transform.position, Quaternion.identity, this.transform);

		DetonateDirection(Vector2.left, detonationRange);
		DetonateDirection(Vector2.up, detonationRange);
		DetonateDirection(Vector2.right, detonationRange);
		DetonateDirection(Vector2.down, detonationRange);
	}


	/// <summary>
	/// Internal function to destroy closest object in that direction. Direction should always be 
	/// 1 meter on that x or y axis. (Ex. Vector2.up = [x: 0, y: 1], Vector2.left = [x: -1, y: 0])
	/// </summary>
	private void DetonateDirection(Vector2 direction, float distance)
	{
		RaycastHit2D hit = Physics2D.Raycast(rigid.position, direction, distance);
		float explosionDistance = distance;

		if (hit.transform != null)
		{
			explosionDistance = (hit.distance + 1);
			Debug.Log("hit tag " + hit.transform.tag);
			switch (hit.transform.tag)
			{
				// Kill a player when hitting it
				case "Player":
					Player player = hit.transform.GetComponent<Player>();
					if (player != null)
					{
						player.Kill();
					}
					break;
				
				// Destroy a box when hitting it
				case "Box":
					Destroy(hit.transform.gameObject);
					break;

				// Detonate another bomb when hitting it
				case "Bomb":
					Bomb bomb = hit.transform.GetComponent<Bomb>();
					if (bomb != null)
					{
						bomb.Detonate();
					}
					break;
				
				// Everything else it hit, but isn't explodable.
				default:
					explosionDistance = hit.distance;
					break;
			}
		}

		// Spawn fire particles in that direction
		Debug.Log("hit distance " + explosionDistance + " direction: " + direction);
		for (int step = 1; step <= explosionDistance; step++)
		{
			Vector3 offsetPosition = new Vector3(direction.x, direction.y, 0) * step;
			Instantiate(explosionPrefab, transform.position + offsetPosition, Quaternion.identity, this.transform);
		}

		// Destroy the bomb object and all effects
		Destroy(gameObject, 1f);
	}
}
