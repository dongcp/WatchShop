using Models.EF;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WatchShopService
{
    /// <summary>
    /// Summary description for ProductService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProductService : System.Web.Services.WebService
    {

        [WebMethod]
        public List<Product> GetAllProducts()
        {
            return ProductDAO.Instance.GetAllProducts();
        }
    }
}
