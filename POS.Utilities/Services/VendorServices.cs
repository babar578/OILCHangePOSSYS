using POS.Database.DatabaseModel;
using POS.Utilities.ReportsModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public static class VendorServices
    {
        #region Quotation
        public static List<QuottionViewModel> GetReportQuottionById(int? Id)
        {
            List<QuottionViewModel> returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"exec QuotationVoucher {Id}";

                    returnValue = context.Database.SqlQuery<QuottionViewModel>(SQL).ToList();




                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;

        }


            public static ResponseDto AddQuoations(QuotationheadViewModel model)
        {

            //. = false;
            ItemViewModel itemss = new ItemViewModel();
            ResponseDto response = new ResponseDto();
            response.Status = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        QuotationHead entity = (QuotationHead)model;
                        context.QuotationHeads.Add(entity);
                        context.SaveChanges();

                        if (model.RawItems?.Count > 0)
                        {
                            List<QuotationDetail> purchaseDetail = new List<QuotationDetail>();
                            int mLNo = 1;

                            foreach (var p in model.RawItems)
                            {

                                itemss.Id = p.Id;
                                itemss.Price = p.Price;

                                bool add = ItemServices.UpdateItem(itemss);
                                response.PrintId = entity.Id;
                                purchaseDetail.Add(new QuotationDetailViewModel
                                {
                                    Id = entity.Id,
                                    ItemId = p.Id,
                                    ItemName = p.Name,
                                    Quantity = Math.Round(p.Quantity, 2),
                                    Rate = Math.Round(p.Price, 2),
                                    LineRemarks = p.Remarks,
                                    TotalRate = Math.Round(p.SubTotal, 2),
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                });
                                mLNo += 1;
                            }
                            context.QuotationDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        response.Message = "Quotation Save successfully";
                        response.Status = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        response.Message = "Quotation Not Save successfully";
                        throw;
                    }
                }
            }
            return response;
        }
        public static QuotationheadViewModel GetQuotationHeadById(int id)
        {
            QuotationheadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from QuotationHead where Id={id}";
                    returnValue = context.Database.SqlQuery<QuotationHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<QuotationDetailViewModel> GetQuotationHeadDetailsByHeadId(int RefID)
        {
            List<QuotationDetailViewModel> returnValue = new List<QuotationDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from QuotationDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<QuotationDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<QuotationheadViewModel> GetQuotationDetailHeads()
        {
            List<QuotationheadViewModel> returnValue = new List<QuotationheadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from QuotationHead";
                    returnValue = context.Database.SqlQuery<QuotationheadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteQuotationHead(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.QuotationDetails.Where(p => p.Id == id).ToList();
                    var del = context.QuotationHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.QuotationDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.QuotationHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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
        #endregion
        public static List<vendorWareHouseVoucherReporViewModel> GetReportvendorwarehouseById(int? Id)
        {
            List<vendorWareHouseVoucherReporViewModel> returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"exec VendorToWareHouseVoucher {Id}";
                    returnValue = context.Database.SqlQuery<vendorWareHouseVoucherReporViewModel>(SQL).ToList();




                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<JobCardViewModelReport> GetReportjobcardById(int? Id)
        {
            List<JobCardViewModelReport> returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"exec JobCard {Id}";
                    returnValue = context.Database.SqlQuery<JobCardViewModelReport>(SQL).ToList();




                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static ItemStockViewModel GetItemKichenByItemId(int? ItemId)
        {
            ItemStockViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select ItemId ,ItemName,IssueToLocationQuantity-ReturnToWarehouseQuantity-ClosingInventoryQuantity as kitchenblan from fn_ItemsStockInHand('01-Jan-1900',GETDATE()) where ItemId={ItemId}";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static ResponseDto AddVendorToWarehouse(VendorToWarehouseHeadViewModel model)
        {

            //. = false;
            ItemViewModel itemss = new ItemViewModel();
            ResponseDto response = new ResponseDto();
            response.Status = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        VendorToWarehouseHead entity = (VendorToWarehouseHead)model;
                        context.VendorToWarehouseHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.RawItems?.Count > 0)
                        {
                            List<VendorToWarehouseDetail> purchaseDetail = new List<VendorToWarehouseDetail>();
                            int mLNo = 1;

                            foreach (var p in model.RawItems)
                            {

                                itemss.Id = p.Id;
                                itemss.Price = p.Price;

                                bool add = ItemServices.UpdateItem(itemss);
                                response.PrintId = entity.Id;
                                purchaseDetail.Add(new VendorToWarehouseDetail
                                {
                                    Id = entity.Id,
                                    ItemId = p.Id,
                                    ItemName = p.Name,
                                    Quantity = Math.Round(p.Quantity, 2),
                                    Rate = Math.Round(p.Price, 2),
                                    PRate = Math.Round(p.PPrice, 2),
                                    LineRemarks = p.Remarks,
                                    TotalRate = Math.Round(p.SubTotal, 2),
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                });
                                mLNo += 1;
                            }
                            context.VendorToWarehouseDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        response.Message = "Voucher Save successfully";
                        response.Status = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        response.Message = "Voucher Not Save successfully";
                        throw;
                    }
                }
            }
            return response;
        }
        public static VendorToWarehouseDetailViewModel GetIssueToLocationAVGByItemId(int itemid)
        {
            VendorToWarehouseDetailViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"SELECT itemid, SUM(Quantity) AS Quantity,  ISNULL((SUM(TotalRate)/NULLIF(SUM(Quantity),0)),0) AS Rate, SUM(TotalRate) AS TotalRate from ( select itemid, Quantity, TotalRate from VendorToWarehouseDetail where itemid={itemid} union all select itemid, Quantity*-1 AS Quantity, TotalRate*-1 AS TotalRate from ReturnToVendorDetail where itemid={itemid}  union all select itemid, Quantity, TotalRate from openingdetail where itemid={itemid}  )a group by itemid";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseDetailViewModel>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static ResponseDto AddIssueToLocation(IssueToLocationHeadViewModel model)
        {

            ResponseDto responseDto = new ResponseDto();
            responseDto.Status = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        IssueToLocationHead entity = (IssueToLocationHead)model;
                        context.IssueToLocationHeads.Add(entity);
                        int success = context.SaveChanges();
                        responseDto.PrintId = entity.Id;
                        if (success == 1 && model.IssueToLocationDetails?.Count > 0)
                        {
                            List<IssueToLocationDetail> items = new List<IssueToLocationDetail>();
                            int mLNo = 1;
                            foreach (var p in model.IssueToLocationDetails)
                            {
                                var rate = VendorServices.GetIssueToLocationAVGByItemId(p.ItemId);
                                p.Rate = Math.Round(Convert.ToDouble(rate.Rate), 2);
                                p.TotalRate = Math.Round(p.Rate * p.Quantity, 2);
                                items.Add(new IssueToLocationDetail
                                {
                                    Id = entity.Id,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                    ItemId = p.ItemId,
                                    ItemName = p.ItemName,
                                    Quantity = p.Quantity,
                                    LineRemarks = p.LineRemarks,
                                    Rate = p.Rate,
                                    TotalRate = p.TotalRate,

                                });
                                mLNo += 1;
                            }
                            context.IssueToLocationDetails.AddRange(items);
                            context.SaveChanges();
                        }
                        transaction.Commit();

                        responseDto.Status = true;


                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return responseDto;
        }
        public static ItemStockViewModel GetItemStockWareHouseByItemId(int? ItemId)
        {
            ItemStockViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select * from [dbo].[fn_ItemsStockInHandWareHouse]('01-Jan-1900',GETDATE()) where ItemId={ItemId}";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemStockViewModel> GetAllDeptWies(int DeptmentId)
        {
            List<ItemStockViewModel> returnValue = new List<ItemStockViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select ItemId,itemname , OpeningQuantity,VendorToWarehouseQuantity,IssueToLocationQuantity,ReturnToVendorQuantity,ReturnToWarehouseQuantity,WastageQuantity,ClosingInventoryQuantity,IssueToLocationQuantity-ReturnToWarehouseQuantity as BalanceQuantity,AverageRate from fn_DeptWiesColsing('01-Jan-1900',GETDATE(),{DeptmentId})";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #region ItemStockInHand
        public static ItemStockViewModel GetItemStockByItemId(int? ItemId)
        {
            ItemStockViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select * from fn_ItemsStockInHand('01-Jan-1900',GETDATE()) where ItemId={ItemId}";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemStockViewModel> GetItemStockbyCategoriesID(int? CategoriesID)
        {
            List<ItemStockViewModel> returnValue = new List<ItemStockViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select * from fn_ItemsStockInHand('01-Jan-1900',GETDATE()) where CategoryId ={CategoriesID}";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).ToList();
                    foreach (var item in returnValue)
                    {
                       var model = ItemServices.GetItemById(item.ItemId);

                        item.Price = model.Price;
                    }




                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemStockViewModel> GetAllItemStock()
        {
            List<ItemStockViewModel> returnValue = new List<ItemStockViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select * from fn_ItemsStockInHand('01-Jan-1900',GETDATE())";
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ItemStockViewModel> GetInventoryBalance()
        {
            List<ItemStockViewModel> returnValue = new List<ItemStockViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    StringBuilder SQL = new StringBuilder();
                    SQL.AppendLine("Select c.Name As CategoryName, u.Name As UnitName, f.* from fn_ItemsStockInHand('01-Jan-1900',GETDATE()) f ");
                    SQL.AppendLine("INNER JOIN Items i ON(f.ItemId = i.Id)");
                    SQL.AppendLine("Left JOIN Categories c ON(i.CategoryId = c.Id)");
                    SQL.AppendLine("Left JOIN Units u ON (i.UnitId=u.Id)");
                    returnValue = context.Database.SqlQuery<ItemStockViewModel>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        #endregion

        #region VendorToWarehouse Functions

        //public static bool AddVendorToWarehouse(VendorToWarehouseHeadViewModel model)
        //{

        //    bool returnValue = false;
        //    ItemViewModel itemss = new ItemViewModel();

        //    using (POSEntities context = new POSEntities())
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                VendorToWarehouseHead entity = (VendorToWarehouseHead)model;
        //                context.VendorToWarehouseHeads.Add(entity);
        //                int success = context.SaveChanges();

        //                if (success == 1 && model.RawItems?.Count > 0)
        //                {
        //                    List<VendorToWarehouseDetail> purchaseDetail = new List<VendorToWarehouseDetail>();
        //                    int mLNo = 1;

        //                    foreach (var p in model.RawItems)
        //                    {

        //                        itemss.Id = p.Id;
        //                        itemss.Price = p.Price;

        //                        bool add = ItemServices.UpdateItem(itemss);
        //                        purchaseDetail.Add(new VendorToWarehouseDetail
        //                        {
        //                            Id = entity.Id,
        //                            ItemId = p.Id,
        //                            ItemName = p.Name,
        //                            Quantity = p.Quantity,
        //                            Rate = p.Price,
        //                            LineRemarks = p.Remarks,
        //                            TotalRate = p.SubTotal,
        //                            VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
        //                        }) ;
        //                        mLNo += 1;
        //                    }
        //                    context.VendorToWarehouseDetails.AddRange(purchaseDetail);
        //                    context.SaveChanges();
        //                }
        //                transaction.Commit();
        //                returnValue = true;
        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //    return returnValue;
        //}
        public static VendorToWarehouseHeadViewModel GetVendorToWarehouseHeadById(int id)
        {
            VendorToWarehouseHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorToWarehouseHead where Id={id}";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VendorToWarehouseDetailViewModel> GetVendorToWarehouseDetailsByHeadId(int RefID)
        {
            List<VendorToWarehouseDetailViewModel> returnValue = new List<VendorToWarehouseDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorToWarehouseDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VendorToWarehouseHeadViewModel> GetVendorToWarehouseHeads()
        {
            List<VendorToWarehouseHeadViewModel> returnValue = new List<VendorToWarehouseHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorToWarehouseHead";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteVendorToWarehouse(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.VendorToWarehouseDetails.Where(p => p.Id == id).ToList();
                    var del = context.VendorToWarehouseHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.VendorToWarehouseDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.VendorToWarehouseHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        public static VendorToWarehouseDetailViewModel GetPurchaseItemById(int? ItemId)
        {
            VendorToWarehouseDetailViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select ItemId, ItemName, SUM(Quantity) As Quantity  from VendorToWarehouseDetail where ItemId={ItemId} group by ItemId, ItemName order By ItemId";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseDetailViewModel>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorToWarehouseDetailViewModel> GetAllPurchaseItems()
        {
            List<VendorToWarehouseDetailViewModel> returnValue = new List<VendorToWarehouseDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorToWarehouseDetailViewModel";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorToWarehouseDetailViewModel> GetAllPurchaseItemsName( )
        {
            DateTime fromDate = new DateTime(2015,12,31);
            DateTime toDate = DateTime.Now;

            List<VendorToWarehouseDetailViewModel> returnValue = new List<VendorToWarehouseDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select ItemId, ItemName from fn_ItemsStockInHand ('{fromDate}','{toDate}') group by ItemId, ItemName";
                    returnValue = context.Database.SqlQuery<VendorToWarehouseDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion

        #region IssueToLocation Functions

        //public static bool AddIssueToLocation(IssueToLocationHeadViewModel model)
        //{
        //    bool returnValue = false;
        //    using (POSEntities context = new POSEntities())
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                IssueToLocationHead entity = (IssueToLocationHead)model;
        //                context.IssueToLocationHeads.Add(entity);
        //                int success = context.SaveChanges();

        //                if (success == 1 && model.IssueToLocationDetails?.Count > 0)
        //                {
        //                    List<IssueToLocationDetail> items = new List<IssueToLocationDetail>();
        //                    int mLNo = 1;
        //                    foreach (var p in model.IssueToLocationDetails)
        //                    {
        //                        items.Add(new IssueToLocationDetail
        //                        {
        //                            Id = entity.Id,
        //                            VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
        //                            ItemId = p.ItemId,
        //                            ItemName = p.ItemName,
        //                            Quantity = p.Quantity,
        //                            LineRemarks = p.LineRemarks,
        //                            Rate = p.Rate,
        //                            TotalRate = p.TotalRate,

        //                        });
        //                        mLNo += 1;
        //                    }
        //                    context.IssueToLocationDetails.AddRange(items);
        //                    context.SaveChanges();
        //                }
        //                transaction.Commit();
        //                returnValue = true;
        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //    return returnValue;
        //}
        public static IssueToLocationHeadViewModel GetIssueToLocationHeadById(int id)
        {
            IssueToLocationHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from IssueToLocationHead where Id={id}";
                    returnValue = context.Database.SqlQuery<IssueToLocationHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<IssueToLocationDetailViewModel> GetIssueToLocationDetailsByHeadId(int RefID)
        {
            List<IssueToLocationDetailViewModel> returnValue = new List<IssueToLocationDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from IssueToLocationDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<IssueToLocationDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<IssueToLocationHeadViewModel> GetIssueToLocationHeads()
        {
            List<IssueToLocationHeadViewModel> returnValue = new List<IssueToLocationHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from IssueToLocationHead";
                    returnValue = context.Database.SqlQuery<IssueToLocationHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteIssueToLocation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.IssueToLocationDetails.Where(p => p.Id == id).ToList();
                    var del = context.IssueToLocationHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.IssueToLocationDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.IssueToLocationHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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
        public static List<IssueToLocationDetailViewModel> GetAllIssueToLocationItemsName()
        {
            List<IssueToLocationDetailViewModel> returnValue = new List<IssueToLocationDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select ItemId, ItemName from IssueToLocationDetail group by ItemId, ItemName";
                    returnValue = context.Database.SqlQuery<IssueToLocationDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
        #region ReturnToWarehouse Functions
        public static bool AddReturnToWarehouse(ReturnToWarehouseHeadViewModel model)
        {
            bool returnValue = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ReturnToWarehouseHead entity = (ReturnToWarehouseHead)model;
                        context.ReturnToWarehouseHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.ReturnToWarehouseDetails?.Count > 0)
                        {
                            List<ReturnToWarehouseDetail> purchaseDetail = new List<ReturnToWarehouseDetail>();
                            int mLNo = 1;
                            foreach (var p in model.ReturnToWarehouseDetails)
                            {
                                purchaseDetail.Add(new ReturnToWarehouseDetail
                                {
                                    Id = entity.Id,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                    ItemId = p.ItemId,
                                    ItemName = p.ItemName,
                                    Quantity = p.Quantity,
                                    LineRemarks = p.LineRemarks,
                                    Rate = p.Rate,
                                    TotalRate = p.TotalRate,
                                });
                                mLNo += 1;
                            }
                            context.ReturnToWarehouseDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        returnValue = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnValue;
        }
        public static ReturnToWarehouseHeadViewModel GetReturnToWarehouseHeadById(int id)
        {
            ReturnToWarehouseHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToWarehouseHead where Id={id}";
                    returnValue = context.Database.SqlQuery<ReturnToWarehouseHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ReturnToWarehouseDetailViewModel> GetReturnToWarehouseDetailsByHeadId(int RefID)
        {
            List<ReturnToWarehouseDetailViewModel> returnValue = new List<ReturnToWarehouseDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToWarehouseDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<ReturnToWarehouseDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ReturnToWarehouseHeadViewModel> GetReturnToWarehouseHeads()
        {
            List<ReturnToWarehouseHeadViewModel> returnValue = new List<ReturnToWarehouseHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToWarehouseHead";
                    returnValue = context.Database.SqlQuery<ReturnToWarehouseHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteReturnToWarehouse(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.ReturnToWarehouseDetails.Where(p => p.Id == id).ToList();
                    var del = context.ReturnToWarehouseHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.ReturnToWarehouseDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.ReturnToWarehouseHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        #endregion

        #region ReturnToVendor Functions

        public static bool AddReturnToVendor(ReturnToVendorHeadViewModel model)
        {
            bool returnValue = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ReturnToVendorHead entity = (ReturnToVendorHead)model;
                        context.ReturnToVendorHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.ReturnToVendorDetails?.Count > 0)
                        {
                            List<ReturnToVendorDetail> purchaseDetail = new List<ReturnToVendorDetail>();
                            int mLNo = 1;
                            foreach (var p in model.ReturnToVendorDetails)
                            {
                                purchaseDetail.Add(new ReturnToVendorDetail
                                {
                                    Id = entity.Id,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                    ItemId = p.ItemId,
                                    ItemName = p.ItemName,
                                    Quantity = p.Quantity,
                                    LineRemarks = p.LineRemarks,
                                    Rate = p.Rate,
                                    TotalRate = p.TotalRate,
                                });
                                mLNo += 1;
                            }
                            context.ReturnToVendorDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        returnValue = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnValue;
        }
        public static ReturnToVendorHeadViewModel GetReturnToVendorHeadById(int id)
        {
            ReturnToVendorHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToVendorHead where Id={id}";
                    returnValue = context.Database.SqlQuery<ReturnToVendorHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ReturnToVendorDetailViewModel> GetReturnToVendorDetailsByHeadId(int RefID)
        {
            List<ReturnToVendorDetailViewModel> returnValue = new List<ReturnToVendorDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToVendorDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<ReturnToVendorDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ReturnToVendorHeadViewModel> GetReturnToVendorHeads()
        {
            List<ReturnToVendorHeadViewModel> returnValue = new List<ReturnToVendorHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ReturnToVendorHead";
                    returnValue = context.Database.SqlQuery<ReturnToVendorHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteReturnToVendor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.ReturnToVendorDetails.Where(p => p.Id == id).ToList();
                    var del = context.ReturnToVendorHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.ReturnToVendorDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.ReturnToVendorHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        #endregion

        #region Wastage Items Functions

        public static bool AddWastage(WastageHeadViewModel model)
        {
            bool returnValue = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        WastageHead entity = (WastageHead)model;
                        context.WastageHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.WastageDetails?.Count > 0)
                        {
                            List<WastageDetail> purchaseDetail = new List<WastageDetail>();
                            int mLNo = 1;
                            foreach (var p in model.WastageDetails)
                            {
                                purchaseDetail.Add(new WastageDetail
                                {
                                    Id = entity.Id,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                    ItemId = p.ItemId,
                                    ItemName = p.ItemName,
                                    Quantity = p.Quantity,
                                    LineRemarks = p.LineRemarks,
                                    Rate = p.Rate,
                                    TotalRate = p.TotalRate,
                                });
                                mLNo += 1;
                            }
                            context.WastageDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        returnValue = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnValue;
        }
        public static WastageHeadViewModel GetWastageHeadById(int id)
        {
            WastageHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from WastageHead where Id={id}";
                    returnValue = context.Database.SqlQuery<WastageHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<WastageDetailViewModel> GetWastageDetailsByHeadId(int RefID)
        {
            List<WastageDetailViewModel> returnValue = new List<WastageDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from WastageDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<WastageDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<WastageHeadViewModel> GetWastageHeads()
        {
            List<WastageHeadViewModel> returnValue = new List<WastageHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from WastageHead";
                    returnValue = context.Database.SqlQuery<WastageHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteWastage(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.WastageDetails.Where(p => p.Id == id).ToList();
                    var del = context.WastageHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.WastageDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.WastageHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        #endregion

        #region Closing Inventory Items Functions

        public static bool AddClosingInventory(ClosingInventoryHeadViewModel model)
        {
            bool returnValue = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        ClosingInventoryHead entity = (ClosingInventoryHead)model;
                        context.ClosingInventoryHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.ClosingInventoryDetails?.Count > 0)
                        {
                            List<ClosingInventoryDetail> purchaseDetail = new List<ClosingInventoryDetail>();
                            int mLNo = 1;
                            foreach (var p in model.ClosingInventoryDetails)
                            {
                                purchaseDetail.Add(new ClosingInventoryDetail
                                {
                                    Id = entity.Id,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                    ItemId = p.ItemId,
                                    ItemName = p.ItemName,
                                    Quantity = p.Quantity,
                                    LineRemarks = p.LineRemarks,
                                    Rate = p.Rate,
                                    TotalRate = p.TotalRate,
                                });
                                mLNo += 1;
                            }
                            context.ClosingInventoryDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        returnValue = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnValue;
        }
        public static ClosingInventoryHeadViewModel GetClosingInventoryHeadById(int id)
        {
            ClosingInventoryHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ClosingInventoryHead where Id={id}";
                    returnValue = context.Database.SqlQuery<ClosingInventoryHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ClosingInventoryDetailViewModel> GetClosingInventoryDetailsByHeadId(int RefID)
        {
            List<ClosingInventoryDetailViewModel> returnValue = new List<ClosingInventoryDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ClosingInventoryDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<ClosingInventoryDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ClosingInventoryHeadViewModel> GetClosingInventoryHeads()
        {
            List<ClosingInventoryHeadViewModel> returnValue = new List<ClosingInventoryHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ClosingInventoryHead";
                    returnValue = context.Database.SqlQuery<ClosingInventoryHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteClosingInventory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.ClosingInventoryDetails.Where(p => p.Id == id).ToList();
                    var del = context.ClosingInventoryHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.ClosingInventoryDetails.RemoveRange(delList);
                        if (del != null)
                        {
                            context.ClosingInventoryHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        #endregion

        #region Vendor Functions

        public static bool AddVendor(VendorViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Vendor entity = (Vendor)model;
                    context.Vendors.Add(entity);
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

        public static VendorViewModel GetVendorById(int id)
        {
            VendorViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                   
                    string SQL = $"select * from Vendors where Id={id}";
                    returnValue = context.Database.SqlQuery<Vendor>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetVendorNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select VendorName from Vendors where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VendorViewModel> GetAllVendors(string search = null)
        {
            List<VendorViewModel> returnValue = new List<VendorViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Vendors";
                    var Vendor = context.Database.SqlQuery<Vendor>(SQL).ToList();
                    returnValue = Vendor.Select(p => (VendorViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VendorViewModel> GetVendorsByName()
        {
            List<VendorViewModel> returnValue = new List<VendorViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select FullName from Vendors";
                    var Vendor = context.Database.SqlQuery<Vendor>(SQL).ToList();
                    returnValue = Vendor.Select(p => (VendorViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateVendor(VendorViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Vendors.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.VendorName))
                            find.VendorName = model.VendorName;
                        if (!string.IsNullOrWhiteSpace(model.ContactPerson))
                            find.ContactPerson = model.ContactPerson;
                        if (!string.IsNullOrWhiteSpace(model.Email))
                            find.Email = model.Email;
                        if (!string.IsNullOrWhiteSpace(model.Mobile))
                            find.Mobile = model.Mobile;
                        if (!string.IsNullOrWhiteSpace(model.Telephone1))
                            find.Telephone1 = model.Telephone1;
                        if (!string.IsNullOrWhiteSpace(model.Telephone2))
                            find.Telephone2 = model.Telephone2;
                        if (!string.IsNullOrWhiteSpace(model.Address))
                            find.Address = model.Address;
                        if (!string.IsNullOrWhiteSpace(model.CNIC))
                            find.CNIC = model.CNIC;
                        if (!string.IsNullOrWhiteSpace(model.NTN))
                            find.NTN = model.NTN;
                        if (!string.IsNullOrWhiteSpace(model.GstNo))
                            find.GstNo = model.GstNo;
                        if (model.CityId > 0)
                            find.CityId = model.CityId;
                        find.ModifyDate = model.ModifyDate;
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

        public static bool DeleteVendor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Vendors.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Vendors.Remove(del);
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

        public static bool DisableVendor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Vendors.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableVendor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Vendors.Where(p => p.Id == id).SingleOrDefault();
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

        #region City Functions

        public static bool AddCity(CityViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    City entity = (City)model;
                    context.Cities.Add(entity);
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

        public static CityViewModel GetCityById(int id)
        {
            CityViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Cities where Id={id}";
                    returnValue = context.Database.SqlQuery<City>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetCityName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Name from Cities where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<CityViewModel> GetAllCities(string search = null)
        {
            List<CityViewModel> returnValue = new List<CityViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Cities";
                    var City = context.Database.SqlQuery<City>(SQL).ToList();
                    returnValue = City.Select(p => (CityViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<CityViewModel> GetCitiesByName()
        {
            List<CityViewModel> returnValue = new List<CityViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Cities";
                    var City = context.Database.SqlQuery<City>(SQL).ToList();
                    returnValue = City.Select(p => (CityViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateCity(CityViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetCityById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Cities set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
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

        public static bool DeleteCity(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Cities.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Cities.Remove(del);
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

        public static bool DisableCity(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Cities.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableCity(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Cities.Where(p => p.Id == id).SingleOrDefault();
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

        #region Location Functions

        public static bool AddLocation(LocationViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Location entity = (Location)model;
                    context.Locations.Add(entity);
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

        public static LocationViewModel GetLocationById(int id)
        {
            LocationViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Locations where Id={id}";
                    returnValue = context.Database.SqlQuery<Location>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetLocationNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Locations where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<LocationViewModel> GetAllLocations(string search = null)
        {
            List<LocationViewModel> returnValue = new List<LocationViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Locations";
                    var Location = context.Database.SqlQuery<Location>(SQL).ToList();
                    returnValue = Location.Select(p => (LocationViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<LocationViewModel> GetLocationsByName()
        {
            List<LocationViewModel> returnValue = new List<LocationViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Locations";
                    var Location = context.Database.SqlQuery<Location>(SQL).ToList();
                    returnValue = Location.Select(p => (LocationViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateLocation(LocationViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetLocationById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                        };
                        string SQL = $"update Locations set Name=@Name where id = @ID";
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

        public static bool DeleteLocation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Locations.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Locations.Remove(del);
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

        public static bool DisableLocation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Locations.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableLocation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Locations.Where(p => p.Id == id).SingleOrDefault();
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

        #region VendorPayment Functions

        public static bool AddVendorPayment(VendorPaymentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    VendorPayment entity = (VendorPayment)model;
                    context.VendorPayments.Add(entity);
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

        public static VendorPaymentViewModel GetVendorPaymentById(int id)
        {
            VendorPaymentViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorPayments where Id={id}";
                    returnValue = context.Database.SqlQuery<VendorPayment>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorPaymentViewModel> GetAllVendorPayments(string search = null)
        {
            List<VendorPaymentViewModel> returnValue = new List<VendorPaymentViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorPayments";
                    var VendorPayment = context.Database.SqlQuery<VendorPayment>(SQL).ToList();
                    returnValue = VendorPayment.Select(p => (VendorPaymentViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        public static bool UpdateVendorPayment(VendorPaymentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.VendorPayments.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (model.VendorId > 0)
                            find.VendorId = model.VendorId;
                        if (!string.IsNullOrEmpty(model.Remarks))
                            find.Remarks = model.Remarks;
                        if (!string.IsNullOrEmpty(model.DocNo))
                            find.DocNo = model.DocNo;
                        if (model.Payment > 0)
                            find.Payment = model.Payment;
                        if (model.RemainingBalance > 0)
                            find.RemainingBalance = model.RemainingBalance;
                        if (model.TransactionDate != DateTime.MinValue)
                            find.TransactionDate = model.TransactionDate;
                        if (model.ModifyDate != DateTime.MinValue)
                            find.ModifyDate = model.ModifyDate;

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

        public static bool DeleteVendorPayment(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.VendorPayments.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.VendorPayments.Remove(del);
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

        public static string GetVendorPaymentTypeNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Name from VendorPaymentTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorPaymentTypeViewModel> GetAllVendorPaymentTypes()
        {
            List<VendorPaymentTypeViewModel> returnValue = new List<VendorPaymentTypeViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select * from VendorPaymentTypes";
                    var VendorPayment = context.Database.SqlQuery<VendorPaymentType>(SQL).ToList();
                    returnValue = VendorPayment.Select(p => (VendorPaymentTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VenderPaymentLedgerViewModel> GetVenderPaymentLedger(DateTime fromDate, DateTime toDate, int? vendorId = null)
        {
            List<VenderPaymentLedgerViewModel> returnValue = new List<VenderPaymentLedgerViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        SQL.AppendLine("SELECT * FROM ");
                        SQL.AppendLine($"fn_VenderPaymentLedger('{fromDate}','{toDate}') ");
                        SQL.AppendLine("WHERE 1=1 ");
                    }
                    if (vendorId.HasValue)
                    {
                        SQL.Append($"AND VendorId = {vendorId}");
                    }
                    returnValue = context.Database.SqlQuery<VenderPaymentLedgerViewModel>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return returnValue;
        }

        #endregion

        #region opening Functions

        public static bool AddOpeningStock(OpeningStockHeadViewModel model)
        {
            bool returnValue = false;
            using (POSEntities context = new POSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        OpeningHead entity = (OpeningHead)model;
                        context.OpeningHeads.Add(entity);
                        int success = context.SaveChanges();

                        if (success == 1 && model.RawItems?.Count > 0)
                        {
                            List<OpeningDetail> purchaseDetail = new List<OpeningDetail>();
                            int mLNo = 1;
                            foreach (var p in model.RawItems)
                            {
                                purchaseDetail.Add(new OpeningDetail
                                {
                                    Id = entity.Id,
                                    ItemId = p.Id,
                                    ItemName = p.Name,
                                    Quantity = p.Quantity,
                                    Rate = p.Price,
                                    LineRemarks = p.Remarks,
                                    TotalRate = p.SubTotal,
                                    VoucherLineNo = mLNo.ToString().PadLeft(4, '0'),
                                });
                                mLNo += 1;
                            }
                            context.OpeningDetails.AddRange(purchaseDetail);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                        returnValue = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnValue;
        }
        public static OpeningStockHeadViewModel GetOpeningStockHeadById(int id)
        {
            OpeningStockHeadViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from OpeningHead where Id={id}";
                    returnValue = context.Database.SqlQuery<OpeningHead>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OpeningStockDetailViewModel> GetOpeningStockDetailsByHeadId(int RefID)
        {
            List<OpeningStockDetailViewModel> returnValue = new List<OpeningStockDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from OpeningDetail where Id= {RefID}";
                    returnValue = context.Database.SqlQuery<OpeningStockDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<OpeningStockHeadViewModel> GetOpeningStockHeads()
        {
            List<OpeningStockHeadViewModel> returnValue = new List<OpeningStockHeadViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from OpeningHead";
                    returnValue = context.Database.SqlQuery<OpeningStockHeadViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteOpeningStock(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var delList = context.OpeningDetails.Where(p => p.Id == id).ToList();
                    var del = context.OpeningHeads.Where(p => p.Id == id).SingleOrDefault();
                    if (delList?.Count > 0)
                    {
                        context.OpeningDetails.RemoveRange(delList);
                        //context.SaveChanges();
                        if (del != null)
                        {
                            context.OpeningHeads.Remove(del);
                            context.SaveChanges();
                            returnValue = true;
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

        public static OpeningStockDetailViewModel GetOpeningPurchaseItemById(int? ItemId)
        {
            OpeningStockDetailViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select ItemId, ItemName, SUM(Quantity) As Quantity  from OpeningDetail where ItemId={ItemId} group by ItemId, ItemName order By ItemId";
                    returnValue = context.Database.SqlQuery<OpeningStockDetailViewModel>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<OpeningStockDetailViewModel> GetOpeningDetail()
        {
            List<OpeningStockDetailViewModel> returnValue = new List<OpeningStockDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from OpeningDetail";
                    returnValue = context.Database.SqlQuery<OpeningStockDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<OpeningStockDetailViewModel> GetAllOpeningPurchaseItemsName()
        {
            List<OpeningStockDetailViewModel> returnValue = new List<OpeningStockDetailViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select ItemId, ItemName from OpeningDetail group by ItemId, ItemName";
                    returnValue = context.Database.SqlQuery<OpeningStockDetailViewModel>(SQL).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion


        #region Customer Functions

        public static bool AddCustomer(CustomerViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Customer entity = (Customer)model;
                    context.Customers.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception ex )
            {
                var sds = ex.ToString();
            }
            return returnValue;
        }

        public static CustomerViewModel GetCustomerById(int id)
        {
            CustomerViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {

                    string SQL = $"select * from Customer where Id={id}";
                    returnValue = context.Database.SqlQuery<Customer>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetCustomerNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select CustomerName from Customer where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<CustomerViewModel> GetAllCustomers(string search = null)
        {
            List<CustomerViewModel> returnValue = new List<CustomerViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Customer";
                    var Vendor = context.Database.SqlQuery<Customer>(SQL).ToList();
                    returnValue = Vendor.Select(p => (CustomerViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<CustomerViewModel> GetCusomersByName()
        {
            List<CustomerViewModel> returnValue = new List<CustomerViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select CustomerName from Customer";
                    var Vendor = context.Database.SqlQuery<Customer>(SQL).ToList();
                    returnValue = Vendor.Select(p => (CustomerViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateCustomer(CustomerViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Customers.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.CustomerName))
                            find.CustomerName = model.CustomerName;
                        if (!string.IsNullOrWhiteSpace(model.Reading))
                            find.Reading = model.Reading;
                        if (!string.IsNullOrWhiteSpace(model.Email))
                            find.Email = model.Email;
                        if (!string.IsNullOrWhiteSpace(model.Mobile))
                            find.Mobile = model.Mobile;
                        if (!string.IsNullOrWhiteSpace(model.Telephone1))
                            find.Telephone1 = model.Telephone1;
                        if (!string.IsNullOrWhiteSpace(model.CarNumber))
                            find.CarNumber = model.CarNumber;
                        if (!string.IsNullOrWhiteSpace(model.Address))
                            find.Address = model.Address;
                        if (!string.IsNullOrWhiteSpace(model.CNIC))
                            find.CNIC = model.CNIC;
                        if (model.CityId > 0)
                            find.CityId = model.CityId;
                        find.ModifyDate = model.ModifyDate;
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

        public static bool DeleteCustomer(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Customers.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Customers.Remove(del);
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

        public static bool DisableCustomer(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Customers.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableCustomer(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Customers.Where(p => p.Id == id).SingleOrDefault();
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
