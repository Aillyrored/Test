using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshModifierVolume))]
public class Obstacle : MonoBehaviour {

    float timer = 0.0f;  
    float timerMax = 3.0f;  
    bool Timers = false;
    public float Level_Room;
    public static Obstacle instace = null;
    public Renderer rend;
    public GameObject effect;
    public AudioSource audio;
    public GameObject Obstacles;
    NavMeshSurface surface;
    Vector3 OBJ_position;
    Vector3 Fireposition;
    NavMeshModifierVolume volume;
    
 
    public static Obstacle Instance()
    {
        if (instace == null)
        {
            instace = new Obstacle();
        }
        return instace;
    }
 

    public void useeffect()
    {
        Fireposition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1.1f, gameObject.transform.position.z);
        effect.transform.position = Fireposition;
        effect.SetActive(true);
        audio.Play();
        Timers = true;

    }
   
    private void Awake()
    {
        effect = GameObject.Find("BurnWoods"); 
    }
  
    void Start()
    {
        volume = GetComponent<NavMeshModifierVolume>();
        rend = GetComponent<Renderer>();
        audio = GameObject.Find("BurnAudio").GetComponent<AudioSource>();
        Obstacles = this.gameObject;
        if (effect)
        {
            effect.SetActive(false);
        }
        else
            print("nope");
    
    }
    void OnMouseOver()
    {
        if (gameObject.tag == "Woods")
        {
            rend.material.SetFloat("_Outline", 0.40f);
            print("over");
        }
    }
    void OnMouseEnter()
    {
        if (gameObject.tag == "Woods")
        {
            rend.material.SetFloat("_Outline", 0.40f);
            print("enter");
        }
    }
    void OnMouseExit()
    {
        if (gameObject.tag == "Woods")
        {
            rend.material.SetFloat("_Outline", 0.0f);
            print("enter");
        }
    }

    void Update () {
        if (Timers == true)
        {
           
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                Debug.Log("timerMax reached !");
                Timers = false;
                timer = 0;
                effect.SetActive(false);
                Obstacles.SetActive(false);
                surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
                surface.BuildNavMesh();
                audio.Stop();
            }
        }
    }
}
