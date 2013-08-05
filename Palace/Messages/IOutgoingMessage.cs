namespace Palace.Messages
{
    public interface IOutgoingMessage : IClientMessage
    {
        void Write();
    }
}