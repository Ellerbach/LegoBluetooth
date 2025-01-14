using LegoBluetooth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoderHelper
{
    public class MockBluetooth : IBluetooth
    {
        private List<byte[]> _toPlay;

        public IBluetooth.ProcessIncomingHandler ProcessIncoming { get; set; }
        public IBluetooth.ClientJoiningStateChangedHandler ClientJoiningStateChanged { get; set; }

        public bool IsConnected => true;

        public event IBluetooth.OnErrorHandler OnError;

        public MockBluetooth(List<string> toPlay)
        {
            _toPlay = new List<byte[]>();
            toPlay.ForEach(data =>
            {
                byte[] bytes = data.Split('-').Select(s => Convert.ToByte(s, 16)).ToArray();
                _toPlay.Add(bytes);
            });
        }

        public MockBluetooth(List<byte[]> toPlay)
        {
            _toPlay = toPlay;
        }

        public bool Connect()
        {
            _toPlay.ForEach(data =>
            {
                ProcessIncoming(data);
            });
            return true;
        }

        public bool Disconnect()
        {
            return true;
        }

        public bool NotifyValue(byte[] data)
        {
            Console.WriteLine($"<- {BitConverter.ToString(data)}");
            return true;
        }
    }
}
