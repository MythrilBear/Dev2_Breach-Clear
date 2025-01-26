using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawnSoldier;
    [SerializeField] GameObject objectToSpawnSniper;
    [SerializeField] GameObject ObjectToSpawnHeavy;
    [SerializeField] int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPosSoldier;
    [SerializeField] Transform[] spawnPosSniper;
    [SerializeField] Transform[] spawnPosHeavy;

    float spawnTimer;

    int spawnCount;

    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (startSpawning)
        {
            if (spawnCount < numToSpawn && spawnTimer >= timeBetweenSpawns)
            {
                spawnSoldier();
                spawnSniper();
                spawnHeavy();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;

        }
    }

    void spawnSoldier()
    {
        int spawnInt = Random.Range(0, spawnPosSoldier.Length);

        Instantiate(objectToSpawnSoldier, spawnPosSoldier[spawnInt].position, spawnPosSoldier[spawnInt].rotation);
        spawnCount++;
        spawnTimer = 0;
    }

    void spawnSniper()
    {
        int spawnInt = Random.Range(0, spawnPosSniper.Length);

        Instantiate(objectToSpawnSniper, spawnPosSniper[spawnInt].position, spawnPosSniper[spawnInt].rotation);
        spawnCount++;
        spawnTimer = 0;
    }

    void spawnHeavy()
    {
        int spawnInt = Random.Range(0, spawnPosHeavy.Length);

        Instantiate(ObjectToSpawnHeavy, spawnPosHeavy[spawnInt].position, spawnPosHeavy[spawnInt].rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}
