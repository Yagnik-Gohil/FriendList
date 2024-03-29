#pragma checksum "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "566731d5e9c6bc506fcbd577fe47485fbc78b177"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_FriendList_FriendRequest), @"mvc.1.0.view", @"/Views/FriendList/FriendRequest.cshtml")]
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
#line 1 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\_ViewImports.cshtml"
using FriendList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\_ViewImports.cshtml"
using FriendList.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
using Microsoft.AspNetCore.Http.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 36 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"566731d5e9c6bc506fcbd577fe47485fbc78b177", @"/Views/FriendList/FriendRequest.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"adb00f87eaafc74d2fcb0cfb9f21092e4667a44a", @"/Views/_ViewImports.cshtml")]
    public class Views_FriendList_FriendRequest : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<DataAccessLayer.Entities.UserTable>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
  
    ViewData["Title"] = "Friend Requests";

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
  

    var userId = Context.Session.GetString("userId");

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"text-center\">\r\n    <h2 class=\"display-4\">Friend Requests</h2>\r\n</div>\r\n\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 37 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
Write(Html.Kendo().Grid<DataAccessLayer.Entities.UserTable>()
        .Name("grid")
        .Columns(columns =>
        {
            columns.Bound(c => c.FirstName).ClientTemplate("#=FirstName# #=LastName#").Title("Name");
            columns.Bound(c => c.UID).ClientTemplate("#=selectButton(data)#").Title("Action");

        })
        .DataSource(dataSource => dataSource
            .Ajax()
            .Model(model => model.Id(s => s.UID))
            .Read(read => read.Action("FriendRequestRead", "FriendList"))
        )
);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<script>\r\n    function selectButton(data) {\r\n        \r\n        var template = kendo.format(\"<a class=\\\"k-button\\\" href=\'\" + \'");
#nullable restore
#line 55 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
                                                                 Write(Url.Action("AcceptRequest", "FriendList", new { uid = "_uid_", ReturnUrl = Context.Request.GetEncodedUrl() }).Replace("_uid_","{0}" ));

#line default
#line hidden
#nullable disable
            WriteLiteral("\' + \" \'>Accept</a> \", data.UID) + kendo.format(\"<a class=\\\"k-button\\\" href=\'\" + \'");
#nullable restore
#line 55 "C:\Users\pca220\source\repos\FriendList\FriendList\Views\FriendList\FriendRequest.cshtml"
                                                                                                                                                                                                                                                                                        Write(Url.Action("CancelRequest", "FriendList", new { uid = "_uid_", ReturnUrl = Context.Request.GetEncodedUrl(), status = "reject" }).Replace("_uid_", "{0}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\' + \" \'>Reject</a> \", data.UID)\r\n        return template;\r\n        \r\n    }\r\n\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<DataAccessLayer.Entities.UserTable>> Html { get; private set; }
    }
}
#pragma warning restore 1591
