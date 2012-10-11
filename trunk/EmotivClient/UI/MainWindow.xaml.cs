using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;


namespace UI
{
    public partial class MainWindow
    {
        private int _gyroX;
        private int _gyroY;


        private Timer timer;


        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(30);
            timer.Elapsed += TimerElapsed;
            timer.Start();

            
            new Thread(new ThreadStart(delegate
                                           {

                                               var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                               var ipep = new IPEndPoint(IPAddress.Any, 9009);

                                               socket.Bind(ipep);

                                               var data = new byte[1024];

                                               while (true)
                                               {
                                                   var result = socket.Receive(data);

                                                   if (result > 0)
                                                   {
                                                       var stringData = Encoding.ASCII.GetString(data, 0, result);
                                                       string[] parameters = stringData.Split(',');

                                                       var x = int.Parse(parameters[2]) - 3;
                                                       var y = int.Parse(parameters[3]);

                                                       lock (this)
                                                       {
                                                           _gyroX = x;
                                                           _gyroY = y;
                                                       }

                                                       Dispatcher.Invoke(new Action(() =>
                                                                                        {
                                                                                            tb1.Text = parameters[2];
                                                                                            tb2.Text = parameters[3];

                                                                                        }));
                                                   }

                                               }
                                           })).Start();

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;

            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    //center.Margin = new Thickness(500 - _gyroX, 300 + _gyroY, 0, 0);


                    var xtRight = 1;
                    var xtLeft = 1;
                    var yt = 1;


                    var sleep = 80;

                    lock (this)
                    {


                        if (Math.Abs(_gyroX) > Math.Abs(_gyroY))
                        {

                            if (_gyroX > xtLeft)
                            {
                                if (!button1.IsKeyboardFocused && !button8.IsKeyboardFocused && !button15.IsKeyboardFocused && !button22.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0, Key.Left)
                                                           {RoutedEvent = KeyDownEvent};
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                            else if (_gyroX < -xtRight)
                            {
                                if (!button14.IsKeyboardFocused && !button7.IsKeyboardFocused && !button21.IsKeyboardFocused && !button28.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0,
                                                                        Key.Right) {RoutedEvent = KeyDownEvent};
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                        }
                        else
                        {

                            if (_gyroY > yt)
                            {
                                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                    Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down)
                                                       {RoutedEvent = KeyDownEvent};
                                InputManager.Current.ProcessInput(keyEventArgs);
                                Thread.Sleep(sleep);
                            }
                            else if (_gyroY < -yt)
                            {
                                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                    Keyboard.PrimaryDevice.ActiveSource, 0, Key.Up)
                                                       {RoutedEvent = KeyDownEvent};
                                InputManager.Current.ProcessInput(keyEventArgs);
                                Thread.Sleep(sleep);
                            }
                        }


                        _gyroX = 0;
                        _gyroY = 0;
                    }



                }));


            }
            finally
            {
                timer.Enabled = true;
            }


        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _gyroX = 0;
            _gyroY = 0;
        }


    }
}
