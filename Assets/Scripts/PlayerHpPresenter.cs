using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHpPresenter : MonoBehaviour
{
    //UI text
    [Header("Player Hp UI text")]
    [SerializeField] TextMeshProUGUI PlayerHpText;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<GameSession>().onHpChange += UpdateUI;

        FindObjectOfType<PlayerMortality>().onHpChange += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        //PlayerHpText.text = GetComponent<GameSession>().getRemainingHp().ToString();;
        PlayerHpText.text = FindObjectOfType<PlayerMortality>().getHp().ToString();
    }
}
