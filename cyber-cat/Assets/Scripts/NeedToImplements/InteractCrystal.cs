using System;
using TaskUnits;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Legacy_do_not_use_it
{
    [Obsolete]
    public class InteractCrystal : MonoBehaviour
    {
        private SphereCollider _collider;

        [SerializeField] private TaskUnitFolder taskFolder;

        [SerializeField] private bool selfTriggerLogic = false;

        void Start()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.OnCollisionStayAsObservable().Subscribe(x => Debug.Log("123"));
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            var isHackModePressed = Input.GetKey(KeyCode.F);
            if (isHackModePressed && HackerVisionSingleton.Instance.Active)
            {
                //TokenSession token = PlayerPrefs.GetToken();
            }
        }
    }
}