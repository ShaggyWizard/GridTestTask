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
        [Header("Options")]
        [SerializeField] private int _numberLimit;

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
    }
}