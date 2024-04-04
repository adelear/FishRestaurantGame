using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraChangeManager : MonoBehaviour
{
    public Transform[] cameras;
    public Button leftButton;
    public Button rightButton;

    private bool canSwitchCamera = true;
    public int currentRoom = 0;

    public static CameraChangeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        AddPointerEnterListener(leftButton, () => SwitchRoom(currentRoom - 1));
        AddPointerEnterListener(rightButton, () => SwitchRoom(currentRoom + 1)); 
    }

    private void AddPointerEnterListener(Button button, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    private void SwitchRoom(int targetRoom)
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.GAME) return; 
        if (!canSwitchCamera || targetRoom < 0 || targetRoom >= cameras.Length)
            return;

        Camera.main.transform.position = cameras[targetRoom].position;
        currentRoom = targetRoom;
        canSwitchCamera = false;
        
        switch (currentRoom)
        {
            case 0:
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(true);
                break;
            case 1:
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);
                break;
            case 2:
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(false);
                break;
            default:
                break; 
        }

        StartCoroutine(EnableCameraSwitch());
    }

    private IEnumerator EnableCameraSwitch()
    {
        yield return new WaitForSeconds(0.2f);
        canSwitchCamera = true;
    }
}
