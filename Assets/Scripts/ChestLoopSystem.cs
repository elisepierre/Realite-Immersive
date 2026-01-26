using UnityEngine;
using UnityEngine.EventSystems;
public class ChestLootSystem : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuration UI")]
    public GameObject lootCanvas;
    public Transform spawnPoint;
    public GameObject helpText;

    [Header("Modèles du Coffre")]
    public GameObject chestClosedModel;
    public GameObject chestOpenModel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;

    [Header("Les Armes")]
    public GameObject weaponPrefab1;
    public GameObject weaponPrefab2;

    private bool isOpened = false;

    void Start()
    {
        if (chestClosedModel != null) chestClosedModel.SetActive(true);
        if (chestOpenModel != null) chestOpenModel.SetActive(false);
        if (lootCanvas != null) lootCanvas.SetActive(false);

        if (helpText != null) helpText.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenChest();
    }

    public void OpenChest()
    {
        if (!isOpened)
        {
            isOpened = true;

            if (chestClosedModel != null) chestClosedModel.SetActive(false);
            if (chestOpenModel != null) chestOpenModel.SetActive(true);

            if (lootCanvas != null) lootCanvas.SetActive(true);
            if (helpText != null) helpText.SetActive(false);

            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }

            if (MusicManager.instance != null)
                MusicManager.instance.MuffleMusic(true);

            Debug.Log("Coffre ouvert via le Laser !");
        }
    }

    public void SpawnWeapon1() { SpawnAndClose(weaponPrefab1); }
    public void SpawnWeapon2() { SpawnAndClose(weaponPrefab2); }

    private void SpawnAndClose(GameObject weaponToSpawn)
    {
        if (weaponToSpawn != null && spawnPoint != null)
        {
            Instantiate(weaponToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        if (lootCanvas != null) lootCanvas.SetActive(false);

        if (MusicManager.instance != null)
            MusicManager.instance.MuffleMusic(false);
    }
}