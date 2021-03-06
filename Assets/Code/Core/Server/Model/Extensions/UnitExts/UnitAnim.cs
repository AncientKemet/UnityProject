#if SERVER
using Server.Model.Entities;

namespace Server.Model.Extensions.UnitExts
{

    public class UnitAnim : UnitUpdateExt
    {
        private bool _stand;
        private string _standAnimation;
        private bool _walk;
        private string _walkAnimation;
        private bool _run;
        private string _runAnimation;
        private bool _action;
        private string _actionAnimation;

        private ServerUnit _lookingAt;

        public ServerUnit LookingAt
        {
            get { return _lookingAt; }
            set {
                _lookingAt = value;
                _wasUpdate = true;
            }
        }

        public string StandAnimation
        {
            get { return _standAnimation; }
            set
            {
                _standAnimation = value;
                _wasUpdate = true;
                _stand = true;
            }
        }

        public string WalkAnimation
        {
            get { return _walkAnimation; }
            set
            {
                _walkAnimation = value;
                _wasUpdate = true;
                _walk = true;
            }
        }

        public string RunAnimation
        {
            get { return _runAnimation; }
            set
            {
                _runAnimation = value;
                _wasUpdate = true;
                _run = true;
            }
        }

        public string ActionAnimation
        {
            get { return _actionAnimation; }
            set
            {
                _actionAnimation = value;
                _wasUpdate = true;
                _action = true;
            }
        }

        public override byte UpdateFlag()
        {
            return 0x08;
        }

        protected override void pSerializeState(Code.Code.Libaries.Net.ByteStream packet)
        {
            packet.addFlag(new []{true,true,true,false});
            packet.addString(StandAnimation);
            packet.addString(WalkAnimation);
            packet.addString(RunAnimation);
            packet.addShort(_lookingAt == null ? -1 : _lookingAt.ID);
        }

        protected override void pSerializeUpdate(Code.Code.Libaries.Net.ByteStream packet)
        {

            packet.addFlag( _stand, _walk, _run, _action);

            if (_stand)
                packet.addString(StandAnimation);
            if (_walk)
                packet.addString(WalkAnimation);
            if (_run)
                packet.addString(RunAnimation);
            if (_action)
                packet.addString(ActionAnimation);

            packet.addShort(_lookingAt == null ? -1 : _lookingAt.ID);

            _stand = false;
            _walk = false;
            _run = false;
            _action = false;
        }

        protected override void OnExtensionWasAdded()
        {
            base.OnExtensionWasAdded();

            StandAnimation = "Idle";
            WalkAnimation = "Walk";
            RunAnimation = "Run";
        }
    }
}
#endif
