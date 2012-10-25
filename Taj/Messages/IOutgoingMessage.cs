namespace Taj.Messages
{
    public interface IOutgoingMessage : IClientMessage
    {
        void Write();
    }
}