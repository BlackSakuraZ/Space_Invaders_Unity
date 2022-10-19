using UnityEngine;

public class Invader : MonoBehaviour {
    public int score = 10;
    public Sprite[] animationSprites = new Sprite[0];
    public SpriteRenderer spriteRenderer;
    public int animationFrame;
    public System.Action<Invader> killed;

    // стартуем анимацию движения
    private void Start() {
        InvokeRepeating(nameof(InvaderMove), 1f, 1f);
    }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }

    private void InvaderMove() {
        animationFrame++;
        if (animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            killed?.Invoke(this);
        }
    }
}
