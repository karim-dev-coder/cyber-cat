using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[RequireComponent(typeof(SphereCollider))]
public class Trigger : MonoBehaviour
{
    private Player _player;
    private ModalPanel _modalPanel;
    private bool _activated;

    public Player Player => _player;
    public ModalPanel ModalPanel => _modalPanel;

    [SerializeField] Transform _transformOfTrigger;
    [SerializeField] ModalInfo _modalInfo;

    [SerializeField] private int _countOfModals;
    [SerializeField] private ModalInfo[] _modalInfos;
    [SerializeField] private TriggerType _triggerType;
    [SerializeField] private EventType _eventType;
    [SerializeField] private KeyCode[] _keyCodes;

    private UnityEvent _entered;
    [SerializeField] private UnityEvent _onEnter;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_transformOfTrigger != null)
        {
            transform.position = _transformOfTrigger.position;
        }
        
        _activated = false;
        _player = GameObject.FindObjectOfType<Player>();
        _modalPanel = ModalPanel.Instance;
        //_entered.AddListener(Activate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activated || _triggerType != TriggerType.Enter || !other.TryGetComponent<Player>(out _player))
        {
            return;
        }
        _entered?.Invoke();

        Do();
    }

    private void Update()
    {
        foreach (var t in _keyCodes)
        {
            if (!_activated && Input.GetKeyDown(t))
            {
                Do();
            }
        }
    }

    private void Do()
    {
        Activate();
        switch (_eventType)
        {
            case EventType.EnterEvent:
                _onEnter?.Invoke();
                break;
            case EventType.Message:
                ShowMessage();
                break;
        }
    }

    private void ShowMessage()
    {
        _modalPanel.MessageBos(_modalInfos);
    }

    private void Activate()
    {
        _activated = true;
    }

    public enum TriggerType
    {
        Enter = 0,
        ButtonPressed = 1
    }


    public enum EventType
    {
        Message = 0,
        EnterEvent = 1,
    }
}
