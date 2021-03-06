﻿Imports UI_AIDE_CommCellServices.ServiceReference1
Imports System.Reflection
Imports System.IO
Imports System.ServiceModel

''' <summary>
''' GIANN CARLO CAMILO AND CHRISTIAN VALONDO
''' </summary>
''' <remarks></remarks>
Class ThreeC_InsertPage
    Implements ServiceReference1.IAideServiceCallback

#Region "Fields"
    Public _AIDEClientService As ServiceReference1.AideServiceClient
    Private email As String
    Private _objConcern As New Concern
    Private _frame As Frame
    Private _addframe As Frame
    Private _menugrid As Grid
    Private _submenuframe As Frame

    Dim concern As New Concern
#End Region
   
    Public Sub New(email As String, _frame As Frame, _addframe As Frame, _menugrid As Grid, _submenuframe As Frame)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.email = email
        Me._addframe = _addframe
        Me._menugrid = _menugrid
        Me._submenuframe = _submenuframe
        setDate()

        Me._frame = _frame
        
        GetGeneRatedRefNo()
    End Sub

#Region "Initialize Service"
    Public Function InitializeService() As Boolean
        Dim bInitialize As Boolean = False
        Try
            'DisplayText("Opening client service...")
            Dim Context As InstanceContext = New InstanceContext(Me)
            _AIDEClientService = New AideServiceClient(Context)
            _AIDEClientService.Open()
            bInitialize = True
            'DisplayText("Service opened successfully...")
            'Return True
        Catch ex As SystemException
            _AIDEClientService.Abort()
        End Try
        Return bInitialize
    End Function
#End Region
   
#Region "METHODS"
    'Get Generated RefNo
    Public Function GetGeneRatedRefNo() As Boolean
        InitializeService()
        Try
            'DisplayText("Signing on as: " & Globals.OutlookAddIn.EmailAddress)
            Dim objConcern As Object = Nothing
            Dim _concern As ServiceReference1.Concern = _AIDEClientService.GetGeneratedRefNo()
            SavegeneratedRefNo(_concern)
            Return True
        Catch ex As SystemException
            _AIDEClientService.Abort()
            Return False
        End Try
    End Function

    'Display generated RefNo
    Public Sub SavegeneratedRefNo(ByVal _concern As Concern)
        Try
            'DisplayText("Saving RefNo...")
            If Not IsNothing(_concern) Then
                Dim _ConcernDBProvider = New ConcernDBProvider
                Dim _ConcernViewModel = New ConcernViewModel

                _ConcernDBProvider.SetToMyRefNo(_concern)

                _ConcernViewModel.GeneratedRefNo = New ConcernModel(_ConcernDBProvider.GetRefNo())
                Me.DataContext = _ConcernViewModel
            End If
        Catch ex As SystemException

        End Try
    End Sub
    
    Private Function ValidateFields(ByVal obj As ConcernViewModel)
        If obj.SelectedConcern.CONCERN = "" Or obj.SelectedConcern.CAUSE = "" Or obj.SelectedConcern.COUNTERMEASURE = "" Then
            MsgBox("Please fill up all required fields!", MsgBoxStyle.Exclamation, "AIDE")
            Return False
        ElseIf dtDate.SelectedDate.ToString() = String.Empty Then
            MsgBox("Please select due date before creating concern.", MsgBoxStyle.Exclamation, "AIDE")
            Return False
        Else
            concern.Cause = obj.SelectedConcern.CAUSE
            concern.Concerns = obj.SelectedConcern.CONCERN
            concern.CounterMeasure = obj.SelectedConcern.COUNTERMEASURE
            concern.Due_Date = obj.SelectedConcern.DUE_DATE
            Return True
        End If
    End Function

    'Set DateTime Now in DatePicker
    Private Sub setDate()
        Dim ob As New ConcernViewModel
        ob.SelectedConcern.DUE_DATE = DateTime.Now
    End Sub

    Private Sub ExitPage()
        _frame.Navigate(New ThreeC_Page(email, _frame, _addframe, _menugrid, _submenuframe))
        _frame.IsEnabled = True
        _frame.Opacity = 1
        _menugrid.IsEnabled = True
        _menugrid.Opacity = 1
        _submenuframe.IsEnabled = True
        _submenuframe.Opacity = 1
        _addframe.Visibility = Visibility.Hidden
    End Sub

#End Region

#Region "Notify Changes"
    Public Sub NotifyError(message As String) Implements IAideServiceCallback.NotifyError

    End Sub

    Public Sub NotifyOffline(EmployeeName As String) Implements IAideServiceCallback.NotifyOffline

    End Sub

    Public Sub NotifyPresent(EmployeeName As String) Implements IAideServiceCallback.NotifyPresent

    End Sub

    Public Sub NotifySuccess(message As String) Implements IAideServiceCallback.NotifySuccess
        MsgBox("Success", MsgBoxStyle.Information)
    End Sub

    Public Sub NotifyUpdate(objData As Object) Implements IAideServiceCallback.NotifyUpdate

    End Sub

#End Region

#Region "Buttons/Events"
    Private Sub btnBackClick(sender As Object, e As RoutedEventArgs)
        If txtConcern.Text <> "" OrElse txtCAUSE.Text.ToString <> "" OrElse txtCounterMeasure.Text.ToString <> "" Then
            If MsgBox("Are you sure you want to navigate to 3c's Home Page?", MsgBoxStyle.YesNo, "AIDE") = vbYes Then
                ExitPage()
            Else
                Return
            End If
        Else
            ExitPage()
        End If
    End Sub

    Private Sub btnCreate3C(sender As Object, e As RoutedEventArgs)
        InitializeService()
        Dim isValidate As Boolean
        isValidate = ValidateFields(Me.DataContext())

        If isValidate Then
            _AIDEClientService.InsertIntoConcern(concern, email)
            MsgBox("Successfully created 3C!", MsgBoxStyle.Information, "AIDE")

            txtRefNo.Text = String.Empty
            txtConcern.Clear()
            txtCAUSE.Clear()
            txtCounterMeasure.Clear()
            GetGeneRatedRefNo()
            setDate()
            _AIDEClientService.Close()
            ExitPage()
        End If
    End Sub

#End Region

End Class
