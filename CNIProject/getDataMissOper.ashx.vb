﻿Imports System.IO
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq




Public Class getDataMissOper
    Implements System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState



    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        '' InitializeComponent()

        ''context.Response.ContentType = "application/json"
        'Dim Token, json As String
        'Dim Filter As String
        'Dim Propertie As String
        'Dim OrderBy As String
        'Dim Ido As String
        'Dim TokenS As String
        'Dim strJson As String
        'Dim RetiveData As Int32


        'oWS = New CNIService.DOWebServiceSoapClient
        'ds = New DataSet
        'Filter = ""
        'strJson = New StreamReader(context.Request.InputStream).ReadToEnd()
        'Dim contents As Array = strJson.Split("&")
        'Dim Fields As Array
        'For index As Integer = 1 To contents.Length
        '    Fields = contents(index - 1).Split("=")

        '    Select Case Fields(0)
        '        Case "Ido"
        '            Ido = Fields(1)
        '        Case "Token"
        '            Token = URLDecode(Fields(1))
        '        Case "Propertie"
        '            Propertie = Fields(1)

        '    End Select
        'Next index

        ''Propertie = "Item,Description"

        'ds = oWS.LoadDataSet(Token, Ido, Propertie, Filter, "CreateDate Desc", "", 100)

        'json = DataTableToJSONWithJavaScriptSerializer(ds)
        'context.Response.ContentType = "text/json"
        'context.Response.Write(json)

        Dim oWS As CNIService.DOWebServiceSoapClient
        Dim ds As DataSet
        Dim ser As JavaScriptSerializer = New JavaScriptSerializer()
        Dim strJson, __json As String
        Dim Token, Ido, Propertie, Filter, OrerBy, PostQueryMethod, RecordCap As String

        oWS = New CNIService.DOWebServiceSoapClient
        ds = New DataSet
        strJson = New StreamReader(context.Request.InputStream).ReadToEnd()
        Dim contents As Array = strJson.Split("&")
        Dim Fields As Array
        For index As Integer = 1 To contents.Length
            Fields = contents(index - 1).Split("=")

            Select Case Fields(0)
                Case "Token"
                    Token = URLDecode(Fields(1))
                Case "Ido"
                    Ido = URLDecode(Fields(1))
                Case "Propertie"
                    Propertie = URLDecode(Fields(1))
                Case "Filter"
                    Filter = URLDecode(Fields(1))
                Case "OrerBy"
                    OrerBy = URLDecode(Fields(1))
                Case "PostQueryMethod"
                    PostQueryMethod = Fields(1)
                Case "RecordCap"
                    RecordCap = Fields(1)
            End Select
        Next index
        'Propertie = "Item,Description"
        ds = oWS.LoadDataSet(Token, Ido, Propertie, Filter, OrerBy, PostQueryMethod, RecordCap)

        __json = DataTableToJSONWithJavaScriptSerializer(ds)
        context.Response.ContentType = "text/json"
        context.Response.Write(__json)

    End Sub


    Public Function URLDecode(StringToDecode As String) As String

        Dim TempAns As String
        Dim CurChr As Integer

        CurChr = 1

        Do Until CurChr - 1 = Len(StringToDecode)
            Select Case Mid(StringToDecode, CurChr, 1)
                Case "+"
                    TempAns = TempAns & " "
                Case "%"
                    TempAns = TempAns & Chr(Val("&h" &
         Mid(StringToDecode, CurChr + 1, 2)))
                    CurChr = CurChr + 2
                Case Else
                    TempAns = TempAns & Mid(StringToDecode, CurChr, 1)
            End Select

            CurChr = CurChr + 1
        Loop

        URLDecode = TempAns
    End Function


    Public Function DataTableToJSONWithJavaScriptSerializer(ByVal dataset As DataSet) As String
        Dim jsSerializer As JavaScriptSerializer = New JavaScriptSerializer()
        Dim ssvalue As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()

        For Each table As DataTable In dataset.Tables
            Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()
            Dim childRow As Dictionary(Of String, Object)
            Dim tablename As String = table.TableName

            For Each row As DataRow In table.Rows
                childRow = New Dictionary(Of String, Object)()

                For Each col As DataColumn In table.Columns
                    childRow.Add(col.ColumnName, row(col))
                Next

                parentRow.Add(childRow)
            Next

            ssvalue.Add(tablename, parentRow)
        Next

        Return jsSerializer.Serialize(ssvalue)
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property



End Class

Class RequestInfo
    Public Property ido As String
    Public Property msg As String

End Class