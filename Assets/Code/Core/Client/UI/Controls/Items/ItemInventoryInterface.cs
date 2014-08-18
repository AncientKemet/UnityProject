﻿using System;
using System.Collections.Generic;
using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Units;
using Code.Core.Client.Units.Managed;
using Code.Libaries.UnityExtensions;
using UnityEngine;

namespace Code.Core.Client.UI.Controls.Items
{
    public class ItemInventoryInterface : UIInterface<ItemInventoryInterface>
    {
        public tk2dTextMesh Title;
        public BoundsFittingSlicedSprite SlicedSprite;

        public ItemInventory ItemInventory;

        public void SetInventorySize(int width, int height)
        {
            ItemInventory.Width = width;
            ItemInventory.Height = height;
            ItemInventory.ForceRebuild();

            SlicedSprite.RecalculateBounds();
        }

        #region static
        private static Dictionary<int, ItemInventoryInterface> _activeInterfaces = new Dictionary<int, ItemInventoryInterface>();

        public static ItemInventoryInterface GetInterface(int unitId)
        {
            if (_activeInterfaces.ContainsKey(unitId) && _activeInterfaces[unitId] != null)
            {
                return _activeInterfaces[unitId];
            }
            else
            {
                return CreateInterface(UnitManager.Instance[unitId]);
            }
        }

        private static ItemInventoryInterface CreateInterface(PlayerUnit unit)
        {
            if (unit == null)
            {
                throw  new Exception("null unit inventory request");
            }

            Vector3 unitPosition = unit.transform.position;
            Vector3 unitPosition2D = tk2dUIManager.Instance.UICamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(unitPosition));

            ItemInventoryInterface newInterface = (Instantiate(I.gameObject) as GameObject).GetComponent<ItemInventoryInterface>();

            newInterface.transform.position = unitPosition2D;
            newInterface.transform.localScale = Vector3.zero;

            newInterface.Title.text = unit.Name;
            newInterface.Title.ForceBuild();

            newInterface.Show();

            if (!_activeInterfaces.ContainsKey(unit.Id))
                _activeInterfaces.Add(unit.Id, newInterface);
            else
                _activeInterfaces[unit.Id] = newInterface;

            return newInterface;
        }
        #endregion

        protected override void Awake()
        {
            if (I == null)
                I = this;
        }

        public void Handle(UIInventoryInterfacePacket packet)
        {
            ItemInventoryInterface instance = GetInterface(packet.UnitID);
            switch (packet.type)
            {
                    case UIInventoryInterfacePacket.PacketType.SHOW:
                    instance.ItemInventory.Width = packet.X;
                    instance.ItemInventory.Height = packet.Y;
                    instance.ItemInventory.ForceRebuild();
                    break;

                    case UIInventoryInterfacePacket.PacketType.HIDE:
                    instance.Hide();
                    break;

                    case UIInventoryInterfacePacket.PacketType.SetItem:
                    int itemID = packet.Value;
                    instance.ItemInventory.SetItem(packet.X,packet.Y,itemID);
                    break;
            }
        }
    }
}
