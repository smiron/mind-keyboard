using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmoEngineClientLibrary
{
    public delegate void BufferFilledEventHandler( object sender, BufferFilledEventArgs bfea );

    public class BufferFilledEventArgs : EventArgs 
    {
        public BufferFilledEventArgs( double[] buffer, EE_DataChannel_t channel )
        {
            this.Buffer = buffer;
            this.Channel = channel;
        }

        public double[] Buffer
        {
            get;
            private set;
        }

        public EE_DataChannel_t Channel
        {
            get;
            private set;
        }
    }
}
