using Code.Libaries.Net.Packets.ForServer;

namespace Code.Core.Server.Model.Extensions.PlayerExtensions.UIHelpers
{

    public enum InterfaceIDs
    {
        AcionBars = 1,
    }

    public class ClientUI : EntityExtension
    {
        public override void Progress()
        {
        }

        public void OnUIEvent(UIInterfaceEvent e)
        {
            if (e.interfaceId == (int)InterfaceIDs.AcionBars)
            {

            }
        }
    }
}

