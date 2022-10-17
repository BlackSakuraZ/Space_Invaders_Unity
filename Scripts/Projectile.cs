using UnityEngine;

public class Projectile : MonoBehaviour {
	public Vector3 direction = Vector3.up; // Vector3(0, 1, 0)
    public new BoxCollider2D collider { get; private set; }
    public float speed = 20f;
    public System.Action<Projectile> destroyed;

    private void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private void CheckCollision(Collider2D other) {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if ((bunker == null) || (bunker.CheckCollision(collider, transform.position))) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        CheckCollision(other);
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        CheckCollision(other);
    }

	private void OnDestroy() {
        if (destroyed != null) {
            destroyed.Invoke(this);
        }
    }
}