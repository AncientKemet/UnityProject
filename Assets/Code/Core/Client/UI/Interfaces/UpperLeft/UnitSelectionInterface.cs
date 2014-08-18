using System.Collections;
using System.Collections.Generic;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Core.Client.UI.Controls;
using Code.Core.Client.Units;
using Code.Libaries.Net.Packets.InGame;
using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;

namespace Code.Code.Libaries.Net.Packets
{
    public class UnitSelectionPacketData : BasePacket
    {
        public string UnitName;
        public float Armor, MagicResist, MovementSpeed, Mobility, CooldownSpeed;


        public bool HasCombat { get; set; }

        public bool HasAttributes { get; set; }

        protected override int GetOpCode()
        {
            return 23;
        }

        protected override void enSerialize(ByteStream bytestream)
        {
            bytestream.addString(UnitName);

            bytestream.addFlag(HasCombat,HasAttributes);

            bytestream.addFloat4B(Armor);
            bytestream.addFloat4B(MagicResist);
            bytestream.addFloat4B(MovementSpeed);
            bytestream.addFloat4B(Mobility);
            bytestream.addFloat4B(CooldownSpeed);
        }

        protected override void deSerialize(ByteStream bytestream)
        {
            UnitName = bytestream.getString();

            BitArray mask = bytestream.GetBitArray();

            HasCombat = mask[0];
            HasAttributes = mask[1];

            Armor = bytestream.getFloat4B();
            MagicResist = bytestream.getFloat4B();
            MovementSpeed = bytestream.getFloat4B();
            Mobility = bytestream.getFloat4B();
            CooldownSpeed = bytestream.getFloat4B();
        }
    }
}

namespace Code.Core.Client.UI.Interfaces.UpperLeft
{
    public class UnitSelectionInterface : UIInterface<UnitSelectionInterface>
    {
        public System.Action<PlayerUnit> OnUnitWasSelected;

        [SerializeField]
        private List<Animation> Animations = new List<Animation>(); 

        private PlayerUnit _unit;

        [SerializeField]
        private tk2dTextMesh UnitNameLabel, Armor, MagicResist, MovementSpeed, CooldownSpeed;

        [SerializeField] private ChannelBar _healthBar, _energyBar;

        [SerializeField] private GameObject HPWing, AttributesPanel, DescriptionWing;

        protected override float AnimSpeed
        {
            get { return 0.2f; }
        }

        public PlayerUnit Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != null && _unit != value)
                {
                    _unit.Projector.gameObject.SetActive(false);
                }
                if (value != null)
                    value.Projector.gameObject.SetActive(true);
                if (OnUnitWasSelected != null)
                    OnUnitWasSelected(value);
                _unit = value;
                
                
            }
        }

        void Start()
        {
            OnUnitWasSelected += ShowUnit;
            Unit = null;
        }

        private void ShowUnit(PlayerUnit u)
        {
            if (Unit != u)
            {
                var packet = new TargetUpdatePacket();
                packet.UnitId = u == null ? -1 : u.Id;
                ClientCommunicator.Instance.SendToServer(packet);
            }
            if (u == null)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (_unit == null) Unit = _unit;

            if(Unit == null) return;
            _healthBar.Progress = Unit.Health / 100f;
            _energyBar.Progress = Unit.Energy / 100f;
        }

        public void OnDataRecieved(UnitSelectionPacketData data)
        {
            AttributesPanel.SetActive(data.HasAttributes);
            HPWing.SetActive(data.HasCombat);

            SetText(UnitNameLabel, data.UnitName);
            SetText(Armor, data.Armor);
            SetText(MagicResist, data.MagicResist);
            SetText(MovementSpeed, data.MovementSpeed);
            SetText(CooldownSpeed, data.Mobility);

            if (Unit != null)
                Unit.Name = data.UnitName;
        }

        private void SetText(tk2dTextMesh mesh, string s)
        {
            mesh.text = s;
            mesh.ForceBuild();
        }

        private void SetText(tk2dTextMesh mesh, float s)
        {
            mesh.text = ""+s;
            mesh.ForceBuild();
        }

        public override void Hide()
        {
            Visible = false;
            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.zero,
                    delegate(Vector3 vector3)
                    {
                        if (!Visible)
                            transform.localScale = vector3;
                    },
                    delegate
                    {
                        if (!Visible)
                            gameObject.SetActive(false);
                    },
                    AnimSpeed
                    )
                );

        }

        public override void Show()
        {
            Visible = true;
            gameObject.SetActive(true);

            StopAllCoroutines();
            foreach (var a in Animations)
            {
                if (a.name == "Wing")
                {
                    a.gameObject.SetActive(false);
                    a.Stop();
                }
            }

            StartCoroutine(PlayAnimations());

            CorotineManager.Instance.StartCoroutine(
                Ease.Vector(
                    transform.localScale,
                    Vector3.one,
                    delegate(Vector3 vector3)
                    {
                        if (Visible)
                            transform.localScale = vector3;
                    },
                    delegate
                    {
                        
                    },
                    AnimSpeed
                    )
                );
        }

        private IEnumerator PlayAnimations()
        {
            foreach (var a in Animations)
            {
                a.gameObject.SetActive(true);
                a.Play();
                yield return new WaitForSeconds(a.clip.length * 0.5f);
            }
        }
    }
}
