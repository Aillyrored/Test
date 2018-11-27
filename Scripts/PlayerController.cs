using UnityEngine.EventSystems;
using UnityEngine;

 

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public Interactable focus;	// Our current focus: Item, Enemy etc.
	public LayerMask movementMask;  // Filter out everything not walkable
    Camera cam;			 
	PlayerMotor motor;	
    
    bool nearObj = false;
    public GameObject Torch;
    public GameObject UseFire;
    public GameObject EquipItem;
    int NumberOfItem;


 
    void Start() {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        //  rend = GameObject.FindGameObjectWithTag("Woods").GetComponent<Renderer>();
        UseFire = GameObject.Find("BurnImage");
        UseFire.SetActive(false);
        Torch.SetActive(false);
     

    }
    int range = 4;
    public void ClickEffect()
    {
        EquipItem.SetActive(true);
        NumberOfItem = 1;
    }
    void OnDrawGizmosSelected()
    { 
      //  Gizmos.color = Color.blue;
      //  Gizmos.DrawSphere(transform.position, range);
    }
    void FindClosestEnemy()
    {
      
        float distanceToClosestEnemy = Mathf.Infinity;
     
        Obstacle closestEnemy = null;
        Obstacle[] allEnemies = GameObject.FindObjectsOfType<Obstacle>();
      
        foreach (Obstacle currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;

            if (distanceToEnemy < range)
            {
    
                Debug.Log(distanceToEnemy);
               // nearObj = true;
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
           else if (distanceToEnemy > range)
            {
                UseFire.SetActive(false);
            }
         }
        
    }
    
    void Update () {

        if (EventSystem.current.IsPointerOverGameObject())
            return;
    
       
            if (Input.GetMouseButtonDown(0))
		{
			// We create a ray
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			// If the ray hits
			if (Physics.Raycast(ray, out hit, 100, movementMask))
			{
				motor.MoveToPoint(hit.point);   
				RemoveFocus();
			}
            RaycastHit hitInfo = new RaycastHit();
            bool hits = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hits)
            {
    
                if (hitInfo.transform.gameObject.tag == "Woods")
                {
                    FindClosestEnemy();
                
                }
                else
                {

                }
            }
            else
            {
               
            }

        }
      
	 
		if (Input.GetMouseButtonDown(1))
		{
 
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
 
			if (Physics.Raycast(ray, out hit, 100))
			{
				Interactable interactable = hit.collider.GetComponent<Interactable>();
				if (interactable != null)
				{
					SetFocus(interactable);
				}
			}
		}
	}
    public void UseSkill()
    {
        nearObj = true;
        UseFire.SetActive(false);
        // FindClosestEnemy();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Torch" && NumberOfItem == 0)
        {
            Torch.SetActive(true);
        }
        if (other.gameObject.tag == "Woods" && NumberOfItem == 1)
        {
            UseFire.SetActive(true);
        }
        if (nearObj == true)
        {
            if (other.gameObject.tag == "Woods" && NumberOfItem == 1)
            {
                Debug.Log("ok!");
                UseFire.SetActive(false);
                other.transform.gameObject.GetComponent<Obstacle>().useeffect();
                FindClosestEnemy();
            }   
                nearObj = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Torch" && NumberOfItem == 0)
        {
            Torch.SetActive(true);
        }
        if (other.gameObject.tag == "Torch" && NumberOfItem == 1)
        {
            Torch.SetActive(false);
        }
   
        if (nearObj == true)
        {
            if (other.gameObject.tag == "Woods" && NumberOfItem == 1 )
            {

                Debug.Log("colliding!");
                UseFire.SetActive(false);
                other.transform.gameObject.GetComponent<Obstacle>().useeffect();
                //other.transform.gameObject.GetComponent<Obstacle>().createCube();
                FindClosestEnemy();
            }

            nearObj = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Torch")
        {
            Torch.SetActive(false);
        }
        if (other.gameObject.tag == "Woods")
        {
            UseFire.SetActive(false);
        }
        if (other.gameObject.tag == "Woods")
        {
            UseFire.SetActive(false);
        }
    }
    // Set our focus to a new focus
    void SetFocus (Interactable newFocus)
	{
		if (newFocus != focus)
		{
		 
			if (focus != null)
				focus.OnDefocused();

			focus = newFocus;	 
			motor.FollowTarget(newFocus);	 
		}
		
		newFocus.OnFocused(transform);
	}
	// Remove our current focus
	void RemoveFocus ()
	{
		if (focus != null)
			focus.OnDefocused();

		focus = null;
		motor.StopFollowingTarget();
	}
}
