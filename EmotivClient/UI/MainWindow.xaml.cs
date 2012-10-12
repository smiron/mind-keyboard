using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Accord.Statistics.Models.Regression;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;


namespace UI
{
    public partial class MainWindow
    {
        private int _gyroX;
        private int _gyroY;
        private volatile string[] _sensors = new string[14];

        public LogisticRegression Regression;
        public List<double[]> NeutralInput;
        public List<double[]> ActionInput;
        
        public bool ColectingNeutralData;
        public bool ColectingActionData;

        private readonly Timer _timer;


        public MainWindow()
        {
            InitializeComponent();
            _timer = new Timer(30);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();


            new Thread(new ThreadStart(delegate
                                           {

                                               var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                               var ipep = new IPEndPoint(IPAddress.Any, 9011);
                                               socket.Bind(ipep);

                                               var data = new byte[1024];

                                               while (true)
                                               {
                                                   var result = socket.Receive(data);

                                                   if (result > 0)
                                                   {
                                                       var stringData = Encoding.ASCII.GetString(data, 0, result);
                                                       string[] parameters = stringData.Split(',').Skip(1).ToArray();

                                                       var x = int.Parse(parameters[2]) - 3;
                                                       var y = int.Parse(parameters[3]);

                                                       _sensors[0] = parameters[4];
                                                       _sensors[1] = parameters[5];
                                                       _sensors[2] = parameters[6];
                                                       _sensors[3] = parameters[7];
                                                       _sensors[4] = parameters[8];
                                                       _sensors[5] = parameters[9];
                                                       _sensors[6] = parameters[10];
                                                       _sensors[7] = parameters[11];
                                                       _sensors[8] = parameters[12];
                                                       _sensors[9] = parameters[13];
                                                       _sensors[10] = parameters[14];
                                                       _sensors[11] = parameters[15];
                                                       _sensors[12] = parameters[16];
                                                       _sensors[13] = parameters[17];

                                                       lock (this)
                                                       {
                                                           _gyroX = x;
                                                           _gyroY = y;
                                                       }
                                                   }

                                               }
                                           })).Start();

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;

            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    const int xt = 1;
                    const int yt = 1;
                    const int sleep = 60;

                    lock (this)
                    {

                        try
                        {
                            
                        if (Math.Abs(_gyroX) > Math.Abs(_gyroY))
                        {

                            if (_gyroX > xt)
                            {
                                if (!bt_a.IsKeyboardFocused && !bt_h.IsKeyboardFocused && !bt_o.IsKeyboardFocused && !bt_v.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0, Key.Left) { RoutedEvent = KeyDownEvent };
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                            else if (_gyroX < -xt)
                            {
                                if (!bt_g.IsKeyboardFocused && !bt_n.IsKeyboardFocused && !bt_u.IsKeyboardFocused && !bt_enter.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0,
                                                                        Key.Right) { RoutedEvent = KeyDownEvent };
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                        }
                        else
                        {

                            if (_gyroY > yt)
                            {
                                if (!bt_v.IsKeyboardFocused && !bt_x.IsKeyboardFocused && !bt_y.IsKeyboardFocused && !bt_z.IsKeyboardFocused && !bt_space.IsKeyboardFocused && !bt_erase.IsKeyboardFocused && !bt_enter.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down) { RoutedEvent = KeyDownEvent };
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                            else if (_gyroY < -yt)
                            {

                                if (!bt_a.IsKeyboardFocused && !bt_b.IsKeyboardFocused && !bt_c.IsKeyboardFocused && !bt_d.IsKeyboardFocused && !bt_e.IsKeyboardFocused && !bt_f.IsKeyboardFocused && !bt_g.IsKeyboardFocused)
                                {
                                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                                                        Keyboard.PrimaryDevice.ActiveSource, 0, Key.Up) { RoutedEvent = KeyDownEvent };
                                    InputManager.Current.ProcessInput(keyEventArgs);
                                    Thread.Sleep(sleep);
                                }
                            }
                        }
                        _gyroX = 0;
                        _gyroY = 0;

                        }
                        catch (Exception)
                        {
                        }
                    }
                    
                    text.Text = string.Join(" , ", _sensors);
                    
                    if (Regression != null)
                    {
                        var doubleArray = new List<string>(_sensors).Select(double.Parse).ToArray();
                        var c = Regression.Compute(doubleArray);
                        confidence.Value = (int) (c*100);
                    }
                    if (ColectingNeutralData)
                    {
                        var doubleArray = new List<string>(_sensors).Select(double.Parse).ToArray();
                        NeutralInput.Add(doubleArray);
                    }
                    if (ColectingActionData)
                    {
                        var doubleArray = new List<string>(_sensors).Select(double.Parse).ToArray();
                        ActionInput.Add(doubleArray);
                    }

                }));


            }
            finally
            {
                _timer.Enabled = true;
            }


        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _gyroX = 0;
            _gyroY = 0;
        }

        private void BtnLearnClick(object sender, RoutedEventArgs e)
        {
            var trainWindow = new Training();

            trainWindow.SetParent(this);
            trainWindow.Show();
        }

        private void bt_a_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "a";
        }

        private void bt_b_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "b";
        }

        private void bt_c_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "c";
        }

        private void bt_d_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "d";
        }

        private void bt_e_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "e";
        }

        private void bt_f_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "f";
        }

        private void bt_g_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "g";
        }

        private void bt_h_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "h";
        }

        private void bt_i_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "i";
        }

        private void bt_j_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "j";
        }

        private void bt_k_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "k";
        }

        private void bt_l_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "l";
        }

        private void bt_m_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "m";
        }

        private void bt_n_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "n";
        }

        private void bt_o_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "o";
        }

        private void bt_p_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "p";
        }

        private void bt_q_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "q";
        }

        private void bt_r_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "r";
        }

        private void bt_s_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "s";
        }

        private void bt_t_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "t";
        }

        private void bt_u_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "u";
        }

        private void bt_v_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "v";
        }

        private void bt_x_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "x";
        }

        private void bt_y_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "y";
        }

        private void bt_z_Click(object sender, RoutedEventArgs e)
        {
            text.Text += "z";
        }

        private void bt_space_Click(object sender, RoutedEventArgs e)
        {
            text.Text += " ";
        }

        private void bt_erase_Click(object sender, RoutedEventArgs e)
        {
            if (text.Text.Length > 0)
            {
                text.Text += text.Text.Substring(0, text.Text.Length - 1);
            }
        }

        private void bt_enter_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
