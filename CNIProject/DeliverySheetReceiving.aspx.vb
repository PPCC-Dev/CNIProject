Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Drawing
Imports System.IO

Public Class DeliverySheetReceiving
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim dt_match As DataTable
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim LenPointQty As Integer = 0
    Dim LenPointQtyFormat As Integer = 0
    Dim DateNow As String

    Private Shared ParmSite As String
    Private Shared Whse As String

    Private Shared Property PSite() As String
        Get
            Return ParmSite
        End Get
        Set(value As String)
            ParmSite = value
        End Set
    End Property

    Private Shared Property PWhse() As String
        Get
            Return Whse
        End Get
        Set(value As String)
            Whse = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("PSession") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("PSession").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        'If GetDsreceiving() = 0 Then
        '    Dim MsgType = ""
        '    MsgType = "Error [ PPCC ]"
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & "กรุณาเลือก Use Web Delivery Sheet Receiving ที่หน้า Purchasing Parameters" & "', 'error');", True)
        '    Response.Redirect("Menu.aspx")
        'End If
        Dim DsReceiving = 0
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        ds = oWS.LoadDataSet(Session("Token"), "SLPoparms", "popUf_PoParms_DSReceiving", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            DsReceiving = ds.Tables(0).Rows(0)("popUf_PoParms_DSReceiving").ToString
        End If

        If DsReceiving = 0 Then
            Dim MsgType = ""
            MsgType = "Error [ PPCC ]"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert2('" & MsgType & "','" & "กรุณาเลือก Use Web Delivery Sheet Receiving ที่หน้า Purchasing Parameters" & "', 'error');", True)
        End If


        If Not Page.IsPostBack Then

            lblWebDeliveryReceive.Text = CheckWebDeliveryReceive()

            PSite = GetSite()
            PWhse = GetDefWhse()

            If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then

                Dim sAction As String = ""

                If Request.QueryString("Action") = "ScanDeliverySheet" Then
                    lblbarcode.Text = "Scan Delivery Sheet : "

                ElseIf Request.QueryString("Action") = "ScanLines" Then
                    lblbarcode.Text = "Scan Lines : "

                ElseIf Request.QueryString("Action") = "ScanTag" Then
                    lblbarcode.Text = "Scan Tag : "

                End If

                lblBarcodeDSNum.Text = Request.QueryString("DSNum").ToString
                CountScanLine.Value = Request.QueryString("CountScanLine").ToString

                BindListDSReceive()

            Else

                lblbarcode.Text = "Scan Delivery Sheet : "
                CountScanLine.Value = 0
            End If

        End If

        If lblInvoceAmt.Text = "" Then
            lblInvoceAmt.Text = 0
        End If

        If lblVAT.Text = "" Then
            lblVAT.Text = 0
        End If

        If lblTotalInvoice.Text = "" Then
            lblTotalInvoice.Text = 0
        End If

        lblInvoceAmt.Text = FormatNumber(lblInvoceAmt.Text, 4)
        lblVAT.Text = FormatNumber(lblVAT.Text, 4)
        lblTotalInvoice.Text = FormatNumber(lblTotalInvoice.Text, 4)

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")
        txtbarcode.Focus()

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sBarcode, Stat, MsgErr, MsgType As String

        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        If txtbarcode.Text <> "" Then

            If String.IsNullOrEmpty(txtbarcode.Text) Or String.IsNullOrWhiteSpace(txtbarcode.Text) Then

                MsgErr = "#1102 : โปรดสแกนอีกครั้ง"

                MsgType = "Error [ PPCC ]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                txtbarcode.Text = String.Empty
                txtbarcode.Focus()

                Exit Sub

            End If

            If lblbarcode.Text = "Scan Delivery Sheet : " Then

                lblBarcodeDSNum.Text = sBarcode

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "S" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "I" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_EX_DSReceiveHdrs", "PPCC_EX_DSReceivingSp", Parms)

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 10 Then
                        Stat = node.InnerText

                    ElseIf i = 11 Then
                        MsgType = node.InnerText

                    ElseIf i = 12 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If Stat = "FALSE" Then

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                    txtbarcode.Text = String.Empty
                Else

                    BindListDSReceive()

                    If lblWebDeliveryReceive.Text = "1" Then
                        lblbarcode.Text = "Scan Lines : "
                    End If

                End If


            ElseIf lblbarcode.Text = "Scan Lines : " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "L" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "I" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_EX_DSReceiveDetails", "PPCC_EX_DSReceivingSp", Parms)

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 10 Then
                        Stat = node.InnerText

                    ElseIf i = 11 Then
                        MsgType = node.InnerText

                    ElseIf i = 12 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If Stat = "FALSE" Then

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                    txtbarcode.Text = String.Empty
                Else

                    BindListDSReceive()

                    CountScanLine.Value = CountScanLine.Value + 1

                    If CountScanLine.Value = txtTotalLine.Text Then
                        lblbarcode.Text = "Scan Tag : "
                    End If

                End If

            ElseIf lblbarcode.Text = "Scan Tag : " Then

                Dim strPO As String = ""
                Dim arrBarcode As String()
                arrBarcode = sBarcode.Split(New Char() {"|"c})

                If arrBarcode.Length > 0 Then

                    strPO = arrBarcode(0)

                End If

                If Len(strPO) <> 10 Then
                    Dim lenBarcode As Integer = Len(sBarcode)
                    sBarcode = Right(Trim(sBarcode), lenBarcode - 1)
                End If

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "T" & "</Parameter>" &
                        "<Parameter>" & Trim(sBarcode) & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "I" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                'Console.WriteLine(Parms)

                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & Parms & "', 'error');", True)

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_EX_DSReceiveHdrs", "PPCC_EX_DSReceivingSp", Parms)

                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement


                    If i = 10 Then
                        Stat = node.InnerText

                    ElseIf i = 11 Then
                        MsgType = node.InnerText

                    ElseIf i = 12 Then
                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If Stat = "FALSE" Then

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                    txtbarcode.Text = String.Empty
                Else

                    BindListDSReceive()

                End If

            End If


        End If

        txtbarcode.Text = String.Empty

    End Sub

    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""


        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblBarcodeDSNum.Text & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "R" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"


        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_DSReceiveHdrs", "PPCC_EX_DSReceivingSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 10 Then
                Stat = node.InnerText

            ElseIf i = 11 Then
                MsgType = node.InnerText

            ElseIf i = 12 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            Session("PSession") = NewSessionID()
            Response.Redirect("DeliverySheetReceiving.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If

    End Sub

    Protected Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btndetail.Click

        Dim PostURL As String = ""

        PostURL = "?SessionID=" & Session("PSession").ToString _
                                & "&Action=" & Replace(Replace(lblbarcode.Text, ":", ""), " ", "") _
                                & "&DSNum=" & lblBarcodeDSNum.Text _
                                & "&CountScanLine=" & CountScanLine.Value

        Response.Redirect("DSTagDetail.aspx" & PostURL)

    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblBarcodeDSNum.Text & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "P" & "</Parameter>" &
                        "<Parameter>" & PSite.ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"


        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_DSReceiveHdrs", "PPCC_EX_DSReceivingSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 10 Then
                Stat = node.InnerText

            ElseIf i = 11 Then
                MsgType = node.InnerText

            ElseIf i = 12 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            Clear()

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & MsgErr & "', 'success');", True)

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If

    End Sub

    Protected Sub PanelList_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        Dim QtyOrder As Decimal = 0
        Dim QtyReceived As Decimal = 0
        Dim QtyRemain As Decimal = 0
        Dim QtySum As Decimal = 0
        Dim MaterialCost As Decimal = 0
        Dim LineAmt As Decimal = 0
        Dim QtyDelivery As Decimal = 0

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblconfirm As Label = CType(e.Item.FindControl("lblconfirm"), Label)

            Dim lblListQtyOrder As Label = CType(e.Item.FindControl("lblListQtyOrder"), Label)
            Dim lblListQtyReceived As Label = CType(e.Item.FindControl("lblListQtyReceived"), Label)
            Dim lblListRemain As Label = CType(e.Item.FindControl("lblListRemain"), Label)
            Dim lblListSumQty As Label = CType(e.Item.FindControl("lblListSumQty"), Label)
            'Dim lblListMatlCost As Label = CType(e.Item.FindControl("lblListMatlCost"), Label)
            'Dim lblListLineAmt As Label = CType(e.Item.FindControl("lblListLineAmt"), Label)
            Dim lblQtyDelivery As Label = CType(e.Item.FindControl("lblQtyDelivery"), Label)

            QtyOrder = CDec(IIf(lblListQtyOrder.Text = "", "0", lblListQtyOrder.Text))
            QtyReceived = CDec(IIf(lblListQtyReceived.Text = "", "0", lblListQtyReceived.Text))
            QtyRemain = CDec(IIf(lblListRemain.Text = "", "0", lblListRemain.Text))
            QtySum = CDec(IIf(lblListSumQty.Text = "", "0", lblListSumQty.Text))
            'MaterialCost = CDec(IIf(lblListMatlCost.Text = "", "0", lblListMatlCost.Text))
            'LineAmt = CDec(IIf(lblListLineAmt.Text = "", "0", lblListLineAmt.Text))
            QtyDelivery = CDec(IIf(lblQtyDelivery.Text = "", "0", lblQtyDelivery.Text))

            lblListQtyOrder.Text = FormatNumber(QtyOrder.ToString, 4)
            lblListQtyReceived.Text = FormatNumber(QtyReceived.ToString, 4)
            lblListRemain.Text = FormatNumber(QtyRemain.ToString, 4)
            lblListSumQty.Text = FormatNumber(QtySum.ToString, 4)
            lblQtyDelivery.Text = FormatNumber(QtyDelivery.ToString, 4)


            If QtySum = QtyDelivery Then
                lblconfirm.Visible = True
            Else
                lblconfirm.Visible = False
            End If


        End If

    End Sub

    Sub Clear()
        AmtTotalFormat.Value = String.Empty
        CountScanLine.Value = "0"
        lblBarcodeDSNum.Text = String.Empty
        'txtLocation.Text = String.Empty
        lblWebDeliveryReceive.Text = CheckWebDeliveryReceive()
        txtdate.Text = String.Empty
        txtPONo.Text = String.Empty
        txtWhse.Text = String.Empty
        txtvendname.Text = String.Empty
        txtvendVendInv.Text = String.Empty
        txtTotalLine.Text = String.Empty
        lblInvoceAmt.Text = FormatNumber(0, 4)
        lblVAT.Text = FormatNumber(0, 4)
        lblTotalInvoice.Text = FormatNumber(0, 4)
        PanelList.DataSource = Nothing
        PanelList.DataBind()
        Session("PSession") = NewSessionID()
        lblbarcode.Text = "Scan Delivery Sheet : "
    End Sub

#Region "Bind Data To Gridview"

    Sub BindListDSReceive()

        Dim PropertyList As String = ""
        Dim InvoceAmt As Decimal = 0
        Dim VAT As Decimal = 0
        Dim TotalInvoice As Decimal = 0

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "SessionID = '" & Session("PSession").ToString & "'"


        PropertyList = "ds_num, po_num, po_line, po_release, loc, vend_num, vend_inv, inv_date, whse, total_ds_line, tax_rate, item, qty_order, qty_received, qty_remain, sum_qty, lot, ItemDescription, DerInvoiceAmt, DerVat, DerTotalInvoice, QtyDelivery"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_DSReceiveDetails", PropertyList, Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            If lblWebDeliveryReceive.Text = "1" Then

                txtdate.Text = DateTime.Parse(ds.Tables(0).Rows(0)("inv_date")).ToString("dd/MM/yyyy")
                txtPONo.Text = ds.Tables(0).Rows(0)("po_num").ToString
                'txtLocation.Text = ds.Tables(0).Rows(0)("loc").ToString
                txtvendname.Text = ds.Tables(0).Rows(0)("vend_num").ToString
                txtvendVendInv.Text = ds.Tables(0).Rows(0)("vend_inv").ToString
                txtWhse.Text = ds.Tables(0).Rows(0)("Whse").ToString
                txtTotalLine.Text = ds.Tables(0).Rows(0)("total_ds_line").ToString
                Decimal.TryParse(ds.Tables(0).Rows(0)("DerInvoiceAmt").ToString, InvoceAmt)
                Decimal.TryParse(ds.Tables(0).Rows(0)("DerVat").ToString, VAT)
                Decimal.TryParse(ds.Tables(0).Rows(0)("DerTotalInvoice").ToString, TotalInvoice)

                'AmtTotalFormat.Value = ds.Tables(0).Rows(0)("Places").ToString

                If lblbarcode.Text = "Scan Lines : " Or lblbarcode.Text = "Scan Tag : " Then
                    PanelList.DataSource = ds.Tables(0)
                    PanelList.DataBind()
                End If

                If AmtTotalFormat.Value = "" And txtPONo.Text <> "" Then
                    AmtTotalFormat.Value = GetPlaces(txtPONo.Text)
                End If

                If AmtTotalFormat.Value <> "" Then
                    lblInvoceAmt.Text = FormatNumber(Math.Round(InvoceAmt, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                    lblVAT.Text = FormatNumber(Math.Round(VAT, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                    lblTotalInvoice.Text = FormatNumber(Math.Round(TotalInvoice, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                End If

                'Else

                '    txtdate.Text = Left(ds.Tables(0).Rows(0)("inv_date").ToString, 10)
                '    txtPONo.Text = ds.Tables(0).Rows(0)("po_num").ToString
                '    'txtLocation.Text = ds.Tables(0).Rows(0)("loc").ToString
                '    txtvendname.Text = ds.Tables(0).Rows(0)("vend_num").ToString
                '    txtvendVendInv.Text = ds.Tables(0).Rows(0)("vend_inv").ToString
                '    txtWhse.Text = ds.Tables(0).Rows(0)("Whse").ToString
                '    txtTotalLine.Text = ds.Tables(0).Rows(0)("total_ds_line").ToString
                '    'AmtTotalFormat.Value = ds.Tables(0).Rows(0)("Places").ToString
                '    Decimal.TryParse(ds.Tables(0).Rows(0)("DerInvoiceAmt").ToString, InvoceAmt)
                '    Decimal.TryParse(ds.Tables(0).Rows(0)("DerVat").ToString, VAT)
                '    Decimal.TryParse(ds.Tables(0).Rows(0)("DerTotalInvoice").ToString, TotalInvoice)


                '    If AmtTotalFormat.Value <> "" Then
                '        lblInvoceAmt.Text = FormatNumber(Math.Round(InvoceAmt, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                '        lblVAT.Text = FormatNumber(Math.Round(VAT, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                '        lblTotalInvoice.Text = FormatNumber(Math.Round(TotalInvoice, CInt(AmtTotalFormat.Value), MidpointRounding.AwayFromZero), 4)
                '    End If

                '    If AmtTotalFormat.Value = "" And txtPONo.Text <> "" Then
                '        AmtTotalFormat.Value = GetPlaces(txtPONo.Text)
                '    End If

            End If

        End If


    End Sub

#End Region

#Region "Function"

    Function GetSite() As String

        GetSite = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLParms", "Site", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetSite = ds.Tables(0).Rows(0)("Site").ToString
        End If

        Return GetSite

    End Function

    Function GetDefWhse() As String

        GetDefWhse = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "SLInvparms", "DefWhse", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDefWhse = ds.Tables(0).Rows(0)("DefWhse").ToString
        End If

        Return GetDefWhse

    End Function
    Function GetDsreceiving() As Integer

        Dim DsReceiving = 0
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token"), "SLPoparms", "popUf_PoParms_DSReceiving", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            DsReceiving = ds.Tables(0).Rows(0)("popUf_PoParms_DSReceiving").ToString
        End If



    End Function

    Function NewSessionID() As String

        NewSessionID = ""

        Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_Ex_GetSessionIDSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 1 Then
                NewSessionID = node.InnerText
                Exit For
            End If

            i += 1
        Next

        Return NewSessionID

    End Function


    Public Function CheckWebDeliveryReceive() As String

        CheckWebDeliveryReceive = "0"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPoparms", "popUf_PoParms_DSReceiving", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            CheckWebDeliveryReceive = ds.Tables(0).Rows(0)("popUf_PoParms_DSReceiving").ToString

        End If

        Return CheckWebDeliveryReceive

    End Function

    Public Function GetPlaces(PoNum As String) As String

        GetPlaces = "0"

        Dim CurrCode As String = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPos", "PoCurrCode", "PoNum = '" & PoNum & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            CurrCode = ds.Tables(0).Rows(0)("PoCurrCode").ToString

        End If

        If CurrCode <> "" Then

            oWS = New CNIService.DOWebServiceSoapClient

            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLCurrencyCodes", "Places", "CurrCode = '" & CurrCode & "'", "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                GetPlaces = ds.Tables(0).Rows(0)("Places").ToString

            End If

        End If

        Return GetPlaces

    End Function

#End Region

End Class