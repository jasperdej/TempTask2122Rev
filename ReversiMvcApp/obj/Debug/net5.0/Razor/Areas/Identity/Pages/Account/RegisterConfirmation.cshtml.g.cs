#pragma checksum "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "efd16b3b670b1e65103b769bcff9bb71f0706bba"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(ReversiMvcApp.Areas.Identity.Pages.Account.Areas_Identity_Pages_Account_RegisterConfirmation), @"mvc.1.0.razor-page", @"/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml")]
namespace ReversiMvcApp.Areas.Identity.Pages.Account
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
#line 1 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\_ViewImports.cshtml"
using ReversiMvcApp.Areas.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\_ViewImports.cshtml"
using ReversiMvcApp.Areas.Identity.Pages;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\_ViewImports.cshtml"
using ReversiMvcApp.Areas.Identity.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\_ViewImports.cshtml"
using ReversiMvcApp.Areas.Identity.Pages.Account;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"efd16b3b670b1e65103b769bcff9bb71f0706bba", @"/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"836380311b7de813c15f5e3d8ec68bea8b531329", @"/Areas/Identity/Pages/_ViewImports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e20d5008fa98b890766d7e481c5e5246274b145", @"/Areas/Identity/Pages/Account/_ViewImports.cshtml")]
    public class Areas_Identity_Pages_Account_RegisterConfirmation : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
  
    ViewData["Title"] = "Register confirmation";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>");
#nullable restore
#line 7 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n");
#nullable restore
#line 8 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
  
    if (@Model.DisplayConfirmAccountLink)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("\t<p>\r\n\t\tBedankt voor je aanmelding bij Reversi!<br />Voordat u begint met spelen, bevestig je account door <a id=\"confirm-link\"");
            BeginWriteAttribute("href", " href=\"", 310, "\"", 344, 1);
#nullable restore
#line 12 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
WriteAttributeValue("", 317, Model.EmailConfirmationUrl, 317, 27, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">hier te klikken.</a>\r\n\t</p>\r\n");
#nullable restore
#line 14 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<p>\r\n        Please check your email to confirm your account.\r\n</p>\r\n");
#nullable restore
#line 20 "C:\Users\totti\Desktop\ReversiMvcApp\ReversiMvcApp\Areas\Identity\Pages\Account\RegisterConfirmation.cshtml"
    }

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<RegisterConfirmationModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<RegisterConfirmationModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<RegisterConfirmationModel>)PageContext?.ViewData;
        public RegisterConfirmationModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591