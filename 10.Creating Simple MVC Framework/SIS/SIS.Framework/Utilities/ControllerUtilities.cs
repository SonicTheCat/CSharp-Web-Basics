namespace SIS.Framework.Utilities
{
    public class ControllerUtilities
    {
        private const string RootDirectoryRelativePath = "../../../";
        private const string fileExtensionsHtml = ".html";

        public static string GetControllerName(object controller)
        {
            return controller
                .GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty);
        }

        public static string GetViewFullQualifiedName(string controller, string viewName)
        {
            return string.Format("{0}/{1}/{2}",
                RootDirectoryRelativePath + MvcContext.Get.ViewsFolder,
                controller,
                viewName + fileExtensionsHtml);
        }
    }
}