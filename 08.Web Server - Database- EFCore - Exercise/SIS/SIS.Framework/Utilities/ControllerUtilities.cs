namespace SIS.Framework.Utilities
{
    public class ControllerUtilities
    {
        public static string GetControllerName(object controller)
        {
            return controller
                .GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty); 
        }

        public static string GetViewFullQualifiedName(string controller, string viewName)
        {
            return $"{MvcContext.Get.ViewsFolder}\\{controller}\\{viewName}"; 
        }
    }
}