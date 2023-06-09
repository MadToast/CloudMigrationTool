using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CloudMigrationTool.Core.Exceptions
{
    public class FileWasDirectoryException : Exception
    {
        //
        // Resumo:
        //     Initializes a new instance of the FileWasDirectoryException class.
        public FileWasDirectoryException() : base("Requested file was actually a directory")
        {
        }

        //
        // Resumo:
        //     Initializes a new instance of the FileWasDirectoryException class with a specified error
        //     message.
        //
        // Parâmetros:
        //   message:
        //     The message that describes the error.
        public FileWasDirectoryException(string message) : base(message)
        {
        }

        //
        // Resumo:
        //     Initializes a new instance of the FileWasDirectoryException class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parâmetros:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public FileWasDirectoryException(string message, Exception exception) : base(message, exception)
        {
        }
        //
        // Resumo:
        //     Initializes a new instance of the FileWasDirectoryException class with serialized data.
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
        protected FileWasDirectoryException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }
    }
}
