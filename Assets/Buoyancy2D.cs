using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy2D : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public int nbDivH = 1;
    public int nbDivV = 1;
    private Vector2 divSize;
    private List<Vector2> divs = new List<Vector2>();

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        divSize = new Vector2(spriteRenderer.size.x / nbDivH, spriteRenderer.size.y / nbDivV);


        for(int j = 0; j < nbDivV; j++)
        {
            for(int i = 0; i < nbDivH; i++)
            {
                Vector3 point = new Vector3(spriteRenderer.bounds.center.x + divSize.x * i, spriteRenderer.bounds.center.y + divSize.y * -j, transform.position.z);
                Matrix4x4 m1 = Matrix4x4.Translate(new Vector3(-divSize.x * i, -divSize.y * -j, 0));
                Matrix4x4 m2 = Matrix4x4.Rotate(transform.rotation);
                Matrix4x4 m3 = Matrix4x4.Translate(new Vector3(divSize.x * i, divSize.y * -j, 0));
                Matrix4x4 m = m3 * m2 * m1;
                point = m.MultiplyPoint3x4(point);
                divs.Add(point);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnDrawGizmos() {
        DrawBounds();
        for(int i = 0; i < divs.Count; i++)
        {
            DrawPoint(divs[i]);
        }
    }

    public void DrawBounds() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(spriteRenderer.bounds.center + transform.TransformDirection(Vector2.up) * spriteRenderer.size.y / 2 + transform.TransformDirection(Vector2.left) * spriteRenderer.size.x / 2, transform.TransformDirection(Vector2.right) * spriteRenderer.size.x);
        Gizmos.DrawRay(spriteRenderer.bounds.center + transform.TransformDirection(Vector2.up) * spriteRenderer.size.y / 2 + transform.TransformDirection(Vector2.right) * spriteRenderer.size.x / 2, transform.TransformDirection(Vector2.down) * spriteRenderer.size.y);
        Gizmos.DrawRay(spriteRenderer.bounds.center + transform.TransformDirection(Vector2.down) * spriteRenderer.size.y / 2 + transform.TransformDirection(Vector2.left) * spriteRenderer.size.x / 2, transform.TransformDirection(Vector2.right) * spriteRenderer.size.x);
        Gizmos.DrawRay(spriteRenderer.bounds.center + transform.TransformDirection(Vector2.up) * spriteRenderer.size.y / 2 + transform.TransformDirection(Vector2.left) * spriteRenderer.size.x / 2, transform.TransformDirection(Vector2.down) * spriteRenderer.size.y);
    }

    public void DrawPoint(Vector2 point) {
        Gizmos.DrawSphere(new Vector3(point.x, point.y, 0) + spriteRenderer.bounds.center, 0.05f);
    }
}
