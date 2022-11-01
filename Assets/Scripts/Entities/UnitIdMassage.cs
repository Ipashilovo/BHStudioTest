using Mirror;

namespace Entities
{
    public struct UnitIdMassage : NetworkMessage
    {
        public UnitId Id;
    }

    public struct ScoreAddMassage : NetworkMessage
    {
        public UnitId Id;
    }
}