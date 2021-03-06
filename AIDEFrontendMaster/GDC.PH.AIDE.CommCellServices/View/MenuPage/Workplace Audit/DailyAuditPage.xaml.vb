﻿Imports System.ComponentModel
Imports System.Collections.ObjectModel
Class DailyAuditPage

    Private lstQuestions() As String = {"1. Has the attendance of the team bee checked?", "2. Does the team attendance match the Daily Resource Planner in the Comm. Cell Board?", "3. Has corresponding tasks been assigned to all resources?", "4. Has the previous weekly audit been completed?"}
    Private lstPeople() As String = {"Daily Auditor", "Daily Auditor", "Jeanette Cha", "Weekly Auditor"}
    Private lstDates() As String = {"4/22", "4/23", "4/24", "4/25", "4/26"}
    Private lstEmployee() As String = {"Celso", "Rose", "Weng", "Belle", "Gliff"}
    Private lstMonths() As String = {"MON", "TUE", "WED", "THU", "FRI"}
    Private lstNotes() As String = {"", "", "", "", ""}
    Private dailyVMM As New dayVM

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        generatedata()
        generateQuestions()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub generateQuestions()
        Dim questModel As New QuestionsDayModel
        Dim x As Integer = 0
        For Each quest As String In lstQuestions

            dailyVMM.QuestionDayList.Add(New QuestionsDayModel(quest, lstPeople(x)))
            x += 1

        Next
        QuarterLVQuestions.ItemsSource = dailyVMM.QuestionDayList
        DataContext = dailyVMM.QuestionDayList
    End Sub

    Private Sub generatedata()

        'Dim dailly As New dailyMod
        'dailly.Days = "9"
        'dailly.Dates = "5/27 - 5/31"
        'dailyVMM.Daily.Add(dailly)



        'Dim questModel As New QuestionsModel
        Dim x As Integer = 9
        Dim y As Integer = 0
        For Each quest As String In lstDates

            dailyVMM.Days.Add(New DayMod(lstMonths(y), quest, lstEmployee(y), lstNotes(y)))
            x += 1
            y += 1
        Next
        QuarterLV.ItemsSource = dailyVMM.Days
        DataContext = dailyVMM.Days
    End Sub

End Class

Public Class dayVM
    Private _dailyobjlst As ObservableCollection(Of DayMod)
    Private QtnLst As ObservableCollection(Of QuestionsDayModel)



    Public Sub New()
        _dailyobjlst = New ObservableCollection(Of DayMod)
        QtnLst = New ObservableCollection(Of QuestionsDayModel)
    End Sub

    Public Property Days As ObservableCollection(Of DayMod)
        Get
            Return _dailyobjlst
        End Get
        Set(value As ObservableCollection(Of DayMod))
            _dailyobjlst = value
        End Set
    End Property
    Public Property QuestionDayList As ObservableCollection(Of QuestionsDayModel)
        Get
            Return QtnLst
        End Get
        Set(value As ObservableCollection(Of QuestionsDayModel))
            QtnLst = value
        End Set
    End Property
End Class

Public Class DayMod
    Private _days As String
    Private _dates As String
    Private _empName As String
    Private _notes As String


    Public Sub New()

    End Sub

    Public Sub New(dayss As String, datess As String, emp As String, note As String)
        _days = dayss
        _dates = datess
        _empName = emp
        If note = String.Empty Then
            _notes = "Nothing"
        Else
            _notes = note
        End If

    End Sub
    Public Property Days As String
        Get
            Return _days
        End Get
        Set(value As String)
            _days = value
        End Set
    End Property
    Public Property Dates As String
        Get
            Return _dates
        End Get
        Set(value As String)
            _dates = value
        End Set
    End Property
    Public Property EmpName As String
        Get
            Return _empName
        End Get
        Set(value As String)
            _empName = value
        End Set
    End Property
    Public Property Notes As String
        Get
            Return "Note: " + _notes
        End Get
        Set(value As String)
            _notes = value
        End Set
    End Property
End Class
Public Class QuestionsDayModel
    Private Qtn As String
    Private Ppl As String
    Public Sub New()

    End Sub
    Public Sub New(st As String, pl As String)
        Qtn = st
        Ppl = pl
    End Sub
    Public Property Questions As String
        Get
            Return Qtn
        End Get
        Set(value As String)
            Qtn = value
        End Set
    End Property

    Public Property People As String
        Get
            Return Ppl
        End Get
        Set(value As String)
            Ppl = value
        End Set
    End Property
End Class

