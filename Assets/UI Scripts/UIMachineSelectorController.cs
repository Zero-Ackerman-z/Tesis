using System.Collections.Generic;
using UnityEngine;

public class UIMachineSelectorController : MonoBehaviour
{
    [Header("Machine Cards")]
    [SerializeField] private MachineCardController cardPrefab;
    [SerializeField] private Transform cardsParent;
    [SerializeField] private List<MachineData> machineDatas; // assign AC and DC SOs via Inspector
    [SerializeField] private MachineFactory factory;

    [Header("Info Panel")]
    [SerializeField] private UIMachineInfoController infoPanel;

    private List<MachineCardController> cards = new List<MachineCardController>();
    private MachineCardController expandedCard;

    private void Start()
    {
        PopulateCards();
    }

    private void PopulateCards()
    {
        // clean
        foreach (Transform t in cardsParent) Destroy(t.gameObject);
        cards.Clear();

        // create cards for each MachineData (we expect 2)
        foreach (var md in machineDatas)
        {
            var card = Instantiate(cardPrefab, cardsParent);
            card.Initialize(md, factory);
            card.OnCardSelected += HandleCardSelected;
            cards.Add(card);
        }
    }

    private void HandleCardSelected(MachineCardController card)
    {
        // collapse previous
        if (expandedCard != null && expandedCard != card)
        {
            expandedCard.Collapse();
        }

        // expand clicked
        expandedCard = card;
        expandedCard.Expand();

        // show info panel with the data
        infoPanel.ShowInfo(card.GetMachineData(), card);
    }

    private void OnDestroy()
    {
        foreach (var c in cards) c.OnCardSelected -= HandleCardSelected;
    }
}
