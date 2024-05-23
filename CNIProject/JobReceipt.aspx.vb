Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class JobReceipt
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim Propertie As String
    Dim DateNow As String
    Dim LenPointQty As Integer = 0
    Dim LabelItemContorl As String = "0"

    Private Shared Whse As String
    Private Shared ParmSite As String
    Private Shared UserID As String

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

    Private Shared Property PUserID() As String
        Get
            Return UserID
        End Get
        Set(value As String)
            UserID = value
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

        'If Session("Employee") Is Nothing Then
        '    Response.Redirect("Menu.aspx")
        'Else
        '    If Session("Employee").ToString = "" Then
        '        Response.Redirect("Menu.aspx")
        '    End If

        'End If

        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

            'Session.Remove("LabelJobreceipt")

            PSite = GetSite()
            PWhse = GetDefWhse()
            PUserID = GetUserID()

            DateNow = Date.Now.ToString("dd/MM/yyyy")

            txtdate.Text = DateNow

            btnprocess.Attributes.Add("disabled", "disabled")

            GetLoc()


            If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then


                txtdate.Text = Request.QueryString("Date")
                txtjob.Text = Request.QueryString("Job")
                txtsuffix.Text = Request.QueryString("Suffix")
                txtoper.Text = Request.QueryString("OperNum")
                txtqty.Text = Request.QueryString("Qty")
                ddlloc.SelectedIndex = ddlloc.Items.IndexOf(ddlloc.Items.FindByValue(Request.QueryString("Loc").ToString))
                Session("Stat") = Request.QueryString("Type")

                HiddenField1.Value = GetCoProductMix(Request.QueryString("Job"), Request.QueryString("Suffix"))

                If Session("Stat").ToString = "Receive" Then
                    btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
                    btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Receive</strong>"
                Else
                    btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
                    btnstat.Text = "<i class=""fa fa-times"" aria-hidden=""true""></i>" & " <strong>Reject</strong>"
                End If

                If Request.QueryString("Fraction") = "1" Then
                    chkfraction.Checked = True
                Else
                    chkfraction.Checked = False
                End If


                BindJobReceipt()

                    'If PanelList.Items.Count > 0 Then
                    btnprocess.Attributes.Remove("disabled")
                    'End If

                Else

                    Session("Stat") = "Receive"

            End If



        End If


        If Session("LabelJobreceipt") Is Nothing Then
            lblbarcode.Text = "Scan Job Order: "
        Else
            If txtjob.Text = String.Empty Then
                lblbarcode.Text = "Scan Job Order: "
            Else
                lblbarcode.Text = Session("LabelJobreceipt").ToString
            End If

        End If

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")
        txtdate.Attributes.Add("readonly", "readonly")

    End Sub

    Protected Sub btnstat_Click(sender As Object, e As EventArgs) Handles btnstat.Click

        If PanelList.Items.Count = 0 Then
            PanelList.DataSource = Nothing
            PanelList.DataBind()
        Else
            Clearwhenclickchangedstat()
        End If

        If Session("Stat").ToString = "Receive" Then
            btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-times"" aria-hidden=""true""></i>" & " <strong>Reject</strong>"
            Session("Stat") = "Reject"
        Else
            btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Receive</strong>"
            Session("Stat") = "Receive"
        End If

    End Sub

    Protected Sub btndetail_Click(sender As Object, e As EventArgs) Handles btndetail.Click

        Dim PostURL As String = ""

        PostURL = "?SessionID=" & Session("PSession").ToString & "&Date=" & txtdate.Text & ""
        PostURL = PostURL & "&Job=" & txtjob.Text & "&Suffix=" & txtsuffix.Text & "&OperNum=" & txtoper.Text & ""
        PostURL = PostURL & "&Qty=" & txtqty.Text & "&Loc=" & ddlloc.SelectedItem.Value & "&Type=" & Session("Stat").ToString & "&Fraction=" & IIf(chkfraction.Checked = True, 1, 0) & ""


        Response.Redirect("JobReceiptDetail.aspx" & PostURL)

    End Sub

    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & "J" & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & txtjob.Text & "</Parameter>" &
                "<Parameter>" & txtsuffix.Text & "</Parameter>" &
                "<Parameter>" & txtoper.Text & "</Parameter>" &
                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                "<Parameter>" & PWhse.ToString & "</Parameter>" &
                "<Parameter>" & IIf(Session("Stat") = "Receive", "I", "W") & "</Parameter>" &
                "<Parameter>" & "R" & "</Parameter>" &
                "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                "<Parameter>" & IIf(chkfraction.Checked = True, 1, 0) & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobReceiptSum", "PPCC_Ex_JobReceiptSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 13 Then
                Stat = node.InnerText

            ElseIf i = 14 Then
                MsgType = node.InnerText

            ElseIf i = 15 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            Session("PSession") = NewSessionID()
            Session.Remove("LabelJobreceipt")
            Response.Redirect("JobReceipt.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If

    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim sBarcode, Stat, MsgErr, MsgType As String
        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                "<Parameter>" & "J" & "</Parameter>" &
                "<Parameter>" & sBarcode & "</Parameter>" &
                "<Parameter>" & txtjob.Text & "</Parameter>" &
                "<Parameter>" & txtsuffix.Text & "</Parameter>" &
                "<Parameter>" & txtoper.Text & "</Parameter>" &
                "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                "<Parameter>" & PWhse.ToString & "</Parameter>" &
                "<Parameter>" & IIf(Session("Stat") = "Receive", "I", "W") & "</Parameter>" &
                "<Parameter>" & "P" & "</Parameter>" &
                "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                "<Parameter>" & IIf(chkfraction.Checked = True, 1, 0) & "</Parameter>" &
                "<Parameter>" & DBNull.Value & "</Parameter>" &
                "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobReceiptSum", "PPCC_Ex_JobReceiptSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 13 Then
                Stat = node.InnerText

            ElseIf i = 14 Then
                MsgType = node.InnerText

            ElseIf i = 15 Then
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

        btnprocess.Attributes.Remove("disabled")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sBarcode, Stat, MsgErr, MsgType, sJob, sSuffix, sOper, CoProduct, sPreassignnedLot, sProductLine, sCheckJob, sTagID As String
        Dim QtyComplete As Decimal

        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        sJob = ""
        sSuffix = ""
        sOper = ""
        CoProduct = "0"
        sPreassignnedLot = ""
        sProductLine = ""
        'LabelItemContorl = "0"
        sCheckJob = ""
        sTagID = ""
        QtyComplete = 0
        Dim doc As XmlDocument = New XmlDocument()
        Dim i As Integer

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Scan Job Order: " Then

                If sBarcode.Contains("|") Then

                    Dim arrBarcode As String()
                    arrBarcode = sBarcode.Split(New Char() {"|"c})

                    If arrBarcode.Length > 0 Then

                        sCheckJob = CheckJobOrder(arrBarcode(0))

                        If sCheckJob <> "" Then
                            sJob = arrBarcode(0)
                            sSuffix = arrBarcode(1)
                            sOper = arrBarcode(2)

                        Else

                            If Left(arrBarcode(4), 2) = "TD" Then
                                sTagID = arrBarcode(4)
                            Else
                                sTagID = arrBarcode(5)
                            End If

                            oWS = New CNIService.DOWebServiceSoapClient
                            ds = New DataSet

                            Filter = "TagID = '" & sTagID & "'"

                            ds = oWS.LoadDataSet(Session("Token"), "ppcc_tags", "RefNum, RefLine", Filter, "", "", 0)

                            If ds.Tables(0).Rows.Count > 0 Then
                                sJob = ds.Tables(0).Rows(0)("RefNum").ToString
                                sSuffix = ds.Tables(0).Rows(0)("RefLine").ToString
                            End If

                        End If

                    End If

                End If

                Parms = "<Parameters><Parameter>" & sJob & "</Parameter>" &
                        "<Parameter>" & sSuffix & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "SLJobs", "JobReceiptValidateJobSp", Parms)

                doc.LoadXml(Parms)

                i = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 5 Then

                        Double.TryParse(node.InnerText, QtyComplete)

                    ElseIf i = 8 Then

                        MsgErr = node.InnerText

                    End If

                    i += 1

                Next

                If MsgErr <> "" Then

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [STD]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                    txtbarcode.Text = String.Empty
                    Exit Sub

                End If

                Parms = ""

                    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "J" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & txtjob.Text & "</Parameter>" &
                            "<Parameter>" & txtsuffix.Text & "</Parameter>" &
                            "<Parameter>" & txtoper.Text & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter>" & PWhse.ToString & "</Parameter>" &
                            "<Parameter>" & IIf(Session("Stat") = "Receive", "I", "W") & "</Parameter>" &
                            "<Parameter>" & "I" & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                            "<Parameter>" & IIf(chkfraction.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & QtyComplete & "</Parameter>" &
                            "</Parameters>"

                ElseIf lblbarcode.Text = "Scan Tag: " Then

                    Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                       "<Parameter>" & "T" & "</Parameter>" &
                       "<Parameter>" & sBarcode & "</Parameter>" &
                       "<Parameter>" & txtjob.Text & "</Parameter>" &
                       "<Parameter>" & txtsuffix.Text & "</Parameter>" &
                       "<Parameter>" & txtoper.Text & "</Parameter>" &
                       "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                       "<Parameter>" & PWhse.ToString & "</Parameter>" &
                       "<Parameter>" & IIf(Session("Stat") = "Receive", "I", "W") & "</Parameter>" &
                       "<Parameter>" & "I" & "</Parameter>" &
                       "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                       "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                       "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                       "<Parameter>" & IIf(chkfraction.Checked = True, 1, 0) & "</Parameter>" &
                       "<Parameter>" & DBNull.Value & "</Parameter>" &
                       "</Parameters>"

            End If

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobReceiptSum", "PPCC_Ex_JobReceiptSp", Parms)

            'Dim doc As XmlDocument = New XmlDocument()
            'doc.LoadXml(Parms)

            'Dim i As Integer = 1

            doc.LoadXml(Parms)

            i = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 13 Then
                    Stat = node.InnerText

                ElseIf i = 14 Then
                    MsgType = node.InnerText

                ElseIf i = 15 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If Stat = "TRUE" Then

                If lblbarcode.Text = "Scan Job Order: " Then

                    If sBarcode.Contains("|") Then

                        Dim arrBarcode As String()
                        arrBarcode = sBarcode.Split(New Char() {"|"c})

                        If arrBarcode.Length > 0 Then

                            sCheckJob = CheckJobOrder(arrBarcode(0))

                            If sCheckJob <> "" Then
                                sJob = arrBarcode(0)
                                sSuffix = arrBarcode(1)
                                sOper = arrBarcode(2)

                            Else

                                If Left(arrBarcode(4), 2) = "TD" Then
                                    sTagID = arrBarcode(4)
                                Else
                                    sTagID = arrBarcode(5)
                                End If

                                oWS = New CNIService.DOWebServiceSoapClient
                                ds = New DataSet

                                Filter = "TagID = '" & sTagID & "'"

                                ds = oWS.LoadDataSet(Session("Token"), "ppcc_tags", "RefNum, RefLine", Filter, "", "", 0)

                                If ds.Tables(0).Rows.Count > 0 Then
                                    sJob = ds.Tables(0).Rows(0)("RefNum").ToString
                                    sSuffix = ds.Tables(0).Rows(0)("RefLine").ToString
                                End If

                            End If

                        End If

                    End If

                    txtjob.Text = sJob
                    txtsuffix.Text = sSuffix
                    txtoper.Text = GetLastOper(txtjob.Text, txtsuffix.Text)


                    HiddenField1.Value = GetCoProductMix(txtjob.Text, txtsuffix.Text)

                    BindJobReceipt()

                    lblbarcode.Text = "Scan Tag: "
                    Session("LabelJobreceipt") = "Scan Tag: "

                ElseIf lblbarcode.Text = "Scan Tag: " Then

                    btnprocess.Attributes.Remove("disabled")

                    BindJobReceipt()

                    lblbarcode.Text = "Scan Tag: "
                    Session("LabelJobreceipt") = "Scan Tag: "

                End If


            End If

            If Stat = "FALSE" Then


                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


            End If

        End If

        txtbarcode.Text = String.Empty

    End Sub

    Sub Clear()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        Session("Stat") = "Receive"
        btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
        btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Receive</strong>"
        lblbarcode.Text = "Scan Job Order: "
        Session.Remove("LabelJobreceipt")

        txtbarcode.Text = String.Empty
        txtjob.Text = String.Empty
        txtsuffix.Text = String.Empty

        ddlitem.Items.Clear()
        txtoper.Text = String.Empty
        txtqty.Text = String.Empty
        HiddenField1.Value = ""

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        Session("PSession") = NewSessionID()

        GetLoc()

        chkfraction.Checked = False

    End Sub

    Sub Clearwhenclickchangedstat()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        lblbarcode.Text = "Scan Job Order: "
        Session.Remove("LabelJobreceipt")

        txtbarcode.Text = String.Empty
        txtjob.Text = String.Empty
        txtsuffix.Text = String.Empty

        ddlitem.Items.Clear()
        txtoper.Text = String.Empty
        txtqty.Text = String.Empty
        HiddenField1.Value = ""

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        Session("PSession") = NewSessionID()

        GetLoc()

    End Sub

    Protected Sub PanelList_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        Dim QtyRelease As Decimal = 0
        Dim QtyComplete As Decimal = 0
        Dim QtyReceive As Decimal = 0
        Dim QtyRemain As Decimal = 0
        Dim QtySum As Decimal = 0

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblListQtyRelease As Label = CType(e.Item.FindControl("lblListQtyRelease"), Label)
            Dim lblListQtyComplete As Label = CType(e.Item.FindControl("lblListQtyComplete"), Label)
            Dim lblListQtyReceive As Label = CType(e.Item.FindControl("lblListQtyReceive"), Label)
            Dim lblListRemain As Label = CType(e.Item.FindControl("lblListRemain"), Label)
            Dim lblListSumQty As Label = CType(e.Item.FindControl("lblListSumQty"), Label)

            QtyRelease = CDec(lblListQtyRelease.Text)
            QtyComplete = CDec(lblListQtyComplete.Text)
            QtyReceive = CDec(IIf(lblListQtyReceive.Text = "", "0", lblListQtyReceive.Text))
            QtyRemain = CDec(IIf(lblListRemain.Text = "", "0", lblListRemain.Text))
            QtySum = CDec(IIf(lblListSumQty.Text = "", "0", lblListSumQty.Text))

            lblListQtyRelease.Text = FormatNumber(QtyRelease.ToString, LenPointQty)
            lblListQtyComplete.Text = FormatNumber(QtyComplete.ToString, LenPointQty)
            lblListQtyReceive.Text = FormatNumber(QtyReceive.ToString, LenPointQty)
            lblListRemain.Text = FormatNumber(QtyRemain.ToString, LenPointQty)
            lblListSumQty.Text = FormatNumber(QtySum.ToString, LenPointQty)

            'txtqty.Text = FormatNumber(QtySum.ToString, LenPointQty)

        End If

    End Sub

    Function GetCoProductMix(Job As String, Suffix As String) As String

        GetCoProductMix = ""

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "CoProductMix", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetCoProductMix = ds.Tables(0).Rows(0)("CoProductMix").ToString
        End If

        Return GetCoProductMix

    End Function

    Function GetLabelItemContorl(Job As String, Suffix As String) As String

        GetLabelItemContorl = "0"

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "jobUf_job_Labelcontrol", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetLabelItemContorl = IIf(IsDBNull(ds.Tables(0).Rows(0)("jobUf_job_Labelcontrol")), "0", ds.Tables(0).Rows(0)("jobUf_job_Labelcontrol").ToString)
        End If

        Return GetLabelItemContorl

    End Function

    Function GetPreassignnedLot(Job As String, Suffix As String) As String

        GetPreassignnedLot = ""

        Filter = "RefNum = '" & Job & "' And RefLineSuf = '" & Suffix & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLPreassignedLots", "Lot", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetPreassignnedLot = ds.Tables(0).Rows(0)("Lot").ToString
        End If

        Return GetPreassignnedLot

    End Function

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

        ds = oWS.LoadDataSet(Session("Token"), "PPCC_SLUserNames", "UserLocalWhse", "Username = '" & Session("UserName").ToString & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDefWhse = ds.Tables(0).Rows(0)("UserLocalWhse").ToString
        End If

        Return GetDefWhse

    End Function

    Function GetProductLine(Job As String, Suffix As String) As String

        GetProductLine = ""

        Dim OperNum As String = ""
        Dim ProductionLine As String = ""

        OperNum = GetLastOper(txtjob.Text, txtsuffix.Text)

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "' And OperNum = '" & OperNum & "'"

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobRoutes", "jbrUf_jobroute_line", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            ProductionLine = ds.Tables(0).Rows(0)("jbrUf_jobroute_line").ToString
        End If

        If ProductionLine <> "" Then

            Filter = "line_num = '" & ProductionLine & "'"

            oWS = New CNIService.DOWebServiceSoapClient

            ds = New DataSet

            ds = oWS.LoadDataSet(Session("Token").ToString, "ppcc_prod_lines", "Loc", Filter, "", "", 0)

            If ds.Tables(0).Rows.Count > 0 Then
                GetProductLine = ds.Tables(0).Rows(0)("Loc").ToString
            End If

        End If


        Return GetProductLine

    End Function

    Function GetLastOper(Job As String, Suffix As String) As String

        GetLastOper = ""

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

        ds = oWS.LoadDataSet(Session("Token"), "SLJobRoutes", "OperNum", Filter, "OperNum DESC", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetLastOper = ds.Tables(0).Rows(0)("OperNum").ToString
        End If

        Return GetLastOper

    End Function

    Public Function UnitQtyFormat() As Integer

        Dim strUnitQtyFormat As String = ""
        Dim PointQty As String = ""

        UnitQtyFormat = 0

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLInvparms", "QtyUnitFormat", "", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            strUnitQtyFormat = ds.Tables(0).Rows(0)("QtyUnitFormat").ToString

            Dim words As String() = strUnitQtyFormat.Split(New Char() {"."c})

            For Each word As String In words
                PointQty = words(1)
                Exit For
            Next

            UnitQtyFormat = Len(PointQty)

        End If

        Return UnitQtyFormat

    End Function


    Public Function GetUserID() As String

        GetUserID = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserNames", "UserId", "Username = '" & Session("UserName").ToString & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            GetUserID = ds.Tables(0).Rows(0)("UserId").ToString

        End If

        Return GetUserID

    End Function

    Public Function CheckJobOrder(Job As String) As String

        CheckJobOrder = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "Job", "Job = '" & Job & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            CheckJobOrder = ds.Tables(0).Rows(0)("Job").ToString

        End If

        Return CheckJobOrder

    End Function

#Region "Get Data Bind To Dropdownlist"

    Sub GetLoc()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet


        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_UserLocations", "LocId", "UserId = '" & PUserID & "'", "CreateDate", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlloc.Items.Add(New ListItem(dRow("LocId"), dRow("LocId")))

        Next

    End Sub

#End Region

#Region "Bind PanelList"


    Sub BindJobReceipt()

        Dim Filter As String
        Dim Propertie As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "SessionID = '" & Session("PSession").ToString & "'"

        Propertie = "Oper, Item, QtyRelease, QtyCompleted, QtyReceive, QtyRemain, QtySum, LabelItemControl, ItemDescription, Lot, SessionID"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobReceiptSum", Propertie, Filter, "CreateDate Desc", "", 0)

        PanelList.DataSource = ds.Tables(0)
        PanelList.DataBind()

        If ds.Tables(0).Rows.Count > 0 Then

            Dim dt As DataTable = ds.Tables(0)
            Dim sum As Decimal = FormatNumber(Convert.ToDecimal(dt.Compute("SUM(QtySum)", "SessionID = '" & Session("PSession").ToString & "'")), LenPointQty)

            txtqty.Text = Decimal.Round(sum.ToString, LenPointQty, MidpointRounding.AwayFromZero)
        Else
            txtqty.Text = Decimal.Round(0, LenPointQty, MidpointRounding.AwayFromZero)
        End If

    End Sub

    Protected Sub ddlitem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlitem.SelectedIndexChanged

        'PanelList.DataSource = Nothing
        'PanelList.DataBind()
        'BindJobReceipt()

    End Sub

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

#End Region

End Class