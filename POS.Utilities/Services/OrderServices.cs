using POS.Database.DatabaseModel;
using POS.Utilities.MultiTenant;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public class OrderServices
    {
        #region Order
        public static OrderViewModel AddOrder(OrderViewModel model, List<CartItemViewModel> cartItems)
        {
            model.returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    Order entity = (Order)model;
                    context.Orders.Add(entity);
                    context.SaveChanges();
                    AddOrderItems(entity.Id, cartItems);
                    if (entity.TableId > 0)
                    {
                        AddOrderTableHistory(entity.Id, entity.TableId ?? 0);
                    }

                    model.Id = entity.Id;
                    model.returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        public static bool AddOrderItems(int orderId, List<CartItemViewModel> cartItems)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    if (cartItems?.Count > 0)
                    {
                        foreach (var item in cartItems)
                        {
                            OrderItem orderItem = new OrderItem
                            {
                                OrderId = orderId,
                                ItemId = item.ItemId,
                                ItemName = item.ItemName,
                                Quantity = (double)item.Quantity,
                                Price = item.Price,
                                TotalPrice = (double)(item.Price * item.Quantity),
                                TaxAmount = item.TaxAmount,
                                CreationDate = DateTime.Now,
                                ModifyDate = DateTime.Now,
                                IsComplimentary = item.IsComplimentary,
                                IsVoid = item.IsVoid,
                                Remarks = item.Remarks,
                                TableId = item.TableId,
                            };
                            context.OrderItems.Add(orderItem);
                        }
                        context.SaveChanges();
                        returnValue = true;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool AddOrderTableHistory(int orderId, int tableId)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    OrderTableHistory entity = new OrderTableHistory
                    {
                        OrderId = orderId,
                        TableId = tableId,
                        CreationDate = DateTime.Now,
                    };
                    context.OrderTableHistories.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool Add_Order_Void_Or_Complimentary(int? OrderId, bool IsVoid, bool IsComplimentary, bool IsPayment, bool IsUpdateMode, string Reason = null)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {

                    var find = GetOrderById(OrderId ?? 0);

                    if (find != null)
                    {
                        SqlParameter[] orderParameters = new SqlParameter[]
                        {
                            new SqlParameter("@Id",find.Id),
                            new SqlParameter("@IsVoid",IsVoid),
                            new SqlParameter("@IsComplimentary",IsComplimentary),
                            new SqlParameter("@IsPayment",IsPayment),
                            new SqlParameter("@IsUpdateMode",IsUpdateMode),
                            new SqlParameter("@Reason",Reason ?? " Testing"),
                            new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL_Order = $"update Orders set IsVoid = @IsVoid, IsComplimentary = @IsComplimentary, IsPayment = @IsPayment, IsUpdateMode = @IsUpdateMode, Reason = @Reason, ModifyDate=@ModifyDate where Id = @Id";
                        context.Database.ExecuteSqlCommand(SQL_Order, orderParameters);
                        returnValue = true;

                        var orderItems = GetOrderItemsByOrderId(find.Id);
                        if (orderItems?.Count > 0)
                        {
                            foreach (var p in orderItems)
                            {
                                SqlParameter[] orderItemParameters = new SqlParameter[]
                                {
                                    new SqlParameter("@OrderId",p.OrderId),
                                    new SqlParameter("@IsVoid",IsVoid),
                                    new SqlParameter("@IsComplimentary",IsComplimentary),
                                    new SqlParameter("@ModifyDate",DateTime.Now),
                                };
                                string SQL_OrderItems = $"Update OrderItems set IsVoid = @IsVoid, IsComplimentary = @IsComplimentary, ModifyDate = @ModifyDate where OrderId = @OrderId";
                                context.Database.ExecuteSqlCommand(SQL_OrderItems, orderItemParameters);
                                returnValue = true;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return returnValue;
        }
        public static bool AddOrderWithReason(int? OrderId, string Reason = null)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {

                    var find = GetOrderById(OrderId ?? 0);

                    if (find != null)
                    {
                        SqlParameter[] orderParameters = new SqlParameter[]
                        {
                            new SqlParameter("@Id",find.Id),
                            new SqlParameter("@Reason",Reason ?? "none"),
                            new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL_Order = $"update Orders set Reason = @Reason, ModifyDate=@ModifyDate where Id = @Id";
                        context.Database.ExecuteSqlCommand(SQL_Order, orderParameters);
                        returnValue = true;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return returnValue;
        }
        public static OrderViewModel GetOrderById(int orderId)
        {
            OrderViewModel returnValue = new OrderViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from Orders where Id={orderId}";
                    returnValue = context.Database.SqlQuery<Order>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static OrderViewModel GetOrder()
        {
            OrderViewModel returnValue = new OrderViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from Orders ";
                    returnValue = context.Database.SqlQuery<Order>(SQL).LastOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        //public static GetHistoryCarVoucherViewModel GetCarVocherById(int CarID)
        //{
        //    GetHistoryCarVoucherViewModel returnValue = new GetHistoryCarVoucherViewModel();
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            string SQL = $"Exec GetHistoryCarVoucher where Id={CarID}";
        //            returnValue = context.Database.SqlQuery<GetHistoryCarVoucher_Result>(SQL).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}

        //public static List<GetHistoryCarVoucherViewModel> GetAllOrders(int CarID)
        //{
        //    List<GetHistoryCarVoucherViewModel> returnValue = new List<GetHistoryCarVoucherViewModel>();
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            string SQL = $"Exec GetHistoryCarVoucher where Id={CarID}";
        //            var orders = context.Database.SqlQuery<GetHistoryCarVoucher_Result>(SQL).ToList();
        //            returnValue = orders.Select(p => (GetHistoryCarVoucherViewModel) p).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}


        public static OrderViewModel GetOrderByTableId(int tableId)
        {
            OrderViewModel returnValue = new OrderViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"SELECT TOP 1 * FROM Orders where TableId={tableId} ORDER BY ID DESC";
                    returnValue = context.Database.SqlQuery<Order>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderViewModel> GetAllOrders()
        {
            List<OrderViewModel> returnValue = new List<OrderViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from Orders where IsUpdateMode = 0 and IsPayment = 1 order by ModifyDate ";
                    var orders = context.Database.SqlQuery<Order>(SQL).ToList();
                    returnValue = orders.Select(p => (OrderViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static OrderViewModel GetLatestPaidOrder()
        {
            OrderViewModel returnValue = new OrderViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"SELECT TOP 1 * FROM Orders WHERE IsUpdateMode = 0 AND IsPayment = 1 ORDER BY ModifyDate DESC, Id DESC";
                    returnValue = context.Database.SqlQuery<Order>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }




        public static CustomerViewModel GetCustomerById(int CustomerId)
        {
            CustomerViewModel returnValue = new CustomerViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from Customer Where id={CustomerId}";
                    returnValue = context.Database.SqlQuery<Customer>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<OrderViewModel> GetOrdersWithOutPayment()
        {
            List<OrderViewModel> returnValue = new List<OrderViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from Orders where IsUpdateMode  <> 0 and IsPayment = 0 order by ModifyDate";
                    var orders = context.Database.SqlQuery<Order>(SQL).ToList();
                    returnValue = orders.Select(p => (OrderViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderViewModel> GetOpenOrders()
        {
            List<OrderViewModel> returnValue = new List<OrderViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from Orders where IsUpdateMode = 0 and IsPayment =0 order by ModifyDate";
                    var orders = context.Database.SqlQuery<Order>(SQL).ToList();
                    returnValue = orders.Select(p => (OrderViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderViewModel> GetDraftOrders()
        {
            List<OrderViewModel> returnValue = new List<OrderViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from Orders where IsUpdateMode <> 0 and IsPayment = 0 order by ModifyDate";
                    var orders = context.Database.SqlQuery<Order>(SQL).ToList();
                    returnValue = orders.Select(p => (OrderViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static OrderViewModel GetOrderByInvoice(string invoiceNo)
        {
            OrderViewModel returnValue = new OrderViewModel();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from Orders where InvoiceNumber='{invoiceNo}'";
                    returnValue = context.Database.SqlQuery<Order>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateOrder(OrderViewModel model, List<CartItemViewModel> cartItems)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {

                    var find = context.Orders.Where(p => p.Id == model.Id).SingleOrDefault();

                    if (find != null)
                    {
                        if (model.GrossAmount > 0)
                            find.GrossAmount = model.GrossAmount;
                        if (model.GSTPerentage > 0)
                            find.GSTPerentage = model.GSTPerentage;
                        if (model.GstCharged > 0)
                            find.GstCharged = model.GstCharged;
                        if (model.DiscountPercentage > 0)
                            find.DiscountPercentage = model.DiscountPercentage;
                        if (model.DiscountCharged > 0)
                            find.DiscountCharged = model.DiscountCharged;
                        if (model.BankDiscount > 0)
                            find.BankDiscount = model.BankDiscount;
                        if (model.Change > 0)
                            find.Change = model.Change;
                        if (model.DiscountVoucher > 0)
                            find.DiscountVoucher = model.DiscountVoucher;
                        if (model.FOCDiscount > 0)
                            find.FOCDiscount = model.FOCDiscount;
                        find.ModifyDate = DateTime.Now;
                        if (model.ReceiptTotalCash > 0)
                            find.ReceiptTotalCash = model.ReceiptTotalCash;
                        if (model.ReceiptTotalCredit > 0)
                            find.ReceiptTotalCredit = model.ReceiptTotalCredit;
                        if (model.ServiceCharged > 0)
                            find.ServiceCharged = model.ServiceCharged;
                        if (model.ServiceChargesPerentage > 0)
                            find.ServiceChargesPerentage = model.ServiceChargesPerentage;
                        if (model.TableId > 0)
                            find.TableId = model.TableId;
                        if (model.Tip > 0)
                            find.Tip = model.Tip;
                        if (model.PaymentTypeId > 0)
                            find.PaymentTypeId = model.PaymentTypeId;
                        if (model.TotalNetAmount > 0)
                            find.TotalNetAmount = model.TotalNetAmount;
                        //if (model.IsPayment)
                        find.IsPayment = model.IsPayment;
                        if (model.IsUpdateMode == false)
                            find.IsUpdateMode = model.IsUpdateMode;
                        context.SaveChanges();
                    }
                    UpdateOrderItems(model.Id, cartItems);
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateOrderItems(int orderId, List<CartItemViewModel> cartItems)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {

                    var delItems = context.OrderItems.Where(p => p.OrderId == orderId).ToList();

                    if (delItems?.Count > 0)
                    {
                        context.OrderItems.RemoveRange(delItems);
                    }

                    if (cartItems?.Count > 0)
                    {
                        foreach (var item in cartItems)
                        {
                            OrderItem orderItem = new OrderItem
                            {
                                OrderId = orderId,
                                ItemId = item.ItemId,
                                ItemName = item.ItemName,
                                Quantity = (double)item.Quantity,
                                Price = item.Price,
                                TotalPrice = item.Price * (double)item.Quantity,
                                TaxAmount = item.TaxAmount,
                                CreationDate = DateTime.Now,
                                ModifyDate = DateTime.Now,
                                IsComplimentary = item.IsComplimentary,
                                IsVoid = item.IsVoid,
                                TableId = item.TableId,
                                Remarks = item.Remarks,
                            };
                            context.OrderItems.Add(orderItem);
                        }
                        context.SaveChanges();
                        returnValue = true;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderItemViewModel> GetOrderItemsByOrderId(int orderId)
        {
            List<OrderItemViewModel> returnValue = new List<OrderItemViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from OrderItems where OrderId={orderId}";
                    var OrderItems = context.Database.SqlQuery<OrderItem>(SQL).ToList();
                    returnValue = OrderItems.Select(p => (OrderItemViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderItemViewModel> GetOrderItemsByOrderIdAndDepartmentId(int orderId, int departmentId)
        {
            List<OrderItemViewModel> returnValue = new List<OrderItemViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select * from OrderItems where OrderId={orderId} and DepartmentId={departmentId}";
                    var OrderItems = context.Database.SqlQuery<OrderItem>(SQL).ToList();
                    returnValue = OrderItems.Select(p => (OrderItemViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool RemoveOrderItems(int orderId)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var del = context.OrderItems.Where(p => p.OrderId == orderId).ToList();
                    if (del?.Count > 0)
                    {
                        context.OrderItems.RemoveRange(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool OpenOrder(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = context.Orders.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsUpdateMode = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool CloseOrder(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = context.Orders.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsUpdateMode = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region OrderType Functions
        public static bool AddOrderType(OrderTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    OrderType entity = (OrderType)model;
                    context.OrderTypes.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static OrderTypeViewModel GetOrderTypeById(int id)
        {
            OrderTypeViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<OrderType>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetOrderTypeName(int id)
        {
            string returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select Name from OrderTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderTypeViewModel> GetAllOrderTypes(string search = null)
        {
            List<OrderTypeViewModel> returnValue = new List<OrderTypeViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderTypes";
                    var OrderType = context.Database.SqlQuery<OrderType>(SQL).ToList();
                    returnValue = OrderType.Select(p => (OrderTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderTypeViewModel> GetOrderTypesByName()
        {
            List<OrderTypeViewModel> returnValue = new List<OrderTypeViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select Name from OrderTypes";
                    var OrderType = context.Database.SqlQuery<OrderType>(SQL).ToList();
                    returnValue = OrderType.Select(p => (OrderTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateOrderType(OrderTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = GetOrderTypeById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update OrderTypes set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteOrderType(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var del = context.OrderTypes.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.OrderTypes.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region OrderFeedback Functions
        public static bool AddOrderFeedback(OrderFeedbackViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    OrderFeedback entity = (OrderFeedback)model;
                    context.OrderFeedbacks.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static OrderFeedbackViewModel GetOrderFeedbackById(int id)
        {
            OrderFeedbackViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderFeedbacks where Id={id}";
                    returnValue = context.Database.SqlQuery<OrderFeedback>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderFeedbackViewModel> GetAllOrderFeedbacks(string search = null)
        {
            List<OrderFeedbackViewModel> returnValue = new List<OrderFeedbackViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderFeedbacks";
                    var OrderFeedback = context.Database.SqlQuery<OrderFeedback>(SQL).ToList();
                    returnValue = OrderFeedback.Select(p => (OrderFeedbackViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteOrderFeedback(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var del = context.OrderFeedbacks.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.OrderFeedbacks.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region OrderFeedbackStatus Functions
        public static bool AddOrderFeedbackStatus(OrderFeedbackStatusViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    OrderFeedbackStatus entity = (OrderFeedbackStatus)model;
                    context.OrderFeedbackStatuses.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static OrderFeedbackStatusViewModel GetOrderFeedbackStatusById(int id)
        {
            OrderFeedbackStatusViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderFeedbackStatuses where Id={id}";
                    returnValue = context.Database.SqlQuery<OrderFeedbackStatus>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetOrderFeedbackStatusName(int id)
        {
            string returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select Name from OrderFeedbackStatuses where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderFeedbackStatusViewModel> GetAllOrderFeedbackStatuses(string search = null)
        {
            List<OrderFeedbackStatusViewModel> returnValue = new List<OrderFeedbackStatusViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from OrderFeedbackStatuses";
                    var OrderFeedbackStatus = context.Database.SqlQuery<OrderFeedbackStatus>(SQL).ToList();
                    returnValue = OrderFeedbackStatus.Select(p => (OrderFeedbackStatusViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OrderFeedbackStatusViewModel> GetOrderFeedbackStatusesByName()
        {
            List<OrderFeedbackStatusViewModel> returnValue = new List<OrderFeedbackStatusViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select Name from OrderFeedbackStatuses";
                    var OrderFeedbackStatus = context.Database.SqlQuery<OrderFeedbackStatus>(SQL).ToList();
                    returnValue = OrderFeedbackStatus.Select(p => (OrderFeedbackStatusViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateOrderFeedbackStatus(OrderFeedbackStatusViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = GetOrderFeedbackStatusById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                        };
                        string SQL = $"update OrderFeedbackStatuses set Name=@Name where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteOrderFeedbackStatus(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var del = context.OrderFeedbackStatuses.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.OrderFeedbackStatuses.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableOrderFeedbackStatus(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = context.OrderFeedbackStatuses.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableOrderFeedbackStatus(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = context.OrderFeedbackStatuses.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion

    }
}
