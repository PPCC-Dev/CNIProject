Imports System.Data
Imports System.Xml
Imports System.Collections
Imports System.Reflection
Imports Microsoft.VisualBasic.DateInterval

Public Class Signin
    Inherits System.Web.UI.Page

    Dim oWS As CNIService.DOWebServiceSoapClient
    Dim ds As DataSet
    Dim SGUID As String
    Dim Parms As String

    Private Shared PSite As String

    Private Shared Property ParmSite() As String
        Get
            Return PSite
        End Get
        Set(value As String)
            PSite = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")

        If Not Page.IsPostBack Then

            oWS = New CNIService.DOWebServiceSoapClient
            For Each str As String In oWS.GetConfigurationNames
                ddlconfig.Items.Add(New ListItem(str, str))
            Next

            Dim SLConfig As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")
            Dim lstConfig As ListItem = ddlconfig.Items.FindByValue(Convert.ToString(SLConfig))

            If SLConfig Is Nothing Or lstConfig Is Nothing Then
                ddlconfig.Enabled = True
            Else
                ddlconfig.SelectedValue = SLConfig.ToString
                ddlconfig.Attributes.Add("disabled", "disabled")
            End If

            'txtBarcode.Attributes.Add("disabled", "disabled")

        End If

        If CheckBox.Checked = True Then

            txtBarcode.Attributes.Remove("disabled")
            txtusername.Attributes.Add("disabled", "disabled")
            txtpassword.Attributes.Add("disabled", "disabled")
            txtBarcode.Attributes.Add("onchange", "javascript:scanbarcode();")
            txtBarcode.Focus()

        ElseIf CheckBox.Checked = False Then

            txtBarcode.Attributes.Add("disabled", "disabled")
            txtusername.Attributes.Remove("disabled")
            txtpassword.Attributes.Remove("disabled")
            txtusername.Focus()

        End If

    End Sub

    Protected Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        Dim Config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")

        Dim MsgErr As String = ""
        Try
            Dim Token As String = ""
            Dim UserName As String = Trim(txtusername.Text)
            Dim UserDesc As String = ""
            Dim Password As String = txtpassword.Text
            'Dim Config As String = Convert.ToString(ddlconfig.SelectedValue)
            Dim Parms As String = ""
            Dim UserCodes As String = ""
            Dim sBarcode As String = ""


            oWS = New CNIService.DOWebServiceSoapClient

            If CheckBox.Checked = True Then

                sBarcode = txtBarcode.Text

                Dim arrLogin As String()
                arrLogin = sBarcode.Split(New Char() {"|"c})

                If arrLogin.Length > 0 Then
                    UserName = arrLogin(0)
                    Password = arrLogin(1)
                End If

                Token = oWS.CreateSessionToken(UserName, Password, Config)

            Else
                Token = oWS.CreateSessionToken(UserName, Password, Config)
            End If

            'MsgBox(Token)

            Session("Token") = Token
            Session("UserName") = UserName
            Session("Config") = Replace(Config, "_", " ")
            'Session("Config") = Replace(Config, "SRN_", "")

            If Session("UserName") IsNot Nothing Then
                Session("UserDesc") = GetUserName(Session("UserName").ToString)
            End If

            If Not IsNothing(Session("Token")) Then

                ParmSite = GetSite()

                Parms = "<Parameters><Parameter ByRef='Y'>" & DBNull.Value & "</Parameter>" &
                            "<Parameter>" & Session("UserName").ToString & "</Parameter>" &
                            "<Parameter>" & ParmSite.ToString & "</Parameter>" &
                            "</Parameters>"

                oWS = New CNIService.DOWebServiceSoapClient
                oWS.CallMethod(Session("Token").ToString, "PPCC_Ex_QtyMove", "PPCC_Ex_GetSessionIDSp", Parms)


                Dim doc As XmlDocument = New XmlDocument()
                doc.LoadXml(Parms)

                Dim i As Integer = 1

                For Each node As XmlNode In doc.DocumentElement

                    If i = 1 Then
                        Session("PSession") = node.InnerText

                        Exit For
                    End If

                    i += 1
                Next

            End If

            'SGUID = System.Guid.NewGuid.ToString()
            'Session("PSession") = SGUID

            Dim time As String = System.Configuration.ConfigurationManager.AppSettings("PageTimeOut")

            If time Is Nothing Then
                time = "30"
            End If

            Session("Token") = Token
            Session.Timeout = time
            Session("PSite") = ParmSite

            Dim AccessJobMonitoring As String
            AccessJobMonitoring = CheckAccessJobMonitoring()

            If String.IsNullOrEmpty(AccessJobMonitoring) Or AccessJobMonitoring = "0" Then
                Response.Redirect("Menu.aspx")
            Else
                Response.Redirect("JobMonitoringStatus.aspx")
            End If



        Catch ex As Exception

            Dim end_pos As Integer = InStr(ex.Message, "at IDOWebService.ThrowSoapException")
            If end_pos > 0 And InStr(ex.Message, "System.Web.Services.Protocols.SoapException:") > 0 Then
                MsgErr = Left(ex.Message, IIf(end_pos = 0, 0, end_pos - 1)).Replace("System.Web.Services.Protocols.SoapException:", "")
            Else
                MsgErr = ex.Message
            End If

            If CheckBox.Checked = True Then
                txtBarcode.Text = String.Empty
            End If

            ddlconfig.SelectedValue = Config


            MsgErr = MsgErr.Replace("'", "\'")
            MsgErr = MsgErr.Replace(vbLf, "<br />")

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "ShowSweetAlert('Error','" & MsgErr & "', 'error');", True)

            'NotificationPanel.Visible = True

        End Try

    End Sub

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

    Function GetUserName(UserName As String) As String

        GetUserName = ""

        ds = New DataSet
        oWS = New CNIService.DOWebServiceSoapClient
        ds = oWS.LoadDataSet(Session("Token").ToString, "UserNames", "UserDesc", "Username='" & UserName & "'", "", "", 0)

        If ds.Tables(0).Rows.Count > 0 Then
            GetUserName = ds.Tables(0).Rows(0)("UserDesc").ToString
        End If

    End Function

    Protected Sub checkBox_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox.CheckedChanged

        Dim Config As String = System.Configuration.ConfigurationManager.AppSettings("Configuration")

        If CheckBox.Checked = True Then

            txtBarcode.Attributes.Remove("disabled")
            txtusername.Attributes.Add("disabled", "disabled")
            txtpassword.Attributes.Add("disabled", "disabled")
            txtBarcode.Focus()

        ElseIf CheckBox.Checked = False Then

            txtBarcode.Attributes.Add("disabled", "disabled")
            txtusername.Attributes.Remove("disabled")
            txtpassword.Attributes.Remove("disabled")
            txtusername.Focus()

        End If

        ddlconfig.SelectedIndex = ddlconfig.Items.IndexOf(ddlconfig.Items.FindByValue(Config))

    End Sub

    Function CheckAccessJobMonitoring() As String

        CheckAccessJobMonitoring = ""

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

            If i = 7 Then
                CheckAccessJobMonitoring = node.InnerText
            End If

            i += 1

        Next

        Return CheckAccessJobMonitoring

    End Function

End Class