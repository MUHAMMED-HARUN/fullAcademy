using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Infrastructure.Persistence;

#nullable disable

namespace System.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261111193000_InvoiceDueDateOnSettlement")]
    public partial class InvoiceDueDateOnSettlement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DueDate",
                table: "StudentInvoices",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.Sql(@"
UPDATE invoice
SET DueDate = CASE
    WHEN invoice.Status = 3 AND invoice.RemainingAmount <= 0
        THEN CAST(paymentDates.LastPaymentDate AS date)
    ELSE NULL
END
FROM StudentInvoices AS invoice
OUTER APPLY (
    SELECT MAX(payment.PaymentDate) AS LastPaymentDate
    FROM Payments AS payment
    LEFT JOIN PaymentReversals AS reversal ON reversal.PaymentID = payment.Id
    WHERE payment.StudentInvoiceID = invoice.Id
      AND reversal.Id IS NULL
) AS paymentDates;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE StudentInvoices
SET DueDate = IssueDate
WHERE DueDate IS NULL;");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DueDate",
                table: "StudentInvoices",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
