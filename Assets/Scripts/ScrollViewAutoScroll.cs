using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollViewAutoScroll : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Toggle[] _toggles;
    float scrollValue;
    private void Start()
    {
        if (_toggles == null || _toggles.Length == 0)
        {
            _toggles = GetComponentsInChildren<Toggle>();
        }
        foreach (Toggle toggle in _toggles)
        {
            EventTrigger trigger = toggle.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = toggle.gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entry.callback.AddListener((data) => { OnToggleSelected(toggle); });
            trigger.triggers.Add(entry);
        }
    }
    private void OnToggleSelected(Toggle selectedToggle)
    {
        int index = System.Array.IndexOf(_toggles, selectedToggle);
        if (index != -1)
        {
            scrollValue = 1f - ((float)index / (_toggles.Length - 1));
        }
    }
    private void LateUpdate()
    {
        _scrollbar.value = Mathf.Lerp(_scrollbar.value, scrollValue, 5 * Time.unscaledDeltaTime);
    }
}