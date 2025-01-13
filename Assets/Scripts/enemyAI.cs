using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPosition;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;

    bool playerInRange;

    Color colorOriginal;

    Vector3 playerDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOriginal = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            playerDirection = GameManager.instance.player.transform.position - transform.position;

            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOriginal;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
