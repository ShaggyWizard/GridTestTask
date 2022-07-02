using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

namespace GridTest
{
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private string _spritePath;
        private Sprite[] _sprites;
        private int _spritesCount;
        private void OnEnable()
        {
            _sprites = Resources.LoadAll<Sprite>(_spritePath);
            _spritesCount = _sprites.Length;
        }

        public Sprite GetRandomSprite()
        {
            return _sprites[Random.Range(0, _spritesCount)];
        }
    }
}