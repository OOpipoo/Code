using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Player
{
    public class CableInstallationTimer : MonoBehaviour
    {
        private Image _image;
        private float _duration;
        private float _remainingDuration;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            gameObject.SetActive(false);
        }

        private void LateUpdate() => 
            transform.rotation = Quaternion.Euler(Vector3.zero);

        public void StartTimer(float duration)
        {
            gameObject.SetActive(true);
            StartCoroutine(UpdateTimer(duration));
        }

        private IEnumerator UpdateTimer(float duration)
        {
            var endTime = Time.time + duration;
            while (Time.time < endTime)
            {
                _image.fillAmount = Mathf.Lerp(1, 0, (endTime - Time.time) / duration);
                yield return null;
            }
            gameObject.SetActive(false);
        }

        public void Stop()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
}
