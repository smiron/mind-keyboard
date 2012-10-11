// Copyright © 2010 James Galasyn 

using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace EmoEngineControlLibrary
{
    [TemplatePart( Name = "PART_SignalPolyline", Type = typeof( Polyline ) )]
    [TemplatePart( Name = "PART_SignalNameTextBlock", Type = typeof( TextBlock ) )]
    [TemplatePart( Name = "PART_CurrentValueTextBlock", Type = typeof( TextBlock ) )]
    [TemplatePart( Name = "PART_Border", Type = typeof( Border ) )]
    [TemplatePart( Name = "PART_Grid", Type = typeof( Grid ) )]
    public class SampleDisplayControl : Control
    {
        static SampleDisplayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( 
                typeof( SampleDisplayControl ), 
                new FrameworkPropertyMetadata( 
                    typeof( SampleDisplayControl ) ) );
        }





        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._signalNameTextBlock = GetTemplateChild( textBlockName ) as TextBlock;
            this._currentValueTextBlock = GetTemplateChild( currentValueTextBlockName ) as TextBlock;
            this._signalPolyline = GetTemplateChild( polylineName ) as Polyline;
        }



        ///////////////////////////////////////////////////////////////////////
        #region Public Properties


        ///////////////////////////////////////////////////////////////////////
        #region DataFrameSource Dependency Property

        //public EmoEngineClient DataFrameSource
        //{
        //    get;
        //    set;
        //}

        //public EmoEngineClient DataFrameSource
        //{
        //    get
        //    {
        //        //this.Dispatcher.BeginInvoke(
        //        //    DispatcherPriority.Background,
        //        //    new DispatcherOperationCallback(
        //        //        delegate( object param )
        //        //        {
        //        //            //EmoEngineClient val = (EmoEngineClient)param;
        //        //            return (EmoEngineClient)GetValue( DataFrameSourceProperty );

        //        //            //return null;

        //        //        } ), null );


        //        return (EmoEngineClient)GetValue( DataFrameSourceProperty );
        //    }

        //    set
        //    {
        //        //this.Dispatcher.BeginInvoke(
        //        //    DispatcherPriority.Background,
        //        //    new DispatcherOperationCallback(
        //        //        delegate( object param )
        //        //        {
        //        //            EmoEngineClient val = (EmoEngineClient)param;
        //        //            SetValue( CurrentDataFrameProperty, value );

        //        //            return null;

        //        //        } ), value );

        //        SetValue( DataFrameSourceProperty, value );
        //    }
        //}

        //// Using a DependencyProperty as the backing store for DataFrameSource.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DataFrameSourceProperty =
        //    DependencyProperty.Register( "DataFrameSource", typeof( EmoEngineClient ), typeof( TimeSeriesControl ) );


        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region TraceStroke Dependency Property

        public static readonly DependencyProperty TraceStrokeProperty =
            DependencyProperty.Register(
            "TraceStroke",
            typeof( Brush ),
            typeof( SampleDisplayControl ),
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
            SampleDisplayControl tsc = d as SampleDisplayControl;

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
            typeof( SampleDisplayControl ),
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
            SampleDisplayControl tsc = d as SampleDisplayControl;

            double thickness = (double)e.NewValue;

            // TBD: Big old hack. Template hasn't been applied.
            if( tsc._signalPolyline != null )
            {
                tsc._signalPolyline.StrokeThickness = thickness;
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
            typeof( SampleDisplayControl ),
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
            //TimeSeriesControl tsc = d as TimeSeriesControl;

            // TBD: Hack
            //if( tsc._counterNameTextBlock != null )
            //{
            //    tsc._counterNameTextBlock.Visibility = ( (bool)e.NewValue ) ? Visibility.Visible : Visibility.Hidden;
            //}

            //if( tsc._currentValueTextBlock != null )
            //{
            //    tsc._currentValueTextBlock.Visibility = ( (bool)e.NewValue ) ? Visibility.Visible : Visibility.Hidden;
            //}
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region CurrentValue Dependency Property

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
            "CurrentValue",
            typeof( float ),
            typeof( SampleDisplayControl ),
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
                    //this.InitializePolyLine( (int)sizeInfo.NewSize.Width );

                    //if( this.PerformanceMonitor != null )
                    //{
                    //    this.PerformanceMonitor.DisplayBufferSize = (int)sizeInfo.NewSize.Width;
                    //}
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
            get;
            set;
        }

        public void UpdateDisplay( double[] data )
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(
                    delegate( object param )
                    {
                        double[] val = (double[])param;
                        RenderData( val );
                        InvalidateVisual();
                        return null;

                    } ), data );
        }

        private void RenderData( double[] data )
        {
            double scaleFactor = 1;

            this.MaxValue = data.Max();
            this.MinValue = data.Min();

            if( this.AutoScale )
            {
                //this.MaxValue = double.MinValue;
                //this.MinValue = double.MaxValue;

                //if( this.MaxValue != double.MinValue && this.MinValue != double.MaxValue )
                {
                    double maxValueAbs = Math.Abs( this.MaxValue );
                    double minValueAbs = Math.Abs( this.MinValue );
                    double span = maxValueAbs + minValueAbs + 10; ;

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




            double xIncrement = this.ActualWidth / this.BufferSize;
            //xIncrement /= 2;

            if( this._signalPolyline.Points.Count == 0 || this._signalPolyline.Points.Count != data.Length )
            {
                this._signalPolyline.Points = new PointCollection( data.Length );

                for( int i = 0; i < data.Length; i++ )
                {
                    this._signalPolyline.Points.Add( new Point() );
                }
            }
            
            double xCurrent = 0;

            for( int j = 0; j < data.Length; j++ )
            {
                double y = this.ActualHeight - ( scaleFactor * data[j] ) - this.ActualHeight / 2.0d;
                Point p = new Point( xCurrent, y );
                this._signalPolyline.Points[j] = p;

                if( data[j] < this.MinValue )
                {
                    this.MinValue = data[j];
                }

                if( data[j] > this.MaxValue )
                {
                    this.MaxValue = data[j];
                }

                xCurrent += xIncrement;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        #region Fields

        /// <summary>
        /// The name of the Polyline template part that traces the signal.
        /// </summary>
        private const string polylineName = "PART_SignalPolyline";

        /// <summary>
        /// The name of the TextBlock template part that shows the 
        /// name of the signal.
        /// </summary>
        private const string textBlockName = "PART_SignalNameTextBlock";

        /// <summary>
        /// The name of the TextBlock template part that shows the 
        /// signal's current value.
        /// </summary>
        private const string currentValueTextBlockName = "PART_CurrentValueTextBlock";

        TextBlock _signalNameTextBlock;
        TextBlock _currentValueTextBlock;
        Polyline _signalPolyline;

        //protected PropertyChangedEventHandler propertyChanged = null;

        #endregion

    }
}
