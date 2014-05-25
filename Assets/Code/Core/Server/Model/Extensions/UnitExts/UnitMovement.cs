using UnityEngine;

namespace OldBlood.Code.Core.Server.Model.Extensions.UnitExts
{

    public class UnitMovement : UnitUpdateExt
    {
        //how fast does force fadeout?
        private const float ForceFade = 0.8f;

        //important variables
        private Vector3 _position = Vector3.zero;
        private float _rotation = 0;

        private Vector3 _force = Vector3.zero;

        //other variables
        private MovementState currentState = new MovementState();
        private bool _positionUpdate = false;
        private bool _rotationUpdate = false;

        //property getters
        public Vector3 Position
        {
            get { return _position; }
            private set { 
                _position = value;
                _positionUpdate = true;
            }
        }

        public Vector3 Force
        {
            get { return _force; }
        }

        public float Rotation
        {
            get { return _rotation; }
            private set { 
                _rotation = value;
                _rotationUpdate = true;
            }
        }

        public bool CanMove
        {
            get { return !currentState.stunned; }
        }

        public bool CanRotate
        {
            get { return !currentState.stunned; }
        }


        //Methods
        public override void Progress()
        {
            base.Progress();
            _force *= ForceFade;
        }

        public void MoveTo(Vector3 newPosition)
        {
            if (CanMove)
            {
                _force += new Vector3(newPosition.x - _position.x, newPosition.y - _position.y,
                    newPosition.z - _position.z);
                Position = newPosition;
                _wasUpdate = true;
            }
        }

        public void RotateTo(float newRotation)
        {
            if (CanRotate)
            {
                Rotation = newRotation;
                _wasUpdate = true;
            }
        }

        //Update flag
        public override byte UpdateFlag()
        {
            return 0x01;
        }

        //write to packet


        protected override void pSerializeState(Libaries.Net.ByteStream packet)
        {
            packet.addFlag(true, true);
            packet.addFloat4B(_position.x);
            packet.addFloat4B(_position.y);
            packet.addFloat4B(_position.z);

            packet.addFloat4B(_rotation);
        }

        protected override void pSerializeUpdate(Libaries.Net.ByteStream packet)
        {
            packet.addFlag(_positionUpdate, _rotationUpdate);
            if (_positionUpdate)
            {
                packet.addFloat4B(_position.x);
                packet.addFloat4B(_position.y);
                packet.addFloat4B(_position.z);
            }
            if (_rotationUpdate)
            {
                packet.addFloat4B(_rotation);
            }
        }
    }

    internal class MovementState
    {
        public bool stunned = false;
    }
}
