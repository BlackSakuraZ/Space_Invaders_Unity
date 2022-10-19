using UnityEngine;

public class MysteryShip : MonoBehaviour {
	public int score = 300;
    public float speed = 3f;
    public System.Action<MysteryShip> killed;
    public Vector3 leftDestination;
    public Vector3 rightDestination;
    public bool spawned;

    private void Start() {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        Vector3 left = transform.position;
        left.x = leftEdge.x - 1f;
        leftDestination = left;
        Vector3 right = transform.position;
        right.x = rightEdge.x + 1f;
        rightDestination = right;
        transform.position = leftDestination;
        Despawn();
    }

    private void Update() {
        if (!spawned) {
            return;
        }
		transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= rightDestination.x) {
            Despawn();
        }
    }

    private void Spawn() {
        spawned = true;
    }

    private void Despawn() {
        spawned = false;
		transform.position = leftDestination;
        Invoke(nameof(Spawn), 30f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            Despawn();
            if (killed != null) {
                killed.Invoke(this);
            }
        }
    }
}
