using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U.DalilaDB
{

    /// <summary>
    /// This class is the result of a process
    /// The operation class can only be set to done once
    /// </summary>
    public class DataOperation<TValue>
    {

        // True when the operation is set as successful or failed
        protected bool isDone;
        public bool IsDone { get => isDone; }

        // True when the operation is set to succesful and false when is set to failed
        protected bool isSuccessful;
        public bool IsSuccessful { get => isSuccessful; }

        // Contains the exeption when the operation fails, and is null when the operation is successful
        protected Exception error;
        public Exception Error { get => error; }

        // Contains a message when the operation is successful, and is null when the operation is failed
        protected string message;
        public string Message { get => message; }

        protected TValue data;
        public TValue Data { get => data; }


        public DataOperation()
        {
            this.isDone = false;
            this.isSuccessful = false;
            this.error = null;
            this.message = "";
            data = default(TValue);
        }


        public DataOperation<TValue> Successful(TValue data, string message = "")
        {
            if (isDone)
                throw new DataOparationAlreadySetException();

            this.isDone = true;
            this.isSuccessful = true;
            this.message = message;
            this.data = data;
            return this;
        }



        public DataOperation<TValue> Fails(TValue data, Exception error)
        {
            if (isDone)
                throw new DataOparationAlreadySetException();

            this.isDone = true;
            this.error = error;
            this.data = data;
            return this;
        }


        public static implicit operator DataOperation(DataOperation<TValue> operation)
        {
            if (operation)
                return new DataOperation().Successful(operation.Message);
            else
                return new DataOperation().Fails(operation.Error);
        }



        /// <summary>
        /// Implicit cast of operation to bool
        /// </summary>
        /// <param name="operation">The operation</param>
        public static implicit operator bool(DataOperation<TValue> operation)
        {
            if (!operation.isDone)
                return false;
            else if (operation.Data == null)
                return false;
            else
                return operation.IsSuccessful;
        }


        /// <summary>
        /// Implicit cast of operation to string
        /// </summary>
        /// <param name="operation">The operation</param>
        // Automatic conversion to string
        public static implicit operator string(DataOperation<TValue> operation)
        {
            if (!operation.isDone)
            {
                return "In progress";
            }
            else if (operation)
            {
                if (operation.Message == null)
                    return "Completed successfully";
                else
                    return "Completed successfully: " + operation.Message;
            }
            else
            {
                return "Failed: " + operation.Error;
            }
        }


        public override string ToString()
        {
            return this + "";
        }



        /// <summary>
        /// Implicit cast to int
        /// </summary>
        /// <param name="operation"></param>
        public static implicit operator int(DataOperation<TValue> operation)
        {
            // Try to parse the message to a int
            if (Int32.TryParse(operation.message, out int value))
                return value;
            else
                return 0;
        }


        /// <summary>
        /// Implicit cast to float
        /// </summary>
        /// <param name="operation"></param>
        public static implicit operator float(DataOperation<TValue> operation)
        {
            // Try to parse the message to a int
            if (float.TryParse(operation.message, out float value))
                return value;
            else
                return 0;
        }




    }
}