using System;
using System.Reflection;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}