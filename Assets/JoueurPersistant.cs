using UnityEngine;

public class JoueurPersistant : MonoBehaviour
{
    public static JoueurPersistant Instance;

    void Awake()
    {
        // PATTERN SINGLETON (Il ne doit en rester qu'un)
        if (Instance == null)
        {
            // Si c'est le premier joueur créé, on le garde
            Instance = this;
            DontDestroyOnLoad(gameObject); // <--- C'est ici que la magie opère
        }
        else
        {
            // Si on charge une scène qui contient DÉJÀ un joueur par défaut,
            // alors que nous arrivons avec notre joueur immortel,
            // on détruit l'imposteur (celui de la scène).
            Destroy(gameObject);
        }
    }
}