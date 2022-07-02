using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridTest
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Grid : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private float _size;
        private int _width; 
        private int _height;

        private void OnEnable()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }
        public void SetResolution(int width, int height)
        {
            _width = width > 0 ? width : 1;
            _height = height > 0 ? height : 1;
            _size = Mathf.Min(_boxCollider2D.size.x / _width, _boxCollider2D.size.y / _height);
        }
        public Vector2Int GetResolution()
        {
            return new Vector2Int(_width, _height);
        }
        public float GetCellScale()
        {
            return _size;
        }
        public Vector2 GetCellPosition(Vector2Int cell)
        {
            return GetCellPosition(cell.x, cell.y);
        }
        public Vector2 GetCellPosition(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                Debug.LogError($"[{x};{y}] is out of range");
                return Vector2.zero;
            }
            return (new Vector2(x - (_width - 1) / 2.0f, y - (_height - 1) / 2.0f)) * _size;
        }
    }
}