using Mirror;

namespace Entities
{
    public struct RemovePlayerMassage : NetworkMessage
    {
        public UnitId UnitId;
    }
}