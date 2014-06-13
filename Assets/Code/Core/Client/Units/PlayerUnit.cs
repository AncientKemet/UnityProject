using System;
using System.Collections;
using Code.Code.Libaries.Net;
using Code.Core.Client.Units.Extensions;
using Code.Core.Client.Units.UnitControllers;
using Code.Libaries.Generic.Managers;
using Code.Libaries.Net.Packets.ForClient;
using UnityEngine;

namespace Code.Core.Client.Units
{
    [RequireComponent(typeof(UnitDisplay))]
    public class PlayerUnit : MonoBehaviour
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

        [SerializeField]
        protected float distanceToTarget;

        [SerializeField]
        protected float _visualSpeed;

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
                _2dNameLabel.text = value;
                _2dNameLabel.ForceBuild();
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

        void Start()
        {
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
            _visualSpeed = Mathf.Clamp(distanceToTarget, 0f, _basemovementSpeed) * (Time.deltaTime * 10);

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

            if (movementUpdate)
            {
                int mask2 = b.getByte();
                bitArray = new BitArray(new[] { mask2 });

                bool positionUpdate = bitArray[0];
                bool rotationUpdate = bitArray[1];

                if (positionUpdate)
                {
                    Vector3 pos = b.getPosition6B();
                    MovementTargetPosition = pos;
                    Display.lookAtPosition = pos;
                }

                if (rotationUpdate)
                {
                    float rotation = b.getFloat4B();
                    TargetRotation = rotation;
                }
            }

            if (displayUpdate)
            {
                int modelId = b.getUnsignedByte();
                Display.SetModel((GameObject)Instantiate( ContentManager.I.Models[modelId]));
            }

            if (combatUpdate)
            {
                int health = b.getUnsignedShort();
                int energy = b.getUnsignedShort();

                var combat = GetComponent<CombatUnit>();
                if (combat != null)
                {
                    combat.SetHealth(health);
                    combat.SetEnergy(energy);
                }
            }

        }
        // Assume 0 is the MSB andd 7 is the LSB.
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
            int layerMask = 1 << 7;
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                position.y = hit.point.y;
            }
        }
    }
}
