using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardSystem : MonoBehaviour
{
    [Header("Configuration UI")]
    public GameObject rewardCanvas;
    public Transform playerHead;
    public float distanceFromPlayer = 1.0f;

    [Header("Les 3 Boutons")]
    public Button button1;
    public Button button2;
    public Button button3;

    [Header("Textes des Boutons (Optionnel)")]
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;

    public enum BuffType { Soin, Vitesse, Force }
    private BuffType buff1, buff2, buff3;

    void Start()
    {
        if (rewardCanvas != null) rewardCanvas.SetActive(false);
    }

    public void ShowRewards()
    {
        if (rewardCanvas == null) return;

        rewardCanvas.SetActive(true);

        PositionMenuInFrontOfPlayer();

        buff1 = GetRandomBuff();
        buff2 = GetRandomBuff();
        buff3 = GetRandomBuff();

        UpdateButtonVisuals();
    }

    void PositionMenuInFrontOfPlayer()
    {
        Vector3 targetPosition = playerHead.position + (playerHead.forward * distanceFromPlayer);
        targetPosition.y = playerHead.position.y;

        rewardCanvas.transform.position = targetPosition;

        rewardCanvas.transform.LookAt(2 * rewardCanvas.transform.position - playerHead.position);
    }

    BuffType GetRandomBuff()
    {
        return (BuffType)Random.Range(0, 3);
    }

    void UpdateButtonVisuals()
    {
        if (text1) text1.text = buff1.ToString();
        if (text2) text2.text = buff2.ToString();
        if (text3) text3.text = buff3.ToString();
    }


    public void OnClickButton1() { ApplyBuff(buff1); }
    public void OnClickButton2() { ApplyBuff(buff2); }
    public void OnClickButton3() { ApplyBuff(buff3); }

    void ApplyBuff(BuffType buff)
    {
        Debug.Log("Bonus activé : " + buff);

        switch (buff)
        {
            case BuffType.Soin:
                // Code pour soigner le joueur
                break;
            case BuffType.Vitesse:
                // Code pour accélérer le joueur
                break;
            case BuffType.Force:
                // Code pour augmenter les dégâts
                break;
        }

        rewardCanvas.SetActive(false);
    }
}
