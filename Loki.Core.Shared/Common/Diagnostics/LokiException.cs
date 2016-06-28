using System;
using System.Text;

namespace Loki.Common.Diagnostics
{
    /// <summary>
    /// Loki base exception class.
    /// </summary>
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
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message.
        /// </param>
        public LokiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message.
        /// </param>
        /// <param name="innerException">
        /// Inner exception.
        /// </param>
        public LokiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder(128);

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