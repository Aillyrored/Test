using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

	Transform target;	
	NavMeshAgent agent;	
    public GameObject PlayerPosition;
    public GameObject[] Levels;
    bool up=false;
    bool startPosition=false;
    Vector3 pos;
    Vector3 pointVector;
    public PassRooms rooms;

    private void Awake()
    {
        rooms = GetComponent<PassRooms>();
    }
   
    void Start ()
    {
		agent = GetComponent<NavMeshAgent>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "up")
        {      
                pointVector = new Vector3(-2.75f, 0, -0.3f);
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 0f, 0));
                agent.isStopped = true;
                agent.ResetPath();
                other.gameObject.name = "Object";
                rooms = GameObject.Find("Object").GetComponent<PassRooms>();
                Debug.Log("rooms.Number_Room=" + rooms.Number_Room);
                for (int i = 0; i <= rooms.Number_Room; i++)
                {
                    // Debug.Log("rooms.Number_Room=" + rooms.Number_Room);
                    if (rooms.Number_Room == i)
                    {
                        foreach (GameObject Rooms in Levels)
                        {
                            Rooms.SetActive(false);
                            Levels[i].SetActive(true);
                            other.gameObject.name = "Way";
                        }
                        up = true;

                    }
                    //  Debug.Log("i" + i);
              
            }
        }
        if (other.gameObject.tag == "Left")
        {
            pointVector = new Vector3(0, 0, -2.5f);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 90f, 0));
            agent.isStopped = true;
            agent.ResetPath();
            other.gameObject.name = "Object";
            rooms = GameObject.Find("Object").GetComponent<PassRooms>();
            Debug.Log("rooms.Number_Room=" + rooms.Number_Room);
            for (int i = 0; i <= rooms.Number_Room; i++)
            {
                if (rooms.Number_Room == i)
                {
                    foreach (GameObject Rooms in Levels)
                    {
                        Rooms.SetActive(false);
                        Levels[i].SetActive(true);
                        other.gameObject.name = "Way";
                    }
                    up = true;

                }
            }
        }
        if (other.gameObject.tag == "Right")
        {
            Level1_Secret lvl1;
            lvl1 = GetComponent<Level1_Secret>();
            // lvl1.changeRoom();
            
            pointVector = new Vector3(0, 0, 2.75f);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 90f, 0));
            other.gameObject.name = "Object";
            agent.isStopped = true;
            agent.ResetPath();
            rooms = GameObject.Find("Object").GetComponent<PassRooms>();

            for (int i = 0; i <= rooms.Number_Room; i++)
            {
                // Debug.Log("rooms.Number_Room=" + rooms.Number_Room);
                if (rooms.Number_Room == i)
                {
                    foreach (GameObject Rooms in Levels)
                    {
                        Rooms.SetActive(false);
                        Levels[i].SetActive(true);
                        other.gameObject.name = "Way";
                    }
                    up = true;

                }
            }
            lvl1.Rooms();

        }
        if (other.gameObject.tag == "Down")
        {
            pointVector = new Vector3(2.5f, 0, -0.3f);
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, 90f, 0));
            other.gameObject.name = "Object";
            agent.isStopped = true;
            agent.ResetPath();
            rooms = GameObject.Find("Object").GetComponent<PassRooms>();
            
            for (int i = 0; i <= rooms.Number_Room; i++)
            {
                if (rooms.Number_Room == i)
                {
                    foreach (GameObject Rooms in Levels)
                    {
                        Rooms.SetActive(false);
                        Levels[i].SetActive(true);
                        other.gameObject.name = "Way";
                    }
                    up = true;

                }
            }
        }
    }
 
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.5f);
    }
    IEnumerator W8()
    {
        yield return new WaitForSeconds(2.5f);
    }

    void Update ()
	{

        if(startPosition==true)
        {
            PlayerPosition.transform.position = pointVector;
            agent.isStopped=true;
            agent.ResetPath();
            W8();
            startPosition = false;
        }
    
        if (up == false)
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
                FaceTarget();
            }
        }
        if (true == up)
        {
            PlayerPosition.transform.position = pointVector;
            agent.isStopped=true;
            agent.ResetPath();
            Wait();
            up = false;
        }
    }
	
  
	public void MoveToPoint (Vector3 point)
	{
		agent.SetDestination(point);
	}

 
	public void FollowTarget (Interactable newTarget)
	{
		agent.stoppingDistance = newTarget.radius * .8f;
		agent.updateRotation = false;

		target = newTarget.interactionTransform;
	}

	public void StopFollowingTarget ()
	{
		agent.stoppingDistance = 0f;
		agent.updateRotation = true;
		target = null;
	}


	void FaceTarget ()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

}
