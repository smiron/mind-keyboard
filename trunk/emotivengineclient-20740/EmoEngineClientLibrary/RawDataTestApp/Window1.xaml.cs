// Copyright © 2010 James Galasyn 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using EmoEngineClientLibrary;
using EmoEngineControlLibrary;

namespace RawDataTestApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        EmoEngineClient _emoEngineClient;

        public Window1()
        {
            InitializeComponent();

            this._emoEngineClient = this.FindResource( "emoEngineClient" ) as EmoEngineClient;
            this._emoEngineClient.ActivePort = EmoEngineClient.EmoComposerPort; // TBD: move to XAML
            //this._emoEngineClient.ActivePort = EmoEngineClient.ControlPanelPort;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
        }

        private void _startButton_Click( object sender, RoutedEventArgs e )
        {
            this._emoEngineClient.StartDataPolling();

            //// TBD: declarative
            //ChannelList channelsToDisplay = new ChannelList();
            //channelsToDisplay.Add( .EE_DataChannel_t.AF3 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.AF4 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.COUNTER );
            ////channelsToDisplay.Add( .EE_DataChannel_t.F3 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.F4 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.F7 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.F8 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.FC5 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.FC6 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.O1 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.O2 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.P7 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.P8 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.T7 );
            ////channelsToDisplay.Add( .EE_DataChannel_t.T8 );

            //this._neuroDataControl.ChannelsToDisplay = channelsToDisplay;

            this._neuroDataControl.Start();
        }

        private void _stopButton_Click( object sender, RoutedEventArgs e )
        {
            this._emoEngineClient.StopDataPolling();
        }

        private void _startEmoEngineButton_Click( object sender, RoutedEventArgs e )
        {
            this._emoEngineClient.StartEmoEngine();
        }

        //private FrameworkElement CreateNeuroDataControl()
        //{
        //    NeuroDataControl neuroDataControl = new NeuroDataControl();
        //    neuroDataControl.HorizontalAlignment = HorizontalAlignment.Stretch;
        //    neuroDataControl.VerticalAlignment = VerticalAlignment.Stretch;

        //    neuroDataControl.Width = Double.NaN;
        //    neuroDataControl.Height = 600;

        //    neuroDataControl.DataFrameSource = this._emoEngineClient;

        //    return neuroDataControl;
        //}

        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            this._neuroDataControl.Shutdown();
        }

        private static AutoResetEvent s_event = new AutoResetEvent( false );
    }
}
