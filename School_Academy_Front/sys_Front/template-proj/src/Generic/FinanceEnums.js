export const FeeType = Object.freeze({
  Tuition: 1,
  Registration: 2,
  Exam: 3,
  Material: 4,
  Other: 5,
});

export const FeeTypeOptions = [
  { value: FeeType.Tuition, label: "رسوم دراسية" },
  { value: FeeType.Registration, label: "تسجيل" },
  { value: FeeType.Exam, label: "امتحان" },
  { value: FeeType.Material, label: "مواد" },
  { value: FeeType.Other, label: "أخرى" },
];

export const InvoiceStatus = Object.freeze({
  Unpaid: 1,
  PartiallyPaid: 2,
  Paid: 3,
  Cancelled: 4,
  Overdue: 5,
});

export const InvoiceStatusOptions = [
  { value: InvoiceStatus.Unpaid, label: "غير مدفوع" },
  { value: InvoiceStatus.PartiallyPaid, label: "مدفوع جزئياً" },
  { value: InvoiceStatus.Paid, label: "مدفوع" },
  { value: InvoiceStatus.Cancelled, label: "ملغى" },
  { value: InvoiceStatus.Overdue, label: "متأخر" },
];

export const PaymentMethod = Object.freeze({
  Cash: 1,
  BankTransfer: 2,
  CreditCard: 3,
  Cheque: 4,
  Online: 5,
});

export const PaymentMethodOptions = [
  { value: PaymentMethod.Cash, label: "نقداً" },
  { value: PaymentMethod.BankTransfer, label: "تحويل بنكي" },
  { value: PaymentMethod.CreditCard, label: "بطاقة" },
  { value: PaymentMethod.Cheque, label: "شيك" },
  { value: PaymentMethod.Online, label: "إلكتروني" },
];

export function getEnumLabel(options, value) {
  const found = options.find((opt) => Number(opt.value) === Number(value));
  return found ? found.label : value ?? "";
}

export function normalizeGuid(value) {
  if (!value || value === "00000000-0000-0000-0000-000000000000") return "";
  return value;
}

export function toDateInput(value) {
  if (!value) return "";
  if (typeof value === "string") return value.substring(0, 10);
  return "";
}

export function toDateTimeInput(value) {
  if (!value) return "";
  if (typeof value === "string") {
    return value.length >= 16 ? value.substring(0, 16) : value.substring(0, 10);
  }
  return "";
}
