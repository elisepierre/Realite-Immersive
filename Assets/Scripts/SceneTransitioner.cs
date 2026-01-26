using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitioner : MonoBehaviour
{
    public void LoadRoom1()
    {
        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Room1");

        while (!operation.isDone)
        {
            yield return null;
        }

   
        yield return new WaitForEndOfFrame();
        var xrOrigin = Object.FindFirstObjectByType<Unity.XR.CoreUtils.XROrigin>();

        GameObject target = GameObject.Find("Target0");

        if (xrOrigin != null && target != null)
        {
            xrOrigin.transform.position = target.transform.position;
            xrOrigin.transform.rotation = target.transform.rotation;
            Debug.Log("Positionnement réussi !");
        }
        else
        {
            if (xrOrigin == null) Debug.LogError("ERREUR : Aucun XR Origin trouvé dans Room1 !");
            if (target == null) Debug.LogError("ERREUR : Aucun objet Target0 trouvé dans Room1 !");
        }
    }
}