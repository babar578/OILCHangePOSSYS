using Newtonsoft.Json;
using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace POS.Utilities.Utilities
{
    public static class CartUtility
    {
        #region Cart Utility


        public static bool AddItemIntoCart(CartItemViewModel cartItem = null)
        {
            bool returnValue = false;
            CartItemViewModel searchItem = null;
            CartItemViewModel findItem = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            //List<CartItemViewModel> items = GetCartItems();
            //double itemPrice = Convert.ToDouble(cartItem.Price);
            //HttpCookie itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);

            List<CartItemViewModel> items = null;
            HttpCookie itemsCookie = null;
            string table = null;
            if (cartItem.TableId != null && !IsUpdate)
            {
                table = WebUtil.CurrentTable + cartItem.TableId;
                items = GetCartItems(cartItem.TableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                items = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (cartItem.TableId == null)
            {
                items = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            if (items.Count == 0)
                items = new List<CartItemViewModel>();
            try
            {
                if (items.Count > 0)
                {
                    findItem = items.Find(p => p.ItemId == cartItem.ItemId);
                }

                if (findItem != null)
                {
                    findItem.Quantity += cartItem.Quantity ?? 1;
                    searchItem = findItem;
                    items.Remove(findItem);
                }
                else
                {
                    searchItem = ItemServices.GetCartItemById(cartItem.ItemId);
                    if (searchItem != null)
                    {
                        searchItem.ItemName = cartItem.ItemName;
                        searchItem.Price = cartItem.Price;
                        searchItem.Quantity = cartItem.Quantity ?? 1;
                        searchItem.OrderId = cartItem.OrderId;
                        searchItem.ItemId = cartItem.ItemId;
                        searchItem.TableId = cartItem.TableId;
                        searchItem.TaxAmount = cartItem.TaxAmount;
                        searchItem.DepartmentId = cartItem.DepartmentId;
                    }
                }

                if (searchItem != null)
                {
                    items.Add(searchItem);

                    string myObjectJson = JsonConvert.SerializeObject(items);
                    var compress = SerializeAndCompress(myObjectJson);
                    var base64 = Convert.ToBase64String(compress);

                    if (cartItem.TableId != null && !IsUpdate)
                    {
                        itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }
                    else if (IsUpdate)
                    {
                        itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(2), };
                    }
                    else if (cartItem.TableId == null)
                    {
                        itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }


                    HttpContext.Current.Response.Cookies.Add(itemsCookie);

                    returnValue = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;

        }

        public static bool CurrentItemRemarks(string id, int? tableId = null, string remarks = null)
        {
            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.Where(x => x.ItemId == Convert.ToInt32(id)).SingleOrDefault();

                    if (searchItem != null)
                    {
                        searchItem.Remarks = remarks;
                        returnValue = true;
                    }
                }

                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool RemoveItemFromCart(string id, int? tableId = null)
        {
            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if(tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.Where(x => x.ItemId == Convert.ToInt32(id)).SingleOrDefault();

                    if (searchItem != null)
                    {
                        cartItems.Remove(searchItem);
                        returnValue = true;
                    }
                }

                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool VoidItemFromCart(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }
            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.IsVoid = true;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate )
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool ComplimentaryItemFromCart(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.IsComplimentary = true;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool ReleaseVoidItemFromCart(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.IsVoid = false;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool ReleaseComplimentaryItemFromCart(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.IsComplimentary = false;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }


        public static bool PlusQuantity(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.Quantity++;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }

        public static bool PointQuantity(string id, int? tableId = null)
        {

            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.Quantity= searchItem.Quantity + 0.25;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;
        }


        public static bool MinusQuantity(string id, int? tableId = null)
        {
            bool returnValue = false;
            CartItemViewModel searchItem = null;
            List<CartItemViewModel> cartItems = null;
            HttpCookie itemsCookie = null;
            string table = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                cartItems = GetCartItems(tableId);
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else if (tableId == null)
            {
                cartItems = GetCartItems();
                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null && searchItem.Quantity > 0)
                    {
                        searchItem.Quantity--;
                        returnValue = true;
                    }
                    else if (searchItem.Quantity == 0)
                    {
                        cartItems.Remove(searchItem);
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                if (tableId != null)
                {
                    itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, };
                }
                else if (IsUpdate)
                {
                    itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true };
                }
                else if (tableId == null)
                {
                    itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true };
                }
                HttpContext.Current.Response.Cookies.Add(itemsCookie);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return returnValue;
        }

        public static bool UpdateItemQty(string id, double qty)
        {
            List<CartItemViewModel> cartItems = GetCartItems();
            CartItemViewModel searchItem = null;
            bool returnValue = false;
            HttpCookie itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            try
            {
                if (cartItems.Count > 0)
                {
                    searchItem = cartItems.SingleOrDefault(x => x.ItemId == Convert.ToInt32(id));

                    if (searchItem != null)
                    {
                        searchItem.Quantity = qty;
                        returnValue = true;
                    }
                }


                string myObjectJson = JsonConvert.SerializeObject(cartItems);
                var compress = SerializeAndCompress(myObjectJson);
                var base64 = Convert.ToBase64String(compress);

                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64)
                {
                    Value = base64,
                    Secure = true,
                };
                HttpContext.Current.Response.Cookies.Add(itemsCookie);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return returnValue;
        }

        public static byte[] SerializeAndCompress(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(zs, obj);
                }
                return ms.ToArray();
            }

        }

        public static T DecompressAndDeserialize<T>(this byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (T)bf.Deserialize(zs);
                }
            }

        }

        public static List<CartItemViewModel> GetCartItems(int? tableId = null)
        {
            List<CartItemViewModel> CartItems = new List<CartItemViewModel>();
            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }
            HttpCookie cultureItemCookies = null;
            if (tableId != null )
            {
                string table = WebUtil.CurrentTable + tableId;
                cultureItemCookies = HttpContext.Current.Request.Cookies[table];
            }
            else if (IsUpdate)
            {
                cultureItemCookies = HttpContext.Current.Request.Cookies[WebUtil.UpdateItemsCookies];
            }
            else if (tableId == null)
            {
                cultureItemCookies = HttpContext.Current.Request.Cookies[WebUtil.CurrentItemsCookies];
            }

            if (cultureItemCookies != null && cultureItemCookies.Value != "")
            {
                var bytes = Convert.FromBase64String(cultureItemCookies.Value);
                var decompress = DecompressAndDeserialize<string>(bytes);
                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(decompress);
            }
            return CartItems;
        }

        public static HttpCookie ItemsCookie()
        {
            HttpCookie itemCookies = null;

            return itemCookies;
        }

        #endregion

        #region Cart With Table Utility

        public static bool AddItemIntoCartWithTable(int? id, int? qty, string price, string itemName, string taxAmount = null, string TaxPercentage = null, int? tableId = null)
        {
            bool returnValue = false;
            CartItemViewModel searchItem = null;
            CartItemViewModel findItem = null;
            List<CartItemViewModel> items = GetCartItems(tableId);
            double itemPrice = Convert.ToDouble(price);

            string table = WebUtil.CurrentTable + tableId;
            HttpCookie itemsCookie = new HttpCookie(table);

            if (items.Count == 0)
                items = new List<CartItemViewModel>();
            try
            {
                if (items.Count > 0)
                {
                    findItem = items.Find(p => p.ItemId == id);
                }

                if (findItem != null)
                {
                    findItem.Quantity += qty ?? 1;
                    searchItem = findItem;
                    items.Remove(findItem);
                }
                else
                {
                    searchItem = ItemServices.GetCartItemById(id ?? 0);
                    if (searchItem != null)
                    {
                        searchItem.ItemName = itemName;
                        searchItem.Price = itemPrice;
                        //searchItem.TaxAmount = Convert.ToDecimal(taxAmount);
                        //searchItem.TaxPercentage = Convert.ToDecimal(TaxPercentage);
                        searchItem.Quantity = qty ?? 1;

                        //var stock = ItemServices.GetStockById(searchItem.ItemId);

                        //if (stock?.Quantity > 0)
                        //{

                        //}
                    }
                }

                if (searchItem != null)
                {
                    items.Add(searchItem);

                    string myObjectJson = JsonConvert.SerializeObject(items);
                    var compress = SerializeAndCompress(myObjectJson);
                    var base64 = Convert.ToBase64String(compress);

                    itemsCookie = new HttpCookie(table, base64)
                    {
                        Value = base64,
                        Expires = DateTime.Now.AddDays(1),
                        Secure = true,
                    };

                    HttpContext.Current.Response.Cookies.Add(itemsCookie);

                    returnValue = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;

        }

        public static List<CartItemViewModel> GetCartItemsWithTable(int? tableId)
        {
            List<CartItemViewModel> CartItems = new List<CartItemViewModel>();
            string table = WebUtil.CurrentTable + tableId;
            HttpCookie cultureItemCookies = HttpContext.Current.Request.Cookies[table];
            if (cultureItemCookies != null && cultureItemCookies.Value != "")
            {
                var bytes = Convert.FromBase64String(cultureItemCookies.Value);
                var decompress = DecompressAndDeserialize<string>(bytes);

                CartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(decompress);
            }
            return CartItems;
        }

        #endregion

        #region Order Utility

        public static OrderViewModel CurrentOrder(int? tableId = null)
        {
            OrderViewModel order = null;
            HttpCookie orderCookies = null;
            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }


            if (tableId != null)
            {
                string table = WebUtil.CurrentOrder + tableId;
                orderCookies = HttpContext.Current.Request.Cookies[table];


            }
            else if (IsUpdate)
            {
                orderCookies = HttpContext.Current.Request.Cookies[WebUtil.UpdateOrderCookies];
            }
            else
            {
                orderCookies = HttpContext.Current.Request.Cookies[WebUtil.CurrentOrderCookies];
            }


            if (orderCookies != null && orderCookies.Value != "")
            {
                order = JsonConvert.DeserializeObject<OrderViewModel>(orderCookies.Value);
            }
            return order;
        }

        public static bool AddCurrentOrder(OrderViewModel model = null)
        {
            bool returnValue = false;
            HttpCookie orderCookie = null;
            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }
            string table = null;
            OrderViewModel order = null;
            List<CartItemViewModel> CartItems = null;
            if (model.TableId != null)
            {
                table = WebUtil.CurrentOrder + model.TableId;
                order = CurrentOrder(model.TableId);
                CartItems = GetCartItems(model.TableId);
                orderCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                order = CurrentOrder();
                CartItems = GetCartItems();
                orderCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else
            {
                order = CurrentOrder();
                CartItems = GetCartItems();
                orderCookie = new HttpCookie(WebUtil.CurrentOrderCookies);
            }

            if (order == null)
                order = new OrderViewModel();
            try
            {
                if (model.Id > 0)
                {
                    order.Id = model.Id;
                }
                if (model.TableId > 0)
                {
                    order.TableId = model.TableId;
                }
                if (model.PaymentTypeId > 0)
                {
                    order.PaymentTypeId = model.PaymentTypeId;
                }
                if (model.NoOfGuest > 0)
                {
                    order.NoOfGuest = model.NoOfGuest;
                }
                if (model.Tip > 0)
                {
                    order.Tip = model.Tip;
                }
                if (model.GSTPerentage > 0)
                {
                    order.GSTPerentage = model.GSTPerentage;
                    order.GstCharged = model.GstCharged;
                }
                if (model.ServiceChargesPerentage > 0)
                {
                    order.ServiceChargesPerentage = model.ServiceChargesPerentage;
                    order.ServiceCharged = model.ServiceCharged;
                }

                //aliment 


                if (model.NitrogenGas > 0)
                {

                    order.NitrogenGas = model.NitrogenGas;
                }

                if (model.TPMS > 0)
                {

                    order.TPMS = model.TPMS;
                }


                if (model.AlignmentAmount > 0)
                {
                
                    order.AlignmentAmount = model.AlignmentAmount;
                }

                if (model.wheelBalanceAmount > 0)
                {

                    order.wheelBalanceAmount = model.wheelBalanceAmount;
                }

                //...

                if (model.DiscountPercentage > 0)
                {
                    order.DiscountVoucher = 0;
                    order.FOCDiscount = 0;
                    order.DiscountPercentage = model.DiscountPercentage;
                    order.DiscountCharged = model.DiscountCharged;
                }
                if (model.DiscountVoucher > 0 && order.TotalNetAmount >= model.DiscountVoucher)
                {
                    order.DiscountPercentage = 0;
                    order.FOCDiscount = 0;
                    order.DiscountVoucher = model.DiscountVoucher;
                }
                if (model.FOCDiscount > 0 && order.DiscountVoucher >= 0 && order.DiscountPercentage >= 0)
                {
                    order.DiscountPercentage = 0;
                    order.DiscountVoucher = 0;
                    order.DiscountCharged = 0;
                    order.FOCDiscount = model.FOCDiscount;
                }
                if (model.ReceiptTotalCash > 0)
                {
                    order.ReceiptTotalCash = model.ReceiptTotalCash;
                }
                if (model.ReceiptTotalCredit > 0)
                {
                    order.ReceiptTotalCredit = model.ReceiptTotalCredit;
                }
                if (model.BankDiscount > 0)
                {
                    order.BankDiscount = model.BankDiscount;
                }
                if (model.Change > 0)
                {
                    order.Change = model.Change;
                }
                if (model.IsPayment) { order.IsPayment = model.IsPayment; } else { model.IsPayment = model.IsPayment; }
                if (IsUpdate) { order.IsUpdateMode = IsUpdate; }
                if (model.IsComplimentary) { order.IsComplimentary = true; } else { order.IsComplimentary = false; }
  
                    order.ComplimantryAmount = (double)CartItems.Where(i => i.IsComplimentary == true).Select(i => i.Price * i.Quantity).Sum();
                order.VoidAmount = (double)CartItems.Where(i => i.IsVoid == true).Select(i => i.Price * i.Quantity).Sum();


                if (model.IsVoid) { order.IsVoid = true; } else { order.IsVoid = false; }
                order.GrossAmount = (double)CartItems.Select(i => i.Price * i.Quantity).Sum();

                order.TotalNetAmount = order.GrossAmount + order.GstCharged + order.ServiceCharged + model.NitrogenGas + model.TPMS + model.wheelBalanceAmount + model.AlignmentAmount- order.DiscountCharged - order.DiscountVoucher - order.ComplimantryAmount - order.VoidAmount;

                if (order.FOCDiscount > 0)
                {
                    order.TotalNetAmount -= order.FOCDiscount;
                }


                if (order != null)
                {
                    string myObjectJson = JsonConvert.SerializeObject(order);
                    //var compress = SerializeAndCompress(myObjectJson);
                    //var base64 = Convert.ToBase64String(compress);

                    if (model.TableId != null)
                    {
                        orderCookie = new HttpCookie(table, myObjectJson) { Value = myObjectJson, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }
                    else if (IsUpdate)
                    {
                        orderCookie = new HttpCookie(WebUtil.UpdateOrderCookies, myObjectJson) { Value = myObjectJson, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }
                    else
                    {
                        orderCookie = new HttpCookie(WebUtil.CurrentOrderCookies, myObjectJson) { Value = myObjectJson, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }

                    HttpContext.Current.Response.Cookies.Add(orderCookie);
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;

        }

        public static bool LoadItemIntoCartWithUpdateMode(List<CartItemViewModel> cartItems = null, int? tableId = null)
        {
            bool returnValue = false;
            List<CartItemViewModel> items = new List<CartItemViewModel>();
            HttpCookie itemsCookie = null;

            bool IsUpdate = false;
            if (HttpContext.Current.Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)HttpContext.Current.Session["IsUpdateMode"];
            }
            string table = null;

            if (tableId != null)
            {
                table = WebUtil.CurrentTable + tableId;
                itemsCookie = new HttpCookie(table);
            }
            else if (IsUpdate)
            {
                itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies);
            }
            else
            {

                itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies);
            }

            try
            {

                if (cartItems?.Count > 0)
                {
                    items.AddRange(cartItems);
                    string myObjectJson = JsonConvert.SerializeObject(items);
                    var compress = SerializeAndCompress(myObjectJson);
                    var base64 = Convert.ToBase64String(compress);
                    //itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };

                    if (tableId != null)
                    {
                        itemsCookie = new HttpCookie(table, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }
                    else if (IsUpdate)
                    {
                        itemsCookie = new HttpCookie(WebUtil.UpdateItemsCookies, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }
                    else
                    {
                        itemsCookie = new HttpCookie(WebUtil.CurrentItemsCookies, base64) { Value = base64, Secure = true, Expires = DateTime.Now.AddDays(1), };
                    }

                    HttpContext.Current.Response.Cookies.Add(itemsCookie);
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return returnValue;

        }

        #endregion
    }
}
