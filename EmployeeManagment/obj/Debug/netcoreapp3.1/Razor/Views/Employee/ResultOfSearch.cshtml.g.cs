#pragma checksum "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "383a9e283c97170b25b3bb7af77a10cd33080a95"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employee_ResultOfSearch), @"mvc.1.0.view", @"/Views/Employee/ResultOfSearch.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\_ViewImports.cshtml"
using EmployeeManagment;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\_ViewImports.cshtml"
using EmployeeManagment.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"383a9e283c97170b25b3bb7af77a10cd33080a95", @"/Views/Employee/ResultOfSearch.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56ac35128e1fdc2316bfa7215dc87cb66a8bf57b", @"/Views/_ViewImports.cshtml")]
    public class Views_Employee_ResultOfSearch : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<EmployeeManagment.Models.UserProfile>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-warning"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("role", new global::Microsoft.AspNetCore.Html.HtmlString("button"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/Employee/Search"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Employee/AddEmployee"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
  
    ViewData["Title"] = "Search";

#line default
#line hidden
#nullable disable
            WriteLiteral("<p style=\"padding-left: 60px; padding-top: 20px;\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "383a9e283c97170b25b3bb7af77a10cd33080a955521", async() => {
                WriteLiteral("\r\n        Back\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</p>\r\n<div style=\"padding-top: 55px;\">\r\n");
#nullable restore
#line 11 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
     if (Model.Count() != 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <h3 style=""color:#2874A6"">Results of search</h3>
        <table class=""table-blue"" style=""padding-bottom:7px;"">
            <tr>
                <th>Login</th>
                <th>Name</th>
                <th>Surname</th>
                <th>Command</th>
                <th>Level</th>
                <th>Work from home</th>
                <th>Work from office</th>
                <th>Experience</th>
                <th>Average task complexity</th>
            </tr>
");
#nullable restore
#line 26 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
             foreach (var user in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr style=\"padding-top:7px;\">\r\n                    <td>");
#nullable restore
#line 29 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.Login);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 30 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 31 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.SecondName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 32 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.CommandId);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 33 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.Level);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 34 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.AbilityToWorkInHome);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 35 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.AbilityToWorkInOffice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 36 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.Experience);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 37 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
                   Write(user.AverageComplexityOfTasks);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "383a9e283c97170b25b3bb7af77a10cd33080a9510614", async() => {
                WriteLiteral("\r\n                            <input type=\"hidden\"");
                BeginWriteAttribute("value", " value=\"", 1518, "\"", 1534, 1);
#nullable restore
#line 40 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
WriteAttributeValue("", 1526, user.Id, 1526, 8, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" name=""Id"">
                            <div style=""padding-bottom:7px;"">
                                <input class=""btn btn-warning"" style=""margin:auto; padding-bottom: 7px;"" type=""submit"" value=""Add"">
                            </div>
                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 47 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </table>\r\n");
#nullable restore
#line 49 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 style=\"color:#2874A6\">Results of search</h3>\r\n");
#nullable restore
#line 53 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Employee\ResultOfSearch.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<EmployeeManagment.Models.UserProfile>> Html { get; private set; }
    }
}
#pragma warning restore 1591
