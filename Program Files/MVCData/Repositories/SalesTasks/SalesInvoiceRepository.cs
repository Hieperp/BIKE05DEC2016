using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;



namespace MVCData.Repositories.SalesTasks
{
    public abstract class SalesInvoiceRepository : GenericWithDetailRepository<SalesInvoice, SalesInvoiceDetail>, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : this(totalBikePortalsEntities, null) { }

        public SalesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable)
            : this(totalBikePortalsEntities, functionNameEditable, null) { }

        public SalesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable, string functionNameApproved)
            : this(totalBikePortalsEntities, functionNameEditable, functionNameApproved, null) { }

        public SalesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities, string functionNameEditable, string functionNameApproved, string functionNameDeletable)
            : base(totalBikePortalsEntities, functionNameEditable, functionNameApproved, functionNameDeletable)
        {
        }


    }







    public class VehiclesInvoiceRepository : SalesInvoiceRepository, IVehiclesInvoiceRepository
    {
        public VehiclesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "VehiclesInvoiceEditable", null, "VehiclesInvoiceDeletable")
        {
            //return;
            //return;

            Helpers.SqlProgrammability.StockTasks.Inventories m = new Helpers.SqlProgrammability.StockTasks.Inventories(totalBikePortalsEntities);
            m.RestoreProcedure();

            Helpers.SqlProgrammability.SalesTasks.AccountInvoice vat = new Helpers.SqlProgrammability.SalesTasks.AccountInvoice(totalBikePortalsEntities);
            vat.RestoreProcedure();

            //return;
            Helpers.SqlProgrammability.SalesTasks.SalesInvoice si = new Helpers.SqlProgrammability.SalesTasks.SalesInvoice(totalBikePortalsEntities);
            si.RestoreProcedure();

            //return;
            Helpers.SqlProgrammability.SalesTasks.ServicesInvoice t = new Helpers.SqlProgrammability.SalesTasks.ServicesInvoice(totalBikePortalsEntities);
            t.RestoreProcedure();

            
            Helpers.SqlProgrammability.SalesTasks.PartsInvoice y = new Helpers.SqlProgrammability.SalesTasks.PartsInvoice(totalBikePortalsEntities);
            y.RestoreProcedure();




            Helpers.SqlProgrammability.SalesTasks.VehiclesInvoice x = new Helpers.SqlProgrammability.SalesTasks.VehiclesInvoice(totalBikePortalsEntities);
            x.RestoreProcedure();
                                    


            return;
            Helpers.SqlProgrammability.StockTasks.TransferOrder to = new Helpers.SqlProgrammability.StockTasks.TransferOrder(totalBikePortalsEntities);
            to.RestoreProcedure();


            
            Helpers.SqlProgrammability.CommonTasks.AccessControl acl = new Helpers.SqlProgrammability.CommonTasks.AccessControl(totalBikePortalsEntities);
            acl.RestoreProcedure();


            
            

            
            
            
            
            

            
            Helpers.SqlProgrammability.StockTasks.VehicleTransfer v = new Helpers.SqlProgrammability.StockTasks.VehicleTransfer(totalBikePortalsEntities);
            v.RestoreProcedure();

            
            
            
            

                        

            

            
            Helpers.SqlProgrammability.StockTasks.PartTransfer b = new Helpers.SqlProgrammability.StockTasks.PartTransfer(totalBikePortalsEntities);
            b.RestoreProcedure();

            
            

            
            
            
            Helpers.SqlProgrammability.SalesTasks.ServiceContracts n = new Helpers.SqlProgrammability.SalesTasks.ServiceContracts(totalBikePortalsEntities);
            n.RestoreProcedure();

            
            Helpers.SqlProgrammability.StockTasks.PartAdjustment pa = new Helpers.SqlProgrammability.StockTasks.PartAdjustment(totalBikePortalsEntities);
            pa.RestoreProcedure();

            Helpers.SqlProgrammability.StockTasks.VehicleAdjustment va = new Helpers.SqlProgrammability.StockTasks.VehicleAdjustment(totalBikePortalsEntities);
            va.RestoreProcedure();

            
            
            
            
            
            
            
            
            
            


            Helpers.SqlProgrammability.PurchaseTasks.PurchaseOrder o = new Helpers.SqlProgrammability.PurchaseTasks.PurchaseOrder(totalBikePortalsEntities);
            o.RestoreProcedure();

            Helpers.SqlProgrammability.PurchaseTasks.PurchaseInvoice z = new Helpers.SqlProgrammability.PurchaseTasks.PurchaseInvoice(totalBikePortalsEntities);
            z.RestoreProcedure();



            Helpers.SqlProgrammability.SalesTasks.Quotation q = new Helpers.SqlProgrammability.SalesTasks.Quotation(totalBikePortalsEntities);
            q.RestoreProcedure();


            
            
            
            
           


            
            Helpers.SqlProgrammability.StockTasks.GoodsReceipt a = new Helpers.SqlProgrammability.StockTasks.GoodsReceipt(totalBikePortalsEntities);
            a.RestoreProcedure();


            

            
            Helpers.SqlProgrammability.CommonTasks.Commons cmm = new Helpers.SqlProgrammability.CommonTasks.Commons(totalBikePortalsEntities);
            cmm.RestoreProcedure();
            
            
        }
    }








    public class PartsInvoiceRepository : SalesInvoiceRepository, IPartsInvoiceRepository
    {
        public PartsInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "PartsInvoiceEditable", "PartsInvoiceApproved")
        {
        }
    }








    public class ServicesInvoiceRepository : SalesInvoiceRepository, IServicesInvoiceRepository
    {
        public ServicesInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "ServicesInvoiceEditable", "ServicesInvoiceApproved", "ServicesInvoiceDeletable")
        {
        }

        public IList<ServiceInvoiceResult> GetActiveServiceInvoices(int locationID, int? serviceInvoiceID, string searchText, int isFinished)
        {
            return this.TotalBikePortalsEntities.SearchServiceInvoices(locationID, serviceInvoiceID, searchText, isFinished).ToList();
        }

        public IList<RelatedPartsInvoiceValue> GetRelatedPartsInvoiceValue(int serviceInvoiceID)
        {
            return this.TotalBikePortalsEntities.GetRelatedPartsInvoiceValue(serviceInvoiceID).ToList();
        }

    }





    public class VehiclesInvoiceAPIRepository : GenericAPIRepository, IVehiclesInvoiceAPIRepository
    {
        public VehiclesInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetVehiclesInvoiceIndexes")
        {
        }
    }

    public class PartsInvoiceAPIRepository : GenericAPIRepository, IPartsInvoiceAPIRepository
    {
        public PartsInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetPartsInvoiceIndexes")
        {
        }
    }

    public class ServicesInvoiceAPIRepository : GenericAPIRepository, IServicesInvoiceAPIRepository
    {
        public ServicesInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetServicesInvoiceIndexes")
        {
        }

        public ICollection<ServicesInvoiceIndex> SearchServicesInvoiceIndexes(int serviceContractID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("ServiceContractID", serviceContractID) };

            return base.ExecuteFunction<ServicesInvoiceIndex>("SearchServicesInvoiceIndexes", parameters);
        }
    }


}
