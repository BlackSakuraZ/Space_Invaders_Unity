using UnityEngine;

public class Player : MonoBehaviour {
    public Projectile laserPrefab;
    public System.Action killed;
    public float speed = 7f;
    public bool laserActive;

	// хотим, чтобы в момент времени была активна только одна пуля
    private void Shoot() {
        if (!laserActive) {
            laserActive = true;
            Projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed(Projectile laser) {
        laserActive = false;
    }
	
	// враги и пули убивают игрока
    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Missile")) ||
            (other.gameObject.layer == LayerMask.NameToLayer("Invader"))) {
            if (killed != null) {
                killed.Invoke();
            }
        }
    }

	private void Update() {
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }
		//предотвращаем вылет за границы экрана
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }
}
