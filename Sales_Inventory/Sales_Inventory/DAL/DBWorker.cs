﻿using System;
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

            using (var tran = context.Database.Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
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

        #region [Employee]
        private GenericRepository<Employee> employeeEntity;
        public GenericRepository<Employee> EmployeeEntity
        {
            get
            {
                if (this.employeeEntity == null)
                {
                    this.employeeEntity = new GenericRepository<Employee>(context);
                }
                return employeeEntity;
            }
        }
        #endregion
    }
}