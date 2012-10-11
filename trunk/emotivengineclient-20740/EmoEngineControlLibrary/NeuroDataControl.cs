// Copyright © 2010 James Galasyn 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace EmoEngineControlLibrary
{
    /// <summary>
    /// Displays realtime electrode data from the Emotiv neuroheadset.
    /// </summary>
    /// <remarks>
    /// <para>Use the <see cref="NeuroDataControl"/> to display electrode data in realtime. </para>
    /// <para>The <see cref="NeuroDataControl"/> class assigns a <see cref="TimeSeriesControl"/>
    /// to each electrode data channel in the <see cref=".EE_DataChannel_t"/> enumeration.</para>
    /// <para>Because of the large amount of data to be rendered, each <see cref="TimeSeriesControl"/>
    /// is created on its own thread. 
    /// </para>
    /// <para>It is important to call the <see cref="Shutdown"/> method when your application is
    /// finished with the <see cref="NeuroDataControl"/>. The <see cref="Shutdown"/> method calls the 
    /// <see cref="InvokeShutdown"/> method for each <see cref="TimeSeriesControl"/> thread. If you
    /// do not call the <see cref="Shutdown"/> method when your application exits, your main user interface
    /// will close, but the <see cref="TimeSeriesControl"/> threads will continue to run.
    /// </para>
    /// </remarks>
    public class NeuroDataControl : Control
    {
        static NeuroDataControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( NeuroDataControl ),
                new FrameworkPropertyMetadata( typeof( NeuroDataControl ) ) );
        }

        public void Start()
        {
            InitializeTimeSeriesControlGrid();

            InitializeTimeSeriesControlCollection();
        }

        private void InitializeTimeSeriesControlGrid()
        {
            for( int i = 0; i < _channelList.Count; i++ )
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength( 50 );
                this._timeSeriesControlGrid.RowDefinitions.Add( row );

                VisualWrapper vw = new VisualWrapper();
                this._timeSeriesControlGrid.Children.Add( vw );
                vw.SetValue( Grid.RowProperty, i );
                vw.SetValue( Grid.RowSpanProperty, 2 );
                vw.SetValue( Grid.ColumnProperty, 0 );
                this._visualWrappers.Add( vw );
            }
        }

        private void InitializeTimeSeriesControlCollection()
        {
            this._timeSeriesControlDictionary = new Dictionary<EE_DataChannel_t, TimeSeriesControl>( _channelList.Count );

            if( ChannelsToDisplay != null )
            {
                for( int i = 0; i < ChannelsToDisplay.Count; i++ )
                {
                    this._timeSeriesControlDictionary[this._channelList[i]] = this._timeSeriesControls[i];
                    this._timeSeriesControls[i].SignalName = this._channelList[i].ToString();
                }
            }
            else
            {
                throw new Exception( "ChannelsToDisplay must be assigned" );
            }

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.AF3] = this._timeSeriesControls[0];
            //this._timeSeriesControls[0].SignalName = .EE_DataChannel_t.AF3.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.AF4] = this._timeSeriesControls[1];
            //this._timeSeriesControls[1].SignalName = .EE_DataChannel_t.AF4.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.COUNTER] = this._timeSeriesControls[2];
            //this._timeSeriesControls[2].SignalName = .EE_DataChannel_t.COUNTER.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.F3] = this._timeSeriesControls[3];
            //this._timeSeriesControls[3].SignalName = .EE_DataChannel_t.F3.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.F4] = this._timeSeriesControls[4];
            //this._timeSeriesControls[4].SignalName = .EE_DataChannel_t.F4.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.F7] = this._timeSeriesControls[5];
            //this._timeSeriesControls[5].SignalName = .EE_DataChannel_t.F7.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.F8] = this._timeSeriesControls[6];
            //this._timeSeriesControls[6].SignalName = .EE_DataChannel_t.F8.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.FC5] = this._timeSeriesControls[7];
            //this._timeSeriesControls[7].SignalName = .EE_DataChannel_t.FC5.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.FC6] = this._timeSeriesControls[8];
            //this._timeSeriesControls[8].SignalName = .EE_DataChannel_t.FC6.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.O1] = this._timeSeriesControls[9];
            //this._timeSeriesControls[9].SignalName = .EE_DataChannel_t.O1.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.O2] = this._timeSeriesControls[10];
            //this._timeSeriesControls[10].SignalName = .EE_DataChannel_t.O2.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.P7] = this._timeSeriesControls[11];
            //this._timeSeriesControls[11].SignalName = .EE_DataChannel_t.P7.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.P8] = this._timeSeriesControls[12];
            //this._timeSeriesControls[12].SignalName = .EE_DataChannel_t.P8.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.T7] = this._timeSeriesControls[13];
            //this._timeSeriesControls[13].SignalName = .EE_DataChannel_t.T7.ToString();

            //this._timeSeriesControlDictionary[.EE_DataChannel_t.T8] = this._timeSeriesControls[14];
            //this._timeSeriesControls[14].SignalName = .EE_DataChannel_t.T8.ToString();
        }

        public List<string> ChannelsToDisplay
        {
            get { return (List<string>)GetValue( ChannelsToDisplayProperty ); }
            set { SetValue( ChannelsToDisplayProperty, value ); }
        }

        public static readonly DependencyProperty ChannelsToDisplayProperty =
            DependencyProperty.Register( "ChannelsToDisplay", typeof( List<string> ), typeof( NeuroDataControl ), new PropertyMetadata( new PropertyChangedCallback( OnChannelsToDisplayChanged ) ) );

        private static void OnChannelsToDisplayChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            NeuroDataControl ndc = d as NeuroDataControl;

            List<string> channelStrings = e.NewValue as List<string>;

            ndc._channelList = new ChannelList();

            for( int i = 0; i < channelStrings.Count; i++ )
            {
                EE_DataChannel_t channel = (EE_DataChannel_t)Enum.Parse( typeof( EE_DataChannel_t ), channelStrings[i] );
                ndc._channelList.Add( channel );
            }
        }



        public double SweepPeriod
        {
            get { return (double)GetValue( SweepPeriodProperty ); }
            set { SetValue( SweepPeriodProperty, value ); }
        }

        public static readonly DependencyProperty SweepPeriodProperty =
            DependencyProperty.Register( "SweepPeriod", typeof( double ), typeof( NeuroDataControl ), new UIPropertyMetadata( 1.0d ) );

        public int DownsamplingFactor
        {
            get { return (int)GetValue( DownsamplingFactorProperty ); }
            set { SetValue( DownsamplingFactorProperty, value ); }
        }

        public static readonly DependencyProperty DownsamplingFactorProperty =
            DependencyProperty.Register(
                "DownsamplingFactor",
                typeof( int ),
                typeof( NeuroDataControl ) );

        public EmoEngineClient DataFrameSource
        {
            get
            {
                return (EmoEngineClient)GetValue( DataFrameSourceProperty );
            }

            set
            {
                SetValue( DataFrameSourceProperty, value );
            }
        }


        public static readonly DependencyProperty DataFrameSourceProperty =
            DependencyProperty.Register( "DataFrameSource", typeof( EmoEngineClient ), typeof( NeuroDataControl ), new PropertyMetadata( new PropertyChangedCallback( OnDataSourceChanged ) ) );

        private static void OnDataSourceChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            NeuroDataControl ndc = d as NeuroDataControl;

            EmoEngineClient emoEngineClient = e.NewValue as EmoEngineClient;

            if( emoEngineClient != null )
            {
                emoEngineClient.PropertyChanged += new PropertyChangedEventHandler( ndc.emoEngineClient_PropertyChanged );
            }

        }

        void emoEngineClient_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( e.PropertyName == "Buffer" )
            {
                EmoEngineClient emoEngineClient = sender as EmoEngineClient;

                // TBD: Does critical section degrade perf too much?
                lock( emoEngineClient.Buffer.ChannelData.SyncRoot )
                {
                    foreach( KeyValuePair<EE_DataChannel_t, double[]> kvp in emoEngineClient.Buffer.ChannelData )
                    {
                        if( this._timeSeriesControlDictionary.ContainsKey( kvp.Key ) )
                        {
                            EE_DataChannel_t channel = kvp.Key;

                            double[] data = kvp.Value;

                            this._timeSeriesControlDictionary[channel].UpdateDisplay( data );
                        }
                    }
                }
            }
        }

        private List<Thread> ControlThreads
        {
            get
            {
                if( this._controlThreads == null )
                {
                    this._controlThreads = new List<Thread>();
                }

                return this._controlThreads;
            }
        }


        public void Shutdown()
        {
            for( int i = 0; i < this._controlThreads.Count; i++ )
            {
                Dispatcher dispatcher = Dispatcher.FromThread( this._controlThreads[i] );
                dispatcher.InvokeShutdown();
            }
        }

        private HostVisual CreateTimeSeriesControlOnWorkerThread()
        {
            // Create the HostVisual that will "contain" the VisualTarget
            // on the worker thread.
            HostVisual hostVisual = new HostVisual();

            // Spin up a worker thread, and pass it the HostVisual that it
            // should be part of.
            Thread thread = new Thread( new ParameterizedThreadStart( TimeSeriesControlWorkerThread ) );
            //thread.ApartmentState = ApartmentState.STA;
            thread.TrySetApartmentState( ApartmentState.STA );
            thread.Priority = ThreadPriority.Highest;

            this.ControlThreads.Add( thread );

            //thread.IsBackground = true;
            thread.Start( hostVisual );

            // Wait for the worker thread to spin up and create the VisualTarget.
            s_event.WaitOne();

            return hostVisual;
        }

        private void TimeSeriesControlWorkerThread( object arg )
        {
            // Create the VisualTargetPresentationSource and then signal the
            // calling thread, so that it can continue without waiting for us.
            HostVisual hostVisual = (HostVisual)arg;
            VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource( hostVisual );
            s_event.Set();

            // Create a TimeSeriesControl and use it as the root visual for the VisualTarget.
            FrameworkElement tsc = CreateTimeSeriesControl();
            visualTargetPS.RootVisual = tsc;
            this._timeSeriesControls.Add( tsc as TimeSeriesControl );

            // Run a dispatcher for this worker thread.  This is the central processing loop for WPF.
            System.Windows.Threading.Dispatcher.Run();
        }

        private FrameworkElement CreateTimeSeriesControl()
        {
            TimeSeriesControl timeSeriesControl = new TimeSeriesControl();
            timeSeriesControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            timeSeriesControl.VerticalAlignment = VerticalAlignment.Stretch;

            timeSeriesControl.Width = 300;
            timeSeriesControl.Height = 50;
            timeSeriesControl.TraceStroke = Brushes.LightBlue;
            timeSeriesControl.TraceStrokeThickness = 1.5;
            timeSeriesControl.Foreground = Brushes.White;
            timeSeriesControl.AutoScale = true;

            timeSeriesControl.XScale = 4;
            timeSeriesControl.BufferSize = 1024; //this.DataFrameSource.BufferSize;
            //timeSeriesControl.DataFrameSource = this.DataFrameSource;
            //timeSeriesControl.ApplyTemplate();

            return timeSeriesControl;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._timeSeriesControlGrid = this.GetTemplateChild( "PART_TimeSeriesControlGrid" ) as Grid;

            this._timeSeriesControls = new List<TimeSeriesControl>( _channelList.Count );

            this._visualWrappers = new List<VisualWrapper>( _channelList.Count );
            InitializeTimeSeriesControlGrid();

            for( int i = 0; i < _channelList.Count; i++ )
            {
                HostVisual hv = CreateTimeSeriesControlOnWorkerThread();

                this._visualWrappers[i].Child = hv;
            }
        }

        protected override void OnRenderSizeChanged( SizeChangedInfo sizeInfo )
        {
            base.OnRenderSizeChanged( sizeInfo );

            if( _timeSeriesControlDictionary != null )
            {
                if( sizeInfo.WidthChanged )
                {
                    foreach( KeyValuePair<EE_DataChannel_t, TimeSeriesControl> kvp in this._timeSeriesControlDictionary )
                    {
                        this._timeSeriesControlDictionary[kvp.Key].Width = sizeInfo.NewSize.Width;
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        #region Private Fields

        private static AutoResetEvent s_event = new AutoResetEvent( false );
        private ChannelList _channelList;

        Grid _timeSeriesControlGrid;
        List<TimeSeriesControl> _timeSeriesControls;
        Dictionary<EE_DataChannel_t, TimeSeriesControl> _timeSeriesControlDictionary;
        List<VisualWrapper> _visualWrappers;
        List<Thread> _controlThreads;

        #endregion
    }
}
