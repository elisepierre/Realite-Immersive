using UnityEngine;
using System.Collections;

public class TestBlessingSpawner : MonoBehaviour
{
    public GameObject slowBlessingPickup;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(10f);

        if (slowBlessingPickup != null)
        {
            slowBlessingPickup.transform.position =
                GameObject.FindWithTag("MainCamera").transform.position;
        }
    }
}
