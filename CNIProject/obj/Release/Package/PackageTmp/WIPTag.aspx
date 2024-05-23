<%@ Page Title="WIP Tag" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="WIPTag.aspx.vb" Inherits="CNIProjet.WIPTag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}
        .txt-right{
            display:block;
            text-align:right;
        }

        .btn-customblue {
            color: #fff;
            background-color: #061355;
            border-color: #061355; 
        }
        .btn-customblue:hover, .btn-customblue:focus, .btn-customblue:active, .btn-customblue.active, .open>.dropdown-toggle.btn-customblue {
            color: #fff;
            background-color: #061355;
            border-color: #061355; 
        }

        .items-center {
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .hide { display: none; }

    </style>

    <script type="text/javascript">
        window.onbeforeunload = function () {
            $("#<%=print.ClientID%>").prop('disabled', true);
        };
    </script>

    <script type="text/javascript">
        
        $(document).ready(function () {
            if (document.getElementById('<%=checkfunction1.ClientID%>').checked == true) {
                $("#<%=deljob2.ClientID%>").prop('disabled', true);
                $("#<%=deljob2.ClientID%>").selectpicker('refresh');
            }

            if (document.getElementById('<%=checkfunction2.ClientID%>').checked == true) {
                $("#<%=deljob2.ClientID%>").prop('disabled', false);
                $("#<%=deljob2.ClientID%>").selectpicker('refresh');
            }
        })

        function EnablePrint() {
            $("#<%=print.ClientID%>").prop('disabled', false);
        }

        function DisabledPrint() {
            $("#<%=print.ClientID%>").prop('disabled', true);
        }
    </script>

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            )

        }

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
        <div class="se-pre-con"></div>
        <div class="col-lg-12">
            <div class="card border-primary">

                <div class="card-header py-12">
                    <div class="row">
                        <div class="col-md-9">
                            <h6 class="m-0"><%--<a href="Menu.aspx">Menu</a><span class="mx-2 mb-0">/</span>--%><strong class="text-black"><%: Page.Title %></strong>  </h6>
                        </div>

                    </div>
                </div>

                <div class="card-body">

                    <div class="form-row mb-1">
                        <div class="col-12">
                            <div class="row align-items-right">
                                <div class="col-sm-3 text-right width-div">
                                    <asp:Label ID="lblbarcode" runat="server" Text="Barcode Job: "></asp:Label>
                                </div>
                                <div class="col-sm-5 width-div">
                                    <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                    
                                </div>
                                <div style="margin-top:20px;display:none;">
                                    <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                </div>
                                    
                            </div>
                        </div>
                                
                    </div>

                    <div class="form-row mb-1">
                        <div class="col-6">
                            <div class="card">
                                <div class="card-body pt-1 pb-2">
                                    <span>Funcion</span><br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton id="checkfunction1" runat="server" GroupName="CheckFunction" AutoPostBack="true"  OnCheckedChanged="checkfunction1_CheckedChanged" ></asp:RadioButton>
                                                <label class="form-check-label" for="checkfunction1">&nbsp;New&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton id="checkfunction2" runat="server" GroupName="CheckFunction" AutoPostBack="true" OnCheckedChanged="checkfunction2_CheckedChanged" ></asp:RadioButton>
                                                <label class="form-check-label" for="checkfunction2">&nbsp;Re-Print&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-6">
                            <div class="card">
                                <div class="card-body pt-1 pb-2">
                                    <span>Format Tag</span><br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton id="RadioFormat1" runat="server" GroupName="CheckFormat" AutoPostBack="true" Checked="false" OnCheckedChanged="RadioFormat1_CheckedChanged"></asp:RadioButton>
                                                <label class="form-check-label" for="RadioFormat1">&nbsp;Outsite Tag&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton id="RadioFormat2" runat="server" GroupName="CheckFormat" AutoPostBack="true" Checked="true" OnCheckedChanged="RadioFormat2_CheckedChanged"></asp:RadioButton>
                                                <label class="form-check-label" for="RadioFormat2">&nbsp;In-House Tag&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-row mb-1">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body pt-1 pb-2">
                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="row d-flex justify-content-center">
                                                <span>Starting</span>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="row d-flex justify-content-center">
                                                <span>Ending</span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4 ">
                                                    <div class="input-group" style="display:block; text-align:right;">
                                                        <span>Job : </span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="deljob1" runat="server" class="form-control form-control-sm mb-1 selectpicker" AutoPostBack="true" data-live-search="true" ></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col-2">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="deljob1_suffix" runat="server" class="form-control form-control-sm txt-margin">0000</asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-row">
                                                <div class="col-4">
                                                    <div class="input-group">
                                                        <span></span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="deljob2" runat="server" class="form-control form-control-sm mb-1 selectpicker" AutoPostBack="true" data-live-search="true"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col-2">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="deljob2_suffix" runat="server" class="form-control form-control-sm txt-margin">9999</asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group" style="display:block; text-align:right;">
                                                        <span>Job Date : </span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="startdate" runat="server" class="form-control form-control-sm txt-margin datepicker" AutoComplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group">
                                                        <span></span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="enddate" runat="server" class="form-control form-control-sm txt-margin datepicker" AutoComplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group" style="display:block; text-align:right;">
                                                        <span>Operation : </span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="OperDd" runat="server" class="form-control form-control-sm mb-1 selectpicker" AutoPostBack="false" data-live-search="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                            </div>
                                        </div>

                                        
                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group">
                                                        <span></span>
                                                    </div>
                                                </div>
                                                <div class="col-8">
                                                    <div class="input-group">
                                                        <asp:CheckBox ID="ckOper" class="form-check-input txt-margin" runat="server" AutoPostBack="true" /><span>Frist Oper And Next Oper  Are Outsite</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group" style="display:block; text-align:right;">
                                                        <span>Tag ID : </span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtStartTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                        <%--<asp:DropDownList ID="tagidstart" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group">
                                                        <span></span>
                                                    </div>
                                                </div>

                                                <div class="col-5">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtEndTagID" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                                        <%--<asp:DropDownList ID="tagidend" runat="server" class="form-control form-control-sm txt-margin selectpicker" data-live-search="true" AutoPostBack="true"></asp:DropDownList>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-1">
                                        <div class="col-md-6">
                                            <div class="form-row mb-1">
                                                <div class="col-4">
                                                    <div class="input-group" style="display:block; text-align:right;">
                                                        <span>Qty per Tag : </span>
                                                    </div>
                                                </div>

                                                <div class="col-6">
                                                    <div class="input-group">
                                                        <textarea id="txtarea" runat="server" name="txtarea" class="form-control form-control-sm txt-margin" rows="2" cols="50"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                 </div>


                                </div>
                            </div>
                        </div>

                    <div class="form-row mb-1">
                        <div class="col-12">
                            <div class="card-body pt-1 pb-2">
                                <div class="row mb-1">
                                    <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                        <asp:Button ID="preview"  runat="server" Text="Preview" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                        <asp:Button ID="print"  runat="server" Text="Print" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />

                                        <asp:Button ID="btnclear"  runat="server" Text="Clear Tag" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                    </div>
                                </div>


                            </div>
                        </div>

                    </div>

                    <div id="divGrid" style="font-size:13px">
                        <div class="form-row mb-1">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-body pt-1 pb-2">
                                        <div class="row mb-1">
                                            <div class="col-sm-12 mt-2 d-flex justify-content-left">
                                                <asp:Button ID="BTSelectAll"  runat="server" Text="Select All" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                                <asp:Button ID="BTDeSelectAll"  runat="server" Text="DeSelect All" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                            </div>
                                        </div>


                                        <div class="table-responsive">
                                            <asp:GridView ID="GridView2" CssClass="table table-bordered" 
                                                runat="server" AutoGenerateColumns="false"
                                                EnableModelValidation="True" 
                                                DataKeyNames="TagID">
                                                 <Columns>
                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" class="form-check-input txt-margin" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>  
                                                    <asp:BoundField HeaderText="Error" DataField="Error" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:TemplateField HeaderText="Tag Format" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblTagFormat" runat="server" Text='<%# Eval("TagFormat") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                            
                                                    <asp:BoundField HeaderText="Job" DataField="Job" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="Suffix" DataField="Suffix" ReadOnly="true"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField HeaderText="OperNum" DataField="OperNum" ReadOnly="true"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField HeaderText="Item" DataField="Item" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="ItemDescription" DataField="Description" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="TagId" DataField="TagId" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:TemplateField HeaderText="Tag Qty" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                                <asp:Label ID="lblTagQty" runat="server" Text='<%# Eval("TagQty") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField HeaderText="UM" DataField="UM" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                                            
                                                    <asp:TemplateField HeaderText="Total Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblTotalWeight" runat="server" Text='<%# Eval("TotalWeight") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Package Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblPackWeight" runat="server" Text='<%# Eval("PackageWeight") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Part Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblPartWeight" runat="server" Text='<%# Eval("PartWeight") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Gross Weight" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                        <asp:Label ID="lblGrossWeight" runat="server" Text='<%# Eval("GrossWeight") %>'></asp:Label>                
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField HeaderText="NextOper" DataField="NextOperNum" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="PoNo" DataField="PoNum" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="PoLine" DataField="PoLine" ReadOnly="true"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField HeaderText="PoRelease" DataField="PoRelease" ReadOnly="true"  ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField HeaderText="VendorCode" DataField="VendNum" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="Vendorname" DataField="VendName" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField HeaderText="Warehouse" DataField="Whse" ReadOnly="true"  ItemStyle-HorizontalAlign="Left"/>
                                                    <asp:BoundField DataField="RowPointer" HeaderText="" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="vBgTaskName" runat="server" />
                <asp:HiddenField ID="vCheckFunciotn" runat="server" />
                <asp:HiddenField ID="vTagType" runat="server" />
                <asp:HiddenField ID="vCkeckOper" runat="server" />
        
                </div>

            </div>
</asp:Content>

