using System.Collections;
using UnityEngine;
using TMPro;

namespace GridTest
{
    public class UIController : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private GridGenerator _gridGenerator;
        [Header("UI Elements")]
        [SerializeField] private TMP_InputField _widthField;
        [SerializeField] private TMP_InputField _heightField;
        [SerializeField] private RectTransform _playableArea;
        [Header("Options")]
        [SerializeField] private int _numberLimit;
        [Header("Play area margin in percent")]
        [SerializeField] [Range(0.0f, 1.0f)] private float _marginHorizontal;
        [SerializeField] [Range(0.0f, 1.0f)] private float _marginVertical;

        private Vector2 _lastPlayableAreaResolution;
        private Camera _camera;

        private void OnEnable()
        {
            _camera = Camera.main;
            ChangeBoxColliderSize();
        }

        public void GenerateGrid()
        {
            if (_gridGenerator == null)
                return;
            int width;
            int height;
            if (int.TryParse(_widthField?.text, out width) && int.TryParse(_heightField?.text, out height))
            {
                width = Mathf.Min(width, _numberLimit);
                height = Mathf.Min(height, _numberLimit);
                _gridGenerator.SetResolution(width, height);
                var actualResolution = _gridGenerator.GetResolution();
                _widthField.text = actualResolution.x.ToString();
                _heightField.text = actualResolution.y.ToString();
            }
        }
        public void ShuffleGrid()
        {
            if (_gridGenerator == null)
                return;
            _gridGenerator.ShuffleGrid();
        }
        public void ClearGrid()
        {
            if (_gridGenerator == null)
                return;
            _gridGenerator.ClearGrid();
        }

        private void Update()
        {
            if (_playableArea.sizeDelta != _lastPlayableAreaResolution)
            {
                ChangeBoxColliderSize();
            }
        }

        private void ChangeBoxColliderSize()
        {
            if (!_camera.orthographic)
            {
                Debug.LogError("Camera must be Orthographic.");
                return;
            }
            if (this._playableArea == null)
            {
                Debug.LogError("Playable area is not set");
                return;
            }

            _lastPlayableAreaResolution = _playableArea.sizeDelta;

            var corners = new Vector3[4];
            _playableArea.GetWorldCorners(corners);

            var min = _camera.ScreenToWorldPoint(corners[0]);
            var max = _camera.ScreenToWorldPoint(corners[2]);

            Vector2 marginScale = new Vector2((1.0f - _marginHorizontal), (1.0f - _marginVertical));

            Vector2 size = (max - min) * marginScale;
            Vector2 offset = (min * marginScale) + (size / 2.0f);

            _gridGenerator.SetBoxCollider(offset, size);
            _gridGenerator.MoveLetters(0);
        }
    }
}