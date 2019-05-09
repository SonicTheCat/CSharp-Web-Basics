namespace SIS.HTTP.Exceptions
{
    using SIS.HTTP.Enums;
    using System;

    public class InternalServerErrorException : Exception
    {
        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.InternalServerError;

        public override string Message => "The Server has encountered an error."; 
    }
}