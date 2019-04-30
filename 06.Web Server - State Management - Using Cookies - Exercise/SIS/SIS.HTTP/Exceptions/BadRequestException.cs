namespace SIS.HTTP.Exceptions
{
    using SIS.HTTP.Enums;
    using System;

    public class BadRequestException : Exception
    {
        public const HttpResponseStatusCode StatusCode = HttpResponseStatusCode.BadRequest; 

        public override string Message => "The Request was malformed or contains unsupported elements.";
    }
}
