Imports System.Data

Public Class SalesRedFuckingPayment

    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public AllowPrint As Boolean = False
    Private Sub BasicForm2_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return


    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As RoutedEventArgs) Handles btnPrint2.Click
        AllowPrint = True
        CType(Parent, Window).Close()
    End Sub

    Private Sub RdoCash_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoCash.Checked, RdoVisa.Checked, RdoCashVisa.Checked, RdoFuture.Checked, RdoCashFuture.Checked, RdoEmployees.Checked
        Try
            lblGroupBoxPaymentType.Content = "طريقة الدفع : " & CType(sender, RadioButton).Content
            PaymentType.Text = 0
            If RdoCash.IsChecked Then
                PaymentType.Text = 1
            ElseIf RdoVisa.IsChecked Then
                PaymentType.Text = 2
            ElseIf RdoCashVisa.IsChecked Then
                PaymentType.Text = 3
            ElseIf RdoFuture.IsChecked Then
                PaymentType.Text = 4
            ElseIf RdoCashFuture.IsChecked Then
                PaymentType.Text = 5
            ElseIf RdoEmployees.IsChecked Then
                PaymentType.Text = 6
            End If
        Catch ex As Exception
        End Try

        Try
            If RdoCashVisa.IsChecked OrElse RdoCashFuture.IsChecked Then
                CashValue.Visibility = Visibility.Visible
                lblCashValue.Visibility = Visibility.Visible
            Else
                CashValue.Visibility = Visibility.Hidden
                lblCashValue.Visibility = Visibility.Hidden
                CashValue.Text = 0
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As Input.KeyEventArgs) Handles CashValue.KeyDown, DiscountPerc.KeyDown, DiscountValue.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub Payed_TextChanged(sender As Object, e As TextChangedEventArgs) Handles Payed.TextChanged, TotalAfterDiscount.TextChanged
        Remaining.Clear()
        If Val(Payed.Text) = 0 Then Return
        Remaining.Text = Val(Payed.Text) - IIf(Val(CashValue.Text) > 0, Val(CashValue.Text), Val(TotalAfterDiscount.Text))

    End Sub

    Dim LopCalc As Boolean = False
    Dim lop As Boolean = False
    Private Sub CalcTotal() Handles Total.TextChanged, DiscountPerc.TextChanged, DiscountValue.TextChanged
        If LopCalc Or lop Then Return
        Try
            LopCalc = True
            
            If Val(DiscountPerc.Text) > 0 Then
                'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
                DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100
            End If


            If Val(DiscountPerc.Text) > 0 Then
                'DiscountValue.Text = Math.Round(Val(Total.Text) * Val(DiscountPerc.Text) / 100, 2)
                DiscountValue.Text = Val(Total.Text) * Val(DiscountPerc.Text) / 100
            End If

            LopCalc = False

            TotalAfterDiscount.Text = Val(Total.Text) - Val(DiscountValue.Text)

        Catch ex As Exception
        End Try
    End Sub
End Class
