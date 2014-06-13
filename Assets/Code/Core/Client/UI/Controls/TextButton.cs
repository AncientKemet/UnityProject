using UnityEngine;

namespace Code.Core.Client.UI.Controls
{
    [RequireComponent(typeof(Clickable))]
    public class TextButton : MonoBehaviour
    {
        [SerializeField] 
        public Color HoverColor;

        [SerializeField]
        private tk2dTextMesh _textMesh;
        [SerializeField]
        private tk2dSlicedSprite _backGround;

        private string _text;
        private Color _normalColor;

        public string Text 
        {
            get { return _text; }
            set
            {
                _text = value;
                _textMesh.text = value ?? "";
                _textMesh.ForceBuild();
                _backGround.dimensions = new Vector2(Width * 20, _backGround.dimensions.y);
                _backGround.ForceBuild();
            } 
        }

        public float Width
        {
            get { return _textMesh.renderer.bounds.size.x; }
            set
            {
                _backGround.dimensions = new Vector2(value * 20, _backGround.dimensions.y);
                _backGround.ForceBuild();
            }
        }

        private void OnMouseEnter()
        {
            _backGround.color = HoverColor;
            _backGround.ForceBuild();
        }

        private void OnMouseExit()
        {
            _backGround.color = _normalColor;
            _backGround.ForceBuild();
        }

        private void Start()
        {
            _normalColor = _backGround.color;
        }
    }
}
