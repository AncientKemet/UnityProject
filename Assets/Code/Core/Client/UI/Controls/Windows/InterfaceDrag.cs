using UnityEngine;

namespace Code.Core.Client.UI.Controls.Windows
{
    [RequireComponent(typeof(tk2dUIDragItem))]
    public class InterfaceDrag : MonoBehaviour
    {
        public InterfaceManager.UIInterface Interface;

        private tk2dUIDragItem _dragItem;
        private Vector3 _localOffset;
        private float InitialZ;

        void Start ()
        {
            _dragItem = GetComponent<tk2dUIDragItem>();
            _localOffset = transform.position - Interface.transform.position;
            InitialZ = Interface.transform.position.z;
            _dragItem.OnUpdate += UpdateIterfacePosition;
        }

        void UpdateIterfacePosition()
        {
            Vector3 tar = (transform.position - _localOffset - _dragItem.Offset);
            tar.z = InitialZ;
            Interface.transform.position = tar;
            transform.localPosition = _localOffset + _dragItem.Offset;
        }
    }
}
