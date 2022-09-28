using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBirdHandControl : MonoBehaviour
{
    public Transform dragStart;
    public LineRenderer lineRender;
    public int stepCount = 50;
    public Rigidbody2D ball;
    public float shootSpeed = 20;
    private bool isDragging = false;
    private Vector3 projectialVec;
    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
        ball.isKinematic = true;
    }
    private void Update()
    {
        //if (isDragging) {
        if (Input.GetMouseButton(0))
        {
            Vector2 dragDir = -(_cam.ScreenToWorldPoint(Input.mousePosition) - dragStart.position);
            dragDir = MyVectorExtension.ClampedAngle(dragDir, Vector2.right, 60);
            projectialVec = dragDir.normalized * shootSpeed * Mathf.Abs(dragDir.x);
            List<Vector3> plots = MyVectorExtension.Plot2D(ball, ball.velocity, projectialVec, -1f, out stepCount);
            lineRender.positionCount = stepCount;
            lineRender.SetPositions(plots.ToArray());

        }
        else if (Input.GetMouseButtonUp(0))
        {
            ball.isKinematic = false;
            Debug.Log(projectialVec);
            ball.AddForce(projectialVec, ForceMode2D.Impulse);

        }
    }


}
