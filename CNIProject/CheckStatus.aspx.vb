Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval


Public Class CheckStatus
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim res As Object
    Dim SGUID As String
    Dim Parms As String
    Dim dt As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("PSession") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("PSession").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        If Not Page.IsPostBack Then

        End If

        txtbarcode.Focus()

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBarcode, Stat, MsgErr, MsgType As String
        Dim TotalLine, TotalPick, TotalShipped, PickListStatus As String
        sBarcode = txtbarcode.Text
        Stat = "FALSE"
        MsgErr = ""
        MsgType = ""
        TotalLine = ""
        TotalPick = ""
        TotalShipped = ""
        PickListStatus = ""

        If txtbarcode.Text <> "" Then
            Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter>" & sBarcode & "</Parameter>" &
                        "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "</Parameters>"

            oWS = New CNIService.DOWebServiceSoapClient
            oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_JobTranTags", "PPCC_Ex_CheckOrderPickListStatusSp", Parms)

            Dim doc As XmlDocument = New XmlDocument()
            doc.LoadXml(Parms)

            Dim i As Integer = 1

            For Each node As XmlNode In doc.DocumentElement

                If i = 1 Then
                    TotalLine = node.InnerText

                ElseIf i = 2 Then
                    TotalPick = node.InnerText

                ElseIf i = 3 Then
                    TotalShipped = node.InnerText

                ElseIf i = 4 Then
                    PickListStatus = node.InnerText

                ElseIf i = 5 Then
                    Stat = node.InnerText

                ElseIf i = 6 Then
                    MsgType = node.InnerText

                ElseIf i = 7 Then
                    MsgErr = node.InnerText

                End If

                i += 1

            Next

            If Stat = "TRUE" Then

                dt = New DataTable()
                dt.Columns.Add("PickListNum")
                dt.Columns.Add("TotalLine")
                dt.Columns.Add("TotalPick")
                dt.Columns.Add("TotalShipped")
                dt.Columns.Add("Status")
                dt.Rows.Add(sBarcode, TotalLine, TotalPick, TotalShipped, PickListStatus)

                ds = New DataSet
                ds.Tables.Add(dt)

                If ds.Tables(0).Rows.Count > 0 Then
                    PanelList.DataSource = ds
                    PanelList.DataBind()
                End If


            Else

                MsgErr = MsgErr.Replace("'", "\'")
                MsgErr = MsgErr.Replace(vbLf, "<br />")

                MsgType = "Error [" & MsgType & "]"

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('" & MsgType & "','" & MsgErr & "', 'error');", True)

            End If

        End If

        txtbarcode.Text = String.Empty

    End Sub

    Protected Sub PanelList_RowDataBound(sender As Object, e As ListViewItemEventArgs) Handles PanelList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then

            Dim lblstat As Label = CType(e.Item.FindControl("lblstat"), Label)

            If lblstat.Text = "S" Then
                lblstat.Text = "Shipped"
                lblstat.Attributes.Add("Style", "color: #28a745; font-size:x-large; font-weight: bolder;")
            ElseIf lblstat.Text = "C" Then
                lblstat.Text = "Confirm"
                lblstat.Attributes.Add("Style", "color: #007bff; font-size:x-large; font-weight: bolder;")
            ElseIf lblstat.Text = "N" Then
                lblstat.Text = "Not Confirm"
                lblstat.Attributes.Add("Style", "color: #dc3545; font-size:x-large; font-weight: bolder;")
            End If



        End If

    End Sub

End Class