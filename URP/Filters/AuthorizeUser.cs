using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URP.Models;

namespace URP.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private Users oUser;
        private URPDBEntities db = new URPDBEntities();
        private int OID;

        public AuthorizeUser(int OID = 0)
        {
            this.OID = OID;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            String OName = "";
            String MName = "";

            try
            {
                oUser = (Users)HttpContext.Current.Session["User"];
                var OLst = from m in db.RolOperations
                           where m.RID == oUser.RID
                           && m.OID == OID
                           select m;

                if (OLst.ToList().Count() < 1)
                {
                    var oOperacion = db.Operations.Find(OID);
                    int? MID = oOperacion.MID;
                    OName = getOName(OID);
                    MName = getMName(MID);
                    filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?OP=" + OName);
                }
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?OP=" + OName);
            }
        }

        public string getOName(int OID)
        {
            var OP = from op in db.Operations
                     where op.OID == OID
                     select op.OName;
            string OName;
            try
            {
                OName = OP.First();
            }
            catch (Exception)
            {

                OName = "";
            }
            return OName;
        }

        public string getMName(int? MID)
        {
            var MO = from m in db.Modules
                     where m.MID == MID
                     select m.MName;
            string MName;
            try
            {
                MName = MO.First();
            }
            catch (Exception)
            {

                MName = "";
            }
            return MName;
        }
    }
}