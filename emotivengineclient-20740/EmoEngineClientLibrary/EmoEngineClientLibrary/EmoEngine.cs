using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EmoEngineClientLibrary
{
    public class EmoEngine
    {
        const int _historySize = 64;

        private FixedSizedQueue<double> _chCounter = new FixedSizedQueue<double> { Limit = _historySize };

        private FixedSizedQueue<double> _ch4 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch5 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch6 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch7 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch8 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch9 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch10 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch11 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch12 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch13 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch14 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch15 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch16 = new FixedSizedQueue<double> { Limit = _historySize };
        private FixedSizedQueue<double> _ch17 = new FixedSizedQueue<double> { Limit = _historySize };

        private Socket _socket;
        private static EmoEngine _instance;
        byte[] _buffer = new byte[1024];

        public static EmoEngine Instance
        {
            get
            {
                return _instance ?? (_instance = new EmoEngine());
            }
        }

        public delegate void EmoEngineConnectedEventHandler(object sender, EmoEngineEventArgs e);
        public delegate void InternalStateChangedEventHandler(object sender, EmoEngineEventArgs e);
        public delegate void EmoStateUpdatedEventHandler(object sender, EmoStateUpdatedEventArgs e);
        public delegate void UserAddedEventHandler(object sender, EmoEngineEventArgs e);

        public event EmoEngineConnectedEventHandler EmoEngineConnected;
        public event InternalStateChangedEventHandler InternalStateChanged;
        public event EmoStateUpdatedEventHandler EmoStateUpdated;
        public event UserAddedEventHandler UserAdded;

        public void DataAcquisitionEnable(uint userId, bool enable)
        {
            var ipep = new IPEndPoint(IPAddress.Any, 9010);
            _socket.Bind(ipep);
        }

        public int DataGetSamplingRate(uint userId)
        {
            return 999;
        }

        public float EE_DataGetBufferSizeInSec()
        {
            throw new NotImplementedException();
        }

        public void Connect()   
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EmoEngineConnected(this, new EmoEngineEventArgs
                                     {
                                         userId = 0
                                     });
        }

        public void Disconnect()
        {
            _socket.Disconnect(false);
        }

        public Dictionary<EE_DataChannel_t, double[]> GetData(uint userId)
        {
            var result = _socket.Receive(_buffer);

            var stringData = Encoding.ASCII.GetString(_buffer, 0, result);

            string[] parameters = stringData.Split(',');

            var ret = new Dictionary<EE_DataChannel_t, double[]>();

            _chCounter.Enqueue(double.Parse(parameters[0]));
            _ch4.Enqueue(double.Parse(parameters[4]));
            _ch5.Enqueue(double.Parse(parameters[5]));
            _ch6.Enqueue(double.Parse(parameters[6]));
            _ch7.Enqueue(double.Parse(parameters[7]));
            _ch8.Enqueue(double.Parse(parameters[8]));
            _ch9.Enqueue(double.Parse(parameters[9]));
            _ch10.Enqueue(double.Parse(parameters[10]));
            _ch11.Enqueue(double.Parse(parameters[11]));
            _ch12.Enqueue(double.Parse(parameters[12]));
            _ch13.Enqueue(double.Parse(parameters[13]));
            _ch14.Enqueue(double.Parse(parameters[14]));
            _ch15.Enqueue(double.Parse(parameters[15]));
            _ch16.Enqueue(double.Parse(parameters[16]));
            _ch17.Enqueue(double.Parse(parameters[17]));

            ret.Add(EE_DataChannel_t.COUNTER, _chCounter.Queue.ToArray());
            ret.Add(EE_DataChannel_t.AF3, _ch4.Queue.ToArray());
            ret.Add(EE_DataChannel_t.AF4, _ch5.Queue.ToArray());
            ret.Add(EE_DataChannel_t.F3, _ch6.Queue.ToArray());
            ret.Add(EE_DataChannel_t.F4, _ch7.Queue.ToArray());
            ret.Add(EE_DataChannel_t.F7, _ch8.Queue.ToArray());
            ret.Add(EE_DataChannel_t.F8, _ch9.Queue.ToArray());
            ret.Add(EE_DataChannel_t.FC5, _ch10.Queue.ToArray());
            ret.Add(EE_DataChannel_t.FC6, _ch11.Queue.ToArray());
            ret.Add(EE_DataChannel_t.O1, _ch12.Queue.ToArray());
            ret.Add(EE_DataChannel_t.O2, _ch13.Queue.ToArray());
            ret.Add(EE_DataChannel_t.P7, _ch14.Queue.ToArray());
            ret.Add(EE_DataChannel_t.P8, _ch15.Queue.ToArray());
            ret.Add(EE_DataChannel_t.T7, _ch16.Queue.ToArray());
            ret.Add(EE_DataChannel_t.T8, _ch17.Queue.ToArray());

            return ret;
        }

        public void ProcessEvents(int i)
        {
            throw new NotImplementedException();
        }
    }
}
