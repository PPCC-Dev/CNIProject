Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class JobMatlTranIssueQty
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim ds1 As DataSet
    Dim Filter As String
    Dim Parms As String
    Dim Propertie As String
    Dim LenPointQty As Integer = 0
    Dim QtyReq As Decimal = 0
    Dim QtyIssued As Decimal = 0
    Dim Qty As Decimal = 0
    Dim sLot As String
    Dim sLoc As String

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

            QtyReq = 0
            QtyIssued = 0
            Qty = 0

            PWhse = GetDefWhse()

            DerPreassignLots.Value = GetDerPreassignLots(Request.QueryString("Item"))

            GetLoc()

            lblItem.Text = Request.QueryString("Item")
            lblItemDesc.Text = GetItemDesc(Request.QueryString("Item"))
            lblQtyRequired.Text = Request.QueryString("QtyRequire")
            lblQtyIssue.Text = Request.QueryString("QtyIssued")
            lblQuantity.Text = GetQtyIsuued(Request.QueryString("Item"))

            Decimal.TryParse(lblQtyRequired.Text, QtyReq)
            Decimal.TryParse(lblQtyIssue.Text, QtyIssued)
            Decimal.TryParse(lblQuantity.Text, Qty)

            txtQty.Text = FormatNumber(QtyReq - QtyIssued - Qty, LenPointQty)


            sLoc = GetLoc(Request.QueryString("Item"))
            sLot = GetLot(Request.QueryString("Item"))

            If Not String.IsNullOrEmpty(sLoc) Or sLoc <> "" Then

                If ddlloc.Items.Count > 0 Then
                    ddlloc.SelectedIndex = ddlloc.Items.IndexOf(ddlloc.Items.FindByValue(sLoc))
                    lblLoc.Text = sLoc

                    GetLot(ddlloc.SelectedItem.Value,
                            PWhse.ToString,
                            Request.QueryString("Item"),
                            Request.QueryString("Job"),
                            Request.QueryString("Suffix"),
                            Request.QueryString("OperNum"))
                Else
                    ddlloc.Items.Insert(0, New ListItem("", ""))
                End If

            Else
                If ddlloc.Items.Count > 0 Then
                    ddlloc.SelectedIndex = ddlloc.Items.IndexOf(ddlloc.Items.FindByValue(""))
                    lblLoc.Text = ""
                Else
                    ddlloc.Items.Insert(0, New ListItem("", ""))
                End If

            End If

            If Not String.IsNullOrEmpty(sLot) Or sLot <> "" Then

                If ddllot.Items.Count > 0 Then
                    ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(sLot))
                    lblLot.Text = sLot

                    lblQtyOnHand.Text = GetOnHandLot(lblLoc.Text,
                                                     PWhse.ToString,
                                                     lblItem.Text,
                                                     lblLot.Text)

                Else
                    ddllot.Items.Insert(0, New ListItem("", ""))
                End If

            Else
                If ddllot.Items.Count > 0 Then
                    ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))
                    lblLot.Text = ""
                Else
                    ddllot.Items.Insert(0, New ListItem("", ""))
                End If

            End If
        End If

            txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, Type, Item As String
        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        Type = ""
        Item = ""

        If txtbarcode.Text <> "" Then

            If lblbarcode.Text = "Scan Location: " Then

                Type = Request.QueryString("Type")
                Item = Request.QueryString("Item")

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & "L" & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & IIf(Type = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "I" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & Item & "</Parameter>" &
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

                If lblbarcode.Text = "Scan Location: " Then

                    ddlloc.SelectedIndex = ddlloc.Items.IndexOf(ddlloc.Items.FindByValue(sBarcode))

                    GetLot(ddlloc.SelectedItem.Value,
                           PWhse.ToString,
                           Request.QueryString("Item"),
                           Request.QueryString("Job"),
                           Request.QueryString("Suffix"),
                           Request.QueryString("OperNum"))

                    lblLoc.Text = ddlloc.SelectedItem.Value

                End If

            ElseIf Stat = "FALSE" Then

                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)


            End If

        End If

        txtbarcode.Text = String.Empty

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click

        Dim PostURL As String = ""
        Dim Job, Suffix, OperNum, Type As String

        Job = Request.QueryString("Job")
        Suffix = Request.QueryString("Suffix")
        OperNum = Request.QueryString("OperNum")
        Type = Request.QueryString("Type")

        PostURL = "?Job=" & Job & "&Suffix=" & Suffix & "&OperNum=" & OperNum & "&SessionID=" & Session("PSession").ToString & "&Type=" & Type & ""

        Response.Redirect("JobMatlTran.aspx" & PostURL)
    End Sub

    Protected Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Dim PostURL As String = ""
        Dim Job, Suffix, OperNum, Type As String

        Job = Request.QueryString("Job")
        Suffix = Request.QueryString("Suffix")
        OperNum = Request.QueryString("OperNum")
        Type = Request.QueryString("Type")

        PostURL = "?Job=" & Job & "&Suffix=" & Suffix & "&OperNum=" & OperNum & "&SessionID=" & Session("PSession").ToString & "&Type=" & Type & ""

        Response.Redirect("JobMatlTran.aspx" & PostURL)

    End Sub

    Protected Sub ddlloc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlloc.SelectedIndexChanged

        If ddlloc.SelectedItem.Value <> "" Then

            GetLot(ddlloc.SelectedItem.Value,
                    PWhse.ToString,
                    Request.QueryString("Item"),
                    Request.QueryString("Job"),
                    Request.QueryString("Suffix"),
                    Request.QueryString("OperNum"))

            lblLoc.Text = ddlloc.SelectedItem.Value


        End If

    End Sub

    Protected Sub ddllot_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddllot.SelectedIndexChanged

        If ddllot.SelectedItem.Value <> "" Then

            lblLot.Text = ddllot.SelectedItem.Value

            lblQtyOnHand.Text = GetOnHandLot(lblLoc.Text,
                                             PWhse.ToString,
                                             lblItem.Text,
                                             lblLot.Text)
        End If

    End Sub

    Protected Sub btnconfirm_Click(sender As Object, e As EventArgs) Handles btnconfirm.Click

        If ddlloc.Items.Count > 0 Then

            If ddlloc.SelectedItem.Value = "" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error [PPCC]','#318 : กรุณาสแกน/เลือก Location', 'error');", True)
                Exit Sub
            End If

        End If

        If ddllot.Items.Count > 0 Then

            If ddllot.SelectedItem.Value = "" Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error [PPCC]','#319 : กรุณาเลือก Lot', 'error');", True)
                Exit Sub
            End If

        End If

        If ddlloc.Items.Count = 0 Then
            ddlloc.Items.Insert(0, New ListItem("", ""))
        End If

        If ddllot.Items.Count = 0 Then
            ddllot.Items.Insert(0, New ListItem("", ""))
        End If

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & txtQty.Text & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "C" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblItem.Text & "</Parameter>" &
                        "<Parameter>" & ddllot.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

            QtyReq = 0
            QtyIssued = 0
            Qty = 0

            lblQuantity.Text = GetQtyIsuued(Request.QueryString("Item"))

            Decimal.TryParse(lblQtyRequired.Text, QtyReq)
            Decimal.TryParse(lblQtyIssue.Text, QtyIssued)
            Decimal.TryParse(lblQuantity.Text, Qty)

            txtQty.Text = FormatNumber(QtyReq - QtyIssued - Qty, LenPointQty)

    End Sub

    Protected Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click

        If ddlloc.Items.Count = 0 Then
            ddlloc.Items.Insert(0, New ListItem("", ""))
        End If

        If ddllot.Items.Count = 0 Then
            ddllot.Items.Insert(0, New ListItem("", ""))
        End If

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & PWhse & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & "0" & "</Parameter>" &
                        "<Parameter>" & 0 & "</Parameter>" &
                        "<Parameter>" & IIf(Session("Stat").ToString = "Issue", "I", "W") & "</Parameter>" &
                        "<Parameter>" & "X" & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & lblItem.Text & "</Parameter>" &
                        "<Parameter>" & ddllot.SelectedItem.Value & "</Parameter>" &
                        "<Parameter>" & ddlloc.SelectedItem.Value & "</Parameter>" &
                        "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "PPCC_Ex_JobMatlSp", Parms)

        ddlloc.SelectedIndex = ddlloc.Items.IndexOf(ddlloc.Items.FindByValue(""))
        ddllot.SelectedIndex = ddllot.Items.IndexOf(ddllot.Items.FindByValue(""))

        Dim Job As String = Request.QueryString("Job")
        Dim Suffix As String = Request.QueryString("Suffix")
        Dim OperNum As String = Request.QueryString("OperNum")
        Dim Item As String = Request.QueryString("Item")
        Dim QtyRequire As String = Request.QueryString("QtyRequire")
        Dim QtyIssued As String = Request.QueryString("QtyIssued")

        Dim PostURL As String = ""
        PostURL = "?Job=" & Job & "&Suffix=" & Suffix & "&OperNum=" & OperNum & "&SessionID=" & Session("PSession").ToString & "&Type=" & Session("Stat").ToString & ""
        PostURL = PostURL & "&Item=" & Item & "&QtyRequire=" & QtyRequire & "&QtyIssued=" & QtyIssued

        Response.Redirect("JobMatlTranIssueQty.aspx" & PostURL)

    End Sub


#Region "Get Data Bind To Dropdownlist"

    Sub GetLoc()

        ddlloc.Items.Clear()

        Dim Item, Loc, UserId As String

        UserId = GetUserId(Session("UserName").ToString)

        Item = Request.QueryString("Item")

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItemLocs", "Loc", "Item = '" & Item & "' AND Whse = '" & PWhse & "' AND QtyOnHand > 0", "Loc", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                Loc = ds.Tables(0).Rows(i)("Loc").ToString

                Filter = "LocId = '" & Loc & "' AND UserId = " & UserId

                oWS = New CNIService.DOWebServiceSoapClient

                ds1 = New DataSet

                ds1 = oWS.LoadDataSet(Session("Token").ToString, "PPCC_UserLocations", "LocId", Filter, "LocId", "", 0)

                If ds1.Tables(0).Rows.Count > 0 Then

                    ddlloc.Items.Add(New ListItem(Loc, Loc))

                End If

            Next

        End If

        ddlloc.Items.Insert(0, New ListItem("", ""))

    End Sub

    Sub GetLot(Loc As String, Whse As String, Item As String, Job As String, Suffix As String, OperNum As String)

        ddllot.Items.Clear()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        If DerPreassignLots.Value = "0" Then

            Filter = "Loc = '" & Loc & "' AND Item = '" & Item & "' AND Whse = '" & Whse & "' AND QtyOnHand > 0"

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "Lot", Filter, "Lot", "", 0)

        ElseIf DerPreassignLots.Value = "1" Then

            Filter = "RefType = 'J' AND Item = '" & Item & "' AND RefNum = '" & Job & "' AND RefLineSuf = " & Suffix & " AND RefRelease = " & OperNum

            ds = oWS.LoadDataSet(Session("Token").ToString, "SLPreassignedLots", "Lot", Filter, "Lot", "", 0)

        End If

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddllot.Items.Add(New ListItem(dRow("Lot"), dRow("Lot")))

        Next

        ddllot.Items.Insert(0, New ListItem("", ""))

    End Sub


#End Region

#Region "Function"

    Function GetUserId(UserName As String) As String

        GetUserId = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserId", "Username='" & UserName & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetUserId = ds.Tables(0).Rows(0)("UserId").ToString
        End If

        Return GetUserId

    End Function

    Function GetItemDesc(Item As String) As String

        GetItemDesc = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLItems", "Description", "Item='" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetItemDesc = ds.Tables(0).Rows(0)("Description").ToString
        End If

        Return GetItemDesc

    End Function

    Function GetOnHandLot(Loc As String, Whse As String, Item As String, Lot As String) As String

        GetOnHandLot = "0"
        Dim QtyOnHand As Decimal = 0
        GetOnHandLot = FormatNumber(GetOnHandLot, LenPointQty)

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        Filter = "Item = '" & Item & "' AND Whse = '" & Whse & "' AND Loc = '" & Loc & "' AND Lot = '" & Lot & "'"
        ds = oWS.LoadDataSet(Session("Token").ToString, "SLLotLocs", "QtyOnHand", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Decimal.TryParse(ds.Tables(0).Rows(0)("QtyOnHand").ToString, QtyOnHand)
            GetOnHandLot = FormatNumber(QtyOnHand, LenPointQty)
        End If

        Return GetOnHandLot

    End Function

    Function GetQtyIsuued(Item As String) As String

        GetQtyIsuued = "0"
        Dim QtyIssued As Decimal = 0
        GetQtyIsuued = FormatNumber(GetQtyIsuued, LenPointQty)

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "QtyIssue", "SessionID = '" & Session("PSession").ToString & "' AND Item='" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            Decimal.TryParse(ds.Tables(0).Rows(0)("QtyIssue").ToString, QtyIssued)
            GetQtyIsuued = FormatNumber(QtyIssued, LenPointQty)
        End If

        Return GetQtyIsuued

    End Function

    Function GetLoc(Item As String) As String

        GetLoc = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "Loc", "SessionID = '" & Session("PSession").ToString & "' AND Item='" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetLoc = ds.Tables(0).Rows(0)("Loc").ToString
        End If

        Return GetLoc

    End Function

    Function GetLot(Item As String) As String

        GetLot = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobMatlDetail", "Lot", "SessionID = '" & Session("PSession").ToString & "' AND Item='" & Item & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetLot = ds.Tables(0).Rows(0)("Lot").ToString
        End If

        Return GetLot

    End Function

    Function GetDerPreassignLots(Item As String) As String

        GetDerPreassignLots = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient

        Filter = "SessionID = '" & Session("PSession").ToString & "' AND Item = '" & Item & "'"

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_JobMatlSum", "DerPreassignLots", Filter, "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetDerPreassignLots = ds.Tables(0).Rows(0)("DerPreassignLots").ToString
        End If

        Return GetDerPreassignLots

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

#End Region

End Class