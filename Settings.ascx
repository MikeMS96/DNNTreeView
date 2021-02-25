<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DNNTreeView.Modules.DNNTreeView.Settings" %>


<!-- uncomment the code below to start using the DNN Form pattern to create and update settings -->
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

	<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowHiddenTab" runat="server" suffix=":"/> 
 
            <asp:CheckBox ID="chkShowHiddenTab" runat="server" AutoPostBack="false"/>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowExpandCollapse" runat="server" suffix=":" /> 
 
            <asp:CheckBox ID="chkShowExpandCollapse" runat="server" AutoPostBack="false"/>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblExpandAllTabs" runat="server" suffix=":"/> 
 
            <asp:CheckBox ID="chkExpandAllTabs" runat="server" AutoPostBack="false"/>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowLines" runat="server" suffix=":" /> 
 
            <asp:CheckBox ID="chkShowLines" runat="server" AutoPostBack="false"/>
        </div>
    </fieldset>