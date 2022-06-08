using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("UI")]
    public RectTransform rectTransform;
    public Text AmoutText;
    public Image image;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public int locateSlotId;
    //[HideInInspector] public bool canDrag=false;

    private void Awake()
    {
        InitUI();
    }

    #region Drag and Drop
    private void InitUI()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0)||Inventory.instance.InventoryData.Data[locateSlotId].amount==1)
        {
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;
        }
        else if (Input.GetMouseButton(1))
        {
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;

            AmoutText.text="";
            Inventory.instance.RightMouseButtonDragItem(locateSlotId);
            Inventory.instance.slots[locateSlotId].isFull=false;
        }

        Inventory.instance.draggedItem=this;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (transform.parent == canvas.transform)
            ComeBackToSlot();
    }

    public void ComeBackToSlot()
    {
        Slot lastSlot = Inventory.instance.slots[locateSlotId];
        transform.SetParent(lastSlot.transform);
        rectTransform.anchoredPosition3D = Vector3.zero;
    }
    #endregion Drag and Drop

    public void SetData(Canvas canvas, Item item)
    {
        transform.SetParent(Inventory.instance.slots[item.slotId].transform);
        rectTransform.anchoredPosition = Vector3.zero;
        rectTransform.localScale=Vector3.one;
        this.canvas = canvas;
        locateSlotId = item.slotId;

        if (item.amount > 1) AmoutText.text = item.amount.ToString();
        image.sprite = Resources.Load<Sprite>($"Image/{item.itemName}");
    }



}
