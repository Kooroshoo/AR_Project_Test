using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agentNavMesh;
    Animator anim;

    [SerializeField] GameObject christamsTree;
    [SerializeField] bool agentIsSitting;
 
    bool agentHasStartedWalking = false;

    enum State { idle, walk, look, clap, Reset};
    State playerState = State.idle;

    int playerStateNum = 0;



    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agentNavMesh = this.GetComponent<NavMeshAgent>();

        anim = this.GetComponent<Animator>();

        if (agentIsSitting == true)
        {
            anim.SetTrigger("Idle2");
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || playerStateNum == 2)
        {
            playerState = State.idle;
            Debug.Log("Idle State");
        }
        else if (Input.GetKeyDown(KeyCode.W) || playerStateNum == 1)
        {
            playerState = State.walk;
            Debug.Log("Walking State");
        }
        else if (Input.GetKeyDown(KeyCode.L) || playerStateNum == 3)
        {
            playerState = State.look;
            Debug.Log("Look State");
        }
        else if (Input.GetKeyDown(KeyCode.C) || playerStateNum == 4)
        {
            playerState = State.clap;
            Debug.Log("Clapping State");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            playerState = State.Reset;
            Debug.Log("Reset State");
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            playerStateNum++;
        }



        switch (playerState)
        {
            case State.idle:

                if (agentHasStartedWalking == true)
                {
                    agentHasStartedWalking = false;

                    anim.SetFloat("Offset", Random.Range(0, 1));
                    anim.SetTrigger("Idle");
                    agentNavMesh.isStopped = true;
                }


                break;

            case State.walk
            :
                if (agentIsSitting == false)
                {
                    agentNavMesh.isStopped = false;

                    if (agentHasStartedWalking == false)
                    {
                        agentHasStartedWalking = true;

                        anim.SetFloat("Offset", Random.Range(0, 1));
                        anim.SetTrigger("Walk");

                        float animSpeed = Random.Range(0.8f, 1.3f);
                        anim.SetFloat("SpeedMultiplier", animSpeed);
                        agentNavMesh.speed *= (animSpeed + 0.5f);

                        agentNavMesh.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
                    }

                    if (agentNavMesh.remainingDistance < 0.1)
                    {
                        agentNavMesh.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
                    }
                }

                break;

            case State.look
            :
                if (agentIsSitting == false)
                {
                    this.transform.LookAt(christamsTree.transform);
                }

                break;


            case State.clap:
                anim.SetFloat("Offset", Random.Range(0, 1));
                int num = Random.Range(0, 2);

                if (num == 0) anim.SetTrigger("Clap"); 
                else anim.SetTrigger("Clap2");

                break;

            case State.Reset:
                agentNavMesh.speed = 0.1f;
                break;

            default:
                break;
        }

    }

}
