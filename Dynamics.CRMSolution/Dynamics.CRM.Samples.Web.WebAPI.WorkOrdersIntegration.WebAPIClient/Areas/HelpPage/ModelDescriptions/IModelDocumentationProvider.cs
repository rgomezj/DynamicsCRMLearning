using System;
using System.Reflection;

namespace Arcos.CUC.WorkOrdersIntegration.WebAPIClient.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}