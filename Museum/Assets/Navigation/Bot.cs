using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour {

    public BotRuntimeSet RuntimeSet;
    private NavMeshAgent _navMeshAgent;
    public NavigationController _navigationController;
    private ViewingLocation _destination;
    private bool _walking;
    private bool _leaving;
    private Animator animator;

    Transform mainCamTransform; 
    private Renderer[] rendererList;

    public ButtonClicked btnClicked;
    public bool autoUpdateFading = true;
    public float distanceToFadeMax = 5f;
    public float distanceToFadeMin = 3f;
    public float distanceToDisappear = 2f;
    public float currentOpacity = 1f;

    void OnEnable()
    {
        RuntimeSet.Add(this);
    }

    void OnDisable()
    {
        RuntimeSet.Remove(this);
    }

    void Awake()
    {
        _navigationController = FindObjectOfType<NavigationController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mainCamTransform = Camera.main.transform;
        rendererList = gameObject.GetComponentsInChildren<Renderer>();

        GameObject controller_canvas = GameObject.FindGameObjectWithTag("ControllerCanvas");
        btnClicked = controller_canvas.GetComponent<ButtonClicked>();
        btnClicked.UpdateValuesOnBots();

        StartWalking();
    }

    void Update()
    {
        animator.SetBool("walking", _walking);
        disappearChecker();
        if (_walking)
        {
            if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= 2)
            {
                _walking = false;
                StartCoroutine(Wait());
            }
        } else if (_leaving)
        {
            Destroy(gameObject);
        }
    }

    public void StartWalking(Vector3? location = null)
    {
        if (location == null)
        {
            _destination = _navigationController.getNewLocation(_destination);
            if (_destination.Area == 0) //Exit
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                _leaving = true;
            }

            float minX = _destination.transform.localPosition.x - _destination.transform.localScale.x / 2;
            float maxX = _destination.transform.localPosition.x + _destination.transform.localScale.x / 2;

            float minZ = _destination.transform.localPosition.z - _destination.transform.localScale.z / 2;
            float maxZ = _destination.transform.localPosition.z + _destination.transform.localScale.z / 2;
            location = new Vector3(Random.Range(minX, maxX), _destination.transform.localPosition.y, Random.Range(minZ, maxZ));
        }
        _navMeshAgent.SetDestination((Vector3)location);
        _walking = true;
    }

    private void disappearChecker()
    {
        float distance = Vector3.Distance(mainCamTransform.position, transform.position);

        float opacityDistance = distance - distanceToFadeMin;
        if(opacityDistance < 0)
        {
            opacityDistance = 0f;
        }

        float opacity = opacityDistance / distanceToFadeMax;
        if(opacity > 1)
        {
            opacity = 1f;
        } else if(opacity < 0)
        {
            opacity = 0f;
        }

        foreach (Renderer renderer in rendererList)
        {
            if(distance < distanceToDisappear)
            {
                renderer.enabled = false;
            } else
            {
                renderer.enabled = true;
            }
            
            if(opacity != currentOpacity)
            {
                renderer.material.color = new Color(1, 1, 1, opacity);
                currentOpacity = opacity;
            }
        }
    }

    public void toggleAutoUpdateFading()
    {
        autoUpdateFading = !autoUpdateFading;

        if (!autoUpdateFading)
        {
            distanceToFadeMax = 0f;
            distanceToFadeMin = 0f;
            distanceToDisappear = 0f;
        }

        btnClicked.UpdateValuesOnBots();
    }
    
    IEnumerator Wait()
    {
        float waitTime = Random.Range(5f, 15f);
        Debug.Log("Now: " + Time.time + ", Waiting for: " + waitTime);
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Waited until: " + Time.time);
        StartWalking();
    }
}
