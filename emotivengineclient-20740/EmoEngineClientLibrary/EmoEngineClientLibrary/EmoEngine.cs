using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EmoEngineClientLibrary
{
    public class EmoEngine
    {
        const int _historySize = 256;

        private Dictionary<EE_DataChannel_t, double[]> _dataDisctionary = new Dictionary<EE_DataChannel_t, double[]>();

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
            var ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9011);
            _socket.Connect(ipep);

            var thread = new Thread(new ThreadStart(CollecterThread));
            thread.Start();
        }

        private void CollecterThread()
        {
            while (true)
            {
                var result = _socket.Receive(_buffer);

                var stringData = Encoding.ASCII.GetString(_buffer, 0, result);

                string[] parameters = stringData.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int indexOfStart = 0;
                if ((indexOfStart = Array.IndexOf(parameters, "start")) < 0)
                {
                    continue;
                }

                parameters = parameters.Skip(indexOfStart + 1).Take(18).ToArray();

                foreach (var keyValue in _dataDisctionary)
                {
                    var newValue = double.Parse(parameters[(int)keyValue.Key]);
                    AddToLastPosition(keyValue.Value, newValue);
                }
            }
        }

        public int DataGetSamplingRate(uint userId)
        {
            return 128;
        }

        public float EE_DataGetBufferSizeInSec()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            _dataDisctionary = new Dictionary<EE_DataChannel_t, double[]>();

            _dataDisctionary.Add(EE_DataChannel_t.COUNTER, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());

            _dataDisctionary.Add(EE_DataChannel_t.AF3, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            _dataDisctionary.Add(EE_DataChannel_t.AF4, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            _dataDisctionary.Add(EE_DataChannel_t.F3, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            _dataDisctionary.Add(EE_DataChannel_t.F4, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            _dataDisctionary.Add(EE_DataChannel_t.F7, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.F8, Enumerable.Range(0, _historySize).Select(x => 8500.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.FC5, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.FC6, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.O1, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.O2, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.P7, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.P8, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.T7, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());
            //_dataDisctionary.Add(EE_DataChannel_t.T8, Enumerable.Range(0, _historySize).Select(x => 0.0d).ToArray());


            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            EmoEngineConnected(this, new EmoEngineEventArgs
                                     {
                                         userId = 0
                                     });
        }

        private void AddToLastPosition(double[] values, double newValue)
        {
            for (int i = 0; i < values.Length - 1; i++)
            {
                values[i] = values[i + 1];
            }
            values[values.Length - 1] = newValue;
        }

        public void Disconnect()
        {
            _socket.Disconnect(false);
        }

        public Dictionary<EE_DataChannel_t, double[]> GetData(uint userId)
        {
            return new Dictionary<EE_DataChannel_t, double[]>(_dataDisctionary);
        }

        public void ProcessEvents(int i)
        {
            throw new NotImplementedException();
        }
    }
}
