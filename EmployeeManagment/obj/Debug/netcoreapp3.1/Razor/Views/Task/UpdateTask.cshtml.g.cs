#pragma checksum "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3b2360c28def5a09a448bc5a73eedec04d9227e8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Task_UpdateTask), @"mvc.1.0.view", @"/Views/Task/UpdateTask.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3b2360c28def5a09a448bc5a73eedec04d9227e8", @"/Views/Task/UpdateTask.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56ac35128e1fdc2316bfa7215dc87cb66a8bf57b", @"/Views/_ViewImports.cshtml")]
    public class Views_Task_UpdateTask : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<EmployeeManagment.Models.Task>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Task/CommandBoard"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "created", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "in progress", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "done", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-blue"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("~/Task/UpdateTask"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div style=\"padding-left: 60px; padding-top: 20px;\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e86207", async() => {
                WriteLiteral("\r\n        <input type=\"hidden\" name=\"commandId\"");
                BeginWriteAttribute("value", " value=\"", 321, "\"", 345, 1);
#nullable restore
#line 9 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 329, Model.CommandId, 329, 16, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n        <input type=\"submit\" class=\"btn btn-warning\" value=\"Back\" \\>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n<div style=\"padding-top:70px;\">\r\n\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e88594", async() => {
                WriteLiteral("\r\n        <h3 style=\"color: aliceblue\">Update task</h3>\r\n        <p>\r\n            <input class=\"form-control\" type=\"text\"");
                BeginWriteAttribute("value", " value=\"", 665, "\"", 691, 1);
#nullable restore
#line 18 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 673, Model.Description, 673, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" name=\"task.Description\" placeholder=\"Description\" required />\r\n        </p>\r\n        <input type=\"hidden\" name=\"task.Id\"");
                BeginWriteAttribute("value", " value=\"", 813, "\"", 830, 1);
#nullable restore
#line 20 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 821, Model.Id, 821, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n        <p>\r\n");
#nullable restore
#line 22 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
              
                string name;
                if (Model.Employee.FirstName != null && Model.Employee.SecondName != null)
                {
                    name = Model.Employee.FirstName + " " + Model.Employee.SecondName;
                }
                else
                {
                    name = Model.Employee.Login;
                }
            

#line default
#line hidden
#nullable disable
                WriteLiteral("            <div class=\"input-group mb-3\" style=\"padding-left: 95px;padding-right: 95px;\">\r\n                <select class=\"custom-select\" name=\"task.EmployeeId\"");
                BeginWriteAttribute("value", " value=\"", 1396, "\"", 1421, 1);
#nullable restore
#line 34 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 1404, Model.EmployeeId, 1404, 17, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e811107", async() => {
#nullable restore
#line 35 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                                          Write(name);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 35 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                WriteLiteral(Model.EmployeeId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 36 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                     foreach (var e in ViewBag.Employees)
                    {
                        if (Model.EmployeeId != e.Id)
                        {
                            string n;
                            if (e.FirstName != null && e.SecondName != null)
                            {
                                n = e.FirstName + " " + e.SecondName;
                            }
                            else
                            {
                                n = e.Login;
                            }

#line default
#line hidden
#nullable disable
                WriteLiteral("                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e814145", async() => {
#nullable restore
#line 49 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                             Write(n);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 49 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                               WriteLiteral(e.Id);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 50 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                        }
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                </select>\r\n            </div>\r\n        </p>\r\n        <p>\r\n            <input class=\"form-control\" type=\"number\"");
                BeginWriteAttribute("value", " value=\"", 2300, "\"", 2325, 1);
#nullable restore
#line 56 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 2308, Model.Complexity, 2308, 17, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" name=""task.Complexity"" placeholder=""Complexity"" min=""0"" max=""23"" oninvalid=""setCustomValidity('Please, enter value >= 0 and <= 23')"" required />
        </p>
        <p>
            <div class=""input-group mb-3"" style=""padding-left: 95px;padding-right: 95px;"">
                <select class=""custom-select"" name=""task.Status""");
                BeginWriteAttribute("value", " value=\"", 2656, "\"", 2677, 1);
#nullable restore
#line 60 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 2664, Model.Status, 2664, 13, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e817602", async() => {
#nullable restore
#line 61 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                                      Write(Model.Status);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 61 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                WriteLiteral(Model.Status);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 62 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                     if (Model.Status != "created")
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e820156", async() => {
                    WriteLiteral("created");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_2.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 65 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 66 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                     if (Model.Status != "in progress")
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e821897", async() => {
                    WriteLiteral("in progress");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 69 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 70 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                     if (Model.Status != "done")
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3b2360c28def5a09a448bc5a73eedec04d9227e823635", async() => {
                    WriteLiteral("done");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 73 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                </select>\r\n            </div>\r\n        </p>\r\n        <p>\r\n            <div style=\"padding-left:95px; color: aliceblue;\"> Start: ");
#nullable restore
#line 78 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
                                                                 Write(Model.Start.ToString("yyyy-MM-dd"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n        </p>\r\n        <p>\r\n            <input class=\"form-control\" type=\"date\"");
                BeginWriteAttribute("value", " value=\"", 3527, "\"", 3568, 1);
#nullable restore
#line 81 "C:\Users\Kateryna\source\repos\EmployeeManagment\EmployeeManagment\Views\Task\UpdateTask.cshtml"
WriteAttributeValue("", 3535, Model.End.ToString("yyyy-MM-dd"), 3535, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" name=""task.End"" required />
        </p>

        <p>
            <div style=""padding-bottom:15px; padding-left:155px;"">
                <input class=""btn btn-warning"" style=""margin:auto; "" type=""submit"" value=""Update"">
            </div>
        </p>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<EmployeeManagment.Models.Task> Html { get; private set; }
    }
}
#pragma warning restore 1591
