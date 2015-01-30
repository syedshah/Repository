namespace IdentityWrapper.Interfaces
{
    using System.Web;

    /// <summary>
    /// HttpContextBase Provider
    /// </summary>
    public interface IHttpContextBaseProvider
    {
        HttpContextBase HttpContext { get; }

    }
}
