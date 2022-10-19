using UnityEngine;

public class Invaders : MonoBehaviour {
    public int rows = 5;
    public int columns = 11;
    public Projectile missilePrefab;
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    public Vector3 direction = Vector3.right;
    public Vector3 startPosition;
    public System.Action<Invader> killed;
    public int CounterKilled;
    public int CountAlive => TotalAmount - CounterKilled;
    public int TotalAmount => rows * columns;
    public float PercentageKilled => (float)CounterKilled / (float)TotalAmount;

    private void Start() {
        InvokeRepeating(nameof(Attack), 1f, 1f + ((float) 1/(CounterKilled + 1)));
    }
	
	// расставляем врагов
    private void Awake() {
        startPosition = transform.position;
        for (int i = 0; i < rows; i++) {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);
            Vector2 center = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(center.x, (2f * i) + center.y, 0f);
            for (int j = 0; j < columns; j++) {
                Invader invader = Instantiate(prefabs[i], transform);
                invader.killed += OnInvaderKilled;
                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Update() {
        float speed = this.speed.Evaluate(PercentageKilled);
        transform.position += direction * speed * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        foreach (Transform invader in transform) {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }
	    // разворачиваем движение в другую сторону и сдвигаем на одну строку вниз
            if ((direction == Vector3.left) && (invader.position.x <= (leftEdge.x + 1f))
		|| (direction == Vector3.right) && (invader.position.x >= (rightEdge.x - 1f))) {
                direction = new Vector3(-direction.x, 0f, 0f);
        	Vector3 position = transform.position;
       		position.y -= 1f;
                transform.position = position;
                break;
            }
        }
    }
	
	// увеличиваем частоту атак с уменьшением врагов
    private void Attack() {
        foreach (Transform invader in transform) {
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }
            if (Random.value < (1f / (float)CountAlive)) {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void OnInvaderKilled(Invader invader) {
        invader.gameObject.SetActive(false);
        CounterKilled++;
        killed(invader);
    }

    public void ResetInvaders() {
        CounterKilled = 0;
        direction = Vector3.right;
        transform.position = startPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }
}
