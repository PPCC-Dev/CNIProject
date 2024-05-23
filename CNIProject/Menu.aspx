<%@ Page Title="Menu" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="Menu.aspx.vb" Inherits="CNIProjet.Menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btn-custom {
            color: #061355;
        }
        .btn-custom:hover, .btn-custom:focus, .btn-custom:active, .btn-custom.active, .open>.dropdown-toggle.btn-custom {
            color: #061355; 
        }
      
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="se-pre-con"></div>
      <div class="container pt-5">
        <div class="row pl-3">
              <%--<div class="col-6">
                
                <asp:TextBox ID="txtemp" AutoPostBack="true"  CssClass="form-control" runat="server" placeholder="Employee" AutoComplete="off"></asp:TextBox>
              </div>
            <div class="col-6">
                <h3><asp:Label ID="lblempname" runat="server" Visible="false"></asp:Label></h3>
                 
            </div>--%>

        </div>

    <!-- Three columns of text below the carousel -->
    
    <div class="row pt-4">

       <div id="divJobMonitoring" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkJobMonitoring" class="btn-custom" runat="server" NavigateUrl="~/JobMonitoringStatus.aspx">
                <img src="asset/image/job_monitoring.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="..." />
                <h6 class="btn-custom"><strong>Job Monitoring Status</strong></h6>         
            </asp:HyperLink>
        </div>
        

      <div id="divUnposted" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkUnpostedJob" class="btn-custom" runat="server" NavigateUrl="~/Unposted.aspx">        
                <img src="asset/image/unpost_job.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="..." />
                <br/>
                <h6 class="btn-custom"><strong>Unposted Job Transaction</strong></h6>
            </asp:HyperLink>
        </div>
        
        <div id="divPrintProduction" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkPrintProduction" class="btn-custom" runat="server" NavigateUrl="~/PrintProductionDeliveryTag.aspx">        
                <img src="asset/image/print_product_delivery_tag.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="..." />
                <br/><h6 class="btn-custom"><strong>Production Tag and Delivery Tag</strong></h6>
            </asp:HyperLink>
        </div>
      
     
      <div class="col-4 text-center"></div><!-- /.col-lg-4 -->
    </div><!-- /.row -->


    <%--<hr>--%>

    <div class="row pt-4">
       <div id="divJobMatlTran" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkJobMatlTran" class="btn-custom" runat="server" NavigateUrl="~/JobMatlTran.aspx">
                <img src="asset/image/job_matl_tran.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">              
                <br/><h6 class="btn-custom"><strong>Job Material</strong></h6>  
            </asp:HyperLink>
        </div>  
              
        <div id="divJobReceipt" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkJobReceipt" class="btn-custom" runat="server" NavigateUrl="~/JobReceipt.aspx">        
              <img src="asset/image/job_rcpt.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
              <br/><h6 class="btn-custom"><strong>Job Receipt</strong></h6>
            </asp:HyperLink>
        </div>

        <div id="divPrintWIP" class="col-4 text-center chover rounded" runat="server">
              <asp:HyperLink ID="LinkPrintWIP" class="btn-custom" runat="server" NavigateUrl="~/WIPTag.aspx">
                    <img src="asset/image/print_wip_tag.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <br/><h6 class="btn-custom"><strong>WIP Tag</strong></h6>           
               </asp:HyperLink>
         </div>
        
        <div class="col-4 text-center"> </div><!-- /.row -->


    </div><!-- /.col-lg-4 -->
   
    

    <%--<hr>--%>

    <div class="row pt-4">
        <div id="divCycleCount" class="col-4 text-center chover rounded" runat="server">
              <asp:HyperLink ID="LinkCycleCount" class="btn-custom" runat="server" NavigateUrl="~/CycleCount.aspx">
                    <img src="asset/image/cycle_count.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">
                    <h6 class="btn-custom"><strong>Cycle Count</strong></h6>           
               </asp:HyperLink>
         </div>
     
        <div id="divConfirmShipping" class="col-4 text-center chover rounded" runat="server">
          <asp:HyperLink ID="LinkConfirmShipping" class="btn-custom" runat="server" NavigateUrl="~/OrderShipping.aspx">
                <img src="asset/image/order_ship.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">         
                <br/><h6 class="btn-custom"><strong>Order Shipping</strong></h6>
          </asp:HyperLink>
        </div>

        <div id="divConfirmOrderPick" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkConfirmOrderPick" class="btn-custom" runat="server" NavigateUrl="~/ConfirmOrderPickList.aspx">        
                <img src="asset/image/confirm_order_pick.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">        
                <br/><h6 class="btn-custom"><strong>Confirm Order PickList</strong></h6>
            </asp:HyperLink>
        </div>

       <div class="col-4 text-center"></div><!-- /.col-lg-4 -->
     </div><!-- /.row -->

    <div class="row pt-4">

        <div id="divPurchaseOrder" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkPurchaseOrderRecpt" class="btn-custom" runat="server" NavigateUrl="~/POReceiving.aspx">        
                <img src="asset/image/po_rcpt.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">        
                <br/><h6 class="btn-custom"><strong>Purchase Order Receiving</strong></h6>    
            </asp:HyperLink>
        </div>

        
        <div id="divCheckOrder" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkCheckOrderPickListStatus" class="btn-custom" runat="server" NavigateUrl="~/CheckStatus.aspx">        
                <img src="asset/image/check_order_pick_stat.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">        
               <br/>
                <h6 class="btn-custom"><strong>Check Order Picklist Status</strong></h6>    
            </asp:HyperLink>
        </div>

         <div id="divQuantityMove" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkQuantityMove" class="btn-custom" runat="server" NavigateUrl="~/QtyMove.aspx">        
                <img src="asset/image/qty_move.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">        
                <br/>
                <h6 class="btn-custom"><strong>Quantity Move</strong></h6>    
            </asp:HyperLink>
        </div>

        <div class="col-4 text-center"></div><!-- /.col-lg-4 -->
     </div><!-- /.row -->

    <div class="row pt-4">

        <div id="divDsReceive" class="col-4 text-center chover rounded" runat="server">
            <asp:HyperLink ID="LinkDsReceive" class="btn-custom" runat="server" NavigateUrl="~/DeliverySheetReceiving.aspx">        
                <img src="asset/image/delivery_receive.png" class="bd-placeholder-img rounded img-fluid" width="120" height="120" alt="...">        
                <br/><h6 class="btn-custom"><strong>Delivery Sheet Receiving</strong></h6>    
            </asp:HyperLink>
        </div>

        <div class="col-4 text-center"></div><!-- /.col-lg-4 -->
     </div><!-- /.row -->

      

    <hr class="featurette-divider">

  </div><!-- /.container -->

</asp:Content>
