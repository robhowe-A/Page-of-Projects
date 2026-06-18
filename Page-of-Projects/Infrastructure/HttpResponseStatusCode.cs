// --Copyright (c) 2026 Robert A. Howell
//Source: https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-8.0
//Date Accessed: 9/4/2024
//Namespace: System.Net
//Assembly: System.Net.Primitives.dll

using System.Net;

namespace ProjectsPage.Infrastructure;

public class HttpResponseStatusCode
{
    public static int GetResponseHttpStatusCode(HttpStatusCode statusCode)
    {
        return statusCode switch
               {
                       HttpStatusCode.Accepted => 202,
                       HttpStatusCode.AlreadyReported => 208,
                       HttpStatusCode.Ambiguous => 300,
                       HttpStatusCode.BadGateway => 502,
                       HttpStatusCode.BadRequest => 400,
                       HttpStatusCode.Conflict => 409,
                       HttpStatusCode.Continue => 100,
                       HttpStatusCode.Created => 201,
                       HttpStatusCode.EarlyHints => 103,
                       HttpStatusCode.ExpectationFailed => 417,
                       HttpStatusCode.FailedDependency => 424,
                       HttpStatusCode.Forbidden => 403,
                       HttpStatusCode.Found => 302,
                       HttpStatusCode.GatewayTimeout => 504,
                       HttpStatusCode.Gone => 410,
                       HttpStatusCode.HttpVersionNotSupported => 505,
                       HttpStatusCode.IMUsed => 226,
                       HttpStatusCode.InsufficientStorage => 507,
                       HttpStatusCode.InternalServerError => 500,
                       HttpStatusCode.LengthRequired => 411,
                       HttpStatusCode.Locked => 423,
                       HttpStatusCode.LoopDetected => 508,
                       HttpStatusCode.MethodNotAllowed => 405,
                       HttpStatusCode.MisdirectedRequest => 421,
                       HttpStatusCode.Moved => 301,

                       //case HttpStatusCode.MovedPermanently => return;
                       //case HttpStatusCode.MultipleChoices => return;
                       HttpStatusCode.MultiStatus => 207,
                       HttpStatusCode.NetworkAuthenticationRequired => 511,
                       HttpStatusCode.NoContent => 204,
                       HttpStatusCode.NonAuthoritativeInformation => 203,
                       HttpStatusCode.NotAcceptable => 406,
                       HttpStatusCode.NotExtended => 510,
                       HttpStatusCode.NotFound => 404,
                       HttpStatusCode.NotImplemented => 501,
                       HttpStatusCode.NotModified => 304,
                       HttpStatusCode.OK => 200,
                       HttpStatusCode.PartialContent => 206,
                       HttpStatusCode.PaymentRequired => 402,
                       HttpStatusCode.PermanentRedirect => 308,
                       HttpStatusCode.PreconditionFailed => 412,
                       HttpStatusCode.PreconditionRequired => 428,
                       HttpStatusCode.Processing => 102,
                       HttpStatusCode.ProxyAuthenticationRequired => 407,

                       //case HttpStatusCode.Redirect => return;
                       HttpStatusCode.RedirectKeepVerb => 307,
                       HttpStatusCode.RedirectMethod => 303,
                       HttpStatusCode.RequestedRangeNotSatisfiable => 416,
                       HttpStatusCode.RequestEntityTooLarge => 413,
                       HttpStatusCode.RequestHeaderFieldsTooLarge => 431,
                       HttpStatusCode.RequestTimeout => 408,
                       HttpStatusCode.RequestUriTooLong => 414,
                       HttpStatusCode.ResetContent => 205,

                       //case HttpStatusCode.SeeOther => return;
                       HttpStatusCode.ServiceUnavailable => 503,
                       HttpStatusCode.SwitchingProtocols => 101,

                       //case HttpStatusCode.TemporaryRedirect => return;
                       HttpStatusCode.TooManyRequests => 429,
                       HttpStatusCode.Unauthorized => 401,
                       HttpStatusCode.UnavailableForLegalReasons => 451,
                       HttpStatusCode.UnprocessableContent => 422,

                       //case HttpStatusCode.UnprocessableEntity=> return;
                       HttpStatusCode.UnsupportedMediaType => 415,
                       HttpStatusCode.Unused => 306,
                       HttpStatusCode.UpgradeRequired => 426,
                       HttpStatusCode.UseProxy => 305,
                       HttpStatusCode.VariantAlsoNegotiates => 506,
                       _ => 0
               };
    }
};
