using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Libaries.Net.Packets.ForServer;
using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;

namespace Code.Core.Client.UI.Controls
{
    public class CircleButton : InterfaceButton
    {
        public KeyCode HotKey = KeyCode.Return;

        public tk2dSprite Circle, BackGround, Icon;

        private Color _originalCircleColor;
        private bool _isRotating = false;

        [SerializeField]
        private bool _canBeHeldDown = false;

        protected override void Start()
        {
            base.Start();

            _originalCircleColor = Circle.color;

            if (_canBeHeldDown)
            {
                OnLeftDown += ScaleDown;
                OnLeftDown += SendButtonDown;
                OnLeftClick += ScaleBack;
                OnLeftClick += RotateCircle;
            }
            else
            {
                OnLeftClick += RotateCircle;
                OnLeftClick += ScaleDownAndBack;
            }
            OnMouseIn += Highlight;
            OnMouseOff += Dehighlight;
        }

        private void SendButtonDown()
        {
            UIInterfaceEvent p = new UIInterfaceEvent();

            p.interfaceId = InterfaceId;
            p.controlID = Index;

            p._eventType = UIInterfaceEvent.EventType.Button_Down;

            ClientCommunicator.Instance.SendToServer(p);
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(HotKey))
            {
                if (_canBeHeldDown)
                {
                    if (OnLeftDown != null)
                    {
                        OnLeftDown();
                    }
                }
                else if (OnLeftClick != null) OnLeftClick();
            }

            if (Input.GetKeyUp(HotKey))
            {
                if (_canBeHeldDown)
                    if (OnLeftClick != null) OnLeftClick();
            }
        }

        private void Dehighlight()
        {
            Circle.color = _originalCircleColor;
            Circle.ForceBuild();
        }

        private void Highlight()
        {
            Circle.color = Color.white;
            Circle.ForceBuild();
        }

        private void RotateCircle()
        {
            if (!_isRotating)
            {
                _isRotating = true;

                CorotineManager.Instance.StartCoroutine(
                    Ease.Vector(
                        Circle.transform.eulerAngles,
                        new Vector3(0, 0, 180f),
                        delegate(Vector3 vector3)
                        {
                            Circle.transform.localEulerAngles = vector3;
                        },
                        delegate
                        {
                            CorotineManager.Instance.StartCoroutine(
                                Ease.Vector(
                                    Circle.transform.eulerAngles,
                                    new Vector3(0, 0, 0),
                                    delegate(Vector3 vector3)
                                    {
                                        Circle.transform.localEulerAngles = vector3;
                                    },
                                    delegate
                                    {
                                        _isRotating = false;
                                    },
                                    0.15f
                                    )
                                );
                        },
                        0.15f
                        )
                    );

            }
        }

        private void ScaleDownAndBack()
        {
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one * 0.7f,
                    delegate(Vector3 vector3) { transform.localScale = vector3; },
                    delegate
                    {
                        CorotineManager.Instance.StartCoroutine(
                            Ease.Vector(
                                transform.localScale,
                                Vector3.one,
                                delegate(Vector3 vector3) { transform.localScale = vector3; },
                                delegate { },
                                0.15f
                                )
                            );
                    },
                    0.15f
                    )
                );
        }

        private void ScaleDown()
        {
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one * 0.7f,
                    delegate(Vector3 vector3) { transform.localScale = vector3; },
                    delegate
                    {
                    },
                    0.15f
                    )
                );
        }

        private void ScaleBack()
        {
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one,
                    delegate(Vector3 vector3) { transform.localScale = vector3; },
                    delegate
                    {
                    },
                    0.15f
                    )
                );
        }
    }
}
