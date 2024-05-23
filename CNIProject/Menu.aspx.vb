Imports System.Data
Imports System.Xml

Public Class Menu
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim Filter As String
    Dim Parms As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("PSession") Is Nothing Then
            Response.Redirect("signin.aspx")
        Else
            If Session("PSession").ToString = "" Then
                Response.Redirect("signin.aspx")
            End If
        End If

        If Not Page.IsPostBack Then

            EnableLink()

            'If Session("DsReceiving") IsNot Nothing Then

            '    If Session("DsReceiving") = "0" Then
            '        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error [ PPCC ]','" & "กรุณาเลือก Use Web Delivery Sheet Receiving ที่หน้า Purchasing Parameters" & "', 'error');", True)
            '        Session.Remove("DsReceiving")
            '    End If

            'End If

            'txtemp.Focus()

            'If Not IsNothing(Session("Employee")) Then

            '    lblempname.Visible = True
            '    lblempname.Text = Session("Name").ToString
            'Else
            '    lblempname.Visible = False
            '    lblempname.Text = String.Empty
            'End If

        End If

    End Sub

    'Protected Sub txtemp_TextChanged(sender As Object, e As EventArgs) Handles txtemp.TextChanged

    '    If txtemp.Text <> String.Empty Then
    '        oWS = New CNIService.DOWebServiceSoapClient
    '        ds = New DataSet

    '        Dim empNum As String = ""
    '        Dim empName As String = ""

    '        Dim Filter As String = "EmpNum = '" & txtemp.Text.Trim & "'"
    '        ds = oWS.LoadDataSet(Session("Token"), "SLEmployees", "EmpNum, Name", Filter, "", "", 0)

    '        If ds.Tables(0).Rows.Count > 0 Then
    '            empNum = ds.Tables(0).Rows(0)("EmpNum").ToString()
    '            empName = ds.Tables(0).Rows(0)("Name").ToString()

    '            Session("Employee") = empNum
    '            Session("Name") = empName

    '            Response.Redirect("Menu.aspx")
    '            'Else
    '            '    NotPassNotifyPanel.Visible = True
    '            '    NotPassText.Text = "Employee is Invalid."
    '            '    txtemp.Text = String.Empty
    '            '    txtemp.Focus()
    '        End If



    '    End If

    'End Sub




    Sub EnableLink()

        Dim Check_Order As String = "0"
        Dim Confirm_Order_Pick As String = "0"
        Dim Cycle_Count As String = "0"
        Dim Job_Material As String = "0"
        Dim Job_Monitoring As String = "0"
        Dim Job_Receipt As String = "0"
        Dim Print_Production As String = "0"
        Dim Print_WIP As String = "0"
        Dim Purchase_Order As String = "0"
        Dim Quantity_Move As String = "0"
        Dim Unposted_Job As String = "0"
        Dim Confirm_Shipping As String = "0"
        Dim DsReceive As String = "0"


        oWS = New CNIService.DOWebServiceSoapClient

        Parms = "<Parameters><Parameter>" & Session("UserName").ToString & "</Parameter>" &
                        "<Parameter>" & Session("PSite").ToString & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                        "<Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
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
        oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_EX_GetAccessFormSp", Parms)

        Dim doc As XmlDocument = New XmlDocument()
        doc.LoadXml(Parms)

        Dim i As Integer = 1


        For Each node As XmlNode In doc.DocumentElement

            If i = 3 Then
                Check_Order = node.InnerText

            ElseIf i = 4 Then
                Confirm_Order_Pick = node.InnerText

            ElseIf i = 5 Then
                Cycle_Count = node.InnerText

            ElseIf i = 6 Then
                Job_Material = node.InnerText

            ElseIf i = 7 Then
                Job_Monitoring = node.InnerText

            ElseIf i = 8 Then
                Job_Receipt = node.InnerText

            ElseIf i = 9 Then
                Print_Production = node.InnerText

            ElseIf i = 10 Then
                Print_WIP = node.InnerText

            ElseIf i = 11 Then
                Purchase_Order = node.InnerText

            ElseIf i = 12 Then
                Quantity_Move = node.InnerText

            ElseIf i = 13 Then
                Unposted_Job = node.InnerText

            ElseIf i = 14 Then
                Confirm_Shipping = node.InnerText

            ElseIf i = 15 Then
                DsReceive = node.InnerText

            End If

            i += 1

        Next

        LinkCheckOrderPickListStatus.Visible = Check_Order
        LinkConfirmOrderPick.Visible = Confirm_Order_Pick
        LinkCycleCount.Visible = Cycle_Count
        LinkJobMatlTran.Visible = Job_Material
        LinkJobMonitoring.Visible = Job_Monitoring
        LinkJobReceipt.Visible = Job_Receipt
        LinkPrintProduction.Visible = Print_Production
        LinkPrintWIP.Visible = Print_WIP
        LinkPurchaseOrderRecpt.Visible = Purchase_Order
        LinkQuantityMove.Visible = Quantity_Move
        LinkUnpostedJob.Visible = Unposted_Job
        LinkConfirmShipping.Visible = Confirm_Shipping
        LinkDsReceive.Visible = DsReceive

        If Check_Order = "0" Then
            Me.divCheckOrder.Attributes("class") = ""
        End If
        If Confirm_Order_Pick = "0" Then
            Me.divConfirmOrderPick.Attributes("class") = ""
        End If
        If Cycle_Count = "0" Then
            Me.divCycleCount.Attributes("class") = ""
        End If
        If Job_Material = "0" Then
            Me.divJobMatlTran.Attributes("class") = ""
        End If
        If Job_Monitoring = "0" Then
            Me.divJobMonitoring.Attributes("class") = ""
        End If
        If Job_Receipt = "0" Then
            Me.divJobReceipt.Attributes("class") = ""
        End If
        If Print_Production = "0" Then
            Me.divPrintProduction.Attributes("class") = ""
        End If
        If Print_WIP = "0" Then
            Me.divPrintWIP.Attributes("class") = ""
        End If
        If Purchase_Order = "0" Then
            Me.divPurchaseOrder.Attributes("class") = ""
        End If
        If Quantity_Move = "0" Then
            Me.divQuantityMove.Attributes("class") = ""
        End If
        If Unposted_Job = "0" Then
            Me.divUnposted.Attributes("class") = ""
        End If
        If Confirm_Shipping = "0" Then
            Me.divConfirmShipping.Attributes("class") = ""
        End If
        If DsReceive = "0" Then
            Me.divDsReceive.Attributes("class") = ""
        End If



    End Sub



End Class