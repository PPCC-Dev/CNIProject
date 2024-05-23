Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class CycleCount
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim ds2 As DataSet
    Dim Filter2 As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim DateNow As String
    Dim LenPointQty As Integer = 0

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

            'DateNow = Date.Now.ToString("dd/MM/yyyy")

            'txtdate.Text = DateNow

            GetWarehouse()
            'GetLoc()
            GetProductCode()
            If Request.QueryString("SessionID") <> "" Or Not String.IsNullOrEmpty(Request.QueryString("SessionID")) Then
                '    SGUID = System.Guid.NewGuid.ToString()
                '    Session("PSession") = SGUID
                'Else
                ddlwhse.SelectedIndex = ddlwhse.Items.IndexOf(ddlwhse.Items.FindByValue(Request.QueryString("Whse").ToString))
                ddlProductCode.SelectedIndex = ddlProductCode.Items.IndexOf(ddlProductCode.Items.FindByValue(Request.QueryString("ProductCode").ToString))
                ddlLoc.SelectedIndex = ddlLoc.Items.IndexOf(ddlLoc.Items.FindByValue(Request.QueryString("Location").ToString))
            End If

            BindGridview()

        End If

        DateNow = Date.Now.ToString("dd/MM/yyyy")

        txtdate.Text = DateNow

        txtbarcode.Focus()

        txtbarcode.Attributes.Add("onchange", "javascript:scanbarcode();")

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType, Prompt As String
        sBarcode = txtbarcode.Text
        Stat = "TRUE"
        MsgErr = ""
        MsgType = ""
        Prompt = ""

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

        If txtbarcode.Text <> "" And txtprocess.Text <> "I" Then

            ValidateDataBeforeProcess(sBarcode, ddlwhse.SelectedItem.Value, ddlProductCode.SelectedItem.Value, Stat, MsgType, MsgErr, Prompt)

            If Stat = "FALSE" Then

                If Prompt = "Error" Then

                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

                    txtbarcode.Text = String.Empty

                ElseIf Prompt = "Prompt" Then


                    MsgErr = MsgErr.Replace("'", "\'")
                    MsgErr = MsgErr.Replace(vbLf, "<br />")

                    MsgType = "Error [" & MsgType & "]"

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & Prompt & "','" & MsgErr & "', 'question');", True)


                End If

            Else

                Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & sBarcode & "</Parameter>" &
                            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
                            "<Parameter>" & 0 & "</Parameter>" &
                            "<Parameter>" & "P" & "</Parameter>" &
                            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "PPCC_Ex_CycleCountSp", Parms)

                BindGridview()
                txtbarcode.Text = String.Empty

            End If



        End If

        If txtbarcode.Text <> "" And txtprocess.Text = "I" Then

            Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
            "<Parameter>" & sBarcode & "</Parameter>" &
            "<Parameter>" & DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd") & "</Parameter>" &
            "<Parameter>" & 0 & "</Parameter>" &
            "<Parameter>" & "I" & "</Parameter>" &
            "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
            "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "PPCC_Ex_CycleCountSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 8 Then
                    Stat = node.InnerText

                ElseIf i = 9 Then
                    MsgType = node.InnerText

                ElseIf i = 10 Then
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
                txtprocess.Text = String.Empty

            Else

                BindGridview()
                txtbarcode.Text = String.Empty
                txtprocess.Text = String.Empty

            End If



        End If



    End Sub


    Protected Sub ddlProductCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProductCode.SelectedIndexChanged
        PanelList.DataSource = Nothing
        PanelList.DataBind()
        BindGridview()
    End Sub

    Protected Sub ddlLoc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLoc.SelectedIndexChanged
        PanelList.DataSource = Nothing
        PanelList.DataBind()
        BindGridview()
    End Sub


    Sub Clear()

        DateNow = Date.Now.ToString("dd/MM/yyyy")
        txtdate.Text = DateNow

        txtbarcode.Text = String.Empty
        txtprocess.Text = String.Empty

        PanelList.DataSource = Nothing
        PanelList.DataBind()

        'GridView1.DataSource = Nothing
        'GridView1.DataBind()


    End Sub

    Protected Sub PanelList_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim lblRemain As Label = CType(e.Item.FindControl("lblremain"), Label)
            Dim lblqty As Label = CType(e.Item.FindControl("lblqty"), Label)
            Dim lblqtycount As Label = CType(e.Item.FindControl("lblqtycount"), Label)
            Dim strQtyCount As String = IIf(lblqtycount.Text = "&nbsp;", "0", IIf(lblqtycount.Text = "", "0", lblqtycount.Text))

            Dim lnkTagDetail As LinkButton = CType(e.Item.FindControl("lnkTagDetail"), LinkButton)
            Dim lnkCount As LinkButton = CType(e.Item.FindControl("lnkCount"), LinkButton)
            Dim lblstat As Label = CType(e.Item.FindControl("lblstat"), Label)
            Dim chkNonScanTag As CheckBox = CType(e.Item.FindControl("chkNonScanTag"), CheckBox)

            Dim Qty As Decimal = CDec(lblqty.Text)
            Dim QtyCount As Decimal = CDec(strQtyCount.ToString)

            lblqty.Text = FormatNumber(lblqty.Text, LenPointQty)
            lblqtycount.Text = FormatNumber(QtyCount, LenPointQty)
            lblRemain.Text = FormatNumber((Qty - QtyCount), LenPointQty)

            If lblstat.Text = "N" Then
                lblstat.Text = "Not Counted"
                lblstat.CssClass = "text-danger"
            ElseIf lblstat.Text = "E" Then
                lblstat.Text = "Exception"
            ElseIf lblstat.Text = "C" Then
                lblstat.Text = "Counted"
                lblstat.CssClass = "text-success"
            ElseIf lblstat.Text = "P" Then
                lblstat.Text = "Posted"
            End If

            If chkNonScanTag.Checked = False Then
                lnkTagDetail.Visible = True
                lnkCount.Text = "Revise Tag Qty"
            Else
                lnkTagDetail.Visible = False
                lnkCount.Text = "Count Qty"
            End If


        End If

    End Sub


    Sub Display(ByVal sender As Object, ByVal e As EventArgs)

        'Dim rowIndex As Integer = Convert.ToInt32(TryCast(TryCast(sender, LinkButton).NamingContainer, GridViewRow).RowIndex)
        'Dim row As GridViewRow = GridView1.Rows(rowIndex)


        'Dim PostURL As String = ""

        'PostURL = "?SessionID=" & Session("PSession").ToString & "&Item=" & TryCast(row.FindControl("lblItem"), Label).Text & "&Lot=" & TryCast(row.FindControl("lblLot"), Label).Text & ""
        'PostURL = PostURL & "&Loc=" & ddlloc.SelectedItem.Value & "&Whse=" & ddlwhse.SelectedItem.Value & ""

        'Response.Redirect("CycleCountDetail.aspx" & PostURL)

    End Sub

    Protected Sub PanelList_ItemCommand(ByVal sender As Object, ByVal e As ListViewCommandEventArgs) Handles PanelList.ItemCommand

        If e.CommandName = "TagDetail" Then

            Dim PostURL As String = ""

            PostURL = "?SessionID=" & Session("PSession").ToString & "&Item=" & TryCast(e.Item.FindControl("lblItem"), Label).Text & "&Lot=" & TryCast(e.Item.FindControl("lblLot"), Label).Text & ""
            PostURL = PostURL & "&Loc=" & TryCast(e.Item.FindControl("lblLoc"), Label).Text & "&Whse=" & ddlwhse.SelectedItem.Value & ""
            PostURL = PostURL & "&ProductCode=" & ddlProductCode.SelectedItem.Value & "&Location=" & ddlLoc.SelectedItem.Value & ""

            Response.Redirect("CycleCountDetail.aspx" & PostURL)

        End If

        If e.CommandName = "Counted" Then

            Dim PostURL As String = ""
            PostURL = "?SessionID=" & Session("PSession").ToString & "&Item=" & TryCast(e.Item.FindControl("lblItem"), Label).Text & "&Lot=" & TryCast(e.Item.FindControl("lblLot"), Label).Text & ""
            PostURL = PostURL & "&Loc=" & TryCast(e.Item.FindControl("lblLoc"), Label).Text & "&Whse=" & ddlwhse.SelectedItem.Value & ""
            PostURL = PostURL & "&ProductCode=" & ddlProductCode.SelectedItem.Value & "&Location=" & ddlLoc.SelectedItem.Value & ""

            Dim chkNonScanTag As CheckBox
            chkNonScanTag = TryCast(e.Item.FindControl("chkNonScanTag"), CheckBox)

            If chkNonScanTag.Checked = True Then
                Response.Redirect("CycleCountQty.aspx" & PostURL)
            Else
                Response.Redirect("CycleCountReviseTagQty.aspx" & PostURL)
            End If


        End If



    End Sub

    Sub ValidateDataBeforeProcess(ByVal TagID As String, ByVal Whse As String, ByVal ProductCode As String,
                                  ByRef Stat As String, ByRef MsgType As String, ByRef Msg As String, ByRef Prompt As String)

        Dim oWhse As String = ""
        Dim oProductCode As String = ""
        Dim oStat As String = ""



        '#### Check Tag
        '#### Check Whse and Location
        '#### Check Tag have data within cyclecount tag and Item have in Cycle Master

        Stat = "TRUE"
        MsgType = ""
        Msg = ""
        Prompt = "Error"

        oWS = New CNIService.DOWebServiceSoapClient

        Parms = "<Parameters><Parameter>" & Session("PSession").ToString & "</Parameter>" &
                            "<Parameter>" & TagID & "</Parameter>" &
                            "<Parameter>" & Whse & "</Parameter>" &
                            "<Parameter>" & ProductCode & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "</Parameters>"

        oWS = New CNIService.DOWebServiceSoapClient
        oWS.CallMethod(Session("Token").ToString, "PPCC_SLCycles", "PPCC_Ex_CheckValidateCycleCountSp", Parms)


        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1

        For Each node As XmlNode In doc.DocumentElement

            If i = 5 Then
                Stat = node.InnerText

            ElseIf i = 6 Then
                MsgType = node.InnerText

            ElseIf i = 7 Then
                Msg = node.InnerText

            ElseIf i = 8 Then
                Prompt = node.InnerText

            End If

            i += 1

        Next


        'Dim Filter As String

        'If Stat = "TRUE" Then

        '    ds = New DataSet

        '    Filter = "tag_id = '" & TagID & "'"

        '    ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "tag_id, item, loc, lot, count_qty, item_description, stat, ProductCode, whse ", Filter, "", "", 0)

        '    If ds.Tables(0).Rows.Count = 0 Then
        '        Stat = "FALSE"
        '        MsgType = "PPCC"
        '        Msg = "#403 : ไม่พบเลขที่ Tag"
        '        Prompt = "Error"

        '    ElseIf ds.Tables(0).Rows.Count > 0 Then

        '        oWhse = ds.Tables(0).Rows(0)("whse").ToString
        '        oProductCode = ds.Tables(0).Rows(0)("ProductCode").ToString

        '        If oWhse <> Whse Or oProductCode <> ProductCode Then
        '            Stat = "FALSE"
        '            MsgType = "PPCC"
        '            Msg = "#404 : ไม่พบเลขที่ Tag ใน Warehouse : [" & Whse & "] และ Location : [" & ProductCode & "]"
        '            Prompt = "Error"

        '        End If



        '    End If


        'End If

        'If Stat = "TRUE" Then

        '    Dim oItem As String = ""

        '    ds = New DataSet

        '    Filter = "TagID = '" & TagID & "'"

        '    ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Tags", "Item", Filter, "", "", 0)

        '    If ds.Tables(0).Rows.Count > 0 Then
        '        oItem = ds.Tables(0).Rows(0)("Item").ToString
        '    End If

        '    If oItem <> "" Then

        '        ds = New DataSet

        '        Filter = "tag_id = '" & TagID & "'"

        '        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "tag_id,stat", Filter, "", "", 0)

        '        If ds.Tables(0).Rows.Count = 0 Then

        '            ds = New DataSet

        '            Filter = "Item = '" & oItem & "'"

        '            ds = oWS.LoadDataSet(Session("Token").ToString, "SLCycles", "Item", Filter, "", "", 0)

        '            If ds.Tables(0).Rows.Count > 0 Then

        '                Stat = "FALSE" '
        '                MsgType = "PPCC"
        '                Msg = "#405 : ไม่พบเลขที่ Tag [" & TagID & "] ในการทำ Cycle Count Tag"
        '                Prompt = "Prompt"

        '            Else

        '                Stat = "FALSE" '
        '                MsgType = "PPCC"
        '                Msg = "#406 : ไม่พบรหัส Item [" & oItem & "] ในการทำ Cycle Count Tag"
        '                Prompt = "Error"

        '            End If

        '        Else

        '            oStat = ds.Tables(0).Rows(0)("stat").ToString
        '            If oStat = "C" Then
        '                Stat = "FALSE"
        '                MsgType = "PPCC"
        '                Msg = "#407 : เลขที่ Tag [" & TagID & "] ได้ทำการนับเรียบร้อยแล้ว"
        '                Prompt = "Error"

        '            End If

        '        End If

        '    End If



        'End If


    End Sub

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

    Sub GetWarehouse()

        Dim DefWhse As String = ""

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet

        ds = oWS.LoadDataSet(Session("Token").ToString, "SLWhses", "Whse", "", "Whse", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            ddlwhse.Items.Add(New ListItem(dRow("Whse"), dRow("Whse")))
        Next

        ddlwhse.Items.Insert(0, New ListItem("", ""))

        DefWhse = GetDefWhse()

        If DefWhse <> "" Then
            ddlwhse.SelectedValue = DefWhse.ToString
        End If

    End Sub


    Sub GetProductCode()

        oWS = New CNIService.DOWebServiceSoapClient

        ds = New DataSet
        Dim crItem As ListItem

        ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_Ex_CycleCntDetail", "ProductCode", "", "ProductCode", "", 0)

        For Each dRow As DataRow In ds.Tables(0).Rows
            crItem = ddlProductCode.Items.FindByValue(dRow("ProductCode"))


            If ddlProductCode.Items.FindByValue(dRow("ProductCode")) Is Nothing Then
                ddlProductCode.Items.Add(New ListItem(dRow("ProductCode"), dRow("ProductCode")))
            End If

        Next

        ddlProductCode.Items.Insert(0, New ListItem("", ""))

    End Sub


#Region "Bind Data To Gridview"

    Sub BindGridview()

        Dim Filter As String
        Dim Propertie As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        Propertie = "Item, Loc, Lot, CutOffQty, CountQty, Stat, Description, ProductCode, Uf_Item_LongDesc, Uf_loc_NonScanTag"

        If ddlLoc.SelectedItem.Value = "A" Then

            Filter = "Whse = '" & ddlwhse.SelectedItem.Value & "' And  ProductCode = '" & ddlProductCode.SelectedItem.Value & "' And Uf_item_Barcodecontrol = 1"
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLCycles", Propertie, Filter, "RecordDate Desc", "", 0)
        ElseIf ddlLoc.SelectedItem.Value = "N" Then

            Filter = "Whse = '" & ddlwhse.SelectedItem.Value & "' And Uf_loc_NonScanTag = 1 And  ProductCode = '" & ddlProductCode.SelectedItem.Value & "' And Uf_item_Barcodecontrol = 1"
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLCycles", Propertie, Filter, "RecordDate Desc", "", 0)

        ElseIf ddlLoc.SelectedItem.Value = "Y" Then

            Filter = "Whse = '" & ddlwhse.SelectedItem.Value & "' And Uf_loc_NonScanTag = 0 And  ProductCode = '" & ddlProductCode.SelectedItem.Value & "' And Uf_item_Barcodecontrol = 1"
            ds = oWS.LoadDataSet(Session("Token").ToString, "PPCC_SLCycles", Propertie, Filter, "RecordDate Desc", "", 0)

        End If

        'GridView1.DataSource = ds.Tables(0)
        'GridView1.DataBind()

        PanelList.DataSource = ds.Tables(0)
        PanelList.DataBind()

    End Sub
#End Region

#Region "Function"

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