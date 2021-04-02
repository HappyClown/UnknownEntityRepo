using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, ISelectHandler, IPointerEnterHandler {
    public RectTransform highlightObject;
    public float yOffset;
    public RectTransform myRectTransform;

    // On mouse hover(highlight).
    public void OnPointerEnter(PointerEventData eventData) {
        // Clear selected UI object.
        EventSystem.current.SetSelectedGameObject(null);
        // Select this UI object.
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
    // When selected.
    public void OnSelect(BaseEventData eventData) {
        // Move the Select object image to the Selected button.
        highlightObject.position = new Vector2(myRectTransform.position.x, myRectTransform.position.y+yOffset);
    }
}
