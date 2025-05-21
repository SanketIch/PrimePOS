namespace Evertech.Interface
{
    interface IDeviceComm
    {
        bool Connect();
        bool SendMessage(byte[] message);
        //event MessageSentEventHandler OnMessageSent;
    }
   
}
