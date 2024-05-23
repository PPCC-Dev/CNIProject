<%@ Page Title="Sign in" Language="vb" AutoEventWireup="false" CodeBehind="Signin.aspx.vb" Inherits="CNIProjet.Signin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="~/asset/image/logo32x32.png" sizes="32x32" type="image/png" />
    <link rel="icon" href="~/asset/image/logo32x32.png" sizes="16x16" type="image/png" />
    <link href="bootstrap/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <link href="asset/Signin/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" type="text/css" />

    <!-- Favicons -->
    <link rel="apple-touch-icon" href="~/asset/image/icon-srn.png" sizes="180x180" />

    <link rel="manifest" href="asset/Signin/manifest.json" />
    <link rel="mask-icon" href="asset/Signin/safari-pinned-tab.svg" color="#563d7c" />

    <meta name="msapplication-config" content="asset/Signin/browserconfig.xml" />
<meta name="theme-color" content="#563d7c" />


    <script src="asset/Signin/sweetalert2.min.js"></script>
    <link href="asset/Signin/sweetalert2.min.css" rel="stylesheet" type="text/css" />

<style type="text/css">
   .txt-margin { margin-bottom:5px;}  
   .width-div { width: 50%;}
   .img-margin { 
       margin-bottom:10px;
       margin-right:150px;
   }
   .border-login {
       border-color:#575364;
   }
</style>

<script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then(function() {
                window.setTimeout(function () {
                    var scanbarcode = document.getElementById('<%=CheckBox.ClientID%>').checked;

                    if (scanbarcode == true) {
                        document.getElementById('<%=txtBarcode.ClientID%>').focus();
                    }

                    
            }, 0);});

        }

       </script>

<script type="text/javascript">

    function scanbarcode() {

        var barcode = document.getElementById('<%=txtBarcode.ClientID%>').value;

        if (barcode.includes("|") == true) {
            document.getElementById('<%=txtBarcode.ClientID%>').readOnly = true;
            document.getElementById('<%=btnlogin.ClientID%>').click();
            document.getElementById('<%=btnlogin.ClientID%>').disabled = true;

        }else {

            document.getElementById('<%=txtBarcode.ClientID%>').readOnly = true;
            document.getElementById('<%=btnlogin.ClientID%>').disabled = false;
        }

    }

</script>

   

</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <br />

        <%--<asp:Panel ID="NotificationPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </asp:Panel>--%>
        <div class="row align-items-center">
            <div class="col-sm-3 text-right width-div"></div>
            <div class="col-sm-6 width-div">
                <p id="notifyWaiting"></p>
            </div>
        </div>

        <div class="row">

            <div class="col-sm-3"></div>

            <div class="col-sm-6 pt-5">
                <div class="shadow-sm text-center rounded-lg border border-login p-5 bg-white">
                    <span>
                        <img src="asset/image/logo.png" class="img-margin" alt="CNI" height="75px"/>
                        </span>


                    <%--txtBarcode--%>
                    <div class="form-row">
                        <div class="col-8">
                            <div class="input-group mb-3">
                            
                                <div  class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-user"></i></span>
                                </div>
                                <asp:TextBox ID="txtBarcode" runat="server" AutoComplete="off" class="form-control"  placeholder="Barcode"></asp:TextBox>
                                
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group mb-3 ml-4">
                                <asp:CheckBox ID="CheckBox" class="form-check-input txt-margin" runat="server" AutoPostBack="true" Checked="true" /><span>Scan Barcode</span>
                            
                            </div>
                        </div>
                        
                        
                    </div>

                    <%--txtusername--%>
                    <div class="form-row">
                        <div class="col-8">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-user"></i></span>
                                </div>
                                <asp:TextBox ID="txtusername" runat="server" class="form-control" placeholder="Username"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4"></div>
                    </div>

                        

                     <%-- txtpassword--%>
                    <div class="form-row">
                        <div class="col-8">
                            <div class="input-group mb-3">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-lock"></i></span>
                                </div>
                                <asp:TextBox ID="txtpassword" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4"></div>
                    </div>

                    <div class="form-row">
                        <div class="col-8">
                            <div class="input-group mt-0">
                                <asp:DropDownList ID="ddlconfig" runat="server"  class="form-control" ></asp:DropDownList >
                                <%--<asp:Button ID="btnlogin" runat="server" Text="Login" class="btn  btn-secondary btn-block my-4" />--%>
                            </div>
                            
                        </div>
                        <div class="col-4"></div>
                    </div>

                    <div class="form-row">
                        <div class="col-8">
                            <div class="input-group mt-0">
                                <asp:Button ID="btnlogin" runat="server" Text="Login" class="btn  btn-secondary btn-block my-4" />
                            </div>
                            
                        </div>
                        <div class="col-4"></div>
                    </div>
 
                        <%--<div class="input-group mb-4">

                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-user"></i></span>
                            </div>

                            <asp:TextBox ID="txtusername" runat="server" class="form-control" placeholder="Username"></asp:TextBox>
                        </div>

                        
                    <div class="input-group mb-4">

                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-lock"></i></span>
                            </div>

                            <asp:TextBox ID="txtpassword" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>

                    </div>

                    <asp:DropDownList ID="ddlconfig" runat="server" class="form-control"></asp:DropDownList>

                    <asp:Button ID="btnlogin" runat="server" Text="Login" class="btn btn-success btn-block my-4" />--%>

                </div>
		   </div>

             <div class="col-sm-3"></div>

        </div>
        
    </div>
    </form>
</body>
</html>
