'Created by James Ruogu Zhang on April 9, 2019
'v1.0: Program created successfully with the first 6 Strategies
'v1.1: Strategies 7, 8, and 9 added
'v2.0: Strategies 10, 11, and 12 added; Users can define the score for different situations: sBC, sCC, sBB, sCB
'v3.0: Players may make mistakes according to the probablity that the user inputs; Some out of bound inputs prevented
'v3.1: Minor bugs with Grudger, Detective and input order fixed

Module Module1
    Public p, p1, p2, ScoringRules, turns, currentP, currentTurn As Integer
    Public Bcount1, Ccount1, Bcount2, Ccount2, score1, score2 As Long
    Public p1Turn(0 To 100000000), p2Turn(0 To 100000000) As Integer
    Public sBC, sCC, sBB, sCB As Single  'sBC means the score you get if you Betray and your opponent Cooperates
    Public mistake As Single  'The probability of making a mistake

    Sub Main()
        Do
            Console.WriteLine("1: AlwaysB, 2: RandomB, 3: Random, 4: RandomC, 5: AlwaysC, 6: Copycat, 7: CopycatWithGoodMemory")
            Console.WriteLine("8: Grudger, 9: Detective, 10: SwitchChoice, 11: CopyKitten, 12: CopyLion")
            Do
                Console.Write("Enter First Player: ")
                p1 = Console.ReadLine()
            Loop While p1 < 1 Or p1 > 12  'Avoid out of bound inputs
            Do
                Console.Write("Enter Second Player: ")
                p2 = Console.ReadLine()
            Loop While p2 < 1 Or p2 > 12  'Avoid out of bound inputs
            Console.WriteLine("1: 5_3_1_0, 2: 3_2_0_-1, 3: Others")
            Console.Write("Enter Scoring Rules: ")
            ScoringRules = Console.ReadLine()
            If ScoringRules = 1 Then
                sBC = 5
                sCC = 3
                sBB = 1
                sCB = 0
            ElseIf ScoringRules = 2 Then
                sBC = 3
                sCC = 2
                sBB = 0
                sCB = -1
            Else
                UserDefineScore()
            End If
            Do
                Console.Write("Enter Probability of making a mistake(0-1): ")
                mistake = Console.ReadLine()
            Loop While mistake < 0 Or mistake > 1  'Avoid out of bound inputs
            Do
                Console.Write("Enter Turns(1-1000000): ")
                turns = Console.ReadLine()
            Loop While turns < 1 Or turns > 1000000
            Initialize()
            For i = 1 To turns
                currentTurn = i
                currentP = 1
                p = p1
                SelectStrategy()
                currentP = 2
                p = p2
                SelectStrategy()
                GetScore()
            Next
            OutputResults()
            Console.WriteLine("")
        Loop
    End Sub

    Sub AlwaysB()
        'Always Betray
        If currentP = 1 Then
            p1Turn(currentTurn) = 0
            MakeMistake1()
            CheckBC1()
        Else
            p2Turn(currentTurn) = 0
            Makemistake2()
            CheckBC2()
        End If
    End Sub

    Sub RandomB()
        'Betray:Cooperate = 3:1
        If currentP = 1 Then
            Randomize()
            If Rnd() < 0.75 Then
                p1Turn(currentTurn) = 0
            Else
                p1Turn(currentTurn) = 1
            End If
            MakeMistake1()
            CheckBC1()
        Else
            Randomize()
            If Rnd() < 0.75 Then
                p2Turn(currentTurn) = 0
            Else
                p2Turn(currentTurn) = 1
            End If
            Makemistake2()
            CheckBC2()
        End If
    End Sub

    Sub Random()
        'Betray:Cooperate = 1:1
        If currentP = 1 Then
            Randomize()
            If Rnd() < 0.5 Then
                p1Turn(currentTurn) = 0
            Else
                p1Turn(currentTurn) = 1
            End If
            MakeMistake1()
            CheckBC1()
        Else
            Randomize()
            If Rnd() < 0.5 Then
                p2Turn(currentTurn) = 0
            Else
                p2Turn(currentTurn) = 1
            End If
            Makemistake2()
            CheckBC2()
        End If
    End Sub

    Sub RandomC()
        'Betray:Cooperate = 1:3
        If currentP = 1 Then
            Randomize()
            If Rnd() < 0.25 Then
                p1Turn(currentTurn) = 0
            Else
                p1Turn(currentTurn) = 1
            End If
            MakeMistake1()
            CheckBC1()
        Else
            Randomize()
            If Rnd() < 0.25 Then
                p2Turn(currentTurn) = 0
            Else
                p2Turn(currentTurn) = 1
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub AlwaysC()
        'Always Cooperate
        If currentP = 1 Then
            p1Turn(currentTurn) = 1
            MakeMistake1()
            CheckBC1()
        Else
            p2Turn(currentTurn) = 1
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub Copycat()
        'First Cooperates, then copy opponent's last move
        If currentP = 1 Then
            If currentTurn = 1 Then
                p1Turn(currentTurn) = 1
            Else
                p1Turn(currentTurn) = p2Turn(currentTurn - 1)
            End If
            MakeMistake1()
            CheckBC1()
        Else
            If currentTurn = 1 Then
                p2Turn(currentTurn) = 1
            Else
                p2Turn(currentTurn) = p1Turn(currentTurn - 1)
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub CopycatWithGoodMemory()
        'Betray:Cooperate starts with 1:1. Betray will +1 if opponent betrays, Cooperate will +1 if opponent cooperates.
        If currentP = 1 Then
            Randomize()
            If Rnd() < (Bcount2 + 1) / (Bcount2 + Ccount2 + 2) Then
                p1Turn(currentTurn) = 0
            Else
                p1Turn(currentTurn) = 1
            End If
            MakeMistake1()
            CheckBC1()
        Else
            Randomize()
            If Rnd() < (Bcount1 + 1) / (Bcount1 + Ccount1 + 2) Then
                p2Turn(currentTurn) = 0
            Else
                p2Turn(currentTurn) = 1
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub Grudger()
        'Cooperate until opponent betrays. Once opponent betrays, it will betray from then on
        If currentP = 1 Then
            If currentTurn = 1 Then
                p1Turn(currentTurn) = 1
            Else
                If Bcount2 = 0 Then 'Opponent hasn't betrayed
                    p1Turn(currentTurn) = 1
                Else  'Opponent has betrayed
                    p1Turn(currentTurn) = 0
                End If
            End If
            MakeMistake1()
            CheckBC1()
        Else
            If currentTurn = 1 Then
                p2Turn(currentTurn) = 1
            Else
                If Bcount1 = 0 Then  'Opponent hasn't betrayed
                    p2Turn(currentTurn) = 1
                Else  'Opponent has betrayed
                    p2Turn(currentTurn) = 0
                End If
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub Detective()
        'Do C-B-C first, if opponent doesn't repond with any betray, then turn into AlwaysB, otherwise turn into Copycat
        If currentP = 1 Then
            Select Case currentTurn
                Case 1
                    p1Turn(currentTurn) = 1
                Case 2
                    p1Turn(currentTurn) = 0
                Case 3
                    p1Turn(currentTurn) = 1
                Case Else
                    If p2Turn(1) + p2Turn(2) + p2Turn(3) = 3 Then  'Opponent didn't betray in the first 3 turns
                        p1Turn(currentTurn) = 0  'Exploit Opponent
                    Else  'Opponent betrayed in at least one of the first 3 turns
                        p1Turn(currentTurn) = p2Turn(currentTurn - 1)
                    End If
            End Select
            MakeMistake1()
            CheckBC1()
        Else
            Select Case currentTurn
                Case 1
                    p2Turn(currentTurn) = 1
                Case 2
                    p2Turn(currentTurn) = 0
                Case 3
                    p2Turn(currentTurn) = 1
                Case Else
                    If p1Turn(1) + p1Turn(2) + p1Turn(3) = 3 Then  'Opponent didn't betray in the first 3 turns
                        p2Turn(currentTurn) = 0  'Exploit Opponent
                    Else  'Opponent betrayed in at least one of the first 3 turns
                        p2Turn(currentTurn) = p1Turn(currentTurn - 1)
                    End If
            End Select
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub SwitchChoice()
        'Cooperate first. If opponent cooperates, don't change. If opponent betrays, change choice.
        If currentP = 1 Then
            If currentTurn = 1 Then
                p1Turn(currentTurn) = 1
            Else
                If p2Turn(currentTurn - 1) = 1 Then  'opponent cooperates
                    p1Turn(currentTurn) = p1Turn(currentTurn - 1)  'don't change
                Else  'opponent betrays
                    p1Turn(currentTurn) = (p1Turn(currentTurn - 1) + 1) Mod 2  'change
                End If
            End If
            MakeMistake1()
            CheckBC1()
        Else
            If currentTurn = 1 Then
                p2Turn(currentTurn) = 1
            Else
                If p1Turn(currentTurn - 1) = 1 Then  'opponent cooperates
                    p2Turn(currentTurn) = p2Turn(currentTurn - 1)  'don't change
                Else  'opponent betrays
                    p2Turn(currentTurn) = (p2Turn(currentTurn - 1) + 1) Mod 2  'change
                End If
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub CopyKitten()
        'Cooperate first. If opponent betrays twice in a row, betray. Else, cooperate.
        If currentP = 1 Then
            If currentTurn = 1 Or currentTurn = 2 Then
                p1Turn(currentTurn) = 1
            Else
                If p2Turn(currentTurn - 2) + p2Turn(currentTurn - 1) = 0 Then  'Opponent Betrays twice in a row
                    p1Turn(currentTurn) = 0
                Else  'Opponent cooperates at least once in the last two turns
                    p1Turn(currentTurn) = 1
                End If
            End If
            MakeMistake1()
            CheckBC1()
        Else
            If currentTurn = 1 Or currentTurn = 2 Then
                p2Turn(currentTurn) = 1
            Else
                If p1Turn(currentTurn - 2) + p1Turn(currentTurn - 1) = 0 Then  'Opponent Betrays twice in a row
                    p2Turn(currentTurn) = 0
                Else  'Opponent cooperates at least once in the last two turns
                    p2Turn(currentTurn) = 1
                End If
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub CopyLion()
        'Cooperate first. If opponent betrays, betray back twice. Else, cooperate.
        If currentP = 1 Then
            If currentTurn = 1 Then
                p1Turn(currentTurn) = 1
            ElseIf currentTurn = 2 Then
                p1Turn(currentTurn) = p2Turn(currentTurn - 1)
            Else
                If p2Turn(currentTurn - 1) + p2Turn(currentTurn - 2) = 2 Then  'Opponent didn't betray in the last two turns
                    p1Turn(currentTurn) = 1
                Else  'Opponent betrayed in the last turn or 2 turns before
                    p1Turn(currentTurn) = 0
                End If
            End If
            MakeMistake1()
            CheckBC1()
        Else
            If currentTurn = 1 Then
                p2Turn(currentTurn) = 1
            ElseIf currentTurn = 2 Then
                p2Turn(currentTurn) = p1Turn(currentTurn - 1)
            Else
                If p1Turn(currentTurn - 1) + p1Turn(currentTurn - 2) = 2 Then  'Opponent didn't betray in the last two turns
                    p2Turn(currentTurn) = 1
                Else  'Opponent betrayed in the last turn or 2 turns before
                    p2Turn(currentTurn) = 0
                End If
            End If
            MakeMistake2()
            CheckBC2()
        End If
    End Sub

    Sub UserDefineScore()
        Console.Write("Enter sBC: ")
        sBC = Console.ReadLine()
        Console.Write("Enter sCC: ")
        sCC = Console.ReadLine()
        Console.Write("Enter sBB: ")
        sBB = Console.ReadLine()
        Console.Write("Enter sCB: ")
        sCB = Console.ReadLine()
    End Sub

    Sub Initialize()
        Bcount1 = 0
        Ccount1 = 0
        Bcount2 = 0
        Ccount2 = 0
        score1 = 0
        score2 = 0
    End Sub

    Sub SelectStrategy()
        Select Case p
            Case 1
                AlwaysB()
            Case 2
                RandomB()
            Case 3
                Random()
            Case 4
                RandomC()
            Case 5
                AlwaysC()
            Case 6
                Copycat()
            Case 7
                CopycatWithGoodMemory()
            Case 8
                Grudger()
            Case 9
                Detective()
            Case 10
                SwitchChoice()
            Case 11
                CopyKitten()
            Case 12
                CopyLion()
        End Select
    End Sub

    Sub GetScore()
        If p1Turn(currentTurn) = 0 And p2Turn(currentTurn) = 0 Then  'Both betray
            score1 += sBB
            score2 += sBB
        End If
        If p1Turn(currentTurn) = 0 And p2Turn(currentTurn) = 1 Then  'p1 betrays, p2 cooperates
            score1 += sBC
            score2 += sCB
        End If
        If p1Turn(currentTurn) = 1 And p2Turn(currentTurn) = 0 Then  'p2 betrays, p1 cooperates
            score1 += sCB
            score2 += sBC
        End If
        If p1Turn(currentTurn) = 1 And p2Turn(currentTurn) = 1 Then  'Both cooperate
            score1 += sCC
            score2 += sCC
        End If
    End Sub

    Sub MakeMistake1()
        'Check whether Player 1 makes a mistake
        Randomize()
        If Rnd() < mistake Then
            p1Turn(currentTurn) = (p1Turn(currentTurn) + 1) Mod 2  '0 turns into 1, 1 turns into 0
        End If
    End Sub

    Sub MakeMistake2()
        'Check whether Player 2 makes a mistake
        Randomize()
        If Rnd() < mistake Then
            p2Turn(currentTurn) = (p2Turn(currentTurn) + 1) Mod 2  '0 turns into 1, 1 turns into 0
        End If
    End Sub

    Sub CheckBC1()
        'Check whether a move is Betray or Cooperate for Player 1
        If p1Turn(currentTurn) = 0 Then
            Bcount1 += 1
        Else
            Ccount1 += 1
        End If
    End Sub

    Sub CheckBC2()
        'Check whether a move is Betray or Cooperate for Player 2
        If p2Turn(currentTurn) = 0 Then
            Bcount2 += 1
        Else
            Ccount2 += 1
        End If
    End Sub

    Sub OutputResults()
        Console.WriteLine("Player 1: " & p1)
        Console.WriteLine("Player 2: " & p2)
        Console.WriteLine("Player 1 Betrayed " & Bcount1 & " times.")
        Console.WriteLine("Player 2 Betrayed " & Bcount2 & " times.")
        Console.WriteLine("Player 1 Cooperated " & Ccount1 & " times.")
        Console.WriteLine("Player 2 Cooperated " & Ccount2 & " times.")
        Console.WriteLine("Player 1 got " & score1 & " points.")
        Console.WriteLine("Player 2 got " & score2 & " points.")
        Console.WriteLine("Player 1 and 2 got " & score1 + score2 & " points.")
        Console.WriteLine("Player 1's average score: " & score1 / turns)
        Console.WriteLine("Player 2's average score: " & score2 / turns)
        Console.WriteLine("Player 1 and 2's average score: " & (score1 + score2) / turns)
    End Sub
End Module
