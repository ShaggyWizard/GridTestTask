using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

namespace GridTest
{
    [RequireComponent(typeof(Grid))]
    [RequireComponent(typeof(SpriteRandomizer))]
    public class GridGenerator : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] [Range(0, 5.0f)] private float _generateTime;
        [SerializeField] [Range(0, 5.0f)] private float _shuffleTime;
        [SerializeField] private int _lettersLimit;
        [SerializeField] private GridShuffler.ShuffleType _shuffleType;
        [SerializeField] private Transform _createTransform;
        [SerializeField] private Transform _destroyTransform;
        [Header("Prefabs")]
        [SerializeField] private Letter _letterPrefab;

        private Grid _grid;
        private SpriteRandomizer _spriteRandomizer;

        private Letter[,] _letters;
        private Vector2 _createPoint;
        private Vector2 _destroyPoint;

        private void OnEnable()
        {
            _grid = GetComponent<Grid>();
            _spriteRandomizer = GetComponent<SpriteRandomizer>();
            _createPoint = _createTransform != null ? _createTransform.position : Vector2.zero;
            _destroyPoint = _destroyTransform != null ? _destroyTransform.position : Vector2.zero;
        }
        public void SetBoxCollider(Vector2 offset, Vector2 size)
        {
            _grid.SetBoxCollider(offset, size);
        }

        public void SetResolution(int width, int height)
        {
            int count = width * height;

            if (count > _lettersLimit)
            {
                float scale = Mathf.Sqrt((float)_lettersLimit / count);
                width = Mathf.FloorToInt(width * scale);
                height = Mathf.FloorToInt(height * scale);
            }

            _grid.SetResolution(width, height);
            FillGrid();
            MoveLetters(_generateTime);
        }
        public Vector2Int GetResolution()
        {
            return _grid.GetResolution();
        }

        public void ShuffleGrid()
        {
            if (_letters == null)
                return;
            GridShuffler.Shuffle(_letters, _shuffleType);
            MoveLetters(_shuffleTime);
        }

        public void FillGrid()
        {
            var oldLetters = _letters;
            var oldResolution = _letters != null ? new Vector2Int(oldLetters.GetLength(0), oldLetters.GetLength(1)) : Vector2Int.zero;
            var newResolution = _grid.GetResolution();
            var maxResolution = Vector2Int.Max(oldResolution, newResolution);

            _letters = new Letter[newResolution.x, newResolution.y];

            for (int x = 0; x < maxResolution.x; x++)
            {
                for (int y = 0; y < maxResolution.y; y++)
                {
                    if (x >= newResolution.x || y >= newResolution.y)
                    {
                        //Trim unsued letters
                        if (x < oldResolution.x && y < oldResolution.y)
                            oldLetters[x, y].SetTarget(_destroyPoint, 0, _generateTime, true);
                        continue;
                    }

                    if (x < oldResolution.x && y < oldResolution.y)
                    {
                        //Reuse old letters
                        _letters[x, y] = oldLetters[x, y];
                    }
                    else
                    {
                        Letter letter = _letters[x, y] = Instantiate(_letterPrefab, transform);
                        var sprite = _spriteRandomizer.GetRandomSprite();
                        letter.SetSprite(sprite, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
                        letter.gameObject.name = sprite.name;
                        letter.transform.position = _createPoint;
                    }
                }
            }
        }
        public void ClearGrid()
        {
            if (_letters == null)
                return;
            foreach (var letter in _letters)
            {
                letter.SetTarget(_destroyPoint, 0, _generateTime, true);
            }
            _letters = null;
        }
        public void MoveLetters(float time)
        {
            var resolution = _grid.GetResolution();

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    _letters[x, y].SetTarget(_grid.GetCellPosition(x, y), _grid.GetCellScale(), time);
                }
            }
        }
    }
}