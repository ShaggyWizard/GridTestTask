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

        public void GenerateGrid()
        {
            int width;
            int height;
            if (int.TryParse(_widthField.text, out width) && int.TryParse(_heightField.text, out height))
                _gridGenerator.SetResolution(width, height);
        }
        public void ShuffleGrid()
        {
            _gridGenerator.ShuffleGrid();
        }
    }
}