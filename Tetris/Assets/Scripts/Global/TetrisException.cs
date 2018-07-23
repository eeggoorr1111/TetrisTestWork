using System;
using System.Runtime.Serialization;

namespace Tetris.Global
{
    public class TetrisException : ApplicationException
    {
        public TetrisException() { }
        public TetrisException(string message) : base(message) { }
        public TetrisException(string message, Exception inner) : base(message, inner) { }
        protected TetrisException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
} 
