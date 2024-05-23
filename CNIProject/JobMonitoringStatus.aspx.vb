Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval
Imports System.Web.UI.WebControls.Expressions
Imports System.IO
Imports System.Linq
Imports System.Drawing
Imports System.Data.SqlClient

Public Class JobMonitoringStatus
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim LenPointQty As Integer = 0
    Dim LenPointQtyFormat As Integer = 0
    Dim DateNow As String

    Private Shared ParmSite As String
    Private Shared JobStockTran As String
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

        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "HideDiv();", True)

            PSite = GetSite()
            PWhse = GetDefWhse()

            BindListJob()

            DateNow = Date.Now.ToString("dd/MM/yyyy")

            txtdate.Text = DateNow

        End If

        'DisplayJobInfo.Attributes.Add("style", "display:none")
        txtBarcode.Focus()

        txtBarcode.Attributes.Add("onchange", "javascript:scanbarcode();")
        txtStartdate.Attributes.Add("readonly", "readonly")
        txtStartTime.Attributes.Add("readonly", "readonly")
        txtResourceName.Attributes.Add("readonly", "readonly")
        txtEnddate.Attributes.Add("readonly", "readonly")
        txtEndTime.Attributes.Add("readonly", "readonly")
        txtEndResource.Attributes.Add("readonly", "readonly")
        txtEndResourceName.Attributes.Add("readonly", "readonly")
        txtReasonDesc.Attributes.Add("readonly", "readonly")

        If lblJobStatus.Text = "InProcess" Then
            lblJobStatus.CssClass = "text-success"
        ElseIf lblJobStatus.Text = "Not Start" Then
            lblJobStatus.CssClass = "text-primary"
        ElseIf lblJobStatus.Text = "Not Complete" Then
            lblJobStatus.CssClass = "text-danger"
        ElseIf lblJobStatus.Text = "Not Complete" Then
            lblJobStatus.CssClass = "text-warning"
        End If

    End Sub

    Function GetUserInitial(UserName As String) As String

        GetUserInitial = ""
        Dim UserID As String = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserId", "Username='" & UserName & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            UserID = ds.Tables(0).Rows(0)("UserId").ToString
        End If

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLUserLocals", "UserCode", "UserId='" & UserID & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetUserInitial = ds.Tables(0).Rows(0)("UserCode").ToString
        End If

        Return GetUserInitial

    End Function


    'Protected Sub btSubmitModal_Click(sender As Object, e As EventArgs) Handles btSubmitModal.Click
    '    Dim StartTime As String = ""

    '    StartTime = lbStartTime.Text

    'End Sub

    'Sub btSubmitModal_Click()

    '    Dim StartTime As String = ""

    '    lbStartTime.Text = lbTime.Text

    '    'btSubmitModal.Attributes.Add("onchange", "javascript:scanbarcode();")


    'End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, JobOrder, Item, Wc, WcDesc, JobOperDesc, JobResource, StartTime, EndTime, Inforbar1, Inforbar2, Jobtext As String
        Dim JobOper, JobSuffix As Integer
        Dim QtyRelease, QtyReciept, QtyComplete, QtyMove As Decimal
        Dim ItemDesc, RESID, DESCR As String

        sBarcode = txtBarcode.Text
        MsgErr = "0"
        Inforbar1 = ""
        Inforbar2 = ""
        MsgType = ""
        QtyRelease = 0
        QtyReciept = 0
        QtyComplete = 0
        QtyMove = 0
        JobOrder = ""
        Item = ""
        Wc = ""
        WcDesc = ""
        JobOperDesc = ""
        JobResource = ""
        StartTime = ""
        EndTime = ""
        Inforbar1 = ""
        Inforbar2 = ""
        Jobtext = ""
        Stat = ""
        ItemDesc = ""
        RESID = ""
        DESCR = ""

        If txtBarcode.Text <> "" Then


            Parms = "<Parameters><Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Error
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@INforbar
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@@MsgType
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_Ex_CheckWorkCenterSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim x As Integer = 1

            For Each node As XmlNode In doc.DocumentElement
                If x = 4 Then
                    MsgErr = node.InnerText
                ElseIf x = 5 Then
                    Inforbar1 = node.InnerText
                ElseIf x = 6 Then
                    MsgType = node.InnerText
                End If
                x += 1
            Next

            If Inforbar1 <> "" Then

                ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "HideDiv();", True)

                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & Inforbar1 & "', 'error');", True)

            Else

                MsgType = ""

                Parms = "<Parameters><Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Job
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobOper
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobSuffix
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Item
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Wc
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@WcDesc
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@QtyRelease
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@QtyReciept
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@QtyComplete
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@QtyMove
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobOperDesc
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobResource
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobStat
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@StartTime
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@EndTime
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Error
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@Infobar
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@JobText
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@MsgType
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@ItemDesc
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@RESID
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" & '@DESCR
                        "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_Ex_GetJobOperSp", Parms)

                Dim doc1 As XmlDocument = New XmlDocument()
                doc1.LoadXml(Parms)
                Dim i As Integer = 1

                For Each node As XmlNode In doc1.DocumentElement

                    If i = 4 Then
                        JobOrder = node.InnerText
                    ElseIf i = 5 Then
                        JobOper = node.InnerText
                    ElseIf i = 6 Then
                        JobSuffix = node.InnerText
                    ElseIf i = 7 Then
                        Item = node.InnerText
                    ElseIf i = 8 Then
                        Wc = node.InnerText
                    ElseIf i = 9 Then
                        WcDesc = node.InnerText
                    ElseIf i = 10 Then
                        QtyRelease = node.InnerText
                    ElseIf i = 11 Then
                        QtyReciept = node.InnerText
                    ElseIf i = 12 Then
                        QtyComplete = node.InnerText
                    ElseIf i = 13 Then
                        QtyMove = node.InnerText
                    ElseIf i = 14 Then
                        JobOperDesc = node.InnerText
                    ElseIf i = 15 Then
                        JobResource = node.InnerText
                    ElseIf i = 16 Then
                        Stat = node.InnerText
                    ElseIf i = 17 Then
                        StartTime = node.InnerText
                    ElseIf i = 18 Then
                        EndTime = node.InnerText
                    ElseIf i = 19 Then
                        MsgErr = node.InnerText
                    ElseIf i = 20 Then
                        Inforbar2 = node.InnerText
                    ElseIf i = 21 Then
                        Jobtext = node.InnerText
                    ElseIf i = 22 Then
                        MsgType = node.InnerText
                    ElseIf i = 23 Then
                        ItemDesc = node.InnerText
                    ElseIf i = 24 Then
                        RESID = node.InnerText
                    ElseIf i = 25 Then
                        DESCR = node.InnerText
                    End If

                    i += 1

                Next

                If Inforbar2 <> "" Then

                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "HideDiv();", True)
                    MsgType = "Error [" & MsgType & "]"

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & Inforbar2 & "', 'error');", True)

                Else

                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ShowDiv();", True)

                    lblJob.Text = JobOrder
                    lblSuffix.Text = JobSuffix
                    lblJobText.Text = Jobtext
                    lblOper.Text = JobOper
                    lblQtyRelease.Text = FormatNumber(QtyRelease, LenPointQty)
                    lblJobStatus.Text = Stat
                    lblwc.Text = Wc
                    lblwcdesc.Text = WcDesc
                    lblQtyReceive.Text = FormatNumber(QtyReciept, LenPointQty)
                    lblQtyComplete.Text = FormatNumber(QtyComplete, LenPointQty)
                    lblQtyMove.Text = FormatNumber(QtyMove, LenPointQty)
                    lblStartTime.Text = StartTime
                    lblEndTime.Text = EndTime
                    lblItemDesc.Text = ItemDesc
                    lblItem.Text = Item
                    lblresource.Text = RESID
                    lblresourceDesc.Text = DESCR

                End If

            End If
        End If

        txtBarcode.Text = String.Empty

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim Infobar As String
        Infobar = ""

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        Dim doc1 As XmlDocument = New XmlDocument()
        doc1.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc1.DocumentElement

            If i = 18 Then
                Infobar = node.InnerText
            End If

            i += 1

        Next

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & Infobar & "', 'success');", True)

        Clear()
        BindListJob()

    End Sub


    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Dim Infobar, NewStartTime, NewEndTime, NewRESID, NewDESCR, NewStat As String
        Infobar = ""
        NewStartTime = ""
        NewEndTime = ""
        NewRESID = ""
        NewDESCR = ""
        NewStat = ""

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & "X" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        Dim doc1 As XmlDocument = New XmlDocument()
        doc1.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc1.DocumentElement

            If i = 14 Then
                NewStartTime = node.InnerText
            ElseIf i = 15 Then
                NewEndTime = node.InnerText
            ElseIf i = 16 Then
                NewRESID = node.InnerText
            ElseIf i = 17 Then
                NewDESCR = node.InnerText
            ElseIf i = 18 Then
                Infobar = node.InnerText
            ElseIf i = 19 Then
                NewStat = node.InnerText
            End If

            i += 1

        Next

        lblStartTime.Text = NewStartTime
        lblEndTime.Text = NewEndTime
        lblresource.Text = NewRESID
        lblresourceDesc.Text = NewDESCR
        lblJobStatus.Text = NewStat

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Success','" & Infobar & "', 'success');", True)


    End Sub

    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        Dim Stat, MsgErr, MsgType As String

        Parms = ""
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_CheckJobMatlSp", Parms)

        Dim doc1 As XmlDocument = New XmlDocument()
        doc1.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc1.DocumentElement

            If i = 8 Then
                Stat = node.InnerText

            ElseIf i = 9 Then
                MsgType = node.InnerText

            ElseIf i = 10 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            DateNow = Date.Now.ToString("dd/MM/yyyy")
            txtStartdate.Text = DateNow

            txtStartTime.Text = Date.Now.ToString("HH:mm")
            GetResource()

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "openModelStart();", True)

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If




    End Sub

    Protected Sub btnEnd_Click(sender As Object, e As EventArgs) Handles btnEnd.Click

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtEnddate.Text = DateNow

        txtEndTime.Text = Date.Now.ToString("HH:mm")
        GetReasonCode()

        Dim sReasonDesc As String = ""

        If ddlReasonCode.Items.Count > 0 Then
            sReasonDesc = GetReasonDesc(ddlReasonCode.SelectedItem.Value)
        End If

        txtReasonDesc.Text = sReasonDesc

        txtEndResource.Text = lblresource.Text
        txtEndResourceName.Text = lblresourceDesc.Text

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Pop", "openModelEnd();", True)

    End Sub

    Protected Sub btnUnposted_Click(sender As Object, e As EventArgs) Handles btnUnposted.Click

        Dim Infobar As String
        Infobar = ""

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        Dim PostURL, sBarCode, sResource As String
        PostURL = ""
        sBarCode = lblJob.Text & "|" & lblSuffix.Text & "|" & lblOper.Text
        sResource = lblresource.Text

        Dim arrResource As String()
        arrResource = lblresource.Text.Split(New Char() {","c})

        If arrResource.Length > 0 Then
            sResource = arrResource(0)
        End If

        PostURL = "?SessionID=" & Session("PSession").ToString & "&sBarcode=" & sBarCode & "&Job=" & lblJob.Text & ""
        PostURL = PostURL & "&Suffix=" & lblSuffix.Text & "&OperNum=" & lblOper.Text & "&StartTime=" & lblStartTime.Text & ""
        PostURL = PostURL & "&EndTime=" & lblEndTime.Text & "&Wc=" & lblwc.Text & "&Resource=" & sResource & ""

        Response.Redirect("Unposted.aspx" & PostURL)

    End Sub

    Protected Sub PanelList_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblListJobStatus As Label = CType(e.Item.FindControl("lblListJobStatus"), Label)

            If lblListJobStatus.Text = "InProcess" Then
                lblListJobStatus.CssClass = "text-success"
            ElseIf lblListJobStatus.Text = "Not Start" Then
                lblListJobStatus.CssClass = "text-primary"
            ElseIf lblListJobStatus.Text = "Not Complete" Then
                lblListJobStatus.CssClass = "text-danger"
            ElseIf lblListJobStatus.Text = "Not Complete" Then
                lblListJobStatus.CssClass = "text-warning"
            End If


        End If

    End Sub

    Sub Clear()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "HideDiv();", True)

        lblJob.Text = String.Empty
        lblSuffix.Text = String.Empty
        lblJobText.Text = String.Empty
        lblOper.Text = String.Empty
        lblQtyRelease.Text = String.Empty
        lblJobStatus.Text = String.Empty
        lblwc.Text = String.Empty
        lblwcdesc.Text = String.Empty
        lblQtyReceive.Text = String.Empty
        lblQtyComplete.Text = String.Empty
        lblQtyMove.Text = String.Empty
        lblStartTime.Text = String.Empty
        lblEndTime.Text = String.Empty
        lblItemDesc.Text = String.Empty
        lblItem.Text = String.Empty

        Session("PSession") = NewSessionID()

    End Sub

    Protected Sub ddlResource_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlResource.SelectedItem.Value <> "" Then

            Dim sResourceName As String = ""

            sResourceName = GetResourceName(lblJob.Text, lblSuffix.Text, lblOper.Text, ddlResource.SelectedItem.Value)
            txtResourceName.Text = sResourceName

            btnAddStart.Attributes.Remove("disabled")

        Else
            btnAddStart.Attributes.Add("disabled", "disabled")

        End If

    End Sub

    Protected Sub ddlReasonCode_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlReasonCode.SelectedItem.Value <> "" Then

            Dim sReasonDesc As String = ""

            sReasonDesc = GetReasonDesc(ddlReasonCode.SelectedItem.Value)
            txtReasonDesc.Text = sReasonDesc

        End If

    End Sub

    Protected Sub btnAddStart_Click(sender As Object, e As EventArgs) Handles btnAddStart.Click

        If ddlResource.SelectedItem.Value <> "" Then

            Parms = ""

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtStartdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & txtStartTime.Text & "</Parameter>" &
                    "<Parameter>" & ddlResource.SelectedItem.Value & "</Parameter>" &
                    "<Parameter>" & txtResourceName.Text & "</Parameter>" &
                    "<Parameter>" & "A" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

            BindAddResource()

            ddlResource.SelectedIndex = ddlResource.Items.IndexOf(ddlResource.Items.FindByValue(""))
            txtResourceName.Text = String.Empty
            btnAddStart.Attributes.Add("disabled", "disabled")

        End If

    End Sub

    Protected Sub btnStartCancel_Click(sender As Object, e As EventArgs) Handles btnStartCancel.Click

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtStartdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & txtStartTime.Text & "</Parameter>" &
                    "<Parameter>" & ddlResource.SelectedItem.Value & "</Parameter>" &
                    "<Parameter>" & txtResourceName.Text & "</Parameter>" &
                    "<Parameter>" & "C" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Pop", "hideModelStart();", True)

    End Sub

    Protected Sub btnStartConfrim_Click(sender As Object, e As EventArgs) Handles btnStartConfrim.Click

        Dim NewStartTime, NewRESID, NewDESCR As String

        NewStartTime = ""
        NewRESID = ""
        NewDESCR = ""

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "S" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtStartdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & txtStartTime.Text & "</Parameter>" &
                    "<Parameter>" & ddlResource.SelectedItem.Value & "</Parameter>" &
                    "<Parameter>" & txtResourceName.Text & "</Parameter>" &
                    "<Parameter>" & "F" & "</Parameter>" &
                    "<Parameter>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 14 Then
                NewStartTime = node.InnerText
            ElseIf i = 16 Then
                NewRESID = node.InnerText
            ElseIf i = 17 Then
                NewDESCR = node.InnerText
            End If

            i += 1

        Next

        lblStartTime.Text = NewStartTime
        lblresource.Text = NewRESID
        lblresourceDesc.Text = NewDESCR

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Pop", "hideModelStart();", True)


    End Sub

    Protected Sub btnEndConfrim_Click(sender As Object, e As EventArgs) Handles btnEndConfrim.Click

        Dim NewEndTime As String

        NewEndTime = ""

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "E" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtEnddate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & txtEndTime.Text & "</Parameter>" &
                    "<Parameter>" & txtEndResource.Text & "</Parameter>" &
                    "<Parameter>" & txtEndResourceName.Text & "</Parameter>" &
                    "<Parameter>" & "F" & "</Parameter>" &
                    "<Parameter>" & ddlReasonCode.SelectedItem.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 15 Then
                NewEndTime = node.InnerText
            End If

            i += 1

        Next

        lblEndTime.Text = NewEndTime

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Pop", "hideModelEnd();", True)

    End Sub

    Protected Sub btnEndCancel_Click(sender As Object, e As EventArgs) Handles btnEndCancel.Click

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter>" & "E" & "</Parameter>" &
                    "<Parameter>" & lblJob.Text & "</Parameter>" &
                    "<Parameter>" & lblSuffix.Text & "</Parameter>" &
                    "<Parameter>" & lblOper.Text & "</Parameter>" &
                    "<Parameter>" & DateTime.Parse(txtEnddate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                    "<Parameter>" & txtEndTime.Text & "</Parameter>" &
                    "<Parameter>" & txtEndResource.Text & "</Parameter>" &
                    "<Parameter>" & txtEndResourceName.Text & "</Parameter>" &
                    "<Parameter>" & "C" & "</Parameter>" &
                    "<Parameter>" & ddlReasonCode.SelectedItem.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "PPCC_Ex_StartEndDateTimeSp", Parms)

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Pop", "hideModelStart();", True)

    End Sub


    'Protected Sub OnPagePropertiesChanging(sender As Object, e As PagePropertiesChangingEventArgs) Handles PanelList.PagePropertiesChanging
    '    TryCast(PanelList.FindControl("DataPager1"), DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
    '    BindListJob()
    'End Sub


    'Sub ItemCOListView(ByVal sCoProduct As String, ByVal sJob As String, ByVal sSuffix As Integer)

    '    Dim Filter As String
    '    Dim Propertie As String

    '    oWS = New CNIService.DOWebServiceSoapClient
    '    ds = New DataSet

    '    If sCoProduct = "1" Then

    '        Filter = "JobCoProductMix = 1 and Job = '" & sJob & "' and JobSuffix = " & sSuffix
    '        Propertie = "Item, ItemDescription"

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobItems", Propertie, Filter, "", "", 0)
    '    Else
    '        Filter = "Job = '" & sJob & "' and Suffix = " & sSuffix
    '        Propertie = "Item, ItemDescription"

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", Propertie, Filter, "", "", 0)
    '    End If

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        ListViewItemCO.DataSource = ds.Tables(0)
    '        ListViewItemCO.DataBind()
    '    End If

    'End Sub

    'Sub ResourceListView(ByVal sJob As String, ByVal sSuffix As Integer, ByVal sOperNum As Integer)

    '    Dim Filter As String
    '    Dim Propertie As String

    '    oWS = New CNIService.DOWebServiceSoapClient
    '    ds = New DataSet
    '    Filter = "Job = '" & sJob & "' and OperNum = " & sOperNum & " and Suffix = " & sSuffix
    '    Propertie = "Rgid, RgrpDESCR"

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLJrtResourceGroups", Propertie, Filter, "", "", 0)

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        ListViewResource.DataSource = ds.Tables(0)
    '        ListViewResource.DataBind()
    '    End If

    'End Sub

    'Function GetCoProductMix(Job As String, Suffix As String) As String

    '    GetCoProductMix = ""

    '    Filter = "Job = '" & Job & "' And Suffix = '" & Suffix & "'"

    '    oWS = New CNIService.DOWebServiceSoapClient

    '    ds = New DataSet

    '    ds = oWS.LoadDataSet(Session("Token").ToString, "SLJobs", "CoProductMix", Filter, "", "", 0)

    '    If ds.Tables(0).Rows.Count > 0 Then
    '        GetCoProductMix = ds.Tables(0).Rows(0)("CoProductMix").ToString
    '    End If

    '    Return GetCoProductMix

    'End Function

#Region "Bind Data To Gridview"

    Sub BindAddResource()


        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "' And Job = '" & lblJob.Text & "' AND Suffix = " & lblSuffix.Text & " AND OperNum = " & lblOper.Text & ""

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_TmpJobStartEndTrans", "RESID, DESCR", Filter, "CreateDate DESC", "", 0)

        'If ds.Tables(0).Rows.Count > 0 Then

        ResourceList.DataSource = ds.Tables(0)
        ResourceList.DataBind()

        'End If


    End Sub

    Sub BindListJob()

        Parms = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                    "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                    "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                    "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_EX_ListJobReleases", "PPCC_Ex_JobOperationReleaseSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)
        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 4 Then
                lblTotalJobOrder.Text = node.InnerText
            ElseIf i = 5 Then
                lblTotalInProcess.Text = node.InnerText
            ElseIf i = 6 Then
                lblTotalNotStart.Text = node.InnerText
            ElseIf i = 7 Then
                lblTotalNotComplete.Text = node.InnerText
            ElseIf i = 8 Then
                lblTotalNotUnposted.Text = node.InnerText
            End If

            i += 1

        Next

        Dim PropertyList As String = ""

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "' AND Username = '" & Session("UserName").ToString & "'"

        PropertyList = "DerJob, Job, Suffix, ItemDesc, OperNum, WCDesc, WC, Item, RESID, Stat"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_ListJobReleases", PropertyList, Filter, "CreateDate DESC", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            PanelList.DataSource = ds.Tables(0)
            PanelList.DataBind()

        End If


    End Sub

    '    Sub BindGridview()

    '        Dim Filter As String
    '        Dim Propertie As String

    '        oWS = New CNIService.DOWebServiceSoapClient
    '        ds = New DataSet
    '        Filter = "SessionID = '" & Session("PSession").ToString & "'"
    '        Propertie = "TagID, Item, FromLoc, ToLoc, Lot, VendLot, Qty"

    '        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_GetJobOperSp", Propertie, Filter, "CreateDate Desc", "", 0)

    '        PanelList.DataSource = ds.Tables(0)
    '        PanelList.DataBind()

    '    End Sub

#End Region

#Region "Get Data Bind To Dropdownlist"

    Sub GetResource()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "Job = '" & lblJob.Text & "' And Suffix = " & lblSuffix.Text & " And OperNum = " & lblOper.Text

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_Resources", "RESID, DESCR", Filter, "RESID", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlResource.Items.Add(New ListItem(dRow("RESID") & IIf(IsDBNull(dRow("DESCR")), "", " : " & dRow("DESCR")), UCase(dRow("RESID").ToString.Trim)))

        Next

        ddlResource.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetReasonCode()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "UserDefinedTypeValues", "Value, Description", "TypeName = 'Reason_End_Job'", "Value", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlReasonCode.Items.Add(New ListItem(dRow("Value") & IIf(IsDBNull(dRow("Description")), "", " : " & dRow("Description")), dRow("Value").ToString.Trim))

        Next

        ddlReasonCode.Items.Insert(0, New ListItem("", ""))

        Dim sReasonDefault As String = ""
        sReasonDefault = GetReasonDefault()

        If sReasonDefault <> "" Then
            ddlReasonCode.SelectedIndex = ddlReasonCode.Items.IndexOf(ddlReasonCode.Items.FindByValue(sReasonDefault))
        End If


    End Sub

#End Region

#Region "Function"

    Function GetResourceName(Job As String, Suffix As String, OperNum As String, RESID As String) As String

        GetResourceName = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "Job = '" & Job & "' And Suffix = " & Suffix & " And OperNum = " & OperNum & " And RESID = '" & RESID & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_EX_Resources", "DESCR", Filter, "RESID", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetResourceName = ds.Tables(0).Rows(0)("DESCR").ToString

        End If


    End Function

    Function GetReasonDefault() As String

        GetReasonDefault = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "TypeName = 'Reason_End_Job' AND Description = 'End Job'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "UserDefinedTypeValues", "Value", Filter, "Value", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetReasonDefault = ds.Tables(0).Rows(0)("Value").ToString

        End If


    End Function

    Function GetReasonDesc(ReasonCode As String) As String

        GetReasonDesc = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        Filter = "TypeName = 'Reason_End_Job' AND Value = '" & ReasonCode & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "UserDefinedTypeValues", "Description", Filter, "Value", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetReasonDesc = ds.Tables(0).Rows(0)("Description").ToString

        End If


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