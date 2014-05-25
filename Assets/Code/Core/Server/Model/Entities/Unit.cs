using System;
using System.Collections.Generic;
using OldBlood.Code.Core.Server.Model.Extensions;
using OldBlood.Code.Core.Server.Model.Extensions.UnitExts;
using OldBlood.Code.Libaries.Net;
using OldBlood.Code.Libaries.Net.Packets;
using UnityEngine;

namespace OldBlood.Code.Core.Server.Model.Entities
{
    public class Unit : WorldEntity, IQuadTreeObject
    {
        private readonly UnitMovement _movement;
        private readonly List<UnitUpdateExt> _updateExtensions = new List<UnitUpdateExt>();

        public override World CurrentWorld
        {
            get { return base.CurrentWorld; }
            set
            {
                if (base.CurrentWorld != null)
                {
                    if (CurrentBranch != null)
                    {
                        CurrentBranch.RemoveObject(this);
                    }
                }
                base.CurrentWorld = value;
                value.Tree.AddObject(this);
            }
        }

        public Unit()
        {
            AddExt(_movement = new UnitMovement());
            AddExt(new UnitDisplay());

            //in the end find all updatable extensions
            foreach (EntityExtension extension in Extensions)
            {
                if (extension is UnitUpdateExt)
                {
                    _updateExtensions.Add(extension as UnitUpdateExt);
                }
            }
        }

        public override void Progress()
        {
            base.Progress();

            ReCreateUpdatePacket();

            if (_updatePacket != null)
            {
                SendPacketToPlayersAround(_updatePacket);
            }

        }

        private void SendPacketToPlayersAround(BasePacket packet)
        {
            foreach (IQuadTreeObject objectAround in CurrentBranch.ObjectsVisible)
            {
                if(objectAround == this)
                    return;
                
                Player playerAround = objectAround as Player;
                if (playerAround != null)
                {
                    playerAround.client.ConnectionHandler.SendPacket(packet);
                }
            }
        }

        #region QUAD TREE IMPL
        private Libaries.Generic.Trees.QuadTree _currentBranch;
        private Vector2 _quadTreePos = Vector2.zero;
        public Libaries.Generic.Trees.QuadTree CurrentBranch
        {
            get { return _currentBranch; }
            set { _currentBranch = value; }
        }


        public Vector2 GetPosition()
        {
            _quadTreePos = new Vector2(_movement.Position.x, _movement.Position.z);
            return _quadTreePos;
        }

        public Vector2 PositionChange()
        {
            return new Vector2(_movement.Position.x, _movement.Position.z) - _quadTreePos;
        }
        #endregion QUAD TREE IMPL

        #region UPDATE_PACKET
        private UnitUpdatePacket _updatePacket;

        private void ReCreateUpdatePacket()
        {

            _updatePacket = null;
            //Crate mask
            int mask = 0;
            foreach (UnitUpdateExt updateExtension in _updateExtensions)
            {
                if (updateExtension.WasUpdate())
                {
                    mask = mask | updateExtension.UpdateFlag();
                }
            }

            if (mask == 0)
                return;

            _updatePacket = new UnitUpdatePacket();

            //add mask
            _updatePacket.SubPacketData.addByte(mask);

            //serialize the rest of the packet
            foreach (UnitUpdateExt updateExtension in _updateExtensions)
            {
                if (updateExtension.WasUpdate())
                {
                    updateExtension.SerializeUpdate(_updatePacket.SubPacketData);
                }
            }

        }

        #endregion

    }
}

