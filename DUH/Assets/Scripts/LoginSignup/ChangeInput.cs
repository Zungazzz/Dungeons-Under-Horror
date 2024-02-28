using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNavigation : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public Button submitButton;

    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
        if (firstInput != null)
        {
            firstInput.Select();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (system.currentSelectedGameObject != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Selectable previous = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                    if (previous != null)
                    {
                        previous.Select();
                    }
                }
                else
                {
                    Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                    if (next != null)
                    {
                        next.Select();
                    }
                }
            }
            else
            {
                if (firstInput != null)
                {
                    firstInput.Select();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (submitButton != null)
            {
                submitButton.onClick.Invoke();
                Debug.Log("Submit");
            }
        }
    }
}
