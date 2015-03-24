using System.Web.Routing;

namespace EPiServerSamples
{
    public interface ICustomAttributesInContentArea
    {
        RouteValueDictionary CustomAttributes { get; }

        bool ReplaceExisitingAttributes { get; }
    }
}