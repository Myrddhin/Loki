using System;
using System.Runtime.Serialization;

namespace Loki.Common
{
    /// <summary>
    /// Loki base exception class.
    /// </summary>
    [Serializable]
    public class LokiException : Exception
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        public LokiException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public LokiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public LokiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected LokiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(128);

            // Make sure we get the standard output
            builder.Append(base.ToString());

            // Since the standard output can be nothing, we only add our variables when necessary
            if (builder.Length > 0)
            {
                builder.Append(Environment.NewLine);
                builder.AppendFormat(Formatter, Code, Message);
            }

            return builder.ToString();
        }

        private const string Formatter = "Code {0} - Message {1}";
    }
}