using Code.Code.Libaries.Net.Packets;
using Code.Core.Client.Net;
using Code.Libaries.Net.Packets.ForServer;
using UnityEngine;

namespace Code.Core.Client.UI.Controls
{
    public class CircleButton : InterfaceButton
    {
        public tk2dSprite Circle, BackGround, Icon;

        private Color originalCircleColor;

        protected override void Start()
        {
            base.Start();

            originalCircleColor = Circle.color;

            OnMouseIn += Highlight;
            OnMouseOff += Dehighlight;
        }

        private void Dehighlight()
        {
            Circle.color = originalCircleColor;
            Circle.ForceBuild();
        }

        private void Highlight()
        {
            Circle.color = Color.white;
            Circle.ForceBuild();
        }
   }
}
