using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace EmoEngineControlLibrary
{
    /// <summary>
    /// Provides a shim class that enables assigning a Visual as the child of a standard element.
    /// </summary>
    /// <remarks>Dwayne Need: The first problem to solve is that the HostVisual class derives from Visual.  
    /// I can't use an existing panel, such as Border, to host this visual.  Border derives from Decorator, 
    /// which is the standard base class for panels that have a single child. Unfortunately, the child is 
    /// strongly typed to be a UIElement. I have to use a HostVisual, which does not derive from UIElement. 
    /// There is no built-in way that I know of to place a Visual as a child of one of the standard elements 
    /// (such as Border, Grid, Canvas, etc). So we make our own. 
    /// http://blogs.msdn.com/dwayneneed/archive/2007/04/26/multithreaded-ui-hostvisual.aspx
    /// </remarks>
    [ContentProperty("Child")]
    public class VisualWrapper : FrameworkElement
    {
        public Visual Child
        {
            get
            {
                return _child;
            }

            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }

                _child = value;

                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (_child != null && index == 0)
            {
                return _child;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _child != null ? 1 : 0;
            }
        }

        private Visual _child;
    }
}


