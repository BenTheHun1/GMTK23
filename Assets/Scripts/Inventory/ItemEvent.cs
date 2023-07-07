using UnityEngine;
using UnityEngine.EventSystems;


public class ItemEvent : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private GridPieceEvent gridParent;

    private void Start()
    {
        gridParent = transform.parent.GetComponent<GridPieceEvent>();
    }

    //the item image pieces use the same functions as the grid behind them
    public void OnPointerDown(PointerEventData eventData)
    {
        gridParent.OnPointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gridParent.OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridParent.OnPointerExit(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gridParent.OnPointerUp(eventData);
    }
}