
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Pour recharger la scène
using UnityEngine.UI; // Pour manipuler l'UI
using System.Collections;

public class chasePlayer : MonoBehaviour
{
    public Transform[] waypoints; // Points de patrouille
    public float waypointTolerance = 1f; // Tolérance de distance pour atteindre le waypoint
    public float detectionRange = 10f; // Distance de détection du joueur
    public float fieldOfViewAngle = 45f; // Champ de vision de l'ennemi
    public Transform player; // Référence au joueur
    public float normalSpeed = 3.5f; // Vitesse de patrouille normale
    public float chaseSpeed = 5.25f; // Vitesse de poursuite du joueur
    public Camera playerCamera; // Référence à la caméra du joueur
    public float shakeDuration = 0.5f; // Durée de secousse de la caméra
    public float shakeMagnitude = 0.1f; // Amplitude de la secousse de la caméra
    public float attackRange = 2f; // Portée d'attaque de l'ennemi
    public float attackCooldown = 1.5f; // Délai entre les attaques
    public int playerHealth = 100; // Santé du joueur

    public GameObject gameOverScreen; // Référence au canvas Game Over
    public Button retryButton; // Bouton pour redémarrer la partie

    private NavMeshAgent navMeshAgent; // Agent de navigation de l'ennemi
    private Animator animator; // Référence à l'Animator
    private int currentWaypointIndex = 0; // Index du waypoint courant
    private bool isChasingPlayer = false; // L'ennemi poursuit-il le joueur ?
    private Vector3 originalCameraPosition; // Position initiale de la caméra du joueur
    private bool canAttack = true; // L'ennemi peut-il attaquer ?

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

        // Configuration du bouton "Retry" pour relancer la partie
        retryButton.onClick.AddListener(RestartGame);
        gameOverScreen.SetActive(false); // Cacher l'écran Game Over au démarrage

        playerHealth = 100; // Réinitialise la santé du joueur au début de la scène
        Time.timeScale = 1; // Remet le jeu en marche
    }

    void Update()
    {
        if (playerHealth > 0)
        {
            // Vérifier si l'ennemi peut voir le joueur
            if (CanSeePlayer())
            {
                isChasingPlayer = true;  // Début de la poursuite
            }

            if (isChasingPlayer)
            {
                // Active la poursuite
                navMeshAgent.speed = chaseSpeed;
                navMeshAgent.SetDestination(player.position);
                RotateTowards(navMeshAgent.steeringTarget);

                // Définir les paramètres de l'Animator pour "crawl_fast"
                animator.SetBool("isChasing", true);

                // Déclencher la secousse de la caméra au début de la poursuite
                StartCoroutine(CameraShake());

                // Si le joueur est à portée, attaquer
                if (Vector3.Distance(transform.position, player.position) <= attackRange && canAttack)
                {
                    AttackPlayer(); // Lancer l'attaque
                }
            }

            else
            {
                // Patrouille normale
                navMeshAgent.speed = normalSpeed;
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < waypointTolerance)
                {
                    MoveToNextWaypoint();
                }

                // Définir les paramètres de l'Animator pour "crawl"
                animator.SetBool("isChasing", false);
            }

            // Ajuster la rotation si l'ennemi se déplace
            if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
            {
                RotateTowards(navMeshAgent.steeringTarget);
            }
        }
        else
        {
            ShowGameOverScreen(); // Afficher l'écran Game Over si le joueur est mort
        }
    }

    // Rotation vers la cible
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)) * Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Passer au prochain waypoint de la patrouille
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    // Vérifie si l'ennemi peut voir le joueur
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

    // Fonction pour attaquer le joueur
    void AttackPlayer()
    {
        canAttack = false;

        // Déclencher l'animation d'attaque avec un Trigger
        animator.SetTrigger("Attack"); // L'animation d'attaque démarre

        StartCoroutine(CameraShake());
        playerHealth = 0; // Réduire la santé du joueur à zéro
        Debug.Log("Player has been oneshotted!");
        StartCoroutine(AttackCooldown());
    }

    // Cooldown d'attaque
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Secousse de la caméra
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

    // Affiche l'écran Game Over
    void ShowGameOverScreen()
    {
        Debug.Log("Game Over Screen Displayed");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0; // Arrête le jeu
        Cursor.lockState = CursorLockMode.None; // Si le curseur est caché, affiche-le
        Cursor.visible = true; // Rendre le curseur visible
    }


    // Fonction pour redémarrer le jeu
    public void RestartGame()
    {
        Debug.Log("RestartGame method triggered");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharge la scène
    }
