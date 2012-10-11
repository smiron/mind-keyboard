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

using EmoEngineClientLibrary;

namespace EmoEngineControlLibrary
{
    public class NeuroHeadsetStatusControl : Control
    {
        static NeuroHeadsetStatusControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( 
                typeof( NeuroHeadsetStatusControl ), 
                new FrameworkPropertyMetadata( typeof( NeuroHeadsetStatusControl ) ) );
        }

        ///////////////////////////////////////////////////////////////////////
        #region HeadsetDataSource Dependency Property

        public EmoEngineClient HeadsetDataSource
        {
            get;
            set;
        }

        #endregion

    }
}
