using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CloudMigrationTool.Core.Exceptions
{
    public class CloudSourceNotAuthenticatedException : Exception
    {
        //
        // Resumo:
        //     Initializes a new instance of the CloudSourceNotAuthenticatedException class.
        public CloudSourceNotAuthenticatedException() : base()
        {
        }

        //
        // Resumo:
        //     Initializes a new instance of the CloudSourceNotAuthenticatedException class with a specified error
        //     message.
        //
        // Parâmetros:
        //   message:
        //     The message that describes the error.
        public CloudSourceNotAuthenticatedException(string message) : base(message) 
        {
        }

        //
        // Resumo:
        //     Initializes a new instance of the CloudSourceNotAuthenticatedException class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parâmetros:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public CloudSourceNotAuthenticatedException(string message, Exception exception) : base(message, exception)
        {
        }
        //
        // Resumo:
        //     Initializes a new instance of the CloudSourceNotAuthenticatedException class with serialized data.
        //
        // Parâmetros:
        //   info:
        //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        //     object data about the exception being thrown.
        //
        //   context:
        //     The System.Runtime.Serialization.StreamingContext that contains contextual information
        //     about the source or destination.
        //
        // Exceções:
        //   T:System.ArgumentNullException:
        //     The info parameter is null.
        //
        //   T:System.Runtime.Serialization.SerializationException:
        //     The class name is null or System.Exception.HResult is zero (0).
        protected CloudSourceNotAuthenticatedException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }
    }
}
