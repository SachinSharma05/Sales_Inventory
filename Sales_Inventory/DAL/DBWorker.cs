using Sales_Inventory.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;


namespace Sales_Inventory.DAL
{
    public class DBWorker : IDisposable
    {
        #region Variables
        private Sales_InventoryEntities context;
        #endregion

        #region DBWorker Constructor
        public DBWorker()
        {
            context = new Sales_InventoryEntities();
        }
        #endregion

        #region Data Save Method with Rollback and Commit
        //Save method use for save change in database
        //If any problem on saving then rollback otherwise commit changes
        public void Save()
        {
            if (context.Database.Connection.State != System.Data.ConnectionState.Open)
                context.Database.Connection.Open();

            using (var tran = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    context.SaveChanges();
                    tran.Commit();
                }
                catch (DbEntityValidationException ex)
                {
                    tran.Rollback();
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    if (context.Database.Connection.State == System.Data.ConnectionState.Open)
                        context.Database.Connection.Close();
                }
            }
        }
        #endregion

        #region Object Dispose 
        // Despose method use for remove garbage and release memory
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region [User]
        private GenericRepository<User> userEntity;
        public GenericRepository<User> UserEntity
        {
            get
            {
                if (this.userEntity == null)
                {
                    this.userEntity = new GenericRepository<User>(context);
                }
                return userEntity;
            }
        }
        #endregion

        #region [Purchase]
        private GenericRepository<Purchase> purchaseEntity;
        public GenericRepository<Purchase> PurchaseEntity
        {
            get
            {
                if (this.purchaseEntity == null)
                {
                    this.purchaseEntity = new GenericRepository<Purchase>(context);
                }
                return purchaseEntity;
            }
        }
        #endregion

        #region [Sales]
        private GenericRepository<Sale> saleEntity;
        public GenericRepository<Sale> SaleEntity
        {
            get
            {
                if (this.saleEntity == null)
                {
                    this.saleEntity = new GenericRepository<Sale>(context);
                }
                return saleEntity;
            }
        }
        #endregion

        #region [Payment]
        private GenericRepository<Payment> paymentEntity;
        public GenericRepository<Payment> PaymentEntity
        {
            get
            {
                if (this.paymentEntity == null)
                {
                    this.paymentEntity = new GenericRepository<Payment>(context);
                }
                return paymentEntity;
            }
        }
        #endregion

        #region [Advance]
        private GenericRepository<Advance> advanceEntity;
        public GenericRepository<Advance> AdvanceEntity
        {
            get
            {
                if (this.advanceEntity == null)
                {
                    this.advanceEntity = new GenericRepository<Advance>(context);
                }
                return advanceEntity;
            }
        }
        #endregion

        #region [Role]
        private GenericRepository<Role> roleEntity;
        public GenericRepository<Role> RoleEntity
        {
            get
            {
                if (this.roleEntity == null)
                {
                    this.roleEntity = new GenericRepository<Role>(context);
                }
                return roleEntity;
            }
        }
        #endregion

        #region [Purchase_Product]
        private GenericRepository<Purchase_Product> purchaseProductEntity;
        public GenericRepository<Purchase_Product> PurchaseProductEntity
        {
            get
            {
                if (this.purchaseProductEntity == null)
                {
                    this.purchaseProductEntity = new GenericRepository<Purchase_Product>(context);
                }
                return purchaseProductEntity;
            }
        }
        #endregion

        #region [Sale_Product]
        private GenericRepository<Sale_Product> saleProductEntity;
        public GenericRepository<Sale_Product> SaleProductEntity
        {
            get
            {
                if (this.saleProductEntity == null)
                {
                    this.saleProductEntity = new GenericRepository<Sale_Product>(context);
                }
                return saleProductEntity;
            }
        }
        #endregion

        #region [Stock]
        private GenericRepository<Stock> stockEntity;
        public GenericRepository<Stock> StockEntity
        {
            get
            {
                if (this.stockEntity == null)
                {
                    this.stockEntity = new GenericRepository<Stock>(context);
                }
                return stockEntity;
            }
        }
        #endregion

        #region [Product Type]
        private GenericRepository<ProductType> productTypeEntity;
        public GenericRepository<ProductType> ProductTypeEntity
        {
            get
            {
                if (this.productTypeEntity == null)
                {
                    this.productTypeEntity = new GenericRepository<ProductType>(context);
                }
                return productTypeEntity;
            }
        }
        #endregion

        #region [Miscellaneous Expenses]
        private GenericRepository<MiscExpens> miscExpensesEntity;
        public GenericRepository<MiscExpens> MiscExpensesEntity
        {
            get
            {
                if (this.miscExpensesEntity == null)
                {
                    this.miscExpensesEntity = new GenericRepository<MiscExpens>(context);
                }
                return miscExpensesEntity;
            }
        }
        #endregion

        #region [Payment Receipt]
        private GenericRepository<PaymentReceipt> paymentReceiptEntity;
        public GenericRepository<PaymentReceipt> PaymentReceiptEntity
        {
            get
            {
                if (this.paymentReceiptEntity == null)
                {
                    this.paymentReceiptEntity = new GenericRepository<PaymentReceipt>(context);
                }
                return paymentReceiptEntity;
            }
        }
        #endregion

        #region [In-House Transaction]
        private GenericRepository<InHouse> inHouseTransactionEntity;
        public GenericRepository<InHouse> InHouseTransactionEntity
        {
            get
            {
                if (this.inHouseTransactionEntity == null)
                {
                    this.inHouseTransactionEntity = new GenericRepository<InHouse>(context);
                }
                return inHouseTransactionEntity;
            }
        }
        #endregion

        #region [Credit Transaction]
        private GenericRepository<AddNewCredit> addNewCreditEntity;
        public GenericRepository<AddNewCredit> AddNewCreditEntity
        {
            get
            {
                if (this.addNewCreditEntity == null)
                {
                    this.addNewCreditEntity = new GenericRepository<AddNewCredit>(context);
                }
                return addNewCreditEntity;
            }
        }
        #endregion
    }
}