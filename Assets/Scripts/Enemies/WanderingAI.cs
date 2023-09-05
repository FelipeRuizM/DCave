using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    private float enemySpeed = 1.75f;
    private float obstacleRange = 5.0f;
    private float sphereRadius = 0.75f;

    private float baseSpeed = 0.25f;
    float difficultySpeedDelta = 0.3f; // the change in speed per level of difficulty

    // Update is called once per frame
    void Update()
    {
        // Move Enemy
        transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
        // generate Ray
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward + new Vector3(0, 1, 0));
        // Spherecast and determine if Enemy needs to turn
        RaycastHit hit;
        if (Physics.SphereCast(ray, sphereRadius, out hit))
        {
            if (hit.distance < obstacleRange)
            {
                float turnAngle = Random.Range(-110, 110);
                transform.Rotate(Vector3.up * turnAngle);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // determine the range vector (starting at the enemy)
        Vector3 rangeTest = transform.position + transform.forward * obstacleRange + new Vector3(0, 1, 0);

        // draw a line to show the range vector
        Debug.DrawLine(transform.position + new Vector3(0, 1, 0), rangeTest);

        // draw a wire sphere at the point on the end of the range vector
        Gizmos.DrawWireSphere(rangeTest, sphereRadius);
    }

    public void SetDifficulty(int difficulty)
    {
        Debug.Log("WanderingAI.SetDifficulty(" + difficulty + ")");
        enemySpeed = baseSpeed + (difficulty * difficultySpeedDelta);
    }
}