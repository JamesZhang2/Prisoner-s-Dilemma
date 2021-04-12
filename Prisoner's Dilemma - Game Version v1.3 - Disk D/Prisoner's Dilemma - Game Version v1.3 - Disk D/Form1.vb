'Created by James Ruogu Zhang on July 16, 2019
'Modified by James Ruogu Zhang on July 20, 2019 (v1.1,v1.2)
'Modified by James Ruogu Zhang on July 26, 2019 (v1.3)
'First Player: Copycat, 6 turns
'Second Player: Cooperator, 4 turns
'Third Player: Detective, 7 turns
'Fourth Player: Grudger, 6 turns
'Fifth Player: Cheater, 5 turns
'v1.0: Program created successfully
'v1.1: Users can enter their nickname
'v1.2: Simpler Name
'v1.3: Different Order, procedure of characters more flexible (use "round" instead of a specific round)
Imports System.IO
Public Class Form1
    Public know As Integer  'How much they know about Game Theory
    Public kind As Boolean  'Whether to begin with kind or unkind opponents
    Public YourScore(0 To 5), OppScore(0 To 5), TotalScore As Integer  'Your score and your opponents' score
    Public YourChoice(0 To 5, 0 To 8), OppChoice(0 To 5, 0 To 8) As Integer  'Your choice and your opponents' choice. 0 = Betray, 1 = Cooperate
    Public turn(0 To 5) As Integer  'Current turn
    Public round As Integer  'Current round
    Public HadBetrayed As Boolean  'Whether you have betrayed in a round (for Grudger and Detective)
    Public FileWriter As StreamWriter
    Public FileName As String
    Public TextOfFile As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Initialize()
        If Rnd() < 0.5 Then
            kind = False
        Else
            kind = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        know = 2
        ShowDescription1()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        know = 1
        ShowDescription1()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        know = 0
        ShowDescription1()
    End Sub

    Private Sub ButtonC1_Click(sender As Object, e As EventArgs) Handles ButtonC1.Click
        YourChoice(1, turn(1)) = 1      'You choose to cooperate in this turn
        Copycat()                       'Your opponent plays Copycat
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(1) = 6 Then
            ButtonNext1.Visible = True  'You may continue
            ButtonC1.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB1.Enabled = False
        Else
            turn(1) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonB1_Click(sender As Object, e As EventArgs) Handles ButtonB1.Click
        YourChoice(1, turn(1)) = 0      'You choose to betray in this turn
        Copycat()                       'Your opponent plays Copycat
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(1) = 6 Then
            ButtonNext1.Visible = True  'You may continue
            ButtonC1.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB1.Enabled = False
        Else
            turn(1) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonC2_Click(sender As Object, e As EventArgs) Handles ButtonC2.Click
        YourChoice(2, turn(2)) = 1      'You choose to cooperate in this turn
        Cooperator()                    'Your opponent plays Cooperator
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(2) = 4 Then
            ButtonNext2.Visible = True  'You may continue
            ButtonC2.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB2.Enabled = False
        Else
            turn(2) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonB2_Click(sender As Object, e As EventArgs) Handles ButtonB2.Click
        YourChoice(2, turn(2)) = 0      'You choose to betray in this turn
        Cooperator()                    'Your opponent plays Cooperator
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(2) = 4 Then
            ButtonNext2.Visible = True  'You may continue
            ButtonC2.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB2.Enabled = False
        Else
            turn(2) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonC3_Click(sender As Object, e As EventArgs) Handles ButtonC3.Click
        YourChoice(3, turn(3)) = 1      'You choose to cooperate in this turn
        Detective()                     'Your opponent plays Detective
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(3) = 7 Then
            ButtonNext3.Visible = True  'You may continue
            ButtonC3.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB3.Enabled = False
        Else
            turn(3) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonB3_Click(sender As Object, e As EventArgs) Handles ButtonB3.Click
        YourChoice(3, turn(3)) = 0      'You choose to betray in this turn
        Detective()                     'Your opponent plays Detective
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(3) = 7 Then
            ButtonNext3.Visible = True  'You may continue
            ButtonC3.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB3.Enabled = False
        Else
            turn(3) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonC4_Click(sender As Object, e As EventArgs) Handles ButtonC4.Click
        YourChoice(4, turn(4)) = 1      'You choose to cooperate in this turn
        Grudger()                       'Your opponent plays Grudger
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(4) = 6 Then
            ButtonNext4.Visible = True  'You may continue
            ButtonC4.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB4.Enabled = False
        Else
            turn(4) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonB4_Click(sender As Object, e As EventArgs) Handles ButtonB4.Click
        YourChoice(4, turn(4)) = 0      'You choose to betray in this turn
        Grudger()                       'Your opponent plays Grudger
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(4) = 6 Then
            ButtonNext4.Visible = True  'You may continue
            ButtonC4.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB4.Enabled = False
        Else
            turn(4) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonC5_Click(sender As Object, e As EventArgs) Handles ButtonC5.Click
        YourChoice(5, turn(5)) = 1      'You choose to cooperate in this turn
        Cheater()                    'Your opponent plays Cheater
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(5) = 5 Then
            ButtonNext5.Visible = True  'You may continue
            ButtonC5.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB5.Enabled = False
        Else
            turn(5) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonB5_Click(sender As Object, e As EventArgs) Handles ButtonB5.Click
        YourChoice(5, turn(5)) = 0      'You choose to betray in this turn
        Cheater()                       'Your opponent plays Cheater
        ShowChoice()                    'Show opponent's choice
        GetScore()                      'Calculate scores
        ShowScore()                     'Show scores
        If turn(5) = 5 Then
            ButtonNext5.Visible = True  'You may continue
            ButtonC5.Enabled = False    'You can't choose either B or C in this round because it is over
            ButtonB5.Enabled = False
        Else
            turn(5) += 1                'Go to the next turn
        End If
    End Sub

    Private Sub ButtonNext1_Click(sender As Object, e As EventArgs) Handles ButtonNext1.Click
        ShowDescription2()
        round = 2
    End Sub

    Private Sub ButtonNext2_Click(sender As Object, e As EventArgs) Handles ButtonNext2.Click
        ShowDescription3()
        round = 3
    End Sub

    Private Sub ButtonNext3_Click(sender As Object, e As EventArgs) Handles ButtonNext3.Click
        ShowDescription4()
        round = 4
        HadBetrayed = False  'Reset HadBetrayed for Grudger
    End Sub

    Private Sub ButtonNext4_Click(sender As Object, e As EventArgs) Handles ButtonNext4.Click
        ShowDescription5()
        round = 5
    End Sub

    Private Sub ButtonNext5_Click(sender As Object, e As EventArgs) Handles ButtonNext5.Click
        ShowDescription6()
        PrintData()
    End Sub

    Sub ShowDescription1()
        'Show the first set of description and hide unnecessary description
        Label3.Visible = True
        Label4.Visible = True
        Label5.Visible = True
        Label6.Visible = True
        Label7.Visible = True
        LabelChoice1.Visible = True
        Label9.Visible = True
        LabelYP1.Visible = True
        Label11.Visible = True
        LabelOP1.Visible = True
        ButtonC1.Visible = True
        ButtonB1.Visible = True
        Label2.Visible = False
        LabelName.Visible = False
        TextBoxName.Visible = False
        Button1.Visible = False
        Button2.Visible = False
        Button3.Visible = False
    End Sub

    Sub ShowDescription2()
        'Show the second set of description hide unnecessary description
        Label14.Visible = True
        Label16.Visible = True
        Label18.Visible = True
        Label19.Visible = True
        LabelYP2.Visible = True
        LabelOP2.Visible = True
        LabelChoice2.Visible = True
        ButtonC2.Visible = True
        ButtonB2.Visible = True
        Label3.Visible = False
        Label6.Visible = False
        Label7.Visible = False
        LabelChoice1.Visible = False
        Label9.Visible = False
        LabelYP1.Visible = False
        Label11.Visible = False
        LabelOP1.Visible = False
        ButtonC1.Visible = False
        ButtonB1.Visible = False
        ButtonNext1.Visible = False
    End Sub

    Sub ShowDescription3()
        Label21.Visible = True
        Label23.Visible = True
        Label25.Visible = True
        Label26.Visible = True
        LabelYP3.Visible = True
        LabelOP3.Visible = True
        LabelChoice3.Visible = True
        ButtonC3.Visible = True
        ButtonB3.Visible = True
        Label14.Visible = False
        Label16.Visible = False
        Label18.Visible = False
        Label19.Visible = False
        LabelYP2.Visible = False
        LabelOP2.Visible = False
        LabelChoice2.Visible = False
        ButtonC2.Visible = False
        ButtonB2.Visible = False
        ButtonNext2.Visible = False
    End Sub

    Sub ShowDescription4()
        Label28.Visible = True
        Label30.Visible = True
        Label32.Visible = True
        Label33.Visible = True
        LabelYP4.Visible = True
        LabelOP4.Visible = True
        LabelChoice4.Visible = True
        ButtonC4.Visible = True
        ButtonB4.Visible = True
        Label21.Visible = False
        Label23.Visible = False
        Label25.Visible = False
        Label26.Visible = False
        LabelYP3.Visible = False
        LabelOP3.Visible = False
        LabelChoice3.Visible = False
        ButtonC3.Visible = False
        ButtonB3.Visible = False
        ButtonNext3.Visible = False
    End Sub

    Sub ShowDescription5()
        Label35.Visible = True
        Label37.Visible = True
        Label39.Visible = True
        Label40.Visible = True
        LabelYP5.Visible = True
        LabelOP5.Visible = True
        LabelChoice5.Visible = True
        ButtonC5.Visible = True
        ButtonB5.Visible = True
        Label28.Visible = False
        Label30.Visible = False
        Label32.Visible = False
        Label33.Visible = False
        LabelYP4.Visible = False
        LabelOP4.Visible = False
        LabelChoice4.Visible = False
        ButtonC4.Visible = False
        ButtonB4.Visible = False
        ButtonNext4.Visible = False
    End Sub

    Sub ShowDescription6()
        Label8.Visible = True
        Label41.Visible = True
        Label10.Visible = True
        For i = 1 To 5
            TotalScore += YourScore(i)
        Next
        LabelYPTotal.Text = TotalScore
        LabelYPTotal.Visible = True
        Label35.Visible = False
        Label37.Visible = False
        Label39.Visible = False
        Label40.Visible = False
        LabelYP5.Visible = False
        LabelOP5.Visible = False
        LabelChoice5.Visible = False
        ButtonC5.Visible = False
        ButtonB5.Visible = False
        ButtonNext5.Visible = False
    End Sub

    Sub Initialize()
        For i = 1 To 5
            YourScore(i) = 0
            OppScore(i) = 0
            turn(i) = 1
        Next
        round = 1
        TotalScore = 0
        HadBetrayed = False
    End Sub

    Sub GetScore()                                        'Calculate scores
        If YourChoice(round, turn(round)) = 0 Then        'You betrayed
            If OppChoice(round, turn(round)) = 0 Then     'Your opponent betrayed. No one gets points
            Else                                          'Your opponent cooperated
                YourScore(round) += 3
                OppScore(round) -= 1
            End If
        Else                                              'You cooperated
            If OppChoice(round, turn(round)) = 0 Then     'Your opponent betrayed
                YourScore(round) -= 1
                OppScore(round) += 3
            Else                                          'Your opponent cooperated
                YourScore(round) += 2
                OppScore(round) += 2
            End If
        End If
    End Sub

    Sub ShowScore()                                       'Show scores
        LabelYP1.Text = YourScore(1)
        LabelOP1.Text = OppScore(1)
        LabelYP2.Text = YourScore(2)
        LabelOP2.Text = OppScore(2)
        LabelYP3.Text = YourScore(3)
        LabelOP3.Text = OppScore(3)
        LabelYP4.Text = YourScore(4)
        LabelOP4.Text = OppScore(4)
        LabelYP5.Text = YourScore(5)
        LabelOP5.Text = OppScore(5)
    End Sub

    Sub ShowChoice()                                      'Show opponent's choice
        If round = 1 Then
            If OppChoice(1, turn(1)) = 0 Then
                LabelChoice1.Text = "背叛"
            Else
                LabelChoice1.Text = "合作"
            End If
        End If

        If round = 2 Then
            If OppChoice(2, turn(2)) = 0 Then
                LabelChoice2.Text = "背叛"
            Else
                LabelChoice2.Text = "合作"
            End If
        End If

        If round = 3 Then
            If OppChoice(3, turn(3)) = 0 Then
                LabelChoice3.Text = "背叛"
            Else
                LabelChoice3.Text = "合作"
            End If
        End If

        If round = 4 Then
            If OppChoice(4, turn(4)) = 0 Then
                LabelChoice4.Text = "背叛"
            Else
                LabelChoice4.Text = "合作"
            End If
        End If

        If round = 5 Then
            If OppChoice(5, turn(5)) = 0 Then
                LabelChoice5.Text = "背叛"
            Else
                LabelChoice5.Text = "合作"
            End If
        End If
    End Sub

    Sub Grudger()
        If turn(round) = 1 Then
            OppChoice(round, turn(round)) = 1                     'Cooperates in the first turn
        Else
            If YourChoice(round, turn(round) - 1) = 0 Then        'You betrayed last turn
                HadBetrayed = True
            End If
            If HadBetrayed Then
                OppChoice(round, turn(round)) = 0                 'Betrays if you had betrayed before
            Else
                OppChoice(round, turn(round)) = 1                 'Cooperates if you hadn't betrayed before
            End If
        End If
    End Sub

    Sub Cheater()
        OppChoice(round, turn(round)) = 0                         'Always betrays
    End Sub

    Sub Detective()                                       'Plays CBCC first, and then copycat if you had betrayed, cheater if you hadn't
        If turn(round) > 1 And YourChoice(round, turn(round) - 1) = 0 Then
            HadBetrayed = True
        End If
        If turn(round) = 1 Then
            OppChoice(round, turn(round)) = 1
        ElseIf turn(round) = 2 Then
            OppChoice(round, turn(round)) = 0
        ElseIf turn(round) = 3 Then
            OppChoice(round, turn(round)) = 1
        ElseIf turn(round) = 4 Then
            OppChoice(round, turn(round)) = 1
        Else
            If HadBetrayed = False Then
                OppChoice(round, turn(round)) = 0
            Else
                OppChoice(round, turn(round)) = YourChoice(round, turn(round) - 1)
            End If
        End If
    End Sub

    Sub Copycat()
        If turn(round) = 1 Then
            OppChoice(round, turn(round)) = 1                     'Cooperates in the first turn
        Else                                              'Copies your previous choice in later turns
            OppChoice(round, turn(round)) = YourChoice(round, turn(round) - 1)
        End If
    End Sub

    Sub Cooperator()
        OppChoice(round, turn(round)) = 1                         'Always cooperates
    End Sub

    Sub PrintData()
        FileName = "D:\record.txt"
        FileWriter = New StreamWriter(FileName, True)     'Can append message without overwriting existing messages
        TextOfFile = "kind,"
        TextOfFile += "name: " & TextBoxName.Text & ","
        TextOfFile += "knowledge: " & know & ","
        TextOfFile += "round 1,"
        For i = 1 To 6
            TextOfFile += YourChoice(1, i) & "," & OppChoice(1, i) & ","
        Next
        TextOfFile += "round 2,"
        For i = 1 To 4
            TextOfFile += YourChoice(2, i) & "," & OppChoice(2, i) & ","
        Next
        TextOfFile += "round 3,"
        For i = 1 To 7
            TextOfFile += YourChoice(3, i) & "," & OppChoice(3, i) & ","
        Next
        TextOfFile += "round 4,"
        For i = 1 To 6
            TextOfFile += YourChoice(4, i) & "," & OppChoice(4, i) & ","
        Next
        TextOfFile += "round 5,"
        For i = 1 To 5
            TextOfFile += YourChoice(5, i) & "," & OppChoice(5, i) & ","
        Next
        TextOfFile += "score,"
        For i = 1 To 5
            TextOfFile += YourScore(i) & "," & OppScore(i) & ","
        Next
        TextOfFile += "total score," & TotalScore
        FileWriter.WriteLine(TextOfFile)
        FileWriter.Close()
    End Sub
End Class