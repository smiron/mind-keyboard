// Copyright © 2010 James Galasyn 

using System;
using System.Windows.Data;
using System.Windows.Media;

using System.Diagnostics;
using System.Globalization;
using EmoEngineClientLibrary;

namespace EmoEngineControlLibrary
{
    [ValueConversion( typeof( EE_EEG_ContactQuality_t ), typeof( Brush ) )]
    class EeContactQualityToBrushConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture )
        {
            Brush b = null;

            // Only apply the conversion if value is assigned and
            // is of type bool.
            if( value != null &&
                value.GetType() == typeof( EE_EEG_ContactQuality_t ) )
            {
                var contactQuality = (EE_EEG_ContactQuality_t)value;
                switch( contactQuality )
                {
                    case EE_EEG_ContactQuality_t.EEG_CQ_NO_SIGNAL:
                        {
                            b = Brushes.Black;
                            break;
                        }

                    case EE_EEG_ContactQuality_t.EEG_CQ_VERY_BAD:
                        {
                            b = Brushes.Red;
                            break;
                        }

                    case EE_EEG_ContactQuality_t.EEG_CQ_POOR:
                        {
                            b = Brushes.Orange;
                            break;
                        }

                    case EE_EEG_ContactQuality_t.EEG_CQ_FAIR:
                        {
                            b = Brushes.Yellow;
                            break;
                        }

                    case EE_EEG_ContactQuality_t.EEG_CQ_GOOD:
                        {
                            b = Brushes.Green;
                            break;
                        }

                    default:
                        {
                            Trace.Assert( false, "Unknown EE_EEG_ContactQuality_t value" );
                            break;
                        }
                }
            }

            return b;
        }

        // Not used.
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture )
        {
            return null;
        }
    }
}
