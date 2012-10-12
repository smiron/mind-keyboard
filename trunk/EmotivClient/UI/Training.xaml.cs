using System;
using System.Collections.Generic;
using System.Threading;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;

namespace UI
{
    public partial class Training
    {
        private MainWindow _mainWindow;
        private readonly Thread _bThread;

        public Training()
        {
            InitializeComponent();


            Closing += TrainingClosing;

            _bThread = new Thread(new ThreadStart(delegate
            {

                SetText("COLECTING DATA FOR 'NEUTRAL'");
                Thread.Sleep(3000);
                SetText("3");
                Thread.Sleep(1000);
                SetText("2");
                Thread.Sleep(1000);
                SetText("1");
                Thread.Sleep(1000);
                SetText("0");
                Thread.Sleep(1000);
                SetText("(...)");
                _mainWindow.NeutralInput = new List<double[]>();
                _mainWindow.ColectingNeutralData = true;
                Thread.Sleep(30000);
                SetText("DONE COLECTING");
                Thread.Sleep(1000);
                _mainWindow.ColectingNeutralData = false;



                SetText("COLECTING DATA FOR 'CLICK'");
                Thread.Sleep(3000);
                SetText("3");
                Thread.Sleep(1000);
                SetText("2");
                Thread.Sleep(1000);
                SetText("1");
                Thread.Sleep(1000);
                SetText("0");
                Thread.Sleep(1000);
                SetText("(...)");
                _mainWindow.ActionInput = new List<double[]>();
                _mainWindow.ColectingActionData = true;
                Thread.Sleep(10000);
                SetText("DONE COLECTING");
                _mainWindow.ColectingActionData = false;
                Thread.Sleep(1000);



                SetText("TRAINING - WAIT");
                var output = new double[_mainWindow.ActionInput.Count + _mainWindow.NeutralInput.Count];
                for (int i = 0; i < _mainWindow.ActionInput.Count; i++)
                {
                    output[i] = 1;
                }
                

                var input = new List<double[]>();
                
                input.AddRange(_mainWindow.ActionInput);
                input.AddRange(_mainWindow.NeutralInput);


                var arrayInput = input.ToArray();

                _mainWindow.Regression = new LogisticRegression(14);

                var teacher = new IterativeReweightedLeastSquares(_mainWindow.Regression);

                double delta; 
                do {
                    delta = teacher.Run(arrayInput, output);  
                } while (delta > 0.001);

                SetText("DONE TRAINING");


            }));

        }

        void TrainingClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _bThread.Abort();
        }


        private void TrainClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _bThread.Start();
            button1.IsEnabled = false;

        }

        private void SetText(string text)
        {
            Dispatcher.Invoke(new Action(() => { lbText.Text = text; }));
        }

        public void SetParent(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }




    }
}
