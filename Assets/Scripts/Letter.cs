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

        private void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(Sprite sprite)
        {
            SetSprite(sprite, Color.black);
        }
        public void SetSprite(Sprite sprite, Color color)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.color = color;
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