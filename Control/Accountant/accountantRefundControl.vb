Imports System.Data
Imports MySql.Data.MySqlClient

Public Class AccountantRefundControl
    Inherits UserControl

    Private dgvRefunds As DataGridView
    Private txtPaymentID, txtBookingID, txtRefundAmount, txtDiscountAmount, txtRemarks, txtProcessedBy As TextBox
    Private btnProcess As Button

    Public Sub New()
        InitializeComponent()
        SetupLayout()
        LoadRefunds()
    End Sub

    Private Sub SetupLayout()
        dgvRefunds = New DataGridView With {
            .Dock = DockStyle.Top,
            .Height = 250,
            .ReadOnly = True,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AllowUserToAddRows = False
        }

        txtPaymentID = New TextBox()
        txtBookingID = New TextBox()
        txtRefundAmount = New TextBox()
        txtDiscountAmount = New TextBox()
        txtRemarks = New TextBox()
        txtProcessedBy = New TextBox()

        btnProcess = New Button With {.Text = "Apply Refund/Discount"}
        AddHandler btnProcess.Click, AddressOf btnProcess_Click

        ' Layout
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 7,
            .Padding = New Padding(10)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70))

        layout.Controls.Add(New Label With {.Text = "Payment ID"}, 0, 0)
        layout.Controls.Add(txtPaymentID, 1, 0)

        layout.Controls.Add(New Label With {.Text = "Booking ID"}, 0, 1)
        layout.Controls.Add(txtBookingID, 1, 1)

        layout.Controls.Add(New Label With {.Text = "Refunded Amount"}, 0, 2)
        layout.Controls.Add(txtRefundAmount, 1, 2)

        layout.Controls.Add(New Label With {.Text = "Discount Amount"}, 0, 3)
        layout.Controls.Add(txtDiscountAmount, 1, 3)

        layout.Controls.Add(New Label With {.Text = "Remarks"}, 0, 4)
        layout.Controls.Add(txtRemarks, 1, 4)

        layout.Controls.Add(New Label With {.Text = "Processed By"}, 0, 5)
        layout.Controls.Add(txtProcessedBy, 1, 5)

        layout.Controls.Add(btnProcess, 1, 6)

        Me.Controls.Add(layout)
        Me.Controls.Add(dgvRefunds)
    End Sub

    Private Sub LoadRefunds()
        Dim query As String = "
            SELECT 
                PaymentID, BookingID, AmountPaid, RefundedAmount, DiscountAmount, Remarks, PaymentDate, ProcessedBy 
            FROM payment 
            WHERE RefundedAmount > 0 OR DiscountAmount > 0 
            ORDER BY PaymentDate DESC"
        dgvRefunds.DataSource = GetData(query)
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtPaymentID.Text) Then
            MessageBox.Show("Payment ID is required.")
            Return
        End If

        Dim refundAmount As Decimal = 0
        Dim discountAmount As Decimal = 0
        Decimal.TryParse(txtRefundAmount.Text, refundAmount)
        Decimal.TryParse(txtDiscountAmount.Text, discountAmount)

        Dim query As String = "
            UPDATE payment 
            SET RefundedAmount = @RefundedAmount, DiscountAmount = @DiscountAmount, Remarks = @Remarks 
            WHERE PaymentID = @PaymentID"

        Dim parameters As New Dictionary(Of String, Object) From {
            {"@RefundedAmount", refundAmount},
            {"@DiscountAmount", discountAmount},
            {"@Remarks", txtRemarks.Text},
            {"@PaymentID", txtPaymentID.Text}
        }

        If ExecuteQuery(query, parameters) Then
            MessageBox.Show("Refund/Discount processed.")
            LoadRefunds()
            ClearFields()
        Else
            MessageBox.Show("Failed to process refund/discount.")
        End If
    End Sub

    Private Sub ClearFields()
        txtPaymentID.Clear()
        txtBookingID.Clear()
        txtRefundAmount.Clear()
        txtDiscountAmount.Clear()
        txtRemarks.Clear()
        txtProcessedBy.Clear()
    End Sub
End Class
