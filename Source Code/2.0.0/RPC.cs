using System;
using System.Collections.Generic;
using System.Text;
using Hazel;
using Reactor;
using Reactor.Networking;

namespace DictatorReworked
{
    public enum CustomRpcCalls : uint
    {
        setDictate
    }
    [RegisterCustomRpc((uint)CustomRpcCalls.setDictate)]
    public class SetDictatedVote : PlayerCustomRpc<DictatorReworked, (bool, int)> //killer id, player id
    {
        public SetDictatedVote(DictatorReworked plugin, uint id) : base(plugin, id)
        { }
        public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;
        public override void Write(MessageWriter writer, (bool, int) data)
        {
            writer.Write(data.Item1);
        }
        public override (bool, int) Read(MessageReader reader)
        {
            bool item1 = reader.ReadBoolean();
            return (item1, 0);
        }
        public override void Handle(PlayerControl innerNetObject, (bool, int) data)
        {
            VotingPatch.isDictating = data.Item1;
        }
    }
}
