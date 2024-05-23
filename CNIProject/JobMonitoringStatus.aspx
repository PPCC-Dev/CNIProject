<%@ Page Title="Job Monitoring Status" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master"  CodeBehind="JobMonitoringStatus.aspx.vb" Inherits="CNIProjet.JobMonitoringStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .width-div { width: 50%;}

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

    </style>


    <script type="text/javascript">

        function scanbarcode() {

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var i = barcode.IndexOf("|");

            if (i>=1) {
                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                    document.getElementById("notifyWaiting").innerHTML = "Waiting....";
                    document.getElementById('<%=Button1.ClientID%>').click();
                    document.getElementById('<%=Button1.ClientID%>').disabled = true;

            }  else {
                    document.getElementById('<%=txtbarcode.ClientID%>').readOnly = false;
                    document.getElementById("notifyWaiting").innerHTML = "Please Enter....";
                    document.getElementById('<%=Button1.ClientID%>').disabled = false;
            }

        }
        

        function openModelStart() {

            $(document).ready(function () {

                //document.getElementById("ItemModel").innerHTML = Item;
                //document.getElementById("CustItemModel").innerHTML = CustItem;

                //$('#StartDateModel').modal('show');
                $('#StartDateModel').modal({
                    backdrop: 'static',
                    keyboard: true,
                    show: true
                });
                //setTimeout(function() {
                //    $('#StartDateModel').modal('hide');
                //}, 2000);              

            });

        }

        function hideModelStart() {

            $(document).ready(function () {

                $('#StartDateModel').fadeOut();
                $('.modal-backdrop').fadeOut();
                $('.modal-open').css({ 'overflow': 'visible' });        

            });

        }

        function openModelEnd() {

            $(document).ready(function () {

                $('#EndDateModel').modal({
                    backdrop: 'static',
                    keyboard: true,
                    show: true
                });           

            });

        }

        function hideModelEnd() {

            $(document).ready(function () {

                $('#EndDateModel').fadeOut();
                $('.modal-backdrop').fadeOut();
                $('.modal-open').css({ 'overflow': 'visible' });        

            });

        }

        $(document).ready(function () {

                $("#<%=txtBarcode.ClientID%>").attr("placeholder", "Scan Barcode Job");           
             
        });

        function HideDiv() {
            document.getElementById("DisplayJobInfo").style.display = "none";
        }

        function ShowDiv() {
            document.getElementById("DisplayJobInfo").style.display = "block";
        }

        $(document).ready(function () {

            var Resource = $("#<%=ddlResource.ClientID%>").val();

            if (Resource == '') {
                $("#<%=btnAddStart.ClientID%>").prop('disabled', true);
            } else {
                $("#<%=btnAddStart.ClientID%>").prop('disabled', false);
            }
            
        });

        function notstart() {
            $("#CardListJob").addClass("card-body bg-danger");
        }

    </script>

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then(function() {
                window.setTimeout(function () { 
                    document.getElementById('<%=txtBarcode.ClientID%>').focus(); 

            }, 0);});

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

                <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
                 <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server" >
                        <ContentTemplate>
                            <div class="card-body">

                            <div class="row align-items-center">                  
                            </div>

                            <table  width="100%">
                                <tr>
                                    <div class="row align-items-center">
                                        <%--<div class="col-sm-3 text-right width-div"></div>--%>
                                        <div class="col-sm-6 width-div">
                                            <p id="notifyWaiting"></p>
                                        </div>
                                    </div>
                                </tr>
                                <tr>
                                <td>
                                    <div class="form-row">
                                        <div class="col-8">
                                            <div class="input-group mb-3">
                                                <asp:TextBox ID="txtBarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-4">
                                            <div class="input-group mb-3 ml-2">
                                                <asp:Button ID="btnSubmit"  runat="server" Text="Submit" UseSubmitBehavior="false"  class="btn btn-success btn-sm" /> 
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancel"  runat="server" Text="Cancel" UseSubmitBehavior="false" class="btn btn-danger btn-sm" />
                                            </div>
                                            <div style="display: none;">
                                                <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                                
                                            </div>
                                        </div>

                                    </div>
                                </td>
                            </tr>

                                <tr>
                                   <td>
                                       <div style="display:none;">
                                           <asp:Label ID="lblbarcode" runat="server"></asp:Label>
                                       </div>
                                        
                                   </td>
                               </tr>
                        
                            </table>

                            <div class="row mb-2">
                               <div id="DisplayJobInfo" class="col-sm-12">
                                       <div class="card">
                                           <div class="card-body">
                                               <div class="form-row" style="font-size:small;">
                                                    <div class="col-8">
                                                        <div class="input-group">
                                                            <strong><span>Job: </span></strong>&nbsp;<asp:Label id="lblJobText" runat="server"></asp:Label>
                                                            <asp:Label id="lblJob" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label id="lblSuffix" runat="server" Visible="false"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-4">
                                                        <div class="input-group  float-right ml-2">
                                                            <strong><asp:Label id="lblJobStatus" runat="server"></asp:Label></strong>
                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-12">
                                                        <div class="input-group">
                                                            <strong><span>Item: </span></strong>&nbsp;<asp:Label id="lblItem" runat="server"></asp:Label>
                                                            &nbsp;</span><asp:Label id="lblItemDesc" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-12">
                                                        <div class="input-group">
                                                            <strong><span>Operation: </span></strong>&nbsp;<asp:Label id="lblOper" runat="server"></asp:Label>
                                                            <span>-</span><asp:Label id="lblwc" runat="server"></asp:Label>&nbsp;</span><asp:Label id="lblwcdesc" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>Qty Release: </span></strong>&nbsp;<asp:Label id="lblQtyRelease" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>Qty Received: </span></strong>&nbsp;<asp:Label id="lblQtyReceive" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>Qty Complete: </span></strong>&nbsp;<asp:Label id="lblQtyComplete" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>Qty Move : </span></strong>&nbsp;<asp:Label id="lblQtyMove" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-12">
                                                        <div class="input-group">
                                                            <strong><span>Resource: </span></strong>&nbsp;<asp:Label id="lblresource" runat="server"></asp:Label>
                                                            &nbsp;</span><asp:Label id="lblresourceDesc" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                </div>
                                               <div class="form-row" style="font-size:small;">
                                                   <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>Start Date/Time : </span></strong>&nbsp;<asp:Label id="lblStartTime" runat="server"></asp:Label>

                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="input-group">
                                                            <strong><span>End Date/Time : </span></strong>&nbsp;<asp:Label id="lblEndTime" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                                        <asp:Button ID="btnStart"  runat="server" Text="Start" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm mr-3" /> 
                                                        
                                                        <asp:Button ID="btnEnd"  runat="server" Text="End" UseSubmitBehavior="false" class="btn btn-customblue btn-sm mr-3" />
                                                        
                                                        <asp:Button ID="btnUnposted"  runat="server" Text="Unposted" UseSubmitBehavior="false" class="btn btn-customblue btn-sm" />
                                                    </div>
                                                </div>
                                           </div>
                                       </div>
                                    </div>
                                </div>
                            
                            <div class="row mb-2">
                                <div class="col-sm-12">
                                    <div class="form-row">
                                        <div class="col-12">
                                            <div class="input-group">
                                                <strong><span>Job Operations Release</span></strong>

                                            </div>
                                        </div>

                                    </div>
                                    <div id="divJobRelease" runat="server" class="row mt-2">
                                        <div class="col-sm-12 text-left">
                                            <span>Total Job Order : <asp:Label ID="lblTotalJobOrder" runat="server" Font-Bold="False"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                Inprocess : <asp:Label ID="lblTotalInProcess" runat="server" Font-Bold="False"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                Not Start : <asp:Label ID="lblTotalNotStart" runat="server" Font-Bold="False"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                Not Complete : <asp:Label ID="lblTotalNotComplete" runat="server" Font-Bold="False"></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                Not Unposted : <asp:Label ID="lblTotalNotUnposted" runat="server" Font-Bold="False"></asp:Label>
                                            </span><br>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row mb-2 align-items-center" style="overflow-y: scroll; height: 300px">
                                    <div class="col-sm-12">
                                        <asp:ListView id="PanelList" runat="server">     
                                            <ItemTemplate>
                                                <div class="col-sm-12 mt-2">
                                                    <div class="card">
                                                        <div id="CardListJob" class="card-body" style="padding: 1.0rem;">
                                                            <div class="form-row" style="font-size:small;">
                                                                <div class="col-8">
                                                                    <div class="input-group">
                                                                        <strong><span>Job: </span></strong>&nbsp;<asp:Label id="lblListJobText" runat="server" Text='<%#Eval("DerJob")%>'></asp:Label>
                                                                        <asp:Label id="lblListJob" runat="server" Text='<%#Eval("Job")%>' Visible="false"></asp:Label>
                                                                        <asp:Label id="lblListSuffix" runat="server" Text='<%#Eval("Suffix")%>' Visible="false"></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-4">
                                                                    <div class="input-group  float-right ml-2">
                                                                        <strong><span><asp:Label id="lblListJobStatus" runat="server" Text='<%#Eval("Stat")%>'></asp:Label></strong>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="form-row" style="font-size:small;">
                                                                <div class="col-12">
                                                                    <div class="input-group">
                                                                        <strong><span>Operation: </span></strong>&nbsp;<asp:Label id="lblListOper" runat="server" Text='<%#Eval("OperNum")%>'></asp:Label>
                                                                        <span>-</span><asp:Label id="lblListwc" runat="server" Text='<%#Eval("WC")%>'></asp:Label>&nbsp;</span><asp:Label id="lblListwcdesc" runat="server" Text='<%#Eval("WCDesc")%>'></asp:Label>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="form-row" style="font-size:small;">
                                                               <div class="col-12">
                                                                    <div class="input-group">
                                                                        <strong><span>Item: </span></strong>&nbsp;<asp:Label id="lblListItem" runat="server" Text='<%#Eval("Item")%>'></asp:Label>
                                                                        &nbsp;</span><asp:Label id="lblListItemDesc" runat="server" Text='<%#Eval("ItemDesc")%>'></asp:Label>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="form-row" style="font-size:small;">
                                                               <div class="col-12">
                                                                    <div class="input-group">
                                                                        <strong><span>Resource: </span></strong>&nbsp;<asp:Label id="lblListResource" runat="server" Text='<%#Eval("RESID")%>'></asp:Label>

                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        </div>
                            </div>

                      </div>
                        </ContentTemplate>
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                     <asp:PostBackTrigger ControlID="btnSubmit" />
                     <asp:PostBackTrigger ControlID="btnCancel" />
                     <asp:PostBackTrigger ControlID="Button1" />
                     <asp:PostBackTrigger ControlID="btnStart" />
                     <asp:PostBackTrigger ControlID="btnEnd" />
                     <asp:PostBackTrigger ControlID="btnUnposted" />
                 </Triggers>
                </asp:UpdatePanel>

                <asp:HiddenField ID="HiddenField1" runat="server" />

                <div class="container">
                    <div class="modal fade" id="StartDateModel" role="dialog" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">

                            <div class="modal-content">
                            <div class="modal-header">

                                <h4>&nbsp;Start Date and Time</h4>
                                <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>--%>
                            </div>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="modal-body">

                                            <div class="row mb-3">
                                                <div class="col-sm-12 d-flex flex-row-reverse">
                                                    <asp:Button ID="btnStartCancel"  runat="server" Text="Cancel" UseSubmitBehavior="false" class="btn btn-danger btn-sm" />
                                                    &nbsp;&nbsp;<asp:Button ID="btnStartConfrim"  runat="server" Text="Confirm" UseSubmitBehavior="false" class="btn btn-success btn-sm" />
                                                </div>
                                            </div>
                                            <div class="row mb-3">
                                                <div class="col-sm-12" style="display:none;">
                                                    <asp:TextBox ID="txtdate" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            
                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Date</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtStartdate" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Time</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtStartTime" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Resource ID</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:DropDownList ID="ddlResource" runat="server" class="form-control form-control-sm" AutoPostBack="true"
                                                      OnSelectedIndexChanged="ddlResource_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Resource Name</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtResourceName" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                                    <asp:Button ID="btnAddStart"  runat="server" Text="Add" UseSubmitBehavior="false"  class="btn btn-customblue btn-sm" /> 
                                                </div>
                                            </div>

                                            <div class="row mt-2">
                                                <div class="col-sm-12 mt-2">
                                                    <div class="card">
                                                        <div class="card-body" style="font-size:small;">
                                                            <asp:ListView id="ResourceList" runat="server">
                                                                <ItemTemplate>
                                                                    <span><%#Eval("RESID")%>&nbsp;&nbsp;<%#Eval("DESCR")%></span><br>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnStartConfrim" />
                                        <asp:PostBackTrigger ControlID="btnStartCancel" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>

                        </div>
                    </div>
                 </div>

                <div class="container">
                    <div class="modal fade" id="EndDateModel" role="dialog" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">

                            <div class="modal-content">
                            <div class="modal-header">

                                <h4>&nbsp;End Date and Time</h4>
                                <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>--%>
                            </div>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="modal-body">
                                            
                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Date</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtEnddate" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Time</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtEndTime" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Resource</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtEndResource" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div"></p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtEndResourceName" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div">Reason</p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:DropDownList ID="ddlReasonCode" runat="server" class="form-control form-control-sm" AutoPostBack="true"
                                                      OnSelectedIndexChanged="ddlReasonCode_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <p class="col-sm-4 lebel-text text-right width-div"></p>
                                                <div class="col-sm-6 text-left width-div">
                                                     <asp:TextBox ID="txtReasonDesc" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-12 mt-2 d-flex justify-content-center">
                                                    <asp:Button ID="btnEndConfrim"  runat="server" Text="Confirm" UseSubmitBehavior="false" class="btn btn-success btn-sm" />
                                                    &nbsp;&nbsp;<asp:Button ID="btnEndCancel"  runat="server" Text="Cancel" UseSubmitBehavior="false" class="btn btn-danger btn-sm" />
                                                    
                                                </div>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnEndConfrim" />
                                        <asp:PostBackTrigger ControlID="btnEndCancel" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>

                        </div>
                    </div>
                 </div>
            </div>

        </div>

        <asp:HiddenField ID="HidItemModel" runat="server" />
        <asp:HiddenField ID="HidCustItemModel" runat="server" />
</asp:Content>