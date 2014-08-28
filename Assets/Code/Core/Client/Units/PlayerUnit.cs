using System;
using System.Collections;
using Code.Code.Libaries.Net;
using Code.Core.Client.UI.Controls;
using Code.Core.Client.UI.Interfaces;
using Code.Core.Client.UI.Interfaces.UpperLeft;
using Code.Core.Client.Units.Extensions;
using Code.Core.Client.Units.Managed;
using Code.Core.Client.Units.UnitControllers;
using Code.Core.Shared.Content.Types;
using Code.Libaries.Generic.Managers;
using Code.Libaries.Net.Packets.ForClient;
using Code.Libaries.UnityExtensions.Independent;
using UnityEngine;

namespace Code.Core.Client.Units
{
    [RequireComponent(typeof(UnitDisplay))]
    public class PlayerUnit : Clickable
    {
        [SerializeField]
        private int _id = -1;

        private UnitDisplay _display;

        private static PlayerUnit _myPlayerUnitInstance;

        [SerializeField]
        private tk2dTextMesh _2dNameLabel;

        private string _name;

        [SerializeField]
        protected float _basemovementSpeed = 1;

        [SerializeField]
        private float _baseTurnSpeed = 1;

        protected Vector3 movementTargetPosition;
        protected Vector3 smoothedTargetPosition;
        protected float targetRotation;

        public Projector Projector;

        [SerializeField]
        protected float distanceToTarget;

        [SerializeField]
        protected float _visualSpeed;

        public float Health { get; private set; }
        public float Energy { get; private set; }
        
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public static PlayerUnit MyPlayerUnit
        {
            get { return _myPlayerUnitInstance; }
            set { _myPlayerUnitInstance = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        /// <value>The current movement speed.</value>
        public float CurrentMovementSpeed
        {
            get
            {
                return _basemovementSpeed;
            }
        }

        /// <summary>
        /// Gets or sets the movement target DirecionVector.
        /// The value has to be in parent's local space;
        /// </summary>
        /// <value>The movement target DirecionVector.</value>
        public Vector3 MovementTargetPosition
        {
            get
            {
                return movementTargetPosition;
            }
            set
            {
                movementTargetPosition = value;
            }
        }

        public float VisualSpeed
        {
            get
            {
                return _visualSpeed;
            }
        }

        public float TargetRotation
        {
            get { return targetRotation; }
            set { targetRotation = value; }
        }

        public UnitDisplay Display
        {
            get
            {
                if (_display == null)
                    _display = GetComponent<UnitDisplay>();
                return _display;
            }
        }

        protected override void Start()
        {
            base.Start();

            if (Projector != null)
            {
                Projector.gameObject.SetActive(false);
                Projector.material = (Material) Instantiate(Projector.material);
            }

            OnLeftClick += delegate
            {
                if (UnitSelectionInterface.I.Unit != this)
                    UnitSelectionInterface.I.Unit = this;
                else
                {
                    if(this == null)
                        return;
                    
                    if (Actions[0].Action != null)
                    {
                        Actions[0].Action();
                        GameObject effect = (GameObject) Instantiate((ContentManager.I.Effects[0]));
                        effect.transform.parent = transform;
                        effect.transform.localPosition = Vector3.zero;
                    }
                }
            };

            OnRightClick += () =>
            {
                if (this == null)
                    return;
                if (UnitSelectionInterface.I.Unit == this)
                {
                    OpenRightClickMenu();
                }
            };

            /*
            OnMouseIn += delegate
            {
                Projector.gameObject.SetActive(true);
            };
            OnMouseOff += delegate
            {
                if (UnitSelectionInterface.I.Unit != this)
                        Projector.gameObject.SetActive(false);
            };*/

            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }

        void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected virtual void OnStart()
        {
            MovementTargetPosition = transform.localPosition;
        }

        protected virtual void OnUpdate()
        {
            distanceToTarget = Vector2.Distance(new Vector2(transform.localPosition.x, transform.localPosition.z), new Vector2(movementTargetPosition.x, movementTargetPosition.z));
            _visualSpeed = Mathf.Clamp(distanceToTarget, 0f, _basemovementSpeed) ;

            smoothedTargetPosition = Vector3.Lerp(smoothedTargetPosition, movementTargetPosition, 0.3f);

            Vector3 calculatedPosition = transform.localPosition;

            if (distanceToTarget > 0.017f)
            {// Process DirecionVector
                //calculatedPosition = Vector3.MoveTowards(calculatedPosition, smoothedTargetPosition, Time.deltaTime * 500f);
                calculatedPosition = Vector3.Lerp(calculatedPosition, smoothedTargetPosition, Time.deltaTime * 5f);

                FixYOnTerrain(ref calculatedPosition);
                transform.localPosition = calculatedPosition;
            }

            Quaternion calculatedRotation;

            {// Process rotation

                if (Mathf.Abs(targetRotation - transform.eulerAngles.y) > 5f)
                {
                    calculatedRotation = Quaternion.Euler(new Vector3(0, TargetRotation, 0));
                    calculatedRotation.x = 0;
                    calculatedRotation.z = 0;

                    calculatedRotation = Quaternion.Lerp(transform.rotation, calculatedRotation, 0.25f);
                    transform.rotation = calculatedRotation;
                }
            }
        }

        protected virtual void OnFixedUpdate()
        { }

        public void DecodeUnitUpdate(UnitUpdatePacket p)
        {
            ByteStream b = p.SubPacketData;
            int mask = b.getByte();

            BitArray bitArray = new BitArray(new[] { mask });

            bool movementUpdate = bitArray[0];
            bool displayUpdate = bitArray[1];
            bool combatUpdate = bitArray[2];
            bool animUpdate = bitArray[3];
#if DEBUG_NETWORK
            string log = "";
            log += "\n" + "Packet size " + b.GetSize();

            log += "\n" + "nMovementUpdate " + movementUpdate;
            log += "\n" + "ndisplayUpdate " + displayUpdate;
            log += "\n" + "combatUpdate " + combatUpdate;
            log += "\n" + "animUpdate " + animUpdate;
            Debug.Log(log);
#endif

            if (movementUpdate)
            {
                int mask2 = b.getByte();
                bitArray = new BitArray(new[] { mask2 });

                bool positionUpdate = bitArray[0];
                bool rotationUpdate = bitArray[1];
                bool teleported = bitArray[2];

                if (positionUpdate)
                {
                    Vector3 pos = b.getPosition6B();
                    if (!teleported)
                    {
                        MovementTargetPosition = pos;
                    }
                    else
                    {
                        MovementTargetPosition = pos;
                        FixYOnTerrain(ref pos);
                        transform.localPosition = pos;
                    }
                }

                if (rotationUpdate)
                {
                    float rotation = b.getFloat4B();
                    TargetRotation = rotation;
                }

#if DEBUG_NETWORK
                log = "";
                log += "\n" + "post movement offset " + b.Offset;
                log += "\n" + "positionUpdate " + positionUpdate;
                log += "\n" + "rotationUpdate " + rotationUpdate;
                Debug.Log(log);
#endif
            }

            if (displayUpdate)
            {
                var displayMask = b.GetBitArray();
                int modelId = b.getUnsignedByte();

                bool isItem = displayMask[0];
                bool wasDestroyed = displayMask[1];

                if (!isItem)
                {
                    if (Display != null)
                        Display.Model = modelId;
                }
                else
                {
                    Item item = ((GameObject) Instantiate(ContentManager.I.Items[modelId].gameObject)).GetComponent<Item>();
                    transform.localPosition += Vector3.up;
                    item.transform.parent = transform;
                    item.transform.localPosition = Vector3.zero;
                    if(collider != null)
                        collider.enabled = false;
                }

                if (wasDestroyed)
                {
                    var destroyMask = b.GetBitArray();

                    var wasPickuped = destroyMask[0];

                    if (wasPickuped)
                    {
                        int unitID = b.getUnsignedShort();
                        PlayerUnit u = UnitManager.Instance[unitID];

                        if (u != null)
                        {
                            GetComponentInChildren<Item>().EnterUnit(u);
                            GetComponentInChildren<Item>().transform.parent = null;
                        }
                        else
                        {
                            Debug.Log("null unit id: "+unitID);
                        }
                    }
                    Destroy(gameObject);
                }

#if DEBUG_NETWORK
                log = "";
                log += "\n" + "post display offset " + b.Offset;
                log += "\n" + "modelId " + modelId;
                log += "\n" + "isItem " + isItem;
                Debug.Log(log);
#endif
            }

            if (combatUpdate)
            {
                int health = b.getUnsignedByte();
                int energy = b.getUnsignedByte();

                Health = health;
                Energy = energy;

                var combat = GetComponent<CombatUnit>();

                if (this == MyPlayerUnit)
                {
                    var channelBar = StatsBarInterfaces.I.Controls[0] as ChannelBar;
                    if (channelBar != null)
                        channelBar.Progress = health / 100f;
                    var bar = StatsBarInterfaces.I.Controls[1] as ChannelBar;
                    if (bar != null)
                        bar.Progress = energy / 100f;
                }
                if (combat != null)
                {
                    combat.SetHealth(health);
                    combat.SetEnergy(energy);
                }

#if DEBUG_NETWORK
                log = "";
                log += "\n" + "post combat offset " + b.Offset;
                log += "\n" + "health " + health;
                log += "\n" + "energy " + energy;
                Debug.Log(log);
#endif
            }

            if (animUpdate)
            {
                int mask2 = b.getByte();

                var bitArray2 = new BitArray(new[] { mask2 });

#if DEBUG_NETWORK
                log = "";
                log += "\n" + "pre anim offset " + b.Offset;
                log += "\n" + "bitArray2 " + bitArray2;
                Debug.Log(log);
#endif

                if (bitArray2[0])
                {
                    string a = b.getString();
                    if (Display != null)
                        Display.StandAnimation = a;
                }
                if (bitArray2[1])
                {
                    string a = b.getString();
                    if (Display != null)
                        Display.WalkAnimation = a;
                }
                if (bitArray2[2])
                {
                    string a = b.getString();
                    if (Display != null)
                        Display.RunAnimation = a;
                }
                if (bitArray2[3])
                {
                    string a = b.getString();
                    if (Display != null)
                        Display.ActionAnimation = a;
                }

                int lookingAtUnitID = b.getShort();

                Display.LookAtUnit = lookingAtUnitID == -1 ? null : UnitManager.Instance.GetUnit(lookingAtUnitID);
            }

        }

        public static bool GetBit(int byt, int index)
        {
            if (index < 0 || index > 7)
                throw new ArgumentOutOfRangeException();

            int shift = 7 - index;

            // Get a single bit in the proper DirecionVector.
            byte bitMask = (byte)(1 << shift);

            // Mask out the appropriate bit.
            byte masked = (byte)(byt & bitMask);

            // If masked != 0, then the masked out bit is 1.
            // Otherwise, masked will be 0.
            return masked != 0;
        }

        protected void FixYOnTerrain(ref Vector3 position)
        {
            Ray ray = new Ray(position + new Vector3(0, 50, 0), Vector3.down);
            RaycastHit hit = new RaycastHit();
            int layerMask = 1 << 8;
            //layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                position.y = hit.point.y;
            }
        }
    }
}
