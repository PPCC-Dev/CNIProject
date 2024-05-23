<%@ Page Title="Unposted Job Transaction" Language="vb" AutoEventWireup="false" MasterPageFile="~/SRN.Master" CodeBehind="Unposted.aspx.vb" Inherits="CNIProjet.Unposted" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .txt-margin { margin-bottom:5px;}
        .txt-margin2 { margin-bottom:10px;}
        .width-div { width: 50%;} 
        .txt-center {text-align:center;}
        .display-col { display:none;}
    </style>

    <style type="text/css">
        .btn-custom {
            color: #fff;
            background-color: #575364;
            border-color: #575364; 
        }
        .btn-custom:hover, .btn-custom:focus, .btn-custom:active, .btn-custom.active, .open>.dropdown-toggle.btn-custom {
            color: #fff;
            background-color: #575364;
            border-color: #575364; 
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
      
    </style>

    <script type="text/javascript">

        function scanbarcode() {

            var barcode = document.getElementById('<%=txtbarcode.ClientID%>').value;
            var label = document.getElementById('<%= lblbarcode.ClientID %>').innerHTML;

            if (label.value == 'Barcode Completed: ') {
                document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                document.getElementById("notifyWaiting").innerHTML = "Waiting.....";
                document.getElementById('<%=Button1.ClientID%>').click();
                document.getElementById('<%=Button1.ClientID%>').disabled = true;

            }else {

                document.getElementById('<%=txtbarcode.ClientID%>').readOnly = true;
                document.getElementById("notifyWaiting").innerHTML = "Waiting.....";
                document.getElementById('<%=Button1.ClientID%>').disabled = false;
            }

        }

        $(function () {
            $("#<%=ddlItemScrapped.ClientID%>").change(function () {
                
                 document.getElementById('<%=txtbarcode.ClientID%>').focus();
                
            })

        })

    </script>
  

    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then(function() {
                window.setTimeout(function () { 
                    document.getElementById('<%=txtbarcode.ClientID%>').focus();
                    $("#<%=btnprocess.ClientID%>").prop('disabled', false);
            }, 0);});

        }

       </script>

       <script type="text/javascript">

        function ShowSweetAlertConfirm(type, msg, icon) {
            swal.fire({
                title: type,
                text: msg,
                type: icon,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then((result) => {  
 
                if (result.value) {
                    form.submit();
    	           //code confirm button  
                } else if (result.dismiss === swal.DismissReason.cancel) {    
                    return;  
 	            }
            });                  

        }

       </script> 

    <script type="text/javascript">
        $(function () {
            $('#<%= txtStartTime.ClientID%>').keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });
        })

         $(function () {
            $('#<%= txtEndTime.ClientID%>').keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });
        })

        $(function () {
            $('#<%= txtDTStartTime.ClientID%>').keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });
        })

         $(function () {
            $('#<%= txtDTEndTime.ClientID%>').keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });
        })

        $(function () {
            $('#<%= txttotalhour.ClientID%>').keypress(function (e) {
                if (e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 45) {
                    return false;
                }
            });
        })

        $(function () {
             $('#<%= txttotalhrsDT.ClientID%>').keypress(function (e) {
                if (e.which != 46 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 45) {
                    return false;
                }
             });
        })

        function functionx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57) && evt.charCode != 46) {
                    return false;
            }
        }

        $(document).ready(function () {
            //$("#<%=txtStartTime.ClientID%>").prop('disabled', true);
            //$("#<%=txtEndTime.ClientID%>").prop('disabled', true);
            $("#<%=txttotalhour.ClientID%>").prop('disabled', true);
            $("#<%=txtDownTimeTransDate.ClientID%>").prop('disabled', true);
            $("#<%=txttotalhrsDT.ClientID%>").prop('disabled', true);

            var txtdate = $("#<%=txtdate.ClientID%>").val();
            $("#<%=txtdate.ClientID%>").val(txtdate);

        });

        function validate_require() {
            var result = true;
            var employee = $("#<%=ddlemployee.ClientID%>").val();
            var resource = $("#<%=ddlResource.ClientID%>").val();
            var schedulingshift = $("#<%=ddlSchedulingShift.ClientID%>").val();
            var trantype = $("#<%=ddltrantype.ClientID%>").val();
            var StartTime = $("#<%=txtStartTime.ClientID%>").val();
            var EndTime = $("#<%=txtEndTime.ClientID%>").val();
            var DTStartTime = $("#<%=txtDTStartTime.ClientID%>").val();
            var DTEndTime = $("#<%=txtDTEndTime.ClientID%>").val();
            var ChkQAQC = document.getElementById('<%=ChkQAQC.ClientID%>').checked;
            var HiddenField1 = $("#<%=HiddenField1.ClientID%>").val();

            if (StartTime.length != 5) {
                ShowSweetAlert('Error', '#207 : รูปแบบเวลาไม่ถูกต้อง', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (EndTime.length != 5) {
                ShowSweetAlert('Error', '#207 : รูปแบบเวลาไม่ถูกต้อง', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (DTStartTime.length != 5) {
                ShowSweetAlert('Error', '#207 : รูปแบบเวลาไม่ถูกต้อง', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (DTEndTime.length != 5) {
                ShowSweetAlert('Error', '#207 : รูปแบบเวลาไม่ถูกต้อง', 'error');
                result = false;
                return false;
                //alert(result);
            }
            

            if ((employee == '' || employee == null) && trantype == 'R') {
                ShowSweetAlert('Error', '#220 : กรุณาสแกน หรือเลือก Employee ก่อน', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (resource == '' || resource == null) {
                ShowSweetAlert('Error', '#221 : กรุณาสแกน หรือเลือก Resource ก่อน', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (schedulingshift == '' || schedulingshift == null) {
                ShowSweetAlert('Error', '#222 : กรุณาสแกน หรือเลือก Scheduling Shift ก่อน', 'error');
                result = false;
                return false;
                //alert(result);
            }

            if (ChkQAQC == false && HiddenField1 == '1') {
                ShowSweetAlert('Error', '#233 : กรุณาเลือก QA/QC Pass ก่อน', 'error');
                result = false;
                return false;
                //alert(result);
            }

            //alert(result);
            return result;


        }


        //function openModal() {

        //        $('#AddModel').modal({
        //            backdrop: 'static',
        //            keyboard: true,
        //            show: true
        //        });
        //};

    </script>

    <script type="text/javascript">
        window.onbeforeunload = function () {
            $("#<%=btnprocess.ClientID%>").prop('disabled', true);
        };
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
                <div class="row">
                    <div class="col-md-9">
                        <div style="margin-top:20px;display:none;">
                            <asp:Button ID="Button1" runat="server" Text="Scan" UseSubmitBehavior="true" />
                                <asp:HiddenField ID="ActiveTabTextBox" runat="server" Value= "1" />
                       </div>
                    </div>
                     <div class="col-md-3 float-right">
                         <asp:Button ID="btnreset" runat="server" class="btn btn-warning btn-sm float-right mx-2 mb-0" Style="width:95px;"  Text="Reset" UseSubmitBehavior="false" />
                         <asp:Button ID="btnprocess" runat="server" class="btn btn-success btn-sm float-right mx-2 mb-0" Style="width:95px;" Text="Process" OnClientClick="return validate_require();" />                                 
                     </div>
                    
                </div>
                <div class="row mt-1">
                    <div class="col-md-9"></div>
                    <div class="col-md-3 float-right">
                         <asp:Button ID="btnPrintPDTag" runat="server" class="btn btn-customblue btn-sm float-right mx-2 mb-0" Style="width:95px;"  Text="PD&DL Tag" UseSubmitBehavior="false" />
                         <asp:Button ID="btnPrintWIPTag" runat="server" class="btn btn-customblue btn-sm float-right mx-2 mb-0" Style="width:95px;" Text="WIP Tag" UseSubmitBehavior="false" />                                 
                     </div>
                </div>

                    <table  width="100%">
                       <tr>
                           <div class="row align-items-center">
                                <div class="col-sm-3 text-right width-div"></div>
                                <div class="col-sm-6 width-div">
                                    <p id="notifyWaiting"></p>
                                </div>
                            </div>
                       </tr>                      
                        <tr>
                            <td>
                                <div class="row align-items-right">
                                <div class="col-sm-3 text-right width-div">
                                    <asp:Label ID="lblbarcode" runat="server" Text="Barcode Job: "></asp:Label>
                                </div>
                                <div class="col-sm-5 width-div">
                                        <asp:TextBox ID="txtbarcode" runat="server" AutoComplete="off" class="form-control form-control-sm txt-margin"></asp:TextBox>
                                    
                                    </div>
                            
                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="row align-items-right">
                                <div class="col-sm-3 text-right width-div">
                                </div>
                                <div class="col-sm-2 text-left width-div" style="display:none;">
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkcancel" class="form-check-input txt-margin" runat="server" AutoPostBack="true" /><span>Cancel Tag</span>
                                </div>

                                </div>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <div class="row align-items-right">
                                <div class="col-sm-2 text-left width-div">
                                    <span>User Scan Tag : </span>
                                </div>
                                <div class="col-sm-10 text-left width-div">
                                    <asp:Label ID="lblqtyscan" runat="server" Text="0"></asp:Label> <asp:Label ID="lblum" runat="server"></asp:Label>
                                </div>
                                </div>
                            </td>
                        </tr>--%>
                        </table>

                    <%--<div class="row align-items-right txt-margin2"></div>
                    <div class="card" style="display:none">
                                <div class="row align-items-center">
                                    <div class="col-sm-12 text-left">
                                    <div class="card-body">
                                        <legend>Information</legend>
                                        <div class="row">
                                            <div class="col-sm-1 text-right width-div">
                                                <span>Job : </span>
                                            </div>
                                            <div class="col-sm-3 text-left width-div">
                                                <asp:Label ID="lbljob" runat="server" ></asp:Label> <asp:Label ID="Label1" runat="server" Text="-" Visible="False"></asp:Label> 
                                                <asp:Label ID="lblSuffix" runat="server" ></asp:Label> <asp:Label ID="Label2" runat="server" Text="-" Visible="False"></asp:Label>
                                                <asp:Label ID="lbloper" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-sm-2 text-right width-div">
                                                <span>Qty Release : </span>
                                            </div>
                                            <div class="col-sm-2 text-left width-div">
                                                <asp:Label ID="lblQtyRelease" runat="server" ></asp:Label>
                                            </div>
                                            <div class="col-sm-2 text-right width-div">
                                                <span>Qty Complete : </span>
                                            </div>
                                            <div class="col-sm-2 text-left width-div">
                                                <asp:Label ID="lblQtyComplete" runat="server" ></asp:Label>
                                            </div>
                                            
                                        </div>
                                        
                                    </div>
                                    </div>
                                </div>
                    </div>--%>
                    

                    <%----------NAV---------%>
                    <div class="row align-items-right txt-margin2"></div>

                    <div class="row align-items-center">
                        <div class="col-sm-12 text-left">
                        <ul class="nav nav-pills" id="myTab">
                            <li id="l_s1" class='nav-item active'>
                                <!-- <a href="#s1" class='nav-link active' data-toggle="tab" id="1">Main</a> -->
                                <a href="#s1" id="tab1" runat="server" style="font-size: 10px;" class="btn btn-custom ml-1" data-toggle="tab">
                                  Main
                                </a>
                                
                                <%--<asp:LinkButton ID="LinkButton1"  data-id="1" runat="server" class='nav-link active' data-toggle='tab' href='#s1'>Main</asp:LinkButton>--%>

                            </li>

                            <li id="l_s2" class='nav-item'>
                                <%--<a href="#s2" style="font-size: 8.5px;" class='nav-link' data-toggle="tab" id="2">Completed</a>--%>
                                <a href="#s2" id="tab2" runat="server" style="font-size: 10px;" class="btn btn-custom ml-1" data-toggle="tab">
                                  Completed
                                </a>
                                <%--<asp:LinkButton ID="LinkButton2"  data-id="2" runat="server" class='nav-link' data-toggle='tab' href='#s2'>Completed</asp:LinkButton>--%>

                            </li>
                            <li id="l_s3" class='nav-item'>
                                <%--<a href="#s3" style="font-size: 8.5px;" class='nav-link' data-toggle="tab" id="3">Scrapped</a>--%>
                                <a href="#s3" id="tab3" runat="server" style="font-size: 10px;" class="btn btn-custom ml-1" data-toggle="tab">
                                  Scrapped
                                </a>
                                <%--<asp:LinkButton ID="LinkButton3" data-id="3" runat="server" class='nav-link' data-toggle='tab' href='#s3'>Scrapped</asp:LinkButton>--%>
     
                            </li>
                            <li id="l_s4" class='nav-item'>
                                <%--<a href="#s4" style="font-size: 8.5px;" class='nav-link' data-toggle="tab" id="4">Backflush</a>--%>
                                <a href="#s4" id="tab4" runat="server" style="font-size: 10px;" class="btn btn-custom ml-1" data-toggle="tab">
                                  Backflush
                                </a>
                                <%--<asp:LinkButton ID="LinkButton4" data-id="4" runat="server" class='nav-link' data-toggle='tab' href='#s4'>Backflush</asp:LinkButton>--%>
                  
                            </li>
                            <li id="l_s5" class='nav-item'>
                                <%--<a href="#s5" style="font-size: 8.5px;" class='nav-link' data-toggle="tab" id="5">Downtime</a>--%>
                                <a href="#s5" id="tab5" runat="server" style="font-size: 10px;" class="btn btn-custom ml-1" data-toggle="tab">
                                  Downtime
                                </a>
                                <%--<asp:LinkButton ID="LinkButton5" data-id="5" runat="server" class='nav-link' data-toggle='tab' href='#s5'>Downtime</asp:LinkButton>--%>
                       
                            </li>
                            
                        </ul>

                        <!-- Tab panes -->
                        <div class="row align-items-right txt-margin2"></div>
                        
                        <div class="tab-content">

                            <div id="s1" class="tab-pane fade in active" >
                                <div class="card">
                                <div class="row align-items-center">
                                    <div class="col-sm-12 text-left">
                                        <div class="card-body">
                                            <legend>Input</legend>
                                            <div class="row">
                                                <div class="col-md-6 float-left width-div">
                                                    <asp:LinkButton runat="server" ID="btnPrevious" class="btn btn-link btn-sm float-left mx-2 mb-0" AutoPostBack="true" UseSubmitBehavior="false" >
                                                        <i class="fas fa-arrow-circle-left" aria-hidden="true"></i> <strong>Previous</strong>
                                                    </asp:LinkButton>
                                                </div>
                                                 <div class="col-md-6 float-right width-div">

                                                     <asp:LinkButton runat="server" ID="btnNext" class="btn btn-link btn-sm float-right mx-2 mb-0" AutoPostBack="true" UseSubmitBehavior="false" >
                                                        <strong>Next</strong> <i class="fas fa-arrow-circle-right" aria-hidden="true"></i>
                                                    </asp:LinkButton>
                                                     
                                                 </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Job : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtjob" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off" ReadOnly="True"></asp:TextBox>
                                                    <asp:Label ID="lbljob" runat="server"  Visible="False"></asp:Label>
                                                    <asp:Label ID="lblSuffix" runat="server"  Visible="False"></asp:Label>
                                                    <asp:Label ID="lbloper" runat="server"  Visible="False"></asp:Label>
                                                </div>
                                                
                                            </div>
                                            <%--<div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span></span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:CheckBox ID="ChkQAQC" runat="server" class="form-check-input txt-margin" Enabled="false" /> <spen>QA/QC Pass</spen>
                                                </div>
                                            </div>--%>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Transaction Type : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddltrantype" runat="server" class="form-control form-control-sm txt-margin" disabled = "disabled" EnableViewState="true" >
                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Setup" Value="S"></asp:ListItem>
                                                        <asp:ListItem Text="Run" Value="R"></asp:ListItem>
                                                        <asp:ListItem Text="Move" Value="M"></asp:ListItem>
                                                        <asp:ListItem Text="Indirect" Value="I"></asp:ListItem>
                                                        <asp:ListItem Text="Machine" Value="C"></asp:ListItem>
                                                        <asp:ListItem Text="Queue" Value="Q"></asp:ListItem>
                                                        <asp:ListItem Text="Direct" Value="D"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Trans Date : </span>
                                                </div>
                                                <div class="col-sm-2 width-div">
                                                    <asp:TextBox ID="txtdate" runat="server" class="form-control form-control-sm txt-margin datepicker" AutoComplete="off" AutoPostBack="true"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Employee : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlemployee" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                            <div class="col-sm-5 text-right width-div">
                                                <span>WC : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtwc" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Resource : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlResource" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Production Line : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtProdLine" runat="server" class="form-control form-control-sm txt-margin" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                            
                                            <%--<div class="row" style="display:none;">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Production Shift : </span>
                                                </div>
                                                <div class="col-sm-2 text-right width-div">
                                                    <asp:RadioButtonList ID="rdDayNight" runat="server" RepeatColumns="2" CellPadding="8">
                                                        <asp:ListItem Value="D"><sped>&nbsp;Day</sped></asp:ListItem>
                                                        <asp:ListItem Value="N"><sped>&nbsp;Night</sped></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                            </div>--%>
                                            
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Start Time : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtStartTime" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off" MaxLength="5"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>End Time : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtEndTime" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off" MaxLength="5"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Total Hour : </span>
                                                </div>
                                                <div class="col-sm-2 text-right width-div">
                                                    <asp:TextBox ID="txttotalhour" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off">0</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Scheduling Shift : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlSchedulingShift" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" ></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                <span>Move To Location : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlmovetoloc" runat="server" class="form-control form-control-sm txt-margin" disabled = "true" EnableViewState="true" ></asp:DropDownList>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>

                            <div id="s2" class="tab-pane fade" >
                                    
                                <div class="card">
                                <div class="row align-items-center">
                                    <div class="col-sm-12 text-left">
                                        <div class="card-body">
                                        <%--<h5 class="card-title txt-center">Input</h5>--%>
                                            <legend>Complete</legend>

                                            <div class="row">
                                             <div id="divcomplete" class="col-sm-12 mt-2">
                                             <asp:ListView id="PanelList1" runat="server">
                                                 <ItemTemplate>        
                                                          <div class="mt-2">
                                                      <%--<div class="card mt-2">--%>                                 
                                                             <div class="card-body">
                                                             <%--<h6 class="card-title text-primary">
                                                                 <%#Eval("Item")%></br>                             
                                                             </h6>
                                                             <div class="row">                        
                                                                <div class="col-sm-6">
                                                                    <span style="font-size:13px;">Operation : <%#Eval("OperNum")%></span><br />
                                                                </div>  
                                                                <div class="col-sm-6"></div>
                                                            </div>--%>
                                                            
                                                            <div class="row"> 
                                                                <div class="form-group col-sm-4 row">
                                                                    <div class="col-sm-4 text-right width-div">
                                                                        <span>Item : </span>
                                                                    </div> 
                                                                    <div class="col-sm-6 text-left width-div">
                                                                        <span><%#Eval("Item")%></span>
                                                                        <asp:Label ID="lblItemJobRowPointer" runat="server" Text='<%# Eval("RowPointer") %>' Style="display:none;"></asp:Label>
                                                                    </div> 
                                                                </div>
                                                                 
                                                                <div class="form-group col-sm-4 row">
                                                                    <div class="col-sm-6 text-right width-div">
                                                                        <span>Qty Complete : </span>
                                                                    </div>                                                                 
                                                                <div class="col-sm-6 text-left width-div">
                                                                    <asp:TextBox ID="txtQtyComplete" runat="server" Text='<%# Eval("QtyComplete") %>' onkeypress="return functionx(event)" 
                                                                        class="form-control form-control-sm txt-margin" AutoComplete="off" AutoPostBack="true"
                                                                        OnTextChanged="txtQtyComplete_TextChanged"></asp:TextBox></span>
                                                                </div>
                                                                    
                                                                </div>
                                                            </div>
                                                            <%--<div class="row">                        
                                                                <div class="col-sm-6">
                                                                    <span style="font-size:13px;">Sum Qty : <asp:Label ID="lblitemComplete" runat="server" Text='<%# Eval("QtyComplete") %>'></asp:Label></span>
                                                                </div>  
                                                                <div class="col-sm-6">
                                                                    <span style="font-size:13px;">Qty Remain : <asp:Label ID="lblitemQtyRemain" runat="server" Text='<%# Eval("DerQtyRemain") %>'></asp:Label></span>
                                                                </div>
                                                            </div>--%>
                                                             <%--<span style="font-size:13px;">Lot : <asp:DropDownList ID="ddlLot" runat="server" class="form-control form-control-sm mh-100 txt-margin" style="width:255px; font-size: 13px;"></asp:DropDownList></span>--%>
                                                         </div>
                                                         </div>
                                                 </ItemTemplate>
                                             </asp:ListView>

                                                     <div class="card-body">
                                                         <div class="row"> 
                                                            <div class="form-group col-sm-4 row">
                                                                <div class="col-sm-4 text-right width-div">
                                                                    <span></span>
                                                                </div> 
                                                                <div class="col-sm-8 text-left width-div">
                                                                    <asp:CheckBox ID="ChkQAQC" runat="server" class="form-check-input txt-margin" Enabled="false" /> <spen>QA/QC Pass</spen>
                                                                </div> 
                                                            </div>
                                                        </div>

                                                         <div class="row"> 
                                                            <div class="form-group col-sm-4 row">
                                                                <div class="col-sm-4 text-right width-div">
                                                                    <span>Operator : </span>
                                                                </div> 
                                                                <div class="col-sm-8 text-left width-div">
                                                                    <asp:TextBox ID="txtOperator" runat="server" class="form-control form-control-sm txt-margin" TextMode="MultiLine" rows="2" AutoPostBack="true"></asp:TextBox>
                                                                </div> 
                                                            </div>

                                                        </div>

                                                     </div>

                                             </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                            <div id="s3" class="tab-pane fade" >

                                <div class="card">
                                <div class="row align-items-center">
                                    <div class="col-sm-12 text-left">
                                        <div class="card-body">
                                              <legend>Scrapped</legend>  
                                            <div class="row">                                                
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Scrap Code : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlscrapcode" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true"  ></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">                                                
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Item : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddlItemScrapped" runat="server" class="form-control form-control-sm txt-margin" ></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="row">                                                
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Qty Scrap : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtQtyScrap" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off">0</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row"> 
                                                <div class="col-sm-5 text-left width-div">
                                                    
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:Button ID="btnAddScrap" runat="server" class="btn btn-customblue btn-sm"  Text="Add"  UseSubmitBehavior="false" />
                                                
                                                </div>

                                            </div>
                                             <div class="row"> 
                                                <div class="col-sm-5 text-left width-div">
                                                    <span>Total Scrap : </span> <asp:Label ID="lbltotalscrap" runat="server" Text="0"></asp:Label>
                                                </div>
                                                <%--<div class="col-sm-6 text-left width-div">

                                                
                                                </div>--%>

                                            </div>

                                            <div class="row">
                                            <div id="divscrap" class="col-sm-12 mt-2">
                                             <asp:ListView id="PanelList2" runat="server">
                                                 <ItemTemplate>                                                     
                                                         <div class="card mt-2">                                     
                                                             <div class="card-body">
                                                                <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-4 text-right width-div">
                                                                            <span style="font-size:13px;">Item : </span>
                                                                        </div> 
                                                                        <div class="col-sm-8 text-left width-div">
                                                                            <span style="font-size:13px;"><%#Eval("Item")%></span>
                                                                        </div> 
                                                                    </div>

                                                                </div> 
                                                                <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-4 text-right width-div">
                                                                            <span style="font-size:13px;">Scrap Code : </span>
                                                                        </div> 
                                                                        <div class="col-sm-8 text-left width-div">
                                                                            <span style="font-size:13px;"><%#Eval("ReasonCode")%>&nbsp;&nbsp;<%#Eval("DerDescription")%></span>
                                                                        </div> 
                                                                    </div>

                                                                </div> 
                                                                <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-4 text-right width-div">
                                                                            <span style="font-size:13px;">Scrap Qty : </span>
                                                                        </div> 
                                                                        <div class="col-sm-8 text-left width-div">
                                                                            <span style="font-size:13px;"><asp:Label ID="lblQty" runat="server" Font-Bold="False" Text='<%#Eval("Qty")%>'></asp:Label></span>
                                                                            
                                                                        </div> 
                                                                    </div>

                                                                </div>
                                                                <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-4 text-right width-div">
                                                                            <span style="font-size:13px;"class="text-danger">
                                                                                <asp:LinkButton ID="lnkDeleteScrap" class="text-danger" runat="server" Font-Bold="true" 
                                                                                    CommandName="DeleteScrap" CommandArgument='<%# Eval("RowPointer")%>'
                                                                                    ToolTip="Click to Delete" Text="Delete"></asp:LinkButton>
                                                                            </span>
                                                                        </div> 
                                                                         
                                                                    </div>

                                                                </div>
                                                         </div>
                                                         </div>
                                                     
                                                 </ItemTemplate>
                                             </asp:ListView>
                                            </div>
                                            </div>

                                            <%--<table style="width:100%">
                                            <tr>
                                                <td>
                                                    
                                                       <asp:GridView ID="GridView5" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-bordered table-hover">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Item" DataField="Item" ReadOnly="true"  />
                                                            <asp:BoundField HeaderText="Scrap Code" DataField="ReasonCode" ReadOnly="true"  />
                                                            <asp:BoundField HeaderText="Description" DataField="DerDescription" ReadOnly="true" />
                                                            <asp:TemplateField HeaderText="Scrap Qty">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                                                </ItemTemplate>
                                                           </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            </table>--%>
         
                                        </div>
                                    </div>
                                </div>
                                </div>

                            </div>
                            <div id="s4" class="tab-pane fade">

                                <div class="card">
                                <div class="row align-items-center">
                                    <div class="col-sm-12 text-left">
                                        <div class="card-body">
                                              <legend>Backflush</legend>  

                                            <div class="row">
                                             <%--<asp:Button ID="Button3" runat="server" Style="display:none" UseSubmitBehavior="false" />--%>
                                            <div id="divbf" class="col-sm-12 mt-2">                                                                                                                               
                                             <asp:ListView id="PanelList4" runat="server" >
                                                 <ItemTemplate>
                                                        <div class="card mt-2">
                                                              <div class="card-body">
                                                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="font-size:13px;"><asp:CheckBox ID="chkSelect" runat="server" CssClass="form-check-input position-static" Checked= '<%# Eval("Selected") %>' AutoPostBack="true" OnCheckedChanged="SelectCheckBox_CheckedChanged" />&nbsp;&nbsp;Operation : <asp:Label ID="lblOperNum" runat="server" Font-Bold="False" Text='<%#Eval("OperNum")%>'></asp:Label> | Seq : <asp:Label ID="lblSeq" runat="server" Font-Bold="False" Text='<%#Eval("Seq")%>'></asp:Label></span></br>
                                                             <span style="font-size:13px;">Lot : <asp:Label ID="lblLot" runat="server" Font-Bold="False" Text='<%#Eval("Lot")%>'></asp:Label></span></br>
                                                             <span style="font-size:13px;">Quantity : <asp:TextBox ID="txtqtyReq" ReadOnly="false" AutoComplete="off" runat="server" AutoPostBack="true" OnTextChanged="txtqtyReq_TextChanged" Text='<%# Eval("Qty") %>' CssClass="form-control form-control-sm mh-100 numeric" style="width:255px; text-align:right; font-size:13px;" onkeypress="return functionx(event)"></asp:TextBox></span>
                                                             <span style="font-size:13px;">UM : <%#Eval("UM")%> | Item : <asp:Label ID="lblItem" runat="server" Font-Bold="False" Text='<%#Eval("Item")%>'></asp:Label></span></br>
                                                             <span style="font-size:13px;">Description : <%#Eval("ItemDesc")%></span></br>
                                                             <span style="font-size:13px;">On Hand : <asp:Label ID="lblQtyOnHand" runat="server" Font-Bold="False" Text='<%#Eval("QtyOnHand")%>'></asp:Label> | Quantity : <asp:Label ID="lblQtyNeeded" runat="server" Font-Bold="False" Text='<%#Eval("QtyNeeded")%>'></asp:Label></span></br>
                                                             <span style="font-size:13px; display:none;">
                                                                 <asp:Label ID="lblTransNum" runat="server" Font-Bold="False" Text='<%#Eval("TransNum")%>'></asp:Label>
                                                                 <asp:Label ID="lblTransSeq" runat="server" Font-Bold="False" Text='<%#Eval("TransSeq")%>'></asp:Label>
                                                                 <asp:Label ID="lblEmpNum" runat="server" Font-Bold="False" Text='<%#Eval("EmpNum")%>'></asp:Label>
                                                                 <asp:Label ID="lblLoc" runat="server" Font-Bold="False" Text='<%#Eval("Loc")%>'></asp:Label>
                                                                 <asp:Label ID="lblWhse" runat="server" Font-Bold="False" Text='<%#Eval("Whse")%>'></asp:Label>
                                                                 <asp:Label ID="lblRowPointer" runat="server" Font-Bold="False" Text='<%#Eval("RowPointer")%>'></asp:Label>
                                                             </span>
                                                              </div>
                                                          </div>                                                             
                                                         
                                                 </ItemTemplate>
                                             </asp:ListView>

                                            </div>

                                            </div>

                                            <%--<table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button3" runat="server" Style="display:none" UseSubmitBehavior="false" />
                                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false" 
                                                            CssClass="table table-striped table-bordered table-hover" 
                                                            ShowFooter="false" ShowHeader="true" >
                                                        <Columns>                         
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">  
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>  
                                                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-check-input position-static" Checked= '<%# Eval("Selected") %>' AutoPostBack="true" OnCheckedChanged="SelectCheckBox_CheckedChanged" />  
                                                            </ItemTemplate>  
                                                        </asp:TemplateField>
                        
                                                        <asp:BoundField HeaderText="Operation" DataField="OperNum" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Seq" DataField="Seq" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Lot" DataField="Lot" ReadOnly="true" />

                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtqtyReq" ReadOnly="false" runat="server" Font-Size="10px"
                                                                        AutoPostBack="true" Text='<%# Eval("Qty") %>' CssClass="form-control numeric" 
                                                                        style="width:100px; text-align:right"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField HeaderText="UM" DataField="UM" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Item" DataField="Item" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Description" DataField="ItemDesc" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="On Hand" DataField="QtyOnHand" ReadOnly="true">
                                                           <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Quantity" DataField="QtyNeeded" ReadOnly="true">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="" DataField="TransNum" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />
                                                        <asp:BoundField HeaderText="" DataField="TransSeq" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />
                                                        <asp:BoundField HeaderText="" DataField="EmpNum" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />
                                                        <asp:BoundField HeaderText="" DataField="Loc" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />
                                                        <asp:BoundField HeaderText="" DataField="Whse" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />
                                                        <asp:BoundField HeaderText="" DataField="RowPointer" ReadOnly="true" ItemStyle-CssClass="display-col" HeaderStyle-CssClass="display-col" />

                                                        </Columns>
                                                    </asp:GridView>

                                                </td>
                                            </tr>
                                        </table>--%>
                                            <hr />
                                            <div class="row">
                                            <div id ="divbfsum" class="col-sm-12 mt-2">
                                             <asp:ListView id="PanelList5" runat="server">
                                                 <ItemTemplate>
                                                         <div class="card mt-2">                                     
                                                         <div class="card-body">                                    
                                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="font-size:13px;"><asp:CheckBox ID="chkMatchSelect" Enabled="false" runat="server" CssClass="form-check-input position-static" Checked= '<%# Eval("Matched") %>' />&nbsp;&nbsp;Operation : <%#Eval("OperNum")%> | Seq : <%#Eval("Seq")%></span></br>
                                                          <span style="font-size:13px;">Target Qty : <asp:Label ID="lblTargetQty" runat="server" Font-Bold="False" Text='<%#Eval("TargetQty")%>'></asp:Label> | Selected Qty : <asp:Label ID="lblSelectedQty" runat="server" Font-Bold="False" Text='<%#Eval("SelectedQty")%>'></asp:Label></span>   
                                                         </div>
                                                         </div>
                                                     
                                                 </ItemTemplate>
                                             </asp:ListView>
                                            </div>
                                            </div>

                                            <%--<table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="false" 
                                                            CssClass="table table-striped table-bordered table-hover" 
                                                            ShowFooter="false" ShowHeader="true" >
                                                        <Columns>                         
                                                        <asp:TemplateField HeaderText="Matched" ItemStyle-HorizontalAlign="Center">  
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>  
                                                                <asp:CheckBox ID="chkMatchSelect" Enabled="false" runat="server" CssClass="form-check-input position-static" Checked= '<%# Eval("Matched") %>' />  
                                                            </ItemTemplate>  
                                                        </asp:TemplateField>

                                                        <asp:BoundField HeaderText="Operation" DataField="OperNum" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Seq" DataField="Seq" ReadOnly="true" />
                                                        <asp:BoundField HeaderText="Target Qty" DataField="TargetQty" ReadOnly="true">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Selected Qty" DataField="SelectedQty" ReadOnly="true">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>

                                                        </Columns>
                                                    </asp:GridView>

                                                </td>
                                            </tr>
                                        </table>--%>
                                        </div>
                                    </div>
                                </div>
                                </div>

                            </div>
                            <div id="s5" class="tab-pane fade">

                                <div class="card">
                                <div class="row align-items-center">                                    
                                    <div class="col-sm-12 text-left">
                                        <div class="card-body">
                                              <legend>Downtime</legend>
                                                <%--<div class="row">
                                                    <div class="col-md-6 float-left width-div">
                                                        <asp:LinkButton runat="server" ID="btnpreviousdt" class="btn btn-link btn-sm float-left mx-2 mb-0" AutoPostBack="true" UseSubmitBehavior="false" >
                                                            <i class="fas fa-arrow-circle-left" aria-hidden="true"></i> <strong>Previous</strong>
                                                        </asp:LinkButton>
                                                    </div>
                                                     <div class="col-md-6 float-right width-div">

                                                         <asp:LinkButton runat="server" ID="btnnextdt" class="btn btn-link btn-sm float-right mx-2 mb-0" AutoPostBack="true" UseSubmitBehavior="false" >
                                                            <strong>Next</strong> <i class="fas fa-arrow-circle-right" aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                     
                                                     </div>
                                                </div>--%>
                                            <div class="row">
                                                <asp:Button ID="Button2" runat="server" Style="display:none" UseSubmitBehavior="false" />
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Trans Date : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtDownTimeTransDate" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off"></asp:TextBox>
                                                </div>

                                                
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Start Time : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtDTStartTime" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" AutoComplete="off" MaxLength="5">00:00</asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>End Time : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtDTEndTime" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" AutoComplete="off" MaxLength="5">00:00</asp:TextBox>
                                                </div>
                                                
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Total Hours : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txttotalhrsDT" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" AutoComplete="off" >0</asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Downtime Code : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:DropDownList ID="ddldowntime" runat="server" class="form-control form-control-sm txt-margin" AutoPostBack="true" ></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-5 text-right width-div">
                                                    <span>Cause By : </span>
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:TextBox ID="txtCauseBy" runat="server" class="form-control form-control-sm txt-margin" AutoComplete="off" ></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row"> 
                                                <div class="col-sm-5 text-left width-div">
                                                    
                                                </div>
                                                <div class="col-sm-2 text-left width-div">
                                                    <asp:Button ID="btnAddDowntime" runat="server" class="btn btn-customblue btn-sm"  Text="Add"  UseSubmitBehavior="false" />                                 
                                                </div>

                                            </div>

                                            <div class="row">
                                             <div id="divdowntime" class="col-sm-12 mt-2">
                                             <asp:ListView id="PanelList3" runat="server">
                                                 <ItemTemplate>                                                    
                                                         <div class="card mt-2">                                     
                                                             <div class="card-body">

                                                                 <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-6 text-right width-div">
                                                                            <span style="font-size:13px;">Downtime Code : </span>
                                                                        </div> 
                                                                        <div class="col-sm-6 text-left width-div">
                                                                            <span style="font-size:13px;"><%#Eval("ReasonCode")%>&nbsp;&nbsp;<%#Eval("DerDescription")%></span>
                                                                        </div> 
                                                                    </div>

                                                                </div>
                                                                 <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-6 text-right width-div">
                                                                            <span style="font-size:13px;">Start Time : </span>
                                                                        </div> 
                                                                        <div class="col-sm-6 text-left width-div">
                                                                            <span style="font-size:13px;"><%#Eval("StartTime")%></span>
                                                                        </div> 
                                                                    </div>
                                                                 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-6 text-right width-div">
                                                                            <span style="font-size:13px;">End Time : </span>
                                                                        </div>                                                                 
                                                                        <div class="col-sm-6 text-left width-div">
                                                                            <span style="font-size:13px;"><%#Eval("EndTime")%></span>
                                                                        </div>
                                                                    
                                                                    </div>
                                                                </div>
                                                                 <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-6 text-right width-div">
                                                                            <span style="font-size:13px;">Total Hours : </span>
                                                                        </div> 
                                                                        <div class="col-sm-6 text-left width-div">
                                                                            <span style="font-size:13px;"><asp:Label ID="lblAhrs" runat="server" Font-Bold="False" Text='<%#Eval("AHrs")%>'></asp:Label></span>
                                                                        </div> 
                                                                    </div>

                                                                </div>

                                                                 <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-6 text-right width-div">
                                                                            <span style="font-size:13px;">Cause By : </span>
                                                                        </div> 
                                                                        <div class="col-sm-6 text-left width-div">
                                                                            <span style="font-size:13px;"><%# Eval("CauseBy") %></span>
                                                                        </div> 
                                                                    </div>

                                                                </div>

                                                                 <div class="row"> 
                                                                    <div class="form-group col-sm-4 row mb-1">
                                                                        <div class="col-sm-4 text-right width-div">
                                                                            <span style="font-size:13px;"class="text-danger">
                                                                                <asp:LinkButton ID="lnkDeleteDowntime" class="text-danger" runat="server" Font-Bold="true"
                                                                                                CommandName="DeleteDowntime" CommandArgument='<%# Eval("RowPointer")%>'
                                                                                                ToolTip="Click to Delete" Text="Delete"></asp:LinkButton>

                                                                            </span>
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
                                    </div>
                                </div>
                                </div>

                            </div>
                            
                        </div>
                        </div>
                    </div>
                    <%----------END NAV---------%>
            </div>

                    <script type="text/javascript">
                        $(function () {

                            HideDiv();
                                   
                        });
                        //On UpdatePanel Refresh
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        if (prm != null) {
                            prm.add_endRequest(function (sender, e) {
                                if (sender._postBackSettings.panelsToUpdate != null) {
                                    HideDiv();
                                }
                            });
                        };
                        function HideDiv() {

                            var totalscrap = $("#<%=lbltotalscrap.ClientID%>").text();            

                            if (totalscrap == '') {
                                    $("#<%=lbltotalscrap.ClientID%>").text('0');
                            }

                            var QtyCompleted = $("#<%=lblQtyCompleted.ClientID%>").text()

                            if ((QtyCompleted == '' || QtyCompleted == 0) && (totalscrap == '' || totalscrap == 0)) {

                                $('#divbf').css('display', 'none');
                                $('#divbfsum').css('display', 'none');
                                $('#divscrap').css('display', 'none');
            
                            } else {

                                $('#divbf').css('display', 'block');
                                $('#divbfsum').css('display', 'block');
                                $('#divscrap').css('display', 'block');

                            }

                        };
                    </script>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtBarcode" EventName="TextChanged" />
                <asp:PostBackTrigger ControlID="Button1" />
                <asp:PostBackTrigger ControlID="Button2" />
                <asp:PostBackTrigger ControlID="btnprocess" />
                <asp:PostBackTrigger ControlID="btnreset" />
                <asp:PostBackTrigger ControlID="btnPrevious" />
                <asp:PostBackTrigger ControlID="btnNext" />
                <%--<asp:PostBackTrigger ControlID="btnpreviousdt" />
                <asp:PostBackTrigger ControlID="btnnextdt" />--%>
                <asp:AsyncPostBackTrigger ControlID="ddltrantype" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlmovetoloc" EventName="SelectedIndexChanged" />
                <asp:PostBackTrigger ControlID="ddlemployee" />
                <asp:PostBackTrigger ControlID="ddlResource" />
                <asp:PostBackTrigger ControlID="ddlSchedulingShift" />
                <asp:PostBackTrigger ControlID="txtDTStartTime" />
                <asp:PostBackTrigger ControlID="txtDTEndTime" />
                <asp:AsyncPostBackTrigger ControlID="txttotalhrsDT" EventName="TextChanged" />
                <asp:PostBackTrigger ControlID="ddldowntime" />
                <asp:PostBackTrigger ControlID="PanelList3" />
                <asp:PostBackTrigger ControlID="PanelList4" />
                <asp:PostBackTrigger ControlID="ddlscrapcode" />
                <asp:PostBackTrigger ControlID="chkcancel" />
                <asp:AsyncPostBackTrigger ControlID="ddlItemScrapped" EventName="SelectedIndexChanged" />
                <asp:PostBackTrigger ControlID="btnAddScrap" />
                <asp:PostBackTrigger ControlID="btnAddDowntime" />
                <asp:PostBackTrigger ControlID="txtdate" />
                <asp:PostBackTrigger ControlID="btnPrintPDTag" />
                <asp:PostBackTrigger ControlID="btnPrintWIPTag" />
                <asp:PostBackTrigger ControlID="PanelList1" />
                <asp:PostBackTrigger ControlID="txtOperator" />
            </Triggers>
            </asp:UpdatePanel>

            <%--<div class="container">
                    <div class="modal fade" id="AddModel" role="dialog" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">
    
                            <div class="modal-content">
                            <div class="modal-header">
                           
                                <h4>&nbsp;Add Quantity</h4> 
                            </div>

                            <div class="modal-body">
                                
                            </div>
                            </div>
      
                        </div>
                    </div>
             </div>--%>
        </div>
    </div>

    <asp:Label ID="Label3" runat="server" Text="1" ForeColor="White"></asp:Label>
    <asp:HiddenField ID="DigitStartTime" runat="server" Value="0" />
    <asp:HiddenField ID="DigitEndTime" runat="server" Value="0" />
    <asp:HiddenField ID="CntrlPoint" runat="server" Value="1" />
    <asp:HiddenField ID="BflushError" runat="server" Value="0" />
    <asp:HiddenField ID="BflushMessage" runat="server" Value="" />
    <asp:Label ID="lblQtyCompleted" runat="server" Text="" ForeColor="White"></asp:Label>
    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
    <asp:HiddenField ID="WipTag" runat="server" Value="0" />
    <asp:HiddenField ID="DigitStartTimeDt" runat="server" Value="0" />
    <asp:HiddenField ID="DigitEndTimeDt" runat="server" Value="0" />

    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>--%>

    <%--<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>--%>
    <script src="asset/Unposted/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>


    <script type="text/javascript">


            $("#l_s1").click(function () {   
                $('#<%= ActiveTabTextBox.ClientID%>').val('1');
                $('#<%= Label3.ClientID%>').val('1');
                $('#<%= Label3.ClientID%>').html('1');

                $('#<%= lblbarcode.ClientID %>').text("Barcode Job: ")                   
                document.getElementById('<%=txtbarcode.ClientID%>').focus(); 


            });

            $("#l_s2").click(function () {
                $('#<%= ActiveTabTextBox.ClientID%>').val('2');
                $('#<%= Label3.ClientID%>').val('2');
                $('#<%= Label3.ClientID%>').html('2');

            var CntrlPoint = document.getElementById('<%=CntrlPoint.ClientID%>').value;    

            //if (CntrlPoint == "1") {
            //    $('#<%= lblbarcode.ClientID %>').text("Barcode Completed: ")
            //}
                $('#<%= lblbarcode.ClientID %>').text("Barcode Operator: ")                
                document.getElementById('<%=txtbarcode.ClientID%>').focus(); 

            });

            $("#l_s3").click(function () {
                $('#<%= ActiveTabTextBox.ClientID%>').val('3');
                $('#<%= Label3.ClientID%>').val('3');
                $('#<%= Label3.ClientID%>').html('3');

                var scrapcode = document.getElementById('<%=ddlscrapcode.ClientID%>').value;                   

                if (scrapcode.length > 0) {
                    $('#<%= lblbarcode.ClientID %>').text("Barcode Scrapped: ")
                } else {
                    $('#<%= lblbarcode.ClientID %>').text("Barcode Scrap Code: ")
                }

                document.getElementById('<%=txtbarcode.ClientID%>').focus(); 


            });

            $("#l_s4").click(function () {
                $('#<%= ActiveTabTextBox.ClientID%>').val('4');
                $('#<%= Label3.ClientID%>').val('4');
                $('#<%= Label3.ClientID%>').html('4');


            });

            $("#l_s5").click(function () {
                $('#<%= ActiveTabTextBox.ClientID%>').val('5');
                $('#<%= Label3.ClientID%>').val('5');
                $('#<%= Label3.ClientID%>').html('5');

                var downtimecode = document.getElementById('<%=ddldowntime.ClientID%>').value;                   

                if (downtimecode.length > 0) {
                    //$('#<%= lblbarcode.ClientID %>').text("Barcode Downtime Start Time: ")
                    $('#<%= lblbarcode.ClientID %>').text("Barcode Downtime Code: ")
                } else {
                    $('#<%= lblbarcode.ClientID %>').text("Barcode Downtime Code: ")
                    
                }
                //var Label = document.getElementById('<%=lblbarcode.ClientID%>').value; 
                //sessionStorage.setItem("LabelScan", Label);
                document.getElementById('<%=Button2.ClientID%>').click();
                document.getElementById('<%=txtbarcode.ClientID%>').focus();
                  

            });

    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            

            if (typeof (Storage) !== "undefined") {

                $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
                    localStorage.setItem('activeTab', $(e.target).attr('href'));
                });

            } else {

                localStorage.setItem('activeTab', '#s1');
            }
            
            var activeTab = localStorage.getItem('activeTab')

            if (activeTab){
                $('#myTab a[href="' + activeTab + '"]').tab('show');
 
            }           

        });

    </script>

    <script type="text/javascript">

        function LocalSet() {

            localStorage.setItem('activeTab', '#s1');

            var activeTab = localStorage.getItem('activeTab')

            //alert(activeTab)

            if (activeTab){
                $('#myTab a[href="' + activeTab + '"]').tab('show');
 
            } 
        }
    </script>

    
    <script type="text/javascript">

       function CalTotalHrs(start_time, end_time, out_text_input) {
            var d1 = new Date("Dec 21 1984 " + start_time);
            var d2 = new Date("Dec 21 1984 " + end_time);
            var tsec = parseFloat('#<%= txttotalhour.ClientID%>') * 60.00 * 60.00;

            var diff_time;

            var diff = d2 - d1;
            diff_time = diff / (60.000 * 60.000 * 1000.000);


           //if ((d2 < d1) && (tsec < 0.000)) {
           if ((d2 < d1)) {
                d2.setDate(d2.getDate() + 1);
                diff = d2 - d1;
                diff_time = diff / (60.000 * 60.000 * 1000.000);
            }

           $(out_text_input).val(diff_time);
        }


       $(document).ready(function () {

           var start_time = $("#<%= txtStartTime.ClientID %>").val();
           var end_time = $("#<%= txtEndTime.ClientID %>").val();

           if (start_time != '00:00' || end_time != '00:00') {
               CalTotalHrs(start_time, end_time, '#<%= txttotalhour.ClientID %>');
           }


       });

        $('#<%= txtStartTime.ClientID%>').change(function () {

            var start_time = $(this).val();
            var end_time = $("#<%= txtEndTime.ClientID %>").val();

            var i;
            i = start_time.indexOf(':');

            if (i < 0) {
                var mid;
                mid = start_time.length / 2;
                start_time = [start_time.slice(0, parseInt(mid)), ":", start_time.slice(parseInt(mid))].join('');
                $(this).val(start_time);
            }

            var diff = new Date("Dec 21 1984") - new Date("Dec 21 1984 " + start_time);
            if (isNaN(diff) || start_time == '') {
                $(this).val('00:00');
                start_time = '00:00';
            }


            CalTotalHrs(start_time, end_time, '#<%= txttotalhour.ClientID %>');
        });

        $('#<%= txtEndTime.ClientID%>').change(function () {
            var start_time = $("#<%= txtStartTime.ClientID %>").val();
            var end_time = $(this).val();

            var i;
            i = end_time.indexOf(':');


            if (i < 0) {
                var mid;
                mid = end_time.length / 2;
                end_time = [end_time.slice(0, parseInt(mid)), ":", end_time.slice(parseInt(mid))].join('');
                $(this).val(end_time);
            }
            var diff = new Date("Dec 21 1984") - new Date("Dec 21 1984 " + end_time);

            if (isNaN(diff) || end_time == '') {
                $(this).val('00:00');
                end_time = '00:00';
            }

            CalTotalHrs(start_time, end_time, '#<%= txttotalhour.ClientID %>');
        });

        $('#<%= txttotalhour.ClientID%>').change(function () {
            if (!$.isNumeric($(this).val()) || $(this).val() == '' || $(this).val() == null) {
                $(this).val('0.000');
            }
        }).blur(function () {
            var start_time = $("#<%= txtStartTime.ClientID %>").val();
            var end_time = $("#<%= txtEndTime.ClientID %>").val();

            var tsec = parseFloat($(this).val()) * 60.00 * 60.00;

            if (start_time == '') {
                start_time = '00:00';
            }

            if (end_time == '') {
                end_time = '00:00';
            }

            var d1 = new Date("Dec 21 1984 " + start_time);
            var d2 = new Date("Dec 21 1984 " + end_time);

            d1.setSeconds(tsec);
            var hh = ('00' + d1.getHours()).slice(-2);
            var mm = ('00' + d1.getMinutes()).slice(-2);

            if (start_time != '00:00' || end_time != '00:00') {

                $("#<%= txtEndTime.ClientID %>").val(hh + ':' + mm);
            }
           

        });



    </script>

    <script type="text/javascript">

        function CalTotalHrsDT(start_time, end_time, out_text_input) {
            var d1 = new Date("Dec 21 1984 " + start_time);
            var d2 = new Date("Dec 21 1984 " + end_time);
            var tsec = parseFloat('#<%= txttotalhrsDT.ClientID%>') * 60.00 * 60.00;

            var diff_time;

            var diff = d2 - d1;
            diff_time = diff / (60.000 * 60.000 * 1000.000);


            //if ((d2 < d1) && (tsec < 0.000)) {
            if ((d2 < d1)) {
                d2.setDate(d2.getDate() + 1);
                diff = d2 - d1;
                diff_time = diff / (60.000 * 60.000 * 1000.000);
            }

            $(out_text_input).val(diff_time);
       }


         $(document).ready(function () {

            var dt_start = $("#<%= txtDTStartTime.ClientID %>").val();
            var dt_end = $("#<%= txtDTEndTime.ClientID %>").val();


            if (dt_start != '00:00' || dt_end != '00:00') {
                CalTotalHrsDT(start_time, end_time, '#<%= txttotalhrsDT.ClientID %>');

                }

         });


        $('#<%= txtDTStartTime.ClientID%>').change(function () {
            var start_time = $(this).val();
            var end_time = $("#<%= txtDTEndTime.ClientID %>").val();

            var i;
            i = start_time.indexOf(':');

            if (i < 0) {
                var mid;
                mid = start_time.length / 2;
                start_time = [start_time.slice(0, parseInt(mid)), ":", start_time.slice(parseInt(mid))].join('');
                $(this).val(start_time);
            }

            var diff = new Date("Dec 21 1984") - new Date("Dec 21 1984 " + start_time);
            if (isNaN(diff) || start_time == '') {
                $(this).val('00:00');
                start_time = '00:00';
            }

            //alert("xxxx")
            CalTotalHrsDT(start_time, end_time, '#<%= txttotalhrsDT.ClientID %>');

        });

         

        $('#<%= txtDTEndTime.ClientID%>').change(function () {

            var start_time = $("#<%= txtDTStartTime.ClientID %>").val();
            var end_time = $(this).val();

            var i;
            i = end_time.indexOf(':');

            if (i < 0) {
                var mid;
                mid = end_time.length / 2;
                end_time = [end_time.slice(0, parseInt(mid)), ":", end_time.slice(parseInt(mid))].join('');
                $(this).val(end_time);
            }
            var diff = new Date("Dec 21 1984") - new Date("Dec 21 1984 " + end_time);

            if (isNaN(diff) || end_time == '') {
                $(this).val('00:00');
                end_time = '00:00';
            }

            CalTotalHrsDT(start_time, end_time, '#<%= txttotalhrsDT.ClientID %>');

        });

        $('#<%= txttotalhrsDT.ClientID%>').change(function () {
            if (!$.isNumeric($(this).val()) || $(this).val() == '' || $(this).val() == null) {
                $(this).val('0.000');
            }
        }).blur(function () {
            var start_time = $("#<%= txtDTStartTime.ClientID %>").val();
            var end_time = $("#<%= txtDTEndTime.ClientID %>").val();

            var tsec = parseFloat($(this).val()) * 60.00 * 60.00;

            if (start_time == '') {
                start_time = '00:00';
            }

            if (end_time == '') {
                end_time = '00:00';
            }

            var d1 = new Date("Dec 21 1984 " + start_time);
            var d2 = new Date("Dec 21 1984 " + end_time);

            d1.setSeconds(tsec);
            var hh = ('00' + d1.getHours()).slice(-2);
            var mm = ('00' + d1.getMinutes()).slice(-2);

            if (start_time != '00:00' || end_time != '00:00') {

                $("#<%= txtDTEndTime.ClientID %>").val(hh + ':' + mm);
            }

            
        });

    </script>
    

    
</asp:Content>
