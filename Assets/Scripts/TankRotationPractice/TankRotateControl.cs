using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankRotateControl : MonoBehaviour
{
    public GameObject camera;
    public Transform barrel;
    public Transform target;
    public Transform shootPoint;
    public int lineCounts = 50;
    public LineRenderer lineRenderer;

    public Rigidbody bellet;

    private void FixedUpdate()
    {

        target.position = transform.forward * 10;

        barrel.rotation = Quaternion.LookRotation(camera.transform.forward);

        Vector3 _shootDir = (shootPoint.position - barrel.position).normalized;
        float _ang = Mathf.Atan2(_shootDir.y, _shootDir.x) * Mathf.Rad2Deg;
        Vector3 shootVec = MyVectorExtension.GetProjectileShootVelocity(target.position,shootPoint.position, _ang);
        Vector3[] plots = MyVectorExtension.ProjectilePlots(shootVec, shootPoint.position, 0.05f, 0,out lineCounts);
        lineRenderer.positionCount = lineCounts;
        lineRenderer.SetPositions(plots);


        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            Rigidbody _bullet = GameObject.Instantiate<Rigidbody>(bellet);
            _bullet.transform.position = shootPoint.position;
            _bullet.velocity = shootVec;
        }

    }
}
