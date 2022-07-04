using System.Collections;
using UnityEngine;

namespace GridTest
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Letter : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private Vector2 _originPosition;
        private Vector2 _targetPosition;

        private Vector2 _originScale;
        private Vector2 _targetScale;

        private float _lerpDuration;
        private float _timeStamp;
        private bool _destroyInProgress;

        private Color _defaultColor = Color.black;

        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = Color.black;
        }

        public void SetSprite(Sprite sprite, Color? color = null)
        {
            _spriteRenderer.color = color ?? Color.black;
            _spriteRenderer.sprite = sprite;
        }

        private void Update()
        {
            if ((Vector2)transform.position != _targetPosition || (Vector2)transform.localScale != _targetScale)
            {
                var timeElapsed = _timeStamp - Time.time;
                if (timeElapsed > 0)
                {
                    transform.position = Vector2.Lerp(_targetPosition, _originPosition, timeElapsed / _lerpDuration);
                    transform.localScale = Vector2.Lerp(_targetScale, _originScale, timeElapsed / _lerpDuration);
                }
                else if (!_destroyInProgress)
                {
                    transform.position = _targetPosition;
                    transform.localScale = _targetScale;
                }
                else
                    Destroy(gameObject);

            }
        }
        public void SetTarget(Vector3 targetPosition, float targetScale, float duration, bool destroyAtTarget = false)
        {
            _targetPosition = targetPosition;
            _originPosition = transform.position;

            _originScale = transform.localScale;
            _targetScale = Vector2.one * targetScale;

            _lerpDuration = duration;
            _timeStamp = Time.time + duration;

            _destroyInProgress = destroyAtTarget;
        }
        public Vector2 GetTarget()
        {
            return _targetPosition;
        }
    }
}