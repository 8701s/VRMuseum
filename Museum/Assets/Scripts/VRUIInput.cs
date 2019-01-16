using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_LaserPointer))]
public class VRUIInput : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointer;
    private SteamVR_TrackedController trackedController;

    private CapsuleCollider cur_collider;


    private void OnEnable()
    {
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut += HandlePointerOut;
        laserPointer.PointerIn += ColliderIn;
        laserPointer.PointerOut += ColliderOut;

        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;
    }

    private void OnDisable()
    {
        laserPointer.PointerOut -= ColliderOut;
        laserPointer.PointerIn -= ColliderIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerIn -= HandlePointerIn;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        if (cur_collider != null)
        {
            Debug.Log("Triger on Collider");

            GameObject bot_gameobject = cur_collider.gameObject;
            Bot bot = bot_gameobject.GetComponent<Bot>();
            bot.toggleAutoUpdateFading();
        }
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            button.Select();
            Debug.Log("HandlePointerIn", e.target.gameObject);
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {

        var button = e.target.GetComponent<Button>();
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log("HandlePointerOut", e.target.gameObject);
        }
    }

    private void ColliderIn(object sender, PointerEventArgs e)
    {
        var collider = e.target.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            cur_collider = collider;

            Debug.Log("Auf Model", e.target.gameObject);
        }
    }

    private void ColliderOut(object sender, PointerEventArgs e)
    {

        var collider = e.target.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            if (cur_collider == collider)
            {
                cur_collider = null;
            }

            Debug.Log("HandlePointerOut", e.target.gameObject);
        }
    }
}