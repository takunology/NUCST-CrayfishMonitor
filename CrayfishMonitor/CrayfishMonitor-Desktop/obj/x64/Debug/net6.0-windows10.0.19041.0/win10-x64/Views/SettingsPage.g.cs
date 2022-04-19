﻿#pragma checksum "C:\Users\Bio7\Documents\okawa\github\NUCST-CrayfishMonitor\CrayfishMonitor\CrayfishMonitor-Desktop\Views\SettingsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "921F77BB98FE5017B7B0961CA2D7E8BD24374F19CDBFD983ECEF037D14438E4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrayfishMonitor_Desktop.Views
{
    partial class SettingsPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private static class XamlBindingSetters
        {
            public static void Set_Microsoft_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Microsoft.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Microsoft_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(global::Microsoft.UI.Xaml.Controls.Primitives.Selector obj, global::System.Int32 value)
            {
                obj.SelectedIndex = value;
            }
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private class SettingsPage_obj1_Bindings :
            global::Microsoft.UI.Xaml.Markup.IDataTemplateComponent,
            global::Microsoft.UI.Xaml.Markup.IXamlBindScopeDiagnostics,
            global::Microsoft.UI.Xaml.Markup.IComponentConnector,
            ISettingsPage_Bindings
        {
            private global::CrayfishMonitor_Desktop.Views.SettingsPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Microsoft.UI.Xaml.Controls.ComboBox obj2;

            // Static fields for each binding's enabled/disabled state
            private static bool isobj2ItemsSourceDisabled = false;
            private static bool isobj2SelectedIndexDisabled = false;

            private SettingsPage_obj1_BindingsTracking bindingsTracking;

            public SettingsPage_obj1_Bindings()
            {
                this.bindingsTracking = new SettingsPage_obj1_BindingsTracking(this);
            }

            public void Disable(int lineNumber, int columnNumber)
            {
                if (lineNumber == 17 && columnNumber == 27)
                {
                    isobj2ItemsSourceDisabled = true;
                }
                else if (lineNumber == 18 && columnNumber == 27)
                {
                    isobj2SelectedIndexDisabled = true;
                }
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2: // Views\SettingsPage.xaml line 16
                        this.obj2 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ComboBox>(target);
                        this.bindingsTracking.RegisterTwoWayListener_2(this.obj2);
                        break;
                    default:
                        break;
                }
            }
                        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
                        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
                        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target) 
                        {
                            return null;
                        }

            // IDataTemplateComponent

            public void ProcessBindings(global::System.Object item, int itemIndex, int phase, out int nextPhase)
            {
                nextPhase = -1;
            }

            public void Recycle()
            {
                return;
            }

            // ISettingsPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            public void DisconnectUnloadedObject(int connectionId)
            {
                throw new global::System.ArgumentException("No unloadable elements to disconnect.");
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                if (newDataRoot != null)
                {
                    this.dataRoot = global::WinRT.CastExtensions.As<global::CrayfishMonitor_Desktop.Views.SettingsPage>(newDataRoot);
                    return true;
                }
                return false;
            }

            public void Activated(object obj, global::Microsoft.UI.Xaml.WindowActivatedEventArgs data)
            {
                this.Initialize();
            }

            public void Loading(global::Microsoft.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::CrayfishMonitor_Desktop.Views.SettingsPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel(obj.ViewModel, phase);
                    }
                }
            }
            private void Update_ViewModel(global::CrayfishMonitor_Desktop.ViewModels.SettingsPageViewModel obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_BaudRate(obj.BaudRate, phase);
                        this.Update_ViewModel_SelectedBaudRateIndex(obj.SelectedBaudRateIndex, phase);
                    }
                }
            }
            private void Update_ViewModel_BaudRate(global::System.Collections.Generic.List<global::System.Int32> obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\SettingsPage.xaml line 16
                    if (!isobj2ItemsSourceDisabled)
                    {
                        XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj2, obj, null);
                    }
                }
            }
            private void Update_ViewModel_SelectedBaudRateIndex(global::Reactive.Bindings.ReactivePropertySlim<global::System.Int32> obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_ViewModel_SelectedBaudRateIndex(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_ViewModel_SelectedBaudRateIndex_Value(obj.Value, phase);
                    }
                }
            }
            private void Update_ViewModel_SelectedBaudRateIndex_Value(global::System.Int32 obj, int phase)
            {
                if ((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    // Views\SettingsPage.xaml line 16
                    if (!isobj2SelectedIndexDisabled)
                    {
                        XamlBindingSetters.Set_Microsoft_UI_Xaml_Controls_Primitives_Selector_SelectedIndex(this.obj2, obj);
                    }
                }
            }
            private void UpdateTwoWay_2_ItemsSource()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        if (this.dataRoot.ViewModel != null)
                        {
                            this.dataRoot.ViewModel.BaudRate = (global::System.Collections.Generic.List<global::System.Int32>)this.obj2.ItemsSource;
                        }
                    }
                }
            }
            private void UpdateTwoWay_2_SelectedIndex()
            {
                if (this.initialized)
                {
                    if (this.dataRoot != null)
                    {
                        if (this.dataRoot.ViewModel != null)
                        {
                            if (this.dataRoot.ViewModel.SelectedBaudRateIndex != null)
                            {
                                this.dataRoot.ViewModel.SelectedBaudRateIndex.Value = this.obj2.SelectedIndex;
                            }
                        }
                    }
                }
            }

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private class SettingsPage_obj1_BindingsTracking
            {
                private global::System.WeakReference<SettingsPage_obj1_Bindings> weakRefToBindingObj; 

                public SettingsPage_obj1_BindingsTracking(SettingsPage_obj1_Bindings obj)
                {
                    weakRefToBindingObj = new global::System.WeakReference<SettingsPage_obj1_Bindings>(obj);
                }

                public SettingsPage_obj1_Bindings TryGetBindingObject()
                {
                    SettingsPage_obj1_Bindings bindingObject = null;
                    if (weakRefToBindingObj != null)
                    {
                        weakRefToBindingObj.TryGetTarget(out bindingObject);
                        if (bindingObject == null)
                        {
                            weakRefToBindingObj = null;
                            ReleaseAllListeners();
                        }
                    }
                    return bindingObject;
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_ViewModel_SelectedBaudRateIndex(null);
                }

                public void PropertyChanged_ViewModel_SelectedBaudRateIndex(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    SettingsPage_obj1_Bindings bindings = TryGetBindingObject();
                    if (bindings != null)
                    {
                        string propName = e.PropertyName;
                        global::Reactive.Bindings.ReactivePropertySlim<global::System.Int32> obj = sender as global::Reactive.Bindings.ReactivePropertySlim<global::System.Int32>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                bindings.Update_ViewModel_SelectedBaudRateIndex_Value(obj.Value, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "Value":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_ViewModel_SelectedBaudRateIndex_Value(obj.Value, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::Reactive.Bindings.ReactivePropertySlim<global::System.Int32> cache_ViewModel_SelectedBaudRateIndex = null;
                public void UpdateChildListeners_ViewModel_SelectedBaudRateIndex(global::Reactive.Bindings.ReactivePropertySlim<global::System.Int32> obj)
                {
                    if (obj != cache_ViewModel_SelectedBaudRateIndex)
                    {
                        if (cache_ViewModel_SelectedBaudRateIndex != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_ViewModel_SelectedBaudRateIndex).PropertyChanged -= PropertyChanged_ViewModel_SelectedBaudRateIndex;
                            cache_ViewModel_SelectedBaudRateIndex = null;
                        }
                        if (obj != null)
                        {
                            cache_ViewModel_SelectedBaudRateIndex = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_ViewModel_SelectedBaudRateIndex;
                        }
                    }
                }
                public void RegisterTwoWayListener_2(global::Microsoft.UI.Xaml.Controls.ComboBox sourceObject)
                {
                    sourceObject.RegisterPropertyChangedCallback(global::Microsoft.UI.Xaml.Controls.ItemsControl.ItemsSourceProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_2_ItemsSource();
                        }
                    });
                    sourceObject.RegisterPropertyChangedCallback(global::Microsoft.UI.Xaml.Controls.Primitives.Selector.SelectedIndexProperty, (sender, prop) =>
                    {
                        var bindingObj = this.TryGetBindingObject();
                        if (bindingObj != null)
                        {
                            bindingObj.UpdateTwoWay_2_SelectedIndex();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1: // Views\SettingsPage.xaml line 1
                {                    
                    global::Microsoft.UI.Xaml.Controls.Page element1 = (global::Microsoft.UI.Xaml.Controls.Page)target;
                    SettingsPage_obj1_Bindings bindings = new SettingsPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                    global::Microsoft.UI.Xaml.Markup.XamlBindingHelper.SetDataTemplateComponent(element1, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

