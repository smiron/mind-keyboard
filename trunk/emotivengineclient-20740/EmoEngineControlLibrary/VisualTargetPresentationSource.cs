using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace EmoEngineControlLibrary
{
    /// <summary>
    /// Provides a root for an element tree. 
    /// </summary>
    /// <remarks> Dwayne Need says: WPF provides a very convenient event called <see cref="Loaded"/>. 
    /// This event basically signals when an element has been fully initialized, measured, arranged, 
    /// rendered, and plugged into a presentation source (such as a <see cref="Window"/>). Many elements 
    /// use this event, including the MediaElement, but sadly this event is not raised for element trees 
    /// that are not plugged into a presentation source, and displaying an element tree through the 
    /// HostVisual/VisualTarget doesn’t count. So work around this, we make our own presentation source 
    /// and use it to root the element tree that the worker thread will own. This immediately leads into 
    /// another problem: layout is suspended on all elements until resumed by a presentation source.  
    /// Unfortunately, the official mechanism to do this is internal, so the best we can do is to explicitly 
    /// measure and arrange the root element.  
    /// http://blogs.msdn.com/dwayneneed/archive/2007/04/26/multithreaded-ui-hostvisual.aspx
    /// </remarks>
    public class VisualTargetPresentationSource : PresentationSource
    {
        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
        }

        public override Visual RootVisual
        {
            get
            {
                return _visualTarget.RootVisual;
            }

            set
            {
                Visual oldRoot = _visualTarget.RootVisual;

                // Set the root visual of the VisualTarget.  This visual will
                // now be used to visually compose the scene.
                _visualTarget.RootVisual = value;

                // Tell the PresentationSource that the root visual has
                // changed.  This kicks off a bunch of stuff like the
                // Loaded event.
                RootChanged(oldRoot, value);

                // Kickoff layout...
                UIElement rootElement = value as UIElement;
                if (rootElement != null)
                {
                    rootElement.Measure(new Size(Double.PositiveInfinity,
                                                 Double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }

        public override bool IsDisposed
        {
            get
            {
                // We don't support disposing this object.
                return false;
            }
        }

        private VisualTarget _visualTarget;
    }
}
