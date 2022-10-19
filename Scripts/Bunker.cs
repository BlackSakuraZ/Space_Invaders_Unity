using UnityEngine;

public class Bunker : MonoBehaviour {
	int killTimes = 10;
    public Texture2D texture;
    public SpriteRenderer spriteRenderer;
    public new BoxCollider2D collider;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        texture = spriteRenderer.sprite.texture;
        ResetBunker();
    }

    public void ResetBunker() {
		killTimes = 10;
        Texture2D copy = new Texture2D(texture.width, texture.height, texture.format, false);
        copy.SetPixels(texture.GetPixels());
        copy.Apply();
        spriteRenderer.sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, 
                                              new Vector2(0.5f, 0.5f), 
                                              spriteRenderer.sprite.pixelsPerUnit);
        gameObject.SetActive(true);
    }

    public bool CheckPoint(Vector3 hitPoint) {
        Vector3 localPoint = transform.InverseTransformPoint(hitPoint);
		Texture2D texture = spriteRenderer.sprite.texture;
        localPoint.x += collider.size.x / 2;
        localPoint.y += collider.size.y / 2;
        int xCoordinate = (int)((localPoint.x / collider.size.x) * texture.width);
        int yCoordinate = (int)((localPoint.y / collider.size.y) * texture.height);
        return texture.GetPixel(xCoordinate, yCoordinate).a != 0f;
    }

    public bool CheckCollision(BoxCollider2D other, Vector3 hitPoint) {
        Vector2 offset = other.size / 2;
        return (CheckPoint(hitPoint)) ||
               (CheckPoint(hitPoint + (Vector3.down * offset.y))) || //(0,-1,0)
               (CheckPoint(hitPoint + (Vector3.up * offset.y))) ||    //(0,1,0)
               (CheckPoint(hitPoint + (Vector3.left * offset.x))) || //(-1,0,0)
               (CheckPoint(hitPoint + (Vector3.right * offset.x)));   //(1,0,0)
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            gameObject.SetActive(false);
        }
		if (other.gameObject.layer == LayerMask.NameToLayer("Missile")
			|| other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            killTimes--;
			if (killTimes <= 0) {
				gameObject.SetActive(false);
			}
        }
    }
}
