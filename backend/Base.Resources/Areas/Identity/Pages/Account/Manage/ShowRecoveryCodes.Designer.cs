﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Base.Resources.Areas.Identity.Pages.Account.Manage {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ShowRecoveryCodes {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ShowRecoveryCodes() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Base.Resources.Areas.Identity.Pages.Account.Manage.ShowRecoveryCodes", typeof(ShowRecoveryCodes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string RecoveryCodesTitle {
            get {
                return ResourceManager.GetString("RecoveryCodesTitle", resourceCulture);
            }
        }
        
        public static string PutTheseCodesInSafePlace {
            get {
                return ResourceManager.GetString("PutTheseCodesInSafePlace", resourceCulture);
            }
        }
        
        public static string IfYouLoseYourDeviceOrRecoveryCodes {
            get {
                return ResourceManager.GetString("IfYouLoseYourDeviceOrRecoveryCodes", resourceCulture);
            }
        }
    }
}
