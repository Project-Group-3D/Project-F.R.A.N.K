using UnityEngine;
using UnityEngine.AI;

public class chasePlayer : MonoBehaviour
{
    public Transform[] waypoints; // Liste des points de passage
    public float waypointTolerance = 1f; // Distance tolérée pour atteindre le waypoint
    public float detectionRange = 10f; // Distance de détection du joueur
    public float fieldOfViewAngle = 45f; // Angle du champ de vision de l'ennemi
    public Transform player; // Référence au joueur

    private NavMeshAgent navMeshAgent;
    private Animator animator; // Référence à l'Animator
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Récupérer l'Animator
        navMeshAgent.updateRotation = false; // Désactiver la rotation automatique
        transform.rotation = Quaternion.Euler(0, 0, 0); // Forcer la rotation initiale
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            // Poursuivre le joueur
            navMeshAgent.SetDestination(player.position);

            // Appliquer la rotation vers la direction du mouvement
            RotateTowards(navMeshAgent.steeringTarget);

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

        // Si le monstre se déplace, tourner dans la direction du mouvement
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            RotateTowards(navMeshAgent.steeringTarget);
        }
    }

    // Fonction pour faire tourner l'ennemi vers la direction du mouvement
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        
        // Appliquer un offset de 90° sur l'axe Y si le modèle est mal orienté
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)) * Quaternion.Euler(0, 90, 0); // Ajuste ici l'offset

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
