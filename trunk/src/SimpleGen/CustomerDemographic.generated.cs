// <autogenerated>
//   This file was generated by T4 code generator DataClasses1.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

namespace Simple.Generator
{
    using System;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table(Name = "dbo.CustomerDemographics")]
    public partial class CustomerDemographic : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private string customerTypeID;
        private string customerDesc;
        private EntitySet<CustomerCustomerDemo> customerCustomerDemos;
        
        public CustomerDemographic()
        {
            this.customerCustomerDemos = new EntitySet<CustomerCustomerDemo>(this.AttachCustomerCustomerDemos, this.DetachCustomerCustomerDemos);
            this.OnCreated();
        }
        
        public event PropertyChangingEventHandler PropertyChanging;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Column(Name = "CustomerTypeID", Storage = "customerTypeID", CanBeNull = false, DbType = "NChar(10) NOT NULL", IsPrimaryKey = true)]
        public string CustomerTypeID
        {
            get
            {
                return this.customerTypeID;
            }
        
            set
            {
                if (this.customerTypeID != value)
                {
                    this.OnCustomerTypeIDChanging(value);
                    this.SendPropertyChanging("CustomerTypeID");
                    this.customerTypeID = value;
                    this.SendPropertyChanged("CustomerTypeID");
                    this.OnCustomerTypeIDChanged();
                }
            }
        }

        [Column(Name = "CustomerDesc", Storage = "customerDesc", CanBeNull = true, DbType = "NText", UpdateCheck = UpdateCheck.Never)]
        public string CustomerDesc
        {
            get
            {
                return this.customerDesc;
            }
        
            set
            {
                if (this.customerDesc != value)
                {
                    this.OnCustomerDescChanging(value);
                    this.SendPropertyChanging("CustomerDesc");
                    this.customerDesc = value;
                    this.SendPropertyChanged("CustomerDesc");
                    this.OnCustomerDescChanged();
                }
            }
        }

        [Association(Name = "CustomerDemographic_CustomerCustomerDemo", Storage = "customerCustomerDemos", ThisKey = "CustomerTypeID", OtherKey = "CustomerTypeID")]
        public EntitySet<CustomerCustomerDemo> CustomerCustomerDemos
        {
            get 
            {
                return this.customerCustomerDemos; 
            }
        
            set 
            { 
                this.customerCustomerDemos.Assign(value); 
            }
        }
        
        protected virtual void SendPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        
        protected virtual void SendPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private void AttachCustomerCustomerDemos(CustomerCustomerDemo entity)
        {
            this.SendPropertyChanging("CustomerCustomerDemos");
            entity.CustomerDemographic = this;
        }
        
        private void DetachCustomerCustomerDemos(CustomerCustomerDemo entity)
        {
            this.SendPropertyChanging("CustomerCustomerDemos");
            entity.CustomerDemographic = null;
        }
        
        #region Extensibility methods
        
        partial void OnCreated();
        
        partial void OnLoaded();
        
        partial void OnValidate(ChangeAction action);
        
        partial void OnCustomerTypeIDChanging(string value);
        
        partial void OnCustomerTypeIDChanged();
        
        partial void OnCustomerDescChanging(string value);
        
        partial void OnCustomerDescChanged();
        
        #endregion
    }
}
