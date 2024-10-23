using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Pour recharger la scène
using UnityEngine.UI; // Pour manipuler l'UI

public class chasePlayer : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointTolerance = 1f;
    public float detectionRange = 10f;
    public float fieldOfViewAngle = 45f;
    public Transform player;
    public float normalSpeed = 3.5f;
    public float chaseSpeed = 5.25f;
    public Camera playerCamera;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int playerHealth = 100;

    public GameObject gameOverScreen; // Référence à l'écran Game Over
    public Button retryButton; // Bouton pour redémarrer la partie

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool isChasingPlayer = false;
    private Vector3 originalCameraPosition;
    private bool canAttack = true;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        MoveToNextWaypoint();

        if (playerCamera != null)
        {
            originalCameraPosition = playerCamera.transform.localPosition;
        }

        // Assigner la fonction de redémarrage au bouton
        retryButton.onClick.AddListener(RestartGame);
        gameOverScreen.SetActive(false); // Cacher l'écran de game over au début
    }

    void Update()
    {
        if (playerHealth > 0)
        {
            if (isChasingPlayer)
            {
                navMeshAgent.speed = chaseSpeed;
                navMeshAgent.SetDestination(player.position);
                RotateTowards(navMeshAgent.steeringTarget);

                if (Vector3.Distance(transform.position, player.position) <= attackRange && canAttack)
                {
                    AttackPlayer(); // Lancer l'attaque si le joueur est à portée
                }

                if (!CanSeePlayer())
                {
                    isChasingPlayer = false;
                    navMeshAgent.speed = normalSpeed;
                    MoveToNextWaypoint();
                }
            }
            else
            {
                navMeshAgent.speed = normalSpeed;
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < waypointTolerance)
                {
                    MoveToNextWaypoint();
                }

                if (CanSeePlayer())
                {
                    isChasingPlayer = true;
                }
            }

            if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
            {
                RotateTowards(navMeshAgent.steeringTarget);
            }
        }
        else
        {
            ShowGameOverScreen(); // Affiche l'écran "Game Over" si le joueur est mort
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)) * Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (directionToPlayer.magnitude < detectionRange && angleToPlayer < fieldOfViewAngle)
        {
            Ray ray = new Ray(transform.position, directionToPlayer);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void AttackPlayer()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        StartCoroutine(CameraShake());
        playerHealth = 0; // Mettre la santé du joueur à zéro
        Debug.Log("Player has been oneshotted!");
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator CameraShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalCameraPosition + Random.insideUnitSphere * shakeMagnitude;
            playerCamera.transform.localPosition = new Vector3(randomPoint.x, originalCameraPosition.y, originalCameraPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = originalCameraPosition;
    }

    // Affiche l'écran "Game Over"
    void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0; // Arrête le jeu
    }

    // Fonction pour redémarrer le jeu
    public void RestartGame()
    {
        Time.timeScale = 1; // Remet le jeu en marche
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la scène
    }
}
