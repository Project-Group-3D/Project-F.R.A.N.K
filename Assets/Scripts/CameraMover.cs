using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Vector3[] waypoints; // Points entre lesquels la caméra va se déplacer
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Déplacer la caméra vers le point actuel
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex], moveSpeed * Time.deltaTime);

        // Vérifier si la caméra a atteint le waypoint actuel
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.1f)
        {
            // Passer au prochain waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
