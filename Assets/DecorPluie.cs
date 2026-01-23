using UnityEngine;

public class DecorPluie : MonoBehaviour
{
    public GameObject meteoritePrefab;
    public float intervalleApparition = 0.5f; // Vitesse de la pluie
    public float rayonZone = 20f; // Taille de la zone de pluie
    public float hauteurChute = 15f;

    private float chrono;

    void Update()
    {
        chrono += Time.deltaTime;

        if (chrono >= intervalleApparition)
        {
            FaireTomberUneMeteorite();
            chrono = 0;
        }
    }

    void FaireTomberUneMeteorite()
    {
        if (meteoritePrefab == null) return;

        // Position aleatoire dans un cercle
        Vector2 cercle = Random.insideUnitCircle * rayonZone;
        Vector3 pos = transform.position + new Vector3(cercle.x, hauteurChute, cercle.y);

        // On cree la meteorite
        Instantiate(meteoritePrefab, pos, Quaternion.identity);
    }
}