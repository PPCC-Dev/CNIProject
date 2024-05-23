Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class JobMatlTran
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


        LenPointQty = UnitQtyFormat()

        If Not Page.IsPostBack Then

            DateNow = Date.Now.ToString("dd/MM/yyyy")

            txtdate.Text = DateNow

            PWhse = GetDefWhse()

            If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then

                txtjob.Text = Request.QueryString("Job")
                txtsuffix.Text = Request.QueryString("Suffix")
                txtOperNum.Text = Request.QueryString("OperNum")

                txtWC.Text = GetJobMatlWC(txtjob.Text, txtsuffix.Text, txtOperNum.Text)

                If Session("Stat").ToString = "Return" Then
                    btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
                    btnstat.Text = "<i class=""fa fa-redo-alt"" aria-hidden=""true""></i>" & " <strong>Return</strong>"

                Else
                    btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
                    btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Issue</strong>"
                End If

                If Session("MsgErr") Is Nothing Then

                    If Session("LabelJobmatl").ToString = "Scan Tag: " Then
                        btnprocess.Attributes.Remove("disabled")
                    End If
                Else

                    btnprocess.Attributes.Remove("disabled")

                End If

                BindGridview()

            Else

                Session("Stat") = "Issue"

            End If


        End If

        If Session("LabelJobmatl") Is Nothing Then
            lblbarcode.Text = "Scan Job Order: "
        Else
            If txtjob.Text = String.Empty Then
                lblbarcode.Text = "Scan Job Order: "
            Else
                lblbarcode.Text = Session("LabelJobmatl").ToString

            End If

        End If

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, strJob, strSuffix, strOperNum As String
        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        strJob = ""
        strSuffix = ""
        strOperNum = ""

        'Try

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Scan Job Order: " Then


                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "J" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter>" & PWhse & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                            "<Parameter>" & "I" & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Scan Tag: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter>" & PWhse & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                            "<Parameter>" & "I" & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

            ElseIf lblbarcode.Text = "Scan Reason Code: " Then

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & "T" & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter>" & PWhse & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & "" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & "0" & "</Parameter>" &
                            "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                            "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                            "<Parameter>" & "O" & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

            End If

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 15 Then
                    Stat = node.InnerText

                ElseIf i = 16 Then
                    MsgType = node.InnerText

                ElseIf i = 17 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If Stat = "TRUE" Then

                If lblbarcode.Text = "Scan Job Order: " Then

                    Dim Position1 As Integer
                    Dim Position2 As Integer

                    Position1 = InStr(sBarcode, "|")
                    Position2 = InStr(sBarcode.Substring(Position1), "|")

                    If Position1 <> 0 And Position2 <> 0 Then

                        Dim arrBarcode As String()
                        arrBarcode = sBarcode.Split(New Char() {"|"c})

                        If arrBarcode.Length > 0 Then

                            strJob = arrBarcode(0)
                            strSuffix = arrBarcode(1)
                            strOperNum = arrBarcode(2)

                        End If

                    End If

                    txtjob.Text = strJob
                    txtsuffix.Text = strSuffix
                    txtOperNum.Text = strOperNum

                    txtWC.Text = GetJobMatlWC(txtjob.Text, txtsuffix.Text, txtOperNum.Text)

                    lblbarcode.Text = "Scan Tag: "
                    Session("LabelJobmatl") = "Scan Tag: "

                ElseIf lblbarcode.Text = "Scan Tag: " Then

                    lblbarcode.Text = "Scan Tag: "
                    Session("LabelJobmatl") = "Scan Tag: "

                ElseIf lblbarcode.Text = "Scan Reason Code: " Then


                    lblbarcode.Text = "Scan Tag: "
                    Session("LabelJobmatl") = "Scan Tag: "
                    btnprocess.Attributes.Remove("disabled")

                End If

                If MsgErr = "NEXT" Then

                    lblbarcode.Text = "Scan Reason Code: "
                    Session("LabelJobmatl") = "Scan Reason Code: "
                    Session("MsgErr") = "NEXT"

                End If

                If Session("LabelJobmatl").ToString = "Scan Tag: " And MsgErr = "" And PanelList.Items.Count > 0 Then
                    btnprocess.Attributes.Remove("disabled")
                End If


                BindGridview()

            ElseIf Stat = "FALSE" Then

                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                'NotPassNotifyPanel.Visible = True
                'NotPassText.Text = MsgErr


            End If



        End If

        txtbarcode.Text = String.Empty
        chkCancelTag.Checked = False

    End Sub

    Protected Sub btnstat_Click(sender As Object, e As EventArgs) Handles btnstat.Click

        If PanelList.Items.Count = 0 Then
            PanelList.DataSource = Nothing
            PanelList.DataBind()
        Else
            Clearwhenclickchangedstat()
        End If

        If Session("Stat").ToString = "Issue" Then
            btnstat.CssClass = "btn btn btn-outline-danger btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-redo-alt"" aria-hidden=""true""></i>" & " <strong>Return</strong>"
            Session("Stat") = "Return"

        Else
            btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
            btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Issue</strong>"
            Session("Stat") = "Issue"
        End If

    End Sub

    Protected Sub btnprocess_Click(sender As Object, e As EventArgs) Handles btnprocess.Click

        Dim sBarcode, Stat, MsgErr, MsgType As String
        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "P" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 15 Then
                Stat = node.InnerText

            ElseIf i = 16 Then
                MsgType = node.InnerText

            ElseIf i = 17 Then
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

    Protected Sub btnreset_Click(sender As Object, e As EventArgs) Handles btnreset.Click

        Dim Stat, MsgErr, MsgType As String
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "R" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 15 Then
                Stat = node.InnerText

            ElseIf i = 16 Then
                MsgType = node.InnerText

            ElseIf i = 17 Then
                MsgErr = node.InnerText

            End If

            i += 1

        Next

        If Stat = "TRUE" Then

            Session("PSession") = NewSessionID()
            Session.Remove("LabelJobmatl")
            Session.Remove("MsgErr")
            Response.Redirect("JobMatlTran.aspx")

        Else

            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            MsgType = "Error [" & MsgType & "]"

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

        End If

    End Sub

    Sub Clear()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        Session("Stat") = "Issue"
        btnstat.CssClass = "btn btn-outline-success btn-block btn-sm"
        btnstat.Text = "<i class=""fa fa-arrow-right"" aria-hidden=""true""></i>" & " <strong>Issue</strong>"
        lblbarcode.Text = "Scan Job Order: "

        chkCancelTag.Checked = False
        txtbarcode.Text = String.Empty
        txtjob.Text = String.Empty
        txtsuffix.Text = String.Empty
        txtOperNum.Text = String.Empty

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        Session("PSession") = NewSessionID()

        lblbarcode.Text = "Scan Job Order: "
        Session.Remove("LabelJobmatl")
        Session.Remove("MsgErr")
    End Sub

    Sub Clearwhenclickchangedstat()
        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        lblbarcode.Text = "Scan Job Order: "
        Session.Remove("LabelJobmatl")
        Session.Remove("MsgErr")

        chkCancelTag.Checked = False
        txtbarcode.Text = String.Empty
        txtjob.Text = String.Empty
        txtsuffix.Text = String.Empty
        txtOperNum.Text = String.Empty

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        Session("PSession") = NewSessionID()
    End Sub

    Protected Sub btndetail_Click(sender As Object, e As EventArgs) Handles btndetail.Click
        Dim PostURL As String = ""
        PostURL = "?Job=" & txtjob.Text & "&Suffix=" & txtsuffix.Text & "&OperNum=" & txtOperNum.Text & "&SessionID=" & Session("PSession").ToString & "&Type=" & Session("Stat").ToString & ""
        Response.Redirect("JobMatlTranDetail.aspx" & PostURL)
    End Sub

    Protected Sub ListView1_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        Dim QtyReq As Double = 0
        Dim QtyIssue As Double = 0
        Dim QtyRemain As Double = 0
        Dim QtySum As Double = 0
        Dim ScanTagJobMatl As String = ""

        If e.Item.ItemType = ListViewItemType.DataItem Then

            If Session("Stat").ToString = "Issue" Then

                Dim lblQtyRequire As Label = CType(e.Item.FindControl("lblQtyRequire"), Label)
                Dim lblQtyRemain As Label = CType(e.Item.FindControl("lblQtyRemain"), Label)
                Dim lblListQtyReq As Label = CType(e.Item.FindControl("lblListQtyReq"), Label)
                Dim lblListRemain As Label = CType(e.Item.FindControl("lblListRemain"), Label)

                lblQtyRequire.Visible = True
                lblQtyRemain.Visible = True
                lblListQtyReq.Visible = True
                lblListRemain.Visible = True

                QtyReq = CDec(lblListQtyReq.Text)
                QtyRemain = CDec(lblListRemain.Text)

                lblListQtyReq.Text = FormatNumber(QtyReq.ToString, LenPointQty)
                lblListRemain.Text = FormatNumber(QtyRemain.ToString, LenPointQty)
            Else

                Dim lblReason As Label = CType(e.Item.FindControl("lblReason"), Label)
                Dim lblLoc As Label = CType(e.Item.FindControl("lblLoc"), Label)
                Dim lblLot As Label = CType(e.Item.FindControl("lblLot"), Label)
                Dim lblListReason As Label = CType(e.Item.FindControl("lblListReason"), Label)
                Dim lblListLoc As Label = CType(e.Item.FindControl("lblListLoc"), Label)
                Dim lblListLot As Label = CType(e.Item.FindControl("lblListLot"), Label)

                lblReason.Visible = True
                lblLoc.Visible = True
                lblLot.Visible = True
                lblListReason.Visible = True
                lblListLoc.Visible = True
                lblListLot.Visible = True

            End If


            Dim lblListQtyIssue As Label = CType(e.Item.FindControl("lblListQtyIssue"), Label)
            Dim lblListQtySum As Label = CType(e.Item.FindControl("lblListQtySum"), Label)
            Dim lblScanTagJobMalt As Label = CType(e.Item.FindControl("lblScanTagJobMalt"), Label)
            Dim lnkIssueQty As LinkButton = CType(e.Item.FindControl("lnkIssueQty"), LinkButton)

            QtyIssue = CDec(IIf(lblListQtyIssue.Text = "", 0, lblListQtyIssue.Text))
            QtySum = CDec(IIf(lblListQtySum.Text = "", 0, lblListQtySum.Text))


            lblListQtyIssue.Text = FormatNumber(QtyIssue.ToString, LenPointQty)
            lblListQtySum.Text = FormatNumber(QtySum.ToString, LenPointQty)

            If lblScanTagJobMalt.Text = "0" Then
                lblScanTagJobMalt.Visible = False
                lnkIssueQty.Visible = True
            ElseIf lblScanTagJobMalt.Text = "1" Then
                lblScanTagJobMalt.Visible = True
                lblScanTagJobMalt.Text = "Scan Tag"
                lnkIssueQty.Visible = False
            End If

        End If

    End Sub

    Protected Sub PanelList_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList.ItemCommand

        If e.CommandName = "LinkIssueQty" Then

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & IIf(chkCancelTag.Checked = True, 1, 0) & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "Q" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & TryCast(e.Item.FindControl("lblListItem"), Label).Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

            Dim PostURL As String = ""
            PostURL = "?Job=" & txtjob.Text & "&Suffix=" & txtsuffix.Text & "&OperNum=" & txtOperNum.Text & "&SessionID=" & Session("PSession").ToString & "&Type=" & Session("Stat").ToString & ""
            PostURL = PostURL & "&Item=" & TryCast(e.Item.FindControl("lblListItem"), Label).Text & "&QtyRequire=" & TryCast(e.Item.FindControl("lblListQtyReq"), Label).Text & ""
            PostURL = PostURL & "&QtyIssued=" & TryCast(e.Item.FindControl("lblListQtyIssue"), Label).Text

            Response.Redirect("JobMatlTranIssueQty.aspx" & PostURL)

        End If


    End Sub


#Region "Bind Data To Gridview"

    Sub BindGridview()

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Filter = "SessionID = '" & Session("PSession").ToString & "'"
        Propertie = "Oper, Item, QtyRequire, QtyIssue, QtyRemain, QtySum, Reason, Loc, Lot, ScanTag"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobMatlSum", Propertie, Filter, "CreateDate Desc", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            PanelList.DataSource = ds.Tables(0)
            PanelList.DataBind()
        End If


    End Sub

#End Region

#Region "Function"

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

    Function GetJobMatlWC(Job As String, Suffix As String, OperNum As String) As String

        GetJobMatlWC = ""
        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet

        Filter = "Job = '" & Job & "' AND Suffix = " & Suffix & " AND OperNum = " & OperNum

        ds = oWS.LoadDataSet(Session("Token"), "SLJobmatls", "JbrWc", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetJobMatlWC = ds.Tables(0).Rows(0)("JbrWc").ToString
        End If

        Return GetJobMatlWC

    End Function

#End Region

End Class