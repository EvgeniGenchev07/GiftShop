using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string ChangePassword => "ChangePassword";

        public static string DownloadPersonalData => "DownloadPersonalData";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string AccountDetails => "AccountDetails";

        public static string PersonalData => "PersonalData";

        public static string Orders => "Orders";
        public static string OrdersAdmin => "OrdersAdmin";
        public static string Feedbacks => "Feedbacks";
        public static string FeedbacksAdmin => "FeedbacksAdmin";
        public static string ProductsAdmin => "ProductsAdmin";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string OrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Orders);
        public static string OrdersAdminNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrdersAdmin);
        public static string FeedbacksNavClass(ViewContext viewContext) => PageNavClass(viewContext, Feedbacks);
        public static string FeedbacksAdminNavClass(ViewContext viewContext) => PageNavClass(viewContext, FeedbacksAdmin);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string AccountDetailsNavClass(ViewContext viewContext) => PageNavClass(viewContext, AccountDetails);
        public static string ProductsAdminNavClass(ViewContext viewContext) => PageNavClass(viewContext, ProductsAdmin);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
