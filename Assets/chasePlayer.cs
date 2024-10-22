using UnityEngine;
using UnityEngine.AI;

public class crawler : MonoBehaviour
{
    public Transform[] waypoints; // Liste des points de passage
    public float waypointTolerance = 1f; // Distance tolérée pour atteindre le waypoint
    public float detectionRange = 10f; // Distance de détection du joueur
    public float fieldOfViewAngle = 45f; // Angle du champ de vision de l'ennemi
    public Transform player; // Référence au joueur

    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            // Poursuivre le joueur
            navMeshAgent.SetDestination(player.position);

            // Vérifier si le joueur est toujours visible
            if (!CanSeePlayer())
            {
                isChasingPlayer = false;
                MoveToNextWaypoint(); // Revenir à la patrouille si le joueur n'est plus visible
            }
        }
        else
        {
            // Continuer la patrouille si le joueur n'est pas détecté
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < waypointTolerance)
            {
                MoveToNextWaypoint();
            }

            // Vérifier si le joueur est dans le champ de vision
            if (CanSeePlayer())
            {
                isChasingPlayer = true;
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Passer au prochain waypoint
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (directionToPlayer.magnitude < detectionRange && angleToPlayer < fieldOfViewAngle)
        {
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;

            // Utiliser un raycast pour s'assurer qu'il n'y a pas d'obstacle entre l'ennemi et le joueur
            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true; // Le joueur est dans le champ de vision
                }
            }
        }

        return false; // Le joueur n'est pas visible
    }
}