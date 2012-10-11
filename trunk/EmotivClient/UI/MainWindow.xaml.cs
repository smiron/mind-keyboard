using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;


namespace UI
{
    public partial class MainWindow
    {
        private int _gyroX;
        private int _gyroY;
        private volatile string[] _sensors = new string[14];

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
                                               var ipep = new IPEndPoint(IPAddress.Any, 9011);

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
            timer.Enabled = false;

            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    const int xt = 1;
                    const int yt = 1;
                    const int sleep = 80;

                    lock (this)
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



                    text.Text = string.Join(" , ", _sensors);


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

        private void BtnLearnClick(object sender, RoutedEventArgs e)
        {
            double[][] input = { new double[] { 55, 0 }, // 0 - no cancer      
                                      new double[] { 28, 0 }, // 0      
                                      new double[] { 65, 1 }, // 0      
                                      new double[] { 46, 0 }, // 1 - have cancer      
                                      new double[] { 86, 1 }, // 1      
                                      new double[] { 56, 1 }, // 1     
                                      new double[] { 85, 0 }, // 0    
                                      new double[] { 33, 0 }, // 0    
                                      new double[] { 21, 1 }, // 0      
                                      new double[] { 42, 1 }, // 1 
                                  };
            // We also know if they have had lung cancer or not, and 
            // we would like to know whether smoking has any connection  
            // with lung cancer (This is completely fictional data). 
            double[] output = { 0, 0, 0, 1, 1, 1, 0, 0, 0, 1 };
            // To verify this hypothesis, we are going to create a logistic  
            // regression model for those two inputs (age and smoking). 
            LogisticRegression regression = new LogisticRegression(inputs: 2);
            //Next, we are going to estimate this model. For this, we  
            // will use the Iteravely reweighted least squares method.  
            var teacher = new IterativeReweightedLeastSquares(regression);
            // Now, we will iteratively estimate our model. The Run method returns  
            // the maximum relative change in the model parameters and we will use  
            // it as the convergence criteria.   
            double delta = 0; do
            {
                // Perform an iteration 
                delta = teacher.Run(input, output);
            } while (delta > 0.001);


            var a = regression.Compute(new double[] {28, 0});

        }


    }
}
