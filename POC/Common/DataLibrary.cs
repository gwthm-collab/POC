using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Common
{
    enum PocPages_t
    {
        LoginPage,
        MainMenuPage,
    }

    enum EventContainer_enum
    {
        cancel,
    }

    public class Customer
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string TelegramNumber { get; set; }
        public string Email_ID { get; set; }
    }

    class DB_StoredProcedures
    {
        #region Customer
        public static readonly string CUSTOMER_INSERT = @"CALL uspInsertCustomer('{0}', '{1}', '{2}', '{3}', '{4}')";//Name, mobile, address, telegramNumber, email
        public static readonly string CUSTOMER_GET = @"CALL uspGetCustomer('{0}', '{1}', '{2}', '{3}')";//mobile, address, name, email
        public static readonly string CUSTOMER_UPDATE = @"CALL uspUpdateCustomer({0}, '{1}', '{2}', '{3}', '{4}', '{5}')";//mobile, address, name, email 
        #endregion
        #region HSN
        public static readonly string HSN_GET = @"CALL uspGetHSN()";
        public static readonly string HSN_UPDATE = @"CALL uspUpdateHSN({0}, '{1}', '{2}', {3}, {4}, {5}, {6}, {7})";//hsncode, goods desc, cgst, sgst, igst, cess, isvalid
        public static readonly string HSN_INSERT = @"CALL uspInsertHSN('{0}', '{1}', {2}, {3}, {4}, {5}, {6})";//hsncode, goods desc, cgst, sgst, igst, cess, isvalid 
        #endregion
        #region Product Category
        public static readonly string PRODCATEGORY_GET = @"CALL uspGetProductCategory()";
        public static readonly string PRODCATEGORY_UPDATE = @"CALL uspUpdateProductCategory({0}, '{1}', {2}, {3})";//productcategory, hsncode, isvalid
        public static readonly string PRODCATEGORY_INSERT = @"CALL uspInsertProductCategory('{0}', '{1}', {2})";//productcategory, hsncode, isvalid
        #endregion
    }
}
