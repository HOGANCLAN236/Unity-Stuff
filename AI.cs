using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class AI : MonoBehaviour
{
    //Ambience
    //Random Roam (Done)
    //Detected/Seen/Jumpscare (---)
    //Voice volume 

    [Header("Enemy")]
    NavMeshAgent Enemy;
    [SerializeField] GameObject Prefab;
    GameObject NewEnemy;
    public float RoamSpeed;
    public float DetectedSpeed;
    public float ChaseSpeed;

    [Header("Player")]
    [SerializeField] GameObject Player;

    [Header("Sounds")]
    //[SerializeField] AudioSource[] Ambience;
    //[SerializeField] AudioSource Detected;

    [Header("Roaming points")]
    [SerializeField] List<Transform> RoamingObjects = new List<Transform>();

    [Header("The Detected radius")]
    [SerializeField] float DetectedRadius;

    [Header("The Chase radius")]
    [SerializeField] float SeeingRadius;

    [SerializeField] Image Fill;

    float X;
    float Z;

    public float RangeX;
    public float RangeX2;
    public float RangeZ;
    public float RangeZ2;

    bool IsGoing;
    bool IsInstantiated;
    public float pathEndThreshold = 0.1f;
    private bool hasPath = false;
    float distance;
    float Volume;
    float maxVolume = 0.65f;


    bool HasChecked;
    bool isInit;
    Vector3 oldPos;

    public MicrophoneManagerMain microphoneManager;

    void SpawnEnemy()
    {
        X = Random.Range(RangeX, RangeX2);
        Z = Random.Range(RangeZ, RangeZ2);


        Vector3 pos = new Vector3(X, 100f, Z);

        if (Physics.Raycast(pos, -Vector3.up, out RaycastHit hit))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                Vector3 Npos = new Vector3(0, 0, 0);
                Npos = hit.point;

                NavMeshHit closestHit;
                if (NavMesh.SamplePosition(hit.point, out closestHit, 100f, NavMesh.AllAreas))
                {
                    NewEnemy = Instantiate(Prefab, hit.point, Quaternion.identity);
                    Debug.Log("Instatiated");
                    isInit = true;
                    Enemy = NewEnemy.GetComponent<NavMeshAgent>();
                    Vector3 Y = new Vector3(NewEnemy.transform.position.x, NewEnemy.transform.position.y, NewEnemy.transform.position.z);
                    Y.y += 0.001f;
                    NewEnemy.transform.position = Y;
                }
                else
                {
                    Debug.Log("Not on Nav mesh");
                }

            }
        }
    }

    bool AtEndOfPath()
    {
        hasPath |= Enemy.hasPath;
        if (hasPath && Enemy.remainingDistance <= Enemy.stoppingDistance + pathEndThreshold)
        {
            // Arrived
            IsGoing = false;
            return true;
        }

        return false;
    }

    void Detected_Void()
    {
        if (!HasChecked)
        {
            oldPos = Player.transform.position;
            HasChecked = true;
        }
        Enemy.speed = DetectedSpeed;
        //Detected.Play();
        Enemy.destination = oldPos;
        HasChecked = false;
    }

    void HardOfHearing()
    {
        if (Fill.color == Color.red)
        {
            Detected_Void();
            Debug.Log("Too Loud");
        }
    }

    void Chasing_Void()
    {
        Enemy.speed = ChaseSpeed;
        Enemy.destination = Player.transform.position;
    }

    void RandomRoam()
    {
        Enemy.speed = RoamSpeed;

        List<int> Indexs = new List<int>();

        //Adding stuff to list 
        for (int i = 0; i < 5; i++)
        {
            int count = 0;
            Indexs.Add(count);
            count++;
        }

        if (!IsGoing && !Enemy.pathPending)
        {
            //Moving / Checking
            for (int i = 0; i < Indexs.Count; i++)
            {
                int Index = Random.Range(0, 4);
                Enemy.destination = RoamingObjects[Index].transform.position;
                Indexs.RemoveAt(Index);
                IsGoing = true;
            }
        }

        if (Indexs.Count == 0)
        {
            for (int j = 0; j < 5; j++)
            {
                int count = 0;
                Indexs.Add(count);
                count++;
            }
        }
    }

    private void Update()
    {
        Volume = microphoneManager.Volume.value;

        if (!isInit)
        {
            SpawnEnemy();
        }

        if (isInit)
        {
            distance = Vector3.Distance(Player.transform.position, NewEnemy.transform.position);
            HardOfHearing();
        }

        if (isInit && AtEndOfPath() && !IsGoing && distance > DetectedRadius)
        {
            RandomRoam();
        }

        if (distance <= DetectedRadius && isInit)
        {
            Detected_Void();
        }
        else
        {
            RandomRoam();
        }

        if (distance <= SeeingRadius && isInit)
        {
            Chasing_Void();
        }
        else
        {
            RandomRoam();
        }

        //Invoke("AmbienceCaller", Random.Range(60, 120));
    }



    //void AmbienceCaller()
    //{
    //    StartCoroutine(Ambience_void(Ambience));
    //}

    //IEnumerator Ambience_void(AudioSource[] Ambience)
    //{
    //    yield return new WaitForSeconds(Random.Range(10, 50));
    //    int Index = Random.Range(0, Ambience.Length);

    //    if (!Ambience[Index].isPlaying)
    //    {
    //        Ambience[Index].Play();
    //    }

    //}
}
