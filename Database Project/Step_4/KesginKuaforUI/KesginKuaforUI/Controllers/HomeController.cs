using KesginKuaforUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KesginKuaforUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=KESGIN_KUAFOR;Integrated Security=True;");

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "select * from Product";
            sqlCommand.Connection = sqlConnection;

            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            List<ProductList> productLists = new List<ProductList>();
            while (sqlDataReader.Read())
            {
                ProductList product = new ProductList();
                product.productID = Convert.ToInt32(sqlDataReader["productID"]);
                product.productName = sqlDataReader["productName"].ToString();
                product.productType = sqlDataReader["productType"].ToString();
                product.price = Convert.ToInt32(sqlDataReader["price"]);
                product.quantity = sqlDataReader["quantity"].ToString();
                productLists.Add(product);
            }
            sqlConnection.Close();
            sqlConnection.Open();

            sqlCommand.CommandText = "select * from Category";

            sqlDataReader = sqlCommand.ExecuteReader();
            List<CategoryList> categoryLists = new List<CategoryList>();
            while (sqlDataReader.Read())
            {
                CategoryList category = new CategoryList();
                category.categoryID = Convert.ToInt32(sqlDataReader["categoryID"]);
                category.categoryName = sqlDataReader["categoryName"].ToString();
                category.price = Convert.ToInt32(sqlDataReader["price"]);
                categoryLists.Add(category);
            }
            sqlConnection.Close();
            CatAndProductList catAndProductList = new CatAndProductList();
            catAndProductList.categoryLists = categoryLists;
            catAndProductList.productLists = productLists;
            return View(catAndProductList);
        }




        
        [HttpPost()]
        public ActionResult getReceiptCategory(int receiptID)
        {

            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=KESGIN_KUAFOR;Integrated Security=True;");

            sqlConnection.Open();

            SqlCommand sqlCommand = null;

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select * from Receipt where receiptID=" + receiptID;


            SqlDataReader mySqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            string priceDate = "";
            int totalPrice = 0;
            if (mySqlDataReader.Read())
            {
                priceDate =  Convert.ToDateTime(mySqlDataReader["date"]).ToString();
                totalPrice = Convert.ToInt32(mySqlDataReader["totalPrice"]);

            }
            sqlConnection.Close();

            sqlConnection.Close();


            return Json( new { priceDate, totalPrice }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public ActionResult addCategoryPost(PostCategory postCategory)
        {

            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=KESGIN_KUAFOR;Integrated Security=True;");

            sqlConnection.Open();

            SqlCommand sqlCommand = null;

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select * from Category where categoryID=" + postCategory.selectedCategory;


            SqlDataReader mySqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            int categoryPrice = 0;
            if (mySqlDataReader.Read())
            {
                categoryPrice = Convert.ToInt32(mySqlDataReader["price"]);

            }
            sqlConnection.Close();

            sqlConnection.Open();




            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "Sp_CreateReservationReceipt";

            sqlCommand.Parameters.AddWithValue("@receiptID", postCategory.receiptID);
            sqlCommand.Parameters.AddWithValue("@newCategoryID", postCategory.selectedCategory);
            sqlCommand.Parameters.AddWithValue("@price", categoryPrice);

            int i = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();


            return Json(i, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public ActionResult TransactionPost(TransactionModel postProduct)
        {

            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=KESGIN_KUAFOR;Integrated Security=True;");

            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "Sp_GetCustomerID";

            sqlCommand.Parameters.AddWithValue("@customerName", postProduct.custName);
            sqlCommand.Parameters.AddWithValue("@customerLastname", postProduct.custLastName);


            sqlCommand.Parameters.Add("@customerID", SqlDbType.SmallInt);
            sqlCommand.Parameters["@customerID"].Direction = ParameterDirection.Output;
            sqlCommand.Connection = sqlConnection;

            int i = sqlCommand.ExecuteNonQuery();

            var customerID = sqlCommand.Parameters["@customerID"].Value;


            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "Sp_CreateReceipt";

            sqlCommand.Parameters.AddWithValue("@customerID", customerID);

            sqlCommand.Parameters.Add("@receiptID", SqlDbType.SmallInt);
            sqlCommand.Parameters["@receiptID"].Direction = ParameterDirection.Output;

            i = sqlCommand.ExecuteNonQuery();

            var receiptId = Convert.ToString(sqlCommand.Parameters["@receiptId"].Value);


            //sqlCommand = new SqlCommand();
            //sqlCommand.Connection = sqlConnection;
            //sqlCommand.CommandText = "select * from Product where productID="+postProduct.selectedProduct;


            //SqlDataReader mySqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            //int productPrice=0;
            //if (mySqlDataReader.Read())
            //{
            //     productPrice= Convert.ToInt32(mySqlDataReader["price"]);
              
            //}
            //sqlConnection.Close();

            //sqlConnection.Open();




            //sqlCommand = new SqlCommand();
            //sqlCommand.Connection = sqlConnection;
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            //sqlCommand.CommandText = "Sp_CreateProductReceipt";

            //sqlCommand.Parameters.AddWithValue("@receiptID", receiptId);
            //sqlCommand.Parameters.AddWithValue("@newProductID", postProduct.selectedProduct);
            //sqlCommand.Parameters.AddWithValue("@newStorageID", 1);
            //sqlCommand.Parameters.AddWithValue("@price", productPrice);

            //i = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();


            return Json(receiptId, JsonRequestBehavior.AllowGet);
        }




        [HttpPost()]
        public ActionResult addProductPost(PostProduct postProduct)
        {

            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=KESGIN_KUAFOR;Integrated Security=True;");

            sqlConnection.Open();

            SqlCommand sqlCommand = null;

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select * from Product where productID=" + postProduct.selectedProduct;


            SqlDataReader mySqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
            int productPrice = 0;
            if (mySqlDataReader.Read())
            {
                productPrice = Convert.ToInt32(mySqlDataReader["price"]);

            }
            sqlConnection.Close();

            sqlConnection.Open();




            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "Sp_CreateProductReceipt";


            sqlCommand.Parameters.AddWithValue("@receiptID", postProduct.receiptID);
            sqlCommand.Parameters.AddWithValue("@newProductID", postProduct.selectedProduct);
            sqlCommand.Parameters.AddWithValue("@newStorageID", 1);
            sqlCommand.Parameters.AddWithValue("@price", productPrice);

            int i = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();


            return Json(i, JsonRequestBehavior.AllowGet);
        }


       

    }
}