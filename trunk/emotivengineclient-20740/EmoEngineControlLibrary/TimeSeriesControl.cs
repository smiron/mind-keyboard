// Copyright © 2010 James Galasyn 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using EmoEngineClientLibrary;
using System.Windows.Threading;

namespace EmoEngineControlLibrary
{
    [TemplatePart( Name = "PART_SignalPolyline", Type = typeof( Polyline ) )]
    [TemplatePart( Name = "PART_SignalNameTextBlock", Type = typeof( TextBlock ) )]
    [TemplatePart( Name = "PART_CurrentValueTextBlock", Type = typeof( TextBlock ) )]
    [TemplatePart( Name = "PART_Border", Type = typeof( Border ) )]
    [TemplatePart( Name = "PART_Grid", Type = typeof( Grid ) )]
    [TemplatePart( Name = "PART_SweepRect", Type = typeof( Rectangle ) )]
    [TemplatePart( Name = "PART_SweepRectTransform", Type = typeof( TranslateTransform ) )]
    public class TimeSeriesControl : Control, INotifyPropertyChanged
    {
        ///////////////////////////////////////////////////////////////////////
        #region Fields

        /// <summary>
        /// The name of the Polyline template part that traces the signal.
        /// </summary>
        private const string polylineName = "PART_SignalPolyline";

        /// <summary>
        /// The name of the TextBlock template part that shows the 
        /// performance counter name.
        /// </summary>
        private const string textBlockName = "PART_SignalNameTextBlock";

        /// <summary>
        /// The name of the TextBlock template part that shows the 
        /// performance counter's current value.
        /// </summary>
        private const string currentValueTextBlockName = "PART_CurrentValueTextBlock";

        private const string sweepRectName = "PART_SweepRect";

        private const string sweepRectTransformName = "PART_SweepRectTransform";

        TextBlock _signalNameTextBlock;
        TextBlock _currentValueTextBlock;
        Polyline _signalPolyline;
        Rectangle _sweepRectangle;
        TranslateTransform _sweepRectTransform;

        protected PropertyChangedEventHandler propertyChanged = null;

        int currentPointIndex = 0;
        private int _bufferSize;

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region Construction and Initialization

        static TimeSeriesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof( TimeSeriesControl ),
                new FrameworkPropertyMetadata( typeof( TimeSeriesControl ) ) );
        }

        public TimeSeriesControl()
        {   
            this.MinValue = double.MaxValue;
            this.MaxValue = double.MinValue;
        }

        protected virtual void InitializePolyLine( int bufferLength, double width )
        {
            this._signalPolyline.Points = new PointCollection( bufferLength );
            double xIncrement = this.XScale * width / (double)bufferLength;
            double xCurrent = 0;
            //this._signalPolyline.Width = width;

            for( int i = 0; i < bufferLength; i++ )
            {
                this._signalPolyline.Points.Add( new Point( xCurrent, 0 ) );

                xCurrent += xIncrement;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region Public Properties



        public double XScale
        {
            get { return (double)GetValue( XScaleProperty ); }
            set { SetValue( XScaleProperty, value ); }
        }

        public static readonly DependencyProperty XScaleProperty =
            DependencyProperty.Register( 
                "XScale", 
                typeof( double ), 
                typeof( TimeSeriesControl ), 
                new UIPropertyMetadata( 1.0d ) );

        

        ///////////////////////////////////////////////////////////////////////
        #region DataFrameSource Dependency Property

        public EmoEngineClient DataFrameSource
        {
            get;
            set;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region TraceStroke Dependency Property

        public static readonly DependencyProperty TraceStrokeProperty =
            DependencyProperty.Register(
            "TraceStroke",
            typeof( Brush ),
            typeof( TimeSeriesControl ),
            new PropertyMetadata( new PropertyChangedCallback( OnTraceStrokeChanged ) ) );

        public Brush TraceStroke
        {
            get
            {
                return (Brush)GetValue( TraceStrokeProperty );
            }

            set
            {
                SetValue( TraceStrokeProperty, value );
            }
        }

        private static void OnTraceStrokeChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            TimeSeriesControl tsc = d as TimeSeriesControl;

            Brush b = e.NewValue as Brush;

            // TBD: Big old hack. Template hasn't been applied.
            if( tsc._signalPolyline != null )
            {
                tsc._signalPolyline.Stroke = b;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region TraceStrokeThickness Dependency Property

        public static readonly DependencyProperty TraceStrokeThicknessProperty =
            DependencyProperty.Register(
            "TraceStrokeThickness",
            typeof( double ),
            typeof( TimeSeriesControl ),
            new PropertyMetadata( new PropertyChangedCallback( OnTraceStrokeThicknessChanged ) ) );

        public double TraceStrokeThickness
        {
            get
            {
                return (double)GetValue( TraceStrokeThicknessProperty );
            }

            set
            {
                SetValue( TraceStrokeThicknessProperty, value );
            }
        }

        private static void OnTraceStrokeThicknessChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            TimeSeriesControl tsc = d as TimeSeriesControl;

            double thickness = (double)e.NewValue;

            // TBD: Big old hack. Template hasn't been applied.
            if( tsc._signalPolyline != null )
            {
                tsc._signalPolyline.StrokeThickness = thickness;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region SignalName


        public static readonly DependencyProperty SignalNameProperty =
            DependencyProperty.Register(
            "SignalName",
            typeof( string ),
            typeof( TimeSeriesControl ),
            new PropertyMetadata( new PropertyChangedCallback( OnSignalNameChanged ) ) );

        public string SignalName
        {
            get
            {
                DispatcherOperation dispop = 
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new DispatcherOperationCallback(
                        delegate( object param )
                        {
                            return (string)GetValue( SignalNameProperty );
                        } ), null );
                ;

                //return (string)GetValue( SignalNameProperty );
                return dispop.Result as string;
            }

            set
            {
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new DispatcherOperationCallback( 
                        delegate( object param )
                        {
                            string val = (string)param;
                            SetValue( SignalNameProperty, value );
                            return null;
                        } ), value );
            }
        }

        private static void OnSignalNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            TimeSeriesControl tsc = d as TimeSeriesControl;

            string signalName = (string)e.NewValue;

            tsc._signalNameTextBlock.Text = signalName;
        }



        #endregion


        ///////////////////////////////////////////////////////////////////////
        #region Width Property Shadow


        public new double Width
        {
            get
            {
                return base.Width;
            }

            set
            {
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new DispatcherOperationCallback(
                        delegate( object param )
                        {
                            double val = (double)param;
                            SetValue( WidthProperty, value );
                            //RenderFrame(new double[] {0} );
                            //UpdateDisplay( new double[] { 0 } );
                            //InitializePolyLine( 1024, val );
                            //UpdateDisplay( new double[] { 0 } );
                            return null;
                        } ), value );

            }
        }

        #endregion





        ///////////////////////////////////////////////////////////////////////
        #region AutoScale Dependency Property

        //public static readonly DependencyProperty AutoScaleProperty =
        //    DependencyProperty.Register(
        //    "AutoScale",
        //    typeof( bool ),
        //    typeof( TimeSeriesControl ),
        //    new PropertyMetadata( true ) );

        //public bool AutoScale
        //{
        //    get
        //    {
        //        return (bool)GetValue( AutoScaleProperty );
        //    }

        //    set
        //    {
        //        SetValue( AutoScaleProperty, value );
        //    }
        //}

        public bool AutoScale
        {
            get;
            set;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region Annotations Dependency Property

        public static readonly DependencyProperty AnnotationsProperty =
            DependencyProperty.Register(
            "Annotations",
            typeof( bool ),
            typeof( TimeSeriesControl ),
            new PropertyMetadata( new PropertyChangedCallback( OnAnnotationsChanged ) ) );

        public bool Annotations
        {
            get
            {
                return (bool)GetValue( AnnotationsProperty );
            }

            set
            {
                SetValue( AnnotationsProperty, value );
            }
        }

        private static void OnAnnotationsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            TimeSeriesControl tsc = d as TimeSeriesControl;

            // TBD: Hack
            if( tsc._signalNameTextBlock != null )
            {
                tsc._signalNameTextBlock.Visibility = ( (bool)e.NewValue ) ? Visibility.Visible : Visibility.Hidden;
            }

            if( tsc._currentValueTextBlock != null )
            {
                tsc._currentValueTextBlock.Visibility = ( (bool)e.NewValue ) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region CurrentValue Dependency Property

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
            "CurrentValue",
            typeof( float ),
            typeof( TimeSeriesControl ),
            new PropertyMetadata( new PropertyChangedCallback( OnCurrentValueChanged ) ) );

        public float CurrentValue
        {
            get
            {
                return (float)GetValue( CurrentValueProperty );
            }

            set
            {
                SetValue( CurrentValueProperty, value );
            }
        }

        private static void OnCurrentValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            //TimeSeriesControl tsc = d as TimeSeriesControl;
            //tsc.UpdateDisplay();
        }

        #endregion


        #endregion


        protected override void OnRenderSizeChanged( SizeChangedInfo sizeInfo )
        {
            base.OnRenderSizeChanged( sizeInfo );

            if( sizeInfo.WidthChanged ) 
            {
                if( this._signalPolyline != null )
                {
                    this._signalPolyline.Points.Clear();
                    this.MaxValue = double.MinValue;
                    this.MinValue = double.MaxValue;
                }
            }
        }

        public void Start()
        {
           
        }

        public void Stop()
        {
            
        }


        public double MaxValue
        {
            get;
            set;
        }

        public double MinValue
        {
            get;
            set;
        }

        public int BufferSize
        {
            get
            {
                return this._bufferSize;
            }

            set
            {
                if( value > 0 )
                {
                    if( value != this._bufferSize )
                    {
                        this._bufferSize = value;
                        //this.InitializePolyLine( this._bufferSize, this.ActualWidth );
                        this.OnPropertyChanged( "BufferSize" );
                    }
                }
                else
                {
                    throw new ArgumentException( "must be > 0", "BufferSize" );
                }
            }
        }

        public void UpdateDisplay( double[] data )
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(
                    delegate( object param )
                    {
                        double[] val = (double[])param;
                        RenderFrame( val );
                        InvalidateVisual();
                        return null;

                    } ), data );

        }

        private void RenderFrame( double[] data )
        {
            double scaleFactor = 1;


            if( this.AutoScale )
            {
                //if( this.MaxValue != double.MinValue && this.MinValue != double.MaxValue )
                {
                    double maxValueAbs = Math.Abs( this.MaxValue );
                    double minValueAbs = Math.Abs( this.MinValue );
                    double span = maxValueAbs + minValueAbs + 10;

                    //double scale = ( maxValueAbs >= minValueAbs ) ? 2*maxValueAbs : 2*minValueAbs;
                    double scale = span;

                    if( span != 0 )
                    {
                        // TBD: revisit the span thing
                        if( span > this.ActualHeight )
                        {
                            scaleFactor = this.ActualHeight / span;
                        }

                        if( scale < this.ActualHeight / 4 )
                        {
                            scaleFactor = this.ActualHeight / span;
                        }
                    }
                }
            }

            if( this._signalPolyline.Points.Count == 0 )
            {
                this.InitializePolyLine( this.BufferSize, this.ActualWidth );
            }

            this.MinValue = data.Min();
            this.MaxValue = data.Max();

            for( int j = 0; j < data.Length; j++ )
            {
                Point p0 = this._signalPolyline.Points[this.currentPointIndex];
                p0.Y = this.ActualHeight - ( scaleFactor * data[j] ) - this.ActualHeight / 2.0d;
                this._signalPolyline.Points[this.currentPointIndex] = p0;

                this.currentPointIndex = ( this.currentPointIndex + 1 ) % this._signalPolyline.Points.Count;

                //this._sweepRectTransform.X = this.currentPointIndex * ( this._signalPolyline.Points[1].X - this._signalPolyline.Points[0].X );
                //this._sweepRectangle.InvalidateVisual();

                //this._sweepRectTransform = new TranslateTransform( ( this.currentPointIndex * this._signalPolyline.Points[1].X  ), this._sweepRectTransform.Y );

                //if( this.currentPointIndex == this._signalPolyline.Points.Count - 1 )
                //{
                //    this.MaxValue = double.MinValue;
                //    this.MinValue = double.MaxValue;
                //    scaleFactor = 1;
                //}

            }

            //this._sweepRectTransform.X = this.currentPointIndex * ( this._signalPolyline.Points[1].X - this._signalPolyline.Points[0].X );
            //this._sweepRectangle.InvalidateVisual();
            //this._sweepRectTransform = new TranslateTransform( ( this.currentPointIndex * ( this._signalPolyline.Points[1].X - this._signalPolyline.Points[0].X ) ), this._sweepRectTransform.Y );
            //this._sweepRectangle.RenderTransform = this._sweepRectTransform;


        }

        protected override void OnVisualParentChanged( DependencyObject oldParent )
        {
            base.OnVisualParentChanged( oldParent );

            var parent = VisualTreeHelper.GetParent( this );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._signalNameTextBlock = GetTemplateChild( textBlockName ) as TextBlock;
            this._currentValueTextBlock = GetTemplateChild( currentValueTextBlockName ) as TextBlock;
            this._signalPolyline = GetTemplateChild( polylineName ) as Polyline;
            this._sweepRectangle = GetTemplateChild( sweepRectName ) as Rectangle;
            this._sweepRectTransform = GetTemplateChild( sweepRectTransformName ) as TranslateTransform;
        }

        ///////////////////////////////////////////////////////////////////////
        #region PropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                this.propertyChanged += value;
            }

            remove
            {
                this.propertyChanged -= value;
            }
        }

        protected virtual void OnPropertyChanged( string propertyName )
        {
            if( this.propertyChanged != null )
            {
                this.propertyChanged(
                    this,
                    new PropertyChangedEventArgs( propertyName ) );
            }
        }

        #endregion

    }
}
