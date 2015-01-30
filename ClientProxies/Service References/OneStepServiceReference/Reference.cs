﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientProxies.OneStepServiceReference {
    using System.Runtime.Serialization;
    using System;

    using Common.Contracts;

  [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileStatusData", Namespace="http://schemas.datacontract.org/2004/07/OneStepWcfServices.Contracts")]
    [System.SerializableAttribute()]
    public partial class FileStatusData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApplicationCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApplicationDescField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> EndDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GridField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int GridRunIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClientProxies.OneStepServiceReference.GridRunStatus GridRunStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDebugField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime StartDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationCode {
            get {
                return this.ApplicationCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationCodeField, value) != true)) {
                    this.ApplicationCodeField = value;
                    this.RaisePropertyChanged("ApplicationCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationDesc {
            get {
                return this.ApplicationDescField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationDescField, value) != true)) {
                    this.ApplicationDescField = value;
                    this.RaisePropertyChanged("ApplicationDesc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> EndDate {
            get {
                return this.EndDateField;
            }
            set {
                if ((this.EndDateField.Equals(value) != true)) {
                    this.EndDateField = value;
                    this.RaisePropertyChanged("EndDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Grid {
            get {
                return this.GridField;
            }
            set {
                if ((object.ReferenceEquals(this.GridField, value) != true)) {
                    this.GridField = value;
                    this.RaisePropertyChanged("Grid");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int GridRunId {
            get {
                return this.GridRunIdField;
            }
            set {
                if ((this.GridRunIdField.Equals(value) != true)) {
                    this.GridRunIdField = value;
                    this.RaisePropertyChanged("GridRunId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClientProxies.OneStepServiceReference.GridRunStatus GridRunStatus {
            get {
                return this.GridRunStatusField;
            }
            set {
                if ((this.GridRunStatusField.Equals(value) != true)) {
                    this.GridRunStatusField = value;
                    this.RaisePropertyChanged("GridRunStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDebug {
            get {
                return this.IsDebugField;
            }
            set {
                if ((this.IsDebugField.Equals(value) != true)) {
                    this.IsDebugField = value;
                    this.RaisePropertyChanged("IsDebug");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime StartDate {
            get {
                return this.StartDateField;
            }
            set {
                if ((this.StartDateField.Equals(value) != true)) {
                    this.StartDateField = value;
                    this.RaisePropertyChanged("StartDate");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GridRunStatus", Namespace="http://schemas.datacontract.org/2004/07/OneStepClasses")]
    public enum GridRunStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Undefined = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Processing = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OK = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessAbort = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessException = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ScriptException = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ApplicationTimeout = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EmptyTrigger = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NoData = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessUnexpectedClosure = 9,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OneStepServiceReference.IFileService")]
    public interface IFileService : IServiceContract
    {
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileService/GetFileStatusInfo", ReplyAction="http://tempuri.org/IFileService/GetFileStatusInfoResponse")]
        System.Collections.Generic.List<ClientProxies.OneStepServiceReference.FileStatusData> GetFileStatusInfo(System.DateTime startDate, string companyCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileService/GetFileStatusInfo", ReplyAction="http://tempuri.org/IFileService/GetFileStatusInfoResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<ClientProxies.OneStepServiceReference.FileStatusData>> GetFileStatusInfoAsync(System.DateTime startDate, string companyCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileServiceChannel : ClientProxies.OneStepServiceReference.IFileService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileServiceClient : System.ServiceModel.ClientBase<ClientProxies.OneStepServiceReference.IFileService>, ClientProxies.OneStepServiceReference.IFileService {
        
        public FileServiceClient() {
        }
        
        public FileServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<ClientProxies.OneStepServiceReference.FileStatusData> GetFileStatusInfo(System.DateTime startDate, string companyCode) {
            return base.Channel.GetFileStatusInfo(startDate, companyCode);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<ClientProxies.OneStepServiceReference.FileStatusData>> GetFileStatusInfoAsync(System.DateTime startDate, string companyCode) {
            return base.Channel.GetFileStatusInfoAsync(startDate, companyCode);
        }
    }
}
