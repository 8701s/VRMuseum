using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour
{
    public GameObject[] botsPrefab;
    public BotRuntimeSet bots;

    public void Start()
    {
        StartCoroutine(ManageBots());
    }

    public void AddBot()
    {
        if(Random.Range(0f, 1f) > 0.5)
        {
            Instantiate(botsPrefab[0], gameObject.transform);
        } else
        {
            Instantiate(botsPrefab[1], gameObject.transform);
        }
    }

    IEnumerator ManageBots()
    {
        float waitTime = Random.Range(5f, 15f);
        yield return new WaitForSeconds(waitTime);
        if(bots.Items.Count < 30)
        {
            AddBot();
        }
        StartCoroutine(ManageBots());
    }

}
