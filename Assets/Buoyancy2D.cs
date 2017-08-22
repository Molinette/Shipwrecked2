using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy2D : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    public bool drawGizmos = false;

    public LayerMask waterLayers;

    public int nbDivH = 1;
    public int nbDivV = 1;
    private Vector2 divSize;
    private List<Vector2> divs = new List<Vector2>();
    private List<Vector2> submergedDivs = new List<Vector2>();

    private Vector2 buoyancyCenter = Vector2.zero;
    private int nbSubDiv = 0;
    private float subVol = 0;
    private Vector2 gravity;
    public float fluidDensity;
    public float linearDrag = 4f;
    public float angularDrag = 4f;







    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        divSize = new Vector2(col.bounds.size.x / nbDivH, col.bounds.size.y / nbDivV);

        gravity = Physics2D.gravity;

        float divX;
        float divY;
        Vector3 point;
        Matrix4x4 m;
        for (int j = 0; j < nbDivV; j++) {
            for(int i = 0; i < nbDivH; i++) {
                divX = (col.bounds.center.x - col.bounds.size.x / 2) + (divSize.x * i + divSize.x / 2);
                divY = (col.bounds.center.y + col.bounds.size.y / 2) + (divSize.y * -j - divSize.y / 2);
                point = transform.InverseTransformPoint(new Vector3(divX, divY, col.bounds.center.z));
                m = Matrix4x4.Rotate(transform.rotation);
                point = m.MultiplyPoint3x4(point);
                if (Physics2D.OverlapPoint(transform.TransformPoint(point)) == col) {
                    divs.Add(point);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerStay2D(Collider2D otherCol) {

        buoyancyCenter = Vector3.zero;
        nbSubDiv = 0;

        for (int i = 0; i < divs.Count; i++) {
            if (Physics2D.OverlapPoint(transform.TransformPoint(divs[i]), waterLayers)) {
                DrawRectDebug(transform.TransformPoint(divs[i]), divSize, Color.green);

                buoyancyCenter += new Vector2(transform.TransformPoint(divs[i]).x, transform.TransformPoint(divs[i]).y);
                nbSubDiv++;
            }
        }
        if (nbSubDiv > 0)
            buoyancyCenter = buoyancyCenter / nbSubDiv;

        subVol = nbSubDiv * divSize.x * divSize.y;

        rb.AddForceAtPosition(-gravity * subVol * fluidDensity, buoyancyCenter);
        rb.AddForce(Mathf.Pow(rb.velocity.magnitude,2) * linearDrag * -rb.velocity.normalized);
        rb.AddTorque(Mathf.Pow(rb.angularVelocity, 2) * angularDrag * -Mathf.Sign(rb.angularVelocity));
    }

    private void OnDrawGizmos()
    {
        DrawPoint(buoyancyCenter);
    }

    //GIZMOS

    public void DrawPoint(Vector2 point) {
        Gizmos.DrawSphere(new Vector3(point.x, point.y, 0), 0.05f);
    }

    public void DrawRectDebug(Vector3 center, Vector2 size, Color color)
    {
        Debug.DrawRay(center + transform.TransformDirection(Vector2.up) * size.y / 2 + transform.TransformDirection(Vector2.left) * size.x / 2, transform.TransformDirection(Vector2.right) * size.x, color);
        Debug.DrawRay(center + transform.TransformDirection(Vector2.up) * size.y / 2 + transform.TransformDirection(Vector2.right) * size.x / 2, transform.TransformDirection(Vector2.down) * size.y, color);
        Debug.DrawRay(center + transform.TransformDirection(Vector2.down) * size.y / 2 + transform.TransformDirection(Vector2.left) * size.x / 2, transform.TransformDirection(Vector2.right) * size.x, color);
        Debug.DrawRay(center + transform.TransformDirection(Vector2.up) * size.y / 2 + transform.TransformDirection(Vector2.left) * size.x / 2, transform.TransformDirection(Vector2.down) * size.y, color);
    }
}
